Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Seagull.BarTender.Print

Imports System.Diagnostics

Namespace LabelPrint
   ''' <summary>
   ''' Label Print Sample
   ''' This sample allows the user to open a format, select a printer, set substrings, and print.
   '''
   ''' This sample is intended to show how to:
   '''  -Generate quick thumbnails without first opening a format in BarTender.
   '''  -Open a Format in BarTender.
   '''  -Get a list of printers and set the printer on a format.
   '''  -Set a DataGridView to use the SubStrings collection as a DataSource.
   '''  -Get and Set the number of serialized copies and the number of identical copies.
   '''  -Start and Stop a BarTender engine.
   ''' </summary>
   Partial Public Class LabelPrint
	   Inherits Form
	  #Region "Fields"
	  ' Common strings.
	  Private Const appName As String = "Label Print"
	  Private Const dataSourced As String = "Data Sourced"

	  Private engine As Engine = Nothing ' The BarTender Print Engine
	  Private format As LabelFormatDocument = Nothing ' The currently open Format
	  Private isClosing As Boolean = False ' Set to true if we are closing. This helps discontinue thumbnail loading.

	  ' Label browser
	  Private browsingFormats() As String ' The list of filenames in the current folder
	  Private listItems As Hashtable ' A hash table containing ListViewItems and indexed by format name.
						   ' It keeps track of what formats have had their image loaded.
	  Private generationQueue As Queue(Of Integer) ' A queue containing indexes into browsingFormats
								  ' to facilitate the generation of thumbnails

	  ' Label browser indexes.
	  Private topIndex As Integer ' The top visible index in the lstLabelBrowser
	  Private selectedIndex As Integer ' The selected index in the lstLabelBrowser
	  #End Region

	  #Region "Enumerations"
	  ' Indexes into our image list for the label browser.
	  Private Enum ImageIndex
		  LoadingFormatImage = 0
		  FailureToLoadFormatImage = 1
	  End Enum
	  #End Region

	  #Region "Constructor"
	  ''' <summary>
	  ''' Constructor
	  ''' </summary>
	  Public Sub New()
		 InitializeComponent()
	  End Sub
	  #End Region

	  #Region "Delegates"
	  Private Delegate Sub DelegateShowMessageBox(ByVal message As String)
	  #End Region

	  #Region "Event Handlers"
	  #Region "Form Event Handlers"
	  ''' <summary>
	  ''' Called when the user opens the program.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub LabelPrint_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		 ' Create and start a new BarTender Print Engine.
		 Try
			engine = New Engine(True)
		 Catch exception As PrintEngineException
			' If the engine is unable to start, a PrintEngineException will be thrown.
			MessageBox.Show(Me, exception.Message, appName)
			Me.Close() ' Close this app. We cannot run without connection to an engine.
			Return
		 End Try

		 ' Get the list of printers
		 Dim printers As New Printers()
		 For Each printer As Printer In printers
			cboPrinters.Items.Add(printer.PrinterName)
		 Next printer

		 If printers.Count > 0 Then
			' Automatically select the default printer.
			cboPrinters.SelectedItem = printers.Default.PrinterName
		 End If

		 ' Setup the images used in the label browser.
		 lstLabelBrowser.View = View.LargeIcon
		 lstLabelBrowser.LargeImageList = New ImageList()
		 lstLabelBrowser.LargeImageList.ImageSize = New Size(100, 100)
		 lstLabelBrowser.LargeImageList.Images.Add(My.Resources.LoadingFormat)
		 lstLabelBrowser.LargeImageList.Images.Add(My.Resources.LoadingError)

		 ' Initialize a list and a queue.
		 listItems = New System.Collections.Hashtable()
		 generationQueue = New Queue(Of Integer)()

		 ' Limit the identical copies and serialized copies to 9
		 ' to match the user interface behavior of BarTender.
		 txtIdenticalCopies.MaxLength = 9
		 txtSerializedCopies.MaxLength = 9
	  End Sub

	  ''' <summary>
	  ''' Called when the user closes the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub LabelPrint_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 isClosing = True

		 ' Make sure the thumbnail worker is stopped before we try closing BarTender or 
		 ' there might be problems in the worker.
		 thumbnailCacheWorker.CancelAsync()
		 Do While thumbnailCacheWorker.IsBusy
			Application.DoEvents()
		 Loop

		 ' Quit the BarTender Print Engine, but do not save changes to any open formats.
		 If engine IsNot Nothing Then
			engine.Stop(SaveOptions.DoNotSaveChanges)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Called when the user presses the open button.
	  ''' Gets a list of formats in the selected folder.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpen.Click
		 folderBrowserDialog.Description = "Select a folder that contains BarTender formats."
		 Dim result As DialogResult = folderBrowserDialog.ShowDialog()
		 If result = System.Windows.Forms.DialogResult.OK Then
			SyncLock generationQueue
			   generationQueue.Clear()
			End SyncLock
			listItems.Clear()

			txtFolderPath.Text = folderBrowserDialog.SelectedPath
			btnPrint.Enabled = False

			browsingFormats = System.IO.Directory.GetFiles(txtFolderPath.Text, "*.btw")

			' Setting the VirtualListSize will cause lstLabelBrowser_RetrieveVirtualItem to be called.
			lstLabelBrowser.VirtualListSize = browsingFormats.Length
		 End If
	  End Sub

	  ''' <summary>
	  ''' Prints the currently loaded format using the selected printer.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
		 ' We lock on the engine here so we don't lose our format object
		 ' if the user were to click on a format that wouldn't load.
		 SyncLock engine
			Dim success As Boolean = True

			' Assign number of identical copies if it is not datasourced.
            If format.PrintSetup.SupportsIdenticalCopies = True Then
               Dim copies As Integer = 1
               success = Int32.TryParse(txtIdenticalCopies.Text, copies) AndAlso (copies >= 1)
               If (Not success) Then
                  MessageBox.Show(Me, "Identical Copies must be an integer greater than or equal to 1.", appName)
               Else
                  format.PrintSetup.IdenticalCopiesOfLabel = copies
               End If
            End If

			' Assign number of serialized copies if it is not datasourced.
            If success AndAlso (format.PrintSetup.SupportsSerializedLabels = True) Then
               Dim copies As Integer = 1
               success = Int32.TryParse(txtSerializedCopies.Text, copies) AndAlso (copies >= 1)
               If (Not success) Then
                  MessageBox.Show(Me, "Serialized Copies must be an integer greater than or equal to 1.", appName)
               Else
                  format.PrintSetup.NumberOfSerializedLabels = copies
               End If
            End If

			' If there are no incorrect values in the copies boxes then print.
			If success Then
			   Cursor.Current = Cursors.WaitCursor

			   ' Get the printer from the dropdown and assign it to the format.
			   If cboPrinters.SelectedItem IsNot Nothing Then
				  format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString()
			   End If

			   Dim messages As Messages = Nothing
			   Dim waitForCompletionTimeout As Integer = 10000 ' 10 seconds
			   Dim result As Result = format.Print(appName, waitForCompletionTimeout, messages)
			   Dim messageString As String = Constants.vbLf + Constants.vbLf & "Messages:"

			   For Each message As Seagull.BarTender.Print.Message In messages
				  messageString &= Constants.vbLf + Constants.vbLf + message.Text
			   Next message

			   If result = Result.Failure Then
				  MessageBox.Show(Me, "Print Failed" & messageString, appName)
			   Else
				  MessageBox.Show(Me, "Label was successfully sent to printer." & messageString, appName)
			   End If
			End If
		 End SyncLock
	  End Sub
	  #End Region

	  #Region "List View Event Handlers"
	  ''' <summary>
	  ''' Called when the user clicks on an item in the format browser.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub lstLabelBrowser_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstLabelBrowser.SelectedIndexChanged
		 If lstLabelBrowser.SelectedIndices.Count = 1 Then
			selectedIndex = lstLabelBrowser.SelectedIndices(0)
			EnableControls(False)
			picUpdatingFormat.Visible = True

			' Start a BackgroundWorker to load the format to extract the copies
			' data and substrings. Then update the UI with the data.
			Dim formatLoader As New BackgroundWorker()
			AddHandler formatLoader.DoWork, AddressOf formatLoader_DoWork
			AddHandler formatLoader.RunWorkerCompleted, AddressOf formatLoader_RunWorkerCompleted
			formatLoader.RunWorkerAsync(selectedIndex)
		 ElseIf lstLabelBrowser.SelectedIndices.Count = 0 Then
			EnableControls(False)
			picUpdatingFormat.Visible = False
		 End If
	  End Sub
	  ''' <summary>
	  ''' Called when the format browser listbox scrolls a listitem into view.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub lstLabelBrowser_RetrieveVirtualItem(ByVal sender As Object, ByVal e As RetrieveVirtualItemEventArgs) Handles lstLabelBrowser.RetrieveVirtualItem
		 Dim btwFileIndex As Integer = e.ItemIndex

		 ' If our list doesn't have this item then add it and set the
		 ' image index to "loading" so that the image will be loaded.
		 If listItems(browsingFormats(btwFileIndex)) Is Nothing Then
			e.Item = New ListViewItem(browsingFormats(btwFileIndex))
			e.Item.ImageIndex = CInt(Fix(ImageIndex.LoadingFormatImage))
			listItems.Add(browsingFormats(btwFileIndex), e.Item)
		 Else
			e.Item = CType(listItems(browsingFormats(btwFileIndex)), ListViewItem)
		 End If

		 ' Add the index to the queue so that the thumbnail thread can get the image.
		 If e.Item.ImageIndex = CInt(Fix(ImageIndex.LoadingFormatImage)) Then
			SyncLock generationQueue
			   If (Not generationQueue.Contains(btwFileIndex)) Then
				  generationQueue.Enqueue(btwFileIndex)
			   End If
			End SyncLock
		 End If

		 ' If we put anything on the queue, start the thumbnail worker if it's not already started.
		 If (Not thumbnailCacheWorker.IsBusy) AndAlso (generationQueue.Count > 0) Then
			thumbnailCacheWorker.RunWorkerAsync()
		 End If
	  End Sub

	  ''' <summary>
	  ''' Called when the format browser listbox needs to cache listitems that may appear soon.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="cacheEvent"></param>
	  Private Sub lstLabelBrowser_CacheVirtualItems(ByVal sender As Object, ByVal cacheEvent As CacheVirtualItemsEventArgs) Handles lstLabelBrowser.CacheVirtualItems
		 topIndex = cacheEvent.StartIndex

		 SyncLock generationQueue
			For index As Integer = cacheEvent.StartIndex To cacheEvent.EndIndex
			   Dim listViewItem As ListViewItem = CType(listItems(browsingFormats(index)), ListViewItem)

			   If (listViewItem IsNot Nothing) AndAlso (listViewItem.ImageIndex = CInt(Fix(ImageIndex.LoadingFormatImage))) Then
				  If (Not generationQueue.Contains(index)) Then
					 generationQueue.Enqueue(index)
				  End If
			   End If
			Next index
		 End SyncLock

		 ' If we put anything on the queue, start the thumbnail worker if it's not already started.
		 If (Not thumbnailCacheWorker.IsBusy) AndAlso (generationQueue.Count > 0) Then
			thumbnailCacheWorker.RunWorkerAsync()
		 End If
	  End Sub
	  #End Region

	  #Region "Thumbnail Event Handlers"
	  ''' <summary>
	  ''' Called when a thumbnail was loaded so it can be shown.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub thumbnailCacheWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles thumbnailCacheWorker.ProgressChanged
		 Dim args() As Object = TryCast(e.UserState, Object())
		 Dim item As ListViewItem = CType(args(0), ListViewItem)

		 ' 100 means we got the image successfully.
		 If e.ProgressPercentage = 100 Then
			Dim thumbnail As Image = CType(args(1), Image)

			lstLabelBrowser.LargeImageList.Images.Add(item.Text, thumbnail)
			item.ImageIndex = lstLabelBrowser.LargeImageList.Images.IndexOfKey(item.Text)
		 ElseIf e.ProgressPercentage = 0 Then ' 0 means we did not successfully get the format image.
			item.ImageIndex = CInt(Fix(ImageIndex.FailureToLoadFormatImage))
		 End If
		 item.ListView.Invalidate(item.Bounds)
	  End Sub

	  ''' <summary>
	  ''' Loads thumbnails in a background thread so the UI doesn't hang.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub thumbnailCacheWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles thumbnailCacheWorker.DoWork
		 Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

		 Dim index As Integer
		 Dim fileName As String

		 ' Loop until the queue of items that need to be loaded is empty or the worker was cancelled.
		 Do While (generationQueue.Count > 0) AndAlso (Not worker.CancellationPending) AndAlso Not isClosing
			SyncLock generationQueue
			   ' Get the index to use.
			   index = generationQueue.Dequeue()
			End SyncLock

			' If this is way out of our view don't bother generating it.
			If Math.Abs(index - topIndex) < 30 Then
			   fileName = browsingFormats(index)

			   ' Check to see if the listitem is already generated and just waiting for a thumbnail.
			   Dim item As ListViewItem = CType(listItems(fileName), ListViewItem)
			   If item Is Nothing Then
				  item = New ListViewItem(fileName)
				  item.ImageIndex = CInt(Fix(ImageIndex.LoadingFormatImage))
				  listItems.Add(fileName, item)
			   End If

			   ' If the listitem doesn't already have a thumbnail, generate it.
			   If item.ImageIndex = CInt(Fix(ImageIndex.LoadingFormatImage)) Then
				  Try
					 Dim btwImage As Image = LabelFormatThumbnail.Create(fileName, Color.Gray, 100, 100)

					 Dim progressReport() As Object = { item, btwImage }
					 worker.ReportProgress(100, progressReport)
				  ' If the file isn't an actual btw format file we will get an exception.
				  Catch
					 Dim progressReport() As Object = { item, Nothing }
					 worker.ReportProgress(0, progressReport)
				  End Try
			   End If
			End If
		 Loop
	  End Sub
	  #End Region

	  #Region "Format Loader Event Handlers"
	  ''' <summary>
	  ''' Loads a format in a separate thread so it doesn't hang the UI.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub formatLoader_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
		 Dim index As Integer = CInt(Fix(e.Argument))
		 Dim errorMessage As String = ""

		 ' We lock the engine here because the engine might still be printing a format.
		 SyncLock engine
			' Make sure this is still the label the user has selected in case they are clicking around fast.
			If selectedIndex = index Then
			   Try
				  If format IsNot Nothing Then
					 format.Close(SaveOptions.DoNotSaveChanges)
				  End If
				  format = engine.Documents.Open(browsingFormats(index))
			   Catch comException As System.Runtime.InteropServices.COMException
				  errorMessage = String.Format("Unable to open format: {0}" & Constants.vbLf & "Reason: {1}", browsingFormats(index), comException.Message)
				  format = Nothing
			   End Try
			End If
		 End SyncLock
		 ' We are in a non-UI thread so we need to use Invoke to show our message properly.
		 If errorMessage.Length <> 0 Then
			Invoke(New DelegateShowMessageBox(AddressOf ShowMessageBox), errorMessage)
		 End If
	  End Sub

      ''' <summary>
      ''' Called when the format is finished loading.
      ''' Fills in the copies boxes and substrings if there are any.
      ''' </summary>
      ''' <param name="sender"></param>
      ''' <param name="e"></param>
      Private Sub formatLoader_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
         ' We lock on the engine here so that if we have a valid format object
         ' we don't lose it while we are using it.
         SyncLock engine
            If format IsNot Nothing Then
               EnableControls(True)
               cboPrinters.SelectedItem = format.PrintSetup.PrinterName
               picUpdatingFormat.Visible = False
               lblFormatError.Visible = False

               ' Set the number of identical copies.
               If format.PrintSetup.SupportsIdenticalCopies = False Then
                  txtIdenticalCopies.Text = dataSourced
                  txtIdenticalCopies.ReadOnly = True
               Else
                  txtIdenticalCopies.Text = format.PrintSetup.IdenticalCopiesOfLabel.ToString()
                  txtIdenticalCopies.ReadOnly = False
               End If

               ' Set the number of serialized copies.
               If format.PrintSetup.SupportsSerializedLabels = False Then
                  txtSerializedCopies.Text = dataSourced
                  txtSerializedCopies.ReadOnly = True
               Else
                  txtSerializedCopies.Text = format.PrintSetup.NumberOfSerializedLabels.ToString()
                  txtSerializedCopies.ReadOnly = False
               End If

               ' Set the substrings grid.
               If format.SubStrings.Count > 0 Then
                  Dim bindingSource As New BindingSource()
                  bindingSource.DataSource = format.SubStrings
                  substringGrid.DataSource = bindingSource
                  substringGrid.AutoResizeColumns()
                  lblNoSubstrings.Visible = False
               Else
                  lblNoSubstrings.Visible = True
               End If
               substringGrid.Invalidate()
            Else ' There is no format loaded, it must have errored out.
               picUpdatingFormat.Visible = False
               lblNoSubstrings.Visible = False
               lblFormatError.Visible = True
            End If
         End SyncLock
      End Sub
	  #End Region
	  #End Region

	  #Region "Methods"
	  ''' <summary>
	  ''' Enables or disables controls based on whether or not a valid format is open.
	  ''' </summary>
	  ''' <param name="enable"></param>
	  Private Sub EnableControls(ByVal enable As Boolean)
		 txtIdenticalCopies.Enabled = enable
		 txtSerializedCopies.Enabled = enable
		 btnPrint.Enabled = enable

		 If (Not enable) Then
			txtIdenticalCopies.Text = ""
			txtSerializedCopies.Text = ""
			substringGrid.DataSource = Nothing
			lblFormatError.Visible = False
			lblNoSubstrings.Visible = False
		 End If
	  End Sub

	  ''' <summary>
	  ''' Show a message box. We need this method to facilitate showing
	  ''' messages from a non-UI thread.
	  ''' </summary>
	  ''' <param name="message">The message to show.</param>
	  Private Sub ShowMessageBox(ByVal message As String)
		 MessageBox.Show(Me, message, appName)
	  End Sub
	  #End Region
   End Class
End Namespace
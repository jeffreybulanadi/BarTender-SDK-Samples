Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports Seagull.BarTender.Print

Namespace PrintPreview
   ''' <summary>
   ''' Print Preview Sample
   ''' This sample allows the user to open a format, select a printer, and view how the printed
   ''' format will look using the selected printer.
   ''' 
   ''' This sample is intended to show how to:
   '''  -Export Print Previews
   '''  -Open a Format in BarTender
   '''  -Get a list of printers and set the printer on a format.
   '''  -Start and Stop the BarTender engine.
   ''' </summary>
   Partial Public Class PrintPreview
	   Inherits Form
	  #Region "Fields"
	  ' Common strings.
	  Private Const appName As String = "Print Preview"

	  Private engine As Engine = Nothing ' The BarTender Print Engine.
	  Private format As LabelFormatDocument = Nothing ' The format that will be exported.
	  Private previewPath As String = "" ' The path to the folder where the previews will be exported.
	  Private currentPage As Integer = 1 ' The current page being viewed.
	  Private totalPages As Integer ' Number of pages.
	  Private messages As Messages
	  #End Region

	  #Region "Constructor"
	  ''' <summary>
	  ''' Constructor
	  ''' </summary>
	  Public Sub New()
		 InitializeComponent()
	  End Sub
	  #End Region

	  #Region "Form Event Handlers"
	  ''' <summary>
	  ''' Start BarTender Print Engine, Initialize printer list,
	  ''' enable/disable controls, setup temporary folder for images.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintPreview_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
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

		 ' Hide/Disable preview controls.
		 DisablePreview()

		 ' Create a temporary folder to hold the images.
		 Dim tempPath As String = Path.GetTempPath() ' Something like "C:\Documents and Settings\<username>\Local Settings\Temp""
		 Dim newFolder As String

		 ' It's not likely we'll have a path that already exists, but we'll check for it anyway.
		 Do
			newFolder = Path.GetRandomFileName()
			previewPath = tempPath & newFolder ' newFolder is something crazy like "gulvwdmt.3r4"
		 Loop While Directory.Exists(previewPath)
		 Directory.CreateDirectory(previewPath)
	  End Sub

	  ''' <summary>
	  ''' Stop the BarTender Print Engine and delete our temporary images.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintPreview_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 ' Quit the BarTender Print Engine but do not save changes to any open formats.
		 If engine IsNot Nothing Then
			engine.Stop(SaveOptions.DoNotSaveChanges)
		 End If

		 ' Remove the temporary path and all the images.
		 If previewPath.Length <> 0 Then
			Directory.Delete(previewPath, True)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Opens the open file dialog and lets the user choose a format to open.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpen.Click
		 ' Let's disable some controls until we are done.
		 btnOpen.Enabled = False
		 cboPrinters.Enabled = False
		 btnPreview.Enabled = False

		 Dim result As DialogResult = openFileDialog.ShowDialog()
		 If result = System.Windows.Forms.DialogResult.OK Then
			Cursor.Current = Cursors.WaitCursor

			' Close the previous format.
			If format IsNot Nothing Then
			   format.Close(SaveOptions.DoNotSaveChanges)
			End If

			' We need to delete the files associated with the last format.
			Dim files() As String = Directory.GetFiles(previewPath)
			For Each filename As String In files
			   File.Delete(filename)
			Next filename

			' Put the UI back into a non-preview state.
			DisablePreview()

			' Open the format.
			txtFilename.Text = openFileDialog.FileName
			Try
			   format = engine.Documents.Open(openFileDialog.FileName)
			Catch comException As System.Runtime.InteropServices.COMException
			   MessageBox.Show(Me, String.Format("Unable to open format: {0}" & Constants.vbLf & "Reason: {1}", openFileDialog.FileName, comException.Message), appName)
			   format = Nothing
			End Try

			' Only allow preview button if we successfully loaded the format.
			btnPreview.Enabled = (format IsNot Nothing)

			If format IsNot Nothing Then
			   ' Select the printer in use by the format.
			   cboPrinters.SelectedItem = format.PrintSetup.PrinterName
			End If

			Cursor.Current = Cursors.Default
		 End If
		 ' Restore some controls.
		 btnOpen.Enabled = True
		 cboPrinters.Enabled = True
	  End Sub

	  ''' <summary>
	  ''' Runs when the Preview button is pressed. Disables controls and starts
	  ''' BarTender working to export print preview images.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPreview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPreview.Click
		 ' Disable some controls until we are finished creating previews.
		 btnOpen.Enabled = False
		 cboPrinters.Enabled = False
		 btnPreview.Enabled = False

		 DisablePreview()

		 ' Get the printer from the dropdown and assign it to the format.
		 format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString()

		 ' Set control states to show working. These will be reset when the work completes.
		 picUpdating.Visible = True

		 ' Have the background worker export the BarTender format.
		 backgroundWorker.RunWorkerAsync(format)
	  End Sub

	  ''' <summary>
	  ''' Show the preview of the first label.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnFirst_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFirst.Click
		 currentPage = 1
		 ShowPreview()
	  End Sub

	  ''' <summary>
	  ''' Show the preview of the previous label.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPrev_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrev.Click
		 currentPage -= 1
		 ShowPreview()
	  End Sub

	  ''' <summary>
	  ''' Show the preview of the next label.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnNext_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNext.Click
		 currentPage += 1
		 ShowPreview()
	  End Sub

	  ''' <summary>
	  ''' Show the preview of the last label.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnLast_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLast.Click
		 currentPage = totalPages
		 ShowPreview()
	  End Sub
	  #End Region

	  #Region "Print Preview Thread Methods"
	  ''' <summary>
	  ''' Runs in a separate thread to allow BarTender to export the preview while allowing UI updates.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker.DoWork
		 Dim format As LabelFormatDocument = CType(e.Argument, LabelFormatDocument)

		 ' Delete any existing files.
		 Dim oldFiles() As String = Directory.GetFiles(previewPath, "*.*")
		 For index As Integer = 0 To oldFiles.Length - 1
			File.Delete(oldFiles(index))
		 Next index

		 ' Export the format's print previews.
		 format.ExportPrintPreviewToFile(previewPath, "PrintPreview%PageNumber%.jpg", ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, New Resolution(picPreview.Width, picPreview.Height), System.Drawing.Color.White, OverwriteOptions.Overwrite, True, True, messages)
		 Dim files() As String = Directory.GetFiles(previewPath, "*.*")
		 totalPages = files.Length
	  End Sub

	  ''' <summary>
	  ''' We are done exporting the preview, so let's show any messages
	  ''' and display the first label.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles backgroundWorker.RunWorkerCompleted
         ' Report any messages.
         If messages IsNot Nothing Then
            If messages.Count > 5 Then
               MessageBox.Show(Me, "There are more than 5 messages from the print preview. Only the first 5 will be displayed.", appName)
            End If
            Dim count As Integer = 0
            For Each message As Seagull.BarTender.Print.Message In messages
               MessageBox.Show(Me, message.Text, appName)
               ' if (++count >= 5)
               count += 1
               If count >= 5 Then
                  Exit For
               End If
            Next message
         End If

         picUpdating.Visible = False

         btnOpen.Enabled = True
         btnPreview.Enabled = True
         cboPrinters.Enabled = True

         ' Only enable the preview if we actual got some pages.
         If totalPages <> 0 Then
            lblNumPreviews.Visible = True

            currentPage = 1
            ShowPreview()
         End If
	  End Sub
	  #End Region

	  #Region "Methods"
	  ''' <summary>
	  ''' Disable/Hide preview controls.
	  ''' </summary>
	  Private Sub DisablePreview()
		 picPreview.ImageLocation = ""
		 picPreview.Visible = False

		 btnPrev.Enabled = False
		 btnFirst.Enabled = False
		 lblNumPreviews.Visible = False
		 btnNext.Enabled = False
		 btnLast.Enabled = False
	  End Sub

	  ''' <summary>
	  ''' Show the preview of the current page.
	  ''' </summary>
	  Private Sub ShowPreview()
		 ' Our current preview number shouldn't be out of range,
		 ' but we'll practice good programming by checking it.
		 If (currentPage < 1) OrElse (currentPage > totalPages) Then
			currentPage = 1
		 End If

		 ' Update the page label and the preview Image.
		 lblNumPreviews.Text = String.Format("Page {0} of {1}", currentPage, totalPages)
		 Dim filename As String = String.Format("{0}\PrintPreview{1}.jpg", previewPath, currentPage)

		 picPreview.ImageLocation = filename
		 picPreview.Visible = True

		 ' Enable or Disable controls as needed.
		 If currentPage = 1 Then
			btnPrev.Enabled = False
			btnFirst.Enabled = False
		 Else
			btnPrev.Enabled = True
			btnFirst.Enabled = True
		 End If

		 If currentPage = totalPages Then
			btnNext.Enabled = False
			btnLast.Enabled = False
		 Else
			btnNext.Enabled = True
			btnLast.Enabled = True
		 End If
	  End Sub
	  #End Region
   End Class
End Namespace
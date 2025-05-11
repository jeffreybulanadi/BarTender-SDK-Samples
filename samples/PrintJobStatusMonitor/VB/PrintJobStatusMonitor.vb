Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.IO
Imports System.Windows.Forms
Imports Seagull.BarTender.Print

Namespace PrintJobStatusMonitor
   ''' <summary>
   ''' Print Job Status Monitor Sample
   ''' This sample allows the user to open a format, select a printer, print the format, and
   ''' view printer status while the format is printing.
   '''  
   ''' This sample is intended to show how to:
   '''  -Hook up to PrintJob status events.
   '''  -Open a Format in BarTender
   '''  -Print a Format
   '''  -Export a thumbnail of the Format
   '''  -Get a list of printers and set the printer on a format.
   '''  -Start and Stop a BarTender engine.
   ''' </summary>
   Partial Public Class PrintJobStatusMonitor
	   Inherits Form
	  #Region "Fields"
	  ' Common strings.
	  Private ReadOnly appName As String = "Print Job Status Monitor"

	  Private engine As Engine = Nothing ' The BarTender Print Engine.
	  Private format As LabelFormatDocument = Nothing ' The currently open Format.
	  Private thumbnailFile As String = ""
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
	  Public Delegate Sub DoUpdateOutputDelegate(ByVal output As String)
	  #End Region

	  #Region "Event Handlers"
	  #Region "Form Event Handlers"
	  ''' <summary>
	  ''' Called when the user opens the program.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintJobStatusMonitor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		 ' Create and start a new BarTender Print Engine.
		 Try
			engine = New Engine(True)
		 Catch exception As PrintEngineException
			' If the engine is unable to start, a PrintEngineException will be thrown.
			MessageBox.Show(Me, exception.Message, appName)
			Me.Close() ' Close this app. We cannot run without connection to an engine.
			Return
		 End Try

		 ' Sign up for print job events.
		 AddHandler engine.JobCancelled, AddressOf Engine_JobCancelled
		 AddHandler engine.JobErrorOccurred, AddressOf Engine_JobErrorOccurred
		 AddHandler engine.JobMonitorErrorOccurred, AddressOf Engine_JobMonitorErrorOccurred
		 AddHandler engine.JobPaused, AddressOf Engine_JobPaused
		 AddHandler engine.JobQueued, AddressOf Engine_JobQueued
		 AddHandler engine.JobRestarted, AddressOf Engine_JobRestarted
		 AddHandler engine.JobResumed, AddressOf Engine_JobResumed
		 AddHandler engine.JobSent, AddressOf Engine_JobSent

		 ' Get the list of printers
		 Dim printers As New Printers()
		 For Each printer As Printer In printers
			cboPrinters.Items.Add(printer.PrinterName)
		 Next printer

		 If printers.Count > 0 Then
			' Automatically select the default printer.
			cboPrinters.SelectedItem = printers.Default.PrinterName
		 End If

		 ' Create a uniquely named, temporary file on disk for our thumbnails.
		 thumbnailFile = Path.GetTempFileName()
	  End Sub

	  ''' <summary>
	  ''' Called when the user closes the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintJobStatusMonitor_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 ' Quit the BarTender Print Engine but do not save changes to any open formats.
		 If engine IsNot Nothing Then
			engine.Stop(SaveOptions.DoNotSaveChanges)
		 End If

		 ' Remove our temporary thumbnail file.
		 If thumbnailFile.Length <> 0 Then
			File.Delete(thumbnailFile)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Open the open file dialog and let the user choose a format to open.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnOpen_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpen.Click
		 Dim result As DialogResult = openFileDialog.ShowDialog()
		 If result = System.Windows.Forms.DialogResult.OK Then
			' Close the previous format
			If format IsNot Nothing Then
			   format.Close(SaveOptions.DoNotSaveChanges)
			End If

			'If the user selected a format, open it in BarTender.
			txtFilename.Text = openFileDialog.FileName

			Try
			   format = engine.Documents.Open(openFileDialog.FileName)
			Catch comException As System.Runtime.InteropServices.COMException
			   MessageBox.Show(Me, String.Format("Unable to open format: {0}" & Constants.vbLf & "Reason: {1}", openFileDialog.FileName, comException.Message), appName)
			   format = Nothing
			End Try

			' Only allow print button if we successfully loaded the format.
			btnPrint.Enabled = (format IsNot Nothing)

			If format IsNot Nothing Then
			   ' Select the printer in use by the format.
			   cboPrinters.SelectedItem = format.PrintSetup.PrinterName

			   'Generate a thumbnail for it.
			   format.ExportImageToFile(thumbnailFile, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, New Resolution(picThumbnail.Width, picThumbnail.Height), OverwriteOptions.Overwrite)
			   picThumbnail.ImageLocation = thumbnailFile
			Else
			   ' Clear any previous image.
			   picThumbnail.ImageLocation = ""
			End If
		 End If
	  End Sub

	  ''' <summary>
	  ''' Print the format. (Actually, tell our thread to print it.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPrint_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrint.Click
		 ' Disable the button until we are finished printing.
		 btnPrint.Enabled = False

		 ' Get the printer from the dropdown and assign it to the format.
		 format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString()

		 ' Have the status updater print the format so we can watch for status messages
		 statusUpdater.RunWorkerAsync(format)
	  End Sub
	  #End Region

	  #Region "Print Job Event Handlers"
	  ''' <summary>
	  ''' Handle the print events.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="printJob"></param>
	  Private Sub Engine_JobSent(ByVal sender As Object, ByVal printJob As JobSentEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 If printJob.JobPrintingVerified Then
			lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Sent/Print Verified on {1}.", printJob.Name, printJob.PrinterInfo.Name)})
		 Else
			lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Sent to {1}.", printJob.Name, printJob.PrinterInfo.Name)})
		 End If
	  End Sub
	  Private Sub Engine_JobResumed(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Resumed.", printJob.Name) })
	  End Sub
	  Private Sub Engine_JobRestarted(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Restarted on {1}.", printJob.Name, printJob.PrinterInfo.Name)})
	  End Sub
	  Private Sub Engine_JobQueued(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Queued on {1}.", printJob.Name, printJob.PrinterInfo.Name)})
	  End Sub
	  Private Sub Engine_JobPaused(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Paused.", printJob.Name) })
	  End Sub
	  Private Sub Engine_JobMonitorErrorOccurred(ByVal sender As Object, ByVal errorInfo As MonitorErrorEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("Job Monitor Error {0}.", errorInfo.Message) })
	  End Sub
	  Private Sub Engine_JobErrorOccurred(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Error {1}.", printJob.Name, printJob.PrinterInfo.Message) })
	  End Sub
	  Private Sub Engine_JobCancelled(ByVal sender As Object, ByVal printJob As PrintJobEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstJobStatus.Invoke(doUpdateOutputDelegate, New Object() { String.Format("PrintJob {0} Cancelled.", printJob.Name) })
	  End Sub
	  #End Region

	  #Region "Status Updater background thread"
	  ''' <summary>
	  ''' This method updates the user interface with the received messages from
	  ''' the print job.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub statusUpdater_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles statusUpdater.ProgressChanged
		 If e.UserState.GetType() Is GetType(Messages) Then
			Dim messages As Messages = CType(e.UserState, Messages)
			Dim text As String
			For Each message As Seagull.BarTender.Print.Message In messages
			   ' Let's remove any carriage returns and linefeeds since
			   ' we are putting each message on a single line.
			   text = message.Text.Replace(ControlChars.Lf, " "c)
			   text = text.Replace(ControlChars.Cr, " "c)
			   lstMessages.Items.Add(text)
			Next message
		 End If

		 ' If we are finished printing, re-enable the print button.
		 If e.ProgressPercentage = 100 Then
			btnPrint.Enabled = True
		 End If
	  End Sub

	  ''' <summary>
	  ''' Print the format and report the messages.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub statusUpdater_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles statusUpdater.DoWork
		 Dim format As LabelFormatDocument = CType(e.Argument, LabelFormatDocument)

		 ' Create a messages object to be filled up by Print.
            Dim messages As Messages = Nothing

		 ' Print the Format.
		 format.Print(appName, System.Threading.Timeout.Infinite, messages)

		 ' Report that the printjob is done so we can re-enable the print button.
		 statusUpdater.ReportProgress(100, messages)
	  End Sub
	  #End Region
	  #End Region

	  #Region "Methods"
	  ''' <summary>
	  ''' Add our status string to the list box.
	  ''' </summary>
	  ''' <param name="output"></param>
	  Private Sub DoUpdateOutput(ByVal output As String)
		 lstJobStatus.Items.Add(output)
	  End Sub
	  #End Region
   End Class
End Namespace
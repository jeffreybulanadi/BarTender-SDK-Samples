Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports Seagull.BarTender.Print

Namespace PrintToFileManager
   ' PrintToFileManager Sample
   ' This sample shows the steps taken to print to file and print the printer file to a printer.
   ' 
   ' Step 1: Due to BarTender's licensing restrictions, a printing license must be obtained before 
   ' printing to file. This printing license needs to be obtained using the computer that will 
   ' eventually print the printer file, and must pass in the printer that will print the file. The
   ' BarTenderWebClientRuntime is used to obtain the printing license.
   ' 
   ' Step 2: The computer that prints to file needs to use the printing license in step 1 and
   ' print to a printer with the same or a compatible driver.
   ' 
   ' Step 3: The computer that will print the printer file now needs to print it using the 
   ' BarTenderWebClientRuntime and specify the original printer that was used to obtain the license. 
   ' 
   ' This sample is intended to show how to:
   '  -Print to file
   '  -Obtain a printer license
   '  -Print a printer file to a printer.
   '  -Open a Format in BarTender
   '  -Get a list of printers and set the printer on a format.
   '  -Start and Stop the BarTender engine
   Partial Public Class PrintToFileManager
	   Inherits Form
	  #Region "Fields"
	  ' Common strings.
	  Private Const appName As String = "Print To File Manager"

	  Private engine As Engine = Nothing ' The BarTender Print Engine.
	  Private format As LabelFormatDocument = Nothing ' The format that will be printed to file.
	  Private printClient As BarTenderPrintClient.Printer
	  Private backgroundPrintingThreadResult As Result
	  Private printToFileThreadResultString As String
	  Private printThreadResultString As String
	  Private printToFileMessages As Messages = Nothing
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
	  ''' Called when the user opens the program.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintToFileManager_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		 ' Create and start a new BarTender Print Engine.
		 Try
			engine = New Engine(True)
		 Catch exception As PrintEngineException
			' If the engine is unable to start, a PrintEngineException will be thrown.
			MessageBox.Show(Me, exception.Message, appName)
			Me.Close() ' Close this app. We cannot run without connection to an engine.
			Return
		 End Try

		 ' Get the list of printers.
		 Dim printers As New Printers()
		 For Each printer As Printer In printers
			cboStep1Printer.Items.Add(printer.PrinterName)
			cboStep2Printer.Items.Add(printer.PrinterName)
			cboStep3Printer.Items.Add(printer.PrinterName)
		 Next printer

		 If printers.Count > 0 Then
			' Automatically select the default printer.
			cboStep1Printer.SelectedItem = printers.Default.PrinterName
			cboStep2Printer.SelectedItem = printers.Default.PrinterName
			cboStep3Printer.SelectedItem = printers.Default.PrinterName
		 End If

		 ' Create the BarTenderWebClientRuntime.Printers object for print file licensing.
		 printClient = New BarTenderPrintClient.Printer()
	  End Sub

	  ''' <summary>
	  ''' Called when the user closes the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub PrintToFileManager_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 ' Quit the BarTender Print Engine, but do not save changes to any open formats.
		 If engine IsNot Nothing Then
			engine.Stop(SaveOptions.DoNotSaveChanges)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Step 1: Generate a license key using the printer that will be used to print the final prinjob.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnGenerateLicense_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGenerateLicense.Click
		 ' Get the license and show it in a text box.
		 Dim printLicenseKey As String = ""
		 If printClient.CreatePrintToFileLicense(cboStep1Printer.SelectedItem.ToString(), printLicenseKey) Then
			txtPrintLicense.Text = printLicenseKey

			' If we have a format file name and a license then we can do the print to file.
			If txtLabelFilename.Text.Length <> 0 Then
			   btnPrintToFile.Enabled = True
			End If
		 Else
			MessageBox.Show(Me, printLicenseKey, appName)
			txtPrintLicense.Text = ""
			btnPrintToFile.Enabled = False
		 End If
	  End Sub

	  ''' <summary>
	  ''' Step 2: Opens the open file dialog and lets the user choose a format to open.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnOpenBTW_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpenBTW.Click
		 If openBTWDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			' Close the previous format
			If format IsNot Nothing Then
			   format.Close(SaveOptions.DoNotSaveChanges)
			End If

			' If the user selected a format, open it in BarTender.
			Try
			   format = engine.Documents.Open(openBTWDialog.FileName)
			Catch comException As System.Runtime.InteropServices.COMException
			   MessageBox.Show(Me, String.Format("Unable to open format: {0}" & Constants.vbLf & "Reason: {1}", openBTWDialog.FileName, comException.Message), appName)
			   format = Nothing
			End Try
			If format IsNot Nothing Then
			   txtLabelFilename.Text = openBTWDialog.FileName
			   cboStep2Printer.SelectedItem = format.PrintSetup.PrinterName

			   ' If we have a format file name and a license then we can do the print to file.
			   If txtPrintLicense.Text.Length <> 0 Then
				  btnPrintToFile.Enabled = True
			   End If
			Else
			   txtLabelFilename.Text = ""
			   btnPrintToFile.Enabled = False
			End If
		 End If
	  End Sub

	  ''' <summary>
	  ''' Step 2: Prints the format to file when the print button is clicked.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPrintToFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintToFile.Click
		 If savePrinterFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			' Let's disable the button so that the user can't repeatedly
			' hit it causing many threads.
			btnPrintToFile.Enabled = False

			' Get the printer from the dropdown and assign it to the format.
			format.PrintSetup.PrinterName = cboStep2Printer.SelectedItem.ToString()

			' Set the selected filename to the printerfile property of the format's PrintSetup.
			format.PrintSetup.PrintToFileName = savePrinterFileDialog.FileName

			' Set the format to print to file.
			format.PrintSetup.PrintToFile = True

			' Set the printing license.
			format.PrintSetup.PrintToFileLicense = txtPrintLicense.Text

			' Do the printing in a thread so our user interface can still operate.
			Dim backgroundPrinter As New BackgroundWorker()
			AddHandler backgroundPrinter.DoWork, AddressOf backgroundPrinter_DoWork
			AddHandler backgroundPrinter.RunWorkerCompleted, AddressOf backgroundPrinter_RunWorkerCompleted

			backgroundPrinter.RunWorkerAsync(format)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Step 3: Select a printer file.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnOpenPrinterFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnOpenPrinterFile.Click
		 If openPrinterFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			txtPrinterFileFilename.Text = openPrinterFileDialog.FileName

			btnPrintPrinterFile.Enabled = True
		 End If
	  End Sub

	  ''' <summary>
	  ''' Step 3: Print the selected printer file using a background thread.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnPrintPrinterFile_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrintPrinterFile.Click
		 ' Let's disable the button so that the user can't repeatedly
		 ' hit it causing many threads.
		 btnPrintPrinterFile.Enabled = False

		 Dim backgroundFilePrinter As New BackgroundWorker()
		 AddHandler backgroundFilePrinter.DoWork, AddressOf backgroundFilePrinter_DoWork
		 AddHandler backgroundFilePrinter.RunWorkerCompleted, AddressOf backgroundFilePrinter_RunWorkerCompleted

		 backgroundFilePrinter.RunWorkerAsync(cboStep3Printer.SelectedItem.ToString())
	  End Sub

	  ''' <summary>
	  ''' When we select the printer to obtain a printer license in step 1, we
	  ''' need to select the same printer in the other 2 lists because that
	  ''' will most likely be the printer used.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub cboStep1Printer_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboStep1Printer.SelectedIndexChanged
		 cboStep2Printer.SelectedItem = cboStep1Printer.SelectedItem
		 cboStep3Printer.SelectedItem = cboStep1Printer.SelectedItem
	  End Sub
	  #End Region

	  #Region "Background Printing Thread"
	  ''' <summary>
	  ''' Prints the format to file in a separate thread so the UI will still update while it is printing.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundPrinter_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
		 Dim format As LabelFormatDocument = CType(e.Argument, LabelFormatDocument)

		 printThreadResultString = ""
		 backgroundPrintingThreadResult = Result.Failure
		 Try
			' Print the Format.
			backgroundPrintingThreadResult = format.Print(appName, Timeout.Infinite, printToFileMessages)
		 Catch ex As Seagull.BarTender.Print.PrintEngineException
			printThreadResultString = ex.Message
		 Catch ex As Exception
			printThreadResultString = ex.Message
		 End Try
	  End Sub

	  ''' <summary>
	  ''' Re-enables the print button when the print to file is completed.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundPrinter_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
		 ' Report the messages from the print job.
		 If printThreadResultString <> "" Then
			MessageBox.Show(Me, printThreadResultString, appName)
		 Else
			For index As Integer = 0 To printToFileMessages.Count - 1
			   MessageBox.Show(Me, printToFileMessages(index).Text, appName)
			Next index
		 End If

		 ' Restore the print button.
		 btnPrintToFile.Enabled = True

		 If backgroundPrintingThreadResult = Result.Success Then
			txtPrinterFileFilename.Text = format.PrintSetup.PrintToFileName
			btnPrintPrinterFile.Enabled = True
		 End If
	  End Sub
	  #End Region

	  #Region "Background Print Printer File Thread"
	  ''' <summary>
	  ''' Send a printer file to a printer.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundFilePrinter_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs)
		 ' Read in the printer file.
		 Dim sr As New StreamReader(txtPrinterFileFilename.Text, System.Text.Encoding.Default)
		 Dim printerFileData As String = sr.ReadToEnd()
		 sr.Close()

		 ' Print the printer file data to the selected printer. SendDataToPrinter returns
		 ' 1 for success and 0 for failure.
		 If printClient.SendPrintCode(e.Argument.ToString(), "Test Print Job", printerFileData) Then
			printToFileThreadResultString = String.Format("Spooling Succeeded" & Constants.vbLf + Constants.vbLf & "{0} was successfully spooled to {1}.", txtPrinterFileFilename.Text, e.Argument.ToString())
		 Else
			printToFileThreadResultString = "Printing Problem" & Constants.vbLf + Constants.vbLf & "There has been a problem printing. There may be a problem " & "with the printer or the print license may be incorrect. The print license " & "times out after 10 minutes, so you may have to print to file again."
		 End If
	  End Sub

	  ''' <summary>
	  ''' Restore the print button.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundFilePrinter_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs)
		 MessageBox.Show(Me, printToFileThreadResultString, appName)
		 btnPrintPrinterFile.Enabled = True
	  End Sub
	  #End Region
   End Class
End Namespace
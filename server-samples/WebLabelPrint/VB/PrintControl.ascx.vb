Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Web.UI

Imports Seagull.BarTender.Print
Imports Seagull.BarTender.PrintServer
Imports Seagull.BarTender.PrintServer.Tasks

Partial Public Class PrintControl
	Inherits System.Web.UI.UserControl
   #Region "Event Handlers"
   Public Event OnPrintPreview As EventHandler ' Fired when the Print Preview button is selected.
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  Dim printType As String = Request.QueryString.Get("print")
	  If printType IsNot Nothing AndAlso printType = "client" Then
		 _labelTitle.Text = "Print Label Format Over Internet"
		 _labelDescription.Text = "Select a label format and printer, and then print. " & ControlChars.CrLf & "                                    Printers shown in the list are Internet printers attached to " & ControlChars.CrLf & "                                    the client computer. Only client side printers that have a matching " & ControlChars.CrLf & "                                    server side printer driver installed are displayed."

		 _listPrinters.ShowClientPrinters = True
		 _listPrinters.ShowServerPrinters = False

		 ' Get the print license for the current printer when the print button is pressed.
		 _buttonPrint.OnClientClick = PrintToFileLicenseJavaScript
	  Else
		 _labelTitle.Text = "Print Label Format"
		 _labelDescription.Text = "Select a label format and printer, and then print.  Printers " & ControlChars.CrLf & "                                    shown in the list are Standard Windows printer drivers " & ControlChars.CrLf & "                                    (Seagull and non-Seagull) installed on the server computer."

		 _listPrinters.ShowClientPrinters = False
		 _listPrinters.ShowServerPrinters = True
	  End If

	  _listPrinters.Alert = _alert
	  _listPrinters.PrintButton = _buttonPrint
	  _listPrinters.PreviewButton = _buttonPrintPreview

	  ' Check to see if there were label formats found in the repository.  
	  If _listLabelFormats.HasLabelFormats Then
		 If (Not Page.IsPostBack) Then
			Dim fileName As String = Request.QueryString.Get("fileName")
			fileName = Server.UrlDecode(fileName)

			_listLabelFormats.SelectedLabelFormatName = fileName
		 End If

		 Try
			' Get the label format data from file and update print panel controls.
			Dim labelFormat As LabelFormat = GetLabelFormatDataFromFile(_listLabelFormats.SelectedLabelFormatRepositoryFullPath)
			If labelFormat IsNot Nothing Then
			   _copyControl.Alert = _alert

			   UpdateControls(labelFormat)

			   ' Clear messages
			   ShowMessage("")
			Else
				ShowMessage("Unable to print. Please make sure you have BarTender installed, activated as <br />Enterprise Print Server edition, and that print engines are running. See the <br />'Manage Print Engines' menu task.")
			End If
		 Catch ex As Exception
			ShowMessage(ex.Message)
		 End Try
	  Else
		 ShowMessage("There are no label formats in the respository available for printing.")
		 _buttonPrint.Enabled = False
		 _buttonPrintPreview.Enabled = False
		 _listPrinters.Enabled = False
	  End If
   End Sub
   #End Region

   #Region "Web Callbacks"
   ''' <summary>
   ''' Called when the Print button is selected.
   ''' </summary>
   Protected Sub ButtonPrint_Click(ByVal sender As Object, ByVal e As EventArgs)
	  Print()
   End Sub

   ''' <summary>
   ''' Called when the print preview button is selected.
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
   Protected Sub ButtonPrintPreview_Click(ByVal sender As Object, ByVal e As EventArgs)
	  ' Pass the event to the container if needed.
	 RaiseEvent OnPrintPreview(Me, e)
   End Sub

   ' Do post-processing of the client print. 
   ' Set the hidden field with the client print code and then call the javascript to print
   Private Sub TaskPrint_Succeeded(ByVal sender As Object, ByVal e As TaskEventArgs)
	  Dim taskPrint As PrintLabelFormatTask = TryCast(sender, PrintLabelFormatTask)

	  If taskPrint IsNot Nothing Then
		 _hiddenClientPrintCode.Value = taskPrint.PrintCode

		 Dim printerName As String = JavaScriptSupport.EscapeSpecialCharacters(_listPrinters.LastPrinterName)
		 Page.ClientScript.RegisterStartupScript(Me.GetType(), "SampleClientPrint", String.Format("SampleClientPrint('{0}','{1}','{2}','{3}');", printerName, _hiddenClientPrintCode.ClientID, _alert.MessageClientID, _alert.PanelClientID), True)
	  End If
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Return the label format object reference. Returns null if the Format's data is invalid.
   ''' </summary>
   Public ReadOnly Property LabelFormatObject() As LabelFormat
	  Get
		 Return CreateLabelFormatFromPageData(_listLabelFormats.SelectedLabelFormatRepositoryFullPath)
	  End Get
   End Property

   Public ReadOnly Property PrintToFileLicenseJavaScript() As String
	  Get
		 Return String.Format("SetPrintLicense(document.getElementById('{0}').value,'{1}','{2}','{3}');", _listPrinters.ListClientID, _listPrinters.PrintLicenseClientID, _alert.MessageClientID, _alert.PanelClientID)
	  End Get
   End Property
   #End Region

   #Region "Public Methods"
   Public Sub Print()
	  Try
		 Dim taskManager As TaskManager = CType(Application("TaskManager"), TaskManager)

		 ' Make sure the task manager is valid and that engines are running.
		 If (taskManager IsNot Nothing) AndAlso (taskManager.TaskEngines.AliveCount <> 0) Then
			Dim labelFormat As LabelFormat = CreateLabelFormatFromPageData(_listLabelFormats.SelectedLabelFormatRepositoryFullPath)
			If labelFormat IsNot Nothing Then
			   Dim taskPrint As New PrintLabelFormatTask(labelFormat)
			   taskPrint.PrintTimeout = Int32.Parse(ConfigurationManager.AppSettings("PrintTimeout"))

			   If _listPrinters.LastPrintType = "client" Then
				  ' Set PrintTimeout to 0 to ensure print job status monitoring does not interfere with client printing
				  taskPrint.PrintTimeout = 0
				  ' Since the client print needs to post-process the print job, sign up for the succeeded event.
				  AddHandler taskPrint.Succeeded, AddressOf TaskPrint_Succeeded
			   End If
			   taskManager.TaskQueue.QueueTaskAndWait(taskPrint, 60000)

			   ' Get messages
			   For Each message As Message In taskPrint.Messages
				  Dim formattedMessage As String = message.Text.Replace(Constants.vbLf, "<br/>")
				  _alert.AddMessage(formattedMessage & "<br/>")
			   Next message
			End If
		 Else
			ShowMessage("Unable to print. Please make sure you have BarTender installed, activated as <br />Enterprise Print Server edition, and that print engines are running. See the <br />'Manage Print Engines' menu task.")
		 End If
	  Catch ex As Exception
		 ShowMessage(ex.Message)
	  End Try
   End Sub
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Show a message
   ''' </summary>
   Private Sub ShowMessage(ByVal msg As String)
	  _alert.Message = msg
   End Sub

   ''' <summary>
   ''' Create a label format object using page data.
   ''' </summary>
   ''' <param name="labelFormatFullPath">Full path to the label format file name.</param>
   ''' <returns>A LabelFormat object reference. If the LabelFormat's data is invalid, null is returned.</returns>
   Private Function CreateLabelFormatFromPageData(ByVal labelFormatFullPath As String) As LabelFormat
	  Dim labelFormat As LabelFormat = Nothing
	  Dim isLabelDataValid As Boolean = True

	  ' Check if we are server or client printing
	  Dim lastPrinter As String = Request.Form.Get(_listPrinters.ListUniqueID)
	  If String.IsNullOrEmpty(lastPrinter) Then
		 ShowMessage("No printers were selected. Unable to print.")
	  Else
		 Try
			labelFormat = New LabelFormat(_listLabelFormats.SelectedLabelFormatRepositoryFullPath)
		 Catch e1 As System.IO.FileNotFoundException
			Dim message As String = String.Format("The selected format '{0}' does not exist. It may have been moved, renamed or deleted.", _listLabelFormats.SelectedLabelFormatName)
			ShowMessage(message)
			isLabelDataValid = False
		 End Try

		 If isLabelDataValid Then
			UpdateControls(labelFormat)

			' For each control get control values into the label format object.
			_subStringsControl.UpdateFormatData(labelFormat)
			_promptControl.UpdateFormatData(labelFormat)
			_queryPromptsControl.UpdateFormatData(labelFormat)
			isLabelDataValid = _copyControl.UpdateFormatData(labelFormat)

			labelFormat.PrintSetup.EnablePrompting = False

			If _listPrinters.LastPrintType = "server" Then
			   labelFormat.PrintSetup.PrintToFile = False
			   labelFormat.PrintSetup.PrinterName = _listPrinters.LastPrinterName
			Else
			   Dim compatibleServerPrinter As String = _listPrinters.LastCompatibleServerPrinter
			   labelFormat.PrintSetup.PrintToFile = True
			   labelFormat.PrintSetup.PrinterName = compatibleServerPrinter
			   labelFormat.PrintSetup.PrintToFileLicense = Request.Form.Get(_listPrinters.PrintLicenseUniqueID)
			   Dim tempFullPath As String = CStr(Application("TempFolderFullPath"))
			   labelFormat.PrintSetup.PrintToFileName = System.IO.Path.Combine(tempFullPath,Guid.NewGuid().ToString() & ".prn")
			End If
		 End If
	  End If

	  If isLabelDataValid Then
		  Return (labelFormat)
	  Else
		  Return (Nothing)
	  End If
   End Function

   ''' <summary>
   ''' Get the label format data from BarTender.
   ''' </summary>
   ''' <param name="labelFormatFullPath">Full path to the label format file name.</param>
   ''' <returns>A LabelFormat object reference.</returns>
   Private Function GetLabelFormatDataFromFile(ByVal labelFormatFullPath As String) As LabelFormat
	  Dim labelFormat As LabelFormat = Nothing

	  Dim taskManager As TaskManager = CType(Application("TaskManager"), TaskManager)

	  ' Make sure the task manager is valid and that engines are running.
	  If (taskManager IsNot Nothing) AndAlso (taskManager.TaskEngines.AliveCount <> 0) Then
		 ' Create a task to get all label format properties and submit to the task queue
		 ' for processing.
		 Dim getFormatTask As New GetLabelFormatTask(labelFormatFullPath)
		 taskManager.TaskQueue.QueueTaskAndWait(getFormatTask, 60000)

		 ' Return the label format object that has all label format data set.
		 labelFormat = getFormatTask.LabelFormat
	  End If
	  Return labelFormat
   End Function

   ''' <summary>
   ''' Update all user print panels with label format object data.
   ''' </summary>
   ''' <param name="labelFormat">A LabelFormat object reference.</param>
   Private Sub UpdateControls(ByVal labelFormat As LabelFormat)
	  If labelFormat IsNot Nothing Then
		 _copyControl.LabelFormatObject = labelFormat
		 _subStringsControl.LabelFormatObject = labelFormat
		 _promptControl.LabelFormatObject = labelFormat
		 _queryPromptsControl.LabelFormatObject = labelFormat
		 _listPrinters.LabelFormatObject = labelFormat
	  End If
   End Sub
   #End Region
End Class

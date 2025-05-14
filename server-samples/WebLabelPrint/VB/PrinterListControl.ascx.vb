Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print

Partial Public Class PrinterList
	Inherits System.Web.UI.UserControl

   #Region "Private Member Variables"

   Private _lastPrinterName As String = "" ' Printer name from last postback
   Private _lastPrintType As String = "" ' Print type from last postback (client or server)
   Private _lastCompatibleServerPrinter As String = "" ' Printer name of last compatible server printer (client only)
   Private _showServerPrinters As Boolean = False ' Show server printers
   Private _showClientPrinters As Boolean = False ' Show client printers
   Private _alert As Alert ' Alert control used for displaying error, warning, or informational messages.
   Private _printButton As Button ' Print button control reference
   Private _previewButton As Button ' Print Preview button control reference
   Private _labelFormat As LabelFormat ' Label format data object.

   #End Region

   #Region "Web Methods"

   ''' <summary>
   ''' Called when the page loads
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Fill the printer list
	  Dim printers As New Printers()

	  Dim serverPrinters As String = ""
	  Dim serverDrivers As String = ""

	  _listPrinters.Items.Clear()

	  For Each printer As Printer In printers
		 If _showServerPrinters Then
			Dim listItem As New ListItem(printer.PrinterName & " [On Server]", printer.PrinterName & ",server")
			_listPrinters.Items.Add(listItem)
		 End If

		 If _showClientPrinters Then

			' Create comma delimited lists to pass to javascript so the client javascript can match client printers
			serverDrivers &= printer.PrinterModel & ","
			serverPrinters &= printer.PrinterName & ","
		 End If
	  Next printer

	  Dim selectedPrinter As String = ""
	  selectedPrinter = GetLastSelectedPrinter()

	  If String.IsNullOrEmpty(selectedPrinter) Then
		 If printers.Default IsNot Nothing Then
			selectedPrinter = GetPrinterNameFromLabelFormat(printers.Default.PrinterName)
		 End If
	  End If

	  If _showClientPrinters Then
		 RegisterClientJavascript(serverPrinters,serverDrivers,selectedPrinter)
	  Else
		 _listPrinters.SelectedValue = selectedPrinter
	  End If

	  ' If only showing server printers, and there are none, then disable the control.
	  ' If showing client printers don't disable here because the javascript to add them hasn't run yet.
	  If _listPrinters.Items.Count = 0 AndAlso _showServerPrinters AndAlso (Not _showClientPrinters) Then
		 _listPrinters.Items.Add("No Printers Installed.")
		 _listPrinters.Enabled = False
		 _previewButton.Enabled = False
		 _printButton.Enabled = False
		 _alert.AddMessage("No printers are installed. <br /><br />Printer drivers must be installed in order to print.<br/>")
	  End If

   End Sub

   #End Region

   #Region "Public Properties"

   Public Property Enabled() As Boolean
	  Get
		 Return _listPrinters.Enabled
	  End Get
	  Set(ByVal value As Boolean)
		 _listPrinters.Enabled = value
	  End Set
   End Property

   Public ReadOnly Property LastPrinterName() As String
	  Get
		 Return _lastPrinterName
	  End Get
   End Property

   Public ReadOnly Property LastPrintType() As String
	  Get
		 Return _lastPrintType
	  End Get
   End Property

   Public ReadOnly Property LastCompatibleServerPrinter() As String
	  Get
		 Return _lastCompatibleServerPrinter
	  End Get
   End Property

   Public Property ShowServerPrinters() As Boolean
	  Get
		 Return _showServerPrinters
	  End Get
	  Set(ByVal value As Boolean)
		 _showServerPrinters = value
	  End Set
   End Property

   Public Property ShowClientPrinters() As Boolean
	  Get
		 Return _showClientPrinters
	  End Get
	  Set(ByVal value As Boolean)
		 _showClientPrinters = value
	  End Set
   End Property

   Public ReadOnly Property ListClientID() As String
	  Get
		 Return _listPrinters.ClientID
	  End Get
   End Property

   Public ReadOnly Property ListUniqueID() As String
	  Get
		 Return _listPrinters.UniqueID
	  End Get
   End Property

   Public ReadOnly Property PrintLicenseClientID() As String
	  Get
		 Return _hiddenPrintLicense.ClientID
	  End Get
   End Property

   Public ReadOnly Property PrintLicenseUniqueID() As String
	  Get
		 Return _hiddenPrintLicense.UniqueID
	  End Get
   End Property

   Public Property Alert() As Alert
	  Set(ByVal value As Alert)
		 _alert = value
	  End Set
	  Get
		 Return _alert
	  End Get
   End Property

   Public Property PrintButton() As Button
	  Set(ByVal value As Button)
		 _printButton = value
	  End Set
	  Get
		 Return _printButton
	  End Get
   End Property

   Public Property PreviewButton() As Button
	  Set(ByVal value As Button)
		 _previewButton = value
	  End Set
	  Get
		 Return _previewButton
	  End Get
   End Property

   ''' <summary>
   ''' Sets the LabelFormat that will be used to get the default printer
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 _labelFormat = value
	  End Set
   End Property

   #End Region

   #Region "Support Methods"

   ''' <summary>
   ''' Gets the printer that was last selected by the user on this page
   ''' </summary>
   ''' <returns>The last selected printer or an empty string if there wasn't one.</returns>
   Private Function GetLastSelectedPrinter() As String
	  Dim lastPrinter As String = Request.Form.Get(ListUniqueID)
	  Dim selectedPrinter As String = ""
	  If (Not String.IsNullOrEmpty(lastPrinter)) Then
		 Dim printerInfo() As String = lastPrinter.Split(","c)
		 _lastPrinterName = printerInfo(0)
		 _lastPrintType = printerInfo(1)
		 If printerInfo.Length > 2 Then
			_lastCompatibleServerPrinter = printerInfo(2)
		 End If
		 selectedPrinter = lastPrinter
	  End If
	  Return selectedPrinter
   End Function

   ''' <summary>
   ''' Gets the format's default printer.
   ''' </summary>
   ''' <param name="machineDefaultPrinter">The name of the default printer.</param>
   ''' <returns>The format's default printer, or an empty string if using the windows default.</returns>
   Private Function GetPrinterNameFromLabelFormat(ByVal machineDefaultPrinter As String) As String
	  Dim selectedPrinter As String = ""
	  If _showServerPrinters Then
		 ' Select the format's default printer or the computer's default printer if applicable
		 If _labelFormat Is Nothing OrElse String.IsNullOrEmpty(_labelFormat.PrintSetup.PrinterName) Then
			selectedPrinter = machineDefaultPrinter & ",server"
		 Else
			selectedPrinter = _labelFormat.PrintSetup.PrinterName & ",server"
		 End If
	  End If
	  If (Not _showServerPrinters) AndAlso _showClientPrinters Then
		 If _labelFormat Is Nothing OrElse String.IsNullOrEmpty(_labelFormat.PrintSetup.PrinterName) Then
			selectedPrinter = ""
		 Else
			selectedPrinter = _labelFormat.PrintSetup.PrinterName
		 End If
	  End If
	  Return selectedPrinter
   End Function

   ''' <summary>
   ''' Registers the client javascript that gets the list of matching client printers.
   ''' </summary>
   ''' <param name="serverPrinters">The list of server printer names.</param>
   ''' <param name="serverDrivers">The list of server driver names.</param>
   ''' <param name="selectedPrinter">The selected printer.</param>
   Private Sub RegisterClientJavascript(ByVal serverPrinters As String, ByVal serverDrivers As String, ByVal selectedPrinter As String)
	  ' Escape special characters so the javascript is evaluated correctly.
	  serverPrinters = JavaScriptSupport.EscapeSpecialCharacters(serverPrinters)
	  serverDrivers = JavaScriptSupport.EscapeSpecialCharacters(serverDrivers)
	  Dim lastPrinter As String = JavaScriptSupport.EscapeSpecialCharacters(_lastPrinterName)
	  Dim printerNameFromLabelFormat As String = JavaScriptSupport.EscapeSpecialCharacters(selectedPrinter)

	  ' Register client script to fill in client printers
	  If _printButton Is Nothing Then
		  If _previewButton Is Nothing Then
			  Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddClientPrintersToList", String.Format("AddClientPrintersToList('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinter, printerNameFromLabelFormat, _listPrinters.ClientID, _alert.MessageClientID, _alert.PanelClientID, (Nothing), (Nothing)), True)
		  Else
			  Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddClientPrintersToList", String.Format("AddClientPrintersToList('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinter, printerNameFromLabelFormat, _listPrinters.ClientID, _alert.MessageClientID, _alert.PanelClientID, (Nothing), (_previewButton.ClientID)), True)
		  End If
	  Else
		  If _previewButton Is Nothing Then
			  Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddClientPrintersToList", String.Format("AddClientPrintersToList('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinter, printerNameFromLabelFormat, _listPrinters.ClientID, _alert.MessageClientID, _alert.PanelClientID, (_printButton.ClientID), (Nothing)), True)
		  Else
			  Page.ClientScript.RegisterStartupScript(Me.GetType(), "AddClientPrintersToList", String.Format("AddClientPrintersToList('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinter, printerNameFromLabelFormat, _listPrinters.ClientID, _alert.MessageClientID, _alert.PanelClientID, (_printButton.ClientID), (_previewButton.ClientID)), True)
		  End If
	  End If
   End Sub

   #End Region
End Class

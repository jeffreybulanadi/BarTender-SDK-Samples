Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls
Imports Seagull.BarTender.Print

Partial Public Class PrinterList
	Inherits System.Web.UI.UserControl

   #Region "Private Member Variables"

   Private m_lastPrinterName As String = "" ' Printer name from last postback
   Private m_lastPrintType As String = "" ' Print type from last postback (client or server)
   Private m_lastCompatibleServerPrinter As String = "" ' Printer name of last compatible server printer (client only)
   Private m_showServerPrinters As Boolean = False ' Show server printers
   Private m_showClientPrinters As Boolean = False ' Show client printers
   Private m_alert As Alert ' Alert control used for displaying error, warning, or informational messages.
   Private m_printButton As Button ' Print button control reference
   Private m_previewButton As Button ' Print Preview button control reference
   Private m_labelFormat As LabelFormat ' Label format data object.

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

	  lstPrinters.Items.Clear()

	  For Each printer As Printer In printers
		 If m_showServerPrinters Then
			Dim listItem As New ListItem(printer.PrinterName & " [On Server]", printer.PrinterName & ",server")
			lstPrinters.Items.Add(listItem)
		 End If

		 If m_showClientPrinters Then

			' Create comma delimited lists to pass to javascript so the client javascript can match client printers
			serverDrivers &= printer.PrinterModel & ","
			serverPrinters &= printer.PrinterName & ","
		 End If
	  Next printer

	  Dim selectedPrinter As String = ""
	  Dim lastPrinter As String = Request.Form.Get(ListUniqueID)
	  If (Not String.IsNullOrEmpty(lastPrinter)) Then
		 Dim printerInfo() As String = lastPrinter.Split(","c)
		 m_lastPrinterName = printerInfo(0)
		 m_lastPrintType = printerInfo(1)
		 If printerInfo.Length > 2 Then
			m_lastCompatibleServerPrinter = printerInfo(2)
		 End If
		 selectedPrinter = lastPrinter
	  End If

	  If String.IsNullOrEmpty(selectedPrinter) Then
		 If m_showServerPrinters Then
			'select the format's default printer or the computer's default printer if applicable
			If m_labelFormat Is Nothing OrElse String.IsNullOrEmpty(m_labelFormat.PrintSetup.PrinterName) Then
			   selectedPrinter = printers.Default.PrinterName & ",server"
			Else
			   selectedPrinter = m_labelFormat.PrintSetup.PrinterName & ",server"
			End If
		 End If
		 If (Not m_showServerPrinters) AndAlso m_showClientPrinters Then
			If m_labelFormat Is Nothing OrElse String.IsNullOrEmpty(m_labelFormat.PrintSetup.PrinterName) Then
			   selectedPrinter = ""
			Else
			   selectedPrinter = m_labelFormat.PrintSetup.PrinterName
			End If
		 End If
	  End If

	  If m_showClientPrinters Then

		 Page.ClientScript.RegisterClientScriptInclude("ClientPrintingInclude", "./JavaScript/ClientPrinting.js")

		 ' Double up backslashes because the escape code will be evaluated twice
		 serverPrinters = serverPrinters.Replace("\", "\\")
		 serverDrivers = serverDrivers.Replace("\", "\\")
		 Dim lastPrinterDoubled As String = m_lastPrinterName.Replace("\", "\\")
		 Dim formatPrinterDoubled As String = selectedPrinter.Replace("\", "\\")

		 ' Register client script to fill in client printers
		 If m_printButton Is Nothing Then
			 If m_previewButton Is Nothing Then
				 Page.ClientScript.RegisterStartupScript(Me.GetType(), "GetClientPrinters", String.Format("GetClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinterDoubled, lastPrinterDoubled, lstPrinters.ClientID, m_alert.MessageClientID, m_alert.PanelClientID, (Nothing), (Nothing)), True)
			 Else
				 Page.ClientScript.RegisterStartupScript(Me.GetType(), "GetClientPrinters", String.Format("GetClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinterDoubled, lastPrinterDoubled, lstPrinters.ClientID, m_alert.MessageClientID, m_alert.PanelClientID, (Nothing), (m_previewButton.ClientID)), True)
			 End If
		 Else
			 If m_previewButton Is Nothing Then
				 Page.ClientScript.RegisterStartupScript(Me.GetType(), "GetClientPrinters", String.Format("GetClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinterDoubled, lastPrinterDoubled, lstPrinters.ClientID, m_alert.MessageClientID, m_alert.PanelClientID, (m_printButton.ClientID), (Nothing)), True)
			 Else
				 Page.ClientScript.RegisterStartupScript(Me.GetType(), "GetClientPrinters", String.Format("GetClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, lastPrinterDoubled, lastPrinterDoubled, lstPrinters.ClientID, m_alert.MessageClientID, m_alert.PanelClientID, (m_printButton.ClientID), (m_previewButton.ClientID)), True)
			 End If
		 End If
	  Else
		 lstPrinters.SelectedValue = selectedPrinter
	  End If

   End Sub

   #End Region

   #Region "Public Properties"

   Public Property Enabled() As Boolean
	  Get
		 Return lstPrinters.Enabled
	  End Get
	  Set(ByVal value As Boolean)
		 lstPrinters.Enabled = value
	  End Set
   End Property

   Public ReadOnly Property LastPrinterName() As String
	  Get
		 Return m_lastPrinterName
	  End Get
   End Property

   Public ReadOnly Property LastPrintType() As String
	  Get
		 Return m_lastPrintType
	  End Get
   End Property

   Public ReadOnly Property LastCompatibleServerPrinter() As String
	  Get
		 Return m_lastCompatibleServerPrinter
	  End Get
   End Property

   Public Property ShowServerPrinters() As Boolean
	  Get
		 Return m_showServerPrinters
	  End Get
	  Set(ByVal value As Boolean)
		 m_showServerPrinters = value
	  End Set
   End Property

   Public Property ShowClientPrinters() As Boolean
	  Get
		 Return m_showClientPrinters
	  End Get
	  Set(ByVal value As Boolean)
		 m_showClientPrinters = value
	  End Set
   End Property

   Public ReadOnly Property ListClientID() As String
	  Get
		 Return lstPrinters.ClientID
	  End Get
   End Property

   Public ReadOnly Property ListUniqueID() As String
	  Get
		 Return lstPrinters.UniqueID
	  End Get
   End Property

   Public ReadOnly Property PrintLicenseClientID() As String
	  Get
		 Return hfPrintLicense.ClientID
	  End Get
   End Property

   Public ReadOnly Property PrintLicenseUniqueID() As String
	  Get
		 Return hfPrintLicense.UniqueID
	  End Get
   End Property

   Public Property Alert() As Alert
	  Set(ByVal value As Alert)
		 m_alert = value
	  End Set
	  Get
		 Return m_alert
	  End Get
   End Property

   Public Property PrintButton() As Button
	  Set(ByVal value As Button)
		 m_printButton = value
	  End Set
	  Get
		 Return m_printButton
	  End Get
   End Property

   Public Property PreviewButton() As Button
	  Set(ByVal value As Button)
		 m_previewButton = value
	  End Set
	  Get
		 Return m_previewButton
	  End Get
   End Property

   ''' <summary>
   ''' Sets the LabelFormat that will be used to get the default printer
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 m_labelFormat = value
	  End Set
   End Property

   #End Region
End Class

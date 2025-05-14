Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print

Partial Public Class ServerPrinters
	Inherits System.Web.UI.Page
   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Show the current menu selection by highlighting the selected menu link's background.
	  CType(Master.FindControl("_printers"), HyperLink).CssClass = "MenuLinkSelected"

	  ' Clear any error messages
	  _alert.Message = ""

	  ' Display server printers
	  Dim printers As New Printers()

	  If printers.Count > 0 Then
		 Dim dataset As New DataSet()

		 Dim sPrinterXML As String = printers.XML

		 Dim stringReader As New System.IO.StringReader(sPrinterXML)
		 dataset.ReadXml(stringReader)

		 _gridServerPrinters.DataSource = dataset

		 _gridServerPrinters.DataBind()
	  Else
		 'No printers, so display an error message.
		 _gridServerPrinters.Visible = False
		 _noPrintersErrorAlert.Visible = True
	  End If

	  ' Display client printers
	  ' Get the list of printers in comma delimited form
	  Dim serverPrinters As String = String.Empty
	  Dim serverDrivers As String = String.Empty

	  For Each printer As Printer In printers
		 serverPrinters &= printer.PrinterName & ","
		 serverDrivers &= printer.PrinterModel & ","
	  Next printer

	  serverPrinters = JavaScriptSupport.EscapeSpecialCharacters(serverPrinters)
	  serverDrivers = JavaScriptSupport.EscapeSpecialCharacters(serverDrivers)

	  _noClientPrintersErrorAlert.ShowAlertPanel(False)
	  _noMatchingClientPrintersErrorAlert.ShowAlertPanel(False)

	  ' Register client script to fill in client printers
	  Page.ClientScript.RegisterStartupScript(Me.GetType(), "DisplayClientPrinters", String.Format("DisplayClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');", serverPrinters, serverDrivers, _tableClientPrinters.ClientID, _tableAvailableClientPrinters.ClientID, _panelClientPrinting.ClientID, _alert.MessageClientID, _alert.PanelClientID, _noClientPrintersErrorAlert.PanelClientID, _noMatchingClientPrintersErrorAlert.PanelClientID), True)
   End Sub
   #End Region
End Class

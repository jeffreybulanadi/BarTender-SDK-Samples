Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print
Imports Seagull.BarTender.PrintServer
Imports Seagull.BarTender.PrintServer.Tasks

Partial Public Class Print
	Inherits System.Web.UI.Page
   #Region "Web Methods"

   Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
	  AddHandler _controlPrint.OnPrintPreview, AddressOf ControlPrint_OnPrintPreview
	  AddHandler _controlPrintPreview.OnBackEvent, AddressOf ControlPrintPreview_OnBackEvent
	  AddHandler _controlPrintPreview.OnPrintEvent, AddressOf ControlPrintPreview_OnPrintEvent
   End Sub

   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  Dim printType As String = Request.QueryString.Get("print")
	  If printType IsNot Nothing AndAlso printType = "client" Then
		  CType(Master.FindControl("_printToClient"), HyperLink).CssClass = "MenuLinkSelected"
		  _controlPrintPreview.PrintButtonClickJavaScript = _controlPrint.PrintToFileLicenseJavaScript
	  Else
		  CType(Master.FindControl("_printToServer"), HyperLink).CssClass = "MenuLinkSelected"
	  End If

	  If (Not Page.IsPostBack) Then
		 ShowPrintPreview(False)
	  End If
   End Sub
   #End Region

   #Region "Web Callbacks"
   ''' <summary>
   ''' Called when the Print Preview button is selected in the Print control.
   ''' </summary>
   Protected Sub ControlPrint_OnPrintPreview(ByVal sender As Object, ByVal e As EventArgs)
	  Dim labelFormat As LabelFormat = _controlPrint.LabelFormatObject
	  If labelFormat IsNot Nothing Then
		 _controlPrintPreview.LabelFormatObject = labelFormat
		 ShowPrintPreview(True)
	  End If
   End Sub

   ''' <summary>
   ''' Called when the Print Preview back button is selected.
   ''' </summary>
   Private Sub ControlPrintPreview_OnBackEvent(ByVal sender As Object, ByVal e As EventArgs)
	  ShowPrintPreview(False)
   End Sub

   ''' <summary>
   ''' Called when the Print button is selected on the Print Preview panel.
   ''' </summary>
   Private Sub ControlPrintPreview_OnPrintEvent(ByVal sender As Object, ByVal e As EventArgs)
	  ShowPrintPreview(False)
	  _controlPrint.Print()
   End Sub

   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Show the Print Preview panel. Hide the Print Panel.
   ''' </summary>
   ''' <param name="show">True to show the print preview panel.</param>
   Private Sub ShowPrintPreview(ByVal show As Boolean)
	  ShowPanel(_panelPrint, (Not show))
	  ShowPanel(_panelPrintPreview, show)
   End Sub

   ''' <summary>
   ''' Show/Hide the panel using styles.
   ''' </summary>
   Private Sub ShowPanel(ByVal panel As Panel, ByVal show As Boolean)
	  If show Then
		 panel.Style("display") = ""
	  Else
		 panel.Style("display") = "none"
	  End If
   End Sub

   #End Region
End Class

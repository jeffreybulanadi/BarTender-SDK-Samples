Imports Microsoft.VisualBasic
Imports System

Partial Public Class Alert
	Inherits System.Web.UI.UserControl
   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Add JavaScript to the OnClick event of the close link to hide the alert box.
	  Dim hideJavaScript As String = String.Format("document.getElementById('{0}').style.display = 'none';", _panelAlert.ClientID)
	  _linkClose.Attributes("OnClick") = hideJavaScript
   End Sub
   #End Region

   #Region "Public Methods"
   ''' <summary>
   ''' Add a new message to the alert.
   ''' </summary>
   ''' <param name="msg">Message text.</param>
   Public Sub AddMessage(ByVal msg As String)
	  If Message.Length > 0 Then
		 Message &= "<hr/>"
	  End If
	  Message += msg
   End Sub

   ''' <summary>
   ''' Show/Hide the alert panel.
   ''' </summary>
   ''' <param name="show">True to show alert panel, else False.</param>
   Public Sub ShowAlertPanel(ByVal show As Boolean)
	  If show Then
		 _panelAlert.Style("display") = "block"
	  Else
		 _panelAlert.Style("display") = "none"
	  End If
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Set/Get the alert message. Will show the panel if a message is set, or will hide 
   ''' the panel if the message is an empty string.
   ''' </summary>
   Public Property Message() As String
	  Set(ByVal value As String)
		 _labelMessage.Text = value
		 ShowAlertPanel(_labelMessage.Text.Length <> 0)
	  End Set
	  Get
		 Return _labelMessage.Text
	  End Get
   End Property

   ''' <summary>
   ''' Get the message client ID.
   ''' </summary>
   Public ReadOnly Property MessageClientID() As String
	  Get
		 Return _labelMessage.ClientID
	  End Get
   End Property

   ''' <summary>
   ''' Get the panel client ID
   ''' </summary>
   Public ReadOnly Property PanelClientID() As String
	  Get
		 Return _panelAlert.ClientID
	  End Get
   End Property
   #End Region
End Class

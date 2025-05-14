Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI.WebControls

Partial Public Class _Default
	Inherits System.Web.UI.Page
   #Region "Web Methods   "
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	' Show the current menu selection by highlighting the selected menu link's background.
	CType(Master.FindControl("_home"), HyperLink).CssClass = "MenuLinkSelected"
   End Sub
   #End Region
End Class

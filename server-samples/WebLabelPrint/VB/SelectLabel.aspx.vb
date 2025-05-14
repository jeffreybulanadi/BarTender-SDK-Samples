Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Partial Public Class SelectLabel
	Inherits System.Web.UI.Page
   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Show the current menu selection by highlighting the selected menu link's background.
	  CType(Master.FindControl("_selectLabel"), HyperLink).CssClass = "MenuLinkSelected"

	  Try
		 ' Get a list of all label formats found in the repository with extension *.btw
		 Dim labelRepositoryFolder As String = ConfigurationManager.AppSettings("LabelRepository")
		 Dim searchOption As SearchOption = SearchOption.AllDirectories
		 Dim formatPath As String = Server.MapPath(labelRepositoryFolder)
		 Dim sFileNames() As String = Directory.GetFiles(formatPath, "*.btw", searchOption)

		 If sFileNames.Length <> 0 Then
			' Setup URL based on print method selected.
			Dim clickUrl As String
			If _listPrintMethod.SelectedValue = "client" Then
			   clickUrl = "./Print.aspx?print=client"
			Else
			   clickUrl = "./Print.aspx?print=server"
			End If

			' Generate an image thumbnail for each label format found in the repository.
			' Add the image thumbnail to the label browser control.
			' All images are saved in the "Temp" folder located in the web application folder.
			' The temporary images are deleted when the web application is closed. See Global.asax.
			For i As Integer = 0 To sFileNames.Length - 1
			   Dim sName As String = Path.GetFileName(sFileNames(i))

			   Dim thumbnail As LabelThumbnail = CType(LoadControl("LabelThumbnail.ascx"), LabelThumbnail)
			   thumbnail.LabelFullPath = sFileNames(i)
			   thumbnail.ThumbnailWidth = 128
			   thumbnail.ThumbnailHeight = 128
			   thumbnail.ShowFilename = True
			   thumbnail.ClickURL = clickUrl & "&filename=" & sName
			   thumbnail.CssClass = "LabelBrowserItems"

			   _labelBrowser.Controls.Add(thumbnail)
			Next i

			ShowMessage("")
		 Else
			ShowMessage("There are no label formats in the repository.")
		 End If
	  Catch ex As Exception
		 ShowMessage("Unable to browse label formats. Please make sure you have BarTender installed and <br />activated as Enterprise Print Server edition.<br /><br />Details: " & ex.Message)
	  End Try
   End Sub
   #End Region

   #Region "Support Methods"
   Private Sub ShowMessage(ByVal msg As String)
	  _alert.Message = msg

	  ' Hide the panel if there is a message.
	  _panelPrintMethod.Visible = (msg.Length = 0)
   End Sub
   #End Region
End Class

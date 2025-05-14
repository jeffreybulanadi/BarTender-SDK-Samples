Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Data
Imports System.Configuration
Imports System.Collections
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
Imports System.Web.UI.HtmlControls

Partial Public Class FormatThumbnail
	Inherits System.Web.UI.UserControl
   ' Declare private members
   Private labelFileName_Renamed As String ' Label format filename
   Private labelFullPath_Renamed As String ' Label format full path (Folder and filename combined)
   Private width As Integer = 128 ' Width of image
   Private height As Integer = 128 ' Height of image
   Private showFilename_Renamed As Boolean = True ' Show filename under image
   Private clickURL_Renamed As String = "" ' URL to navigate to when clicked.

   #Region "WebMethods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  If (Not IsPostBack) Then
		 If labelFullPath_Renamed IsNot Nothing Then
			Dim labelRepositoryFolder As String = ConfigurationManager.AppSettings("LabelRespository")
			labelFileName_Renamed = System.IO.Path.GetFileName(labelFullPath_Renamed)

			' Get and generate if needed a reference to the label format thumbnail image. 
			imgThumbnail.ImageUrl = LabelFormatThumbnailImage.GetURL(labelFullPath_Renamed, Path.GetDirectoryName(labelFullPath_Renamed), "./" & labelRepositoryFolder, System.Drawing.Color.Gray, width, height, System.Drawing.Imaging.ImageFormat.Png)
			imgThumbnail.Visible = True
			imgThumbnail.CssClass = "LabelItemImage"

			If showFilename_Renamed Then
			   lblThumbnailText.Text = "<br/>" & labelFileName_Renamed
			Else
			   lblThumbnailText.Visible = False
			End If
		 End If
	  End If

	  If clickURL_Renamed <> "" Then
		 ' Setup navigation links
		 imageThumbnailLink.NavigateUrl = clickURL_Renamed
		 imageThumbnailLink.Enabled = True
		 textThumbnailLink.NavigateUrl = clickURL_Renamed
		 textThumbnailLink.Enabled = True
	  End If
   End Sub
   #End Region

   #Region "Properties"
   ''' <summary>
   ''' Set/Get the full path to the label format.
   ''' </summary>
   Public Property LabelFullPath() As String
	  Get
		 Return labelFullPath_Renamed
	  End Get
	  Set(ByVal value As String)
		 labelFullPath_Renamed = value
	  End Set
   End Property

   ''' <summary>
   ''' Return the label format file name.
   ''' </summary>
   Public ReadOnly Property LabelFileName() As String
	  Get
		 Return labelFileName_Renamed
	  End Get
   End Property

   ''' <summary>
   ''' Set/Get the thumbnails width in pixels
   ''' </summary>
   Public Property ThumbnailWidth() As Integer
	  Get
		 Return width
	  End Get
	  Set(ByVal value As Integer)
		 width = value
	  End Set
   End Property

   ''' <summary>
   ''' Set/Get the thumbnails height in pixels
   ''' </summary>
   Public Property ThumbnailHeight() As Integer
	  Get
		 Return height
	  End Get
	  Set(ByVal value As Integer)
		 height = value
	  End Set
   End Property

   ''' <summary>
   ''' Specify to show the filename as a link below the thumbnail image.
   ''' </summary>
   Public Property ShowFilename() As Boolean
	  Get
		 Return showFilename_Renamed
	  End Get
	  Set(ByVal value As Boolean)
		 showFilename_Renamed = value
	  End Set
   End Property

   ''' <summary>
   ''' Specify the URL reference used when the thumbnail or text link is clicked.
   ''' </summary>
   Public Property ClickURL() As String
	  Get
		 Return clickURL_Renamed
	  End Get
	  Set(ByVal value As String)
		 clickURL_Renamed = value
	  End Set
   End Property

   ''' <summary>
   ''' Set/Get the CSS class name used for styling the page.
   ''' </summary>
   Public Property CssClass() As String
	  Get
		 Return panelLabelBrowserItem.CssClass
	  End Get
	  Set(ByVal value As String)
		 panelLabelBrowserItem.CssClass = value
	  End Set
   End Property
   #End Region
End Class

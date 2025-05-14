Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Web.UI

Partial Public Class LabelThumbnail
	Inherits System.Web.UI.UserControl
   #Region "Private Member Variables"
   Private _labelFileName As String ' Label format filename
   Private _labelFullPath As String ' Label format full path (Folder and filename combined)
   Private _width As Integer = 128 ' Width of image
   Private _height As Integer = 128 ' Height of image
   Private _showFilename As Boolean = True ' Show filename under image
   Private _clickURL As String = "" ' URL to navigate to when clicked.
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  If (Not IsPostBack) Then
		 If _labelFullPath IsNot Nothing Then
			Dim labelRepositoryFolder As String = ConfigurationManager.AppSettings("LabelRepository")
			_labelFileName = System.IO.Path.GetFileName(_labelFullPath)

			' Get and generate if needed a reference to the label format thumbnail image. 
			_imageThumbnail.ImageUrl = LabelFormatThumbnailImage.GetURL(_labelFullPath, Path.GetDirectoryName(_labelFullPath), labelRepositoryFolder, System.Drawing.Color.Gray, _width, _height, System.Drawing.Imaging.ImageFormat.Png)
			_imageThumbnail.Visible = True
			_imageThumbnail.CssClass = "LabelItemImage"

			If _showFilename Then
			   _labelThumbnailText.Text = "<br/>" & _labelFileName
			Else
			   _labelThumbnailText.Visible = False
			End If
		 End If
	  End If

	  If _clickURL <> "" Then
		 ' Setup navigation links
		 _imageThumbnailLink.NavigateUrl = _clickURL
		 _imageThumbnailLink.Enabled = True
		 _textThumbnailLink.NavigateUrl = _clickURL
		 _textThumbnailLink.Enabled = True
	  End If
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Set/Get the full path to the label format.
   ''' </summary>
   Public Property LabelFullPath() As String
	  Get
		 Return _labelFullPath
	  End Get
	  Set(ByVal value As String)
		 _labelFullPath = value
	  End Set
   End Property

   ''' <summary>
   ''' Return the label format file name.
   ''' </summary>
   Public ReadOnly Property LabelFileName() As String
	  Get
		 Return _labelFileName
	  End Get
   End Property

   ''' <summary>
   ''' Set/Get the thumbnails width in pixels
   ''' </summary>
   Public Property ThumbnailWidth() As Integer
	  Get
		 Return _width
	  End Get
	  Set(ByVal value As Integer)
		 _width = value
	  End Set
   End Property

   ''' <summary>
   ''' Set/Get the thumbnails height in pixels
   ''' </summary>
   Public Property ThumbnailHeight() As Integer
	  Get
		 Return _height
	  End Get
	  Set(ByVal value As Integer)
		 _height = value
	  End Set
   End Property

   ''' <summary>
   ''' Specify to show the filename as a link below the thumbnail image.
   ''' </summary>
   Public Property ShowFilename() As Boolean
	  Get
		 Return _showFilename
	  End Get
	  Set(ByVal value As Boolean)
		 _showFilename = value
	  End Set
   End Property

   ''' <summary>
   ''' Specify the URL reference used when the thumbnail or text link is clicked.
   ''' </summary>
   Public Property ClickURL() As String
	  Get
		 Return _clickURL
	  End Get
	  Set(ByVal value As String)
		 _clickURL = value
	  End Set
   End Property

   ''' <summary>
   ''' Set/Get the CSS class name used for styling the page.
   ''' </summary>
   Public Property CssClass() As String
	  Get
		 Return _panelLabelBrowserItem.CssClass
	  End Get
	  Set(ByVal value As String)
		 _panelLabelBrowserItem.CssClass = value
	  End Set
   End Property
   #End Region
End Class

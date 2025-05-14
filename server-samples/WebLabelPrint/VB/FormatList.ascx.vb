Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Web.UI.WebControls

Partial Public Class FormatList
	Inherits System.Web.UI.UserControl
   #Region "Private Member Variables"
   Private _selectedLabelName As String = "" ' Selected label format name.
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page loads
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
   End Sub

   ''' <summary>
   ''' Called when the page is initialized
   ''' </summary>
   Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
	  ' Get the repository folder from Web.Config.
	  Dim labelRepositoryFolder As String = ConfigurationManager.AppSettings("LabelRepository")

	  ' Get a list of all label formats found in the repository.
	  Dim searchOption As SearchOption = SearchOption.TopDirectoryOnly
        Dim repositoryPath As String = Server.MapPath(labelRepositoryFolder)
        Dim sFileNames() As String = Directory.GetFiles(repositoryPath, "*.btw", searchOption)

	  If sFileNames.Length <> 0 Then
		 ' Add each label format to the list control.
		 For i As Integer = 0 To sFileNames.Length - 1
			Dim sName As String = Path.GetFileName(sFileNames(i))

			Dim item As New ListItem(sName, sName)
			_listFormats.Items.Add(item)
		 Next i

		 If (Not String.IsNullOrEmpty(_selectedLabelName)) Then
			_listFormats.SelectedValue = _selectedLabelName
		 End If
	  Else
		 Dim itemText As String = "No Labels Available"
		 _listFormats.Enabled = False
		 _listFormats.Items.Add(New ListItem(itemText, itemText))
		 _listFormats.SelectedValue = itemText
	  End If

	  If Page.IsPostBack Then
		 _selectedLabelName = Request.Form.Get(_listFormats.UniqueID)

		 If _selectedLabelName IsNot Nothing Then
			_listFormats.SelectedValue = _selectedLabelName
		 End If
	  End If
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Does the label format list have any formats.
   ''' </summary>
   Public ReadOnly Property HasLabelFormats() As Boolean
	  Get
		 Return _listFormats.Enabled
	  End Get
   End Property

   ''' <summary>
   ''' Sets/Gets the system's full repository path to the selected label.
   ''' </summary>
   Public ReadOnly Property SelectedLabelFormatRepositoryFullPath() As String
	  Get
		 ' Get the repository folder from Web.Config.
		 Dim labelRepositoryFolder As String = ConfigurationManager.AppSettings("LabelRepository")

		 ' Get full path to folder.
		 Dim repositoryPath As String = Server.MapPath(labelRepositoryFolder)
		 Dim formatPath As String = String.Empty

		 If (Not String.IsNullOrEmpty(_selectedLabelName)) Then
			formatPath = Path.Combine(repositoryPath, _selectedLabelName)
		 ElseIf _listFormats.SelectedItem IsNot Nothing Then
			formatPath = Path.Combine(repositoryPath, _listFormats.SelectedValue)
		 Else
			formatPath = ""
		 End If

		 Return formatPath
	  End Get
   End Property

   ''' <summary>
   ''' Sets/Gets the selected label format name. This is the name only.
   ''' </summary>
   Public Property SelectedLabelFormatName() As String
	  Get
		 Return _selectedLabelName
	  End Get
	  Set(ByVal value As String)
		 If _listFormats IsNot Nothing Then
			_listFormats.SelectedValue = value
			_selectedLabelName = value
		 End If
	  End Set
   End Property
   #End Region

End Class

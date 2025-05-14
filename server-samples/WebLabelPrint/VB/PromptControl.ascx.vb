Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Xml
Imports System.Xml.Xsl

Imports Seagull.BarTender.Print

Partial Public Class PromptControl
	Inherits System.Web.UI.UserControl
   #Region "Private Member Variables"
   Private _labelFormat As LabelFormat ' Label format data object.
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  CreateControls(_labelFormat)
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Sets the LabelFormat that will be used to get prompts.
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 _labelFormat = value
	  End Set
   End Property
   #End Region

   #Region "Public Methods"
   ''' <summary>
   ''' Gets the input prompt data and sets it on the given LabelFormat.
   ''' </summary>
   ''' <param name="format">The LabelFormat to set prompt data on.</param>
   Public Sub UpdateFormatData(ByVal format As LabelFormat)
	  For Each control As Control In _panelPrompts.Controls
		 Dim promptName As String = ""

		 If control.ID IsNot Nothing Then
			promptName = control.ID.Replace("_"c, " "c)
			promptName = promptName.Substring(format.BaseName.Length)
		 End If
		 If TypeOf control Is TextBox Then
			Dim textBox As TextBox = TryCast(control, TextBox)

			format.Prompts.SetPrompt(promptName, textBox.Text)
		 ElseIf TypeOf control Is ListControl Then
			Dim listControl As ListControl = TryCast(control, ListControl)

			format.Prompts.SetPrompt(promptName, listControl.SelectedValue)
		 End If
	  Next control
   End Sub
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Update controls on the page using the label format object data.
   ''' </summary>
   ''' <param name="labelFormat">Label format object.</param>
   Private Sub CreateControls(ByVal labelFormat As LabelFormat)
	  If labelFormat IsNot Nothing Then
		 Dim xml As String = labelFormat.Prompts.LayoutXML

		 ' Show the control if there is an xml layout.
		 If xml <> "" Then
			Dim xslPath As String = Server.MapPath("XSL\Prompt.xsl")

			' Transform the prompt layout XML using an xsl transform which will create
			' a prompt using html and css that looks like the prompt does in BarTender.
			Dim stringReader As TextReader = New StringReader(xml)
			Dim xmlReader As XmlReader = XmlReader.Create(stringReader)

			Dim trans As New XslCompiledTransform()
			trans.Load(xslPath)

			Dim sb As New System.Text.StringBuilder()
			Dim ws As XmlWriterSettings = trans.OutputSettings.Clone()
			Dim xmlWriter As XmlWriter = XmlWriter.Create(sb, ws)

			trans.Transform(xmlReader, xmlWriter)

			' Add the transformed html as a LiteralControl.
			Dim promptString As String = sb.ToString()

			' remove the namespace attribute 
			promptString = promptString.Replace("xmlns:asp=""remove""", "")

			Dim promptControls As Control = Page.ParseControl(promptString)
			Do While promptControls.Controls.Count > 0
			   Dim controlID As String = promptControls.Controls(0).ID
			   If controlID IsNot Nothing Then
				  promptControls.Controls(0).ID = labelFormat.BaseName.Replace(" "c, "_"c) & controlID
			   End If
			   _panelPrompts.Controls.Add(promptControls.Controls(0))
			Loop

			_panelPrompts.Visible = True
		 Else
			_panelPrompts.Visible = False
		 End If
	  Else
		 _panelPrompts.Visible = False
	  End If
   End Sub
   #End Region
End Class

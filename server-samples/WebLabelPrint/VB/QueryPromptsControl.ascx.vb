Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print
Imports Seagull.BarTender.Print.Database

Partial Public Class QueryPromptsControl
	Inherits System.Web.UI.UserControl
   #Region "Private Member Variables"
   Private _labelFormat As LabelFormat ' Label format data object.
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page loads
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  CreateControls(_labelFormat)
   End Sub
   #End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Sets the LabelFormat that will be used to get query prompts.
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 _labelFormat = value
	  End Set
   End Property
   #End Region

   #Region "Public Methods"
   ''' <summary>
   ''' Gets the input query prompt data and sets it on the given LabelFormat.
   ''' </summary>
   ''' <param name="format">The LabelFormat to set query prompt data on.</param>
   Public Sub UpdateFormatData(ByVal labelFormat As LabelFormat)
	  Dim labelFormatFileName As String = labelFormat.BaseName
	  Dim posId As Integer = 1

	  For Each row As TableRow In _tableQueryPrompts.Rows
		 ' Make sure the row has two cells.
		 If row.Cells.Count = 2 Then
			'Query prompt value is in the textbox in the second cell.
			Dim txtBoxControl As TextBox = TryCast(row.Cells(1).Controls(0), TextBox)

			If txtBoxControl IsNot Nothing Then
			   Dim queryPromptName As String = txtBoxControl.ID.Substring(labelFormatFileName.Length)

			   ' Set the query prompt in the cached LabelFormat so they will be set before the format is printed.
			   labelFormat.DatabaseConnections.QueryPrompts.SetQueryPrompt(queryPromptName, txtBoxControl.Text, "", "")
			End If
		 End If
		 posId += 1
	  Next row
   End Sub
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Update all controls on the page with data from the label format object.
   ''' </summary>
   ''' <param name="labelFormat">Label format object.</param>
   Private Sub CreateControls(ByVal labelFormat As LabelFormat)
	  ' Start out with the query prompt panel hidden.
	  _panelQueryPrompts.Visible = False

	  ' Make sure the label format object is good.
	  If _labelFormat IsNot Nothing Then
		 If _labelFormat.DatabaseConnections.QueryPrompts.Count <> 0 Then
			_tableQueryPrompts.Rows.Clear()
			Dim labelFormatFileName As String = labelFormat.BaseName
			Dim posId As Integer = 1

			For Each queryPrompt As QueryPrompt In _labelFormat.DatabaseConnections.QueryPrompts
			   Dim row As New TableRow()
			   Dim nameCell As New TableCell()
			   Dim textCell As New TableCell()

			   ' Add the query prompt user prompt label
			   Dim label As New Label()
			   label.Text = queryPrompt.UserPrompt
			   nameCell.Controls.Add(label)
			   row.Cells.Add(nameCell)

			   ' Add the query prompt value textbox
			   Dim textBox As New TextBox()
			   textBox.ID = labelFormatFileName & queryPrompt.Name
			   textBox.Text = queryPrompt.DefaultReply
			   textCell.Controls.Add(textBox)
			   row.Cells.Add(textCell)

			   _tableQueryPrompts.Rows.Add(row)

			   posId += 1
			Next queryPrompt
			_panelQueryPrompts.Visible = True
		 End If
	  End If
   End Sub
   #End Region

End Class


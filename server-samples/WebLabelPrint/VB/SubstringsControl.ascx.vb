Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Seagull.BarTender.Print

Partial Public Class SubstringsControl
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
   ''' Sets the LabelFormat that will be used to get substrings.
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 _labelFormat = value
	  End Set
   End Property
   #End Region

   #Region "Public Methods"
   ''' <summary>
   ''' Gets the input substring data and sets it on the given LabelFormat.
   ''' </summary>
   ''' <param name="format">The LabelFormat to set substring data on.</param>
   Public Sub UpdateFormatData(ByVal labelFormat As LabelFormat)
	  Dim rowNum As Integer = 0

	  For Each row As TableRow In _tableSubstrings.Rows
		 ' Make sure the row has two cells.
		 If row.Cells.Count = 2 Then
			Dim lblControl As Label = CType(row.FindControl(CreateControlName("Name_", labelFormat.BaseName, rowNum)), Label)
			Dim txtBoxControl As TextBox = CType(row.FindControl(CreateControlName("Value_", labelFormat.BaseName, rowNum)), TextBox)

			If (lblControl IsNot Nothing) AndAlso (txtBoxControl IsNot Nothing) Then
			   ' Set the substring in the cached LabelFormat so they will be set before the format is printed.
			   labelFormat.SubStrings.SetSubString(lblControl.Text, txtBoxControl.Text)
			End If
		 End If
		 rowNum += 1
	  Next row
   End Sub
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Update all controls on the page with data from the label format object.
   ''' </summary>
   ''' <param name="labelFormat">Label format object.</param>
   Private Sub CreateControls(ByVal labelFormat As LabelFormat)
	  ' Start out with the substring panel hidden.
	  _panelSubStrings.Visible = False

	  ' Make sure the label format object is good.
	  If _labelFormat IsNot Nothing Then
		 If _labelFormat.SubStrings.Count <> 0 Then
			_tableSubstrings.Rows.Clear()
			Dim rowNum As Integer = 0

			For Each substring As SubString In _labelFormat.SubStrings
			   Dim row As New TableRow()
			   Dim nameCell As New TableCell()
			   Dim textCell As New TableCell()

			   ' Add the substring name label
			   Dim label As New Label()
			   label.ID = CreateControlName("Name_", labelFormat.BaseName, rowNum)
			   label.Text = substring.Name
			   nameCell.Controls.Add(label)
			   row.Cells.Add(nameCell)

			   ' Add the substring value textbox
			   Dim textBox As New TextBox()
			   textBox.ID = CreateControlName("Value_", labelFormat.BaseName, rowNum)
			   textBox.Text = substring.Value
			   textCell.Controls.Add(textBox)
			   row.Cells.Add(textCell)

			   _tableSubstrings.Rows.Add(row)

			   rowNum += 1
			Next substring
			_panelSubStrings.Visible = True
		 End If
	  End If
   End Sub

   Private Function CreateControlName(ByVal prefix As String, ByVal labelFormatFileName As String, ByVal rowNum As Integer) As String
	  Return prefix & labelFormatFileName & rowNum.ToString()
   End Function
   #End Region

End Class


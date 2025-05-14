Imports Microsoft.VisualBasic
Imports System
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print

Partial Public Class CopyControl
	Inherits System.Web.UI.UserControl
   #Region "Private Member Variables"
   Private _labelFormat As LabelFormat ' Label format data object.
   Private _alert As Alert
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

   ''' <summary>
   ''' The Alert that will notify the user about any problems with the copy values.
   ''' </summary>
   Public Property Alert() As Alert
	  Set(ByVal value As Alert)
		 _alert = value
	  End Set
	  Get
		 Return _alert
	  End Get
   End Property
   #End Region

   #Region "Public Methods"
   ''' <summary>
   ''' Gets the input substring data and sets it on the given LabelFormat.
   ''' </summary>
   ''' <param name="format">The LabelFormat to set substring data on.</param>
   ''' <returns>Whether or not the format data was valid.</returns>
   Public Function UpdateFormatData(ByVal labelFormat As LabelFormat) As Boolean
	  Dim valid As Boolean = True
	  If (labelFormat IsNot Nothing) AndAlso (_tableCopies.Rows.Count <> 0) Then
		 Dim labelFormatFileName As String = labelFormat.BaseName

		 Dim copies As Integer
		 ' Get Identical Copies
		 Dim success As Boolean = GetCopiesValue(_tableCopies.Rows(0), "IdentCopies", labelFormatFileName, copies)
		 If success Then
			' Make sure IdenticalCopiesOfLabel is not datasourced.
			If copies > 0 Then
			   labelFormat.PrintSetup.IdenticalCopiesOfLabel = copies
			End If
		 Else
			_alert.AddMessage("Number Of Identical copies must be a number greater than 0.")
			valid = False
		 End If

		 ' Get Serialized Copies
		 success = GetCopiesValue(_tableCopies.Rows(1), "SerialCopies", labelFormatFileName, copies)
		 If success Then
			' Make sure NumberOfSerializedLabels is not datasourced.
			If copies > 0 Then
			   labelFormat.PrintSetup.NumberOfSerializedLabels = copies
			End If
		 Else
			_alert.AddMessage("Number Of Serialized copies must be a number greater than 0.")
			valid = False
		 End If
	  End If

	  Return valid
   End Function
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Update all controls on the page with data from the label format object.
   ''' </summary>
   ''' <param name="labelFormat">Label format object.</param>
   Private Sub CreateControls(ByVal labelFormat As LabelFormat)
	  ' Start out with the substring panel hidden.
	  _panelCopies.Visible = False

	  ' Make sure the label format object is good.
	  If _labelFormat IsNot Nothing Then
		 Dim labelFormatFileName As String = labelFormat.BaseName
		 CreateCopiesRow("IdentCopies", "Identical Copies:", labelFormat.PrintSetup.IdenticalCopiesOfLabel, labelFormatFileName)
		 CreateCopiesRow("SerialCopies", "Serialized Copies:", labelFormat.PrintSetup.NumberOfSerializedLabels, labelFormatFileName)
		 _panelCopies.Visible = True
	  End If
   End Sub

   ''' <summary>
   ''' Create a new copies table row.
   ''' </summary>
   ''' <param name="rowName">Short row name used to identify the child controls.</param>
   ''' <param name="labelText">Label text for control.</param>
   ''' <param name="copiesValue">Copies value.</param>
   ''' <param name="labelFormatFileName">Label Format file name (short name, not path).</param>
   Private Sub CreateCopiesRow(ByVal rowName As String, ByVal labelText As String, ByVal copiesValue As Integer, ByVal labelFormatFileName As String)
	  ' Create Identical Copies Row
	  Dim row As New TableRow()
	  Dim nameCell As New TableCell()
	  Dim textCell As New TableCell()

	  ' Add the substring name label
	  Dim label As New Label()
	  label.ID = "Name_" & rowName & "_" & labelFormatFileName
	  label.Text = labelText
	  nameCell.Controls.Add(label)
	  row.Cells.Add(nameCell)

	  ' Add the substring value textbox
	  Dim textBox As New TextBox()
	  textBox.ID = "Value_" & rowName & "_" & labelFormatFileName
	  If (copiesValue = 0) Then
		  textBox.Text = "Data Sourced"
	  Else
		  textBox.Text = copiesValue.ToString()
	  End If
	  textBox.Enabled = (copiesValue <> 0)
	  textCell.Controls.Add(textBox)
	  row.Cells.Add(textCell)

	  _tableCopies.Rows.Add(row)
   End Sub

   ''' <summary>
   ''' Get the copies value for the specified row.
   ''' </summary>
   ''' <param name="row">TableRow control for the row.</param>
   ''' <param name="rowName">Short row name used to identify the child controls.</param>
   ''' <param name="labelFormatFileName">Label Format file name (short name, not path).</param>
   ''' <param name="copies">The copies value. Returns 0 if invalid or datasourced.</param>
   ''' <returns>True if the copies are a valid value, or false if invalid.</returns>
   Private Function GetCopiesValue(ByVal row As TableRow, ByVal rowName As String, ByVal labelFormatFileName As String, <System.Runtime.InteropServices.Out()> ByRef copies As Integer) As Boolean
	  Dim txtBoxControl As TextBox = CType(row.FindControl("Value_" & rowName & "_" & labelFormatFileName), TextBox)
	  Dim valid As Boolean = False

	  copies = 0
	  If txtBoxControl IsNot Nothing Then
		 Dim parseCopies As Integer = 1
		 ' The control will be disabled if copies are datasourced. Don't try to parse the text.
		 If txtBoxControl.Enabled = True Then
			Dim parseOK As Boolean = Integer.TryParse(txtBoxControl.Text, parseCopies)
			If parseOK Then
			   If parseCopies > 0 Then
				  copies = parseCopies
				  valid = True
			   End If
			End If
		 Else
			valid = True
		 End If
	  End If
	  Return valid
   End Function

   #End Region

End Class


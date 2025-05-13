Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Reflection
Imports System.Text
Imports System.Windows.Forms
Imports Seagull.BarTender.Print
Imports System.IO 'For MemoryStream
Imports System.Xml 'For XMLReader and XMLWriter
Imports System.Xml.Xsl 'For XslCompiledTransform

Namespace XMLScripter
   ' XML Scripter Sample
   ' This sample allows the user to enter/load/save XML scripts,
   ' have BarTender run the XML script, and view the XMLResponse.
   ' 
   ' This sample is intended to show how to:
   '  -Run XMLScript
   '  -Start and Stop the BarTender engine.
   '
   Partial Public Class XMLScripter
	   Inherits Form
	  #Region "Fields"
	  ' Common strings.
	  Private Const appName As String = "XML Scripter"
	  Private Const fileFilter As String = "XML Files|*.xml"
	  Private Const defaultExtension As String = "xml"
	  Private engine As Engine = Nothing
	  Private xmlResponse As String
	  #End Region

	  #Region "Constructor"
	  ''' <summary>
	  ''' Constructor
	  ''' </summary>
	  Public Sub New()
		 InitializeComponent()
	  End Sub
	  #End Region

	  #Region "Form Event Handlers"
	  ''' <summary>
	  ''' Called when the user opens the program.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub XMLScripter_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		 ' Create and start a new BarTender Print Engine.
		 Try
			engine = New Engine(True)
		 Catch exception As PrintEngineException
			' If the engine is unable to start, a PrintEngineException will be thrown.
			MessageBox.Show(Me, exception.Message, appName)
			Me.Close() ' Close this app. We cannot run without connection to an engine.
			Return
		 End Try

		 ' This allows us to copy the response XML.
		 webXMLResponse.WebBrowserShortcutsEnabled = True

		 ' Change the default XML Script to reference the installed format.
		 Dim labelFilename As String = Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) & "\..\..\ISBN.btw")
		 txtXMLScript.Text = txtXMLScript.Text.Replace("format file name goes here", labelFilename)
	  End Sub

	  ''' <summary>
	  ''' Called when the user closes the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub XMLScripter_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 ' Quit the BarTender Print Engine, but do not save changes to any open formats.
		 If engine IsNot Nothing Then
			engine.Stop(SaveOptions.DoNotSaveChanges)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Runs the XMLScript via a background thread when the button is clicked.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnRunXMLScript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRunXMLScript.Click
		 ' This will clear the response window.
		 btnRunXMLScript.Enabled = False
		 btnCopyResponseToClipboard.Enabled = False
		 picUpdating.Visible = True
		 webXMLResponse.DocumentText = "<HTML></HTML>"
		 webXMLResponse.Refresh()

		 ' Run the xml script in a separate thread so we call still act on the user interface.
		 backgroundWorker.RunWorkerAsync(txtXMLScript.Text)
	  End Sub

	  ''' <summary>
	  ''' Load an XML script.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnLoadXMLScript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnLoadXMLScript.Click
		 Dim openFileDialog As New OpenFileDialog()
		 openFileDialog.Filter = fileFilter
		 openFileDialog.DefaultExt = defaultExtension
		 openFileDialog.Title = "Select an XML Script file"
		 If openFileDialog.ShowDialog(Me) = System.Windows.Forms.DialogResult.OK Then
			Using streamReader As New StreamReader(openFileDialog.FileName)
			   txtXMLScript.Text = streamReader.ReadToEnd()
			End Using
		 End If
	  End Sub

	  ''' <summary>
	  ''' Save the XML Script to a file.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnSaveXMLScript_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSaveXMLScript.Click
		 Dim saveFileDialog As New SaveFileDialog()
		 saveFileDialog.Filter = fileFilter
		 saveFileDialog.DefaultExt = defaultExtension
		 If saveFileDialog.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
			Using streamWriter As New StreamWriter(saveFileDialog.FileName)
			   streamWriter.Write(txtXMLScript.Text)
			End Using
		 End If
	  End Sub

	  ''' <summary>
	  ''' Copy the resulting XML Script to the clipboard.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnCopyResponseToClipboard_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCopyResponseToClipboard.Click
		 Clipboard.SetData(DataFormats.Text, xmlResponse)
	  End Sub
	  #End Region

	  #Region "Script Executer Background Thread"
	  ''' <summary>
	  ''' Runs the XML script in a separate thread
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles backgroundWorker.DoWork
		 Dim xmlString As String = CStr(e.Argument)

		 ' Run the XMLScript that was entered in the text box and get the XMLResponse
		 xmlResponse = engine.XMLScript(xmlString, XMLSourceType.ScriptString)

		 ' IE uses a built in XSL transform to display XML. The .NET web browser doesn't
		 ' do this automatically so we need to transform it ourselves.

		 ' Set up the XmlReader to read from the xmlResponse.
		 Dim xmlStream As MemoryStream = New System.IO.MemoryStream(Encoding.Unicode.GetBytes(xmlResponse))
		 Dim xmlReader As XmlReader = System.Xml.XmlReader.Create(xmlStream)

		 ' Set up the XslCompiledTransform to use the xsl-transform.
		 Dim transform As New XslCompiledTransform()
		 Dim AppPath As String = Application.ExecutablePath
		 Dim fileInfo As New FileInfo(AppPath)
		 transform.Load(fileInfo.DirectoryName & "\defaultss.xsl")

		 ' Set up the xmlWriter that the transform will write to.
		 Dim stringBuilder As New StringBuilder()
		 Dim writerSettings As XmlWriterSettings = transform.OutputSettings.Clone()
		 writerSettings.CheckCharacters = False
		 Dim xmlWriter As XmlWriter = XmlWriter.Create(stringBuilder, writerSettings)

		 ' Do the transform and display it in the web browser control.
		 transform.Transform(xmlReader, xmlWriter)

		 backgroundWorker.ReportProgress(100)

		 webXMLResponse.DocumentText = stringBuilder.ToString()
		 webXMLResponse.Refresh()
	  End Sub

	  ''' <summary>
	  ''' When the work is done we update the control to show the xml response.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub backgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles backgroundWorker.ProgressChanged
		 If e.ProgressPercentage = 100 Then
			btnRunXMLScript.Enabled = True
			btnCopyResponseToClipboard.Enabled = True
			picUpdating.Visible = False
		 End If
	  End Sub
	  #End Region
   End Class
End Namespace
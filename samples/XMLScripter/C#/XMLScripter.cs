using System;
using System.ComponentModel;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Seagull.BarTender.Print;
using System.IO; //For MemoryStream
using System.Xml; //For XMLReader and XMLWriter
using System.Xml.Xsl; //For XslCompiledTransform

namespace XMLScripter
{
   // XML Scripter Sample
   // This sample allows the user to enter/load/save XML scripts,
   // have BarTender run the XML script, and view the XMLResponse.
   // 
   // This sample is intended to show how to:
   //  -Run XMLScript
   //  -Start and Stop the BarTender engine.
   //
   public partial class XMLScripter : Form
   {
      #region Fields
      // Common strings.
      private const string appName = "XML Scripter";
      private const string fileFilter = "XML Files|*.xml";
      private const string defaultExtension = "xml";
      private Engine engine = null;
      private string xmlResponse;
      #endregion

      #region Constructor
      /// <summary>
      /// Constructor
      /// </summary>
      public XMLScripter()
      {
         InitializeComponent();
      }
      #endregion

      #region Form Event Handlers
      /// <summary>
      /// Called when the user opens the program.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void XMLScripter_Load(object sender, EventArgs e)
      {
         // Create and start a new BarTender Print Engine.
         try
         {
            engine = new Engine(true);
         }
         catch (PrintEngineException exception)
         {
            // If the engine is unable to start, a PrintEngineException will be thrown.
            MessageBox.Show(this, exception.Message, appName);
            this.Close(); // Close this app. We cannot run without connection to an engine.
            return;
         }

         // This allows us to copy the response XML.
         webXMLResponse.WebBrowserShortcutsEnabled = true;

         // Change the default XML Script to reference the installed format.
         string labelFilename = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\..\..\ISBN.btw");
         txtXMLScript.Text = txtXMLScript.Text.Replace("format file name goes here", labelFilename);
      }

      /// <summary>
      /// Called when the user closes the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void XMLScripter_FormClosed(object sender, FormClosedEventArgs e)
      {
         // Quit the BarTender Print Engine, but do not save changes to any open formats.
         if (engine != null)
            engine.Stop(SaveOptions.DoNotSaveChanges);
      }

      /// <summary>
      /// Runs the XMLScript via a background thread when the button is clicked.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnRunXMLScript_Click(object sender, EventArgs e)
      {
         // This will clear the response window.
         btnRunXMLScript.Enabled = false;
         btnCopyResponseToClipboard.Enabled = false;
         picUpdating.Visible = true;
         webXMLResponse.DocumentText = "<HTML></HTML>";
         webXMLResponse.Refresh();

         // Run the xml script in a separate thread so we call still act on the user interface.
         backgroundWorker.RunWorkerAsync(txtXMLScript.Text);
      }

      /// <summary>
      /// Load an XML script.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnLoadXMLScript_Click(object sender, EventArgs e)
      {
         OpenFileDialog openFileDialog = new OpenFileDialog();
         openFileDialog.Filter = fileFilter;
         openFileDialog.DefaultExt = defaultExtension;
         openFileDialog.Title = "Select an XML Script file";
         if (openFileDialog.ShowDialog(this) == DialogResult.OK)
         {
            using (StreamReader streamReader = new StreamReader(openFileDialog.FileName)) 
            {
               txtXMLScript.Text = streamReader.ReadToEnd();
            }
         }
      }

      /// <summary>
      /// Save the XML Script to a file.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnSaveXMLScript_Click(object sender, EventArgs e)
      {
         SaveFileDialog saveFileDialog = new SaveFileDialog();
         saveFileDialog.Filter = fileFilter;
         saveFileDialog.DefaultExt = defaultExtension;
         if (saveFileDialog.ShowDialog() == DialogResult.OK)
         {
            using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
            {
               streamWriter.Write(txtXMLScript.Text);
            }
         }
      }

      /// <summary>
      /// Copy the resulting XML Script to the clipboard.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnCopyResponseToClipboard_Click(object sender, EventArgs e)
      {
         Clipboard.SetData(DataFormats.Text, xmlResponse);
      }
      #endregion

      #region Script Executer Background Thread
      /// <summary>
      /// Runs the XML script in a separate thread
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         string xmlString = (string)e.Argument;

         // Run the XMLScript that was entered in the text box and get the XMLResponse
         xmlResponse = engine.XMLScript(xmlString, XMLSourceType.ScriptString);
         
         // IE uses a built in XSL transform to display XML. The .NET web browser doesn't
         // do this automatically so we need to transform it ourselves.

         // Set up the XmlReader to read from the xmlResponse.
         MemoryStream xmlStream = new System.IO.MemoryStream(Encoding.Unicode.GetBytes(xmlResponse));
         XmlReader xmlReader = System.Xml.XmlReader.Create(xmlStream);

         // Set up the XslCompiledTransform to use the xsl-transform.
         XslCompiledTransform transform = new XslCompiledTransform();
         string AppPath = Application.ExecutablePath;
         FileInfo fileInfo = new FileInfo(AppPath);
         transform.Load(fileInfo.DirectoryName + @"\defaultss.xsl");

         // Set up the xmlWriter that the transform will write to.
         StringBuilder stringBuilder = new StringBuilder();
         XmlWriterSettings writerSettings = transform.OutputSettings.Clone();
         writerSettings.CheckCharacters = false;
         XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, writerSettings);

         // Do the transform and display it in the web browser control.
         transform.Transform(xmlReader, xmlWriter);

         backgroundWorker.ReportProgress(100);

         webXMLResponse.DocumentText = stringBuilder.ToString();
         webXMLResponse.Refresh();
      }

      /// <summary>
      /// When the work is done we update the control to show the xml response.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         if (e.ProgressPercentage == 100)
         {
            btnRunXMLScript.Enabled = true;
            btnCopyResponseToClipboard.Enabled = true;
            picUpdating.Visible = false;
         }
      }
      #endregion
   }
}
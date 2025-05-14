using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Xml.Xsl;

using Seagull.BarTender.Print;

public partial class PromptControl : System.Web.UI.UserControl
{
   #region Private Member Variables
   private LabelFormat _labelFormat;     // Label format data object.
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      CreateControls(_labelFormat);
   }
   #endregion

   #region Public Properties
   /// <summary>
   /// Sets the LabelFormat that will be used to get prompts.
   /// </summary>
   public LabelFormat LabelFormatObject
   {
      set
      {
         _labelFormat = value;
      }
   }
   #endregion

   #region Public Methods
   /// <summary>
   /// Gets the input prompt data and sets it on the given LabelFormat.
   /// </summary>
   /// <param name="format">The LabelFormat to set prompt data on.</param>
   public void UpdateFormatData(LabelFormat format)
   {
      foreach (Control control in _panelPrompts.Controls)
      {
         string promptName = "";

         if (control.ID != null)
         {
            promptName = control.ID.Replace('_', ' ');
            promptName = promptName.Substring(format.BaseName.Length);
         }
         if (control is TextBox)
         {
            TextBox textBox = control as TextBox;

            format.Prompts.SetPrompt(promptName, textBox.Text);
         }
         else if (control is ListControl)
         {
            ListControl listControl = control as ListControl;

            format.Prompts.SetPrompt(promptName, listControl.SelectedValue);
         }
      }
   }
   #endregion
   
   #region Support Methods
   /// <summary>
   /// Update controls on the page using the label format object data.
   /// </summary>
   /// <param name="labelFormat">Label format object.</param>
   void CreateControls(LabelFormat labelFormat)
   {
      if (labelFormat != null)
      {
         string xml = labelFormat.Prompts.LayoutXML;

         // Show the control if there is an xml layout.
         if (xml != "")
         {
            string xslPath = Server.MapPath("XSL\\Prompt.xsl");

            // Transform the prompt layout XML using an xsl transform which will create
            // a prompt using html and css that looks like the prompt does in BarTender.
            TextReader stringReader = new StringReader(xml);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            XslCompiledTransform trans = new XslCompiledTransform();
            trans.Load(xslPath);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            XmlWriterSettings ws = trans.OutputSettings.Clone();
            XmlWriter xmlWriter = XmlWriter.Create(sb, ws);

            trans.Transform(xmlReader, xmlWriter);

            // Add the transformed html as a LiteralControl.
            string promptString = sb.ToString();

            // remove the namespace attribute 
            promptString = promptString.Replace("xmlns:asp=\"remove\"", ""); 

            Control promptControls = Page.ParseControl(promptString);
            while (promptControls.Controls.Count > 0)
            {
               string controlID = promptControls.Controls[0].ID;
               if(controlID != null)
               {
                  promptControls.Controls[0].ID = labelFormat.BaseName.Replace(' ', '_') + controlID;
               }
               _panelPrompts.Controls.Add(promptControls.Controls[0]);
            }
            
            _panelPrompts.Visible = true;
         }
         else
         {
            _panelPrompts.Visible = false;
         }
      }
      else
      {
         _panelPrompts.Visible = false;
      }
   }
   #endregion
}

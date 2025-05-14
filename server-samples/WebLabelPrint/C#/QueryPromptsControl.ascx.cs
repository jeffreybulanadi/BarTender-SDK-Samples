using System;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;
using Seagull.BarTender.Print.Database;

public partial class QueryPromptsControl : System.Web.UI.UserControl
{
   #region Private Member Variables
   private LabelFormat _labelFormat;     // Label format data object.
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page loads
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      CreateControls(_labelFormat);
   }
   #endregion

   #region Public Properties
   /// <summary>
   /// Sets the LabelFormat that will be used to get query prompts.
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
   /// Gets the input query prompt data and sets it on the given LabelFormat.
   /// </summary>
   /// <param name="format">The LabelFormat to set query prompt data on.</param>
   public void UpdateFormatData(LabelFormat labelFormat)
   {
      string labelFormatFileName = labelFormat.BaseName;
      int posId = 1;

      foreach (TableRow row in _tableQueryPrompts.Rows)
      {
         // Make sure the row has two cells.
         if (row.Cells.Count == 2)
         {
            //Query prompt value is in the textbox in the second cell.
            TextBox txtBoxControl = row.Cells[1].Controls[0] as TextBox;

            if (txtBoxControl != null)
            {
               string queryPromptName = txtBoxControl.ID.Substring(labelFormatFileName.Length);

               // Set the query prompt in the cached LabelFormat so they will be set before the format is printed.
               labelFormat.DatabaseConnections.QueryPrompts.SetQueryPrompt(queryPromptName, txtBoxControl.Text, "", "");
            }
         }
         posId++;
      }
   }
   #endregion

   #region Support Methods
   /// <summary>
   /// Update all controls on the page with data from the label format object.
   /// </summary>
   /// <param name="labelFormat">Label format object.</param>
   void CreateControls(LabelFormat labelFormat)
   {
      // Start out with the query prompt panel hidden.
      _panelQueryPrompts.Visible = false;
      
      // Make sure the label format object is good.
      if (_labelFormat != null)
      {
         if (_labelFormat.DatabaseConnections.QueryPrompts.Count != 0)
         {
            _tableQueryPrompts.Rows.Clear();
            string labelFormatFileName = labelFormat.BaseName;
            int    posId = 1;
            
            foreach (QueryPrompt queryPrompt in _labelFormat.DatabaseConnections.QueryPrompts)
            {
               TableRow row = new TableRow();
               TableCell nameCell = new TableCell();
               TableCell textCell = new TableCell();

               // Add the query prompt user prompt label
               Label label = new Label();
               label.Text = queryPrompt.UserPrompt;
               nameCell.Controls.Add(label);
               row.Cells.Add(nameCell);

               // Add the query prompt value textbox
               TextBox textBox = new TextBox();
               textBox.ID = labelFormatFileName + queryPrompt.Name;
               textBox.Text = queryPrompt.DefaultReply;
               textCell.Controls.Add(textBox);
               row.Cells.Add(textCell);

               _tableQueryPrompts.Rows.Add(row);
               
               posId++;
            }
            _panelQueryPrompts.Visible = true;
         }
      }
   }
   #endregion
   
}


using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using Seagull.BarTender.Print;

public partial class SubstringsControl : System.Web.UI.UserControl
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
   /// Sets the LabelFormat that will be used to get substrings.
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
   /// Gets the input substring data and sets it on the given LabelFormat.
   /// </summary>
   /// <param name="format">The LabelFormat to set substring data on.</param>
   public void UpdateFormatData(LabelFormat labelFormat)
   {
      int rowNum = 0;

      foreach (TableRow row in _tableSubstrings.Rows)
      {
         // Make sure the row has two cells.
         if (row.Cells.Count == 2)
         {
            Label lblControl = (Label)row.FindControl(CreateControlName("Name_", labelFormat.BaseName, rowNum));
            TextBox txtBoxControl = (TextBox)row.FindControl(CreateControlName("Value_", labelFormat.BaseName, rowNum));
            
            if ((lblControl != null) && (txtBoxControl != null))
            {
               // Set the substring in the cached LabelFormat so they will be set before the format is printed.
               labelFormat.SubStrings.SetSubString(lblControl.Text, txtBoxControl.Text);
            }
         }
         rowNum++;
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
      // Start out with the substring panel hidden.
      _panelSubStrings.Visible = false;
      
      // Make sure the label format object is good.
      if (_labelFormat != null)
      {
         if (_labelFormat.SubStrings.Count != 0)
         {
            _tableSubstrings.Rows.Clear();
            int    rowNum = 0;
            
            foreach (SubString substring in _labelFormat.SubStrings)
            {
               TableRow row = new TableRow();
               TableCell nameCell = new TableCell();
               TableCell textCell = new TableCell();

               // Add the substring name label
               Label label = new Label();
               label.ID = CreateControlName("Name_", labelFormat.BaseName, rowNum);
               label.Text = substring.Name;
               nameCell.Controls.Add(label);
               row.Cells.Add(nameCell);

               // Add the substring value textbox
               TextBox textBox = new TextBox();
               textBox.ID = CreateControlName("Value_", labelFormat.BaseName, rowNum);
               textBox.Text = substring.Value;
               textCell.Controls.Add(textBox);
               row.Cells.Add(textCell);

               _tableSubstrings.Rows.Add(row);

               rowNum++;
            }
            _panelSubStrings.Visible = true;
         }
      }
   }

   string CreateControlName(string prefix, string labelFormatFileName, int rowNum)
   {
      return prefix + labelFormatFileName + rowNum.ToString();
   }
   #endregion
   
}


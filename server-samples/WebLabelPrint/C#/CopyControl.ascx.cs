using System;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;

public partial class CopyControl : System.Web.UI.UserControl
{
   #region Private Member Variables
   private LabelFormat _labelFormat;        // Label format data object.
   private Alert _alert;
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

   /// <summary>
   /// The Alert that will notify the user about any problems with the copy values.
   /// </summary>
   public Alert Alert
   {
      set
      {
         _alert = value;
      }
      get
      {
         return _alert;
      }
   }
   #endregion

   #region Public Methods
   /// <summary>
   /// Gets the input substring data and sets it on the given LabelFormat.
   /// </summary>
   /// <param name="format">The LabelFormat to set substring data on.</param>
   /// <returns>Whether or not the format data was valid.</returns>
   public bool UpdateFormatData(LabelFormat labelFormat)
   {
      bool valid = true;
      if ((labelFormat != null) && (_tableCopies.Rows.Count != 0))
      {
         string labelFormatFileName = labelFormat.BaseName;

         int copies;
         // Get Identical Copies
         bool success = GetCopiesValue(_tableCopies.Rows[0], "IdentCopies", labelFormatFileName, out copies);
         if (success)
         {
            // Make sure IdenticalCopiesOfLabel is not datasourced.
            if (copies > 0)
               labelFormat.PrintSetup.IdenticalCopiesOfLabel = copies;
         }
         else
         {
            _alert.AddMessage("Number Of Identical copies must be a number greater than 0.");
            valid = false;
         }
            
         // Get Serialized Copies
         success = GetCopiesValue(_tableCopies.Rows[1], "SerialCopies", labelFormatFileName, out copies);
         if (success)
         {
            // Make sure NumberOfSerializedLabels is not datasourced.
            if (copies > 0)
               labelFormat.PrintSetup.NumberOfSerializedLabels = copies;
         }
         else
         {
            _alert.AddMessage("Number Of Serialized copies must be a number greater than 0.");
            valid = false;
         }
      }

      return valid;
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
      _panelCopies.Visible = false;

      // Make sure the label format object is good.
      if (_labelFormat != null)
      {
         string labelFormatFileName = labelFormat.BaseName;
         CreateCopiesRow("IdentCopies", "Identical Copies:", labelFormat.PrintSetup.IdenticalCopiesOfLabel, labelFormatFileName);
         CreateCopiesRow("SerialCopies", "Serialized Copies:", labelFormat.PrintSetup.NumberOfSerializedLabels, labelFormatFileName);
         _panelCopies.Visible = true;
      }
   }

   /// <summary>
   /// Create a new copies table row.
   /// </summary>
   /// <param name="rowName">Short row name used to identify the child controls.</param>
   /// <param name="labelText">Label text for control.</param>
   /// <param name="copiesValue">Copies value.</param>
   /// <param name="labelFormatFileName">Label Format file name (short name, not path).</param>
   void CreateCopiesRow(string rowName, string labelText, int copiesValue, string labelFormatFileName)
   {
      // Create Identical Copies Row
      TableRow row = new TableRow();
      TableCell nameCell = new TableCell();
      TableCell textCell = new TableCell();

      // Add the substring name label
      Label label = new Label();
      label.ID = "Name_" + rowName + "_" + labelFormatFileName;
      label.Text = labelText;
      nameCell.Controls.Add(label);
      row.Cells.Add(nameCell);

      // Add the substring value textbox
      TextBox textBox = new TextBox();
      textBox.ID = "Value_" + rowName + "_" + labelFormatFileName;
      textBox.Text = (copiesValue == 0) ? "Data Sourced" : copiesValue.ToString();
      textBox.Enabled = (copiesValue != 0);
      textCell.Controls.Add(textBox);
      row.Cells.Add(textCell);

      _tableCopies.Rows.Add(row);
   }

   /// <summary>
   /// Get the copies value for the specified row.
   /// </summary>
   /// <param name="row">TableRow control for the row.</param>
   /// <param name="rowName">Short row name used to identify the child controls.</param>
   /// <param name="labelFormatFileName">Label Format file name (short name, not path).</param>
   /// <param name="copies">The copies value. Returns 0 if invalid or datasourced.</param>
   /// <returns>True if the copies are a valid value, or false if invalid.</returns>
   bool GetCopiesValue(TableRow row, string rowName, string labelFormatFileName, out int copies)
   {
      TextBox txtBoxControl = (TextBox)row.FindControl("Value_" + rowName + "_" + labelFormatFileName);
      bool valid = false;

      copies = 0;
      if (txtBoxControl != null)
      {
         int parseCopies = 1;
         // The control will be disabled if copies are datasourced. Don't try to parse the text.
         if (txtBoxControl.Enabled == true)
         {
            bool parseOK = int.TryParse(txtBoxControl.Text, out parseCopies);
            if (parseOK)
            {
               if (parseCopies > 0)
               {
                  copies = parseCopies;
                  valid = true;
               }
            }
         }
         else
         {
            valid = true;
         }
      }
      return valid;
   }
   
   #endregion

}


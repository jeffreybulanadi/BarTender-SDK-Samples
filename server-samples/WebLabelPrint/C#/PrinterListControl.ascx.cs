using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;

public partial class PrinterList : System.Web.UI.UserControl
{

   #region Private Member Variables

   private string _lastPrinterName             = "";       // Printer name from last postback
   private string _lastPrintType               = "";       // Print type from last postback (client or server)
   private string _lastCompatibleServerPrinter = "";       // Printer name of last compatible server printer (client only)
   private bool   _showServerPrinters          = false;    // Show server printers
   private bool   _showClientPrinters          = false;    // Show client printers
   private Alert  _alert;                                  // Alert control used for displaying error, warning, or informational messages.
   private Button _printButton;                            // Print button control reference
   private Button _previewButton;                          // Print Preview button control reference
   private LabelFormat  _labelFormat;                      // Label format data object.

   #endregion

   #region Web Methods

   /// <summary>
   /// Called when the page loads
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Fill the printer list
      Printers printers = new Printers();

      string serverPrinters = "";
      string serverDrivers = "";

      _listPrinters.Items.Clear();

      foreach (Printer printer in printers)
      {
         if (_showServerPrinters)
         {
            ListItem listItem = new ListItem(printer.PrinterName + " [On Server]", printer.PrinterName + ",server");
            _listPrinters.Items.Add(listItem);
         }

         if (_showClientPrinters)
         {

            // Create comma delimited lists to pass to javascript so the client javascript can match client printers
            serverDrivers += printer.PrinterModel + ",";
            serverPrinters += printer.PrinterName + ",";
         }
      }
      
      string selectedPrinter = "";
      selectedPrinter = GetLastSelectedPrinter();

      if (string.IsNullOrEmpty(selectedPrinter))
      {
         if (printers.Default != null)
         {
            selectedPrinter = GetPrinterNameFromLabelFormat(printers.Default.PrinterName);
         }
      }

      if (_showClientPrinters)
      {
         RegisterClientJavascript(serverPrinters,serverDrivers,selectedPrinter);
      }
      else
      {
         _listPrinters.SelectedValue = selectedPrinter;
      }

      // If only showing server printers, and there are none, then disable the control.
      // If showing client printers don't disable here because the javascript to add them hasn't run yet.
      if (_listPrinters.Items.Count == 0 && _showServerPrinters && !_showClientPrinters)
      {
         _listPrinters.Items.Add("No Printers Installed.");
         _listPrinters.Enabled = false;
         _previewButton.Enabled = false;
         _printButton.Enabled = false;
         _alert.AddMessage("No printers are installed. <br /><br />Printer drivers must be installed in order to print.<br/>");
      }

   }

   #endregion

   #region Public Properties

   public bool Enabled
   {
      get
      {
         return _listPrinters.Enabled;
      }
      set
      {
         _listPrinters.Enabled = value;
      }
   }

   public string LastPrinterName
   {
      get
      {
         return _lastPrinterName;
      }
   }

   public string LastPrintType
   {
      get
      {
         return _lastPrintType;
      }
   }

   public string LastCompatibleServerPrinter
   {
      get
      {
         return _lastCompatibleServerPrinter;
      }
   }

   public bool ShowServerPrinters
   {
      get
      {
         return _showServerPrinters;
      }
      set
      {
         _showServerPrinters = value;
      }
   }

   public bool ShowClientPrinters
   {
      get
      {
         return _showClientPrinters;
      }
      set
      {
         _showClientPrinters = value;
      }
   }

   public string ListClientID
   {
      get
      {
         return _listPrinters.ClientID;
      }
   }

   public string ListUniqueID
   {
      get
      {
         return _listPrinters.UniqueID;
      }
   }

   public string PrintLicenseClientID
   {
      get
      {
         return _hiddenPrintLicense.ClientID;
      }
   }

   public string PrintLicenseUniqueID
   {
      get
      {
         return _hiddenPrintLicense.UniqueID;
      }
   }

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

   public Button PrintButton
   {
      set
      {
         _printButton = value;
      }
      get
      {
         return _printButton;
      }
   }

   public Button PreviewButton
   {
      set
      {
         _previewButton = value;
      }
      get
      {
         return _previewButton;
      }
   }

   /// <summary>
   /// Sets the LabelFormat that will be used to get the default printer
   /// </summary>
   public LabelFormat LabelFormatObject
   {
      set
      {
         _labelFormat = value;
      }
   }

   #endregion

   #region Support Methods

   /// <summary>
   /// Gets the printer that was last selected by the user on this page
   /// </summary>
   /// <returns>The last selected printer or an empty string if there wasn't one.</returns>
   string GetLastSelectedPrinter()
   {
      string lastPrinter = Request.Form.Get(ListUniqueID);
      string selectedPrinter = "";
      if (!string.IsNullOrEmpty(lastPrinter))
      {
         string[] printerInfo = lastPrinter.Split(',');
         _lastPrinterName = printerInfo[0];
         _lastPrintType = printerInfo[1];
         if (printerInfo.Length > 2)
         {
            _lastCompatibleServerPrinter = printerInfo[2];
         }
         selectedPrinter = lastPrinter;
      }
      return selectedPrinter;
   }

   /// <summary>
   /// Gets the format's default printer.
   /// </summary>
   /// <param name="machineDefaultPrinter">The name of the default printer.</param>
   /// <returns>The format's default printer, or an empty string if using the windows default.</returns>
   string GetPrinterNameFromLabelFormat(string machineDefaultPrinter)
   {
      string selectedPrinter = "";
      if (_showServerPrinters)
      {
         // Select the format's default printer or the computer's default printer if applicable
         if (_labelFormat == null || string.IsNullOrEmpty(_labelFormat.PrintSetup.PrinterName))
         {
            selectedPrinter = machineDefaultPrinter + ",server";
         }
         else
         {
            selectedPrinter = _labelFormat.PrintSetup.PrinterName + ",server";
         }
      }
      if (!_showServerPrinters && _showClientPrinters)
      {
         if (_labelFormat == null || string.IsNullOrEmpty(_labelFormat.PrintSetup.PrinterName))
         {
            selectedPrinter = "";
         }
         else
         {
            selectedPrinter = _labelFormat.PrintSetup.PrinterName;
         }
      }
      return selectedPrinter;
   }

   /// <summary>
   /// Registers the client javascript that gets the list of matching client printers.
   /// </summary>
   /// <param name="serverPrinters">The list of server printer names.</param>
   /// <param name="serverDrivers">The list of server driver names.</param>
   /// <param name="selectedPrinter">The selected printer.</param>
   void RegisterClientJavascript(string serverPrinters, string serverDrivers, string selectedPrinter)
   {
      // Escape special characters so the javascript is evaluated correctly.
      serverPrinters = JavaScriptSupport.EscapeSpecialCharacters(serverPrinters);
      serverDrivers = JavaScriptSupport.EscapeSpecialCharacters(serverDrivers);
      string lastPrinter = JavaScriptSupport.EscapeSpecialCharacters(_lastPrinterName);
      string printerNameFromLabelFormat = JavaScriptSupport.EscapeSpecialCharacters(selectedPrinter);

      // Register client script to fill in client printers
      Page.ClientScript.RegisterStartupScript(this.GetType(), "AddClientPrintersToList",
                       string.Format("AddClientPrintersToList('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');",
                                               serverPrinters,
                                               serverDrivers,
                                               lastPrinter,
                                               printerNameFromLabelFormat,
                                               _listPrinters.ClientID,
                                               _alert.MessageClientID,
                                               _alert.PanelClientID,
                                               (_printButton == null ? null : _printButton.ClientID),
                                               (_previewButton == null ? null : _previewButton.ClientID)), true);
   }

   #endregion
}

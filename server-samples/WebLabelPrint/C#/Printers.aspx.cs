using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;

public partial class ServerPrinters : System.Web.UI.Page
{
   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Show the current menu selection by highlighting the selected menu link's background.
      ((HyperLink)Master.FindControl("_printers")).CssClass = "MenuLinkSelected";
      
      // Clear any error messages
      _alert.Message = "";

      // Display server printers
      Printers printers = new Printers();

      if (printers.Count > 0)
      {
         DataSet dataset = new DataSet();

         string sPrinterXML = printers.XML;

         System.IO.StringReader stringReader = new System.IO.StringReader(sPrinterXML);
         dataset.ReadXml(stringReader);

         _gridServerPrinters.DataSource = dataset;

         _gridServerPrinters.DataBind();
      }
      else
      {
         //No printers, so display an error message.
         _gridServerPrinters.Visible = false;
         _noPrintersErrorAlert.Visible = true;
      }

      // Display client printers
      // Get the list of printers in comma delimited form
      string serverPrinters = string.Empty;
      string serverDrivers = string.Empty;

      foreach (Printer printer in printers)
      {
         serverPrinters += printer.PrinterName + ",";
         serverDrivers += printer.PrinterModel + ",";
      }

      serverPrinters = JavaScriptSupport.EscapeSpecialCharacters(serverPrinters);
      serverDrivers = JavaScriptSupport.EscapeSpecialCharacters(serverDrivers);

      _noClientPrintersErrorAlert.ShowAlertPanel(false);
      _noMatchingClientPrintersErrorAlert.ShowAlertPanel(false);

      // Register client script to fill in client printers
      Page.ClientScript.RegisterStartupScript(this.GetType(), "DisplayClientPrinters",
                       string.Format("DisplayClientPrinters('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}');",
                                               serverPrinters,
                                               serverDrivers,
                                               _tableClientPrinters.ClientID,
                                               _tableAvailableClientPrinters.ClientID,
                                               _panelClientPrinting.ClientID,
                                               _alert.MessageClientID,
                                               _alert.PanelClientID,
                                               _noClientPrintersErrorAlert.PanelClientID,
                                               _noMatchingClientPrintersErrorAlert.PanelClientID), true);
   }
   #endregion
}

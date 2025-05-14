using System;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;

public partial class Print : System.Web.UI.Page
{
   #region Web Methods

   protected void Page_Init(object sender, EventArgs e)
   {
      _controlPrint.OnPrintPreview += new EventHandler(ControlPrint_OnPrintPreview);
      _controlPrintPreview.OnBackEvent += new EventHandler(ControlPrintPreview_OnBackEvent);
      _controlPrintPreview.OnPrintEvent += new EventHandler(ControlPrintPreview_OnPrintEvent);
   }

   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      string printType = Request.QueryString.Get("print");
      if (printType!= null && printType == "client")
      {
          ((HyperLink)Master.FindControl("_printToClient")).CssClass = "MenuLinkSelected";
          _controlPrintPreview.PrintButtonClickJavaScript = _controlPrint.PrintToFileLicenseJavaScript;
      }
      else
      {
          ((HyperLink)Master.FindControl("_printToServer")).CssClass = "MenuLinkSelected";
      }

      if (!Page.IsPostBack)
      {
         ShowPrintPreview(false);
      }
   }
   #endregion

   #region Web Callbacks
   /// <summary>
   /// Called when the Print Preview button is selected in the Print control.
   /// </summary>
   protected void ControlPrint_OnPrintPreview(object sender, EventArgs e)
   {
      LabelFormat labelFormat = _controlPrint.LabelFormatObject;
      if (labelFormat != null)
      {
         _controlPrintPreview.LabelFormatObject = labelFormat;
         ShowPrintPreview(true);
      }
   }

   /// <summary>
   /// Called when the Print Preview back button is selected.
   /// </summary>
   void ControlPrintPreview_OnBackEvent(object sender, EventArgs e)
   {
      ShowPrintPreview(false);
   }

   /// <summary>
   /// Called when the Print button is selected on the Print Preview panel.
   /// </summary>
   void ControlPrintPreview_OnPrintEvent(object sender, EventArgs e)
   {
      ShowPrintPreview(false);
      _controlPrint.Print();
   }

   #endregion

   #region Support Methods
   /// <summary>
   /// Show the Print Preview panel. Hide the Print Panel.
   /// </summary>
   /// <param name="show">True to show the print preview panel.</param>
   void ShowPrintPreview(bool show)
   {
      ShowPanel(_panelPrint, !show);
      ShowPanel(_panelPrintPreview, show);
   }

   /// <summary>
   /// Show/Hide the panel using styles.
   /// </summary>
   void ShowPanel(Panel panel, bool show)
   {
      if (show)
         panel.Style["display"] = "";
      else
         panel.Style["display"] = "none";
   }

   #endregion
}

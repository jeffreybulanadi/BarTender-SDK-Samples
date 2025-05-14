using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SelectLabel : System.Web.UI.Page
{
   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Show the current menu selection by highlighting the selected menu link's background.
      ((HyperLink)Master.FindControl("_selectLabel")).CssClass = "MenuLinkSelected";

      try
      {
         // Get a list of all label formats found in the repository with extension *.btw
         string labelRepositoryFolder = ConfigurationManager.AppSettings["LabelRepository"];
         SearchOption searchOption = SearchOption.AllDirectories;
         string formatPath = Server.MapPath(labelRepositoryFolder);
         string[] sFileNames = Directory.GetFiles(formatPath, "*.btw", searchOption);

         if (sFileNames.Length != 0)
         {
            // Setup URL based on print method selected.
            string clickUrl;
            if (_listPrintMethod.SelectedValue == "client")
               clickUrl = "./Print.aspx?print=client";
            else
               clickUrl = "./Print.aspx?print=server";

            // Generate an image thumbnail for each label format found in the repository.
            // Add the image thumbnail to the label browser control.
            // All images are saved in the "Temp" folder located in the web application folder.
            // The temporary images are deleted when the web application is closed. See Global.asax.
            for (int i = 0; i < sFileNames.Length; i++)
            {
               string sName = Path.GetFileName(sFileNames[i]);

               LabelThumbnail thumbnail = (LabelThumbnail)LoadControl("LabelThumbnail.ascx");
               thumbnail.LabelFullPath = sFileNames[i];
               thumbnail.ThumbnailWidth = 128;
               thumbnail.ThumbnailHeight = 128;
               thumbnail.ShowFilename = true;
               thumbnail.ClickURL = clickUrl + "&filename=" + sName;
               thumbnail.CssClass = "LabelBrowserItems";

               _labelBrowser.Controls.Add(thumbnail);
            }
            
            ShowMessage("");
         }
         else
         {
            ShowMessage("There are no label formats in the repository.");
         }
      }
      catch (Exception ex)
      {
         ShowMessage("Unable to browse label formats. Please make sure you have BarTender installed and <br />activated as Enterprise Print Server edition.<br /><br />Details: " + ex.Message);
      }
   }
   #endregion
   
   #region Support Methods
   void ShowMessage(string msg)
   {
      _alert.Message = msg;
      
      // Hide the panel if there is a message.
      _panelPrintMethod.Visible = (msg.Length == 0);
   }
   #endregion
}

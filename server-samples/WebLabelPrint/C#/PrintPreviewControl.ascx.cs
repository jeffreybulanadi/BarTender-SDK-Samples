using System;
using System.Configuration;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;

public partial class PrintPreviewControl : System.Web.UI.UserControl
{
   #region Event Handlers
   public event EventHandler OnBackEvent;          // Fired when the Back button is selected.
   public event EventHandler OnPrintEvent;         // Fired when the Print button is selected.
   #endregion
   
   #region Private Member Variables
   private string       _imageTempFolder;         // Image temporary folder. Used for print preview images.
   private LabelFormat  _labelFormat;             // Label format data object.
   #endregion

   #region Enumerations
   // Declare navigation type enumeration.
   enum NavType
   {
      First,
      Previous,
      Next,
      Last
   };
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Add JavaScript to the OnError event of the preview image to notify if there is a problem loading the image.
      string errorNotifyScript = String.Format("onPreviewImageError(document.getElementById('{0}'))", _imagePreview.ClientID);
      _imagePreview.Attributes["OnError"] = errorNotifyScript;

      // Clear out any messages
      if (!IsPostBack)
         ShowMessage("");

      // Get the folder for temporary image files from Web.Config.
      _imageTempFolder = ConfigurationManager.AppSettings["ImageTempFolder"];
      if (string.IsNullOrEmpty(_imageTempFolder))
         _imageTempFolder = "Temp";
   }
   #endregion

   #region Web Callbacks
   /// <summary>
   /// Called when the print button is selected.
   /// </summary>
   protected void ButtonPrint_Click(object sender, EventArgs e)
   {
      if (OnPrintEvent != null)
         OnPrintEvent(this, e);
   }

   /// <summary>
   /// Called when the First navigation button is selected.
   /// </summary>
   protected void ButtonFirst_Click(object sender, EventArgs e)
   {
      Navigate(NavType.First);
   }

   /// <summary>
   /// Called when the Last navigation button is selected.
   /// </summary>
   protected void ButtonLast_Click(object sender, EventArgs e)
   {
      Navigate(NavType.Last);
   }

   /// <summary>
   /// Called when the Next navigation button is selected.
   /// </summary>
   protected void ButtonNext_Click(object sender, EventArgs e)
   {
       Navigate(NavType.Next);
   }
   /// <summary>
   /// Called when the Previou navigation button is selected.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
   protected void ButtonPrevious_Click(object sender, EventArgs e)
   {
      Navigate(NavType.Previous);
   }

   /// <summary>
   /// Called when the Back button is selected.
   /// </summary>
   protected void ButtonBack_Click(object sender, EventArgs e)
   {
      if (OnBackEvent != null)
         OnBackEvent(this, e);
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
         GeneratePreview(_labelFormat);
      }
   }

   public string PrintButtonClickJavaScript
   {
      get
      {
         return _buttonPrint.OnClientClick;
      }
      set
      {
         _buttonPrint.OnClientClick = value;
      }
   }

   #endregion

   #region Support Methods
   /// <summary>
   /// Navigate the page previews.
   /// </summary>
   /// <param name="navType">Navigation type</param>
   /// <returns></returns>
   void Navigate(NavType navType)
   {
      // Get values from the panel cache.
      int currentPageNumber = Int32.Parse(_previewPanel.Attributes["currentPreview"]);
      int totalPages = Int32.Parse(_previewPanel.Attributes["totalPreviews"]);

      // Based on navigation type selected set current page number.
      switch (navType)
      {
         case NavType.First:
            currentPageNumber = 1;
            break;

         case NavType.Last:
            currentPageNumber = totalPages;
            break;

         case NavType.Next:
            currentPageNumber += 1;
            break;

         case NavType.Previous:
            currentPageNumber -= 1;
            break;
      }

      // Enable/Disable navigation buttons.
      EnableNavButton(NavType.First, _buttonFirst, !(currentPageNumber == 1));
      EnableNavButton(NavType.Previous, _buttonPrevious, !(currentPageNumber == 1));
      EnableNavButton(NavType.Next, _buttonNext, !(currentPageNumber == totalPages));
      EnableNavButton(NavType.Last, _buttonLast, !(currentPageNumber == totalPages));

      // Update current page label
      _labelCurrentPage.Text = currentPageNumber.ToString();

      // Cache values in the panel.
      _previewPanel.Attributes["currentPreview"] = currentPageNumber.ToString();

      // Update the image preview     
      _imagePreview.ImageUrl = string.Format(@"./{0}/{1}.{2}.png", _imageTempFolder, _previewPanel.Attributes["imageGUID"], currentPageNumber);

      // Show alternate message in case the session has timed out and all preview images
      // have been deleted.
      _imagePreview.AlternateText = string.Format("Print preview image {0} of {1}.",currentPageNumber,totalPages);
   }

   /// <summary>
   /// Enable/Disable the navigation image button.
   /// </summary>
   /// <param name="navType">Navigation Type</param>
   /// <param name="imgBtn">Image button control</param>
   /// <param name="enabled">Enable/Disable control</param>
   void EnableNavButton(NavType navType, ImageButton imgBtn, bool enabled)
   {
      // Based on navigation type selected set enable/disable image control.
      switch (navType)
      {
         case NavType.First:
            imgBtn.ImageUrl = enabled ? "~/images/First.png" : "~/images/FirstDisabled.png";
            break;

         case NavType.Last:
            imgBtn.ImageUrl = enabled ? "~/images/Last.png" : "~/images/LastDisabled.png";
            break;

         case NavType.Next:
            imgBtn.ImageUrl = enabled ? "~/images/Next.png" : "~/images/NextDisabled.png";
            break;

         case NavType.Previous:
            imgBtn.ImageUrl = enabled ? "~/images/Previous.png" : "~/images/PreviousDisabled.png";
            break;
      }
      imgBtn.Enabled = enabled;
   }

   /// <summary>
   /// Generate the print preview pages for the lable format selected.
   /// </summary>
   void GeneratePreview(LabelFormat labelFormat)
   {
      try
      {
         // Get the Task Manager from the application cache.
         TaskManager taskManager = (TaskManager)Application["TaskManager"];

         // Make sure the task manager is valid and that engines are running.
         if ((taskManager != null) && (taskManager.TaskEngines.AliveCount != 0))
         {
            // Generate output image path and file name template
            string outputImagePath = Server.MapPath(_imageTempFolder);
            string guid = Guid.NewGuid().ToString();
            string sessionID = Session.SessionID;
            
            // Save all print preview generatede images using the Session ID and a unique GUID.
            // When the session ends all temporary files generated for a given Session ID are 
            // deleted (See Global.asax).
            string imageGuid = sessionID + "." + guid;
            string fileNameTemplate = imageGuid + ".%PageNumber%.png";

            // Create a ExportPrintPreviewToFileTask task object to export all print preview
            // pages to an image folder.  Generate all images using a set size of 850x850 as 
            // PNGs. Included all margins and borders.
            ExportPrintPreviewToFileTask taskPrint = new ExportPrintPreviewToFileTask(labelFormat,
                                                                                       outputImagePath,
                                                                                       fileNameTemplate,
                                                                                       ImageType.PNG,
                                                                                       new Resolution(850, 850),
                                                                                       ColorDepth.ColorDepth24bit,
                                                                                       System.Drawing.Color.White,
                                                                                       true,
                                                                                       true,
                                                                                       OverwriteOptions.Overwrite);

            // If a label format has prompting be sure to disable all prompts.
            taskPrint.LabelFormat.PrintSetup.EnablePrompting = false;

            // Put the export task onto the task queue for processing. Wait until it is done. Timeout if
            // longer than 60 seconds.
            TaskStatus status = taskManager.TaskQueue.QueueTaskAndWait(taskPrint, 60000);

            // Check for success. If good, then setup panel attributes and navigation to the first
            // page.
            if ((status == TaskStatus.Success) && (taskPrint.NumberOfPreviews > 0))
            {
               // Setup and cache preview panel attributes.
               _labelCurrentPage.Text = "1";
               _labelTotalPages.Text = taskPrint.NumberOfPreviews.ToString();
               _previewPanel.Attributes.Add("imageGUID", imageGuid);
               _previewPanel.Attributes.Add("currentPreview", "1");
               _previewPanel.Attributes.Add("totalPreviews", taskPrint.NumberOfPreviews.ToString());

               // Navigate to the first page.
               Navigate(NavType.First);

               // Show label format name in toolbar.
               _labelFormatFileName.Text = Path.GetFileName(labelFormat.FileName);

               // Display the preview panel.
               _previewPanel.Visible = true;
            }

            // If we have any error, then display.
            string msg = "";
            int msgPos = 1;
            foreach (Message message in taskPrint.Messages)
            {
               msg += message.Text.Replace("\n", "<br/>") + "<br/>";

               // In there are multiple messages separate with an extra break.
               if (msgPos != taskPrint.Messages.Count)
                  msg += "<br/>";
               msgPos++;
            }
            
            if (taskPrint.Messages.Count != 0)
               _imagePreview.AlternateText = "Preview image is not available.";

            ShowMessage(msg);
         }
         else
         {
            _previewPanel.Visible = false;
            ShowMessage("Unable to view the label print preview. Please make sure you have BarTender <br />installed, activated as Enterprise Print Server edition, and that print engines are <br />running. See the 'Manage Print Engines' menu task.");
         }

         // Show label format name in toolbar.
         _labelFormatFileName.Text = Path.GetFileName(labelFormat.FileName);
      }
      catch (Exception ex)
      {
         ShowMessage(ex.Message);
      }
   }

   /// <summary>
   /// Show messages to user.
   /// </summary>
   /// <param name="msg"></param>

   void ShowMessage(string msg)
   {
      _alert.Message = msg;
   }
   #endregion
}

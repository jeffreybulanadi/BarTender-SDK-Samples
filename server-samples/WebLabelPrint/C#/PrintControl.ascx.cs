using System;
using System.Configuration;
using System.Web.UI;

using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;

public partial class PrintControl : System.Web.UI.UserControl
{
   #region Event Handlers
   public EventHandler OnPrintPreview;       // Fired when the Print Preview button is selected.
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      string printType = Request.QueryString.Get("print");
      if (printType != null && printType == "client")
      {
         _labelTitle.Text = "Print Label Format Over Internet";
         _labelDescription.Text = @"Select a label format and printer, and then print. 
                                    Printers shown in the list are Internet printers attached to 
                                    the client computer. Only client side printers that have a matching 
                                    server side printer driver installed are displayed.";
         
         _listPrinters.ShowClientPrinters = true;
         _listPrinters.ShowServerPrinters = false;

         // Get the print license for the current printer when the print button is pressed.
         _buttonPrint.OnClientClick = PrintToFileLicenseJavaScript;
      }
      else
      {
         _labelTitle.Text = "Print Label Format";
         _labelDescription.Text = @"Select a label format and printer, and then print.  Printers 
                                    shown in the list are Standard Windows printer drivers 
                                    (Seagull and non-Seagull) installed on the server computer.";
         
         _listPrinters.ShowClientPrinters = false;
         _listPrinters.ShowServerPrinters = true;
      }

      _listPrinters.Alert = _alert;
      _listPrinters.PrintButton = _buttonPrint;
      _listPrinters.PreviewButton = _buttonPrintPreview;

      // Check to see if there were label formats found in the repository.  
      if (_listLabelFormats.HasLabelFormats)
      {
         if (!Page.IsPostBack)
         {
            string fileName = Request.QueryString.Get("fileName");
            fileName = Server.UrlDecode(fileName);

            _listLabelFormats.SelectedLabelFormatName = fileName;
         }

         try
         {
            // Get the label format data from file and update print panel controls.
            LabelFormat labelFormat = GetLabelFormatDataFromFile(_listLabelFormats.SelectedLabelFormatRepositoryFullPath);
            if (labelFormat != null)
            {
               _copyControl.Alert = _alert;

               UpdateControls(labelFormat);

               // Clear messages
               ShowMessage("");
            }
            else
                ShowMessage("Unable to print. Please make sure you have BarTender installed, activated as <br />Enterprise Print Server edition, and that print engines are running. See the <br />'Manage Print Engines' menu task.");
         }
         catch (Exception ex)
         {
            ShowMessage(ex.Message);
         }
      }
      else
      {
         ShowMessage("There are no label formats in the respository available for printing.");
         _buttonPrint.Enabled = false;
         _buttonPrintPreview.Enabled = false;
         _listPrinters.Enabled = false;
      }
   }
   #endregion

   #region Web Callbacks
   /// <summary>
   /// Called when the Print button is selected.
   /// </summary>
   protected void ButtonPrint_Click(object sender, EventArgs e)
   {
      Print();
   }

   /// <summary>
   /// Called when the print preview button is selected.
   /// </summary>
   /// <param name="sender"></param>
   /// <param name="e"></param>
   protected void ButtonPrintPreview_Click(object sender, EventArgs e)
   {
      // Pass the event to the container if needed.
      if (OnPrintPreview != null)
         OnPrintPreview(this, e);
   }

   // Do post-processing of the client print. 
   // Set the hidden field with the client print code and then call the javascript to print
   void TaskPrint_Succeeded(object sender, TaskEventArgs e)
   {
      PrintLabelFormatTask taskPrint = sender as PrintLabelFormatTask;

      if (taskPrint != null)
      {
         _hiddenClientPrintCode.Value = taskPrint.PrintCode;

         string printerName = JavaScriptSupport.EscapeSpecialCharacters(_listPrinters.LastPrinterName);
         Page.ClientScript.RegisterStartupScript(GetType(), "SampleClientPrint", string.Format("SampleClientPrint('{0}','{1}','{2}','{3}');", printerName, _hiddenClientPrintCode.ClientID, _alert.MessageClientID, _alert.PanelClientID), true);
      }
   }
   #endregion

   #region Public Properties
   /// <summary>
   /// Return the label format object reference. Returns null if the Format's data is invalid.
   /// </summary>
   public LabelFormat LabelFormatObject
   {
      get
      {
         return CreateLabelFormatFromPageData(_listLabelFormats.SelectedLabelFormatRepositoryFullPath);
      }
   }

   public string PrintToFileLicenseJavaScript
   {
      get
      {
         return string.Format("SetPrintLicense(document.getElementById('{0}').value,'{1}','{2}','{3}');", _listPrinters.ListClientID, _listPrinters.PrintLicenseClientID, _alert.MessageClientID, _alert.PanelClientID);
      }
   }
   #endregion
   
   #region Public Methods
   public void Print()
   {
      try
      {
         TaskManager taskManager = (TaskManager)Application["TaskManager"];
         
         // Make sure the task manager is valid and that engines are running.
         if ((taskManager != null) && (taskManager.TaskEngines.AliveCount != 0))
         {
            LabelFormat labelFormat = CreateLabelFormatFromPageData(_listLabelFormats.SelectedLabelFormatRepositoryFullPath);
            if (labelFormat != null)
            {
               PrintLabelFormatTask taskPrint = new PrintLabelFormatTask(labelFormat);
               taskPrint.PrintTimeout = Int32.Parse(ConfigurationManager.AppSettings["PrintTimeout"]);

               if (_listPrinters.LastPrintType == "client")
               {
                  // Set PrintTimeout to 0 to ensure print job status monitoring does not interfere with client printing
                  taskPrint.PrintTimeout = 0;
                  // Since the client print needs to post-process the print job, sign up for the succeeded event.
                  taskPrint.Succeeded += new EventHandler<TaskEventArgs>(TaskPrint_Succeeded);
               }
               taskManager.TaskQueue.QueueTaskAndWait(taskPrint, 60000);

               // Get messages
               foreach (Message message in taskPrint.Messages)
               {
                  string formattedMessage = message.Text.Replace("\n", "<br/>");
                  _alert.AddMessage(formattedMessage + "<br/>");
               }
            }
         }
         else
         {
            ShowMessage("Unable to print. Please make sure you have BarTender installed, activated as <br />Enterprise Print Server edition, and that print engines are running. See the <br />'Manage Print Engines' menu task.");
         }
      }
      catch (Exception ex)
      {
         ShowMessage(ex.Message);
      }
   }   
   #endregion

   #region Support Methods
   /// <summary>
   /// Show a message
   /// </summary>
   void ShowMessage(string msg)
   {
      _alert.Message = msg;
   }

   /// <summary>
   /// Create a label format object using page data.
   /// </summary>
   /// <param name="labelFormatFullPath">Full path to the label format file name.</param>
   /// <returns>A LabelFormat object reference. If the LabelFormat's data is invalid, null is returned.</returns>
   LabelFormat CreateLabelFormatFromPageData(string labelFormatFullPath)
   {
      LabelFormat labelFormat = null;
      bool isLabelDataValid = true;

      // Check if we are server or client printing
      String lastPrinter = Request.Form.Get(_listPrinters.ListUniqueID);
      if (string.IsNullOrEmpty(lastPrinter))
      {
         ShowMessage("No printers were selected. Unable to print.");
      }
      else
      {
         try
         {
            labelFormat = new LabelFormat(_listLabelFormats.SelectedLabelFormatRepositoryFullPath);
         }
         catch (System.IO.FileNotFoundException)
         {
            string message = string.Format("The selected format '{0}' does not exist. It may have been moved, renamed or deleted.", _listLabelFormats.SelectedLabelFormatName);
            ShowMessage(message);
            isLabelDataValid = false;
         }

         if (isLabelDataValid)
         {
            UpdateControls(labelFormat);

            // For each control get control values into the label format object.
            _subStringsControl.UpdateFormatData(labelFormat);
            _promptControl.UpdateFormatData(labelFormat);
            _queryPromptsControl.UpdateFormatData(labelFormat);
            isLabelDataValid = _copyControl.UpdateFormatData(labelFormat);

            labelFormat.PrintSetup.EnablePrompting = false;

            if (_listPrinters.LastPrintType == "server")
            {
               labelFormat.PrintSetup.PrintToFile = false;
               labelFormat.PrintSetup.PrinterName = _listPrinters.LastPrinterName;
            }
            else
            {
               string compatibleServerPrinter = _listPrinters.LastCompatibleServerPrinter;
               labelFormat.PrintSetup.PrintToFile = true;
               labelFormat.PrintSetup.PrinterName = compatibleServerPrinter;
               labelFormat.PrintSetup.PrintToFileLicense = Request.Form.Get(_listPrinters.PrintLicenseUniqueID);
               string tempFullPath = (string)Application["TempFolderFullPath"];
               labelFormat.PrintSetup.PrintToFileName = System.IO.Path.Combine(tempFullPath,Guid.NewGuid().ToString() + ".prn");
            }
         }
      }

      return (isLabelDataValid ? labelFormat : null);
   }

   /// <summary>
   /// Get the label format data from BarTender.
   /// </summary>
   /// <param name="labelFormatFullPath">Full path to the label format file name.</param>
   /// <returns>A LabelFormat object reference.</returns>
   LabelFormat GetLabelFormatDataFromFile(string labelFormatFullPath)
   {
      LabelFormat labelFormat = null;
      
      TaskManager taskManager = (TaskManager)Application["TaskManager"];

      // Make sure the task manager is valid and that engines are running.
      if ((taskManager != null) && (taskManager.TaskEngines.AliveCount != 0))
      {
         // Create a task to get all label format properties and submit to the task queue
         // for processing.
         GetLabelFormatTask getFormatTask = new GetLabelFormatTask(labelFormatFullPath);
         taskManager.TaskQueue.QueueTaskAndWait(getFormatTask, 60000);

         // Return the label format object that has all label format data set.
         labelFormat = getFormatTask.LabelFormat;
      }
      return labelFormat;
   }

   /// <summary>
   /// Update all user print panels with label format object data.
   /// </summary>
   /// <param name="labelFormat">A LabelFormat object reference.</param>
   void UpdateControls(LabelFormat labelFormat)
   {
      if (labelFormat != null)
      {
         _copyControl.LabelFormatObject = labelFormat;
         _subStringsControl.LabelFormatObject = labelFormat;
         _promptControl.LabelFormatObject = labelFormat;
         _queryPromptsControl.LabelFormatObject = labelFormat;
         _listPrinters.LabelFormatObject = labelFormat;
      }
   }
   #endregion
}

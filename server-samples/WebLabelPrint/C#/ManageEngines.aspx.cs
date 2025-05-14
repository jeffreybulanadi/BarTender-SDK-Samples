using System;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

using Seagull.BarTender.PrintServer;

public partial class ManageEngines : System.Web.UI.Page
{
   #region Private Member Variables
   private TaskManager _taskManager;              // BarTender Task Manager object.
   private int         _numEnginesForPage = 1;    // Number of print engines
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Show the current menu selection by highlighting the selected menu link's background.
      ((HyperLink)Master.FindControl("_manageEngines")).CssClass = "MenuLinkSelected";

      // The Print Server task manager instance is created in the Global.aspx
      _taskManager = (TaskManager)Application["TaskManager"];

      if (_taskManager != null)
      {
         // Clear all error messages on the page.
         ClearMessage();

         // Perform validation on the number of engines specified.
         if (ValidatePage())
         {
            _numEnginesForPage = Int32.Parse(_textNumberPrintEngines.Text);

            // Multiple users can be viewing this page from different client 
            // browsers at the same time. Therefore, initialize the number of 
            // print engines based on what the web application is currently running.
            int engineAliveCount = _taskManager.TaskEngines.AliveCount;
            _textNumberPrintEngines.Text = ((engineAliveCount != 0) ? engineAliveCount.ToString() : "1");
         }

         // Update controls and status tables.
         Update();
      }
      else
      {
         ShowMessage("Unable to manage print engines. Please make sure you have BarTender installed and activated as Enterprise Print Server edition.");
         
         // Disable all controls.
         EnableStartStopPanelControls(false);
      }
   }
   #endregion

   #region Web Callbacks
   /// <summary>
   /// Called when the Start button is selected.
   /// </summary>
   protected void ButtonStartEngines_Click(object sender, EventArgs e)
   {
      // Validate correct print engine entry.
      if (ValidatePage())
      {
         try
         {
            // Start one or more BarTender print engines using the Print Server SDK.
            // This will start separate bartend.exe processes in the background.
            _taskManager.Start(_numEnginesForPage);
            _textNumberPrintEngines.Text = _numEnginesForPage.ToString();
         }
         catch (Exception ex)
         {
            ShowMessage("One or more print engines could not be started. <br /><br />Reason: " + ex.Message);
         }
      }
      Update();
   }

   /// <summary>
   /// Called when the Stop button is selected.
   /// </summary>
   protected void ButtonStopEngines_Click(object sender, EventArgs e)
   {
      try
      {
         // Stop all running BarTender print engines running in the background.
         // If for whatever reason one or more print engines are taking a long
         // time to stop kill all processes after "engineStopTimeout" seconds (e.g. 3 seconds).
         int engineStopTimeout = Int32.Parse(ConfigurationManager.AppSettings["EngineStopTimeout"]);
         _taskManager.Stop(engineStopTimeout, true);
      }
      catch (Exception ex)
      {
         ShowMessage("An error occured while trying to stop one or more print engines.<br /><br />Reason: " + ex.Message);
      }
      Update();
   }

   /// <summary>
   /// Called when the Change button is selected.
   /// </summary>
   protected void ButtonChangeNumPrintEngines_Click(object sender, EventArgs e)
   {
      // Validate correct print engine entry.
      if (ValidatePage())
      {
         try
         {
            // Resize all running BarTender print engines running in the background.
            // If for whatever reason one or more print engines are taking a long
            // time to stop kill the needed processes after "engineStopTimeout" seconds (e.g. 3 seconds).
            int engineStopTimeout = Int32.Parse(ConfigurationManager.AppSettings["EngineStopTimeout"]);
            _taskManager.Resize(_numEnginesForPage, engineStopTimeout, true);
            _textNumberPrintEngines.Text = _numEnginesForPage.ToString();
         }
         catch (Exception ex)
         {
            ShowMessage("One or more print engines could not be restarted.<br /><br />Reason: " + ex.Message);
         }
      }
      Update();
   }
   #endregion

   #region Support Methods
   /// <summary>
   /// Validate the page for correct entry of number of print engines.
   /// </summary>
   /// <returns>
   /// True if OK to continue, else False.
   /// </returns>
   private bool ValidatePage()
   {
      bool validated = true;
      int numEngines = 0;
      int maxAllowedPrintEngines = 0;
      // More print engines can be increased from 5. The maximum number of print engines
      // that can be started is dependant on system resources available and the operating 
      // system being used.
      try
      {
         maxAllowedPrintEngines = Int32.Parse(ConfigurationManager.AppSettings["MaxAllowedPrintEngines"]);
      }
      catch (Exception)
      {
         ShowMessage("maxAllowedPrintEngines in the web.config must be a number.");
         validated = false;
      }

      if(validated)
      {      
         try
         {
            numEngines = Int32.Parse(_textNumberPrintEngines.Text);
         }
         catch (Exception)
         {
            ShowMessage("The number of print engines must be a number.");
            SetFocus(_textNumberPrintEngines);
            validated = false;
         }

         if (validated && (numEngines < 1) || (numEngines > maxAllowedPrintEngines))
         {
            ShowMessage("The number of print engines must be between 1 and " + maxAllowedPrintEngines.ToString());
            SetFocus(_textNumberPrintEngines);
            validated = false;
         }
      }

      return validated;
   }

   /// <summary>
   /// Update engine controls and status tables.
   /// </summary>
   void Update()
   {
      UpdateStartStopPanel();
      UpdateStatusTables();
   }

   /// <summary>
   /// Update the controls on the engine controls panel.
   /// </summary>
   void UpdateStartStopPanel()
   {
      bool printEnginesStarted = (_taskManager.TaskEngines.AliveCount > 0);
      _buttonStartEngines.Enabled = !printEnginesStarted;
      _buttonStopEngines.Enabled = printEnginesStarted;
      _buttonChangeNumPrintEngines.Enabled = printEnginesStarted;
   }

   /// <summary>
   /// Update the status table values.
   /// </summary>
   void UpdateStatusTables()
   {
      // Print engine status values
      _labelNumPrintEngines.Text = _taskManager.TaskEngines.AliveCount.ToString();
      _labelBusyPrintEngines.Text = _taskManager.TaskEngines.BusyCount.ToString();

      // Task queue status values
      _labelNumTasks.Text = _taskManager.TaskQueue.Count.ToString();
      _labelQueueLocked.Text = _taskManager.TaskQueue.IsLocked.ToString();
      _labelQueuePaused.Text = _taskManager.TaskQueue.IsPaused.ToString();
   }
   
   void EnableStartStopPanelControls(bool enable)
   {
      _buttonStartEngines.Enabled = enable;
      _buttonStopEngines.Enabled = enable;
      _buttonChangeNumPrintEngines.Enabled = enable;
      _textNumberPrintEngines.Enabled = enable;
   }

   /// <summary>
   /// Show error message text.
   /// </summary>
   /// <param name="msg">Message text.</param>
   private void ShowMessage(string msg)
   {
      _alert.Message = msg;
   }

   /// <summary>
   /// Clear error message text. 
   /// </summary>
   /// <param name="msg">Message text.</param>
   private void ClearMessage()
   {
      _alert.Message = string.Empty;
   }
   #endregion
  
}

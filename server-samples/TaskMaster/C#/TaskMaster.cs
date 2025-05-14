using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer;
using Seagull.BarTender.PrintServer.Tasks;

namespace TaskMaster
{
   /// <summary>
   /// TaskMaster Sample
   /// This sample allows the user to select Tasks to run and view the resulting output.
   ///  
   /// This sample is intended to show how to:
   ///  -Start a number of TaskEngines.
   ///  -Create various Tasks.
   ///  -Submit Tasks to the TaskQueue.
   ///  -Receive events from Tasks to get the output.
   /// </summary>
   public partial class TaskMaster : Form
   {
      #region Fields
      private TaskManager taskManager;
      private readonly string labelFilename;
      private const string appName = "Task Manager";
      private string thumbnailPath = "";
      #endregion // Fields

      #region Constructor
      public TaskMaster()
      {
         InitializeComponent();

         labelFilename = Path.GetFullPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + @"\..\..\Address.btw");
      }
      #endregion // Constructor

      #region Delegates
      public delegate void DoUpdateOutputDelegate(string output, Task task);
      #endregion // Delegates

      #region Event Handlers
      #region Form Event Handlers
      /// <summary>
      /// Called when the user opens the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void TaskExecutor_Load(object sender, EventArgs e)
      {
         taskManager = new TaskManager();
         taskManager.TaskEngineStatusChanged += new EventHandler<TaskEngineStatusChangedEventArgs>(taskEngines_EngineStatusChanged);
         taskManager.ErrorOccurred += new EventHandler<EngineErrorEventArgs>(taskEngines_ErrorOccurred);

         // Place a set of default Tasks into the available list
         lstAvailableTasks.Items.Add(Properties.Resources.PrintTaskLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.ExportThumbnailLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.ExportPreviewLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.XMLScriptTaskLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.GetFormatTaskLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.GroupTaskLabel);
         lstAvailableTasks.Items.Add(Properties.Resources.CustomTaskLabel);

         txtNumEngines.MaxLength = 2;
      }

      /// <summary>
      /// Called when the user closes the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void TaskExecutor_FormClosed(object sender, FormClosedEventArgs e)
      {
         taskManager.Stop(1000, true);
      }

      /// <summary>
      /// Starts the selected number of Engines.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnStart_Click(object sender, EventArgs e)
      {
         int numEngines = 1;
         bool success = true;

         try
         {
            numEngines = Int32.Parse(txtNumEngines.Text);
            if (numEngines < 1)
               success = false;
         }
         catch (FormatException)
         {
            success = false;
         }

         if (!success)
         {
            MessageBox.Show(this, "The number of engines must be a positive integer.", appName);
         }
         else
         {
            try
            {
               Cursor.Current = Cursors.WaitCursor;
               taskManager.Start(numEngines);
            }
            catch (LicenseException exception)
            {
               string msg = string.Format("You are using the BarTender {0} edition. To continue the BarTender Enterprise Print Server edition must be installed and activated.", exception.Edition);
               MessageBox.Show(this, msg, appName);
            }
            catch (PrintEngineException exception)
            {
               MessageBox.Show(this, exception.Message, appName);
            }
            catch (Exception exc)
            {
               MessageBox.Show(this, exc.Message, appName);
            }

            lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString();
            if (taskManager.TaskEngines.AliveCount > 0)
            {
               txtNumEngines.Enabled = false;
               btnStop.Enabled = true;
               btnStart.Enabled = false;
               if (lstRunTasks.Items.Count > 0)
                  btnRun.Enabled = true;
            }
         }
      }

      /// <summary>
      /// Stops the running Engines.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnStop_Click(object sender, EventArgs e)
      {
         Result result = taskManager.Stop(10000, false);
         lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString();
         if (result == Result.Timeout)
         {
            MessageBox.Show(this, "Unable to stop the print engines. They may still be busy.", appName);
         }
         else if (result == Result.Failure)
         {
            MessageBox.Show(this, "Unable to stop the print engines. Catastrophic error.", appName);
         }
         if (taskManager.TaskEngines.AliveCount == 0)
         {
            txtNumEngines.Enabled = true;
            btnStop.Enabled = false;
            btnStart.Enabled = true;
            btnRun.Enabled = false;
         }
      }

      /// <summary>
      /// Adds the selected Task to the list of Tasks to run.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnAdd_Click(object sender, EventArgs e)
      {
         if (lstAvailableTasks.SelectedIndex >= 0)
         {
            string selected = lstAvailableTasks.SelectedItem.ToString();
            lstRunTasks.Items.Add(selected);
            if (taskManager.TaskEngines.AliveCount != 0)
               btnRun.Enabled = true;
         }
      }

      /// <summary>
      /// Sends all Tasks in the right pane to the TaskQueue.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnRun_Click(object sender, EventArgs e)
      {
         QueueTasks();
         lblTasksInQueue.Text = taskManager.TaskQueue.Count.ToString();
         lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString();
         lblEnginesBusy.Text = taskManager.TaskEngines.BusyCount.ToString();
      }

      /// <summary>
      /// Removes the selected Task.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnRemove_Click(object sender, EventArgs e)
      {
         if (lstRunTasks.SelectedIndex >= 0)
            lstRunTasks.Items.RemoveAt(lstRunTasks.SelectedIndex);
         if (lstRunTasks.Items.Count == 0)
            btnRun.Enabled = false;
      }

      /// <summary>
      /// Enables or disables the remove button based on whether the user has
      /// a Task selected or not.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lstRunTasks_SelectedIndexChanged(object sender, EventArgs e)
      {
         btnRemove.Enabled = (lstRunTasks.SelectedIndex != -1);
      }

      /// <summary>
      /// Updates the stat counter for the number of Tasks in the queue,
      /// the number of running Engines and the number of busy Engines.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void timerTaskUpdater_Tick(object sender, EventArgs e)
      {
         lblTasksInQueue.Text = taskManager.TaskQueue.Count.ToString();
         lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString();
         lblEnginesBusy.Text = taskManager.TaskEngines.BusyCount.ToString();
      }

      /// <summary>
      /// Enables or disables the add button based on whether the user
      /// has a Task selected. Also changes the Task description based
      /// on which Task is selected.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lstAvailableTasks_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (lstAvailableTasks.SelectedIndex != -1)
         {
            btnAdd.Enabled = true;
            string selected = lstAvailableTasks.SelectedItem.ToString();
            if (selected == Properties.Resources.PrintTaskLabel)
            {
               lblTaskInfo.Text = Properties.Resources.PrintTaskDescription;
            }
            else if (selected == Properties.Resources.ExportThumbnailLabel)
            {
               lblTaskInfo.Text = Properties.Resources.ExportThumbnailDescription;
            }
            else if (selected == Properties.Resources.ExportPreviewLabel)
            {
               lblTaskInfo.Text = Properties.Resources.ExportPreviewDescription;
            }
            else if (selected == Properties.Resources.XMLScriptTaskLabel)
            {
               lblTaskInfo.Text = Properties.Resources.XMLScriptTaskDescription;
            }
            else if (selected == Properties.Resources.GetFormatTaskLabel)
            {
               lblTaskInfo.Text = Properties.Resources.GetFormatTaskDescription;
            }
            else if (selected == Properties.Resources.GroupTaskLabel)
            {
               lblTaskInfo.Text = Properties.Resources.GroupTaskDescription;
            }
            else if (selected == Properties.Resources.CustomTaskLabel)
            {
               lblTaskInfo.Text = Properties.Resources.CustomTaskDescription;
            }
         }
         else
         {
            btnAdd.Enabled = false;
            lblTaskInfo.Text = Properties.Resources.NoTaskDescription;
         }
      }
      #endregion // Form Event Handlers

      #region Task Event Handlers
      /// <summary>
      /// Event called when the Print Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Print Task error.", task });
      }

      /// <summary>
      /// Event called when the Print Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask_Completed(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Print Task has completed.", task });
      }

      /// <summary>
      /// Event called when the Export Thumbnail Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void thumbnailTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Export Thumbnail Task error.", task });
      }

      /// <summary>
      /// Event called when the Export Thumbnail Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void thumbnailTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         ExportImageToFileTask exportTask = (ExportImageToFileTask)task;
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Export Thumbnail Task exported to " + exportTask.FileName, task });
      }

      /// <summary>
      /// Event called when the Export Print Preview Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void previewTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Export Preview Task error.", task });
      }

      /// <summary>
      /// Event called when the Export Print Preview Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void previewTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         ExportPrintPreviewToFileTask exportTask = (ExportPrintPreviewToFileTask)task;
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Export Preview Task exported to " + exportTask.Directory, task });
      }

      /// <summary>
      /// Event called when the XMLScript Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void xmlTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "XMLScript Task error.", task });
      }

      /// <summary>
      /// Event called when the XMLScript Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void xmlTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "XMLScript Task complete.", task });
      }

      /// <summary>
      /// Event called when the Get Format Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void getFormatTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Get Format Task error.", task });
      }

      /// <summary>
      /// Event called when the Get Format Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void getFormatTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         GetLabelFormatTask formatTask = (GetLabelFormatTask)task;
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Get Format Task complete.", task });
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Name: " + formatTask.LabelFormat.SubStrings["LastName"].Value, null });
      }

      /// <summary>
      /// Event called when the Group Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void groupTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task error.", task });
      }

      /// <summary>
      /// Event called when the Group Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void groupTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         GroupTask groupTask = (GroupTask)task;
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group task complete.", task });
      }

      /// <summary>
      /// Event called when the first print Task in the GroupTask failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask1_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print1 error.", task });
      }

      /// <summary>
      /// Event called when the first print Task in the Group Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask1_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print1 has completed.", task });
      }

      /// <summary>
      /// Event called when the second print Task in the Group Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask2_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print2 error.", task });
      }

      /// <summary>
      /// Event called when the second print Task in the Group Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask2_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print2 has completed.", task });
      }

      /// <summary>
      /// Event called when the third print Task in the Group Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask3_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print3 has completed.", task });
      }

      /// <summary>
      /// Event called when the third print Task in the GroupTask failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void printTask3_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Group Task Print3 error.", task });
      }

      /// <summary>
      /// Event called when the Custom Task failed.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void customTask_ErrorOccurred(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Custom Task error.", task });
      }

      /// <summary>
      /// Event called when the Custom Task succeeded.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEventArgs"></param>
      void customTask_Succeeded(object task, TaskEventArgs taskEventArgs)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Custom Task has completed.", task });
      }
      #endregion // Task Event Handlers

      #region TaskEngine Event Handlers
      /// <summary>
      /// Event called when a TaskEngine's status changes.
      /// </summary>
      /// <param name="engine"></param>
      /// <param name="status"></param>
      void taskEngines_EngineStatusChanged(object sender, TaskEngineStatusChangedEventArgs args)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Engine Status: " + args.Status, null });
      }

      /// <summary>
      /// Event called when a TaskEngine has an error.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="exception"></param>
      void taskEngines_ErrorOccurred(object sender, EngineErrorEventArgs args)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstOutput.BeginInvoke(doUpdateOutputDelegate, new object[] { "Engines error occurred: " + args.Exception.Message, null });
      }
      #endregion // TaskEngine Event Handlers

      #endregion // Event Handlers

      #region Methods
      /// <summary>
      /// Updates the output listbox when it receives a message.
      /// </summary>
      /// <param name="task"></param>
      /// <param name="taskEngine"></param>
      private void DoUpdateOutput(string output, Task task)
      {
         lstOutput.Items.Add(output);
         if (task != null)
         {
            // Output the Task's messages.
            if (task.Messages.Count > 1)
            {
               lstOutput.Items.Add("Task Messages:");
               string text;
               foreach (Seagull.BarTender.Print.Message message in task.Messages)
               {
                  // Let's remove any carriage returns and linefeeds since
                  // we are putting each message on a single line.
                  text = message.Text.Replace('\n', ' ');
                  text = text.Replace('\r', ' ');
                  lstOutput.Items.Add("\t" + text);
               }
            }
         }
      }

      /// <summary>
      /// Thumbnails need to go into a new place to assure we don't
      /// conflict with any existing files.
      /// </summary>
      private void CreateThumbnailPathIfNeeded()
      {
         if (thumbnailPath.Length == 0)
         {
            // Create a temporary folder to hold the images.
            string tempPath = Path.GetTempPath(); // Something like "C:\Documents and Settings\<username>\Local Settings\Temp\"
            string newFolder;

            // It's not likely we'll have a path that already
            // exists, but we'll check for it anyway.
            do
            {
               newFolder = Path.GetRandomFileName();
               thumbnailPath = tempPath + newFolder; // newFolder is something crazy like "gulvwdmt.3r4"
            } while (Directory.Exists(thumbnailPath));
            Directory.CreateDirectory(thumbnailPath);
            DoUpdateOutput("Temporary folder created: " + thumbnailPath, null);
         }
      }

      /// <summary>
      /// Adds copies of all Tasks in the list to the TaskQueue to be run.
      /// </summary>
      private void QueueTasks()
      {
         foreach (string taskName in lstRunTasks.Items)
         {
            try
            {
               Task task = null;
               if (taskName == Properties.Resources.PrintTaskLabel)
               {
                  // The print task.
                  PrintLabelFormatTask printTask = new PrintLabelFormatTask(labelFilename);
                  printTask.ErrorOccurred += new EventHandler<TaskEventArgs>(printTask_ErrorOccurred);
                  printTask.Completed += new EventHandler<TaskEventArgs>(printTask_Completed);
                  task = printTask;
               }
               else if (taskName == Properties.Resources.ExportThumbnailLabel)
               {
                  CreateThumbnailPathIfNeeded();

                  // The thumbnail export task.
                  ExportImageToFileTask thumbnailTask = new ExportImageToFileTask(labelFilename, thumbnailPath + @"\exportThumbnail.jpg");
                  thumbnailTask.ImageType = ImageType.JPEG;
                  thumbnailTask.Colors = Seagull.BarTender.Print.ColorDepth.ColorDepth24bit;
                  thumbnailTask.Resolution = new Resolution(ImageResolution.Screen);
                  thumbnailTask.ErrorOccurred += new EventHandler<TaskEventArgs>(thumbnailTask_ErrorOccurred);
                  thumbnailTask.Succeeded += new EventHandler<TaskEventArgs>(thumbnailTask_Succeeded);
                  task = thumbnailTask;
               }
               else if (taskName == Properties.Resources.ExportPreviewLabel)
               {
                  CreateThumbnailPathIfNeeded();

                  // The print preview export task.
                  ExportPrintPreviewToFileTask previewTask = new ExportPrintPreviewToFileTask(labelFilename, thumbnailPath, @"exportPreview%PageNumber%.jpg");
                  previewTask.ImageType = ImageType.JPEG;
                  previewTask.Colors = Seagull.BarTender.Print.ColorDepth.ColorDepth24bit;
                  previewTask.Resolution = new Resolution(ImageResolution.Screen);
                  previewTask.ErrorOccurred += new EventHandler<TaskEventArgs>(previewTask_ErrorOccurred);
                  previewTask.Succeeded += new EventHandler<TaskEventArgs>(previewTask_Succeeded);
                  task = previewTask;
               }
               else if (taskName == Properties.Resources.XMLScriptTaskLabel)
               {
                  // XMLScript task.
                  string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
                                   "<XMLScript Version=\"2.0\" Name=\"XMLScripter Sample\" ID=\"123\">" +
                                      "<Command Name=\"XMLScripter\">" +
                                         "<Print WaitForJobToComplete=\"true\" JobName=\"XMLScript Task\" Timeout=\"30000\" " +
                                            "ReturnPrintData=\"true\" ReturnSummary=\"true\"  ReturnLabelData=\"true\" ReturnChecksum=\"true\">"+
                                            "<Format>" + labelFilename  + "</Format>"+
                                         "</Print>"+
                                      "</Command>"+
                                   "</XMLScript>";
                  XMLScriptTask xmlTask = new XMLScriptTask(xml, XMLSourceType.ScriptString);
                  xmlTask.ErrorOccurred += new EventHandler<TaskEventArgs>(xmlTask_ErrorOccurred);
                  xmlTask.Succeeded += new EventHandler<TaskEventArgs>(xmlTask_Succeeded);
                  task = xmlTask;
               }
               else if (taskName == Properties.Resources.GetFormatTaskLabel)
               {
                  // Get Format Properties Task - this will get a substring on the label.
                  GetLabelFormatTask getFormatTask = new GetLabelFormatTask(labelFilename);
                  getFormatTask.ErrorOccurred += new EventHandler<TaskEventArgs>(getFormatTask_ErrorOccurred);
                  getFormatTask.Succeeded += new EventHandler<TaskEventArgs>(getFormatTask_Succeeded);
                  task = getFormatTask;
               }
               else if (taskName == Properties.Resources.GroupTaskLabel)
               {
                  // Group Task - this will run three printjobs.
                  GroupTask groupTask = new GroupTask();
                  groupTask.ErrorOccurred += new EventHandler<TaskEventArgs>(groupTask_ErrorOccurred);
                  groupTask.Succeeded += new EventHandler<TaskEventArgs>(groupTask_Succeeded);

                  PrintLabelFormatTask printTask1 = new PrintLabelFormatTask(labelFilename);
                  groupTask.Add(printTask1);
                  printTask1.ErrorOccurred += new EventHandler<TaskEventArgs>(printTask1_ErrorOccurred);
                  printTask1.Succeeded += new EventHandler<TaskEventArgs>(printTask1_Succeeded);

                  PrintLabelFormatTask printTask2 = new PrintLabelFormatTask(labelFilename);
                  groupTask.Add(printTask2);
                  printTask2.ErrorOccurred += new EventHandler<TaskEventArgs>(printTask2_ErrorOccurred);
                  printTask2.Succeeded += new EventHandler<TaskEventArgs>(printTask2_Succeeded);

                  PrintLabelFormatTask printTask3 = new PrintLabelFormatTask(labelFilename);
                  groupTask.Add(printTask3);
                  printTask3.ErrorOccurred += new EventHandler<TaskEventArgs>(printTask3_ErrorOccurred);
                  printTask3.Succeeded += new EventHandler<TaskEventArgs>(printTask3_Succeeded);

                  task = groupTask;
               }
               else if (taskName == Properties.Resources.CustomTaskLabel)
               {
                  // Custom Task - this will change a substring on the label and print it.
                  CustomTask customTask = new CustomTask(labelFilename);
                  customTask.ErrorOccurred += new EventHandler<TaskEventArgs>(customTask_ErrorOccurred);
                  customTask.Succeeded += new EventHandler<TaskEventArgs>(customTask_Succeeded);
                  task = customTask;
               }
               taskManager.TaskQueue.QueueTask(task);
            }
            catch (Exception e)
            {
               MessageBox.Show(this, e.Message, appName);
            }
         }
      }
      #endregion // Methods
   }
}
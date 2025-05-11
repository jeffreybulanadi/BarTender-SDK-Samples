using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace PrintJobStatusMonitor
{
   /// <summary>
   /// Print Job Status Monitor Sample
   /// This sample allows the user to open a format, select a printer, print the format, and
   /// view printer status while the format is printing.
   ///  
   /// This sample is intended to show how to:
   ///  -Hook up to PrintJob status events.
   ///  -Open a Format in BarTender
   ///  -Print a Format
   ///  -Export a thumbnail of the Format
   ///  -Get a list of printers and set the printer on a format.
   ///  -Start and Stop a BarTender engine.
   /// </summary>
   public partial class PrintJobStatusMonitor : Form
   {
      #region Fields
      // Common strings.
      private readonly string appName = "Print Job Status Monitor";

      private Engine engine = null; // The BarTender Print Engine.
      private LabelFormatDocument format = null; // The currently open Format.
      private string thumbnailFile = "";
      #endregion

      #region Constructor
      /// <summary>
      /// Constructor
      /// </summary>
      public PrintJobStatusMonitor()
      {
         InitializeComponent();
      }
      #endregion

      #region Delegates
      public delegate void DoUpdateOutputDelegate(string output);
      #endregion

      #region Event Handlers
      #region Form Event Handlers
      /// <summary>
      /// Called when the user opens the program.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintJobStatusMonitor_Load(object sender, EventArgs e)
      {
         // Create and start a new BarTender Print Engine.
         try
         {
            engine = new Engine(true);
         }
         catch (PrintEngineException exception)
         {
            // If the engine is unable to start, a PrintEngineException will be thrown.
            MessageBox.Show(this, exception.Message, appName);
            this.Close(); // Close this app. We cannot run without connection to an engine.
            return;
         }

         // Sign up for print job events.
         engine.JobCancelled += new EventHandler<PrintJobEventArgs>(Engine_JobCancelled);
         engine.JobErrorOccurred += new EventHandler<PrintJobEventArgs>(Engine_JobErrorOccurred);
         engine.JobMonitorErrorOccurred += new EventHandler<MonitorErrorEventArgs>(Engine_JobMonitorErrorOccurred);
         engine.JobPaused += new EventHandler<PrintJobEventArgs>(Engine_JobPaused);
         engine.JobQueued += new EventHandler<PrintJobEventArgs>(Engine_JobQueued);
         engine.JobRestarted += new EventHandler<PrintJobEventArgs>(Engine_JobRestarted);
         engine.JobResumed += new EventHandler<PrintJobEventArgs>(Engine_JobResumed);
         engine.JobSent += new EventHandler<JobSentEventArgs>(Engine_JobSent);

         // Get the list of printers
         Printers printers = new Printers();
         foreach (Printer printer in printers)
         {
            cboPrinters.Items.Add(printer.PrinterName);
         }

         if (printers.Count > 0)
         {
            // Automatically select the default printer.
            cboPrinters.SelectedItem = printers.Default.PrinterName;
         }

         // Create a uniquely named, temporary file on disk for our thumbnails.
         thumbnailFile = Path.GetTempFileName();
      }

      /// <summary>
      /// Called when the user closes the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintJobStatusMonitor_FormClosed(object sender, FormClosedEventArgs e)
      {
         // Quit the BarTender Print Engine but do not save changes to any open formats.
         if (engine != null)
            engine.Stop(SaveOptions.DoNotSaveChanges);

         // Remove our temporary thumbnail file.
         if (thumbnailFile.Length != 0)
            File.Delete(thumbnailFile);
      }

      /// <summary>
      /// Open the open file dialog and let the user choose a format to open.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnOpen_Click(object sender, EventArgs e)
      {
         DialogResult result = openFileDialog.ShowDialog();
         if (result == DialogResult.OK)
         {
            // Close the previous format
            if (format != null)
               format.Close(SaveOptions.DoNotSaveChanges);

            //If the user selected a format, open it in BarTender.
            txtFilename.Text = openFileDialog.FileName;

            try
            {
               format = engine.Documents.Open(openFileDialog.FileName);
            }
            catch (System.Runtime.InteropServices.COMException comException)
            {
               MessageBox.Show(this, String.Format("Unable to open format: {0}\nReason: {1}", openFileDialog.FileName, comException.Message), appName);
               format = null;
            }

            // Only allow print button if we successfully loaded the format.
            btnPrint.Enabled = (format != null);

            if (format != null)
            {
               // Select the printer in use by the format.
               cboPrinters.SelectedItem = format.PrintSetup.PrinterName;

               //Generate a thumbnail for it.
               format.ExportImageToFile(thumbnailFile, ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(picThumbnail.Width, picThumbnail.Height), OverwriteOptions.Overwrite);
               picThumbnail.ImageLocation = thumbnailFile;
            }
            else
            {
               // Clear any previous image.
               picThumbnail.ImageLocation = "";
            }
         }
      }

      /// <summary>
      /// Print the format. (Actually, tell our thread to print it.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPrint_Click(object sender, EventArgs e)
      {
         // Disable the button until we are finished printing.
         btnPrint.Enabled = false;

         // Get the printer from the dropdown and assign it to the format.
         format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString();

         // Have the status updater print the format so we can watch for status messages
         statusUpdater.RunWorkerAsync(format);
      }
      #endregion

      #region Print Job Event Handlers
      /// <summary>
      /// Handle the print events.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="printJob"></param>
      void Engine_JobSent(object sender, JobSentEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         if (printJob.JobPrintingVerified)
            lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Sent/Print Verified on {1}.", printJob.Name, printJob.PrinterInfo.Name)});
         else
            lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Sent to {1}.", printJob.Name, printJob.PrinterInfo.Name)});
      }
      void Engine_JobResumed(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Resumed.", printJob.Name) });
      }
      void Engine_JobRestarted(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Restarted on {1}.", printJob.Name, printJob.PrinterInfo.Name)});
      }
      void Engine_JobQueued(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Queued on {1}.", printJob.Name, printJob.PrinterInfo.Name)});
      }
      void Engine_JobPaused(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Paused.", printJob.Name) });
      }
      void Engine_JobMonitorErrorOccurred(object sender, MonitorErrorEventArgs errorInfo)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("Job Monitor Error {0}.", errorInfo.Message) });
      }
      void Engine_JobErrorOccurred(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Error {1}.", printJob.Name, printJob.PrinterInfo.Message) });
      }
      void Engine_JobCancelled(object sender, PrintJobEventArgs printJob)
      {
         DoUpdateOutputDelegate doUpdateOutputDelegate = new DoUpdateOutputDelegate(DoUpdateOutput);
         lstJobStatus.Invoke(doUpdateOutputDelegate, new object[] { string.Format("PrintJob {0} Cancelled.", printJob.Name) });
      }
      #endregion

      #region Status Updater background thread
      /// <summary>
      /// This method updates the user interface with the received messages from
      /// the print job.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void statusUpdater_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         if (e.UserState.GetType() == typeof(Messages))
         {
            Messages messages = (Messages)e.UserState;
            string text;
            foreach (Seagull.BarTender.Print.Message message in messages)
            {
               // Let's remove any carriage returns and linefeeds since
               // we are putting each message on a single line.
               text = message.Text.Replace('\n', ' ');
               text = text.Replace('\r', ' ');
               lstMessages.Items.Add(text);
            }
         }

         // If we are finished printing, re-enable the print button.
         if (e.ProgressPercentage == 100)
            btnPrint.Enabled = true;
      }

      /// <summary>
      /// Print the format and report the messages.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void statusUpdater_DoWork(object sender, DoWorkEventArgs e)
      {
         LabelFormatDocument format = (LabelFormatDocument)e.Argument;

         // Create a messages object to be filled up by Print.
         Messages messages;

         // Print the Format.
         format.Print(appName, System.Threading.Timeout.Infinite, out messages);

         // Report that the printjob is done so we can re-enable the print button.
         statusUpdater.ReportProgress(100, messages);
      }
      #endregion
      #endregion

      #region Methods
      /// <summary>
      /// Add our status string to the list box.
      /// </summary>
      /// <param name="output"></param>
      private void DoUpdateOutput(string output)
      {
         lstJobStatus.Items.Add(output);
      }
      #endregion
   }
}
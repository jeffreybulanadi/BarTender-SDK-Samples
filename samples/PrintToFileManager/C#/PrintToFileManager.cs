using System;
using System.ComponentModel;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace PrintToFileManager
{
   // PrintToFileManager Sample
   // This sample shows the steps taken to print to file and print the printer file to a printer.
   // 
   // Step 1: Due to BarTender's licensing restrictions, a printing license must be obtained before 
   // printing to file. This printing license needs to be obtained using the computer that will 
   // eventually print the printer file, and must pass in the printer that will print the file. The
   // BarTenderWebClientRuntime is used to obtain the printing license.
   // 
   // Step 2: The computer that prints to file needs to use the printing license in step 1 and
   // print to a printer with the same or a compatible driver.
   // 
   // Step 3: The computer that will print the printer file now needs to print it using the 
   // BarTenderWebClientRuntime and specify the original printer that was used to obtain the license. 
   // 
   // This sample is intended to show how to:
   //  -Print to file
   //  -Obtain a printer license
   //  -Print a printer file to a printer.
   //  -Open a Format in BarTender
   //  -Get a list of printers and set the printer on a format.
   //  -Start and Stop the BarTender engine
   public partial class PrintToFileManager : Form
   {
      #region Fields
      // Common strings.
      private const string appName = "Print To File Manager";

      private Engine engine = null; // The BarTender Print Engine.
      private LabelFormatDocument format = null; // The format that will be printed to file.
      private BarTenderPrintClient.Printer printClient;
      private Result backgroundPrintingThreadResult;
      private string printToFileThreadResultString;
      private string printThreadResultString;
      private Messages printToFileMessages = null;
      #endregion

      #region Constructor
      /// <summary>
      /// Constructor
      /// </summary>
      public PrintToFileManager()
      {
         InitializeComponent();
      }
      #endregion

      #region Form Event Handlers
      /// <summary>
      /// Called when the user opens the program.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintToFileManager_Load(object sender, EventArgs e)
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

         // Get the list of printers.
         Printers printers = new Printers();
         foreach (Printer printer in printers)
         {
            cboStep1Printer.Items.Add(printer.PrinterName);
            cboStep2Printer.Items.Add(printer.PrinterName);
            cboStep3Printer.Items.Add(printer.PrinterName);
         }

         if (printers.Count > 0)
         {
            // Automatically select the default printer.
            cboStep1Printer.SelectedItem = printers.Default.PrinterName;
            cboStep2Printer.SelectedItem = printers.Default.PrinterName;
            cboStep3Printer.SelectedItem = printers.Default.PrinterName;
         }

         // Create the BarTenderWebClientRuntime.Printers object for print file licensing.
         printClient = new BarTenderPrintClient.Printer();
      }

      /// <summary>
      /// Called when the user closes the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintToFileManager_FormClosed(object sender, FormClosedEventArgs e)
      {
         // Quit the BarTender Print Engine, but do not save changes to any open formats.
         if (engine != null)
            engine.Stop(SaveOptions.DoNotSaveChanges);
      }

      /// <summary>
      /// Step 1: Generate a license key using the printer that will be used to print the final prinjob.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnGenerateLicense_Click(object sender, EventArgs e)
      {
         // Get the license and show it in a text box.
         string printLicenseKey;
         if (printClient.CreatePrintToFileLicense(cboStep1Printer.SelectedItem.ToString(), out printLicenseKey))
         {
            txtPrintLicense.Text = printLicenseKey;

            // If we have a format file name and a license then we can do the print to file.
            if (txtLabelFilename.Text.Length != 0)
               btnPrintToFile.Enabled = true;
         }
         else
         {
            MessageBox.Show(this, printLicenseKey, appName);
            txtPrintLicense.Text = "";
            btnPrintToFile.Enabled = false;
         }
      }

      /// <summary>
      /// Step 2: Opens the open file dialog and lets the user choose a format to open.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnOpenBTW_Click(object sender, EventArgs e)
      {
         if (openBTWDialog.ShowDialog() == DialogResult.OK)
         {
            // Close the previous format
            if (format != null)
               format.Close(SaveOptions.DoNotSaveChanges);

            // If the user selected a format, open it in BarTender.
            try
            {
               format = engine.Documents.Open(openBTWDialog.FileName);
            }
            catch (System.Runtime.InteropServices.COMException comException)
            {
               MessageBox.Show(this, String.Format("Unable to open format: {0}\nReason: {1}", openBTWDialog.FileName, comException.Message), appName);
               format = null;
            }
            if (format != null)
            {
               txtLabelFilename.Text = openBTWDialog.FileName;
               cboStep2Printer.SelectedItem = format.PrintSetup.PrinterName;

               // If we have a format file name and a license then we can do the print to file.
               if (txtPrintLicense.Text.Length != 0)
                  btnPrintToFile.Enabled = true;
            }
            else
            {
               txtLabelFilename.Text = "";
               btnPrintToFile.Enabled = false;
            }
         }
      }

      /// <summary>
      /// Step 2: Prints the format to file when the print button is clicked.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPrintToFile_Click(object sender, EventArgs e)
      {
         if (savePrinterFileDialog.ShowDialog() == DialogResult.OK)
         {
            // Let's disable the button so that the user can't repeatedly
            // hit it causing many threads.
            btnPrintToFile.Enabled = false;

            // Get the printer from the dropdown and assign it to the format.
            format.PrintSetup.PrinterName = cboStep2Printer.SelectedItem.ToString();

            // Set the selected filename to the printerfile property of the format's PrintSetup.
            format.PrintSetup.PrintToFileName  = savePrinterFileDialog.FileName;

            // Set the format to print to file.
            format.PrintSetup.PrintToFile = true;

            // Set the printing license.
            format.PrintSetup.PrintToFileLicense = txtPrintLicense.Text;

            // Do the printing in a thread so our user interface can still operate.
            BackgroundWorker backgroundPrinter = new BackgroundWorker();
            backgroundPrinter.DoWork += new DoWorkEventHandler(backgroundPrinter_DoWork);
            backgroundPrinter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundPrinter_RunWorkerCompleted);

            backgroundPrinter.RunWorkerAsync(format);
         }
      }

      /// <summary>
      /// Step 3: Select a printer file.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnOpenPrinterFile_Click(object sender, EventArgs e)
      {
         if (openPrinterFileDialog.ShowDialog() == DialogResult.OK)
         {
            txtPrinterFileFilename.Text = openPrinterFileDialog.FileName;

            btnPrintPrinterFile.Enabled = true;
         }
      }

      /// <summary>
      /// Step 3: Print the selected printer file using a background thread.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPrintPrinterFile_Click(object sender, EventArgs e)
      {
         // Let's disable the button so that the user can't repeatedly
         // hit it causing many threads.
         btnPrintPrinterFile.Enabled = false;

         BackgroundWorker backgroundFilePrinter = new BackgroundWorker();
         backgroundFilePrinter.DoWork += new DoWorkEventHandler(backgroundFilePrinter_DoWork);
         backgroundFilePrinter.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundFilePrinter_RunWorkerCompleted);

         backgroundFilePrinter.RunWorkerAsync(cboStep3Printer.SelectedItem.ToString());
      }

      /// <summary>
      /// When we select the printer to obtain a printer license in step 1, we
      /// need to select the same printer in the other 2 lists because that
      /// will most likely be the printer used.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void cboStep1Printer_SelectedIndexChanged(object sender, EventArgs e)
      {
         cboStep2Printer.SelectedItem = cboStep1Printer.SelectedItem;
         cboStep3Printer.SelectedItem = cboStep1Printer.SelectedItem;
      }
      #endregion

      #region Background Printing Thread
      /// <summary>
      /// Prints the format to file in a separate thread so the UI will still update while it is printing.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundPrinter_DoWork(object sender, DoWorkEventArgs e)
      {
         LabelFormatDocument format = (LabelFormatDocument)e.Argument;

         printThreadResultString = "";
         backgroundPrintingThreadResult = Result.Failure;
         try
         {
            // Print the Format.
            backgroundPrintingThreadResult = format.Print(appName, Timeout.Infinite, out printToFileMessages);
         }
         catch (Seagull.BarTender.Print.PrintEngineException ex)
         {
            printThreadResultString = ex.Message;
         }
         catch (Exception ex)
         {
            printThreadResultString = ex.Message;
         }
      }

      /// <summary>
      /// Re-enables the print button when the print to file is completed.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void backgroundPrinter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         // Report the messages from the print job.
         if (printThreadResultString != "")
            MessageBox.Show(this, printThreadResultString, appName);
         else
            for (int index = 0; index < printToFileMessages.Count; ++index)
            {
               MessageBox.Show(this, printToFileMessages[index].Text, appName);
            }

         // Restore the print button.
         btnPrintToFile.Enabled = true;

         if (backgroundPrintingThreadResult == Result.Success)
         {
            txtPrinterFileFilename.Text = format.PrintSetup.PrintToFileName;
            btnPrintPrinterFile.Enabled = true;
         }
      }
      #endregion

      #region Background Print Printer File Thread
      /// <summary>
      /// Send a printer file to a printer.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void backgroundFilePrinter_DoWork(object sender, DoWorkEventArgs e)
      {
         // Read in the printer file.
         StreamReader sr = new StreamReader(txtPrinterFileFilename.Text, System.Text.Encoding.Default);
         string printerFileData = sr.ReadToEnd();
         sr.Close();

         // Print the printer file data to the selected printer. SendDataToPrinter returns
         // 1 for success and 0 for failure.
         if (printClient.SendPrintCode(e.Argument.ToString(), "Test Print Job", printerFileData))
            printToFileThreadResultString = string.Format("Spooling Succeeded\n\n{0} was successfully spooled to {1}.",
                                                          txtPrinterFileFilename.Text,
                                                          e.Argument.ToString());
         else
            printToFileThreadResultString = "Printing Problem\n\nThere has been a problem printing. There may be a problem " +
                                            "with the printer or the print license may be incorrect. The print license " +
                                            "times out after 10 minutes, so you may have to print to file again.";
      }

      /// <summary>
      /// Restore the print button.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void backgroundFilePrinter_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         MessageBox.Show(this, printToFileThreadResultString, appName);
         btnPrintPrinterFile.Enabled = true;
      }
      #endregion
   }
}
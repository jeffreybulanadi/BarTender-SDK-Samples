using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Seagull.BarTender.Print;

namespace PrintPreview
{
   /// <summary>
   /// Print Preview Sample
   /// This sample allows the user to open a format, select a printer, and view how the printed
   /// format will look using the selected printer.
   /// 
   /// This sample is intended to show how to:
   ///  -Export Print Previews
   ///  -Open a Format in BarTender
   ///  -Get a list of printers and set the printer on a format.
   ///  -Start and Stop the BarTender engine.
   /// </summary>
   public partial class PrintPreview : Form
   {
      #region Fields
      // Common strings.
      private const string appName = "Print Preview";

      private Engine engine = null; // The BarTender Print Engine.
      private LabelFormatDocument format = null; // The format that will be exported.
      private string previewPath = ""; // The path to the folder where the previews will be exported.
      private int currentPage = 1; // The current page being viewed.
      private int totalPages; // Number of pages.
      private Messages messages;
      #endregion

      #region Constructor
      /// <summary>
      /// Constructor
      /// </summary>
      public PrintPreview()
      {
         InitializeComponent();
      }
      #endregion

      #region Form Event Handlers
      /// <summary>
      /// Start BarTender Print Engine, Initialize printer list,
      /// enable/disable controls, setup temporary folder for images.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintPreview_Load(object sender, EventArgs e)
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

         // Hide/Disable preview controls.
         DisablePreview();

         // Create a temporary folder to hold the images.
         string tempPath = Path.GetTempPath(); // Something like "C:\Documents and Settings\<username>\Local Settings\Temp\"
         string newFolder;

         // It's not likely we'll have a path that already exists, but we'll check for it anyway.
         do
         {
            newFolder = Path.GetRandomFileName();
            previewPath = tempPath + newFolder; // newFolder is something crazy like "gulvwdmt.3r4"
         } while (Directory.Exists(previewPath));
         Directory.CreateDirectory(previewPath);
      }

      /// <summary>
      /// Stop the BarTender Print Engine and delete our temporary images.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void PrintPreview_FormClosed(object sender, FormClosedEventArgs e)
      {
         // Quit the BarTender Print Engine but do not save changes to any open formats.
         if (engine != null)
            engine.Stop(SaveOptions.DoNotSaveChanges);

         // Remove the temporary path and all the images.
         if (previewPath.Length != 0)
            Directory.Delete(previewPath, true);
      }

      /// <summary>
      /// Opens the open file dialog and lets the user choose a format to open.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnOpen_Click(object sender, EventArgs e)
      {
         // Let's disable some controls until we are done.
         btnOpen.Enabled = false;
         cboPrinters.Enabled = false;
         btnPreview.Enabled = false;

         DialogResult result = openFileDialog.ShowDialog();
         if (result == DialogResult.OK)
         {
            Cursor.Current = Cursors.WaitCursor;

            // Close the previous format.
            if (format != null)
               format.Close(SaveOptions.DoNotSaveChanges);

            // We need to delete the files associated with the last format.
            string[] files = Directory.GetFiles(previewPath);
            foreach (string file in files)
               File.Delete(file);

            // Put the UI back into a non-preview state.
            DisablePreview();

            // Open the format.
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

            // Only allow preview button if we successfully loaded the format.
            btnPreview.Enabled = (format != null);

            if (format != null)
            {
               // Select the printer in use by the format.
               cboPrinters.SelectedItem = format.PrintSetup.PrinterName;
            }

            Cursor.Current = Cursors.Default;
         }
         // Restore some controls.
         btnOpen.Enabled = true;
         cboPrinters.Enabled = true;
      }

      /// <summary>
      /// Runs when the Preview button is pressed. Disables controls and starts
      /// BarTender working to export print preview images.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPreview_Click(object sender, EventArgs e)
      {
         // Disable some controls until we are finished creating previews.
         btnOpen.Enabled = false;
         cboPrinters.Enabled = false;
         btnPreview.Enabled = false;

         DisablePreview();

         // Get the printer from the dropdown and assign it to the format.
         format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString();

         // Set control states to show working. These will be reset when the work completes.
         picUpdating.Visible = true;

         // Have the background worker export the BarTender format.
         backgroundWorker.RunWorkerAsync(format);
      }

      /// <summary>
      /// Show the preview of the first label.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnFirst_Click(object sender, EventArgs e)
      {
         currentPage = 1;
         ShowPreview();
      }

      /// <summary>
      /// Show the preview of the previous label.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPrev_Click(object sender, EventArgs e)
      {
         --currentPage;
         ShowPreview();
      }

      /// <summary>
      /// Show the preview of the next label.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnNext_Click(object sender, EventArgs e)
      {
         ++currentPage;
         ShowPreview();
      }

      /// <summary>
      /// Show the preview of the last label.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnLast_Click(object sender, EventArgs e)
      {
         currentPage = totalPages;
         ShowPreview();
      }
      #endregion

      #region Print Preview Thread Methods
      /// <summary>
      /// Runs in a separate thread to allow BarTender to export the preview while allowing UI updates.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         LabelFormatDocument format = (LabelFormatDocument)e.Argument;

         // Delete any existing files.
         string[] oldFiles = Directory.GetFiles(previewPath, "*.*");
         for (int index = 0; index < oldFiles.Length; ++index)
            File.Delete(oldFiles[index]);

         // Export the format's print previews.
         format.ExportPrintPreviewToFile(previewPath, "PrintPreview%PageNumber%.jpg", ImageType.JPEG, Seagull.BarTender.Print.ColorDepth.ColorDepth24bit, new Resolution(picPreview.Width, picPreview.Height), System.Drawing.Color.White, OverwriteOptions.Overwrite, true, true, out messages);
         string[] files = Directory.GetFiles(previewPath, "*.*");
         totalPages = files.Length;
      }

      /// <summary>
      /// We are done exporting the preview, so let's show any messages
      /// and display the first label.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         // Report any messages.
         if (messages != null)
         {
            if (messages.Count > 5)
               MessageBox.Show(this, "There are more than 5 messages from the print preview. Only the first 5 will be displayed.", appName);
            int count = 0;
            foreach (Seagull.BarTender.Print.Message message in messages)
            {
               MessageBox.Show(this, message.Text, appName);
               if (++count >= 5)
                  break;
            }
         }

         picUpdating.Visible = false;

         btnOpen.Enabled = true;
         btnPreview.Enabled = true;
         cboPrinters.Enabled = true;

         // Only enable the preview if we actual got some pages.
         if (totalPages != 0)
         {
            lblNumPreviews.Visible = true;

            currentPage = 1;
            ShowPreview();
         }
      }
      #endregion

      #region Methods
      /// <summary>
      /// Disable/Hide preview controls.
      /// </summary>
      private void DisablePreview()
      {
         picPreview.ImageLocation = "";
         picPreview.Visible = false;

         btnPrev.Enabled = false;
         btnFirst.Enabled = false;
         lblNumPreviews.Visible = false;
         btnNext.Enabled = false;
         btnLast.Enabled = false;
      }

      /// <summary>
      /// Show the preview of the current page.
      /// </summary>
      private void ShowPreview()
      {
         // Our current preview number shouldn't be out of range,
         // but we'll practice good programming by checking it.
         if ((currentPage < 1) || (currentPage > totalPages))
            currentPage = 1;

         // Update the page label and the preview Image.
         lblNumPreviews.Text = string.Format("Page {0} of {1}", currentPage, totalPages);
         string filename = string.Format("{0}\\PrintPreview{1}.jpg", previewPath, currentPage);

         picPreview.ImageLocation = filename;
         picPreview.Visible = true;

         // Enable or Disable controls as needed.
         if (currentPage == 1)
         {
            btnPrev.Enabled = false;
            btnFirst.Enabled = false;
         }
         else
         {
            btnPrev.Enabled = true;
            btnFirst.Enabled = true;
         }

         if (currentPage == totalPages)
         {
            btnNext.Enabled = false;
            btnLast.Enabled = false;
         }
         else
         {
            btnNext.Enabled = true;
            btnLast.Enabled = true;
         }
      }
      #endregion
   }
}
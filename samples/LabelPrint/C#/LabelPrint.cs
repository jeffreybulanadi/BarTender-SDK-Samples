using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Seagull.BarTender.Print;

using System.Diagnostics;

namespace LabelPrint
{
   /// <summary>
   /// Label Print Sample
   /// This sample allows the user to open a format, select a printer, set substrings, and print.
   ///
   /// This sample is intended to show how to:
   ///  -Generate quick thumbnails without first opening a format in BarTender.
   ///  -Open a Format in BarTender.
   ///  -Get a list of printers and set the printer on a format.
   ///  -Set a DataGridView to use the SubStrings collection as a DataSource.
   ///  -Get and Set the number of serialized copies and the number of identical copies.
   ///  -Start and Stop a BarTender engine.
   /// </summary>
   public partial class LabelPrint : Form
   {
      #region Fields
      // Common strings.
      private const string appName = "Label Print";
      private const string dataSourced = "Data Sourced";

      private Engine engine = null; // The BarTender Print Engine
      private LabelFormatDocument format = null; // The currently open Format
      private bool isClosing = false; // Set to true if we are closing. This helps discontinue thumbnail loading.

      // Label browser
      private string[] browsingFormats; // The list of filenames in the current folder
      Hashtable listItems; // A hash table containing ListViewItems and indexed by format name.
                           // It keeps track of what formats have had their image loaded.
      Queue<int> generationQueue; // A queue containing indexes into browsingFormats
                                  // to facilitate the generation of thumbnails

      // Label browser indexes.
      int topIndex; // The top visible index in the lstLabelBrowser
      int selectedIndex; // The selected index in the lstLabelBrowser
      #endregion

      #region Enumerations
      // Indexes into our image list for the label browser.
      enum ImageIndex { LoadingFormatImage = 0, FailureToLoadFormatImage = 1 };
      #endregion

      #region Constructor
      /// <summary>
      /// Constructor
      /// </summary>
      public LabelPrint()
      {
         InitializeComponent();
      }
      #endregion

      #region Delegates
      delegate void DelegateShowMessageBox(string message);
      #endregion

      #region Event Handlers
      #region Form Event Handlers
      /// <summary>
      /// Called when the user opens the program.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void LabelPrint_Load(object sender, EventArgs e)
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

         // Setup the images used in the label browser.
         lstLabelBrowser.View = View.LargeIcon;
         lstLabelBrowser.LargeImageList = new ImageList();
         lstLabelBrowser.LargeImageList.ImageSize = new Size(100, 100);
         lstLabelBrowser.LargeImageList.Images.Add(Properties.Resources.LoadingFormat);
         lstLabelBrowser.LargeImageList.Images.Add(Properties.Resources.LoadingError);

         // Initialize a list and a queue.
         listItems = new System.Collections.Hashtable();
         generationQueue = new Queue<int>();

         // Limit the identical copies and serialized copies to 9
         // to match the user interface behavior of BarTender.
         txtIdenticalCopies.MaxLength = 9;
         txtSerializedCopies.MaxLength = 9;
      }

      /// <summary>
      /// Called when the user closes the application.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void LabelPrint_FormClosed(object sender, FormClosedEventArgs e)
      {
         isClosing = true;

         // Make sure the thumbnail worker is stopped before we try closing BarTender or 
         // there might be problems in the worker.
         thumbnailCacheWorker.CancelAsync();
         while (thumbnailCacheWorker.IsBusy)
         {
            Application.DoEvents();
         };

         // Quit the BarTender Print Engine, but do not save changes to any open formats.
         if (engine != null)
            engine.Stop(SaveOptions.DoNotSaveChanges);
      }

      /// <summary>
      /// Called when the user presses the open button.
      /// Gets a list of formats in the selected folder.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnOpen_Click(object sender, EventArgs e)
      {
         folderBrowserDialog.Description = "Select a folder that contains BarTender formats.";
         DialogResult result = folderBrowserDialog.ShowDialog();
         if (result == DialogResult.OK)
         {
            lock (generationQueue)
            {
               generationQueue.Clear();
            }
            listItems.Clear();

            txtFolderPath.Text = folderBrowserDialog.SelectedPath;
            btnPrint.Enabled = false;

            browsingFormats = System.IO.Directory.GetFiles(txtFolderPath.Text, "*.btw");

            // Setting the VirtualListSize will cause lstLabelBrowser_RetrieveVirtualItem to be called.
            lstLabelBrowser.VirtualListSize = browsingFormats.Length;
         }
      }

      /// <summary>
      /// Prints the currently loaded format using the selected printer.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void btnPrint_Click(object sender, EventArgs e)
      {
         // We lock on the engine here so we don't lose our format object
         // if the user were to click on a format that wouldn't load.
         lock (engine)
         {
            bool success = true;

            // Assign number of identical copies if it is not datasourced.
            if (format.PrintSetup.SupportsIdenticalCopies)
            {
               int copies = 1;
               success = Int32.TryParse(txtIdenticalCopies.Text, out copies) && (copies >= 1);
               if (!success)
                  MessageBox.Show(this, "Identical Copies must be an integer greater than or equal to 1.", appName);
               else
               {
                  format.PrintSetup.IdenticalCopiesOfLabel = copies;
               }
            }

            // Assign number of serialized copies if it is not datasourced.
            if (success && (format.PrintSetup.SupportsSerializedLabels))
            {
               int copies = 1;
               success = Int32.TryParse(txtSerializedCopies.Text, out copies) && (copies >= 1);
               if (!success)
               {
                  MessageBox.Show(this, "Serialized Copies must be an integer greater than or equal to 1.", appName);
               }
               else
               {
                  format.PrintSetup.NumberOfSerializedLabels = copies;
               }
            }

            // If there are no incorrect values in the copies boxes then print.
            if (success)
            {
               Cursor.Current = Cursors.WaitCursor;

               // Get the printer from the dropdown and assign it to the format.
               if (cboPrinters.SelectedItem != null)
                  format.PrintSetup.PrinterName = cboPrinters.SelectedItem.ToString();

               Messages messages;
               int waitForCompletionTimeout = 10000; // 10 seconds
               Result result = format.Print(appName, waitForCompletionTimeout, out messages);
               string messageString = "\n\nMessages:";

               foreach (Seagull.BarTender.Print.Message message in messages)
               {
                  messageString += "\n\n" + message.Text;
               }

               if (result == Result.Failure)
                  MessageBox.Show(this, "Print Failed" + messageString, appName);
               else
                  MessageBox.Show(this, "Label was successfully sent to printer." + messageString, appName);
            }
         }
      }
      #endregion

      #region List View Event Handlers
      /// <summary>
      /// Called when the user clicks on an item in the format browser.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lstLabelBrowser_SelectedIndexChanged(object sender, EventArgs e)
      {
         if (lstLabelBrowser.SelectedIndices.Count == 1)
         {
            selectedIndex = lstLabelBrowser.SelectedIndices[0];
            EnableControls(false);
            picUpdatingFormat.Visible = true;

            // Start a BackgroundWorker to load the format to extract the copies
            // data and substrings. Then update the UI with the data.
            BackgroundWorker formatLoader = new BackgroundWorker();
            formatLoader.DoWork += new DoWorkEventHandler(formatLoader_DoWork);
            formatLoader.RunWorkerCompleted += new RunWorkerCompletedEventHandler(formatLoader_RunWorkerCompleted);
            formatLoader.RunWorkerAsync(selectedIndex);
         }
         else if (lstLabelBrowser.SelectedIndices.Count == 0)
         {
            EnableControls(false);
            picUpdatingFormat.Visible = false;
         }
      }
      /// <summary>
      /// Called when the format browser listbox scrolls a listitem into view.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      private void lstLabelBrowser_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
      {
         int btwFileIndex = e.ItemIndex;

         // If our list doesn't have this item then add it and set the
         // image index to "loading" so that the image will be loaded.
         if (listItems[browsingFormats[btwFileIndex]] == null)
         {
            e.Item = new ListViewItem(browsingFormats[btwFileIndex]);
            e.Item.ImageIndex = (int)ImageIndex.LoadingFormatImage;
            listItems.Add(browsingFormats[btwFileIndex], e.Item);
         }
         else
         {
            e.Item = (ListViewItem)listItems[browsingFormats[btwFileIndex]];
         }

         // Add the index to the queue so that the thumbnail thread can get the image.
         if (e.Item.ImageIndex == (int)ImageIndex.LoadingFormatImage)
         {
            lock (generationQueue)
            {
               if (!generationQueue.Contains(btwFileIndex))
                  generationQueue.Enqueue(btwFileIndex);
            }
         }

         // If we put anything on the queue, start the thumbnail worker if it's not already started.
         if (!thumbnailCacheWorker.IsBusy && (generationQueue.Count > 0))
         {
            thumbnailCacheWorker.RunWorkerAsync();
         }
      }

      /// <summary>
      /// Called when the format browser listbox needs to cache listitems that may appear soon.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="cacheEvent"></param>
      private void lstLabelBrowser_CacheVirtualItems(object sender, CacheVirtualItemsEventArgs cacheEvent)
      {
         topIndex = cacheEvent.StartIndex;

         lock (generationQueue)
         {
            for (int index = cacheEvent.StartIndex; index <= cacheEvent.EndIndex; ++index)
            {
               ListViewItem listViewItem = (ListViewItem)listItems[browsingFormats[index]];

               if ((listViewItem != null) && (listViewItem.ImageIndex == (int)ImageIndex.LoadingFormatImage))
               {
                  if (!generationQueue.Contains(index))
                     generationQueue.Enqueue(index);
               }
            }
         }

         // If we put anything on the queue, start the thumbnail worker if it's not already started.
         if (!thumbnailCacheWorker.IsBusy && (generationQueue.Count > 0))
         {
            thumbnailCacheWorker.RunWorkerAsync();
         }
      }
      #endregion

      #region Thumbnail Event Handlers
      /// <summary>
      /// Called when a thumbnail was loaded so it can be shown.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void thumbnailCacheWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
      {
         object[] args = e.UserState as object[];
         ListViewItem item = (ListViewItem)args[0];

         // 100 means we got the image successfully.
         if (e.ProgressPercentage == 100)
         {
            Image thumbnail = (Image)args[1];

            lstLabelBrowser.LargeImageList.Images.Add(item.Text, thumbnail);
            item.ImageIndex = lstLabelBrowser.LargeImageList.Images.IndexOfKey(item.Text);
         }
         else if (e.ProgressPercentage == 0) // 0 means we did not successfully get the format image.
         {
            item.ImageIndex = (int)ImageIndex.FailureToLoadFormatImage;
         }
         item.ListView.Invalidate(item.Bounds);
      }

      /// <summary>
      /// Loads thumbnails in a background thread so the UI doesn't hang.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void thumbnailCacheWorker_DoWork(object sender, DoWorkEventArgs e)
      {
         BackgroundWorker worker = (BackgroundWorker)sender;

         int index;
         string fileName;

         // Loop until the queue of items that need to be loaded is empty or the worker was cancelled.
         while ((generationQueue.Count > 0) && !worker.CancellationPending && !isClosing)
         {
            lock (generationQueue)
            {
               // Get the index to use.
               index = generationQueue.Dequeue();
            }

            // If this is way out of our view don't bother generating it.
            if (Math.Abs(index - topIndex) < 30)
            {
               fileName = browsingFormats[index];

               // Check to see if the listitem is already generated and just waiting for a thumbnail.
               ListViewItem item = (ListViewItem)listItems[fileName];
               if (item == null)
               {
                  item = new ListViewItem(fileName);
                  item.ImageIndex = (int)ImageIndex.LoadingFormatImage;
                  listItems.Add(fileName, item);
               }

               // If the listitem doesn't already have a thumbnail, generate it.
               if (item.ImageIndex == (int)ImageIndex.LoadingFormatImage)
               {
                  try
                  {
                     Image btwImage = LabelFormatThumbnail.Create(fileName, Color.Gray, 100, 100);

                     object[] progressReport = new object[] { item, btwImage };
                     worker.ReportProgress(100, progressReport);
                  }
                  // If the file isn't an actual btw format file we will get an exception.
                  catch
                  {
                     object[] progressReport = new object[] { item, null };
                     worker.ReportProgress(0, progressReport);
                  }
               }
            }
         }
      }
      #endregion

      #region Format Loader Event Handlers
      /// <summary>
      /// Loads a format in a separate thread so it doesn't hang the UI.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void formatLoader_DoWork(object sender, DoWorkEventArgs e)
      {
         int index = (int)e.Argument;
         string errorMessage = "";

         // We lock the engine here because the engine might still be printing a format.
         lock (engine)
         {
            // Make sure this is still the label the user has selected in case they are clicking around fast.
            if (selectedIndex == index)
            {
               try
               {
                  if (format != null)
                     format.Close(SaveOptions.DoNotSaveChanges);
                  format = engine.Documents.Open(browsingFormats[index]);
               }
               catch (System.Runtime.InteropServices.COMException comException)
               {
                  errorMessage = String.Format("Unable to open format: {0}\nReason: {1}", browsingFormats[index], comException.Message);
                  format = null;
               }
            }
         }
         // We are in a non-UI thread so we need to use Invoke to show our message properly.
         if (errorMessage.Length != 0)
            Invoke(new DelegateShowMessageBox(ShowMessageBox), errorMessage);
      }

      /// <summary>
      /// Called when the format is finished loading.
      /// Fills in the copies boxes and substrings if there are any.
      /// </summary>
      /// <param name="sender"></param>
      /// <param name="e"></param>
      void formatLoader_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
      {
         // We lock on the engine here so that if we have a valid format object
         // we don't lose it while we are using it.
         lock (engine)
         {
            if (format != null)
            {
               EnableControls(true);
               cboPrinters.SelectedItem = format.PrintSetup.PrinterName;
               picUpdatingFormat.Visible = false;
               lblFormatError.Visible = false;

               // Set the number of identical copies.
               if (format.PrintSetup.SupportsIdenticalCopies == false)
               {
                  txtIdenticalCopies.Text = dataSourced;
                  txtIdenticalCopies.ReadOnly = true;
               }
               else
               {
                  txtIdenticalCopies.Text = format.PrintSetup.IdenticalCopiesOfLabel.ToString();
                  txtIdenticalCopies.ReadOnly = false;
               }

               // Set the number of serialized copies.
               if (format.PrintSetup.SupportsSerializedLabels == false)
               {
                  txtSerializedCopies.Text = dataSourced;
                  txtSerializedCopies.ReadOnly = true;
               }
               else
               {
                  txtSerializedCopies.Text = format.PrintSetup.NumberOfSerializedLabels.ToString();
                  txtSerializedCopies.ReadOnly = false;
               }

               // Set the substrings grid.
               if (format.SubStrings.Count > 0)
               {
                  BindingSource bindingSource = new BindingSource();
                  bindingSource.DataSource = format.SubStrings;
                  substringGrid.DataSource = bindingSource;
                  substringGrid.AutoResizeColumns();
                  lblNoSubstrings.Visible = false;
               }
               else
               {
                  lblNoSubstrings.Visible = true;
               }
               substringGrid.Invalidate();
            }
            else // There is no format loaded, it must have errored out.
            {
               picUpdatingFormat.Visible = false;
               lblNoSubstrings.Visible = false;
               lblFormatError.Visible = true;
            }
         }
      }
      #endregion
      #endregion

      #region Methods
      /// <summary>
      /// Enables or disables controls based on whether or not a valid format is open.
      /// </summary>
      /// <param name="enable"></param>
      void EnableControls(bool enable)
      {
         txtIdenticalCopies.Enabled = enable;
         txtSerializedCopies.Enabled = enable;
         btnPrint.Enabled = enable;

         if (!enable)
         {
            txtIdenticalCopies.Text = "";
            txtSerializedCopies.Text = "";
            substringGrid.DataSource = null;
            lblFormatError.Visible = false;
            lblNoSubstrings.Visible = false;
         }
      }

      /// <summary>
      /// Show a message box. We need this method to facilitate showing
      /// messages from a non-UI thread.
      /// </summary>
      /// <param name="message">The message to show.</param>
      void ShowMessageBox(string message)
      {
         MessageBox.Show(this, message, appName);
      }
      #endregion
   }
}
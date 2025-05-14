using System;
using Seagull.BarTender.Print;
using Seagull.BarTender.PrintServer.Tasks;

namespace TaskMaster
{
   /// <summary>
   /// Example of a custom Task.
   /// This task will load the requested format, set some substrings on it, then print the label.
   /// The named substrings are meant to be used with the supplied Address.btw label.
   /// 
   /// To write a custom Task, the OnRun method must be overridden from the base class. This
   /// is the logic that will be run by whichever Print Engine is selected to run the task.
   /// </summary>
   class CustomTask : Task
   {
      LabelFormat format = null;

      /// <summary>
      /// Initialize the LabelFormat object.
      /// </summary>
      /// <param name="labelFormatFileName"></param>
      public CustomTask(string labelFormatFileName)
      {
         format = new LabelFormat(labelFormatFileName);
      }

      /// <summary>
      /// Override this method to perform custom Task logic.
      /// This sample method writes values to the named substrings on the label and then 
      /// prints the label.
      /// </summary>
      /// <returns></returns>
      protected override bool OnRun()
      {
         LabelFormatDocument formatDoc = null;
         try
         {
            // Open a LabelFormatDocument using the LabelFormat that was passed in.
            formatDoc = Engine.Documents.Open(format, out messages);

            // Assign this to the member LabelFormat variable so 
            // it can be accessed after the Task finishes.
            format = formatDoc;

            // Set some substrings on the label
            formatDoc.SubStrings["FirstName"].Value = "John";
            formatDoc.SubStrings["LastName"].Value = "Doe";
            formatDoc.SubStrings["Company"].Value = "Acme Widgets";
            formatDoc.SubStrings["StreetAddress"].Value = "1234 Main Street";
            formatDoc.SubStrings["City"].Value = "Bellevue";
            formatDoc.SubStrings["State"].Value = "WA";
            formatDoc.SubStrings["Zip"].Value = "98005";
            formatDoc.PrintSetup.UseDatabase = false;

            // Print the label
            Messages printMessages;
            formatDoc.Print("", 1000, out printMessages);
            foreach (Message message in printMessages)
            {
               messages.Add(message);
            }

            // Close the LabelFormatDocument to free up resources
            // in the TaskEngine that was used.
            formatDoc.Close(SaveOptions.DoNotSaveChanges);
         }
         catch
         {
            try
            {
               // Attempt to close if the format is still open.
               formatDoc.Close(SaveOptions.DoNotSaveChanges);
            }
            catch (Exception)
            {
            }
            throw;
         }
         return base.OnRun();
      }

      /// <summary>
      /// Override this method to validate that this Task's properties are 
      /// correct when it gets added to the TaskQueue. Typically you would
      /// throw an exception that you would catch during a
      /// TaskManager.TaskQueue.QueueTask call.
      /// </summary>
      protected override void OnValidate()
      {
      }

      /// <summary>
      /// Allow access to the LabelFormat used in this Task.
      /// </summary>
      LabelFormat LabelFormat
      {
         get
         {
            return format;
         }
      }
   }
}

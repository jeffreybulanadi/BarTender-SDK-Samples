<%@ Application Language="C#" %>
<%@ Import Namespace="Seagull.BarTender.PrintServer" %>

<script RunAt="server">
    
   void Application_Start(object sender, EventArgs e)
   {
      TaskManager taskManager = null;
      try
      {
         taskManager = new TaskManager();
         taskManager.Start(1);
         Application["TaskManager"] = taskManager;

         // Get the temporary folder where all print previews are stored and 
         // store it into an application variable for later use in Session_End.
         string tempFolder = ConfigurationManager.AppSettings["ImageTempFolder"];
         if (string.IsNullOrEmpty(tempFolder))
            tempFolder = "Temp";

         string tempFolderFullPath = Server.MapPath(tempFolder);

         if (!System.IO.Directory.Exists(tempFolderFullPath))
         {
            System.IO.Directory.CreateDirectory(tempFolderFullPath);
         }

         Application["TempFolderFullPath"] = tempFolderFullPath;
      }
      catch (Exception)
      {
         // Something went wrong. Let other modules handle the error appropriately.
         Application["TaskManager"] = null;
         Application["TempFolderFullPath"] = "";
         taskManager.Dispose();
      }
      
   }

   void Application_End(object sender, EventArgs e)
   {
      try
      {
         // Shut down all BarTender print engines and the task manager.
         TaskManager taskManager = (TaskManager)Application["TaskManager"]; 
         if (taskManager != null)
         {
            taskManager.Stop(3000, true);
            taskManager.Dispose();
         }
      }
      catch (Exception)
      {
         // Something went wrong but do nothing. The web application is stopping.
      }
   }

   void Application_Error(object sender, EventArgs e)
   {
     // Code that runs when an unhandled error occurs

   }

   void Session_Start(object sender, EventArgs e)
   {
     // Code that runs when a new session is started

   }

   void Session_End(object sender, EventArgs e)
   {
      // Delete all print preview images starting with Session.SessionID.
      try
      {
         string tempFolderFullPath = (string)Application["TempFolderFullPath"];
         string[] sFileNames = System.IO.Directory.GetFiles(tempFolderFullPath, Session.SessionID + "*.png", System.IO.SearchOption.AllDirectories);

         foreach (string fileName in sFileNames)
         {
            try
            {
               System.IO.File.Delete(fileName);
            }
            catch (Exception)
            {
               // Ignore if not found.
            }
         }
      }
      catch (Exception)
      {
      }
   }
       
</script>


<%@ Application Language="vb" %>
<%@ Import Namespace="Seagull.BarTender.PrintServer" %>

<script RunAt="server">

   Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
	  Dim taskManager As TaskManager = Nothing
	  Try
		 taskManager = New TaskManager()
		 taskManager.Start(1)
		 Application("TaskManager") = taskManager

		 ' Get the temporary folder where all print previews are stored and 
		 ' store it into an application variable for later use in Session_End.
		 Dim tempFolder As String = ConfigurationManager.AppSettings("ImageTempFolder")
		 If String.IsNullOrEmpty(tempFolder) Then
			tempFolder = "Temp"
		 End If

		 Dim tempFolderFullPath As String = Server.MapPath(tempFolder)

		 If (Not System.IO.Directory.Exists(tempFolderFullPath)) Then
			System.IO.Directory.CreateDirectory(tempFolderFullPath)
		 End If

		 Application("TempFolderFullPath") = tempFolderFullPath
	  Catch e1 As Exception
		 ' Something went wrong. Let other modules handle the error appropriately.
		 Application("TaskManager") = Nothing
		 Application("TempFolderFullPath") = ""
		 taskManager.Dispose()
	  End Try

   End Sub

   Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
	  Try
		 ' Shut down all BarTender print engines and the task manager.
		 Dim taskManager As TaskManager = CType(Application("TaskManager"), TaskManager)
		 If taskManager IsNot Nothing Then
			taskManager.Stop(3000, True)
			taskManager.Dispose()
		 End If
	  Catch e1 As Exception
		 ' Something went wrong but do nothing. The web application is stopping.
	  End Try
   End Sub

   Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
	 ' Code that runs when an unhandled error occurs

   End Sub

   Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
	 ' Code that runs when a new session is started

   End Sub

   Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
	  ' Delete all print preview images starting with Session.SessionID.
	  Try
		 Dim tempFolderFullPath As String = CStr(Application("TempFolderFullPath"))
		 Dim sFileNames() As String = System.IO.Directory.GetFiles(tempFolderFullPath, Session.SessionID & "*.png", System.IO.SearchOption.AllDirectories)

		 For Each fileName As String In sFileNames
			Try
			   System.IO.File.Delete(fileName)
			Catch e1 As Exception
			   ' Ignore if not found.
			End Try
		 Next fileName
	  Catch e2 As Exception
	  End Try
   End Sub

</script>
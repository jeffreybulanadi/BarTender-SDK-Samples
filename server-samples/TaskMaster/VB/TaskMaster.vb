Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports Seagull.BarTender.Print
Imports Seagull.BarTender.PrintServer
Imports Seagull.BarTender.PrintServer.Tasks

Namespace TaskMaster
   ''' <summary>
   ''' TaskMaster Sample
   ''' This sample allows the user to select Tasks to run and view the resulting output.
   '''  
   ''' This sample is intended to show how to:
   '''  -Start a number of TaskEngines.
   '''  -Create various Tasks.
   '''  -Submit Tasks to the TaskQueue.
   '''  -Receive events from Tasks to get the output.
   ''' </summary>
   Partial Public Class TaskMaster
	   Inherits Form
	  #Region "Fields"
	  Private taskManager As TaskManager
	  Private ReadOnly labelFilename As String
	  Private Const appName As String = "Task Manager"
	  Private thumbnailPath As String = ""
	  #End Region ' Fields

	  #Region "Constructor"
	  Public Sub New()
		 InitializeComponent()

		 labelFilename = Path.GetFullPath(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) & "\..\..\Address.btw")
	  End Sub
	  #End Region ' Constructor

	  #Region "Delegates"
	  Public Delegate Sub DoUpdateOutputDelegate(ByVal output As String, ByVal task As Task)
	  #End Region ' Delegates

	  #Region "Event Handlers"
	  #Region "Form Event Handlers"
	  ''' <summary>
	  ''' Called when the user opens the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub TaskExecutor_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
		 taskManager = New TaskManager()
		 AddHandler taskManager.TaskEngineStatusChanged, AddressOf taskEngines_EngineStatusChanged
		 AddHandler taskManager.ErrorOccurred, AddressOf taskEngines_ErrorOccurred

		 ' Place a set of default Tasks into the available list
		 lstAvailableTasks.Items.Add(Resources.PrintTaskLabel)
		 lstAvailableTasks.Items.Add(Resources.ExportThumbnailLabel)
		 lstAvailableTasks.Items.Add(Resources.ExportPreviewLabel)
		 lstAvailableTasks.Items.Add(Resources.XMLScriptTaskLabel)
		 lstAvailableTasks.Items.Add(Resources.GetFormatTaskLabel)
		 lstAvailableTasks.Items.Add(Resources.GroupTaskLabel)
		 lstAvailableTasks.Items.Add(Resources.CustomTaskLabel)

		 txtNumEngines.MaxLength = 2
	  End Sub

	  ''' <summary>
	  ''' Called when the user closes the application.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub TaskExecutor_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles MyBase.FormClosed
		 taskManager.Stop(1000, True)
	  End Sub

	  ''' <summary>
	  ''' Starts the selected number of Engines.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnStart_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStart.Click
		 Dim numEngines As Integer = 1
		 Dim success As Boolean = True

		 Try
			numEngines = Int32.Parse(txtNumEngines.Text)
			If numEngines < 1 Then
			   success = False
			End If
		 Catch e1 As FormatException
			success = False
		 End Try

		 If (Not success) Then
			MessageBox.Show(Me, "The number of engines must be a positive integer.", appName)
		 Else
			Try
			   Cursor.Current = Cursors.WaitCursor
			   taskManager.Start(numEngines)
			Catch exception As LicenseException
			   Dim msg As String = String.Format("You are using the BarTender {0} edition. To continue the BarTender Enterprise Print Server edition must be installed and activated.", exception.Edition)
			   MessageBox.Show(Me, msg, appName)
			Catch exception As PrintEngineException
			   MessageBox.Show(Me, exception.Message, appName)
			Catch exc As Exception
			   MessageBox.Show(Me, exc.Message, appName)
			End Try

			lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString()
			If taskManager.TaskEngines.AliveCount > 0 Then
			   txtNumEngines.Enabled = False
			   btnStop.Enabled = True
			   btnStart.Enabled = False
			   If lstRunTasks.Items.Count > 0 Then
				  btnRun.Enabled = True
			   End If
			End If
		 End If
	  End Sub

	  ''' <summary>
	  ''' Stops the running Engines.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnStop_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnStop.Click
		 Dim result As Result = taskManager.Stop(10000, False)
		 lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString()
		 If result = Result.Timeout Then
			MessageBox.Show(Me, "Unable to stop the print engines. They may still be busy.", appName)
		 ElseIf result = Result.Failure Then
			MessageBox.Show(Me, "Unable to stop the print engines. Catastrophic error.", appName)
		 End If
		 If taskManager.TaskEngines.AliveCount = 0 Then
			txtNumEngines.Enabled = True
			btnStop.Enabled = False
			btnStart.Enabled = True
			btnRun.Enabled = False
		 End If
	  End Sub

	  ''' <summary>
	  ''' Adds the selected Task to the list of Tasks to run.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnAdd_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdd.Click
		 If lstAvailableTasks.SelectedIndex >= 0 Then
			Dim selected As String = lstAvailableTasks.SelectedItem.ToString()
			lstRunTasks.Items.Add(selected)
			If taskManager.TaskEngines.AliveCount <> 0 Then
			   btnRun.Enabled = True
			End If
		 End If
	  End Sub

	  ''' <summary>
	  ''' Sends all Tasks in the right pane to the TaskQueue.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnRun_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRun.Click
		 QueueTasks()
		 lblTasksInQueue.Text = taskManager.TaskQueue.Count.ToString()
		 lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString()
		 lblEnginesBusy.Text = taskManager.TaskEngines.BusyCount.ToString()
	  End Sub

	  ''' <summary>
	  ''' Removes the selected Task.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub btnRemove_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRemove.Click
		 If lstRunTasks.SelectedIndex >= 0 Then
			lstRunTasks.Items.RemoveAt(lstRunTasks.SelectedIndex)
		 End If
		 If lstRunTasks.Items.Count = 0 Then
			btnRun.Enabled = False
		 End If
	  End Sub

	  ''' <summary>
	  ''' Enables or disables the remove button based on whether the user has
	  ''' a Task selected or not.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub lstRunTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstRunTasks.SelectedIndexChanged
		 btnRemove.Enabled = (lstRunTasks.SelectedIndex <> -1)
	  End Sub

	  ''' <summary>
	  ''' Updates the stat counter for the number of Tasks in the queue,
	  ''' the number of running Engines and the number of busy Engines.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub timerTaskUpdater_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles timerTaskUpdater.Tick
		 lblTasksInQueue.Text = taskManager.TaskQueue.Count.ToString()
		 lblEnginesRunning.Text = taskManager.TaskEngines.AliveCount.ToString()
		 lblEnginesBusy.Text = taskManager.TaskEngines.BusyCount.ToString()
	  End Sub

	  ''' <summary>
	  ''' Enables or disables the add button based on whether the user
	  ''' has a Task selected. Also changes the Task description based
	  ''' on which Task is selected.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="e"></param>
	  Private Sub lstAvailableTasks_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles lstAvailableTasks.SelectedIndexChanged
		 If lstAvailableTasks.SelectedIndex <> -1 Then
			btnAdd.Enabled = True
			Dim selected As String = lstAvailableTasks.SelectedItem.ToString()
			If selected = Resources.PrintTaskLabel Then
			   lblTaskInfo.Text = Resources.PrintTaskDescription
			ElseIf selected = Resources.ExportThumbnailLabel Then
			   lblTaskInfo.Text = Resources.ExportThumbnailDescription
			ElseIf selected = Resources.ExportPreviewLabel Then
			   lblTaskInfo.Text = Resources.ExportPreviewDescription
			ElseIf selected = Resources.XMLScriptTaskLabel Then
			   lblTaskInfo.Text = Resources.XMLScriptTaskDescription
			ElseIf selected = Resources.GetFormatTaskLabel Then
			   lblTaskInfo.Text = Resources.GetFormatTaskDescription
			ElseIf selected = Resources.GroupTaskLabel Then
			   lblTaskInfo.Text = Resources.GroupTaskDescription
			ElseIf selected = Resources.CustomTaskLabel Then
			   lblTaskInfo.Text = Resources.CustomTaskDescription
			End If
		 Else
			btnAdd.Enabled = False
			lblTaskInfo.Text = Resources.NoTaskDescription
		 End If
	  End Sub
	  #End Region ' Form Event Handlers

	  #Region "Task Event Handlers"
	  ''' <summary>
	  ''' Event called when the Print Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Print Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Print Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask_Completed(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Print Task has completed.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Export Thumbnail Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub thumbnailTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Export Thumbnail Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Export Thumbnail Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub thumbnailTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 Dim exportTask As ExportImageToFileTask = CType(task, ExportImageToFileTask)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Export Thumbnail Task exported to " & exportTask.FileName, task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Export Print Preview Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub previewTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Export Preview Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Export Print Preview Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub previewTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 Dim exportTask As ExportPrintPreviewToFileTask = CType(task, ExportPrintPreviewToFileTask)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Export Preview Task exported to " & exportTask.Directory, task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the XMLScript Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub xmlTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "XMLScript Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the XMLScript Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub xmlTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "XMLScript Task complete.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Get Format Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub getFormatTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Get Format Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Get Format Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub getFormatTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 Dim formatTask As GetLabelFormatTask = CType(task, GetLabelFormatTask)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Get Format Task complete.", task })
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Name: " & formatTask.LabelFormat.SubStrings("LastName").Value, Nothing })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Group Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub groupTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Group Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub groupTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 Dim groupTask As GroupTask = CType(task, GroupTask)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group task complete.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the first print Task in the GroupTask failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask1_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print1 error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the first print Task in the Group Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask1_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print1 has completed.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the second print Task in the Group Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask2_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print2 error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the second print Task in the Group Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask2_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print2 has completed.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the third print Task in the Group Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask3_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print3 has completed.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the third print Task in the GroupTask failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub printTask3_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Group Task Print3 error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Custom Task failed.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub customTask_ErrorOccurred(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Custom Task error.", task })
	  End Sub

	  ''' <summary>
	  ''' Event called when the Custom Task succeeded.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEventArgs"></param>
	  Private Sub customTask_Succeeded(ByVal task As Object, ByVal taskEventArgs As TaskEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Custom Task has completed.", task })
	  End Sub
	  #End Region ' Task Event Handlers

	  #Region "TaskEngine Event Handlers"
	  ''' <summary>
	  ''' Event called when a TaskEngine's status changes.
	  ''' </summary>
	  ''' <param name="engine"></param>
	  ''' <param name="status"></param>
	  Private Sub taskEngines_EngineStatusChanged(ByVal sender As Object, ByVal args As TaskEngineStatusChangedEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Engine Status: " & args.Status, Nothing })
	  End Sub

	  ''' <summary>
	  ''' Event called when a TaskEngine has an error.
	  ''' </summary>
	  ''' <param name="sender"></param>
	  ''' <param name="exception"></param>
	  Private Sub taskEngines_ErrorOccurred(ByVal sender As Object, ByVal args As EngineErrorEventArgs)
		 Dim doUpdateOutputDelegate As New DoUpdateOutputDelegate(AddressOf DoUpdateOutput)
		 lstOutput.BeginInvoke(doUpdateOutputDelegate, New Object() { "Engines error occurred: " & args.Exception.Message, Nothing })
	  End Sub
	  #End Region ' TaskEngine Event Handlers

	  #End Region ' Event Handlers

	  #Region "Methods"
	  ''' <summary>
	  ''' Updates the output listbox when it receives a message.
	  ''' </summary>
	  ''' <param name="task"></param>
	  ''' <param name="taskEngine"></param>
	  Private Sub DoUpdateOutput(ByVal output As String, ByVal task As Task)
		 lstOutput.Items.Add(output)
		 If task IsNot Nothing Then
			' Output the Task's messages.
			If task.Messages.Count > 1 Then
			   lstOutput.Items.Add("Task Messages:")
			   Dim text As String
			   For Each message As Seagull.BarTender.Print.Message In task.Messages
				  ' Let's remove any carriage returns and linefeeds since
				  ' we are putting each message on a single line.
				  text = message.Text.Replace(ControlChars.Lf, " "c)
				  text = text.Replace(ControlChars.Cr, " "c)
				  lstOutput.Items.Add(Constants.vbTab & text)
			   Next message
			End If
		 End If
	  End Sub

	  ''' <summary>
	  ''' Thumbnails need to go into a new place to assure we don't
	  ''' conflict with any existing files.
	  ''' </summary>
	  Private Sub CreateThumbnailPathIfNeeded()
		 If thumbnailPath.Length = 0 Then
			' Create a temporary folder to hold the images.
			Dim tempPath As String = Path.GetTempPath() ' Something like "C:\Documents and Settings\<username>\Local Settings\Temp""
			Dim newFolder As String

			' It's not likely we'll have a path that already
			' exists, but we'll check for it anyway.
			Do
			   newFolder = Path.GetRandomFileName()
			   thumbnailPath = tempPath & newFolder ' newFolder is something crazy like "gulvwdmt.3r4"
			Loop While Directory.Exists(thumbnailPath)
			Directory.CreateDirectory(thumbnailPath)
			DoUpdateOutput("Temporary folder created: " & thumbnailPath, Nothing)
		 End If
	  End Sub

	  ''' <summary>
	  ''' Adds copies of all Tasks in the list to the TaskQueue to be run.
	  ''' </summary>
	  Private Sub QueueTasks()
		 For Each taskName As String In lstRunTasks.Items
			Try
			   Dim task As Task = Nothing
			   If taskName = Resources.PrintTaskLabel Then
				  ' The print task.
				  Dim printTask As New PrintLabelFormatTask(labelFilename)
				  AddHandler printTask.ErrorOccurred, AddressOf printTask_ErrorOccurred
				  AddHandler printTask.Completed, AddressOf printTask_Completed
				  task = printTask
			   ElseIf taskName = Resources.ExportThumbnailLabel Then
				  CreateThumbnailPathIfNeeded()

				  ' The thumbnail export task.
				  Dim thumbnailTask As New ExportImageToFileTask(labelFilename, thumbnailPath & "\exportThumbnail.jpg")
				  thumbnailTask.ImageType = ImageType.JPEG
				  thumbnailTask.Colors = Seagull.BarTender.Print.ColorDepth.ColorDepth24bit
				  thumbnailTask.Resolution = New Resolution(ImageResolution.Screen)
				  AddHandler thumbnailTask.ErrorOccurred, AddressOf thumbnailTask_ErrorOccurred
				  AddHandler thumbnailTask.Succeeded, AddressOf thumbnailTask_Succeeded
				  task = thumbnailTask
			   ElseIf taskName = Resources.ExportPreviewLabel Then
				  CreateThumbnailPathIfNeeded()

				  ' The print preview export task.
				  Dim previewTask As New ExportPrintPreviewToFileTask(labelFilename, thumbnailPath, "exportPreview%PageNumber%.jpg")
				  previewTask.ImageType = ImageType.JPEG
				  previewTask.Colors = Seagull.BarTender.Print.ColorDepth.ColorDepth24bit
				  previewTask.Resolution = New Resolution(ImageResolution.Screen)
				  AddHandler previewTask.ErrorOccurred, AddressOf previewTask_ErrorOccurred
				  AddHandler previewTask.Succeeded, AddressOf previewTask_Succeeded
				  task = previewTask
			   ElseIf taskName = Resources.XMLScriptTaskLabel Then
				  ' XMLScript task.
				  Dim xml As String = "<?xml version=""1.0"" encoding=""utf-8""?>" & "<XMLScript Version=""2.0"" Name=""XMLScripter Sample"" ID=""123"">" & "<Command Name=""XMLScripter"">" & "<Print WaitForJobToComplete=""true"" JobName=""XMLScript Task"" Timeout=""30000"" " & "ReturnPrintData=""true"" ReturnSummary=""true""  ReturnLabelData=""true"" ReturnChecksum=""true"">" & "<Format>" & labelFilename & "</Format>" & "</Print>" & "</Command>" & "</XMLScript>"
				  Dim xmlTask As New XMLScriptTask(xml, XMLSourceType.ScriptString)
				  AddHandler xmlTask.ErrorOccurred, AddressOf xmlTask_ErrorOccurred
				  AddHandler xmlTask.Succeeded, AddressOf xmlTask_Succeeded
				  task = xmlTask
			   ElseIf taskName = Resources.GetFormatTaskLabel Then
				  ' Get Format Properties Task - this will get a substring on the label.
				  Dim getFormatTask As New GetLabelFormatTask(labelFilename)
				  AddHandler getFormatTask.ErrorOccurred, AddressOf getFormatTask_ErrorOccurred
				  AddHandler getFormatTask.Succeeded, AddressOf getFormatTask_Succeeded
				  task = getFormatTask
			   ElseIf taskName = Resources.GroupTaskLabel Then
				  ' Group Task - this will run three printjobs.
				  Dim groupTask As New GroupTask()
				  AddHandler groupTask.ErrorOccurred, AddressOf groupTask_ErrorOccurred
				  AddHandler groupTask.Succeeded, AddressOf groupTask_Succeeded

				  Dim printTask1 As New PrintLabelFormatTask(labelFilename)
				  groupTask.Add(printTask1)
				  AddHandler printTask1.ErrorOccurred, AddressOf printTask1_ErrorOccurred
				  AddHandler printTask1.Succeeded, AddressOf printTask1_Succeeded

				  Dim printTask2 As New PrintLabelFormatTask(labelFilename)
				  groupTask.Add(printTask2)
				  AddHandler printTask2.ErrorOccurred, AddressOf printTask2_ErrorOccurred
				  AddHandler printTask2.Succeeded, AddressOf printTask2_Succeeded

				  Dim printTask3 As New PrintLabelFormatTask(labelFilename)
				  groupTask.Add(printTask3)
				  AddHandler printTask3.ErrorOccurred, AddressOf printTask3_ErrorOccurred
				  AddHandler printTask3.Succeeded, AddressOf printTask3_Succeeded

				  task = groupTask
			   ElseIf taskName = Resources.CustomTaskLabel Then
				  ' Custom Task - this will change a substring on the label and print it.
				  Dim customTask As New CustomTask(labelFilename)
				  AddHandler customTask.ErrorOccurred, AddressOf customTask_ErrorOccurred
				  AddHandler customTask.Succeeded, AddressOf customTask_Succeeded
				  task = customTask
			   End If
			   taskManager.TaskQueue.QueueTask(task)
			Catch e As Exception
			   MessageBox.Show(Me, e.Message, appName)
			End Try
		 Next taskName
	  End Sub
	  #End Region ' Methods
   End Class
End Namespace
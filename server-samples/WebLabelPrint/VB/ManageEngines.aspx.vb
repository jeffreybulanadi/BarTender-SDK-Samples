Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.PrintServer

Partial Public Class ManageEngines
	Inherits System.Web.UI.Page
   #Region "Private Member Variables"
   Private _taskManager As TaskManager ' BarTender Task Manager object.
   Private _numEnginesForPage As Integer = 1 ' Number of print engines
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Show the current menu selection by highlighting the selected menu link's background.
	  CType(Master.FindControl("_manageEngines"), HyperLink).CssClass = "MenuLinkSelected"

	  ' The Print Server task manager instance is created in the Global.aspx
	  _taskManager = CType(Application("TaskManager"), TaskManager)

	  If _taskManager IsNot Nothing Then
		 ' Clear all error messages on the page.
		 ClearMessage()

		 ' Perform validation on the number of engines specified.
		 If ValidatePage() Then
			_numEnginesForPage = Int32.Parse(_textNumberPrintEngines.Text)

			' Multiple users can be viewing this page from different client 
			' browsers at the same time. Therefore, initialize the number of 
			' print engines based on what the web application is currently running.
			Dim engineAliveCount As Integer = _taskManager.TaskEngines.AliveCount
			If (engineAliveCount <> 0) Then
				_textNumberPrintEngines.Text = (engineAliveCount.ToString())
			Else
				_textNumberPrintEngines.Text = ("1")
			End If
		 End If

		 ' Update controls and status tables.
		 Update()
	  Else
		 ShowMessage("Unable to manage print engines. Please make sure you have BarTender installed and activated as Enterprise Print Server edition.")

		 ' Disable all controls.
		 EnableStartStopPanelControls(False)
	  End If
   End Sub
   #End Region

   #Region "Web Callbacks"
   ''' <summary>
   ''' Called when the Start button is selected.
   ''' </summary>
   Protected Sub ButtonStartEngines_Click(ByVal sender As Object, ByVal e As EventArgs)
	  ' Validate correct print engine entry.
	  If ValidatePage() Then
		 Try
			' Start one or more BarTender print engines using the Print Server SDK.
			' This will start separate bartend.exe processes in the background.
			_taskManager.Start(_numEnginesForPage)
			_textNumberPrintEngines.Text = _numEnginesForPage.ToString()
		 Catch ex As Exception
			ShowMessage("One or more print engines could not be started. <br /><br />Reason: " & ex.Message)
		 End Try
	  End If
	  Update()
   End Sub

   ''' <summary>
   ''' Called when the Stop button is selected.
   ''' </summary>
   Protected Sub ButtonStopEngines_Click(ByVal sender As Object, ByVal e As EventArgs)
	  Try
		 ' Stop all running BarTender print engines running in the background.
		 ' If for whatever reason one or more print engines are taking a long
		 ' time to stop kill all processes after "engineStopTimeout" seconds (e.g. 3 seconds).
		 Dim engineStopTimeout As Integer = Int32.Parse(ConfigurationManager.AppSettings("EngineStopTimeout"))
		 _taskManager.Stop(engineStopTimeout, True)
	  Catch ex As Exception
		 ShowMessage("An error occured while trying to stop one or more print engines.<br /><br />Reason: " & ex.Message)
	  End Try
	  Update()
   End Sub

   ''' <summary>
   ''' Called when the Change button is selected.
   ''' </summary>
   Protected Sub ButtonChangeNumPrintEngines_Click(ByVal sender As Object, ByVal e As EventArgs)
	  ' Validate correct print engine entry.
	  If ValidatePage() Then
		 Try
			' Resize all running BarTender print engines running in the background.
			' If for whatever reason one or more print engines are taking a long
			' time to stop kill the needed processes after "engineStopTimeout" seconds (e.g. 3 seconds).
			Dim engineStopTimeout As Integer = Int32.Parse(ConfigurationManager.AppSettings("EngineStopTimeout"))
			_taskManager.Resize(_numEnginesForPage, engineStopTimeout, True)
			_textNumberPrintEngines.Text = _numEnginesForPage.ToString()
		 Catch ex As Exception
			ShowMessage("One or more print engines could not be restarted.<br /><br />Reason: " & ex.Message)
		 End Try
	  End If
	  Update()
   End Sub
   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Validate the page for correct entry of number of print engines.
   ''' </summary>
   ''' <returns>
   ''' True if OK to continue, else False.
   ''' </returns>
   Private Function ValidatePage() As Boolean
	  Dim validated As Boolean = True
	  Dim numEngines As Integer = 0
	  Dim maxAllowedPrintEngines As Integer = 0
	  ' More print engines can be increased from 5. The maximum number of print engines
	  ' that can be started is dependant on system resources available and the operating 
	  ' system being used.
	  Try
		 maxAllowedPrintEngines = Int32.Parse(ConfigurationManager.AppSettings("MaxAllowedPrintEngines"))
	  Catch e1 As Exception
		 ShowMessage("maxAllowedPrintEngines in the web.config must be a number.")
		 validated = False
	  End Try

	  If validated Then
		 Try
			numEngines = Int32.Parse(_textNumberPrintEngines.Text)
		 Catch e2 As Exception
			ShowMessage("The number of print engines must be a number.")
			SetFocus(_textNumberPrintEngines)
			validated = False
		 End Try

		 If validated AndAlso (numEngines < 1) OrElse (numEngines > maxAllowedPrintEngines) Then
			ShowMessage("The number of print engines must be between 1 and " & maxAllowedPrintEngines.ToString())
			SetFocus(_textNumberPrintEngines)
			validated = False
		 End If
	  End If

	  Return validated
   End Function

   ''' <summary>
   ''' Update engine controls and status tables.
   ''' </summary>
   Private Sub Update()
	  UpdateStartStopPanel()
	  UpdateStatusTables()
   End Sub

   ''' <summary>
   ''' Update the controls on the engine controls panel.
   ''' </summary>
   Private Sub UpdateStartStopPanel()
	  Dim printEnginesStarted As Boolean = (_taskManager.TaskEngines.AliveCount > 0)
	  _buttonStartEngines.Enabled = Not printEnginesStarted
	  _buttonStopEngines.Enabled = printEnginesStarted
	  _buttonChangeNumPrintEngines.Enabled = printEnginesStarted
   End Sub

   ''' <summary>
   ''' Update the status table values.
   ''' </summary>
   Private Sub UpdateStatusTables()
	  ' Print engine status values
	  _labelNumPrintEngines.Text = _taskManager.TaskEngines.AliveCount.ToString()
	  _labelBusyPrintEngines.Text = _taskManager.TaskEngines.BusyCount.ToString()

	  ' Task queue status values
	  _labelNumTasks.Text = _taskManager.TaskQueue.Count.ToString()
	  _labelQueueLocked.Text = _taskManager.TaskQueue.IsLocked.ToString()
	  _labelQueuePaused.Text = _taskManager.TaskQueue.IsPaused.ToString()
   End Sub

   Private Sub EnableStartStopPanelControls(ByVal enable As Boolean)
	  _buttonStartEngines.Enabled = enable
	  _buttonStopEngines.Enabled = enable
	  _buttonChangeNumPrintEngines.Enabled = enable
	  _textNumberPrintEngines.Enabled = enable
   End Sub

   ''' <summary>
   ''' Show error message text.
   ''' </summary>
   ''' <param name="msg">Message text.</param>
   Private Sub ShowMessage(ByVal msg As String)
	  _alert.Message = msg
   End Sub

   ''' <summary>
   ''' Clear error message text. 
   ''' </summary>
   ''' <param name="msg">Message text.</param>
   Private Sub ClearMessage()
	  _alert.Message = String.Empty
   End Sub
   #End Region

End Class

Imports Microsoft.VisualBasic
Imports System
Imports System.Configuration
Imports System.IO
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Seagull.BarTender.Print
Imports Seagull.BarTender.PrintServer
Imports Seagull.BarTender.PrintServer.Tasks

Partial Public Class PrintPreviewControl
	Inherits System.Web.UI.UserControl
   #Region "Event Handlers"
   Public Event OnBackEvent As EventHandler ' Fired when the Back button is selected.
   Public Event OnPrintEvent As EventHandler ' Fired when the Print button is selected.
   #End Region

   #Region "Private Member Variables"
   Private _imageTempFolder As String ' Image temporary folder. Used for print preview images.
   Private _labelFormat As LabelFormat ' Label format data object.
   #End Region

   #Region "Enumerations"
   ' Declare navigation type enumeration.
   Private Enum NavType
	  First
	  Previous
	  [Next]
	  Last
   End Enum
   #End Region

   #Region "Web Methods"
   ''' <summary>
   ''' Called when the page is loaded.
   ''' </summary>
   Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
	  ' Add JavaScript to the OnError event of the preview image to notify if there is a problem loading the image.
	  Dim errorNotifyScript As String = String.Format("onPreviewImageError(document.getElementById('{0}'))", _imagePreview.ClientID)
	  _imagePreview.Attributes("OnError") = errorNotifyScript

	  ' Clear out any messages
	  If (Not IsPostBack) Then
		 ShowMessage("")
	  End If

	  ' Get the folder for temporary image files from Web.Config.
	  _imageTempFolder = ConfigurationManager.AppSettings("ImageTempFolder")
	  If String.IsNullOrEmpty(_imageTempFolder) Then
		 _imageTempFolder = "Temp"
	  End If
   End Sub
   #End Region

   #Region "Web Callbacks"
   ''' <summary>
   ''' Called when the print button is selected.
   ''' </summary>
    Protected Sub ButtonPrint_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent OnPrintEvent(Me, e)
    End Sub

   ''' <summary>
   ''' Called when the First navigation button is selected.
   ''' </summary>
    Protected Sub ButtonFirst_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Navigate(NavType.First)
    End Sub

   ''' <summary>
   ''' Called when the Last navigation button is selected.
   ''' </summary>
    Protected Sub ButtonLast_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Navigate(NavType.Last)
    End Sub

   ''' <summary>
   ''' Called when the Next navigation button is selected.
   ''' </summary>
    Protected Sub ButtonNext_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Navigate(NavType.Next)
    End Sub
   ''' <summary>
   ''' Called when the Previou navigation button is selected.
   ''' </summary>
   ''' <param name="sender"></param>
   ''' <param name="e"></param>
    Protected Sub ButtonPrevious_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Navigate(NavType.Previous)
    End Sub

   ''' <summary>
   ''' Called when the Back button is selected.
   ''' </summary>
    Protected Sub ButtonBack_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        RaiseEvent OnBackEvent(Me, e)
    End Sub
#End Region

   #Region "Public Properties"
   ''' <summary>
   ''' Sets the LabelFormat that will be used to get substrings.
   ''' </summary>
   Public WriteOnly Property LabelFormatObject() As LabelFormat
	  Set(ByVal value As LabelFormat)
		 _labelFormat = value
		 GeneratePreview(_labelFormat)
	  End Set
   End Property

   Public Property PrintButtonClickJavaScript() As String
	  Get
		 Return _buttonPrint.OnClientClick
	  End Get
	  Set(ByVal value As String)
		 _buttonPrint.OnClientClick = value
	  End Set
   End Property

   #End Region

   #Region "Support Methods"
   ''' <summary>
   ''' Navigate the page previews.
   ''' </summary>
   ''' <param name="navType">Navigation type</param>
    Private Sub Navigate(ByVal navType As NavType)
        ' Get values from the panel cache.
        Dim currentPageNumber As Integer = Int32.Parse(_previewPanel.Attributes("currentPreview"))
        Dim totalPages As Integer = Int32.Parse(_previewPanel.Attributes("totalPreviews"))

        ' Based on navigation type selected set current page number.
        Select Case navType
            Case navType.First
                currentPageNumber = 1

            Case navType.Last
                currentPageNumber = totalPages

            Case navType.Next
                currentPageNumber += 1

            Case navType.Previous
                currentPageNumber -= 1
        End Select

        ' Enable/Disable navigation buttons.
        EnableNavButton(navType.First, _buttonFirst, Not (currentPageNumber = 1))
        EnableNavButton(navType.Previous, _buttonPrevious, Not (currentPageNumber = 1))
        EnableNavButton(navType.Next, _buttonNext, Not (currentPageNumber = totalPages))
        EnableNavButton(navType.Last, _buttonLast, Not (currentPageNumber = totalPages))

        ' Update current page label
        _labelCurrentPage.Text = currentPageNumber.ToString()

        ' Cache values in the panel.
        _previewPanel.Attributes("currentPreview") = currentPageNumber.ToString()

        ' Update the image preview     
        _imagePreview.ImageUrl = String.Format("./{0}/{1}.{2}.png", _imageTempFolder, _previewPanel.Attributes("imageGUID"), currentPageNumber)

        ' Show alternate message in case the session has timed out and all preview images
        ' have been deleted.
        _imagePreview.AlternateText = String.Format("Print preview image {0} of {1}.", currentPageNumber, totalPages)
    End Sub

   ''' <summary>
   ''' Enable/Disable the navigation image button.
   ''' </summary>
   ''' <param name="navType">Navigation Type</param>
   ''' <param name="imgBtn">Image button control</param>
   ''' <param name="enabled">Enable/Disable control</param>
   Private Sub EnableNavButton(ByVal navType As NavType, ByVal imgBtn As ImageButton, ByVal enabled As Boolean)
	  ' Based on navigation type selected set enable/disable image control.
	  Select Case navType
		 Case NavType.First
			If enabled Then
				imgBtn.ImageUrl = "~/images/First.png"
			Else
				imgBtn.ImageUrl = "~/images/FirstDisabled.png"
			End If

		 Case NavType.Last
			If enabled Then
				imgBtn.ImageUrl = "~/images/Last.png"
			Else
				imgBtn.ImageUrl = "~/images/LastDisabled.png"
			End If

		 Case NavType.Next
			If enabled Then
				imgBtn.ImageUrl = "~/images/Next.png"
			Else
				imgBtn.ImageUrl = "~/images/NextDisabled.png"
			End If

		 Case NavType.Previous
			If enabled Then
				imgBtn.ImageUrl = "~/images/Previous.png"
			Else
				imgBtn.ImageUrl = "~/images/PreviousDisabled.png"
			End If
	  End Select
	  imgBtn.Enabled = enabled
   End Sub

   ''' <summary>
   ''' Generate the print preview pages for the lable format selected.
   ''' </summary>
   Private Sub GeneratePreview(ByVal labelFormat As LabelFormat)
	  Try
		 ' Get the Task Manager from the application cache.
		 Dim taskManager As TaskManager = CType(Application("TaskManager"), TaskManager)

		 ' Make sure the task manager is valid and that engines are running.
		 If (taskManager IsNot Nothing) AndAlso (taskManager.TaskEngines.AliveCount <> 0) Then
			' Generate output image path and file name template
			Dim outputImagePath As String = Server.MapPath(_imageTempFolder)
                Dim sessionIDGuid As String = Guid.NewGuid().ToString()
			Dim sessionID As String = Session.SessionID

			' Save all print preview generatede images using the Session ID and a unique GUID.
			' When the session ends all temporary files generated for a given Session ID are 
			' deleted (See Global.asax).
                Dim imageGuid As String = sessionID & "." & sessionIDGuid
			Dim fileNameTemplate As String = imageGuid & ".%PageNumber%.png"

			' Create a ExportPrintPreviewToFileTask task object to export all print preview
			' pages to an image folder.  Generate all images using a set size of 850x850 as 
			' PNGs. Included all margins and borders.
			Dim taskPrint As New ExportPrintPreviewToFileTask(labelFormat, outputImagePath, fileNameTemplate, ImageType.PNG, New Resolution(850, 850), ColorDepth.ColorDepth24bit, System.Drawing.Color.White, True, True, OverwriteOptions.Overwrite)

			' If a label format has prompting be sure to disable all prompts.
			taskPrint.LabelFormat.PrintSetup.EnablePrompting = False

			' Put the export task onto the task queue for processing. Wait until it is done. Timeout if
			' longer than 60 seconds.
			Dim status As TaskStatus = taskManager.TaskQueue.QueueTaskAndWait(taskPrint, 60000)

			' Check for success. If good, then setup panel attributes and navigation to the first
			' page.
                If (status = TaskStatus.Success) AndAlso (taskPrint.NumberOfPreviews > 0) Then
                    ' Setup and cache preview panel attributes.
                    _labelCurrentPage.Text = "1"
                    _labelTotalPages.Text = taskPrint.NumberOfPreviews.ToString()
                    _previewPanel.Attributes.Add("imageGUID", imageGuid)
                    _previewPanel.Attributes.Add("currentPreview", "1")
                    _previewPanel.Attributes.Add("totalPreviews", taskPrint.NumberOfPreviews.ToString())

                    ' Navigate to the first page.
                    Navigate(NavType.First)

                    ' Show label format name in toolbar.
                    _labelFormatFileName.Text = Path.GetFileName(labelFormat.FileName)

                    ' Display the preview panel.
                    _previewPanel.Visible = True
                End If

			' If we have any error, then display.
			Dim msg As String = ""
			Dim msgPos As Integer = 1
			For Each message As Message In taskPrint.Messages
			   msg &= message.Text.Replace(Constants.vbLf, "<br/>") & "<br/>"

			   ' In there are multiple messages separate with an extra break.
			   If msgPos <> taskPrint.Messages.Count Then
				  msg &= "<br/>"
			   End If
			   msgPos += 1
			Next message

			If taskPrint.Messages.Count <> 0 Then
			   _imagePreview.AlternateText = "Preview image is not available."
			End If

			ShowMessage(msg)
		 Else
			_previewPanel.Visible = False
			ShowMessage("Unable to view the label print preview. Please make sure you have BarTender <br />installed, activated as Enterprise Print Server edition, and that print engines are <br />running. See the 'Manage Print Engines' menu task.")
		 End If

		 ' Show label format name in toolbar.
		 _labelFormatFileName.Text = Path.GetFileName(labelFormat.FileName)
	  Catch ex As Exception
		 ShowMessage(ex.Message)
	  End Try
   End Sub

   ''' <summary>
   ''' Show messages to user.
   ''' </summary>
   ''' <param name="msg"></param>

   Private Sub ShowMessage(ByVal msg As String)
	  _alert.Message = msg
   End Sub
   #End Region
End Class

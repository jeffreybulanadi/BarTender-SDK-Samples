Imports Microsoft.VisualBasic
Imports System
Imports Seagull.BarTender.Print
Imports Seagull.BarTender.PrintServer.Tasks

Namespace TaskMaster
   ''' <summary>
   ''' Example of a custom Task.
   ''' This task will load the requested format, set some substrings on it, then print the label.
   ''' The named substrings are meant to be used with the supplied Address.btw label.
   ''' 
   ''' To write a custom Task, the OnRun method must be overridden from the base class. This
   ''' is the logic that will be run by whichever Print Engine is selected to run the task.
   ''' </summary>
   Friend Class CustomTask
	   Inherits Task
	  Private format As LabelFormat = Nothing

	  ''' <summary>
	  ''' Initialize the LabelFormat object.
	  ''' </summary>
	  ''' <param name="labelFormatFileName"></param>
	  Public Sub New(ByVal labelFormatFileName As String)
		 format = New LabelFormat(labelFormatFileName)
	  End Sub

	  ''' <summary>
	  ''' Override this method to perform custom Task logic.
	  ''' This sample method writes values to the named substrings on the label and then 
	  ''' prints the label.
	  ''' </summary>
	  ''' <returns></returns>
	  Protected Overrides Function OnRun() As Boolean
		 Dim formatDoc As LabelFormatDocument = Nothing
		 Try
			' Open a LabelFormatDocument using the LabelFormat that was passed in.
			formatDoc = Engine.Documents.Open(format, messages)

			' Assign this to the member LabelFormat variable so 
			' it can be accessed after the Task finishes.
			format = formatDoc

			' Set some substrings on the label
			formatDoc.SubStrings("FirstName").Value = "John"
			formatDoc.SubStrings("LastName").Value = "Doe"
			formatDoc.SubStrings("Company").Value = "Acme Widgets"
			formatDoc.SubStrings("StreetAddress").Value = "1234 Main Street"
			formatDoc.SubStrings("City").Value = "Bellevue"
			formatDoc.SubStrings("State").Value = "WA"
			formatDoc.SubStrings("Zip").Value = "98005"
			formatDoc.PrintSetup.UseDatabase = False

			' Print the label
			Dim printMessages As Messages = Nothing
			formatDoc.Print("", 1000, printMessages)
			For Each message As Message In printMessages
			   messages.Add(message)
			Next message

			' Close the LabelFormatDocument to free up resources
			' in the TaskEngine that was used.
			formatDoc.Close(SaveOptions.DoNotSaveChanges)
		 Catch
			Try
			   ' Attempt to close if the format is still open.
			   formatDoc.Close(SaveOptions.DoNotSaveChanges)
			Catch e1 As Exception
			End Try
			Throw
		 End Try
		 Return MyBase.OnRun()
	  End Function

	  ''' <summary>
	  ''' Override this method to validate that this Task's properties are 
	  ''' correct when it gets added to the TaskQueue. Typically you would
	  ''' throw an exception that you would catch during a
	  ''' TaskManager.TaskQueue.QueueTask call.
	  ''' </summary>
	  Protected Overrides Sub OnValidate()
	  End Sub

	  ''' <summary>
	  ''' Allow access to the LabelFormat used in this Task.
	  ''' </summary>
	  Private ReadOnly Property LabelFormat() As LabelFormat
		 Get
			Return format
		 End Get
	  End Property
   End Class
End Namespace

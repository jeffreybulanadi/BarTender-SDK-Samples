Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Windows.Forms

Namespace XMLScripter
   Friend NotInheritable Class Program
	  ''' <summary>
	  ''' The main entry point for the application.
	  ''' </summary>
	  Private Sub New()
	  End Sub
	  <STAThread> _
	  Shared Sub Main()
		 Application.EnableVisualStyles()
		 Application.SetCompatibleTextRenderingDefault(False)
		 Application.Run(New XMLScripter())
	  End Sub
   End Class
End Namespace
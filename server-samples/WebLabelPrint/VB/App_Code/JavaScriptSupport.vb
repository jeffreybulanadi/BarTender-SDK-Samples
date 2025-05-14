Imports Microsoft.VisualBasic
Imports System

''' <summary>
''' Support methods for JavaScript.
''' </summary>
Public Class JavaScriptSupport
    Public Shared Function EscapeSpecialCharacters(ByVal input As String) As String
        input = input.Replace("\", "\\") ' Escape backslash
        input = input.Replace("'", "\'") ' Escape apostrophes
        input = input.Replace("""", "\""") ' Escape double-quotes

        Return input
    End Function
End Class

Imports Microsoft.VisualBasic
Imports System
Imports System.IO

Imports Seagull.BarTender.Print

''' <summary>
''' This class provides simple thumbnail image caching for a given label format file.
''' </summary>
Public NotInheritable Class LabelFormatThumbnailImage
    Private Sub New()
    End Sub
    ''' <summary>
    ''' Get the URL for a thumbnail image based on supplied label format file name. The returned URL can be 
    ''' used as a web page image reference. If the thumbnail does not exist, or if the label format's last 
    ''' modified time is greater than the thumbnail's image last modified time, a new thumbnail image is 
    ''' created and cached in the same folder. 
    ''' 
    ''' The thumbnail file name format is:
    '''     [LabelFullPath].[Width]x[Height].[ImageFormat], where
    '''     [LabelFullPath] = Label format file name
    '''     [Width]         = Width of thumbnail.
    '''     [Height]        = Height of thumbnail.
    '''     [ImageFormat]   = Image format type.
    '''     
    ''' Example:
    '''   Label format file name:     c:\Labels\Retail.btw
    '''   Thumbnail Size:            Width: 128px  Height: 128px
    '''   Thumbnail image file name:  c:\Labels\Retial.128x128.jpg
    ''' 
    ''' Note: The thumbnail image created is extracted from the label format's BTW file. The thumbnail saved in the BTW 
    ''' is of limited size. If GetURL() is called with a width and height larger than the size of the thumbnail image saved
    ''' in the BTW label format, then image quality will be poor.
    ''' 
    ''' </summary>
    ''' <param name="labelFullPath">Label format file name.</param>
    ''' <param name="thumbnailFolder">Folder where the thumbnail exists on the system.</param>
    ''' <param name="urlThumbnailFolder">URL folder where the thumbnail can be referenced.</param>
    ''' <param name="bgColor">Background color for the thumbnail image.</param>
    ''' <param name="width">Width of thumbnail image.</param>
    ''' <param name="height">Height of thumbnail image.</param>
    ''' <param name="imgFormat">The image format type to use. (e.g. JPG, BMP)</param>
    ''' <returns>
    ''' Return the URL pointing to the thumbnail image location. An empty string is returned if any error occurs (e.g. the label 
    ''' format file is not found).
    ''' </returns>
    Public Shared Function GetURL(ByVal labelFullPath As String, ByVal thumbnailFolder As String, ByVal urlThumbnailFolder As String, ByVal bgColor As System.Drawing.Color, ByVal width As Integer, ByVal height As Integer, ByVal imgFormat As System.Drawing.Imaging.ImageFormat) As String
        Dim urlFullPath As String = ""
        Dim labelFileName As String = Path.GetFileName(labelFullPath)
        Dim labelFileNameNoExt As String = Path.GetFileNameWithoutExtension(labelFullPath)

        Dim thumbnailExtension As String = imgFormat.ToString().ToLower()
        Dim thumbnailFileName As String = String.Format("{0}.{1}x{2}.{3}", labelFileNameNoExt, width, height, thumbnailExtension)
        Dim thumbnailFullPath As String = Path.Combine(thumbnailFolder, thumbnailFileName)

        ' The label format file must exist
        If File.Exists(labelFullPath) Then
            urlFullPath = urlThumbnailFolder & "/" & Path.GetFileName(thumbnailFullPath)

            ' Check to see if the thumbnail is out of date. Out of date means:
            '    * The thumbnail file does not exist
            '    * The label format file last modified time is newer than the thumbnail's image last modified time.
            If IsThumbnailOutOfDate(labelFullPath, thumbnailFullPath) Then
                ' Create the thumbnail image.
                Dim imageOK As Boolean = CreateThumbnailImage(labelFullPath, thumbnailFullPath, bgColor, width, height, imgFormat)
                If (Not imageOK) Then
                    urlFullPath = ""
                End If
            End If
        End If
        Return urlFullPath
    End Function

	''' <summary>
	''' Check to see if the thumbnail is out of date. 
	''' </summary>
	''' <param name="labelFullPath">Label format file name</param>
	''' <param name="thumbnailFullPath">Thumbnail image file name</param>
	''' <returns>
   ''' Returns True if:
   '''   * The thumbnail file name does not exist.
   '''   * The label format file last modified time is newer than the thumbnail's image last modified time.
   ''' </returns>
   Private Shared Function IsThumbnailOutOfDate(ByVal labelFullPath As String, ByVal thumbnailFullPath As String) As Boolean
	  Dim isOutOfDate As Boolean = True

	  ' We are always out of date if the label format does not exist.
	  If File.Exists(labelFullPath) Then
		 Try
			' Gather up the last modified date/time information.
			Dim labelFileLastModified As DateTime = File.GetLastWriteTimeUtc(labelFullPath)
			Dim thumbnailFileLastModified As DateTime = File.GetLastWriteTimeUtc(thumbnailFullPath)

			' We are out of date if the label format last modified time is newer then the thumbnail image.
			isOutOfDate = (labelFileLastModified > thumbnailFileLastModified)
		 Catch e1 As Exception
			isOutOfDate = True
		 End Try
	  End If
	  Return isOutOfDate
   End Function

   ''' <summary>
   ''' Creates a thumbnail image using the label format file. The thumbnail image is
   ''' extracted from the label format BTW file.
   ''' </summary>
   ''' <param name="labelFullPath">Label Format file name</param>
   ''' <param name="thumbnailFullPath">Thumbnail image file name</param>
   ''' <param name="bgColor">Background color of thumbnail image</param>
   ''' <param name="width">Width of thumbnail image</param>
   ''' <param name="height">Height of thumbnail image</param>
   ''' <param name="imgFormat">The image format to use. (e.g. JPG, BMP)</param>
   ''' <returns>
   ''' Returns True if the thumbnail was created successfully, else False.
   ''' </returns>
   Private Shared Function CreateThumbnailImage(ByVal labelFullPath As String, ByVal thumbnailFullPath As String, ByVal bgColor As System.Drawing.Color, ByVal width As Integer, ByVal height As Integer, ByVal imgFormat As System.Drawing.Imaging.ImageFormat) As Boolean
	  Dim success As Boolean = False

	  Try
		 ' Create the thumbnail image using the label format BTW. The thumbnail image creation
		 ' is very quick because the thumbnail image is extracted from the BTW file. The thumbnail is 
		 ' not generated using the BarTender print engine.
		 Dim image As System.Drawing.Image = LabelFormatThumbnail.Create(labelFullPath, bgColor, width, height)
		 image.Save(thumbnailFullPath, imgFormat)
		 success = True
	  Catch e1 As Exception
		 success = False
	  End Try
	  Return success
   End Function
End Class

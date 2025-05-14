using System;
using System.IO;

using Seagull.BarTender.Print;

/// <summary>
/// This class provides simple thumbnail image caching for a given label format file.
/// </summary>
public static class LabelFormatThumbnailImage
{
   /// <summary>
   /// Get the URL for a thumbnail image based on supplied label format file name. The returned URL can be 
   /// used as a web page image reference. If the thumbnail does not exist, or if the label format's last 
   /// modified time is greater than the thumbnail's image last modified time, a new thumbnail image is 
   /// created and cached in the same folder. 
   /// 
   /// The thumbnail file name format is:
   ///     [LabelFullPath].[Width]x[Height].[ImageFormat], where
   ///     [LabelFullPath] = Label format file name
   ///     [Width]         = Width of thumbnail.
   ///     [Height]        = Height of thumbnail.
   ///     [ImageFormat]   = Image format type.
   ///     
   /// Example:
   ///   Label format file name:     c:\Labels\Retail.btw
   ///   Thumbnail Size:            Width: 128px  Height: 128px
   ///   Thumbnail image file name:  c:\Labels\Retial.128x128.jpg
   /// 
   /// Note: The thumbnail image created is extracted from the label format's BTW file. The thumbnail saved in the BTW 
   /// is of limited size. If GetURL() is called with a width and height larger than the size of the thumbnail image saved
   /// in the BTW label format, then image quality will be poor.
   /// 
   /// </summary>
   /// <param name="labelFullPath">Label format file name.</param>
   /// <param name="thumbnailFolder">Folder where the thumbnail exists on the system.</param>
   /// <param name="urlThumbnailFolder">URL folder where the thumbnail can be referenced.</param>
   /// <param name="bgColor">Background color for the thumbnail image.</param>
   /// <param name="width">Width of thumbnail image.</param>
   /// <param name="height">Height of thumbnail image.</param>
   /// <param name="imgFormat">The image format type to use. (e.g. JPG, BMP)</param>
   /// <returns>
   /// Return the URL pointing to the thumbnail image location. An empty string is returned if any error occurs (e.g. the label 
   /// format file is not found).
   /// </returns>
   public static string GetURL(string labelFullPath,
                               string thumbnailFolder, 
                               string urlThumbnailFolder,
                               System.Drawing.Color bgColor, 
                               int width, 
                               int height, 
                               System.Drawing.Imaging.ImageFormat imgFormat)
   {
      string urlFullPath         = "";
      string labelFileName       = Path.GetFileName(labelFullPath);
      string labelFileNameNoExt  = Path.GetFileNameWithoutExtension(labelFullPath);

      string thumbnailExtension = imgFormat.ToString().ToLower();
      string thumbnailFileName = string.Format("{0}.{1}x{2}.{3}", labelFileNameNoExt, width, height, thumbnailExtension);
      string thumbnailFullPath = Path.Combine(thumbnailFolder, thumbnailFileName); 

	   // The label format file must exist
	   if (File.Exists(labelFullPath))
	   {
         urlFullPath = urlThumbnailFolder + "/" + Path.GetFileName(thumbnailFullPath);
         
	      // Check to see if the thumbnail is out of date. Out of date means:
	      //    * The thumbnail file does not exist
	      //    * The label format file last modified time is newer than the thumbnail's image last modified time.
         if (IsThumbnailOutOfDate(labelFullPath, thumbnailFullPath))
         {
            // Create the thumbnail image.
            bool imageOK = CreateThumbnailImage(labelFullPath, thumbnailFullPath, bgColor, width, height, imgFormat);
            if (!imageOK)
               urlFullPath = "";
         }
      }   
      return urlFullPath;
   }
	
	/// <summary>
	/// Check to see if the thumbnail is out of date. 
	/// </summary>
	/// <param name="labelFullPath">Label format file name</param>
	/// <param name="thumbnailFullPath">Thumbnail image file name</param>
	/// <returns>
   /// Returns True if:
   ///   * The thumbnail file name does not exist.
   ///   * The label format file last modified time is newer than the thumbnail's image last modified time.
   /// </returns>
   private static bool IsThumbnailOutOfDate(string labelFullPath, string thumbnailFullPath)
   {
      bool isOutOfDate = true;
      
      // We are always out of date if the label format does not exist.
      if (File.Exists(labelFullPath))
      {
         try
         {
            // Gather up the last modified date/time information.
            DateTime labelFileLastModified = File.GetLastWriteTimeUtc(labelFullPath);
            DateTime thumbnailFileLastModified = File.GetLastWriteTimeUtc(thumbnailFullPath);

            // We are out of date if the label format last modified time is newer then the thumbnail image.
            isOutOfDate = (labelFileLastModified > thumbnailFileLastModified);
         }
         catch (Exception)
         {
            isOutOfDate = true;
         }
      }
      return isOutOfDate; 
   }

   /// <summary>
   /// Creates a thumbnail image using the label format file. The thumbnail image is
   /// extracted from the label format BTW file.
   /// </summary>
   /// <param name="labelFullPath">Label Format file name</param>
   /// <param name="thumbnailFullPath">Thumbnail image file name</param>
   /// <param name="bgColor">Background color of thumbnail image</param>
   /// <param name="width">Width of thumbnail image</param>
   /// <param name="height">Height of thumbnail image</param>
   /// <param name="imgFormat">The image format to use. (e.g. JPG, BMP)</param>
   /// <returns>
   /// Returns True if the thumbnail was created successfully, else False.
   /// </returns>
   private static bool CreateThumbnailImage(string labelFullPath, 
                                            string thumbnailFullPath, 
                                            System.Drawing.Color bgColor, 
                                            int width, 
                                            int height, 
                                            System.Drawing.Imaging.ImageFormat imgFormat)
   {
      bool success = false;
      
      try
      {
         // Create the thumbnail image using the label format BTW. The thumbnail image creation
         // is very quick because the thumbnail image is extracted from the BTW file. The thumbnail is 
         // not generated using the BarTender print engine.
         System.Drawing.Image image = LabelFormatThumbnail.Create(labelFullPath, bgColor, width, height);
         image.Save(thumbnailFullPath, imgFormat);
         success = true;
      }
      catch (Exception)
      {
         success = false;         
      }
      return success;
   }
}

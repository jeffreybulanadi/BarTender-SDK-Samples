using System;
using System.Configuration;
using System.IO;
using System.Web.UI;

public partial class LabelThumbnail : System.Web.UI.UserControl
{
   #region Private Member Variables
   private string _labelFileName;        // Label format filename
   private string _labelFullPath;        // Label format full path (Folder and filename combined)
   private int    _width = 128;          // Width of image
   private int    _height = 128;         // Height of image
   private bool   _showFilename = true;  // Show filename under image
   private string _clickURL = "";        // URL to navigate to when clicked.
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      if (!IsPostBack)
      {
         if (_labelFullPath != null)
         {
            string labelRepositoryFolder = ConfigurationManager.AppSettings["LabelRepository"];
            _labelFileName = System.IO.Path.GetFileName(_labelFullPath);

            // Get and generate if needed a reference to the label format thumbnail image. 
            _imageThumbnail.ImageUrl = LabelFormatThumbnailImage.GetURL(_labelFullPath, 
                                                                   Path.GetDirectoryName(_labelFullPath), 
                                                                   labelRepositoryFolder, 
                                                                   System.Drawing.Color.Gray, 
                                                                   _width, 
                                                                   _height,
                                                                   System.Drawing.Imaging.ImageFormat.Png);
            _imageThumbnail.Visible = true;
            _imageThumbnail.CssClass = "LabelItemImage";

            if (_showFilename)
               _labelThumbnailText.Text = "<br/>" + _labelFileName;
            else
               _labelThumbnailText.Visible = false;
         }
      }
      
      if (_clickURL != "")
      {
         // Setup navigation links
         _imageThumbnailLink.NavigateUrl = _clickURL;
         _imageThumbnailLink.Enabled = true;
         _textThumbnailLink.NavigateUrl = _clickURL;
         _textThumbnailLink.Enabled = true;
      }
   }
   #endregion
   
   #region Public Properties
   /// <summary>
   /// Set/Get the full path to the label format.
   /// </summary>
   public string LabelFullPath
   {
      get
      {
         return _labelFullPath;
      }
      set
      {
         _labelFullPath = value;
      }
   }

   /// <summary>
   /// Return the label format file name.
   /// </summary>
   public string LabelFileName
   {
      get
      {
         return _labelFileName;
      }
   }

   /// <summary>
   /// Set/Get the thumbnails width in pixels
   /// </summary>
   public int ThumbnailWidth
   {
      get
      {
         return _width;
      }
      set
      {
         _width = value;
      }
   }

   /// <summary>
   /// Set/Get the thumbnails height in pixels
   /// </summary>
   public int ThumbnailHeight
   {
      get
      {
         return _height;
      }
      set
      {
         _height = value;
      }
   }

   /// <summary>
   /// Specify to show the filename as a link below the thumbnail image.
   /// </summary>
   public bool ShowFilename
   {
      get
      {
         return _showFilename;
      }
      set
      {
         _showFilename = value;
      }
   }

   /// <summary>
   /// Specify the URL reference used when the thumbnail or text link is clicked.
   /// </summary>
   public string ClickURL
   {
      get
      {
         return _clickURL;
      }
      set
      {
         _clickURL = value;
      }
   }

   /// <summary>
   /// Set/Get the CSS class name used for styling the page.
   /// </summary>
   public string CssClass
   {
      get
      {
         return _panelLabelBrowserItem.CssClass;
      }
      set
      {
         _panelLabelBrowserItem.CssClass = value;
      }
   }
   #endregion
}

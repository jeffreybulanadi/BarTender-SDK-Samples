using System;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;

public partial class FormatList : System.Web.UI.UserControl
{
   #region Private Member Variables
   private string _selectedLabelName = "";        // Selected label format name.
   #endregion

   #region Web Methods
   /// <summary>
   /// Called when the page loads
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
   }

   /// <summary>
   /// Called when the page is initialized
   /// </summary>
   protected void Page_Init(object sender, EventArgs e)
   {
      // Get the repository folder from Web.Config.
      string labelRepositoryFolder = ConfigurationManager.AppSettings["LabelRepository"];

      // Get a list of all label formats found in the repository.
      SearchOption searchOption = SearchOption.TopDirectoryOnly;
      string path = Server.MapPath(labelRepositoryFolder);
      string[] sFileNames = Directory.GetFiles(path, "*.btw", searchOption);

      if (sFileNames.Length != 0)
      {
         // Add each label format to the list control.
         for (int i = 0; i < sFileNames.Length; i++)
         {
            string sName = Path.GetFileName(sFileNames[i]);

            ListItem item = new ListItem(sName, sName);
            _listFormats.Items.Add(item);
         }

         if (!string.IsNullOrEmpty(_selectedLabelName))
            _listFormats.SelectedValue = _selectedLabelName;
      }
      else
      {
         string itemText = "No Labels Available";
         _listFormats.Enabled = false;
         _listFormats.Items.Add(new ListItem(itemText, itemText));
         _listFormats.SelectedValue = itemText;
      }

      if (Page.IsPostBack)
      {
         _selectedLabelName = Request.Form.Get(_listFormats.UniqueID);

         if (_selectedLabelName != null)
            _listFormats.SelectedValue = _selectedLabelName;
      }
   }
   #endregion

   #region Public Properties
   /// <summary>
   /// Does the label format list have any formats.
   /// </summary>
   public bool HasLabelFormats
   {
      get
      {
         return _listFormats.Enabled;
      }
   }

   /// <summary>
   /// Sets/Gets the system's full repository path to the selected label.
   /// </summary>
   public string SelectedLabelFormatRepositoryFullPath
   {
      get
      {
         // Get the repository folder from Web.Config.
         string labelRepositoryFolder = ConfigurationManager.AppSettings["LabelRepository"];

         // Get full path to folder.
         string repositoryPath = Server.MapPath(labelRepositoryFolder);
         string formatPath = string.Empty;

         if (!string.IsNullOrEmpty(_selectedLabelName))
            formatPath = Path.Combine(repositoryPath, _selectedLabelName);
         else if (_listFormats.SelectedItem != null)
            formatPath = Path.Combine(repositoryPath, _listFormats.SelectedValue);
         else
            formatPath = "";

         return formatPath;
      }
   }

   /// <summary>
   /// Sets/Gets the selected label format name. This is the name only.
   /// </summary>
   public string SelectedLabelFormatName
   {
      get
      {
         return _selectedLabelName;
      }
      set
      {
         if (_listFormats != null)
         {
            _listFormats.SelectedValue = value;
            _selectedLabelName = value;
         }
      }
   }
   #endregion

}

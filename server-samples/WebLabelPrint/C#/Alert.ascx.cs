using System;

public partial class Alert : System.Web.UI.UserControl
{
   #region Web Methods
   /// <summary>
   /// Called when the page is loaded.
   /// </summary>
   protected void Page_Load(object sender, EventArgs e)
   {
      // Add JavaScript to the OnClick event of the close link to hide the alert box.
      string hideJavaScript = String.Format("document.getElementById('{0}').style.display = 'none';", _panelAlert.ClientID);
      _linkClose.Attributes["OnClick"] = hideJavaScript;
   }
   #endregion

   #region Public Methods
   /// <summary>
   /// Add a new message to the alert.
   /// </summary>
   /// <param name="msg">Message text.</param>
   public void AddMessage(string msg)
   {
      if (Message.Length > 0)
         Message += "<hr/>";
      Message += msg;
   }

   /// <summary>
   /// Show/Hide the alert panel.
   /// </summary>
   /// <param name="show">True to show alert panel, else False.</param>
   public void ShowAlertPanel(bool show)
   {
      if (show)
      {
         _panelAlert.Style["display"] = "block";
      }
      else
      {
         _panelAlert.Style["display"] = "none";
      }
   }
   #endregion

   #region Public Properties
   /// <summary>
   /// Set/Get the alert message. Will show the panel if a message is set, or will hide 
   /// the panel if the message is an empty string.
   /// </summary>
   public string Message
   {
      set
      {
         _labelMessage.Text = value;
         ShowAlertPanel(_labelMessage.Text.Length != 0);
      }
      get
      {
         return _labelMessage.Text;
      }
   }

   /// <summary>
   /// Get the message client ID.
   /// </summary>
   public string MessageClientID
   {
      get
      {
         return _labelMessage.ClientID;
      }
   }

   /// <summary>
   /// Get the panel client ID
   /// </summary>
   public string PanelClientID
   {
      get
      {
         return _panelAlert.ClientID;
      }
   }
   #endregion
}

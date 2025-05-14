using System;
using System.Web.UI.WebControls;

public partial class _Default : System.Web.UI.Page 
{
   #region Web Methods   
   protected void Page_Load(object sender, EventArgs e)
   {
    // Show the current menu selection by highlighting the selected menu link's background.
    ((HyperLink)Master.FindControl("_home")).CssClass = "MenuLinkSelected";
   }
   #endregion
}

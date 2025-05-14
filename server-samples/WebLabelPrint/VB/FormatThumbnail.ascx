<%@ Control Language="vb" AutoEventWireup="true" CodeFile="FormatThumbnail.ascx.vb" Inherits="FormatThumbnail" %>
<asp:Panel ID="panelLabelBrowserItem" runat="server">
	<asp:HyperLink ID="imageThumbnailLink" Enabled = "false" runat = "server">
	<asp:Image ID="imgThumbnail" cssClass="LabelImage" runat="server" />
	</asp:HyperLink>
	<asp:HyperLink ID="textThumbnailLink" cssClass="LabelText" Enabled = "false" runat = "server">
		<asp:Label ID="lblThumbnailText"  runat="server" Text="Label"></asp:Label>
	</asp:HyperLink>
</asp:Panel>
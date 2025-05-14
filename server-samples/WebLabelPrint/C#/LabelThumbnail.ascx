<%@ Control Language="C#" AutoEventWireup="true" CodeFile="LabelThumbnail.ascx.cs" Inherits="LabelThumbnail" %>
<asp:Panel ID="_panelLabelBrowserItem" runat="server">
    <asp:HyperLink ID="_imageThumbnailLink" Enabled = "false" runat = "server">
    <asp:Image ID="_imageThumbnail" cssClass="LabelImage" runat="server" />
    </asp:HyperLink>
    <asp:HyperLink ID="_textThumbnailLink" cssClass="LabelText" Enabled = "false" runat = "server">
        <asp:Label ID="_labelThumbnailText"  runat="server" Text="Label"></asp:Label>
    </asp:HyperLink>
</asp:Panel>

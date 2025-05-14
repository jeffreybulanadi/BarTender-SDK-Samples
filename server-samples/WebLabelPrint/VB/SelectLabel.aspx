<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="SelectLabel.aspx.vb" Inherits="SelectLabel" Title="Select Label Format" %>
<%@ Register TagPrefix="Seagull" TagName="FormatList" Src="FormatList.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="LabelThumbnail" Src="LabelThumbnail.ascx" %>
<%@ Register TagPrefix="Seagull" tagname="Alert" src="Alert.ascx"%>

<asp:Content ID="_content" ContentPlaceHolderID="_content" Runat="Server">

   <h1>
	Select Label Format</h1>
	<p>
		Select and print label formats from the web repository.</p>
<br />
	<Seagull:Alert ID="_alert" runat="server" />
	<asp:Panel ID="_labelBrowser" CssClass="LabelBrowser" runat="server">
	</asp:Panel>
	<asp:Panel ID="_panelPrintMethod" CssClass="PrintMethod" runat="server">
		<span class="PrintMethodLabel"> Print Method: </span>    
		<asp:DropDownList ID="_listPrintMethod" runat="server" AutoPostBack="True" CssClass="PrintMethodList">
			<asp:ListItem  Selected="True" Value="client">Over Internet</asp:ListItem>
			<asp:ListItem Value="server">Use Standard Windows Printing</asp:ListItem>
		</asp:DropDownList>
	</asp:Panel>
	<br />
</asp:Content>
<%@ Page Language="vb" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Printers.aspx.vb" Inherits="ServerPrinters" Title="Display Server Printers" %>
<%@ Register TagPrefix="Seagull" tagname="Alert" src="Alert.ascx"%>

<asp:Content ID="_content" ContentPlaceHolderID="_content" Runat="Server">
<div style='display: none;'>
<object id='BarTenderPrintClient.Printers' classid='CLSID:13EB0B26-2C54-45FA-8AEC-60C98EEED492' codebase='.\BarTenderPrintClient\BarTenderPrintClient.cab#Version=1,10,0,0' style='display:none;'></object>
</div>
<script type="text/javascript" src="./JavaScript/BarTenderPrintClient.js" language="javascript"></script>
<script type="text/javascript" src="./JavaScript/WebLabelPrintSample.js" language="javascript"></script>

<h1>Server Printers</h1>
	<p>
		These printers are installed on the web server.</p>
<asp:GridView ID="_gridServerPrinters" runat="server" CssClass="PropertyTable" AutoGenerateColumns="False" EmptyDataText="No Printers are installed on the server.">
	<HeaderStyle CssClass="TableHeader" />
	<RowStyle CssClass="TableEvenRow" />
	<AlternatingRowStyle CssClass="TableOddRow" />
	<Columns>
		<asp:BoundField DataField="FriendlyName" HeaderText="Printer Name" />
		<asp:BoundField DataField="DriverName" HeaderText="Driver Name" />
		<asp:BoundField DataField="Port" HeaderText="Port" />
		<asp:BoundField DataField="Default" HeaderText="Default Printer" />
	</Columns>
	<EmptyDataRowStyle CssClass="Alert" Height="100%" Width="100%" />
</asp:GridView>
<Seagull:Alert ID="_noPrintersErrorAlert" runat="server" Visible="false" Message ="There are no printers installed on the server."/>
<br />
<h1>Client Printers</h1>
		These printers are installed on the local client.<br />
<asp:Panel ID="_panelClientPrinting" runat="server">
<asp:Table ID="_tableClientPrinters" runat="server" CssClass="PropertyTable">
	<asp:TableRow runat="server" CssClass="TableHeader" TableSection="TableHeader">
		<asp:TableCell runat="server">Printer Name</asp:TableCell>
		<asp:TableCell runat="server">Driver Name</asp:TableCell>
		<asp:TableCell runat="server">Default Printer</asp:TableCell>
	</asp:TableRow>
</asp:Table>
<Seagull:Alert ID="_noClientPrintersErrorAlert" runat="server" Message ="There are no printers installed on the client computer."/>
<br />
<h1>Client Printers Available for Internet Printing</h1>
		These printers are installed on the local client and are available for internet printing. Only
		printer models with a matching printer model server side are available.
		<br />
<asp:Table ID="_tableAvailableClientPrinters" runat="server" CssClass="PropertyTable">
	<asp:TableRow runat="server" CssClass="TableHeader" TableSection="TableHeader">
		<asp:TableCell runat="server">Printer Name</asp:TableCell>
		<asp:TableCell runat="server">Matching Server Printer</asp:TableCell>
	</asp:TableRow>
</asp:Table>
<Seagull:Alert ID="_noMatchingClientPrintersErrorAlert" runat="server" Message ="There are no printers installed on the client computer with drivers that match printers installed on the web server computer."/>
</asp:Panel>

<Seagull:Alert ID="_alert" runat="server" />


</asp:Content>
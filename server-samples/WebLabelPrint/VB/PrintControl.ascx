<%@ Control Language="vb" AutoEventWireup="true" CodeFile="PrintControl.ascx.vb" Inherits="PrintControl" %>
<%@ Register TagPrefix="Seagull" TagName="FormatList" Src="FormatList.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="PrinterListControl" Src="PrinterListControl.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="CopyControl" Src="CopyControl.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="SubstringsControl" Src="SubstringsControl.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="PromptControl" Src="PromptControl.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="QueryPromptsControl" Src="QueryPromptsControl.ascx" %>
<%@ Register TagPrefix="Seagull" tagname="Alert" src="Alert.ascx"%>

   <script type="text/javascript" src="./JavaScript/BarTenderPrintClient.js" language="javascript"></script>
   <script type="text/javascript" src="./JavaScript/WebLabelPrintSample.js" language="javascript"></script>
   <div style='display: none;'>
<object id='BarTenderPrintClient.Printer' 
	  classid='CLSID:E2F6F4B7-E96D-44D7-B081-59427EA64AC' 
	  codebase='./BarTenderPrintClient/BarTenderPrintClient.cab#Version=1,10,0,0' 
	  style='display: none;'></object></div>
	<h1><asp:Label ID="_labelTitle" runat="server">Print Label Formats</asp:Label></h1>
	<p><asp:Label ID="_labelDescription" runat="server">Print Page Description</asp:Label></p>
		<br />
		<div class ="PrintPanel">
			<div class ="TableHeader">

				Print
			</div>
			<table>
				<tr>
					<td >
						Label Format:</td>
					<td ><Seagull:FormatList ID="_listLabelFormats" runat="server"></Seagull:FormatList></td>
				</tr>
				<tr>
					<td>Printer Name:</td>
					<td ><Seagull:PrinterListControl ID="_listPrinters" runat="server"></Seagull:PrinterListControl></td>
				</tr>
				<tr>
					<td></td>
					<td ><asp:Button ID="_buttonPrint" runat="server" Text="Print" OnClick="ButtonPrint_Click" />&nbsp;
					   <asp:Button ID="_buttonPrintPreview" runat="server" onclick="ButtonPrintPreview_Click" 
						  Text="Print Preview" />
					</td>
				</tr>
				<tr>
					<td colspan = "2">
					   <Seagull:Alert ID="_alert" runat="server" />
					</td>
				</tr>
			</table>    
		</div>   
		<Seagull:CopyControl ID="_copyControl" runat = "server" />
		<Seagull:QueryPromptsControl ID="_queryPromptsControl" runat = "server" />
		<Seagull:SubstringsControl ID="_subStringsControl" runat = "server" />
		<Seagull:PromptControl ID="_promptControl" runat = "server" />

	<br /><br />
	<asp:HiddenField runat="server" ID="_hiddenClientPrintCode"/>
<%@ Control Language="vb" AutoEventWireup="true" CodeFile="PrinterList.ascx.vb" Inherits="PrinterList" %>
<%@ Register src="Alert.ascx" tagname="Alert" tagprefix="Seagull" %>

<object id='BarTenderPrintClient.Printers' classid='CLSID:13EB0B26-2C54-45FA-8AEC-60C98EEED492' codebase='.\BarTenderPrintClient\BarTenderPrintClient.cab#Version=1,7,0,0' style='display:none;'></object>

<script type="text/javascript" src="./JavaScript/ClientPrinting.js" language="javascript"></script>

<asp:DropDownList ID="lstPrinters" AutoPostBack="false" runat="server">
</asp:DropDownList>
<asp:HiddenField runat="server" ID="hfPrintLicense"/>
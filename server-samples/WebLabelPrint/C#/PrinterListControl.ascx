<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrinterListControl.ascx.cs" Inherits="PrinterList" %>
<%@ Register src="Alert.ascx" tagname="Alert" tagprefix="Seagull" %>
<div style='display: none;'>
<object id='BarTenderPrintClient.Printers' classid='CLSID:13EB0B26-2C54-45FA-8AEC-60C98EEED492' codebase='.\BarTenderPrintClient\BarTenderPrintClient.cab#Version=1,10,0,0' style='display: none;'></object>
</div>
<script type="text/javascript" src="./JavaScript/BarTenderPrintClient.js" language="javascript"></script>
<script type="text/javascript" src="./JavaScript/WebLabelPrintSample.js" language="javascript"></script>
   
<asp:DropDownList ID="_listPrinters" AutoPostBack="false" runat="server">
</asp:DropDownList>
<asp:HiddenField runat="server" ID="_hiddenPrintLicense"/>
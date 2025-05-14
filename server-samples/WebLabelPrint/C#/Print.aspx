<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" EnableEventValidation="false" CodeFile="Print.aspx.cs" Inherits="Print" Title="Print Label Format" %>
<%@ Register TagPrefix="Seagull" TagName="PrintPreviewControl" Src="PrintPreviewControl.ascx" %>
<%@ Register TagPrefix="Seagull" TagName="PrintControl" Src="PrintControl.ascx" %>

<asp:Content ID="_content" ContentPlaceHolderID="_content" Runat="server">
   <asp:Panel ID="_panelPrint" runat="server">
      <Seagull:PrintControl ID="_controlPrint" runat="server" />
   </asp:Panel>
   <asp:Panel ID="_panelPrintPreview" runat="server">
      <Seagull:PrintPreviewControl ID="_controlPrintPreview" runat="server" />
   </asp:Panel>
</asp:Content>


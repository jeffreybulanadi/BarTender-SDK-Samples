<%@ Control Language="vb" AutoEventWireup="true" CodeFile="CopyControl.ascx.vb" Inherits="CopyControl" %>
<%@ Register src="Alert.ascx" tagname="Alert" tagprefix="Seagull" %>

<asp:Panel ID="_panelCopies" CssClass="PrintPanel" runat="server">
   <div class ="TableHeader">
	   Copy Control
   </div>
   <asp:Table style="float:left; clear: left;" ID="_tableCopies" runat="server">
   </asp:Table>
</asp:Panel>
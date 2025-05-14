<%@ Control Language="vb" AutoEventWireup="true" CodeFile="Alert.ascx.vb" Inherits="Alert" %>
<asp:Panel ID="_panelAlert" runat="server" CssClass="Alert">
   <table>
	   <tr>
		   <td class="AlertMessage">
			   <asp:Label ID="_labelMessage" CssClass="AlertContent" runat="server" Text="">Alert Message</asp:Label>
		   </td>
		   <td class="AlertClose">
			   <asp:HyperLink ID="_linkClose" runat="server">Close</asp:HyperLink>
		   </td>
		</tr>
	</table>
</asp:Panel>
<%@ Control Language="C#" AutoEventWireup="true" CodeFile="PrintPreviewControl.ascx.cs" Inherits="PrintPreviewControl" %>
<%@ Register TagPrefix="Seagull" TagName="FormatList" src="FormatList.ascx" %>
<%@ Register src="Alert.ascx" tagname="Alert" tagprefix="uc1" %>

<script type="text/javascript" language="javascript">
function onPreviewImageError(image) 
{
   image.style.display = 'none';
   var alert = document.createElement("div");
   alert.innerHTML = "Preview image is not available. Your session may have timed out. Return to the 'Print Label Format' page and try again.";
   alert.className = "Alert";

   var parentNode = image.parentNode;

   parentNode.replaceChild(alert,image);

   // disable onerror to prevent endless loop
   image.onerror = "";
   return true;
}
</script>

<asp:Button id="_buttonBack" runat="server" Text="Back to Print Label Format" 
   onclick="ButtonBack_Click" />
<br />
<br />
<h1>Print Preview: <asp:Label ID="_labelFormatFileName" runat="server"></asp:Label>
                  </h1>
    <uc1:Alert ID="_alert" runat="server" />
    <asp:Panel ID="_previewPanel" runat="server">
      <div class="PrintPreviewContainer">
         <asp:Panel ID="_panelToolbar" CssClass="PrintPreviewToolbar" runat="server">
            <table>
               <tr>
                  <td>
                     <asp:ImageButton ID="_buttonPrint" CssClass="NavButton" runat="server" OnClick="ButtonPrint_Click" 
                        ImageUrl="~/images/Printer.png" ToolTip="Print the label format." />
                  </td>
                  <td>
                  </td>
                  <td>
                     <asp:ImageButton ID="_buttonFirst" CssClass="NavButton" runat="server" OnClick="ButtonFirst_Click" 
                        ImageUrl="~/images/First.png" ToolTip="Move to first page." />
                  </td>
                  <td>
                     <asp:ImageButton ID="_buttonPrevious" CssClass="NavButton" runat="server" OnClick="ButtonPrevious_Click" 
                        ImageUrl="~/images/Previous.png" ToolTip="Move to previous page." />
                  </td>
                  <td>
                    Page
                    <asp:Label ID="_labelCurrentPage" runat="server" Text="0"></asp:Label>
                    of
                    <asp:Label ID="_labelTotalPages" runat="server" Text="0"></asp:Label>
                  </td>
                  <td>
                    <asp:ImageButton ID="_buttonNext" runat="server" OnClick="ButtonNext_Click" 
                        ImageUrl="~/images/Next.png" CssClass="NavButton" ToolTip="Move to next page." />
                  </td>
                  <td>
                     <asp:ImageButton ID="_buttonLast" runat="server" OnClick="ButtonLast_Click" 
                        ImageUrl="~/images/Last.png" CssClass="NavButton" ToolTip="Move to last page." />
                  </td>
                  <td class="LabelFileName">
                     &nbsp;</td>
               </tr>
            </table>
         </asp:Panel>
         <div class="PrintPreviewBackground" id="_printPreviewBackground">
            <asp:Image ID="_imagePreview" CssClass="PrintPreviewImage" runat="server" AlternateText="Preview image is not available." />
         </div>
       </div>
     </asp:Panel>
    <br />

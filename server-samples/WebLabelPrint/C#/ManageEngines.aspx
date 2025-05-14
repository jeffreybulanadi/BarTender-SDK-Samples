<%@ Page Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="ManageEngines.aspx.cs" Inherits="ManageEngines" Title="Manage Print Engines" %>
<%@ Register TagPrefix="Seagull" Src="Alert.ascx" TagName="Alert" %>

<asp:Content ID="_content" ContentPlaceHolderID="_content" Runat="Server">
   <asp:Panel ID="_managePrintEnginesContent" CssClass="ManagePrintEnginesContent" runat="server">
<h1>Manage BarTender Print Engines</h1>
    <p>
        Start and stop BarTender print engines in the background. Add or remove print engines
        as print load increases or declines.</p>
   <br />
   <br />
      <asp:Panel ID="_panelStartStop" CssClass="StartStopPanel" runat="server">
       <table>
          <tr>
             <td class="Column1">
                Print Engines:
             </td>
             <td class="Column2">
                <asp:Button ID="_buttonStartEngines" CssClass="Button" runat="server" Text="Start" 
                   onclick="ButtonStartEngines_Click" />
             </td>
             <td class="Column3">
                <asp:Button ID="_buttonStopEngines" CssClass="Button" runat="server" Text="Stop" 
                   onclick="ButtonStopEngines_Click" />
             </td>
          </tr>
          <tr>
             <td class="Column1">
                Number of Print Engines:
             </td>
             <td class="Column2">
                <asp:TextBox ID="_textNumberPrintEngines" runat="server" CssClass="TextBox" 
                   MaxLength="1">1</asp:TextBox>
             </td>
             <td class="Column3">
                <asp:Button ID="_buttonChangeNumPrintEngines" runat="server" CssClass="Button" 
                   OnClick="ButtonChangeNumPrintEngines_Click" Text="Change" />
             </td>
          </tr>
       </table>
         <Seagull:Alert ID="_alert" runat="server" />
         <br />
    </asp:Panel>
    <br />

   <asp:Panel ID="_panelPrintEngineStatus" CssClass="StatusPanel" runat="server">
    <h1>
       Print Engine Status</h1>
       <p>
       View current status for task print engines that are running.
       </p>
       <br />
       <br />
            <div class="StatusContent">
              <table class="PropertyTable">
                 <thead class="TableHeader">
                     <tr>
                         <td>Description</td>
                         <td>Value</td>
                     </tr>
                 </thead>
                 <tr>
                     <td>Print Engines Running</td>
                     <td><asp:Label ID="_labelNumPrintEngines" runat="server" Text="0"></asp:Label></td>
                 </tr>
                 <tr>
                     <td>Print Engines Busy</td>
                     <td><asp:Label ID="_labelBusyPrintEngines" runat="server" Text="0"></asp:Label></td>
                 </tr>
              </table>
               <asp:LinkButton ID="_linkBtnUpdateEngines" runat="server">Update</asp:LinkButton>
            </div>
      <br />
      <br />
    </asp:Panel>

    <asp:Panel ID="_panelTaskQueueStatus" CssClass="StatusPanel" runat="server">
       <h1>
          Task Queue Status</h1>
       <p>
       View current status of the task queue.
       </p>
       <br />
       <br />
            <div class="StatusContent">
              <table class="PropertyTable">
                 <thead class="TableHeader">
                     <tr>
                         <td>Description</td>
                         <td>Value</td>
                     </tr>
                 </thead>
                 <tr>
                     <td>Tasks in Queue</td>
                     <td><asp:Label ID="_labelNumTasks" runat="server" Text="0"></asp:Label></td>
                 </tr>
                 <tr>
                     <td>Task Queue Locked</td>
                     <td><asp:Label ID="_labelQueueLocked" runat="server" Text="0"></asp:Label></td>
                 </tr>
                 <tr>
                     <td>Task Queue Paused</td>
                     <td><asp:Label ID="_labelQueuePaused" runat="server" Text="0"></asp:Label></td>
                 </tr>
              </table>
              <asp:LinkButton ID="_linkBtnUpdateTasks" runat="server">Update</asp:LinkButton>
          </div>
       </asp:Panel>
    </asp:Panel>
   
</asp:Content>


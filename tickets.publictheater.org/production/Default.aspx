<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>
<%@ Register TagPrefix="nysf" TagName="ProdDetails" Src="~/usercontrols/ProdDetails.ascx" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2 class="SelfEvident">Production Details</h2>
    <div id="ProdDetails" class="Content">
        <nysf:ProdDetails runat="server" />
    </div>
</asp:Content>
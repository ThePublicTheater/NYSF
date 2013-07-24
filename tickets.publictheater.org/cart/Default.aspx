<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/OldStyleTickets.master" %>
<%@ Register TagPrefix="nysf" TagName="CartViewer" Src="~/usercontrols/CartViewer.ascx" %>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server"
>    <h2>Your Cart</h2>
    <div id="CartView" class="Content">
        <form runat="server">

            <nysf:CartViewer runat="server" />
        </form>
    </div>
</asp:Content>
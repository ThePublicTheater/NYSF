<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site1.master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/usercontrols/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="CheckoutWizard"
    Src="~/usercontrols/CheckoutWizard.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server"
>    <nysf:SessionHandler ID="SessionHandler1" AllowAnonymous="false" RequireSecureConnection="true"
runat="server" /></asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">  
      <h2>Checkout</h2>
    <div class="Content">
        <form id="Form1" runat="server">
            <nysf:CheckoutWizard ID="CheckoutWizard1" runat="server" />
        </form>
    </div>
</asp:Content>
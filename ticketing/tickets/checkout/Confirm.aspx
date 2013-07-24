<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="tickets.checkout.Confirmed" %>

<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<meta http-equiv="refresh" content="4; url=/default.aspx">
       
        <p class="SuccessMessage">
            Thank you! Your order has been processed. You should receive an email confirmation
            shortly.
        </p>
        <ul class="LinkMenu">
            <li><a href="/" title="Home" class="CommandLink">Home</a></li>
        </ul>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
 
  
    
</asp:Content>

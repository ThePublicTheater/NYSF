<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Confirm.aspx.cs" Inherits="tickets.login.Confirmed" %>

<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
<meta http-equiv="refresh" content="2; url=<% =REDIRECT_URL %>">
    <asp:Label runat="server" ID="message" ForeColor="Green" text="You have successfully logged in."></asp:Label>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
 
  
    
</asp:Content>

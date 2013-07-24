<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="tickets.production.Default" %>

<%@ Register Src="~/usercontrols/ProdDetails.ascx" TagPrefix="uc1" TagName="ProdDetails" %>
<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2 class="SelfEvident">Production Details</h2>
    <div id="ProdDetails" class="Content">
        <uc1:ProdDetails runat="server" ID="ProdDetails1" />
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Site1.Master" AutoEventWireup="true" CodeBehind="communications.aspx.cs" Inherits="tickets.account.communications_aspx" %>

<%@ Register Src="~/usercontrols/CommPrefsForm.ascx" TagPrefix="uc1" TagName="CommPrefsForm" %>
<%@ Register Src="~/usercontrols/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
    <uc1:SessionHandler runat="server" ID="SessionHandler" AllowAnonymous="false" RequireSecureConnection="true" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="EndOfHeadPlaceHolder" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
    <h2>Communication Preferences</h2>
    <div id="CommPrefs" class="Content">
        <form id="Form1" runat="server">
            <uc1:CommPrefsForm runat="server" ID="CommPrefsForm" />
        </form>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="EndOfBodyPlaceholder" runat="server">
</asp:Content>

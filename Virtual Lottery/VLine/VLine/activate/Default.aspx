<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VLine.register.Default" %>

<%@ Register Src="~/User Controls/AccountActivator.ascx" TagPrefix="uc1" TagName="AccountActivator" %>
<%@ Register Src="~/User Controls/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
    <uc1:SessionHandler runat="server" ID="SessionHandler" AllowTemporary="true" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div style="text-align:center;"><h2>Update Account</h2>
    <div class="Content">
        <form runat="server">
            <uc1:AccountActivator runat="server" ID="AccountActivator" />
        </form>
        </div>
        </div>
</asp:Content>

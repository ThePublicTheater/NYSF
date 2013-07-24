<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site1.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="VLine.recover.Default" %>

<%@ Register Src="~/User Controls/AccountFinder.ascx" TagPrefix="uc1" TagName="AccountFinder" %>
<%@ Register Src="~/User Controls/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
  
    <div style="text-align:center">
          <h2>Account Lookup</h2>
        <form id="Form1" runat="server">
            <uc1:AccountFinder runat="server" ID="AccountFinder" EmailTemplateNumber="128" />
        </form>
    </div>
</asp:Content>

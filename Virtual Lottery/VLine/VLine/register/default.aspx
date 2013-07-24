<%@ Page Title="" Language="C#" MasterPageFile="~/WebForms/Site1.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="VLine.WebForms.Register" %>

<%@ Register Src="~/User Controls/SessionHandler.ascx" TagPrefix="uc1" TagName="SessionHandler" %>
<%@ Register Src="~/User Controls/RegistrationWizard.ascx" TagPrefix="uc1" TagName="RegistrationWizard" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
        
    <uc1:SessionHandler runat="server" ID="SessionHandler" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <form id="Form1" runat="server">
    <uc1:RegistrationWizard runat="server" id="RegistrationWizard" EmailTemplateNumber="127"  />
        </form>
</asp:Content>

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AccountExistsCheck.ascx.cs"
	Inherits="Nysf.UserControls.AccountExistsCheck"
%><div class="InputRow">
	<asp:TextBox ID="EmailInput" CssClass="FieldScaleX20" MaxLength="80" runat="server" />
	<asp:Button Text="Submit" OnClick="CheckAddress" runat="server" />
</div>
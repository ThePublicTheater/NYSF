<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CartForm.ascx.cs" Inherits="Nysf.Web.UserControls.CartForm" %>
<asp:Literal ID="OutputLiteral" runat="server" />
<div class="SubmitSet">
	<asp:Button ID="Label_Checkout" Text="Checkout" OnClick="Do_RedirectCheckout" runat="server" />
</div>
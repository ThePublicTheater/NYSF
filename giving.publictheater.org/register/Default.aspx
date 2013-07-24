<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="RegisterForm" Src="~/UserControls/RegisterForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowAuthenticated="false" RequireSsl="true" SetLastPage="false"
			runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Register - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Register</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Create a new account</h2>
	<div class="Content">
		<form runat="server">
			<aside>
				<p>
					Already registered? <a href="/login">Sign in</a> or <a href="/recover">look up</a> your account.
				</p>
			</aside>
			<p>
				Please fill out the form below to register.
			</p>
			<nysf:RegisterForm SendConfirmationEmail="true" EmailTemplateId="106" NewAccountAttributeValuePairs="cp_Em_The Public Theater=1" runat="server" />
		</form>
		<section>
			<h3 class="SelfEvident">Other options</h3>
			<ul class="SubmitSet">
				<li>
					<a href="/register">Create a new account</a>
				</li>
				<li>
					<a href="/recover">Find my username / password</a>
				</li>
			</ul>
		</section>
	</div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="SessionHandler" Src="~/UserControls/SessionHandler.ascx" %>
<%@ Register TagPrefix="nysf" TagName="LoginForm" Src="~/UserControls/LoginForm.ascx" %>

<asp:Content ContentPlaceHolderID="SessionHandlerPlaceHolder" runat="server">
	<nysf:SessionHandler AllowAuthenticated="false" RequireSsl="true" SetLastPage="false"
			runat="server" />
</asp:Content>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Login - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Login</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2 class="SelfEvident">Sign in</h2>
	<div class="Content">
		<form runat="server">
			<p>
				Enter your username and password.
			</p>
			<nysf:LoginForm runat="server" />
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

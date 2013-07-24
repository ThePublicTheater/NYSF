<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="OrderForm" Src="~/UserControls/FellowsOrderForm.ascx" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Fellows - Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">FellowsOrder</asp:Content>

<asp:Content ContentPlaceHolderID="BodyOpenTagEndPlaceHolder" runat="server">class="GivingForm"</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>The Public Theater Fellows Program</h2>
	<div class="Content">
		<p>
			To date, The Public Theater has distributed over 5 million tickets to performances at the Delacorte Theater in Central Park, and we continue to welcome audiences to our historic home on Lafayette Street, which contains five intimate theaters, and our recently
 renovated cabaret space, Joe’s Pub. 
		</p>
		<p>
			Our dedicated group of supporters account for over 75% of our annual budget, and by joining their ranks today as a Fellow, you will ensure that the values and artistry which have distinguished
 The Public for decades will flourish in the years to come.
		</p>
		<form runat="server">
			<nysf:OrderForm runat="server" />
		</form>
	</div>
	<footer>
		<p>
			If you have trouble making your gift online or have a question, please contact the Partners Desk at 212-539-8734 or partners@publictheater.org.
		</p>
	</footer>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/Master.master" AutoEventWireup="true"
		CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">Home</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<p style="font-weight:bold;padding-bottom:1em">Due to audience demand, UTR Paks for this year’s festival are now sold-out.</p>
	<hgroup>
		<h2>The UTR Pak: 5 shows for $75</h2>
		<h3>See five shows and save up to $50!</h3>
	</hgroup>
	<div class="Content">
		<%--<aside>
			<a href="/order">Order now</a>
		</aside>--%>
		<p>
			The UTR Pak entitles the buyer to admission to any five UTR shows* in any venue for only $75.
		</p>
		<p>
			Save $25 on the cost of your tickets ($100 value) and avoid ticket fees (an additional $25 in savings)
		</p>
		<p>
			Priority Booking: Get a head start on the festival and book your tickets during the priority booking period November 10 through December 6.
		</p>
		<p>
			Flexible Scheduling: Tickets may be booked for events at The Public anytime before curtain and for events at partner venues up to 48 hours prior to curtain time, subject to availability.  No exchange fees.
		</p>
		<p>
			Share the love: 2 tickets can be redeemed for one production, so you can bring a friend along.
		</p>
	</div>
	<footer>
		<small>*Except for one-night only events <i>Camille O'Sullivan: Feel</i> and <i>The Plot is The Revolution</i>. For further ticketing policies, <a href="/policies" title="Ticketing Policies">click here</a>.</small>
	</footer>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>
<%@ Register TagPrefix="nysf" TagName="OrderForm" Src="~/UserControls/GoingPublicOrderForm.ascx" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">
	Support the Public
</asp:Content>

<asp:Content ContentPlaceHolderID="BodyIdPlaceHolder" runat="server">SupportOrder</asp:Content>

<asp:Content ContentPlaceHolderID="BodyOpenTagEndPlaceHolder" runat="server">class="GivingForm"</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>GIVE A GIFT OF ANY AMOUNT</h2>
	<div class="Content">
		<p>
		Our supporters are a dedicated group of individuals whose contributions help The Public advance our mission of supporting the work of emerging and established artists and providing access to theater for all.
		</p>
		<p>
		Over 75% of The Public’s annual budget comes from the financial support of our generous donors. Their gifts help ensure our financial stability and the development and expansion of our outreach programming, low-cost ticketing initiatives, and free Shakespeare in the Park.
		</p>
		<p>
		<strong>Contributions to The Public Theater of $55 or more are eligible for a variety of benefits in thanks -- for more information about benefits at specific levels of support, visit:</strong>
		</p>
		<p>
		<a href="http://dev.giving.publictheater.org/partners/">The Partners Program</a> for donations of $1,000 or more. 
		</p>
		<p>
		<a href="http://dev.giving.publictheater.org/fellows/">The Fellows Program</a> for donations of $55 - $1000.
		</p>
		<p>
		<a href="http://youngpartners.publictheater.org/">The Young Partners Program.</a> for donors in their 20s or 30s.
		</p>
		<div id='razoo_donation_widget'><span><a href="http://www.razoo.com/story/New-York-Shakespeare-Festival">Donate to The Public Theater</a> at <a href="http://www.razoo.com/">Razoo</a></span></div><script type='text/javascript'>var r_params = {"title":"The Public Theater","short_description":"As the nation's foremost theatrical producer of Shakespeare and new work, The Public Theater is dedicated to achieving artistic excellence w","long_description":"For over 50 years, The Public has been proud to bring free performances to over 100,000 people each summer at the Delacorte Theater in Central Park. This long-standing tradition has joined esteemed artists with audiences from all over the world to celebrate Shakespeare and other classic works. Free tickets are available both in person and online to the general public. Outreach initiatives, student programs, and borough ticket distribution help bring these productions to a broad cross-section of the community.","color":"#ee221b","errors":false,"donation_options":{"15":"$15 Gift","30":"$30 Gift","50":"$50 Gift"},"image":"true"};var r_protocol=(("https:"==document.location.protocol)?"https://":"http://");var r_path='www.razoo.com/javascripts/widget_loader.js';var r_identifier='New-York-Shakespeare-Festival';document.write(unescape("%3Cscript id='razoo_widget_loader_script' src='"+r_protocol+r_path+"' type='text/javascript'%3E%3C/script%3E"));</script>
		<p>
		If you have any questions or if you are having any trouble making your gift online, please contact our Call Center at 212.967.7555. Thank you for your support.
		</p>
	</div>
</asp:Content>

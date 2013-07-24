<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Master.Master" %>

<asp:Content ContentPlaceHolderID="TitlePlaceHolder" runat="server">Fellows Program</asp:Content>

<asp:Content ContentPlaceHolderID="BodyOpenTagEndPlaceHolder" runat="server">class="BenefitsList"</asp:Content>

<asp:Content ContentPlaceHolderID="MainContentPlaceHolder" runat="server">
	<h2>Fellows Program Benefits</h2>
	<nav>
		<h3 class="SelfEvident">Contribution Levels</h3>
		<ul>
			<li><a href="#Level3">$400 - Champion</a></li>
            <li><a href="#Level5">$1000 - Partner</a></li>
		</ul>
	</nav>
	<div class="Content">


		<section id="Level3">
			<hgroup>
				<h3>Champion</h3>
				<h4>$400</h4>
			</hgroup>
			<div class="Content Single">
				<div>
	<ul><li>2 reserved seats to Shakespeare in the Park
	</li><li>2 discounted tickets to Public Theater productions
	</li><li>Opportunity to purchase 2 Summer Supporter tickets for the 2013 Shakespeare in the Park season at a discounted rate in advance of the general public*
	</li><li>Access to the Member Hotline to easily purchase tickets to Public Theater productions or reserve Summer Supporter tickets to Shakespeare in the Park before they go on-sale to the general public.
	</li><li>Exemption from reservation fees and service charges 
	</li><li>15% discount off the full price of an additional ticket to select shows.
	</li><li>Flexible rescheduling policies with nominal fees
	</li><li>Invitations to talkbacks and other special events throughout the season 
	</li><li>20% off food and beverages at Joe’s Pub
	</li><li>Discounts at neighborhood restaurants near The Public Theater</li></ul>
	<a href="/fellows/?l=3" title="Become a Champion">[choose this level]</a>
				</div>
			</div>
		</section>
		<section id="Level5">
			<hgroup>
				<h3>Partner</h3>
				<h4>$1,000</h4>
			</hgroup>
			<div class="Content Single">
				<div>
				<p>At this level, you are now a part of our Public Theater Partners Program. This entitles you to exclusive access to our Partners Desk (212-539-8734) to reserve House Seats to The Public Theater, Shakespeare in the Park and Broadway Productions produced in conjunction with The Public Theater along with the benefits below.</p>
	<ul><li>Four reserved seats to Shakespeare in the Park in priority Partner locations
	</li><li>Two tickets to a production of your choice at 425 Lafayette Street
	</li><li>2 discounted tickets to Public Theater productions
	</li><li>Invitation to select behind-the-scenes events
	</li><li>Recognition in Shakespeare in the Park Playbill as a Summer Partner of The Public Theater
	</li><li>Exemption from reservation fees and service charges 
	</li><li>15% discount off the full price of an additional ticket to select shows.
	</li><li>Flexible rescheduling policies with nominal fees
	</li><li>Invitations to talkbacks and other special events throughout the season 
	</li><li>20% off food and beverages at Joe’s Pub
	</li><li>Discounts at neighborhood restaurants near The Public Theater</li></ul>
	<a href="/fellows/?l=5" title="Become a Partner">[choose this level]</a>
				</div>
			</div>
		</section>
	</div>
	<footer>
		<small>*Reserved seats to Shakespeare in the Park through the Summer Supporter program will be available for a limited time in Spring 2013.</small>
	</footer>
</asp:Content>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GoingPublicOrderForm.ascx.cs" Inherits="Nysf.Apps.Giving.GoingPublicOrderForm" %>
<asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
<fieldset>
	<legend class="SelfEvident">Gift target</legend>
	<div id="GiftTargetOptions" class="Parallels2">
		<div class="TargetOption">
			<asp:RadioButton ID="Input_PublicTheater" GroupName="Input_Target" runat="server" />
			<asp:Label AssociatedControlId="Input_PublicTheater" runat="server">
				<header>Going Public Campaign</header>
				<div class="Content">
					<section>
						<header>Giving Levels</header>
						<div class="GivingLevelList">
							<div class="Item">
								<span class="Name">Supporters</span>
								<span class="Range">$1 - $999</span>
							</div>
							<div class="Item">
								<span class="Name">Friends</span>
								<span class="Range">$1,000 - $9,999</span>
							</div>
							<div class="Item">
								<span class="Name">Peers</span>
								<span class="Range">$10,000 - $24,999</span>
							</div>
							<div class="Item">
								<span class="Name">Contributors</span>
								<span class="Range">$25,000 - $99,999</span>
							</div>
							<div class="Item">
								<span class="Name">Angels</span>
								<span class="Range">$100,000 - $999,999</span>
							</div>
							<div class="Item">
								<span class="Name">Leaders</span>
								<span class="Range">$1,000,000+</span>
							</div>
						</div>
					</section>
					<section>
						<p>
							<a href="/goingpublic/naming" title="Naming Opportunities for Going Public Contributions to the Public Theater">Click here</a> for information on naming opportunities</p>
					</section>
				</div>
			</asp:Label>
		</div>
		<div class="TargetOption">
			<asp:RadioButton ID="Input_JoesPub" GroupName="Input_Target" runat="server" />
			<asp:Label AssociatedControlId="Input_JoesPub" runat="server">
				<header>Going Public for Joe's Pub</header>
				<div class="Content">
					<section>
						<header>Giving Levels</header>
						<div class="GivingLevelList">
							<div class="Item">
								<span class="Name">Opening Act</span>
								<span class="Range">$1 - $999</span>
							</div>
							<div class="Item">
								<span class="Name">Rising Star</span>
								<span class="Range">$1,000 - $4,999</span>
							</div>
							<div class="Item">
								<span class="Name">Headliner</span>
								<span class="Range">$5,000 - $9,999</span>
							</div>
							<div class="Item">
								<span class="Name">Diva</span>
								<span class="Range">$10,000 - $14,999</span>
							</div>
							<div class="Item">
								<span class="Name">Rock Star</span>
								<span class="Range">$15,000 - $24,999</span>
							</div>
							<div class="Item">
								<span class="Name">Icon</span>
								<span class="Range">$25,000+</span>
							</div>
						</div>
					</section>
					<section>
						<p>
							<a href="/goingpublic/naming" title="Naming Opportunities for Going Public Contributions to Joe's Pub">Click here</a> for information on naming opportunities
						</p>
					</section>
				</div>
			</asp:Label>
		</div>
	</div>
	<p class="Field">
		<asp:Label AssociatedControlID="Input_Amount" runat="server">I choose to make a gift of $</asp:Label>
		<asp:TextBox ID="Input_Amount" MaxLength="12" CssClass="FieldScaleX9" runat="server" />
		<asp:RequiredFieldValidator ControlToValidate="Input_Amount" CssClass="Warning" Display="Dynamic" Text="*" ErrorMessage="Please enter a gift amount." runat="server" />
		<asp:RegularExpressionValidator ControlToValidate="Input_Amount" CssClass="Warning" Display="Dynamic" Text="*" ErrorMessage="Please enter only numerals for your gift amount." ValidationExpression="^\d+$" runat="server" />
	</p>
</fieldset>
<fieldset>
	<legend>Gift preferences</legend>
	<div class="Field" id="Field_Acknowl">
		<div class="Description">
			Acknowledgement
		</div>
		<div class="Entry">
			<div class="InputSet">
				<asp:RadioButton ID="Input_DoAcknowl" GroupName="Input_Acknowl" Text="Please specify how you would like to be acknowledged for your gift:" runat="server" /><br />
				<asp:TextBox ID="Input_AcknowlName" CssClass="FieldScaleX20" MaxLength="60" runat="server" />
				<div class="Subtext">
					Examples: John & Jane Smith<br />
					Mr. John & Mrs. Jane Smith<br />
					Mr. & Mrs. Smith<br />
					John Smith<br />
					The Smith Family Foundation<br />
					In honor of Robert Smith<br />
					In memory of Robert Smith
				</div>
			</div>
			<div class="InputSet">
				<asp:RadioButton ID="Input_Anon" GroupName="Input_Acknowl" Text="I wish to make this donation anonymously" runat="server" />
			</div>
		</div>
	</div>
	<%--<div class="Field" id="Field_Tribute">
		<div class="Description">
			Tribute
		</div>
		<div class="Entry">
			<asp:RadioButtonList ID="Input_Tribute" RepeatLayout="Flow" runat="server">
				<asp:ListItem Text="This gift is in honor of a loved one" Value="1" />
				<asp:ListItem Text="This gift is in memory of a loved one" Value="2" />
				<asp:ListItem Text="Neither" Value="0" Selected="True" />
			</asp:RadioButtonList>
		</div>
	</div>--%>
	<div class="Field" id="Field_Matching">
		<div class="Description">
			Corporate matching
		</div>
		<div class="Entry">
			<div class="InputSet">
				<asp:CheckBox ID="Input_Matching" Text="I plan to apply for matching gifts from my company" runat="server" /><br />
				Company name: <asp:TextBox ID="Input_CorpName" MaxLength="60" CssClass="FieldScaleX15" runat="server" />
			</div>
		</div>
	</div>
</fieldset>
<p><small><strong>
	If you have any questions or if you are having any trouble making your gift online please contact campaign staff at:<br />
	T: 212.539.8632<br />
	E: <a href="mailto:goingpublic@publictheater.org">goingpublic@publictheater.org</a> or <a href="mailto:goingpublic@joespub.com">goingpublic@joespub.com</a>
</strong></small></p>
<div class="SubmitSet">
	<asp:Button ID="Button1" Text="Submit" OnClick="Do_AddContributionToCart" runat="server" />
</div>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FellowsOrderForm.ascx.cs" Inherits="Nysf.Apps.Giving.FellowsOrderForm" %>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<fieldset>
	<legend class="SelfEvident">Gift amount</legend>
	<ul id="GivingLevelOptions">
		<li class="Option">
			<span class="TitleSection">
				<asp:Label ID="Label3" CssClass="Title" AssociatedControlID="Input_L3" runat="server">$400 Champion</asp:Label>
				<a href="/fellows/benefits/#Level3">(View benefits)</a>
			</span>
			<span class="SelectorSection SoloOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L3" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<asp:Label ID="Label5" CssClass="Title" AssociatedControlID="Input_L5" runat="server">$1,000 Partner</asp:Label>
				<a href="/fellows/benefits/#Level5">(View benefits)</a>
			</span>
			<span class="SelectorSection SoloOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L5" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
	</ul>
</fieldset>
<p>
	<a href="/media/unmanaged/documents/fellows_brochure_11-12.pdf" title="50th Anniversary Fellows brochure">Click here for more information</a> about joining the 50th Anniversary Fellows Program.
</p>
<fieldset id="Fieldset1">
	<legend>Gift preferences</legend>
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
	<div class="Field" id="Field_DeclineBenefits">
		<div class="Description">
			Declination of benefits
		</div>
		<div class="Entry">
			<asp:CheckBox ID="Input_DeclineBenefits" Text="I wish my gift to be fully tax-deductible. Please do not provide any benefits." runat="server" />
		</div>
	</div>
</fieldset>
<div class="SubmitSet">
	<asp:Button ID="Button1" Text="Submit" OnClick="Do_AddContributionToCart" runat="server" />
</div>
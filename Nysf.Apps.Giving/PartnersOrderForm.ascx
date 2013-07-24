<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartnersOrderForm.ascx.cs" Inherits="Nysf.Apps.Giving.PartnersOrderForm" %>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<fieldset>
	<legend class="SelfEvident">Gift amount</legend>
	<ul id="GivingLevelOptions">
		<li class="Option">
			<span class="TitleSection">
				<asp:Label ID="Label1" CssClass="Title" AssociatedControlID="Input_L1" runat="server">$1,000 Summer Partner</asp:Label>
				<a href="/partners/benefits/#Level1">(View benefits)</a>
			</span>
			<span class="SelectorSection SoloOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L1" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<span class="Title">$2,000 Shiva Partner</span>
				<a href="/partners/benefits/#Level2">(View benefits)</a>
			</span>
			<span class="SelectorSection MultiOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L2A" Text="Option A" GroupName="Input_Level" runat="server" />
				</span>
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L2B" Text="Option B" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<span class="Title">$4,000 Martinson Partner</span>
				<a href="/partners/benefits/#Level3">(View benefits)</a>
			</span>
			<span class="SelectorSection MultiOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L3A" Text="Option A" GroupName="Input_Level" runat="server" />
				</span>
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L3B" Text="Option B" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<span class="Title">$6,000 Anspacher Partner</span>
				<a href="/partners/benefits/#Level4">(View benefits)</a>
			</span>
			<span class="SelectorSection MultiOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L4A" Text="Option A" GroupName="Input_Level" runat="server" />
				</span>
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L4B" Text="Option B" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<span class="Title">$10,000 Newman Partner</span>
				<a href="/partners/benefits/#Level5">(View benefits)</a>
			</span>
			<span class="SelectorSection MultiOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L5A" Text="Option A" GroupName="Input_Level" runat="server" />
				</span>
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L5B" Text="Option B" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
		<li class="Option">
			<span class="TitleSection">
				<span class="Title">$15,000 LuEsther Partner</span>
				<a href="/partners/benefits/#Level6">(View benefits)</a>
			</span>
			<span class="SelectorSection MultiOption">
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L6A" Text="Option A" GroupName="Input_Level" runat="server" />
				</span>
				<span class="SelectorSet">
					<asp:RadioButton ID="Input_L6B" Text="Option B" GroupName="Input_Level" runat="server" />
				</span>
			</span>
		</li>
	</ul>
</fieldset>
<p>
	<a href="/media/unmanaged/documents/partners_brochure_11-12.pdf" title="Partners Program brochure">Click here for more information</a> about joining the Partners Program.
</p>
<fieldset id="Fieldset1">
	<legend>Gift preferences</legend>
	<div class="Field" id="Field_Acknowl">
		<div class="Description">
			Acknowledgement
		</div>
		<div class="Entry">
			<div class="InputSet">
				<asp:RadioButton ID="Input_DoAcknowl" GroupName="Input_Acknowl" Text="Please specific how you would like to be listed in our Playbill:" runat="server" /><br />
				<asp:TextBox ID="Input_AcknowlName" CssClass="FieldScaleX20" MaxLength="60" runat="server" />
				<div class="Subtext">
					Examples: John & Jane Smith<br />
					Mr. John & Mrs. Jane Smith<br />
					John Smith
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
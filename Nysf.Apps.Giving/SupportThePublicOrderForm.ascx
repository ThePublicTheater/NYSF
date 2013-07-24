<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportThePublicOrderForm.ascx.cs" Inherits="Nysf.Apps.Giving.SupportThePublicOrderForm" %>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<h3>
    I would like to contribute
<asp:TextBox ID="Input_Contrib" runat="server" 
        Height="20px" ontextchanged="TextBox1_TextChanged" 
        style="padding:0px; margin-top: 0px; margin-bottom: 0px">$0</asp:TextBox>
    to support The Public Theater.
</h3>

<p>
Contributions to The Public Theater of $55 or more are eligible for a variety of benefits in thanks. For more inforation about benefits at specific levels of support, visit 
<a href="#" title="The Partners Program"> The Partners Program</a>,
<a href="#" title="The Fellows Program"> The Fellows Program</a>, or
<a href="#" title="The Young Partners Program"> The Young Partners Program</a>.
</p>

<fieldset id="Fieldset1">
	<legend>Gift preferences</legend>
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
</fieldset>
<div class="SubmitSet">
	<asp:Button ID="Button1" Text="Submit" OnClick="Do_AddContributionToCart" runat="server" />
</div>
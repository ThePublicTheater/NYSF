<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SupportThePublicOrderForm.ascx.cs" Inherits="Nysf.Apps.Giving.SupportThePublicOrderForm" %>
<asp:ValidationSummary ID="ValidationSummary1" CssClass="Warning" DisplayMode="BulletList" runat="server" />
<% 
	int test = -1;
    if (Int32.TryParse(Request.QueryString["cont"], out test))
    {
		Input_Contrib.Text = test.ToString();
    }
%>
<h4>
    I would like to contribute $
<asp:TextBox ID="Input_Contrib" runat="server" 
        Height="20px" style="padding:0px; margin-top: 0px; margin-bottom: 0px" 
        ReadOnly="false"></asp:TextBox>
    to support The Public Theater.
</h4>


<p><strong>Contributions to The Public Theater of $60 or more are eligible for a variety of benefits in thanks</strong> — for more information about benefits at specific levels of support, visit:</p>
<a href="/partners/Default.aspx" title="The Partners Program"> The Partners Program</a> for donations of $1,000 or more.<br />
<a href="/fellows/Default.aspx" title="The Fellows Program"> The Fellows Program</a> for donations of $60-$1,000.<br />
<a href="http://youngpartners.publictheater.org/" title="The Young Partners Program"> The Young Partners Program</a> for donors in their 20s or 30s.<br />

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
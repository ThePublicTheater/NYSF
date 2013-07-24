<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MailingOptInHandler.ascx.cs"
    Inherits="Nysf.UserControls.MailingOptInHandler"
%><asp:MultiView ID="OptInViews" ActiveViewIndex="0" runat="server">
	<asp:View ID="StartView" runat="server">
		<p id="Prompt" class="Prompt" runat="server">Click below to join our e-mailing list.</p>
		<div class="InputRow">
			<asp:Button ID="JoinButton" Text="Join!" OnClick="DoJoin" runat="server" />
		</div>
	</asp:View>
	<asp:View ID="DoneView" runat="server">
		<p class="SuccessMessage">
			You have been added to the mailing list for <asp:Literal ID="OrgTitleBlurb"
			runat="server" />.
		</p>
	</asp:View>
</asp:MultiView>
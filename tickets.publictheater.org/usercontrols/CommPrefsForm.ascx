<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CommPrefsForm.ascx.cs"
    Inherits="Nysf.UserControls.CommPrefsForm"
%><p id="SuccessMessage" class="SuccessMessage" runat="server">
    Your communication preferences have been updated.
</p>
<fieldset>
    <legend class="SelfEvident">Communication Routes</legend>
    <p>I would like to receive marketing communications by:</p>
    <dl>
        <dt>
            <asp:Label AssociatedControlID="WantPhoneField" runat="server">Phone</asp:Label>
        </dt>
        <dd>
            <asp:CheckBox ID="WantPhoneField" runat="server" />
        </dd>
        <dt>
            <asp:Label AssociatedControlID="WantMailField" runat="server">Postal
                mail</asp:Label>
        </dt>
        <dd>
            <asp:CheckBox ID="WantMailField" runat="server" />
        </dd>
        <dt>
            <asp:Label AssociatedControlID="WantEmailField"
                runat="server">E-mail</asp:Label>
        </dt>
        <dd>
            <asp:CheckBox ID="WantEmailField" AutoPostBack="true" runat="server" />
        </dd>
    </dl>
</fieldset>
<fieldset id="EmailTopicsFieldset" runat="server">
    <legend class="SelfEvident">Email Topics</legend>
    <p>I would like to receive emails about the following topics:</p>
    <asp:Panel ID="PtPanel" CssClass="OrgPrefSection" runat="server">
		<%-- disabled 'AutoPostBack="true"' for the following control --%>
        <asp:CheckBox ID="PtCheckBox" Text="Public Theater" runat="server" />
        <asp:CheckBoxList ID="PtSubCheckBoxes" RepeatLayout="UnorderedList"
            runat="server" />
    </asp:Panel>
    <asp:Panel ID="JpPanel" CssClass="OrgPrefSection" runat="server">
		<%-- disabled 'AutoPostBack="true"' for the following control --%>
        <asp:CheckBox ID="JpCheckBox" Text="Joe's Pub" runat="server" />
        <asp:CheckBoxList ID="JpSubCheckBoxes" RepeatLayout="UnorderedList"
            runat="server" />
    </asp:Panel>
    <asp:Panel ID="SitpPanel" CssClass="OrgPrefSection" runat="server">
		<%-- disabled 'AutoPostBack="true"' for the following control --%>
        <asp:CheckBox ID="SitpCheckBox" runat="server"
            Text="Shakespeare in the Park" />
        <asp:CheckBoxList ID="SitpSubCheckBoxes" RepeatLayout="UnorderedList"
            runat="server" />
    </asp:Panel>
</fieldset>
<div class="InputRow">
    <asp:Button Text="Back" OnClick="GoBack" runat="server" />
    <asp:Button ID="SubmitButton" Text="Submit" OnClick="Submit" runat="server" />
</div>
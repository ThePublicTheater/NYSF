<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ManageAccountMenu.ascx.cs"
    Inherits="Nysf.UserControls.ManageAccountMenu"
%><ul class="LinkMenu">
    <li>
        <asp:HyperLink ID="UpdateLink" ToolTip="Account Info Updater"
            runat="server">Update account info</asp:HyperLink>
    </li>
    <li>
        <asp:HyperLink ID="ChangePasswordLink" ToolTip="Password Updater"
            runat="server">Change password</asp:HyperLink>
    </li>
    <li>
        <asp:HyperLink ID="CommunicationsLink" ToolTip="Communications Management"
            runat="server">Manage communications</asp:HyperLink>
    </li>
</ul>
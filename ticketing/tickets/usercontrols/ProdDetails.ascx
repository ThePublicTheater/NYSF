<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProdDetails.ascx.cs"
    Inherits="Nysf.Apps.OldStyleTickets.ProdDetails"
%><asp:MultiView ID="ProductionViews" ActiveViewIndex="0" runat="server">
    <asp:View runat="server"><h3><asp:Literal ID="TitleBlurb" runat="server" /></h3>
        <div class="Content">
            <aside class="ProdLocationBlurb">
                <span class="SelfEvident">Showing at </span><asp:Literal ID="LocationBlurb"
                    runat="server" />
            </aside>
            <br />
            <asp:HyperLink ID="OrderLink" ToolTip="Order tickets for this production"
                CssClass="CommandLink" runat="server">Order tickets</asp:HyperLink>
            <br />
            <br />
            <span">Public Theater Members: Please call your Member Hotline to reserve your tickets.</span>

            <p id="SoldOutBlurb" class="Warning" runat="server" visible="false">
                <strong>SOLD OUT</strong>
            </p>
            <div class="Unvalidated">
                <asp:Literal ID="SynopsisBlurb" runat="server" />
                 <br />
                <asp:Literal ID="PreSaleBlurb" runat="server"></asp:Literal>
            </div>
            <span id="AssistancePrompt">For further assistance please call 212.967.7555</span>
        </div>
    </asp:View>
    <asp:View runat="server">
        <p class="Warning">Sorry, but the production cannot be found.</p>
    </asp:View>
</asp:MultiView>
<aside>
    <asp:HyperLink ID="CalendarLink" ToolTip="Performances calendar" CssClass="MajorNavLink"
        runat="server">Home</asp:HyperLink>
</aside>
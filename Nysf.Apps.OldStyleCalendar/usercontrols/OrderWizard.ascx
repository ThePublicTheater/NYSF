<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderWizard.ascx.cs"
    Inherits="Nysf.Apps.OldStyleTickets.OrderWizard"
%>
<h3 id="ShowNameHeader" runat="server">
    Reservations for <i><asp:Literal ID="PerfNameBlurb" runat="server" /></i>
</h3>
<asp:MultiView ID="OrderWizardViews" runat="server">
    <asp:View ID="PerfSelectionView" runat="server">
        <asp:Literal ID="VenueNameBlurb" runat="server" />
        <fieldset>
            <legend class="SelfEvident">Date Selection</legend>
            <p class="Prompt">
                Which performance would you like to attend?<asp:Literal ID="VenueNamesNote"
                visible="false" runat="server"><br />Venue names are in parentheses.</asp:Literal>
            </p>
            <div class="InputRow">
                <asp:RadioButtonList ID="PerfField" RepeatLayout="UnorderedList" runat="server" />
            </div>
        </fieldset>
        <div class="InputRow">
            <asp:Button Text="Next" OnClick="ShowZonePriceView" runat="server" />
            <asp:Button Text="Cancel" OnClick="ReturnToReferer" CausesValidation="false"
                runat="server" />
        </div>
    </asp:View>
    <asp:View ID="SeatChoiceView" runat="server">
        <p id="ReservationErrorMessage" class="Warning" visible="false" runat="server">
            Sorry, but the system was unable to reserve those tickets.
        </p>
        <p id="PromoCodeSuccessMessage" class="SuccessMessage" visible="false" runat="server">
            Your promotion code was accepted.
        </p>
        <p id="PromoCodeWarning" class="Warning" visible="false" runat="server">
            Sorry, but that promotion code is not recognized.
        </p>
        <p id="SeatAddSuccessMessage" class="SuccessMessage" visible="false" runat="server">
            A seat was added to your cart.
        </p>
        <p id="FailedSeatAddMessage" class="Warning" visible="false" runat="server">
            Sorry, but that seat has already been reserved.
        </p>
        <asp:Panel ID="SeatsAddSuccessPanel" Visible="false" runat="server">
            <p class="SuccessMessage">The following tickets were added to your cart:</p>
            <asp:BulletedList ID="AddedSeatsList" CssClass="CartSummary" runat="server" />
        </asp:Panel>
        <asp:Panel ID="FailedSeatsAddPanel" Visible="false" runat="server">
            <p class="Warning">Unfortunately, the following tickets could not be added:</p>
            <asp:BulletedList ID="FailedSeatsList" CssClass="CartSummary" runat="server" />
        </asp:Panel>
        <aside class="Review clearfix">
            <p class="SelfEvident">You have chosen:</p>
            <dl>
                <dt>Date:</dt>
                <dd><asp:Literal ID="DateBlurb" runat="server" /></dd>
                <dt>Start time:</dt>
                <dd><asp:Literal ID="StartTimeBlurb" runat="server" /></dd>
                <dt>Venue:</dt>
                <dd><asp:Literal ID="VenueBlurb" runat="server" /></dd>
            </dl>
        </aside>
        <asp:Panel ID="PromoCodeEntryPanel" runat="server">
            <h4 class="SelfEvident">Promotion Code Entry</h4>
            <dl>
                <dt>
                    <asp:Label ID="Label1" AssociatedControlID="PromoCodeField" runat="server">Promotion
                        code:</asp:Label>
                </dt>
                <dd>
                    <asp:TextBox ID="PromoCodeField" MaxLength="80" runat="server" /><br />
                    <asp:Button ID="Button1" Text="Update prices" OnClick="UpdatePrices" runat="server" />
                </dd>
            </dl>
        </asp:Panel>
        <asp:MultiView ID="SelectionModeViews" runat="server">
            <asp:View ID="BestSeatingModeView" runat="server">
                <fieldset>
                    <legend class="SelfEvident">Seating Selection</legend>
                    <p>How many seats would you like?</p>
                    <table>
                        <tr>
                            <th>Section</th>
                            <th>Price Per Seat</th>
                            <th>Number of Seats</th>
                        </tr>
                        <asp:Literal ID="SeatingRowsOutput" runat="server" />
                    </table>
                </fieldset>
                <div class="InputRow">
                    <asp:Button ID="AddToCartButton" Text="Add to cart" runat="server"
                        OnClick="AddSeatsToCart" />
                    <asp:Button ID="SeatChoiceBackButton" Text="Back" CausesValidation="false"
                        runat="server" OnClick="BackToPerfSelection" />
                    <asp:Button ID="SeatChoiceCancelButton" Text="Cancel" CausesValidation="false"
                        runat="server" OnClick="ReturnToReferer" Visible="false" />
                </div>
            </asp:View>
            <asp:View ID="SyosModeView" runat="server">
                <fieldset id="SyosSeatOptions" runat="server">
                    <legend class="SelfEvident">Seat Options</legend>
                    <aside class="SyosLegend" runat="server">
                        <h4>Color Key</h4>
                        <ul id="ColorLegend" runat="server">
                            <li class="SyosSeatInCart">in cart</li>
                            <li class="SyosSeatTaken">sold</li>
                        </ul>
                    </aside>
                </fieldset>
                <div class="InputRow">
                    <asp:Button Text="Add to cart" runat="server" OnClick="AddSeatsToCart" />
                    <%-- TODO: implement "back to perf selection" button --%>
                    <asp:Button Text="View cart" runat="server" OnClick="GoToCart"
                        CausesValidation="false" />
                </div>
            </asp:View>
        </asp:MultiView>
    </asp:View>
    <asp:View ID="ProdNotFoundView" runat="server">
        <p class="Warning">
            The production could not be found.
        </p>
    </asp:View>
    <asp:View ID="PerfsNotAvailableView" runat="server">
        <p class="Warning">
            Sorry. This production has no tickets for sale online.
        </p>
    </asp:View>
    <asp:View ID="ConfirmationView" runat="server">
        <aside class="Review clearfix">
            <h4 class="SelfEvident">Performance information</h4>
            <dl>
                <dt>Date:</dt>
                <dd><asp:Literal ID="ConfirmDateBlurb" runat="server" /></dd>
                <dt>Start time:</dt>
                <dd><asp:Literal ID="ConfirmStartTimeBlurb" runat="server" /></dd>
                <dt>Venue:</dt>
                <dd><asp:Literal ID="ConfirmVenueBlurb" runat="server" /></dd>
            </dl>
        </aside>
        <div>
            <p class="SuccessMessage">The following tickets were added to your cart:</p>
            <asp:BulletedList ID="AddedTicketsList" CssClass="CartSummary" runat="server" />
        </div>
        <asp:Panel ID="FailedResPanel" runat="server">
            <p class="Warning">Unfortunately, the following tickets could not be added:</p>
            <asp:BulletedList ID="FailedTicketsList" CssClass="CartSummary" runat="server" />
        </asp:Panel>
        <ul class="LinkMenu">
            <li>
                <asp:HyperLink Text="Keep shopping" ToolTip="Keep shopping" runat="server"
                    CssClass="CommandLink" ID="KeepShoppingLink" />
            </li>
            <li>
                <asp:HyperLink Text="View cart" ToolTip="View cart" runat="server"
                    CssClass="CommandLink" ID="ViewCartLink" />
            </li>
            <li>
                <asp:HyperLink Text="Check out" ToolTip="Check out" runat="server"
                    CssClass="CommandLink" ID="CheckOutLink" />
            </li>
        </ul>
    </asp:View>
</asp:MultiView>
<asp:HiddenField ID="ReservationErrorFlag" Value="0" runat="server" />
<asp:HiddenField ID="PromoCodeErrorFlag" Value="0" runat="server" />
<asp:HiddenField ID="PromoCodeSuccessFlag" Value="0" runat="server" />
<asp:HiddenField ID="SeatAddSuccessFlag" Value="0" runat="server" />
<asp:HiddenField ID="FailedSeatAddFlag" Value="0" runat="server" />
<asp:HiddenField ID="SeatsAddSuccessFlag" Value="0" runat="server" />
<asp:HiddenField ID="FailedSeatsAddFlag" Value="0" runat="server" />

<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VLineForm.ascx.cs"
    Inherits="Nysf.Apps.VLineEntryForm.VLineForm" %>
<div style="margin-left: 50px;">
    <asp:MultiView ID="VLineViews" runat="server">

        <%-- View 0: Entry Form --%>

        <asp:View runat="server">
            <asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />

            <p style="font-size: 19px; font-weight: bold;">
                Request Tickets for tonight&#39;s <asp:Literal ID="PerfTimeBlurb" runat="server"></asp:Literal>
            </p>

            <section>
                <h4>Step 1</h4>
                <p>
                    Enter participant name and address. NOTE, if selected to receive tickets, the name/address entered here MUST match a valid photo ID and 
                address of the person picking up tickets. <strong>There are no exceptions.</strong>
                </p>

            </section>
            <fieldset class="clearfix">
                <legend class="SelfEvident">Entry Details</legend>
                <dl>
                    <dt>
                        <asp:Label AssociatedControlID="NameField" runat="server">Full name:</asp:Label>
                        <%--<div class="FieldSubtext">
                        (in first-middle-last name order)
                    </div>--%>
                    </dt>
                    <dd>
                        <asp:TextBox ID="NameField" MaxLength="96" CssClass="FieldScaleX20"
                            runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="NameField" CssClass="Warning"
                            ErrorMessage="Please enter your full name." Text="*" runat="server" />
                        <%--<div class="FieldSubtext">
                        Examples: Thomas Alva Edison, Rosa Louise McCauley Parks
                    </div>--%>
                    </dd>
                    <dt>
                        <asp:Label AssociatedControlID="AddressField" runat="server">Street
                        address:</asp:Label>
                        <%-- <div class="FieldSubtext">
                        (with apartment / unit number)
                    </div>--%>
                    </dt>
                    <dd>
                        <asp:TextBox ID="AddressField" MaxLength="55" CssClass="FieldScaleX20"
                            runat="server" />
                        <asp:RequiredFieldValidator ControlToValidate="AddressField" CssClass="Warning"
                            ErrorMessage="Please enter your street address." Text="*" runat="server" />
                        <%-- <div class="FieldSubtext">
                        Example: 425 Lafayette St Apt 23
                    </div>--%>
                    </dd>

                </dl>
            </fieldset>
            <h4>Step 2</h4>
            <p>
                Indicate if your request is for General, Senior (65+), or Accessible (ADA) seating.
            </p>
            <fieldset class="clearfix">
                <legend class="SelfEvident">Entry Details</legend>
                <dl>

                    <dt>
                        <asp:Label ID="Label1" AssociatedControlID="LineTypeField" runat="server">
                        Seating preference:
                        </asp:Label>
                    </dt>
                    <dd>
                        <asp:DropDownList ID="LineTypeField" AppendDataBoundItems="true" runat="server">
                            <asp:ListItem Value="" Text="Select one" Selected="True" />
                        </asp:DropDownList>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="LineTypeField" CssClass="Warning"
                            ErrorMessage="Please select your seating preference." Text="*"
                            runat="server" />
                    </dd>
                </dl>
            </fieldset>
            <h4>Step 3</h4>
            <p>
                Click here to verify your request.
            </p>
            <div style="width:20%; text-align:right;">
              <asp:Button ID="Button1" Text="CONTINUE"  OnClick="ProcessInitialEntry" runat="server" />
            </div>
        </asp:View>

        <%-- View 1: Details Verification --%>

        <asp:View runat="server">
            <asp:ValidationSummary CssClass="Warning" DisplayMode="BulletList" runat="server" />
            <p style="font-size: 20px; font-weight: bold">VERIFY & SUBMIT YOUR REQUEST</p>
            <p>
                Please read the following terms and verify your ticket request at the bottom, then click SUBMIT.
            </p>
            <br />

            <p style="font-size: 20px; font-weight: bold">VIRTUAL TICKETING TERMS AND CONDITIONS</p>
            <p style="font-weight: bold">Today’s Virtual Ticketing drawing will take place at 12pm (noon). Winning participants are chosen randomly by the computer system.</p>
            <p style="font-weight: bold">An email will be sent by 2pm to let you know if you have been selected to receive tickets. Winners will receive two (2) tickets to tonight’s performance. </p>
            <p style="font-weight: bold">Those selected to receive tickets may pick them up between 5:00pm and 7:00pm at the Delacorte Theater Box Office. Tickets not picked up by 7:00pm will be released to the stand-by line. <span style="font-style: italic; font-weight: bold;">There are no exceptions.</span></p>
            <p style="font-weight: bold" runat="server" id="nonSenior">We require photo ID and address verification for everyone picking up tickets. The name and address provided during the ticket request process MUST match a valid photo ID and address in order to pick up tickets. <span style="font-style: italic; font-weight: bold;">There are no exceptions.</span></p>
            <p style="font-weight: bold" runat="server" id="senior" visible="false">For Senior (65+) Seating, we require photo ID with proof of age and address verification for everyone picking up tickets. The name and address provided during the ticket request process MUST match a valid photo ID and address in order to pick up tickets. <span style="font-style: italic; font-weight: bold;">There are no exceptions.</span></p>
            <p>The Public Theater reserves the right to deny tickets to anyone participating in the selling of free tickets and to deny admission to anyone found in possession of sold tickets. The Public Theater does not condone ticket scalping, which goes against the spirit of Free Shakespeare in the Park.</p>
            <p>Virtual Ticketing entries are limited to one per person. Tickets are limited to two (2) per person regardless of number of email confirmations received.</p>
            <p>Anyone with children under 5 may obtain a lap-seating voucher for their child when entering the theater through their assigned gate. Strollers of any kind are not allowed in the theater.</p>
            <p>Only tickets obtained from the Delacorte Theater Box Office or The Public Theater will be accepted as forms of entry into the theater.</p>
            <p>By accepting a ticket from the Delacorte Theater or The Public Theater or using any such ticket, you voluntarily agree to comply with the Free Ticket Distribution policy and, if applicable, any additional terms and conditions set forth on the back of the ticket.</p>
            <p>Any disagreement about ticket distribution will be settled by The Public Theater in its sole discretion. The Public Theater reserves the right to create additional rules as needed to address unanticipated situations.</p>
            <p>The area surrounding the Delacorte Theater is monitored by video surveillance.</p>
            <p>The address information you enter below is for ticket pick-up purposes only and will not be saved after tonight's performance. We value your privacy, and will never send you anything without your permission.</p>

            <%--<section>
            
            <ul>
                <li>
                    <strong>Winners are required to present government-issued ID that exactly
                    matches the name and address used to enter the drawing.</strong>
                </li>
                <li>
                    Virtual Ticket Drawing entries are limited to one per person.
                </li>
                <li>
                    Tickets are limited to one pair per person regardless of the number of email
                    confirmations received.
                </li>
                <li id="SeniorsCondition" runat="server" visible="false">
                    <strong>For Senior Virtual Ticketing:</strong> The person who registered/signed
                    in must present government issued photo ID that proves age (65 or over) when
                    picking up tickets.
                </li>
                <li id="DisabledCondition" runat="server" visible="false">
                    <strong>For ADA-Accessible Virtual Ticketing:</strong> The person who
                    registered/signed in must present government issued photo ID and proof of
                    disability when picking up tickets.
                </li>
                <li>
                    Winners may pick up their tickets between 6pm and 7pm at the Delacorte Theater
                    Box Office. Tickets not picked up by 7pm will be released to the stand-by line.
                </li>
                <li>
                    Violation of an above condition may result in ineligibility to receive tickets,
                    at the discretion of The Public Theater staff.
                </li>
            </ul>
        </section>
                --%>
        <br />
        <fieldset>
            <legend class="SelfEvident">Agreement</legend>
            <div>
                <asp:CheckBox ID="AgreeField" runat="server"
                    Text="" />&nbsp;<span style="font-style:italic;">I agree to the above conditions.</span>
                <asp:CustomValidator runat="server"
                    OnServerValidate="ValidateAgreement" Text="*" CssClass="Warning"
                    ErrorMessage="You must agree to the Terms and Conditions." />
                <br />
               </div>
        </fieldset>

            <hr />
        <p style="font-size: 20px"><strong>SUBMIT YOUR TICKET REQUEST</strong></p>
            <p>
            Please double-check that the information below is exactly as it appears on your ID.
        </p>
            <p style="font-size: 13px; font-weight: bold;">YOUR INFORMATION</p>
            <div style="margin: 16px;">
                <ul class="InfoForReview">
                    <li>
                        <asp:Literal ID="NameConfirmBlurb" runat="server" /></li>
                    <li>
                        <asp:Literal ID="AddressConfirmBlurb" runat="server" /></li>
                    <li>
                        <asp:Literal ID="LineNameBlurb" runat="server" /></li>
                </ul>
            </div>
            <br />
            <p><strong>Please verify that the information above matches a valid photo ID. To fix your information,
                <asp:LinkButton ID="LinkButton1" runat="server" OnClick="ReturnToForm">click here</asp:LinkButton></strong></p>
            <p><strong>You will be notified after <asp:Literal ID="closingTimeBlurb" runat="server"></asp:Literal> via email if you did or did not receive tickets to tonight’s performance.</strong></p>
            <p style="font-style:italic;">Join Our Email Lists for Breaking News from The Public</p>
            <div style="margin-left:14px;">
             <asp:CheckBox ID="cbSitPEmail" runat="server" Checked="true" />&nbsp;Please add me to the email list for news and updates about The Public’s Free Shakespeare in the Park.
                 <br />
                <asp:CheckBox ID="cbPTEmail" runat="server" />&nbsp;Please also add me to the email list for news and tickets for events at The Public Theater’s downtown home at Astor Place.
                
                <br />
                <asp:CheckBox ID="cbJP" runat="server" />&nbsp;Please also add me to the email list for music news and tickets at Joe’s Pub at The Public Theater.
            </div>
            <br />
            <p>Thank you for entering and good luck!</p>
            <div class="InputRow">
                <asp:Button Text="SUBMIT" OnClick="ProcessSubmission" runat="server" />

            </div>
        </asp:View>

        <%-- View 2: Entry Confirmation --%>

        <asp:View runat="server">
            <p>You have successfully registered for today’s Virtual Ticketing drawing! </p>
            <p>
                You will receive an email after <asp:Literal ID="drawingTimeBlurb2" runat="server"></asp:Literal> to let you know if you have or have not been selected to receive tickets to tonight's
                 <asp:Literal ID="PerfTimeBlurb2" runat="server"></asp:Literal>
            </p>

            <br />
            <br />
            <br />
            <asp:HyperLink ID="HyperLink1" NavigateUrl="http://www.shakespeareinthepark.org" runat="server">Home</asp:HyperLink>
        </asp:View>

        <%-- View 3: Win Notice --%>

        <asp:View runat="server">
            <p>
                You have been selected to receive two tickets to tonight's performance of <i>
                    <asp:Literal
                        ID="WinPerfNameBlurb" runat="server" /></i>!
            </p>
            <p>
                Your tickets will be available for pick-up at The Delacorte Theater Box Office between <asp:Literal ID="pickupStartTimeBlurb" runat="server"></asp:Literal> 
                and <asp:Literal ID="pickupStartEndBlurb" runat="server"></asp:Literal> this evening. 
                 Tickets will be held under the name and address you used for registration – please present photo ID <asp:Literal
                    ID="SeniorsExtraReqBlurb" Visible="false" runat="server"> with proof of age (65 or
            over)</asp:Literal> when picking up your tickets.  
                Tickets not picked up by <asp:Literal ID="pickupStartEndBlurb1" runat="server"></asp:Literal>  will be released to the stand-by line.</p>
                
            <p>Enjoy the show!</p>
            <br />
            <br />
            <br />
            <asp:HyperLink ID="HyperLink2" NavigateUrl="http://www.shakespeareinthepark.org" runat="server">Home</asp:HyperLink>
        </asp:View>

        <%-- View 4: Drawing Over Message --%>

        <asp:View runat="server">
            <p>Virtual Ticketing for today’s performance has closed. </p>
<p>
Please try again another day for tickets to Free Shakespeare in the Park. </p>

            <br />
            </asp:View>

        <%-- View 5: Loss Notice --%>

        <asp:View runat="server">
            <p>Unfortunately, you have not been selected to receive tickets to tonight’s performance. </p>
            <p>
            Please try again another day for tickets to Free Shakespeare in the Park. </p>

            <br />
            
        </asp:View>

        <%-- View 6: Wait Notice --%>

        <asp:View runat="server">
             <p>You have successfully registered for today’s Virtual Ticketing drawing! </p>
            <p>
                You will receive an email after <asp:Literal ID="DrawingTimeBlurb" runat="server"></asp:Literal> to let you know if you have or have not been selected to receive tickets to tonight's
                 <asp:Literal ID="PerfTimeBlurb3" runat="server"></asp:Literal>
            </p>

            <br />
            <br />
            <br />
            <asp:HyperLink ID="HyperLink3" NavigateUrl="http://www.shakespeareinthepark.org" runat="server">Home</asp:HyperLink>
        </asp:View>

        <%-- View 7: No Shows Today --%>

        <asp:View runat="server">
            <p>
                There is no performance today.
            </p>
            <br />
           
        </asp:View>

        <%-- View 8: Year End --%>

        <asp:View runat="server">
            <p>
                This year's Shakespeare in the Park has concluded. Thank you for your patronage. See you
            next year!
            </p>
            <br />
           
        </asp:View>
        <%-- View 9: After drawing, before resolution--%>

        <asp:View runat="server">
            <p>Tickets to tonight’s performance of Free Shakespeare in the Park are currently being distributed.
        </p><p>
        Please check back shortly to see the results of today’s Virtual Ticketing Drawing.
        </p>
        </asp:View>
            <%-- View 10: Before drawing of todays show--%>

        <asp:View runat="server">
            <p>The drawing does not open until <asp:Literal ID="openTimeBlurb" runat="server"></asp:Literal>
        </p><p>
        Please check back after then to enter today's Virtual Ticket Drawing.
        </p>
        </asp:View>

    </asp:MultiView>
    <asp:Panel ID="NextLotteryOpeningBlurb" runat="server">
        <p>
            The next Virtual Ticketing Drawing will take place on
            <asp:Literal ID="NextDateBlurb"
                runat="server" />.
        </p>
         <br />
            <br />
            <asp:HyperLink ID="HyperLink6" NavigateUrl="http://www.shakespeareinthepark.org" runat="server">Home</asp:HyperLink>
    </asp:Panel>
</div>

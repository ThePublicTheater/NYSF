<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="CheckoutFormNew.ascx.cs" Inherits="tickets.usercontrols.CheckoutFormNew" %>
<%@ Register Src="~/usercontrols/cartWidget.ascx" TagPrefix="uc1" TagName="cartWidget" %>

<script src="/js/libs/jQuery/jquery-1.9.1.js"></script>
<script src="/js/libs/jQuery/jquery-ui-1.10.3.custom.js"></script>
<script type="text/javascript">
    $(function () {
        $(".accordion").accordion({ heightStyle: "content", disabled: true });
        $(".accordion").show();
    });



</script>
<style type="text/css">
    body {
    }

    #left {
        float: left;
    }

    #right {
        width: 300px;
        float: left;
        margin-left: 15px;
    }

    #outside {
        overflow: hidden;
        width: 950px;
        margin: 0 auto;
    }

    .label {
        font-family: Arial, Sans-Serif;
        font-size: 16px;
        width: 150px;
        text-align: right;
        display: inline-block;
    }

    .longLabel {
        font-family: Arial, Sans-Serif;
        font-size: 16px;
        width: 200px;
        text-align: right;
        display: inline-block;
    }

    .accordion {
        width: 600px;
        margin-top: 15px;
        display: none;
    }

    .accordionHeader {
        border: 1px solid #FFFFFF;
        color: white;
        background-color: #E02400;
        font-family: Arial, Sans-Serif;
        font-size: 20px;
        font-weight: bold;
        padding: 5px;
        text-align: center;
    }

    .accordionHeaderSelected {
        border: 1px solid #FFFFFF;
        color: white;
        background-color: #A62006;
        font-family: Arial, Sans-Serif;
        font-size: 20px;
        font-weight: bold;
        padding: 5px;
        margin-top: 5px;
        cursor: pointer;
        text-align: center;
    }

    .accordionContent {
        padding-top: 10px;
        border-top-style: none;
        border-top-color: inherit;
        border-top-width: medium;
        margin-bottom: 0px;
        padding-left: 5px;
        padding-right: 5px;
        padding-bottom: 5px;
        overflow: hidden;
    }


    .flatTextBox {
        width: 220px;
        padding: 7px 25px;
        font-family: "HelveticaNeue-Light", "Helvetica Neue Light", "Helvetica Neue", Helvetica, Arial, "Lucida Grande", sans-serif;
        font-weight: 400;
        font-size: 14px;
        color: #9d9e9e;
        text-shadow: 1px 1px 0 rgba(256,256,256,1.0);
        background: #fff;
        border: 1px solid #fff;
        border-radius: 5px;
        box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -moz-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -webkit-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
    }


        .flatTextBox:hover {
            background: #dfe9ec;
            color: #414848;
        }

        .flatTextBox:focus {
            background: #dfe9ec;
            color: #414848;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
            -moz-box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
            -webkit-box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
        }

            .flatTextBox:focus + div {
                left: -46px;
            }

    .flatTextBoxShort {
        width: 175px;
        padding: 7px 25px;
        font-family: "HelveticaNeue-Light", "Helvetica Neue Light", "Helvetica Neue", Helvetica, Arial, "Lucida Grande", sans-serif;
        font-weight: 400;
        font-size: 14px;
        color: #9d9e9e;
        text-shadow: 1px 1px 0 rgba(256,256,256,1.0);
        background: #fff;
        border: 1px solid #fff;
        border-radius: 5px;
        box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -moz-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -webkit-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
    }


        .flatTextBoxShort:hover {
            background: #dfe9ec;
            color: #414848;
        }

        .flatTextBoxShort:focus {
            background: #dfe9ec;
            color: #414848;
            box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
            -moz-box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
            -webkit-box-shadow: inset 0 1px 2px rgba(0,0,0,0.25);
        }

            .flatTextBoxShort:focus + div {
                left: -46px;
            }

    /* Animation */
    .flatTextBox {
        transition: all 0.5s ease;
        -moz-transition: all 0.5s ease;
        -webkit-transition: all 0.5s ease;
        -o-transition: all 0.5s ease;
        -ms-transition: all 0.5s ease;
    }

    .fieldDiv {
        margin-top: 10px;
    }


    select {
        padding: 7px 4px 7px 25px;
        width: 270px;
        outline: none;
        color: #74646e;
        border: 1px solid #C8BFC4;
        border-radius: 4px;
        text-shadow: 1px 1px 0 rgba(256,256,256,1.0);
        background-color: #fff;
        font-size: 14px;
        box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -moz-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -webkit-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
    }

    .selectHalfSize {
        padding: 7px 4px 7px 25px;
        width: 132px !important;
        outline: none;
        color: #74646e;
        border: 1px solid #C8BFC4;
        border-radius: 4px;
        text-shadow: 1px 1px 0 rgba(256,256,256,1.0);
        background-color: #fff;
        font-size: 14px;
        margin-right: 3px;
        box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -moz-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
        -webkit-box-shadow: inset 0 1px 3px rgba(0,0,0,0.50);
    }

    select:hover {
        background: #dfe9ec;
        color: #414848;
    }

    .italic {
        font-style: italic;
    }

    .myButton {
        background-color: #0f0e0e;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
        display: inline-block;
        color: #ffffff !important;
        font-family: Trebuchet MS;
        font-size: 17px;
        font-weight: bold;
        padding: 5px 11px;
        text-decoration: none;
    }

        .myButton:hover {
            background-color: #7d857d;
        }

        .myButton:active {
            position: relative;
            top: 1px;
        }

    .floatRight {
        float: right;
    }


    .alignRight {
        text-align: right;
    }



    input[type=checkbox].css-checkbox {
        display: none;
    }

        input[type=checkbox].css-checkbox + label.css-label {
            padding-left: 20px;
            height: 15px;
            display: inline-block;
            line-height: 15px;
            background-repeat: no-repeat;
            background-position: 0 0;
            font-size: 14px;
            font-family: Arial, Sans-Serif;
            vertical-align: middle;
            cursor: pointer;
            margin-top: 10px;
        }

        input[type=checkbox].css-checkbox:checked + label.css-label {
            background-position: 0 -15px;
        }

    .css-label {
        background-image: url(http://csscheckbox.com/checkboxes/lite-red-check.png);
    }

    .shippingHeader {
        font-weight: bold;
        font-size: 115%;
    }

    .topBottomMargin {
        margin-bottom: 10px;
        margin-top: 10px;
    }

    .topBottomMargin {
        margin-bottom: 10px;
        margin-top: 5px;
    }

    input[type="submit"] {
    }

    .submitLink {
        background-color: transparent !important;
        text-decoration: underline;
        border: none;
        font-variant: initial !important;
        text-transform: initial !important;
        color: blue !important;
        cursor: pointer;
        font-size: 12px !important;
        margin: 0px;
        padding: 0;
        vertical-align: inherit !important;
    }


    #confirmPanel {
        position: relative;
        height: 350px;
    }

        #confirmPanel .leftcontent {
            position: absolute;
            left: 0px;
            width: 200px;
        }

        #confirmPanel .centercontent {
            margin-left: 201px;
            margin-right: 212px;
            text-align: center;
        }

        #confirmPanel .rightcontent {
            position: absolute;
            right: 0px;
            width: 200px;
            top: 0px;
        }

    h4 {
        font-family: Arial, Sans-Serif;
        font-size: 18px;
        font-weight: bold;
        color: black;
    }

    h5 {
        font-family: Arial, Sans-Serif;
        font-size: 16px;
        font-weight: bold;
        color: black;
    }

    .indent {
        margin-left: 17px;
    }

    .panelWarning {
        text-align: center;
        color: red;
    }

    .alignLeft {
        text-align: left;
    }

    .myButtonGreen {
        background-color: #01BB29 !important;
        -moz-border-radius: 5px;
        -webkit-border-radius: 5px;
        border-radius: 5px;
        display: inline-block;
        color: #ffffff !important;
        font-family: Trebuchet MS;
        font-size: 17px;
        font-weight: bold;
        padding: 4px 7px;
        text-decoration: none;
    }

        .myButtonGreen:hover {
            background-color: #9CFF8A !important;
        }

        .myButtonGreen:active {
            position: relative;
            top: 1px;
        }

    .nameLabel {
        font-family: Arial, Sans-Serif;
        font-size: 15px;
        font-weight: bold;
    }

    .loggedInNotice {
        font-weight: bold;
        font-size: 14px;
        height: 17px;
        width: 600px;
    }

    .orderSumLeftCol {
        width: 110px;
        text-align: left;
        margin-right: 8px;
        margin-left: 17px;
        display: inline-block;
        font-weight: bold;
    }

    .orderSumRightCol {
        text-align: left;
    }

    .checkboxes {
        margin-left: 30px;
    }
</style>
<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
<asp:MultiView ID="CheckoutViews" runat="server">
    <asp:View ID="BillingEntryView" runat="server">

        <div id="outside">
            <div id="left">
                <br />
                <asp:UpdatePanel ID="UpdatePanel5" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div class="loggedInNotice">
                            <asp:Label ID="lb_loggedIn" runat="server" Text="" Visible="false" ClientIDMode="Static"></asp:Label>
                            <asp:Label ID="lb_newAccountCreated" runat="server" Text="" Visible="false" ClientIDMode="Static"></asp:Label>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="accordion" class="accordion">
                    <h3 class="accordionHeader">Account Information</h3>
                    <div class="accordionContent">
                        <asp:UpdatePanel ID="UpdatePanel3" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                                <h4>Sign In</h4>
                                <br />
                                <h5>
                                    <asp:RadioButton ID="rb_havePass" runat="server" ClientIDMode="Static" Text="I am a returning customer " GroupName="account" Checked="true" /></h5>
                                <br />
                                <div id="havePassDiv">
                                    <div class="fieldDiv">
                                        <span class="indent">
                                            <label class="longLabel">Username:</label>
                                            <asp:TextBox ID="tb_email" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            <div class="FormFieldExample"><span style="margin-left: 210px">Your username may be your email address.</span> </div>
                                        </span>
                                    </div>
                                    <div class="fieldDiv">
                                        <span class="indent">
                                            <label class="longLabel">Password:</label>
                                            <asp:TextBox ID="tb_password" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static" TextMode="Password"></asp:TextBox>
                                        </span>
                                    </div>
                                    <br />
                                    <a href="/recover" style="margin-left: 50px;">Forgot Password</a>
                                </div>
                                <br />
                                <h5>
                                    <asp:RadioButton ID="rb_noPass" runat="server" ClientIDMode="Static" Text="I am a new customer" GroupName="account" /></h5>
                                <br />
                                <div class="indent">
                                    <div class="fieldDiv">
                                        <div id="newPassDiv" style="display: none">
                                            <div class="fieldDiv">
                                                <label class="label">First Name: </label>
                                                <asp:TextBox ID="tb_newFirstName" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                            <div class="fieldDiv">
                                                <label class="label">Last Name: </label>
                                                <asp:TextBox ID="tb_newLastName" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                            <div class="fieldDiv">
                                                <label class="label">Email Address: </label>
                                                <asp:TextBox ID="tb_newEmail" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static"></asp:TextBox>
                                            </div>
                                            <div class="fieldDiv">
                                                <label class="label">Password: </label>
                                                <asp:TextBox ID="tb_newPassword1" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static" TextMode="Password"></asp:TextBox>
                                            </div>
                                            <div class="fieldDiv">
                                                <label class="label">Re-enter Password: </label>
                                                <asp:TextBox ID="tb_newPassword2" CssClass="flatTextBoxShort" runat="server" ClientIDMode="Static" TextMode="Password"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <asp:Panel ID="panelWarning0" runat="server" ClientIDMode="Static" CssClass="panelWarning"></asp:Panel>
                                <div style="text-align: right;">
                                    <asp:Button ID="next0" runat="server" Text="Next" ClientIDMode="Static" CssClass="myButton" OnClick="loginCreate" />
                                    <%--<a id="next0" href="#" class="myButton">Next</a>--%>
                                </div>
                                <asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" Value="false" />
                                <asp:HiddenField ID="hf_sliderState" runat="server" ClientIDMode="Static" Value="closed" />
                                <asp:HiddenField ID="hf_state" runat="server" Value="0" ClientIDMode="Static" />
                                <script>
                                    function pageLoad() {
                                        $("#rb_havePass").on("change", function (event) {
                                            $("#havePassDiv").slideDown();
                                            $("#newPassDiv").slideUp();
                                        });
                                        $("#rb_noPass").on("change", function (event) {

                                            $("#newPassDiv").slideDown();
                                            $("#havePassDiv").slideUp();
                                        });
                                    }

                                </script>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <h3 class="accordionHeader">Contact and Shipping Information</h3>
                    <div class="accordionContent">
                        <asp:UpdatePanel ID="UpdatePanel4" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <div class="fieldDiv">
                                    <label class="label">Full Name:</label>
                                    <asp:Label ID="lb_name" CssClass="nameLabel" runat="server" Text=""></asp:Label>

                                </div>
                                <div class="fieldDiv">
                                    <label class="label">Country:</label>

                                    <asp:DropDownList ID="dd_country" runat="server" ClientIDMode="Static" AutoPostBack="true" OnSelectedIndexChanged="dd_country_SelectedIndexChanged">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" ValidationGroup="addressInfo"
                                        ControlToValidate="dd_country" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please select a country." />
                                </div>

                                <div class="fieldDiv">
                                    <label class="label">Street Address:</label>
                                    <asp:TextBox ID="tb_streetAddress" CssClass="flatTextBox" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" ValidationGroup="addressInfo"
                                        ControlToValidate="tb_streetAddress" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter an address." />
                                </div>
                                <div class="fieldDiv">
                                    <label class="label">Sub Address:</label>
                                    <asp:TextBox ID="tb_subAddress" CssClass="flatTextBox" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </div>
                                <div class="fieldDiv">
                                    <label class="label">City:</label>
                                    <asp:TextBox ID="tb_City" CssClass="flatTextBox" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" ValidationGroup="addressInfo"
                                        ControlToValidate="tb_City" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter a city." />
                                </div>
                                <asp:Panel ID="stateGroup" runat="server" CssClass="fieldDiv" Visible="false">
                                    <label class="label">State/Province:</label>

                                    <asp:DropDownList ID="dd_states" runat="server" ClientIDMode="Static">
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" ValidationGroup="addressInfo"
                                        ControlToValidate="dd_states" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please select a state/province." />
                                </asp:Panel>
                                <div class="fieldDiv">
                                    <label class="label">Postal Code:</label>
                                    <asp:TextBox ID="tb_zip" CssClass="flatTextBox" runat="server" ClientIDMode="Static"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator11" ValidationGroup="addressInfo"
                                        ControlToValidate="tb_zip" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter a zip code." />
                                </div>
                                <div class="fieldDiv">
                                    <label class="label">Phone Number:</label>
                                    <asp:TextBox ID="tb_phoneNumber" CssClass="flatTextBox" runat="server" ClientIDMode="Static"></asp:TextBox>
                                </div>

                                <div style="text-align: right;">
                                    <asp:Button ID="Button2" runat="server" Text="Next" ClientIDMode="Static" CssClass="myButton" OnClick="nextPanel" />
                                </div>
                            </ContentTemplate>
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="dd_country" EventName="SelectedIndexChanged" />
                            </Triggers>
                        </asp:UpdatePanel>

                    </div>
                    <h3 class="accordionHeader">Preferences</h3>
                    <div class="accordionContent">
                        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
                            <ContentTemplate>



                                <div class="fieldDiv">
                                    <label class="label">Delivery Method:</label>

                                    <asp:DropDownList ID="dd_delivery" runat="server" ClientIDMode="Static">
                                        <asp:ListItem></asp:ListItem>
                                        <asp:ListItem Value="-1">Hold at Box Office</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="deliveryMethodValidator" ValidationGroup="deliveryMethodValidator"
                                        ControlToValidate="dd_delivery" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please select a delivery method." />
                                </div>
                                <br />
                                <div style="text-align: center; font-style: italic; font-size: 15px;">Join our email lists for breaking news and updates!</div>
                                <br />
                                <div class="checkboxes">
                                    <div id="firstJpCheckBox" runat="server">
                                        <input id="cbJP0" class="css-checkbox" type="checkbox" runat="server" />
                                        <label for="MainContentPlaceHolder_CheckoutFormNew_cbJP0" name="demo_lbl_4" class="css-label">
                                            Joe’s Pub</label>
                                        <br />
                                        <br />
                                    </div>
                                    <input id="cbPT" class="css-checkbox" type="checkbox" checked="checked" runat="server" />
                                    <label for="MainContentPlaceHolder_CheckoutFormNew_cbPT" name="demo_lbl_1" class="css-label">
                                        The Public Theater</label>

                                    <!-- Checkbox powered by CssCheckbox.com -->
                                    <br />
                                    <br />
                                    <div id="joesPubCheckBox" runat="server">
                                        <input id="cbJP" class="css-checkbox" type="checkbox" runat="server" />
                                        <label for="MainContentPlaceHolder_CheckoutFormNew_cbJP" name="demo_lbl_2" class="css-label">
                                            Joe’s Pub</label>
                                        <br />
                                        <br />
                                    </div>
                                    <input id="cbSitP" class="css-checkbox" type="checkbox" runat="server" />
                                    <label for="MainContentPlaceHolder_CheckoutFormNew_cbSitP" name="demo_lbl_3" class="css-label">Shakespeare in the Park</label>
                                    <br />
                                    <br />
                                    <input id="cbNyt" class="css-checkbox" type="checkbox" runat="server" />
                                    <label for="MainContentPlaceHolder_CheckoutFormNew_cbNyt" name="demo_lbl_5" class="css-label">Click here to receive a FREE subscription to New York Magazine with your order, courtesy of our season sponsor New York Media.</label>
                                </div>
                                <br />
                                <br />
                                <br />
                                <div style="clear: both;"></div>
                                <asp:Button ID="Button7" runat="server" Text="Prev" ClientIDMode="Static" CssClass="myButton" style="float:left" OnClick="previousPanel" />
                                <div style="float: right;">
                                    <asp:Button ID="Button3" runat="server" Text="Next" ClientIDMode="Static" CssClass="myButton" OnClick="nextPanel" />
                                </div>
                                <div style="clear: both;"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <h3 class="accordionHeader">Payment Information</h3>
                    <div class="accordionContent">
                        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
                            <ContentTemplate>
                                <div class="fieldDiv">
                                    <asp:Label ID="Label1" AssociatedControlID="CardTypeField" CssClass="label" runat="server">
                        Credit card type:
                                    </asp:Label>
                                    <asp:DropDownList ID="CardTypeField" runat="server" ClientIDMode="Static">
                                        <asp:ListItem Text="" Value="" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ValidationGroup="paymentInfo"
                                        ControlToValidate="CardTypeField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please select your card type." />
                                    <div class="FormFieldExample">
                                        <asp:Literal ID="amexOnlyBlurb" runat="server"></asp:Literal>
                                    </div>

                                    <%-- TODO: Make sure initial selection (value 0) triggers invalidation --%>
                                </div>
                                <div class="fieldDiv">
                                    <asp:Label ID="Label2" CssClass="label" AssociatedControlID="CardNumField" runat="server">
                        Credit card number:
                                    </asp:Label>
                                    <asp:TextBox ID="CardNumField" MaxLength="16" CssClass="flatTextBox" runat="server" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" ValidationGroup="paymentInfo"
                                        ControlToValidate="CardNumField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter your credit card number." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" ValidationGroup="paymentInfo"
                                        ControlToValidate="CardNumField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning"
                                        ErrorMessage="Please enter only digits for your credit card number."
                                        ValidationExpression="^[0-9]+$" />
                                    <div class="FormFieldExample">(only numbers, no spaces or dashes please)</div>
                                </div>
                                <div class="fieldDiv">
                                    <asp:Label ID="Label3" CssClass="label" AssociatedControlID="NameField" runat="server">
                        Name on credit card:
                                    </asp:Label>
                                    <asp:TextBox ID="NameField" CssClass="flatTextBox" MaxLength="97" runat="server" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" ValidationGroup="paymentInfo"
                                        ControlToValidate="NameField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter the cardholder name." />
                                </div>
                                <div class="fieldDiv">
                                    <asp:Label ID="Label4" CssClass="label" AssociatedControlID="ExpMonthField" runat="server">
                        Expiration:
                                    </asp:Label>
                                    <asp:DropDownList ID="ExpMonthField" runat="server" CssClass="selectHalfSize">
                                        <asp:ListItem Text="" Value="" Selected="True" />
                                        <asp:ListItem Text="Jan" Value="1" />
                                        <asp:ListItem Text="Feb" Value="2" />
                                        <asp:ListItem Text="Mar" Value="3" />
                                        <asp:ListItem Text="Apr" Value="4" />
                                        <asp:ListItem Text="May" Value="5" />
                                        <asp:ListItem Text="Jun" Value="6" />
                                        <asp:ListItem Text="Jul" Value="7" />
                                        <asp:ListItem Text="Aug" Value="8" />
                                        <asp:ListItem Text="Sep" Value="9" />
                                        <asp:ListItem Text="Oct" Value="10" />
                                        <asp:ListItem Text="Nov" Value="11" />
                                        <asp:ListItem Text="Dec" Value="12" />
                                    </asp:DropDownList>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" ValidationGroup="paymentInfo"
                                        ControlToValidate="ExpMonthField" Display="Dynamic" Text=" Required"
                                        CssClass="Warning" runat="server"
                                        ErrorMessage="Please select an expiration month." />
                                    <asp:DropDownList ID="ExpYearField" runat="server" CssClass="selectHalfSize">
                                        <asp:ListItem Text="" Value="" Selected="True" />
                                    </asp:DropDownList><%-- TODO: add years in pageload --%>

                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" ValidationGroup="paymentInfo"
                                        ControlToValidate="ExpYearField" Display="Dynamic" Text=" Required"
                                        CssClass="Warning" runat="server"
                                        ErrorMessage="Please select an expiration year." />
                                    <asp:CustomValidator ID="CustomValidator1" ControlToValidate="ExpYearField" ValidationGroup="paymentInfo"
                                        runat="server" Display="Dynamic" Text=" Required" CssClass="Warning"
                                        ErrorMessage="Please enter an expiration date in the future."
                                        OnServerValidate="VerifyExpDateInFuture" />
                                </div>
                                <div class="fieldDiv">
                                    <asp:Label ID="Label5" CssClass="label" AssociatedControlID="AuthCodeField" runat="server">
                        CVV / CID number:
                                    </asp:Label>
                                    <asp:TextBox ID="AuthCodeField" MaxLength="4" runat="server" CssClass="flatTextBox" ClientIDMode="Static" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" ValidationGroup="paymentInfo"
                                        ControlToValidate="AuthCodeField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter the CVV / CID number." />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ValidationGroup="paymentInfo"
                                        ControlToValidate="AuthCodeField" Display="Dynamic" Text=" Required" runat="server"
                                        CssClass="Warning" ErrorMessage="Please enter a 3-4 digit CVV / CID number."
                                        ValidationExpression="^[0-9]{3,4}$" />
                                </div>
                                <div class="SecuritySeal">
                                    <h4 class="visuallyhidden">Security Seal</h4>
                                    <table width="135" border="0" cellpadding="1" cellspacing="1"
                                        style="display: inline">
                                        <tr>
                                            <td width="135" align="center">
                                                <script src="https://sealserver.trustkeeper.net/compliance/seal_js.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en">
                                                </script>
                                                <noscript>
                                                    <a href="https://sealserver.trustkeeper.net/compliance/cert.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en" target="hATW">
                                                        <img src="https://sealserver.trustkeeper.net/compliance/seal.php?code=x4ij3BrNflKuByuB1cTROdOFOTLL5E&style=normal&size=105x54&language=en" border="0" alt="Trusted Commerce" />
                                                    </a>
                                                </noscript>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
                                <br />
                                <div style="clear: both;"></div>
                                <asp:Button ID="Button5" runat="server" Text="Prev" ClientIDMode="Static" CssClass="myButton" style="float:left" OnClick="previousPanel" />
                                <div style="float: right;">
                                    <asp:Button ID="Button4" runat="server" Text="Next" ClientIDMode="Static" CssClass="myButton" OnClick="nextPanel" />
                                </div>
                                 <div style="position: absolute; top: 275px; text-align: center">
                                        <p id="BillingErrorMessage" class="Warning" visible="false" runat="server">
                                            Sorry.  Your credit card could not be processed.  Please double-check your information
			                                below and try again.
                                        </p>
                                    </div>
                                <div style="clear: both;"></div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <h3 class="accordionHeader">Confirmation</h3>
                    <div class="accordionContent">
                        <asp:UpdatePanel ID="confirmationUpdatePanel" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>

                        <div id="confirmPanel">
                            <div class="leftcontent" id="leftcontent">
                                <span class="shippingHeader" id="shippingHeader" runat="server">Shipping address</span>
                                <p class="topBottomMargin">
                                    <asp:Label ID="name" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                    <br />
                                    <asp:Label ID="address1" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                    <br />
                                    <asp:Label ID="address2" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                    <br />
                                    <asp:Label ID="cityStateZip" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                    <br />
                                    <asp:Label ID="country" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                    <br />
                                    <asp:Label ID="phone" runat="server" Text="" ClientIDMode="Static"></asp:Label>
                                </p>
                                <a href="#" id="changeAddress" class="topBottomMargin">Change Information</a>
                            </div>
                            <div class="centercontent">
                                <span class="shippingHeader">Payment information</span>
                                <p class="topBottomMargin2">
                                    <span style="vertical-align: middle;"><span id="paymentSummary" runat="server"></span><span id="endingIn" style="margin-left: 3px;" runat="server"></span></span>
                                </p>
                                <div style="text-align: right;"><a href="#" id="changePayment">Change Card</a></div>

                            </div>
                            <div class="rightcontent">

                                <span class="shippingHeader">Delivery preference</span>
                                <p class="topBottomMargin">
                                    <span id="deliveryChoice" style="float: left;" runat="server"></span>
                                    <a href="#" id="changeDelivery" class="floatRight">Change</a>
                                </p>

                            </div>
                            <asp:UpdatePanel ID="UpdatePanel2" UpdateMode="Conditional" runat="server">
                                <ContentTemplate>
                                    <div style="position: absolute; top: 250px; right: 25px;">
                                        <span class="shippingHeader">Confirm payment of:</span>
                                        <asp:Literal ID="amountLiteral" runat="server"></asp:Literal>
                                        <asp:Button ID="Button1" CssClass="myButtonGreen" runat="server" Text="Submit" OnClick="Button1_Click" />
                                    </div>
                                   
                                </ContentTemplate>
                                <%--                                <Triggers>
                                    <asp:PostBackTrigger ControlID="Button1" />
                                </Triggers>--%>
                            </asp:UpdatePanel>
                            <div style="position: absolute; top: 250px; left: 0px;">
                                <asp:Button ID="Button6" runat="server" Text="Prev" ClientIDMode="Static" CssClass="myButton" OnClick="previousPanel" />
                            </div>
                        </div>
                    </div>
                </div>
</ContentTemplate>
                        </asp:UpdatePanel>
            </div>

            <div id="right">
                <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
                    <ContentTemplate>
                        <uc1:cartWidget runat="server" ID="cartWidget" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div style="margin-left: 65px; font-size: 16px;"><a href="/default.aspx">Continue shopping</a></div>

            </div>
            
                            
        </div>

        <asp:HiddenField ID="OrderNumber" runat="server" />
        <asp:HiddenField ID="AmountDue" runat="server" />
        <asp:HiddenField ID="CartOrgs" runat="server" />
        <asp:HiddenField ID="hf_onConfirmation" runat="server" Value="false" ClientIDMode="Static" />

    </asp:View>
    <asp:View ID="EmptyCartView" runat="server">
        <p class="Warning">Sorry, but your cart is empty and may have expired. Please try again.</p>
        <div class="InputRow">
            <a href="/" title="Home" class="CommandLink">Home</a>
        </div>
    </asp:View>
    <asp:View ID="SuccessView" runat="server">
        <p class="SuccessMessage">
            Thank you! Your order has been processed. You should receive an email confirmation
            shortly.
        </p>
        <ul class="LinkMenu">
            <li><a href="/" title="Home" class="CommandLink">Home</a></li>
        </ul>
    </asp:View>
</asp:MultiView>

<script>

    $(document).keydown(function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            e.preventDefault();
            nextPanel();
            return false;
        }
    });

    $(document).ready(function () {
        var state = parseInt($("#hf_state").val(), 10);
        $(".accordion").accordion({ active: state });
        Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
    });
    function EndRequestHandler() {

        var state = parseInt($("#hf_state").val(), 10);
        $(".accordion").accordion({ active: state });
        //$("#lb_loggedIn").delay(5000).fadeOut(1500);
        //$("#lb_newAccountCreated").delay(5000).fadeOut(1500);
        //if ($("#hf_onConfirmation").val() == "true") {
        //    $(".accordion").accordion({ active: 4 });
        //}
        //else if ($("#HiddenField1").val() == "true") {
        //    $(".accordion").accordion({ active: 1 });
        //}

        if ($("#hf_sliderState").val() == "open") {
            $("#newPassDiv").show();
            $("#havePassDiv").hide();
        }

    }

    //$("#next0").click(function () {
    //    $("#panelWarning0").html("");
    //    if (Page_ClientValidate('email')) {
    //        if ($("#rb_havePass").prop('checked') == true) {
    //            if ($("#tb_password").val().length == 0) {

    //                $("#panelWarning0").html("Please input your password");
    //            }
    //        }

    //        else {
    //            if ($("#tb_newPassword1").val().length == 0) {

    //                $("#panelWarning0").html("Please input a password");
    //            }
    //            else {
    //                if ($("#tb_newPassword1").val() != $("#tb_newPassword2").val()) {

    //                    $("#panelWarning0").html("Passwords do not match.");
    //                }
    //            }
    //        }
    //if ($("#panelWarning0").html().length == 0) {
    //    $(".accordion").accordion({ active: 1 });
    //}

    //   }
    // });

    //function nextPanel() {
    //    if ($("#hf_state").val() == "1") {
    //        if (Page_ClientValidate('addressInfo')) {
    //            $(".accordion").accordion({ active: 2 });
    //            $("#hf_state").val("2");
    //        }
    //    }
    //    else if ($("#hf_state").val() == "2") {
    //        if (Page_ClientValidate('deliveryMethodValidator')) {
    //            $(".accordion").accordion({ active: 3 });
    //            $("#hf_state").val("3");
    //        }
    //    }
    //    else if ($("#hf_state").val() == "3") {
    //        if (Page_ClientValidate('paymentInfo')) {
    //            $(".accordion").accordion({ active: 4 });
    //            $("#hf_state").val("4");
    //            $("#name").html($("#tb_name").val());
    //            $("#address1").html($("#tb_streetAddress").val());
    //            $("#address2").html($("#tb_subAddress").val());
    //            $("#cityStateZip").html($("#tb_City").val() + ", " + $("#dd_states").val() + " " + $("#tb_zip").val());
    //            $("#country").html($("#dd_country option:selected").text());
    //            $("#phone").html("Phone: " + $("#tb_phoneNumber").val());
    //            if ($("#CardTypeField option:selected").text().toUpperCase().indexOf("VISA") >= 0) {
    //                $("#paymentSummary").html("<img src=\"/media/unmanaged/images/visa.png\" />");
    //            }
    //            if ($("#CardTypeField option:selected").text().toUpperCase().indexOf("MASTER") >= 0) {
    //                $("#paymentSummary").html("<img src=\"/media/unmanaged/images/mastercard.jpg\" />");
    //            }
    //            if ($("#CardTypeField option:selected").text().toUpperCase().indexOf("AMERICAN") >= 0) {
    //                $("#paymentSummary").html("<img src=\"/media/unmanaged/images/amex.png\" width=\"40\" height=\"25\" />");
    //            }
    //            $("#endingIn").html(" ending in " + $("#CardNumField").val().substr($("#CardNumField").val().length - 4, 4));
    //            $("#deliveryChoice").html($("#dd_delivery option:selected").text());
    //            if ($("#dd_delivery option:selected").text().toUpperCase().indexOf("HOLD") >= 0) {

    //                $("#shippingHeader").html("Account Information");
    //            }
    //        }
    //    }
    //}
    $("#next").click(function () {
        nextPanel();
    });
    $("#next2").click(function () {
        nextPanel();

    });
    $("#next3").click(function () {
        nextPanel();


    });
    $("#prev1").click(function () {
        $(".accordion").accordion({ active: 1 });
        $("#hf_state").val("1");
    });
    $("#prev2").click(function () {
        $(".accordion").accordion({ active: 2 });
        $("#hf_state").val("2");
    });
    $("#prev3").click(function () {
        $(".accordion").accordion({ active: 3 });
        $("#hf_state").val("3");
    });
    $("#changeAddress").click(function () {
        $(".accordion").accordion({ active: 1 });
        $("#hf_state").val("1");
    });
    $("#changePayment").click(function () {
        $(".accordion").accordion({ active: 3 });
        $("#hf_state").val("3");
    });
    $("#changeDelivery").click(function () {
        $(".accordion").accordion({ active: 2 });
        $("#hf_state").val("2");
    });



</script>

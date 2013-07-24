using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf;
using Nysf.Tessitura;
using Nysf.Types;
using System.Text;
using System.Data;
using System.Web.Configuration;

namespace tickets.usercontrols
{
    public partial class CheckoutFormNew : System.Web.UI.UserControl
    {
        
        protected void Page_Load(object sender, EventArgs e)
        {
            Cart cart = WebClient.GetCart();
            if (!Page.IsPostBack)
            {
                if (WebClient.IsLoggedIn())
                {
                    hf_state.Value = "1";
                    UpdatePanel3.Update();
                }
       
                Utility.SetReferer(ViewState);
                if (cart == null || !cart.HasItems)
                    CheckoutViews.SetActiveView(EmptyCartView);
                else
                {
                    // Build the UI
                    buildCart(cart);
                    OrderNumber.Value = cart.Id.ToString();
                    AmountDue.Value = cart.Total.ToString();

                    // Build the card type selector
                    Dictionary<int, string> cardTypes = WebClient.GetCreditCardTypes();
                    foreach (int key in cardTypes.Keys)
                    {
                        CardTypeField.Items.Add(new ListItem(cardTypes[key], key.ToString()));
                    }


                    List<Organization> orgs = new List<Organization>();
                    foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                    {
                        if (!orgs.Contains(seatGroup.Performance.Organization))
                            orgs.Add(seatGroup.Performance.Organization);
                        Performance perf = seatGroup.Performance;
                        if (perf.WebContent != null && perf.WebContent.Rows.Count > 0)
                        {
                            foreach (DataRow row in perf.WebContent.Rows)
                                if ((string)row["content_type_desc"] == "Amex Presale" && (string)row["content_value"] == "1")
                                {

                                    amexOnlyBlurb.Text = "Exclusive Pre-Sale: Tickets for “" + perf.Name + "” are currently available only for American Express ® Cardmembers.";
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("id");
                                    dt.Columns.Add("value");
                                    dt.Rows.Add(new string[2] { "43", "American Express" });
                                    CardTypeField.DataSource = dt;
                                    CardTypeField.DataTextField = "value";
                                    CardTypeField.DataValueField = "id";
                                    CardTypeField.DataBind();
                                    int x = 5;

                                }
                        }

                    }
                    StringBuilder orgsText = new StringBuilder();
                    if (orgs.Count > 0)
                        orgsText.Append(orgs[0].ToString());
                    for (int c = 1; c < orgs.Count; c++)
                    {
                        orgsText.Append("," + orgs[c].ToString());
                    }
                    CartOrgs.Value = orgsText.ToString();



                    // Build the years selector

                    for (int i = 0; i < 20; i++)
                    {
                        ExpYearField.Items.Add((DateTime.Now.Year + i).ToString());
                    }

                    // Determine if mail delivery option should be available

                    bool foundPerfSoon = false;
                    foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                    {
                        if (seatGroup.Performance.StartTime < DateTime.Now.AddDays(14))
                        {
                            foundPerfSoon = true;
                            break;
                        }
                    }
                    if (!foundPerfSoon)
                        dd_delivery.Items.Add(new ListItem("Mail", "1"));


                    DataSet countries = WebClient.GetCountries();
                    dd_country.DataSource = countries.Tables["Country"];
                    dd_country.DataValueField = "id";
                    dd_country.DataTextField = "description";
                    dd_country.AppendDataBoundItems = true;
                    dd_country.DataBind();
                    dd_country.SelectedValue = "1";

                    CheckoutViews.SetActiveView(BillingEntryView);
                    if (WebClient.IsLoggedIn())
                    {
                        ConfigureAddressFields();
                    }
                    configureCheckBoxes();

                }

                //  BillingErrorMessage.Visible = false;
            }
        }
        private void ConfigureAddressFields()
        {

            // Build the Tess-dependend UI elements


            AccountInfo accountInfo = WebClient.GetAccountInformation();


            AccountPerson person = accountInfo.People.Person1;
            if (!String.IsNullOrEmpty(person.FirstName))
                NameField.Text = person.FirstName;
            if (!String.IsNullOrEmpty(person.MiddleName))
                NameField.Text += " " + person.MiddleName;
            if (!String.IsNullOrEmpty(person.LastName))
                NameField.Text += " " + person.LastName;
            lb_name.Text = NameField.Text;
            lb_loggedIn.Text = "Welcome, "+NameField.Text+"!";
            lb_newAccountCreated.Text = "Thank you, " + NameField.Text + "! An account will be created for you when you complete your purchase.";
            if (!String.IsNullOrEmpty(accountInfo.Email))
                tb_email.Text = accountInfo.Email;
            if (!String.IsNullOrEmpty(accountInfo.Phone))
                tb_phoneNumber.Text = formatPhoneNumber(accountInfo.Phone);
            if (accountInfo.CountryId != null)
            {
                dd_country.SelectedValue = accountInfo.CountryId.ToString();
            }

            configureBasedOnCountry();

            if (!String.IsNullOrEmpty(accountInfo.Address))
                tb_streetAddress.Text = accountInfo.Address;
            if (!String.IsNullOrEmpty(accountInfo.SubAddress))
                tb_subAddress.Text = accountInfo.SubAddress;
            if (!String.IsNullOrEmpty(accountInfo.City))
                tb_City.Text = accountInfo.City;
            if (!String.IsNullOrEmpty(accountInfo.StateId))
                dd_states.SelectedValue = accountInfo.StateId;
            if (!String.IsNullOrEmpty(accountInfo.PostalCode))
                tb_zip.Text = formatZip(accountInfo.PostalCode);

            
        }
        private void configureBasedOnCountry()
        {
            // Fill dd_states with states of the selected country

            if (dd_country.SelectedValue != "")
            {
                DataSet states = WebClient.GetStates(int.Parse(dd_country.SelectedValue));
                dd_states.DataSource = states.Tables["StateProvince"];
                dd_states.DataValueField = "id";
                dd_states.DataTextField = "description";
             //   dd_states.AppendDataBoundItems = true;
                dd_states.DataBind();
                if (dd_states.Items.Count > 0)
                {
                    int i = 0;
                    foreach (ListItem li in dd_states.Items)
                    {
                        if (li.Text == "New York")
                        {
                            dd_states.SelectedIndex = i;
                        }
                        i++;
                    }
                }
                //else
                //    dd_states.Visible = false;

            }


            // Show or hide the address fields, "mail" shipping option based on whether states were
            // found

            if (dd_states.Items.Count > 1)
            {
                if (dd_delivery.Items.Count == 3)
                    dd_delivery.Items[2].Enabled = true;
                stateGroup.Visible = true;
            }
            else
            {
                if (dd_delivery.Items.Count == 3)
                {
                    dd_delivery.SelectedItem.Selected = false;
                    dd_delivery.Items[0].Selected = true;
                    dd_delivery.Items[2].Enabled = false;
                }
                stateGroup.Visible = false;
            }
        }
        protected void VerifyExpDateInFuture(object source, ServerValidateEventArgs args)
        {
            int inputYear =
                ExpYearField.SelectedValue == "" ? 0 : Int32.Parse(ExpYearField.SelectedValue);
            int inputMonth =
                ExpMonthField.SelectedValue == "" ? 0 : Int32.Parse(ExpMonthField.SelectedValue);
            if (inputYear == DateTime.Now.Year && inputMonth < DateTime.Now.Month)
                args.IsValid = false;
            else
                args.IsValid = true;
        }
        protected void cartUpdate(object sender, EventArgs e)
        {
            foreach (string key in Request.Form.AllKeys)
            {
                if (key.StartsWith("remove_"))
                {
                    string[] parts = key.Split('_');
                    WebClient.RemoveSeatGroupFromCart(Int32.Parse(parts[1]), Int32.Parse(parts[2]));
                    Response.Redirect(Request.RawUrl);
                }
            }
        }
        private void buildCart(Cart cart)
        {
            cartWidget.buildCart(cart);
            amountLiteral.Text = cart.Total.ToString("C");
            UpdatePanel1.Update();
            UpdatePanel2.Update();
        }
        protected void loginCreate(object sender, EventArgs e)
        {
            Label lab = new Label();
            if (rb_havePass.Checked)
            {
                if (tb_email.Text.Trim().Length > 0 && tb_password.Text.Trim().Length > 0)
                {
                    if (WebClient.Login(tb_email.Text, tb_password.Text))
                    {
                        HiddenField1.Value = "true";
                        lb_loggedIn.Visible = true;
                        ConfigureAddressFields();
                        hf_state.Value = "1";
                        
                        UpdatePanel3.Update();
                        UpdatePanel4.Update();
                        UpdatePanel5.Update();
                    }
                    else
                    {
                        lab.ID = "warningLabel0";
                        lab.Text = "Login failed.";
                        panelWarning0.Controls.Add(lab);
                    }
                }
                else
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Please input both an email and a password.";
                    panelWarning0.Controls.Add(lab);
                }

            }
            if (rb_noPass.Checked)
            {
                hf_sliderState.Value = "open";
                //Organization org;
                //if (Request.RawUrl.Contains("joespub"))
                //    org = Organization.JoesPub;
                //else
                //    org = Organization.PublicTheater;
                if (tb_newEmail.Text.Trim().Length == 0)
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Please input an email address.";
                    panelWarning0.Controls.Add(lab);
                    UpdatePanel3.Update();
                    return;
                }
                if (tb_newFirstName.Text.Trim().Length == 0 || tb_newLastName.Text.Trim().Length == 0)
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Please input both your first and last name.";
                    panelWarning0.Controls.Add(lab);
                    UpdatePanel3.Update();
                    return;
                }
                if (tb_newPassword1.Text.Trim().Length == 0)
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Please input a password.";
                    panelWarning0.Controls.Add(lab);
                    UpdatePanel3.Update();
                    return;
                }
                if (tb_newPassword1.Text.Trim() != tb_newPassword2.Text.Trim())
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Passwords do not match.";
                    panelWarning0.Controls.Add(lab);
                    UpdatePanel3.Update();
                    return;
                }
                bool success;
                success = WebClient.RegisterNewConstituentWithPassword( tb_newEmail.Text, tb_newPassword1.Text, tb_newFirstName.Text, tb_newLastName.Text, -1, Organization.Other);
                if (success)
                {
                    HiddenField1.Value = "true";
                    lb_newAccountCreated.Visible = true;
                    ConfigureAddressFields();
                    hf_state.Value = "1";
                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
                }
                else
                {
                    lab.ID = "warningLabel0";
                    lab.Text = "Account creation failed. If you already have an account but forgot your password please click ";
                    HyperLink hl = new HyperLink();
                    hl.NavigateUrl = "/recover";
                    hl.Text = "here.";
                    panelWarning0.Controls.Add(lab);
                    panelWarning0.Controls.Add(hl);
                    UpdatePanel3.Update();
                }
                
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // Save address to primary address
                
                AccountInfo accountInfo = WebClient.GetAccountInformation();
                accountInfo.Address = tb_streetAddress.Text.Trim();
                accountInfo.SubAddress = tb_subAddress.Text.Trim();
                accountInfo.City = tb_City.Text.Trim();
                accountInfo.StateId = dd_states.SelectedValue;
                accountInfo.PostalCode = tb_zip.Text.Trim();
                //accountInfo.AddressTypeId = Int32.Parse(AddressTypeField.SelectedValue);
                accountInfo.Phone = tb_phoneNumber.Text.Trim();
                WebClient.UpdateAccountInfo(accountInfo);

                Nysf.Tessitura.AttributeCollection sysAtts = WebClient.GetAttributes();
                ConstituentAttributeCollection acctAtts = WebClient.GetConstituentAttributes();


                Cart cart = WebClient.GetCart();
                Dictionary<int,int> perfs=new Dictionary<int,int>();

                foreach (CartSeatGroupItem item in cart.SeatGroups)
                {
                    Performance perf = item.Performance;
                    if (!perfs.Keys.Contains(perf.Id))
                        perfs.Add(perf.Id, 0);
                    perfs[perf.Id] += item.SeatCount;
                    foreach (DataRow row in perf.WebContent.Rows)
                    {
                        if ((string)row["content_type_desc"] == "Ticket Limit" && ((string)row["content_value"]).Length > 0)
                        {
                            if (perfs[perf.Id] > Convert.ToInt32(((string)row["content_value"])))
                            {
                                BillingErrorMessage.Visible = true;
                                BillingErrorMessage.InnerHtml="Your cart exceeds the ticket limit of "+(string)row["content_value"]+" set for " +perf.Name + ". Please adjust your cart to get below this limit";
                                UpdatePanel2.Update();
                                return;
                            }
                        }
                    }

                }

                if (cbNyt.Checked)
                {
                    // if the box is checked

                    Nysf.Tessitura.Attribute attrJP = sysAtts.GetAttributeByName("NYMag Promo");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrJP.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrJP.Id, 1, acctAtts.HasAttributeValuePair(attrJP.Id, 0));
                        }
                        catch { }
                    }
                }
                if (cbPT.Checked)
                {
                    // if the box is checked

                    Nysf.Tessitura.Attribute attrJP = sysAtts.GetAttributeByName("cp_Em_The Public Theater");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrJP.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrJP.Id, 1, acctAtts.HasAttributeValuePair(attrJP.Id, 0));
                        }
                        catch { }
                    }
                }
                if (cbJP.Checked || cbJP0.Checked)
                {
                    // if the box is checked
                    Nysf.Tessitura.Attribute attrPT = sysAtts.GetAttributeByName("cp_Em_Joe’s Pub");                   
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrPT.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrPT.Id, 1, acctAtts.HasAttributeValuePair(attrPT.Id, 0));
                        }
                        catch { }
                    }
                }

                if (cbSitP.Checked)
                {
                    // if the box is checked
                    Nysf.Tessitura.Attribute attrPT = sysAtts.GetAttributeByName("cp_Em_Shakespeare in the Park");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrPT.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrPT.Id, 1, acctAtts.HasAttributeValuePair(attrPT.Id, 0));
                        }
                        catch { }
                    }
                }

                CheckoutResult result = WebClient.Checkout(Int32.Parse(OrderNumber.Value),
                    (Decimal)WebClient.GetCart().Total, Int32.Parse(dd_delivery.SelectedValue),
                    NameField.Text.Trim(), CardNumField.Text.Trim(),
                    Int32.Parse(CardTypeField.SelectedValue),
                    Int32.Parse(ExpMonthField.SelectedValue),
                    Int32.Parse(ExpYearField.SelectedValue),
                    AuthCodeField.Text.Trim());

               // CheckoutResult result = CheckoutResult.Succeeded;
                if (result == CheckoutResult.Succeeded)
                {
                    try
                    {
                        WebClient.SendOrderConfirmationEmail(Int32.Parse(OrderNumber.Value),
                            CartOrgs.Value);
                    }
                    catch (Exception) { }
                    //CheckoutViews.SetActiveView(SuccessView);
                    Response.Redirect("confirm.aspx");
                }
                else
                {
                    hf_state.Value = "3";
                    BillingErrorMessage.Visible = true;
                
                }
            }
        }
        private void configureCheckBoxes()
        {
            if (HttpContext.Current.Request.Url.Authority.ToUpper().Contains("JOESPUB"))
                
            {

                joesPubCheckBox.Visible = false;
                cbPT.Checked = false;
                cbJP0.Checked = true;
            }
            else
           { 
                firstJpCheckBox.Visible = false;
            }

        }
        protected void dd_country_SelectedIndexChanged(object sender, EventArgs e)
        {
            configureBasedOnCountry();
           // UpdatePanel4.Update();
        }

        private string formatPhoneNumber(string number)
        {
            if (number.Length == 10)
                return "(" + number.Substring(0, 3) + ")-" + number.Substring(3, 3) + "-" + number.Substring(6);
            if (number.Length == 7)
                return number.Substring(0, 3) + "-" + number.Substring(3);
            else
                return number;
        }
        private string formatZip(string zip)
        {
            if (zip.Length == 9)
                return zip.Substring(0, 5) + "-" + zip.Substring(5);
            else
                return zip;
            
        }
        protected bool IsGroupValid(string sValidationGroup)
        {
           foreach (BaseValidator validator in Page.Validators)
           {
              if (validator.ValidationGroup == sValidationGroup)
              {
                 bool fValid = validator.IsValid;
                 if (fValid)
                 {
                    validator.Validate();
                    fValid = validator.IsValid;
                    validator.IsValid = true;
                 }
                 if (!fValid)
                 {
                     validator.Validate();
                    return false;
                 }
              }

           }
           return true;
        }
        protected void nextPanel(object sender, EventArgs e)
        {
            if(hf_state.Value=="1")
            {
                if(IsGroupValid("addressInfo"))
                {
                    hf_state.Value = "2";
                        
                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
                 }
            }
            else if(hf_state.Value=="2")
            {
                if(IsGroupValid("deliveryMethodValidator"))
                {
                    hf_state.Value = "3";
                        
                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
                }
            }
            else if(hf_state.Value=="3")
            {
                if(IsGroupValid("paymentInfo"))
                {
                    hf_state.Value = "4";
                        
                    name.Text=lb_name.Text;
                    address1.Text=tb_streetAddress.Text;
                    address2.Text=tb_subAddress.Text;
                    cityStateZip.Text=tb_City.Text+", "+dd_states.SelectedValue+" " +tb_zip.Text;
                    country.Text=dd_country.SelectedItem.Text;
                    phone.Text=tb_phoneNumber.Text;
                    if(CardTypeField.SelectedItem.Text.ToUpper().Contains("VISA"))
                        paymentSummary.InnerHtml="<img src=\"/media/unmanaged/images/visa.png\" />";
                    else if(CardTypeField.SelectedItem.Text.ToUpper().Contains("MASTER"))
                        paymentSummary.InnerHtml="<img src=\"/media/unmanaged/images/mastercard.jpg\" />";
                    else if(CardTypeField.SelectedItem.Text.ToUpper().Contains("AMERICAN"))
                        paymentSummary.InnerHtml="<img src=\"/media/unmanaged/images/amex.jpg\" />";
                    endingIn.InnerHtml=" ending in "+CardNumField.Text.Substring(CardNumField.Text.Length-4,4);
                    deliveryChoice.InnerHtml=dd_delivery.SelectedItem.Text;
                    if(dd_delivery.SelectedItem.Text.ToUpper().Contains("HOLD"))
                        shippingHeader.InnerText="Account Information";
                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
                    confirmationUpdatePanel.Update();
                    
                }
            }

        }
        protected void previousPanel(object sender, EventArgs e)
        {
            if (hf_state.Value == "3")
            {
              
                    hf_state.Value = "2";

                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
            }
            
            else if (hf_state.Value == "4")
            {

                    hf_state.Value = "3";

                    UpdatePanel3.Update();
                    UpdatePanel4.Update();
                    UpdatePanel5.Update();
            }
            else if (hf_state.Value == "2")
            {

                hf_state.Value = "1";

                UpdatePanel3.Update();
                UpdatePanel4.Update();
                UpdatePanel5.Update();
            }
        
            }
        }
    }

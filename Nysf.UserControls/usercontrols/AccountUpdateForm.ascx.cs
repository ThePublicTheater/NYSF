using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.UserControls
{
    public partial class AccountUpdateForm : GenericControl
    {

        #region Properties

        public string EmailChangeNotificationText
        {
            get { return EmailChangeNotification.Text; }
            set { EmailChangeNotification.Text = value; }
        }

        public string UsernameChangeWarningText
        {
            get { return UsernameChangeWarning.Text; }
            set { UsernameChangeWarning.Text = value; }
        }

        public string EmailActivationBlurbText
        {
            get { return EmailActivationBlurb.Text; }
            set { EmailActivationBlurb.Text = value; }
        }

        public string ConfirmationBlurbText
        {
            get { return ConfirmationBlurb.InnerText; }
            set { ConfirmationBlurb.InnerText = value; }
        }

        public string EmailExistsWarningText
        {
            get { return EmailExistsWarning.InnerText; }
            set { EmailExistsWarning.InnerText = value; }
        }

        public int EmailTemplateNumber { get; set; }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!WebClient.IsLoggedIn())
                {
                    throw new ApplicationException("The AccountUpdateForm requires an "
                        + "authenticated Tessitura session.");
                }

                // TODO: make sure EmailTemplateNumber is set via attribute

                Utility.SetReferer(ViewState);

                // Build the Tess-dependend UI elements

                DataSet countriesWithStates = WebClient.GetCountries(true);
                DataSet countriesWithoutStates = WebClient.GetCountries(false);
                CountryField.DataSource = countriesWithStates.Tables["Country"];
                CountryField.DataValueField = "id";
                CountryField.DataTextField = "description";
                CountryField.AppendDataBoundItems = true;
                CountryField.DataBind();
                CountryField.DataSource = countriesWithoutStates.Tables["Country"];
                CountryField.DataBind();

                // Get the current account information

                AccountInfo oldInfo = WebClient.GetAccountInformation();

                // Fill out the UI with the current account information

                AccountPerson person1 = oldInfo.People.Person1;
                AccountPerson person2 = oldInfo.People.Person2;

                if (!String.IsNullOrEmpty(person1.Prefix))
                {
                    // Make sure the prefix exists in the UI

                    string prefix = person1.Prefix;
                    bool prefixExistsInUi = false;
                    foreach (ListItem item in PrefixField.Items)
                    {
                        if (item.Text == prefix)
                        {
                            prefixExistsInUi = true;
                            break;
                        }
                    }
                    if (!prefixExistsInUi)
                    {
                        ListItem newItem = new ListItem(prefix, prefix);
                        ListItem newItem2 = new ListItem(prefix, prefix);
                        PrefixField.Items.Add(newItem);
                        Prefix2Field.Items.Add(newItem2);
                    }
                    PrefixField.SelectedValue = prefix;
                }
                if (!String.IsNullOrEmpty(person1.FirstName))
                    FirstNameField.Text = person1.FirstName;
                if (!String.IsNullOrEmpty(person1.MiddleName))
                    MiddleNameField.Text = person1.MiddleName;
                if (!String.IsNullOrEmpty(person1.LastName))
                    LastNameField.Text = person1.LastName;
                if (!String.IsNullOrEmpty(person1.Suffix))
                    SuffixField.Value = person1.Suffix;
                if (person1.Gender != null)
                {
                    if (person1.Gender != 'M' && person1.Gender != 'F' && person1.Gender != 'I')
                    {
                        GenderField.Items.Add(
                            new ListItem(person1.Gender.ToString(), person1.Gender.ToString()));
                        Gender2Field.Items.Add(
                            new ListItem(person1.Gender.ToString(), person1.Gender.ToString()));
                    }
                    GenderField.SelectedValue = person1.Gender.ToString();
                }
                if (person2 != null)
                {
                    Name2Button.Visible = false;
                    Name2Fieldset.Visible = true;
                    if (!String.IsNullOrEmpty(person2.Prefix))
                    {
                        // Make sure the prefix exists in the UI

                        string prefix = person2.Prefix;
                        bool prefixExistsInUi = false;
                        foreach (ListItem item in Prefix2Field.Items)
                        {
                            if (item.Text == prefix)
                            {
                                prefixExistsInUi = true;
                                break;
                            }
                        }
                        if (!prefixExistsInUi)
                        {
                            ListItem newItem = new ListItem(prefix, prefix);
                            ListItem newItem2 = new ListItem(prefix, prefix);
                            PrefixField.Items.Add(newItem);
                            Prefix2Field.Items.Add(newItem2);
                        }
                        Prefix2Field.SelectedValue = prefix;
                    }
                    if (!String.IsNullOrEmpty(person2.FirstName))
                        FirstName2Field.Text = person2.FirstName;
                    if (!String.IsNullOrEmpty(person2.MiddleName))
                        MiddleName2Field.Text = person2.MiddleName;
                    if (!String.IsNullOrEmpty(person2.LastName))
                        LastName2Field.Text = person2.LastName;
                    if (!String.IsNullOrEmpty(person2.Suffix))
                        Suffix2Field.Value = person2.Suffix;
                    if (person2.Gender != null)
                    {
                        if (person2.Gender != 'M' && person2.Gender != 'F' && person2.Gender != 'I')
                        {
                            GenderField.Items.Add(
                                new ListItem(person2.Gender.ToString(), person2.Gender.ToString()));
                            Gender2Field.Items.Add(
                                new ListItem(person2.Gender.ToString(), person2.Gender.ToString()));
                        }
                        Gender2Field.SelectedValue = person2.Gender.ToString();
                    }
                }
                if (!String.IsNullOrEmpty(oldInfo.Email))
                    EmailField.Text = oldInfo.Email;
                if (!String.IsNullOrEmpty(oldInfo.Phone))
                    PhoneField.Text = oldInfo.Phone;
                if (!String.IsNullOrEmpty(oldInfo.Phone2))
                    Phone2Field.Text = oldInfo.Phone2;
                if (!String.IsNullOrEmpty(oldInfo.Fax))
                    FaxField.Text = oldInfo.Fax;
                if (oldInfo.CountryId != null)
                {
                    CountryField.SelectedValue = oldInfo.CountryId.ToString();
                    ConfigureAddressFields();
                }
                if (!String.IsNullOrEmpty(oldInfo.Address))
                    AddressField.Text = oldInfo.Address;
                if (!String.IsNullOrEmpty(oldInfo.SubAddress))
                    SubAddressField.Text = oldInfo.SubAddress;
                if (!String.IsNullOrEmpty(oldInfo.City))
                    CityField.Text = oldInfo.City;
                if (!String.IsNullOrEmpty(oldInfo.StateId))
                    StateField.SelectedValue = oldInfo.StateId;
                if (!String.IsNullOrEmpty(oldInfo.PostalCode))
                    ZipField.Text = oldInfo.PostalCode;
                if (oldInfo.AddressTypeId != null)
                {
                    if (oldInfo.AddressTypeId != 3 && oldInfo.AddressTypeId != 2
                        && oldInfo.AddressTypeId != 13)
                    {
                        AddressTypeField.Items.Add(new ListItem(
                            oldInfo.AddressTypeId.ToString(), oldInfo.AddressTypeDesc));
                    }
                    AddressTypeField.SelectedValue = oldInfo.AddressTypeId.ToString();
                }

                WantEmailField.Value = (oldInfo.WantsEmail ?? true).ToString();
                WantMailField.Value = (oldInfo.WantsMail ?? true).ToString();
                WantPhoneField.Value = (oldInfo.WantsPhone ?? true).ToString();
                CanReceiveHtmlEmailField.Value = (oldInfo.CanReceiveHtmlEmail ?? true).ToString();
                BusinessTitleField.Value = oldInfo.BusinessTitle;
                OldEmailField.Value = oldInfo.Email;
            }
        }

        protected void RedirectToReferer(object sender, EventArgs e)
        {
            Utility.RedirectToReferer(ViewState);
        }

        private void ConfigureAddressFields()
        {
            // Reset and empty StateField

            StateField.SelectedIndex = 0;
            for (int i = StateField.Items.Count - 1; i > 0; i--)
                StateField.Items.RemoveAt(i);

            // Fill StateField with states of the selected country

            if (CountryField.SelectedValue != "0")
            {
                DataSet states = WebClient.GetStates(int.Parse(CountryField.SelectedValue));
                StateField.DataSource = states.Tables["StateProvince"];
                StateField.DataValueField = "id";
                StateField.DataTextField = "description";
                StateField.AppendDataBoundItems = true;
                StateField.DataBind();
            }

            // Show or hide the address fields based on whether states were found

            if (StateField.Items.Count > 1)
                StateFieldGroup.Visible = true;
            else
                StateFieldGroup.Visible = false;
        }

        protected void CountryChanged(object sender, EventArgs e)
        {
            ConfigureAddressFields();
        }

        protected void Name2Button_Click(object sender, EventArgs e)
        {
            Name2Button.Visible = false;
            Name2Fieldset.Visible = true;
            ConfirmationBlurb.Visible = false;
        }

        protected void Save_Click(object sender, EventArgs e)
        {
            ConfirmationBlurb.Visible = false;
            if (Page.IsValid)
            {
                if (OldEmailField.Value == EmailField.Text.Trim())
                {
                    SubmitUpdate();
                    ConfirmationBlurb.Visible = true;
                }
                else
                {
                    AccountUpdateViews.ActiveViewIndex = 1;
                    EnteredEmailBlurb.Text = EmailField.Text.Trim();
                }
            }
        }

        protected void ConfirmSubmit(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                if (SubmitUpdate())
                {
                    WebClient.SetNewEmail(EmailField.Text);
                    WebClient.SendRecoveryEmail(EmailField.Text, EmailTemplateNumber);
                    AccountUpdateViews.ActiveViewIndex = 2;
                }
                else
                {
                    AccountUpdateViews.ActiveViewIndex = 0;
                    EmailExistsWarning.Visible = true;
                }
            }
        }

        protected bool SubmitUpdate()
        {
            AccountPerson person1 = null;
            AccountPerson person2 = null;
            if (/*FirstNameField.Text.Trim() != "" &&*/ LastNameField.Text.Trim() != "")
            {
                char? gender = null;
                if (GenderField.SelectedValue != "0")
                    gender = GenderField.SelectedValue[0];
                person1 = new AccountPerson(
                    PrefixField.SelectedValue,
                    FirstNameField.Text,
                    MiddleNameField.Text,
                    LastNameField.Text,
                    SuffixField.Value,
                    gender);
            }
            if (/*FirstName2Field.Text.Trim() != "" &&*/ LastName2Field.Text.Trim() != "")
            {
                char? gender = null;
                if (Gender2Field.SelectedValue != "0")
                    gender = Gender2Field.SelectedValue[0];
                person2 = new AccountPerson(
                    Prefix2Field.SelectedValue,
                    FirstName2Field.Text,
                    MiddleName2Field.Text,
                    LastName2Field.Text,
                    Suffix2Field.Value,
                    gender);
            }
            AccountPersonPair pair = new AccountPersonPair(person1, person2);
            AccountInfo newInfo = new AccountInfo();
            newInfo.BusinessTitle = BusinessTitleField.Value;
            newInfo.CanReceiveHtmlEmail = Boolean.Parse(CanReceiveHtmlEmailField.Value);
            if (CountryField.SelectedValue != "0")
            {
                newInfo.CountryId = Int32.Parse(CountryField.SelectedValue);
                newInfo.PostalCode = ZipField.Text;
                if (StateField.SelectedValue != "0")
                    newInfo.StateId = StateField.SelectedValue;
                newInfo.SubAddress = SubAddressField.Text;
                newInfo.City = CityField.Text;
                newInfo.Address = AddressField.Text;
                if (AddressTypeField.SelectedValue != "0")
                    newInfo.AddressTypeId = Int32.Parse(AddressTypeField.SelectedValue);
            }
            newInfo.Email = EmailField.Text;
            newInfo.Fax = FaxField.Text;
            newInfo.People = pair;
            newInfo.Phone = PhoneField.Text;
            newInfo.Phone2 = Phone2Field.Text;
            newInfo.WantsEmail = Boolean.Parse(WantEmailField.Value);
            newInfo.WantsMail = Boolean.Parse(WantMailField.Value);
            newInfo.WantsPhone = Boolean.Parse(WantPhoneField.Value);
            return WebClient.UpdateAccountInfo(newInfo, true);
        }

        protected void GoBack(object source, EventArgs e)
        {
            AccountUpdateViews.ActiveViewIndex = 0;
            ConfirmationBlurb.Visible = false;
            EmailExistsWarning.Visible = false;
        }

        protected void ValidateNames(object source, ServerValidateEventArgs args)
        {
            if ((String.IsNullOrWhiteSpace(FirstNameField.Text) ||
                    String.IsNullOrWhiteSpace(LastNameField.Text)) &&
                (String.IsNullOrWhiteSpace(FirstName2Field.Text) ||
                    String.IsNullOrWhiteSpace(LastName2Field.Text)))
            {
                args.IsValid = false;
            }
            else
                args.IsValid = true;
        }

        protected void CheckPassword(object source, ServerValidateEventArgs args)
        {
            if (WebClient.RevalidatePassword(PasswordField.Text))
                args.IsValid = true;
            else
                args.IsValid = false;
        }
    }
}
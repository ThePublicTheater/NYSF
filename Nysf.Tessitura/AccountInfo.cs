// TODO: handle nulls in properties

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Nysf.Tessitura
{
    public class AccountInfo
    {
        #region Variables

        private int? customerNumber;
        private AccountPersonPair people;
        private string businessTitle;
        private string email;
        private bool? canReceiveHtmlEmail;
        private string phone;
        private int? phoneId;
        private string phone2;
        private int? phone2Id;
        private string fax;
        private int? faxId;
        private string address;
        private string subAddress;
        private string city;
        private string stateId;
        private string postalCode;
        private int? countryId;
        private string countryDesc;
        private int? addressTypeId;
        private string addressTypeDesc;
        private bool? wantsEmail;
        private bool? wantsMail;
        private bool? wantsPhone;

        #endregion

        #region Properties

        public int? CustomerNumber
        {
            get { return customerNumber; }
            set
            {
                if (value != null && value <= 0)
                    throw new ArgumentException("The customer number must be greater than zero.");
                customerNumber = value;
            }
        }

        public AccountPersonPair People
        {
            get { return people; }
            set
            {
                if (value == null)
                    throw new ArgumentException("An account must have associated people.");
                people = value;
            }
        }

        public string BusinessTitle
        {
            get { return businessTitle; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 55)
                        throw new ArgumentException("The business title must not exceed 55 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                businessTitle = value;
            }
        }

        // EmailUnprotected allows bad email addresses that already exist in Tess when calling
        // GetAccountInfo()

        public string EmailUnprotected
        {
            set { email = String.IsNullOrEmpty(value) ? null : value; }
        }

        public string Email
        {
            get { return email; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                    if (value.Length > 80)
                        throw new ArgumentException("The email address must not exceed 80 "
                            + "characters.");
                    if (value.Length > 0
                        && !Regex.IsMatch(value,
                            "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*"))
                    {
                        throw new ArgumentException("The email address is invalid.");
                    }
                }
                if (value == "")
                    value = null;
                email = value;
            }
        }

        public bool? CanReceiveHtmlEmail
        {
            get { return canReceiveHtmlEmail; }
            set { canReceiveHtmlEmail = value; }
        }

        public string Phone
        {
            get { return phone; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 32)
                        throw new ArgumentException("The primary phone number must not exceed 32 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                phone = value;
                phoneId = null;
            }
        }

        public int? PhoneId
        {
            get { return phoneId; }
        }

        public string Phone2
        {
            get { return phone2; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 32)
                        throw new ArgumentException("The secondary phone number must not exceed 32 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                phone2 = value;
                phone2Id = null;
            }
        }

        public int? Phone2Id
        {
            get { return phone2Id; }
        }

        public string Fax
        {
            get { return fax; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 32)
                        throw new ArgumentException("The fax number must not exceed 32 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                fax = value;
                faxId = null;
            }
        }

        public int? FaxId
        {
            get { return faxId; }
        }

        public string Address
        {
            get { return address; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 55)
                        throw new ArgumentException("The address must not exceed 55 characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                address = value;
            }
        }

        public string SubAddress
        {
            get { return subAddress; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 55)
                        throw new ArgumentException("The sub-address must not exceed 55 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                subAddress = value;
            }
        }

        public string City
        {
            get { return city; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 30)
                        throw new ArgumentException("The city must not exceed 30 characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                city = value;
            }
        }

        public string StateId
        {
            get { return stateId; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 20)
                        throw new ArgumentException("The state ID must not exceed 20 characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                    // TODO: make sure stateId is found in TR_STATE
                }
                if (value == "")
                    value = null;
                stateId = value;
            }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 10)
                        throw new ArgumentException("The postal code must not exceed 10 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                postalCode = value;
            }
        }

        public int? CountryId
        {
            get { return countryId; }
            set
            {
                if (value != null)
                {
                    if (value <= 0)
                        throw new ArgumentException("The country ID must be greater than zero.");
                    // TODO: make sure countryId is found in TR_COUNTRY
                }
                // TODO: replace the following; set countryDesc according to countryId

                // Set the desc. to null when the country ID has changed, because it no longer
                // applies.
                // TODO: make sure this works when id set to null
                if (countryId != value)
                    countryDesc = null;
                countryId = value;
            }
        }

        public string CountryDesc
        {
            get { return countryDesc; }
        }

        public int? AddressTypeId
        {
            get { return addressTypeId; }
            set
            {
                if (value != null)
                {
                    if (value < 0)
                        throw new ArgumentException("The address type ID must be greater than zero.");
                    // TODO: make sure addressTypeId is found in TR_ADDRESS_TYPE
                }
                // TODO: replace the following; set addressTypeDesc according to addressTypeId

                // Set the desc. to null when the addressTypeId has changed, because it no longer
                // applies.
                // TODO: make sure this works when id set to null
                if (addressTypeId != value)
                    addressTypeDesc = null;
                addressTypeId = value;
            }
        }

        public string AddressTypeDesc
        {
            get { return addressTypeDesc; }
        }

        public bool? WantsEmail
        {
            get { return wantsEmail; }
            set { wantsEmail = value; }
        }

        public bool? WantsMail
        {
            get { return wantsMail; }
            set { wantsMail = value; }
        }

        public bool? WantsPhone
        {
            get { return wantsPhone; }
            set { wantsPhone = value; }
        }

        #endregion

        #region Methods

        public AccountInfo() { }

        public AccountInfo(int? customerNumber, AccountPersonPair people, string businessTitle,
            string email, bool? canReceiveHtmlEmail, string phone, int? phoneId, string phone2, 
            int? phone2Id, string fax, int? faxId, string address, string subAddress, string city,
            string stateId, string postalCode, int? countryId, string countryDesc,
            int? addressTypeId, string addressTypeDesc, bool? wantsEmail, bool? wantsMail,
            bool? wantsPhone)
        {
            CustomerNumber = customerNumber;
            People = people;
            BusinessTitle = businessTitle;
            Email = email;
            CanReceiveHtmlEmail = canReceiveHtmlEmail;
            Phone = phone;
            this.phoneId = phoneId;
            Phone2 = phone2;
            this.phone2Id = phone2Id;
            Fax = fax;
            this.faxId = faxId;
            Address = address;
            SubAddress = subAddress;
            City = city;
            StateId = stateId;
            PostalCode = postalCode;
            CountryId = countryId;
            this.countryDesc = countryDesc;
            AddressTypeId = addressTypeId;
            this.addressTypeDesc = addressTypeDesc;
            WantsEmail = wantsEmail;
            WantsMail = wantsMail;
            WantsPhone = wantsPhone;
        }

        // TODO: remove the following method after the usual setter is modified to set desc autom.ly
        public void SetNewCountryIdWithDesc(int? id, string desc)
        {
            CountryId = id;
            countryDesc = desc;
        }

        // TODO: remove the following method after the usual setter is modified to set desc autom.ly
        public void SetAddressTypeIdWithDesc(int? id, string desc)
        {
            AddressTypeId = id;
            addressTypeDesc = desc;
        }

        public void SetPhoneWithId(string number, int? id)
        {
            Phone = number;
            phoneId = id;
        }

        public void SetPhone2WithId(string number, int? id)
        {
            Phone2 = number;
            phone2Id = id;
        }

        public void SetFaxWithId(string number, int? id)
        {
            Fax = number;
            faxId = id;
        }
        
        #endregion
    }
}
using System;

namespace Nysf.Tessitura
{
    public class AccountPerson
    {
        #region Variables

        private string prefix;
        private string firstName;
        private string middleName;
        private string lastName;
        private string suffix;
        private char? gender;

        #endregion

        #region Properties

        public string Prefix
        {
            get { return prefix; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 30)
                        throw new ArgumentException("The prefix must not exceed 30 characters.");
                    // TODO: check if prefix fits TR_PREFIX (add lookup method to ApiInfoClient)
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                prefix = value;
            }
        }

        public string FirstName
        {
            get { return firstName; }
            set
            {
                if (value != null)
                {
                    /*if (String.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("A first name is required.");*/
                    value = value.Trim();
                    if (value.Length > 20)
                        throw new ArgumentException("A first name must not exceed 20 characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                firstName = value;
            }
        }

        public string MiddleName
        {
            get { return middleName; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 20)
                        throw new ArgumentException("The middle name must not exceed 20 "
                            + "characters.");
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                middleName = value;
            }
        }

        public string LastName
        {
            get { return lastName; }
            set
            {
                if (String.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("A last name is required.");
                value = value.Trim();
                if (value.Length > 55)
                    throw new ArgumentException("The last name must not exceed 55 characters.");
                if (value == "[]")
                    throw new ArgumentException("The value, \"[]\", is not allowed.");
                if (value == "")
                    value = null;
                lastName = value;
            }
        }

        public string Suffix
        {
            get { return suffix; }
            set
            {
                if (value != null)
                {
                    value = value.Trim();
                    if (value.Length > 30)
                        throw new ArgumentException("The suffix must not exceed 30 characters.");
                    // TODO: check if suffix fits TR_SUFFIX (add lookup method to ApiInfoClient)
                    if (value == "[]")
                        throw new ArgumentException("The value, \"[]\", is not allowed.");
                }
                if (value == "")
                    value = null;
                suffix = value;
            }
        }

        public char? Gender
        {
            get { return gender; }
            set
            {
                if (value != null && value.ToString().Trim() == "")
                    value = null;
                gender = value;
            }
        }

        #endregion

        #region Methods

        public AccountPerson() { }

        public AccountPerson(string prefix, string firstName, string middleName, string lastName,
            string suffix, char? gender)
            : this(prefix, firstName, middleName, lastName, suffix, gender, true) { }

        public AccountPerson(string prefix, string firstName, string middleName, string lastName,
            string suffix, char? gender, bool protectProperties)
        {
            if (protectProperties)
            {
                Prefix = prefix;
                FirstName = firstName;
                MiddleName = middleName;
                LastName = lastName;
                Suffix = suffix;
                Gender = gender;
            }
            else
            {
                if (!String.IsNullOrWhiteSpace(prefix))
                    this.prefix = prefix;
                if (!String.IsNullOrWhiteSpace(firstName))
                    this.firstName = firstName;
                if (!String.IsNullOrWhiteSpace(middleName))
                    this.middleName = middleName;
                if (!String.IsNullOrWhiteSpace(lastName))
                    this.lastName = lastName;
                if (!String.IsNullOrWhiteSpace(suffix))
                    this.suffix = suffix;
                if (gender != null && gender.ToString().Trim() != "")
                    this.gender = gender;
            }
        }

        #endregion
        
    }
}

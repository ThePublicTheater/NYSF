using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nysf.Tessitura
{
    public class AccountPersonPair
    {
        private AccountPerson person1;
        private AccountPerson person2;

        public AccountPerson Person1
        {
            get { return person1; }
        }

        public AccountPerson Person2
        {
            get { return person2; }
        }

        public AccountPersonPair(AccountPerson p1, AccountPerson p2)
        {
            if (p1 == null && p2 == null)
                throw new ArgumentException("An account requires at least one person.");
            person1 = p1;
            person2 = p2;
            if (person1 == null)
            {
                person1 = person2;
                person2 = null;
            }
        }
    }
}

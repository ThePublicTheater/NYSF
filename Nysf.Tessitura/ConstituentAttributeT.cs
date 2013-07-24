using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class ConstituentAttribute<T> : ConstituentAttribute
    {
        private Object thisAttributeValue;

        public override Object Value
        {
            get { return thisAttributeValue; }
            protected set { thisAttributeValue = value; }
        }

        public ConstituentAttribute(string name, int id, T value)
        {
            Name = name;
            Id = id;
            Value = value;
        }
    }
}
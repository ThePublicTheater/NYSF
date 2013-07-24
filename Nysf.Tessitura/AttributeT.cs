using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class Attribute<T> : Attribute
    {
        private Object[] values;

        public override Object[] Values
        {
            get { return values; }
            protected set { values = value; }
        }

        public Attribute(string name, int id, bool multValAllowed, T[] values)
        {
            Name = name;
            Id = id;
            MultipleValuesAllowed = multValAllowed;
            Values = new Object[values.Length];
            for (int c = 0; c < values.Length; c++)
            {
                Values[c] = values[c];
            }
        }
    }
}
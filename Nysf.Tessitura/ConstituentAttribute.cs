using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public abstract class ConstituentAttribute
    {
        public string Name { get; protected set; }
        public int Id { get; protected set; }
        public abstract Object Value { get; protected set; }

        public Type DataType
        {
            get { return GetType().GetGenericArguments()[0]; }
        }
    }
}
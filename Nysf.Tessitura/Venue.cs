using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class Venue
    {
        private int id;
        private string name;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(
                        "The venue ID must be positive or zero.");
                id = value;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("A venue name is required.");
                if (value.Length > 30)
                    throw new ArgumentException("The venue's name may not exceed 30 characters.");
                name = value;
            }
        }

        public Venue(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
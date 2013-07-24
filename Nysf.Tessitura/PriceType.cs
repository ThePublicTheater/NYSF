using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    /// <summary>
    ///     A price type exposed for a particular seating zone for a particular performance.
    /// </summary>
    [Serializable]
    public class PriceType
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public bool IsDefault { get; private set; }
        public bool IsPromo { get; private set; }

        public PriceType(int id, string name, double price, bool isDefault, bool isPromo)
        {
            Id = id;
            Name = name;
            Price = price;
            IsDefault = isDefault;
            IsPromo = isPromo;
        }
    }
}
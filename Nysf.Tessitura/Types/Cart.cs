using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura.Types
{
    public class Cart
    {
        public int Id { get; private set; }
        public SeatGroup[] SeatGroups { get; private set; }

        public double SubTotal
        {
            get
            {
                throw new NotImplementedException(); // TODO
            }
        }

        public double Fees
        {
            get
            {
                throw new NotImplementedException(); // TODO
            }
        }

        public double Total
        {
            get
            {
                return SubTotal + Fees;
            }
        }
    }
}
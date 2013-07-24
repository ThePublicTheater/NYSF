using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class CartSeat
    {
        public string RowIdentifier { get; set; }
        public int SeatNumInRow { get; set; }
        public List<CartFee> Fees { get; set; }
        public int Id { get; set; }
    }
}
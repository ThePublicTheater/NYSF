using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class PriceTypeSeatsPair
    {
        public int PriceTypeId { get; set; }
        public int SeatCount { get; set; }

        public PriceTypeSeatsPair(int priceTypeId, int seatCount)
        {
            PriceTypeId = priceTypeId;
            SeatCount = seatCount;
        }
    }
}
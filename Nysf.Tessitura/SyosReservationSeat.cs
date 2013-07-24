using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class SyosReservationSeat
    {
        public int Id { get; set; }
        public int PriceTypeId { get; set; }

        public SyosReservationSeat(int id, int priceTypeId)
        {
            Id = id;
            PriceTypeId = priceTypeId;
        }
    }
}
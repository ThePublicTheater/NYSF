using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class ReservationSeatingZone
    {
        public int SectionId { get; set; }
        public List<PriceTypeSeatsPair> PriceTypesSeats { get; set; }

        public int NumOfSeats
        {
            get
            {
                int seatCount = 0;
                foreach (PriceTypeSeatsPair seatsPrice in PriceTypesSeats)
                {
                    seatCount += seatsPrice.SeatCount;
                }
                return seatCount;
            }
        }

        public ReservationSeatingZone(int sectionId, List<PriceTypeSeatsPair> priceTypeSeatsPair)
        {
            SectionId = sectionId;
            PriceTypesSeats = priceTypeSeatsPair;
        }

        public ReservationSeatingZone(int sectionId, int priceTypeId, int numOfSeats)
        {
            SectionId = sectionId;
            PriceTypesSeats = new List<PriceTypeSeatsPair>();
            PriceTypesSeats.Add(new PriceTypeSeatsPair(priceTypeId, numOfSeats));
        }
    }
}
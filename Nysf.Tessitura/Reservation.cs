using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class Reservation
    {
        public int PerfId { get; set; }
        public List<ReservationSeatingZone> Sections { get; set; }

        public Reservation(int perfId)
        {
            PerfId = perfId;
            Sections = new List<ReservationSeatingZone>();
        }

        public void AddPriceTypeSeats(int sectionId, int priceTypeId, int numOfSeats)
        {
            int sectionIndex = -1;
            for (int c = 0; c < Sections.Count; c++)
            {
                if (Sections[c].SectionId == sectionId)
                {
                    sectionIndex = c;
                    break;
                }
            }
            if (sectionIndex == -1)
            {
                Sections.Add(new ReservationSeatingZone(sectionId, priceTypeId, numOfSeats));
            }
            else
            {
                Sections[sectionIndex].PriceTypesSeats.Add(
                    new PriceTypeSeatsPair(priceTypeId, numOfSeats));
            }
        }

        public void RemoveSections(List<ReservationSeatingZone> sectionsToRemove)
        {
            foreach (ReservationSeatingZone section in sectionsToRemove)
            {
                for (int c = Sections.Count - 1; c >= 0; c--)
                {
                    if (Sections[c].SectionId == section.SectionId)
                        Sections.RemoveAt(c);
                }
            }
        }
    }
}
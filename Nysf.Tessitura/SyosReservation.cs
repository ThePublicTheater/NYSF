using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class SyosReservation
    {
        public int PerfId { get; set; }
        public List<SyosReservationSection> Sections { get; set; }

        public int SeatCount
        {
            get
            {
                int seatCount = 0;
                foreach (SyosReservationSection section in Sections)
                {
                    foreach (SyosReservationZone zone in section.Zones)
                    {
                        seatCount += zone.Seats.Count;
                    }
                }
                return seatCount;
            }
        }

        public SyosReservation(int perfId)
        {
            PerfId = perfId;
            Sections = new List<SyosReservationSection>();
        }

        public void AddSeat(int sectionId, int zoneId, int priceTypeId, int seatId)
        {
            int sectionIndex = -1;
            for (int c = 0; c < Sections.Count; c++)
            {
                if (Sections[c].Id == sectionId)
                {
                    sectionIndex = c;
                    break;
                }
            }
            if (sectionIndex == -1)
            {
                SyosReservationSeat newSeat = new SyosReservationSeat(seatId, priceTypeId);
                SyosReservationZone newZone = new SyosReservationZone(zoneId);
                newZone.Seats.Add(newSeat);
                SyosReservationSection newSection = new SyosReservationSection(sectionId);
                newSection.Zones.Add(newZone);
                Sections.Add(newSection);
            }
            else
            {
                int zoneIndex = -1;
                for (int c = 0; c < Sections[sectionIndex].Zones.Count; c++)
                {
                    if (Sections[sectionIndex].Zones[c].Id == zoneId)
                    {
                        zoneIndex = c;
                        break;
                    }
                }
                if (zoneIndex == -1)
                {
                    SyosReservationSeat newSeat = new SyosReservationSeat(seatId, priceTypeId);
                    SyosReservationZone newZone = new SyosReservationZone(zoneId);
                    newZone.Seats.Add(newSeat);
                    Sections[sectionIndex].Zones.Add(newZone);
                }
                else
                {
                    SyosReservationSeat newSeat = new SyosReservationSeat(seatId, priceTypeId);
                    Sections[sectionIndex].Zones[zoneIndex].Seats.Add(newSeat);
                }
            }
        }

    }
}
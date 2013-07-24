using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class SyosSeatSection
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public SyosSeatZone[] Zones { get; private set; }

        public int SeatCount
        {
            get
            {
                int count = 0;
                foreach (SyosSeatZone zone in Zones)
                {
                    count += zone.SeatCount;
                }
                return count;
            }
        }

        public SyosSeatSection(int id, string name, SyosSeatZone[] zones)
        {
            Id = id;
            Name = name;
            Zones = zones;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class SyosSeatCollection
    {
        public SyosSeatSection[] Sections { get; private set; }
        public int ZoneMapId { get; private set; }

        public List<double> PriceScale
        {
            get
            {
                List<double> prices = new List<double>();
                foreach (SyosSeatSection section in Sections)
                {
                    foreach (SyosSeatZone zone in section.Zones)
                    {
                        foreach (PriceType priceType in zone.PriceTypes)
                        {
                            if (!prices.Contains(priceType.Price))
                            {
                                prices.Add(priceType.Price);
                            }
                        }
                    }
                }
                if (prices.Count > 10)
                    throw new ApplicationException("This collection of seats contains more than 10 "
                        + "different prices, but a price scale greater than 10 is not "
                        + "supported.");
                prices.Sort();
                if (prices.Count < 10)
                {
                    int extraSpaces = 10 - prices.Count;
                    float indexSkipValue = 10 / (((float)extraSpaces) + 1);
                    for (int c = 1; c <= extraSpaces; c++)
                    {
                        prices.Insert((int)(c * indexSkipValue), -1);
                    }
                }
                return prices;
            }
        }

        public int SeatCount
        {
            get
            {
                int count = 0;
                foreach (SyosSeatSection section in Sections)
                {
                    count += section.SeatCount;
                }
                return count;
            }
        }

        public SyosSeatCollection(SyosSeatSection[] sections, int zoneMapId)
        {
            Sections = sections;
            ZoneMapId = zoneMapId;
        }

    }
}
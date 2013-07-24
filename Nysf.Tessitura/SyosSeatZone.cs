using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    [Serializable]
    public class SyosSeatZone
    {
        public int Id { get; private set; }
        public string Description { get; private set; }
        public PriceType[] PriceTypes { get; private set; }
        public SyosSeatGroup[] SeatGroups { get; private set; }

        public int SeatCount
        {
            get
            {
                int count = 0;
                foreach (SyosSeatGroup group in SeatGroups)
                {
                    count += group.Seats.Length;
                }
                return count;
            }
        }

        public PriceType BestPriceType
        {
            get
            {
                int bestPriceTypeIndex = -1;
                double bestPriceFound = double.MaxValue;
                for (int c = 0; c < PriceTypes.Length; c++)
                {
                    if (PriceTypes[c].Price < bestPriceFound)
                    {
                        bestPriceTypeIndex = c;
                        bestPriceFound = PriceTypes[c].Price;
                    }
                }
                return PriceTypes[bestPriceTypeIndex];
            }
        }

        public SyosSeatZone(int id, string description, PriceType[] priceTypes,
            SyosSeatGroup[] seatGroups)
        {
            Id = id;
            Description = description;
            PriceTypes = priceTypes;
            SeatGroups = seatGroups;
        }
    }
}
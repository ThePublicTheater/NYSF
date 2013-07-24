using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    /// <summary>
    ///     A seating zone with available seats and price types for a particular performance.
    /// </summary>
    [Serializable]
    public class SeatingZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AvailableSeatCount { get; set; }
        public PriceType[] PriceTypes { get; set; }

        public SeatingZone(int id, string name, int seatCount, PriceType[] prices)
        {
            Id = id;
            Name = name;
            AvailableSeatCount = seatCount;
            PriceTypes = prices;
        }

        /// <summary>
        ///     Returns for each zone either the promo price or the default price.
        /// </summary>
        /// <param name="zones">
        ///     An array of SeatingZones representing all zones and prices available to a particular
        ///     constituent.
        /// </param>
        public static SeatingZone[] GetIdealPricesPerZone(SeatingZone[] zones)
        {
            List<SeatingZone> tempZones = new List<SeatingZone>();
            foreach (SeatingZone zone in zones)
            {
                PriceType[] priceTypes = zone.PriceTypes;
                PriceType idealPriceType = null;
                foreach (PriceType priceType in priceTypes)
                {
                    if (priceType.IsPromo)
                    {
                        idealPriceType = priceType;
                        break;
                    }
                    else if (priceType.IsDefault)
                    {
                        idealPriceType = priceType;
                    }
                }
                //PriceType[] tempPriceTypes = new PriceType[1];
                //tempPriceTypes[0] = idealPriceType;
                SeatingZone tempZone = new SeatingZone(zone.Id, zone.Name, zone.AvailableSeatCount,
                    new PriceType[] {idealPriceType});
                tempZones.Add(tempZone);
            }
            return tempZones.ToArray();
        }

        /// <summary>
        ///     Returns the lowest prices available for each zone.
        /// </summary>
        /// <param name="zones">
        ///     An array of SeatingZones representing all zones and prices available to a particular
        ///     constituent.
        /// </param>
        public static SeatingZone[] GetLowestPricesPerZone(SeatingZone[] zones)
        {
            List<SeatingZone> tempZones = new List<SeatingZone>();
            foreach (SeatingZone zone in zones)
            {
                int lowestPriceIndex = -1;
                double lowestPrice = Int32.MaxValue;
                PriceType[] priceTypes = zone.PriceTypes;
                for (int i = 0; i < priceTypes.Length; i++)
                {
                    if (priceTypes[i].Price < lowestPrice
                        || (priceTypes[i].Price == lowestPrice && priceTypes[i].IsDefault))
                    {
                        lowestPriceIndex = i;
                        lowestPrice = priceTypes[i].Price;
                    }
                }
                PriceType[] tempPriceTypes = new PriceType[1];
                tempPriceTypes[0] = priceTypes[lowestPriceIndex];
                SeatingZone tempZone = new SeatingZone(zone.Id, zone.Name, zone.AvailableSeatCount,
                    tempPriceTypes);
                tempZones.Add(tempZone);
            }
            return tempZones.ToArray();
        }
    }
}
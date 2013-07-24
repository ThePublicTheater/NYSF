using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class CartSeatGroupItem
    {
        public int Id { get; set; }
        public Performance Performance { get; set; }
        public int SeatingZoneId { get; set; }
        public string SeatingZoneName { get; set; }
        public List<CartPriceTypeSeats> SeatsPerPriceTypes { get; set; }

        public int SeatCount
        {
            get
            {
                int count = 0;
                foreach (CartPriceTypeSeats seats in SeatsPerPriceTypes)
                {
                    count += seats.SeatCount;
                }
                return count;
            }
        }

        public double SubTotal
        {
            get
            {
                double subTotal = 0;
                foreach (CartPriceTypeSeats priceTypeGroup in SeatsPerPriceTypes)
                {
                    subTotal += priceTypeGroup.SubTotal;
                }
                return subTotal;
            }
        }

        public double FeeSum
        {
            get
            {
                double feeSum = 0;
                foreach (CartPriceTypeSeats priceTypeGroup in SeatsPerPriceTypes)
                {
                    feeSum += priceTypeGroup.FeeSum;
                }
                return feeSum;
            }
        }

        public CartFee[] FeeSumPerType
        {
            get
            {
                List<CartFee> feeSumPerTypes = new List<CartFee>();
                foreach (CartPriceTypeSeats priceType in SeatsPerPriceTypes)
                {
                    foreach (CartFee fee in priceType.FeeSumPerType)
                    {
                        bool found = false;
                        for (int c = 0; c < feeSumPerTypes.Count; c++)
                        {
                            if (feeSumPerTypes[c].Description == fee.Description)
                            {
                                feeSumPerTypes[c].Amount += fee.Amount;
                                found = true;
                                break;
                            }
                        }
                        if (!found)
                        {
                            feeSumPerTypes.Add(new CartFee {
                                Amount = fee.Amount,
                                Description = fee.Description });
                        }
                    }
                }
                return feeSumPerTypes.ToArray();
            }
        }

        public double Total
        {
            get
            {
                double total = 0;
                foreach (CartPriceTypeSeats seats in SeatsPerPriceTypes)
                {
                    total += seats.Total;
                }
                return total;
            }
        }
    }
}
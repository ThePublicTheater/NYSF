using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class CartPriceTypeSeats
    {
        public int PriceTypeId { get; set; }
        public string PriceTypeName { get; set; }
        public bool PriceTypeIsDefault { get; set; }
        public double PricePerSeat { get; set; }
        public int SeatCount { get; set; }
        public List<CartSeat> Seats { get; set; }

        public double SubTotal
        {
            get
            {
                return SeatCount * PricePerSeat;
            }
        }

        public double FeeSum
        {
            get
            {
                double sum = 0;
                foreach (CartSeat seat in Seats)
                {
                    foreach (CartFee fee in seat.Fees)
                    {
                        sum += fee.Amount;
                    }
                }
                return sum;
            }
        }

        public CartFee[] FeeSumPerType
        {
            get
            {
                List<CartFee> feeSums = new List<CartFee>();
                foreach (CartSeat seat in Seats)
                {
                    foreach (CartFee fee in seat.Fees)
                    {
                        int foundIndex = -1;
                        for (int c = 0; c < feeSums.Count; c++)
                        {
                            if (feeSums[c].Description == fee.Description)
                            {
                                foundIndex = c;
                                break;
                            }
                        }
                        if (foundIndex == -1)
                        {
                            CartFee newFee = new CartFee
                            {
                                Amount = fee.Amount,
                                Description = fee.Description
                            };
                            feeSums.Add(newFee);
                        }
                        else
                        {
                            feeSums[foundIndex].Amount += fee.Amount;
                        }
                    }
                }
                return feeSums.ToArray();
            }
        }

        public double Total
        {
            get
            {
                return SubTotal + FeeSum;
            }
        }

        public void Absorb(CartPriceTypeSeats addition)
        {
            SeatCount += addition.SeatCount;
            foreach (CartSeat newSeat in addition.Seats)
            {
                Seats.Add(newSeat);
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Tessitura
{
    public class Cart
    {
        public int Id { get; set; }

        public List<CartSeatGroupItem> SeatGroups { get; set; }

        public CartFee[] FeeSumPerType
        {
            get
            {
                List<CartFee> feeSumPerTypes = new List<CartFee>();
                foreach (CartSeatGroupItem seatGroup in SeatGroups)
                {
                    foreach (CartFee fee in seatGroup.FeeSumPerType)
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
                            feeSumPerTypes.Add(new CartFee
                            {
                                Amount = fee.Amount,
                                Description = fee.Description
                            });
                        }
                    }
                }
                return feeSumPerTypes.ToArray();
            }
        }

        public bool HasItems
        {
            get
            {
                return SeatGroups.Count > 0;
            }
        }

        public double SubTotal
        {
            get
            {
                double subTotal = 0;
                foreach (CartSeatGroupItem seatGroup in SeatGroups)
                {
                    subTotal += seatGroup.SubTotal;
                }
                return subTotal;
                
            }
        }

        public double FeeSum
        {
            get
            {
                double feeSum = 0;
                foreach (CartSeatGroupItem seatGroup in SeatGroups)
                {
                    feeSum += seatGroup.FeeSum;
                }
                return feeSum;
            }
        }

        public double Total
        {
            get
            {
                double total = 0;
                foreach (CartSeatGroupItem seatGroup in SeatGroups)
                {
                    total += seatGroup.Total;
                }
                return total;
            }
        }

        public static Cart BuildSummaryCart(Reservation reservation, SeatingZone[] seatingInfo)
        {
            Cart cart = new Cart();
            List<CartSeatGroupItem> seatGroups = cart.SeatGroups = new List<CartSeatGroupItem>();

            foreach (ReservationSeatingZone seatingZone in reservation.Sections)
            {
                CartSeatGroupItem newItem = new CartSeatGroupItem();
                newItem.Performance = WebClient.GetPerformance(reservation.PerfId);
                string description = null;
                foreach (SeatingZone zone in seatingInfo)
                {
                    if (zone.Id == seatingZone.SectionId)
                    {
                        description = zone.Name;
                        break;
                    }
                }
                newItem.SeatingZoneName = description;
                newItem.SeatingZoneId = seatingZone.SectionId;
                newItem.SeatsPerPriceTypes = new List<CartPriceTypeSeats>();
                foreach (PriceTypeSeatsPair seatsPerPriceType in seatingZone.PriceTypesSeats)
                {
                    double pricePerSeat = 0;
                    string priceTypeName = null;
                    bool found = false;
                    foreach (SeatingZone zone in seatingInfo)
                    {
                        if (zone.Id == seatingZone.SectionId)
                        {
                            foreach (PriceType priceType in zone.PriceTypes)
                            {
                                if (priceType.Id == seatsPerPriceType.PriceTypeId)
                                {
                                    pricePerSeat = priceType.Price;
                                    priceTypeName = priceType.Name;
                                    found = true;
                                    break;
                                }
                            }
                            if (found)
                                break;
                        }
                    }
                    newItem.SeatsPerPriceTypes.Add(
                        new CartPriceTypeSeats
                        {
                            SeatCount = seatsPerPriceType.SeatCount,
                            PricePerSeat = pricePerSeat,
                            PriceTypeName = priceTypeName
                        });
                }
                seatGroups.Add(newItem);
            }
            return cart;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class SyosSeatsPerPriceTypeCollection : IEnumerable<SyosSeatsPerPriceType>
	{
		List<SyosSeatsPerPriceType> priceTypeSeats;

		public SyosSeatsPerPriceType this[int i]
		{
			get
			{
				return priceTypeSeats[i];
			}
		}

		public int TotalSeats
		{
			get
			{
				int total = 0;
				foreach (SyosSeatsPerPriceType seatsPrice in priceTypeSeats)
				{
					total += seatsPrice.Count;
				}
				return total;
			}
		}

		public int PriceTypeCount
		{
			get
			{
				return priceTypeSeats.Count;
			}
		}

		public SyosSeatsPerPriceTypeCollection()
		{
			priceTypeSeats = new List<SyosSeatsPerPriceType>();
		}

		public SyosSeatsPerPriceTypeCollection(int priceTypeId, params int[] seatIds)
		{
			priceTypeSeats = new List<SyosSeatsPerPriceType>();
			Add(priceTypeId, seatIds);
		}

		public SyosSeatsPerPriceTypeCollection(params SyosSeatsPerPriceType[] spts)
		{
			priceTypeSeats = spts.ToList<SyosSeatsPerPriceType>();
		}

		public void Add(int priceTypeId, params int[] seatIds)
		{
			SyosSeatsPerPriceType spt = GetByPriceTypeId(priceTypeId);
			if (spt == null)
			{
				spt = new SyosSeatsPerPriceType(priceTypeId, seatIds);
				priceTypeSeats.Add(spt);
			}
			else
			{
				spt.Add(seatIds);
			}
		}

		public void Add(SyosSeatsPerPriceType spt)
		{
			SyosSeatsPerPriceType targetSpt = GetByPriceTypeId(spt.PriceTypeId);
			if (targetSpt == null)
			{
				priceTypeSeats.Add(spt);
			}
			else
			{
				targetSpt.Absorb(spt);
			}
		}

		public SyosSeatsPerPriceType GetByPriceTypeId(int id)
		{
			return (from p in priceTypeSeats
					where p.PriceTypeId == id
					select p).FirstOrDefault<SyosSeatsPerPriceType>();
		}

		public IEnumerator<SyosSeatsPerPriceType> GetEnumerator()
		{
			return priceTypeSeats.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

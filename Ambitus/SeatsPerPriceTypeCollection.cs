using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class SeatsPerPriceTypeCollection
			: Internals.ApiResultInterpreter, IEnumerable<SeatsPerPriceType>
	{
		List<SeatsPerPriceType> priceTypeSeats;

		public SeatsPerPriceType this[int i]
		{
			get
			{
				return priceTypeSeats[i];
			}
		}

		public byte TotalSeats
		{
			get
			{
				byte total = 0;
				foreach (SeatsPerPriceType seatPrice in priceTypeSeats)
				{
					total += seatPrice.NumOfSeats;
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

		public SeatsPerPriceTypeCollection()
		{
			priceTypeSeats = new List<SeatsPerPriceType>();
		}

		public SeatsPerPriceTypeCollection(int priceTypeId, byte numOfSeats)
		{
			priceTypeSeats = new List<SeatsPerPriceType> {
				new SeatsPerPriceType(priceTypeId, numOfSeats) };
		}

		public SeatsPerPriceTypeCollection(SeatsPerPriceType[] seatsPerPriceType)
		{
			priceTypeSeats = new List<SeatsPerPriceType>();
			foreach (SeatsPerPriceType seatPriceReq in seatsPerPriceType)
			{
				SeatsPerPriceType newSeatPrice = GetByPriceType(seatPriceReq.PriceTypeId);
				if (newSeatPrice == null)
				{
					newSeatPrice = seatPriceReq;
					priceTypeSeats.Add(newSeatPrice);
				}
				else
				{
					newSeatPrice.NumOfSeats += seatPriceReq.NumOfSeats;
				}
			}
		}

		public void Add(int priceTypeId, byte numOfSeats)
		{
			SeatsPerPriceType targetSeatsPrice = GetByPriceType(priceTypeId);
			if (targetSeatsPrice == null)
			{
				targetSeatsPrice = new SeatsPerPriceType(priceTypeId, numOfSeats);
				priceTypeSeats.Add(targetSeatsPrice);
			}
			else
			{
				targetSeatsPrice.NumOfSeats += numOfSeats;
			}
		}

		public void Remove(int priceTypeId, byte numOfSeats)
		{
			SeatsPerPriceType targetSeatsPrice = GetByPriceType(priceTypeId);
			if (targetSeatsPrice == null)
			{
				return;
			}
			targetSeatsPrice.NumOfSeats -= numOfSeats;
			if (targetSeatsPrice.NumOfSeats <= 0)
			{
				priceTypeSeats.Remove(targetSeatsPrice);
			}
		}

		public void Remove(int priceTypeId)
		{
			SeatsPerPriceType targetSeatsPrice = GetByPriceType(priceTypeId);
			if (targetSeatsPrice == null)
			{
				return;
			}
			priceTypeSeats.Remove(targetSeatsPrice);
		}

		public SeatsPerPriceType GetByPriceType(int id)
		{
			return (from p in priceTypeSeats
					where p.PriceTypeId == id
					select p).FirstOrDefault<SeatsPerPriceType>();
		}

		public IEnumerator<SeatsPerPriceType> GetEnumerator()
		{
			return priceTypeSeats.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

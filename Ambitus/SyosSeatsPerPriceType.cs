using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Ambitus
{
	public class SyosSeatsPerPriceType : IEnumerable<int>
	{
		private List<int> m_seatIds;
		public int PriceTypeId { get; set; }

		public int this[int i]
		{
			get
			{
				return m_seatIds[i];
			}
		}

		public int Count
		{
			get
			{
				return m_seatIds.Count;
			}
		}

		public SyosSeatsPerPriceType(int priceTypeId, params int[] seatIds)
		{
			PriceTypeId = priceTypeId;
			m_seatIds = seatIds.ToList<int>();
		}

		public void Add(params int[] seatIds)
		{
			foreach (int id in seatIds)
			{
				if (!m_seatIds.Contains(id))
				{
					m_seatIds.Add(id);
				}
			}
		}

		public void Remove(params int[] seatIds)
		{
			foreach (int id in seatIds)
			{
				m_seatIds.Remove(id);
			}
		}

		public void Absorb(SyosSeatsPerPriceType spt)
		{
			int[] seatIdsToAdd = new int[spt.Count];
			for (int c = 0; c < seatIdsToAdd.Length; c++)
			{
				seatIdsToAdd[c] = spt[c];
			}
			Add(seatIdsToAdd);
		}

		public IEnumerator<int> GetEnumerator()
		{
			return m_seatIds.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

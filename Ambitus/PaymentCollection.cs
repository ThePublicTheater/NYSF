using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class PaymentCollection : IEnumerable<Payment>
	{
		List<Payment> payments;

		public Payment this[int i]
		{
			get
			{
				return payments[i];
			}
		}

		public int Count
		{
			get
			{
				return payments.Count;
			}
		}

		public PaymentCollection(DataTable tessResults)
		{
			payments = (from row in tessResults.AsEnumerable()
						  select new Payment(
							  id: row.Field<int?>("payment_no"),
							  amount: row.Field<decimal?>("pmt_amt"),
							  giftCertificateId: row.Field<string>("gc_no"),
							  description: row.Field<string>("description")))
						.ToList<Payment>();
		}

		public Payment GetById(int id)
		{
			return (from p in payments
					where p.Id == id
					select p).FirstOrDefault<Payment>();
		}

		public IEnumerator<Payment> GetEnumerator()
		{
			return payments.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
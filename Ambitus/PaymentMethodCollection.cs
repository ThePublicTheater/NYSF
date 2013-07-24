using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Ambitus
{
	public class PaymentMethodCollection
			: Internals.ApiResultInterpreter, IEnumerable<PaymentMethod>
	{
		List<PaymentMethod> methods;

		public PaymentMethod this[int i]
		{
			get
			{
				return methods[i];
			}
		}

		public int Count
		{
			get
			{
				return methods.Count;
			}
		}

		public PaymentMethodCollection(DataTable tessResults /* "PaymentMethod" */)
		{
			methods = (from row in tessResults.AsEnumerable()
						  select new PaymentMethod(
							   id: row.Field<int?>("id"),
							   name: row.Field<string>("description"),
							   auth: ToBool(row.Field<string>("auth_ind")),
							   paymentTypeId: row.Field<short?>("pmt_type"),
							   paymentTypeName: row.Field<string>("pmt_type_desc"),
							   accountType: row.Field<string>("account_type"),
							   cardPrefix: row.Field<string>("card_prefix"),
							   addressVerification:
									ToBool(row.Field<string>("address_verification")),
							   merchantId: row.Field<string>("merchant_id"),
							   useCv2: ToBool(row.Field<string>("use_cv2")),
							   cardLength: row.Field<string>("card_length"),
							   mod10: ToBool(row.Field<string>("mod10_ind"))))
						.ToList<PaymentMethod>();
		}

		public PaymentMethod GetById(int id)
		{
			return (from p in methods
					where p.Id == id
					select p).FirstOrDefault<PaymentMethod>();
		}

		public IEnumerator<PaymentMethod> GetEnumerator()
		{
			return methods.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}

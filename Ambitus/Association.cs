using System;

namespace Ambitus
{
	public class Association
	{
		public int? Id { get; private set; }
		public int? BackwardAssocId { get; private set; }
		public int? AssociateId { get; private set; }
		public string Category { get; private set; }
		public int? RelationTypeId { get; private set; }
		public string RelationTypeName { get; private set; }
		public string AssociateName { get; private set; }
		public string AssociateGender { get; private set; }
		public string AssociateTitle { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public decimal? Salary { get; private set; }
		public string Notes { get; private set; }
		public int? OriginN1N2 { get; private set; }
		public int? AssociateN1N2 { get; private set; }
		public bool? Active { get; private set; }

		public Association(int? _xref_no, int? _xref_no_inv, int? _associate_no, string _category,
				int? _xref_type, string _xref_type_desc, string _name, string _gender,
				string _title, DateTime? _start_dt, DateTime? _end_dt, decimal? _salary,
				string _notes, int? _n1n2_ind, int? _n1n2_ind_assoc, bool? active)
		{
			Id = _xref_no;
			BackwardAssocId = _xref_no_inv;
			AssociateId = _associate_no;
			Category = _category;
			RelationTypeId = _xref_type;
			RelationTypeName = _xref_type_desc;
			AssociateName = _name;
			AssociateGender = _gender;
			AssociateTitle = _title;
			StartDate = _start_dt;
			EndDate = _end_dt;
			Salary = _salary;
			Notes = _notes;
			OriginN1N2 = _n1n2_ind;
			AssociateN1N2 = _n1n2_ind_assoc;
			Active = active;
		}
	}
}

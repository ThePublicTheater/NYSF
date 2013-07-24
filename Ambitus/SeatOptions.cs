using System.Data;

namespace Ambitus
{
	public class SeatOptions
	{
		public SeatCollection Seats { get; private set; }
		public SectionCollection Sections { get; private set; }
		public SeatTypeCollection SeatTypes { get; private set; }
		public AllocationCollection Allocations { get; private set; }

		public SeatOptions(DataTable seatsTable, DataTable sectionsTable, DataTable seatTypesTable,
				DataTable allocTable)
		{
			Seats = new SeatCollection(seatsTable);
			Sections = new SectionCollection(sectionsTable);
			SeatTypes = new SeatTypeCollection(seatTypesTable);
			Allocations = new AllocationCollection(allocTable);
		}
	}
}
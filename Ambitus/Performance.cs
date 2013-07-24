using System;

namespace Ambitus
{
	public class Performance
	{
		public int? PackageId { get; private set; }
		public int? Id { get; private set; }
		public string PackageCode { get; private set; }
		public string Code { get; private set; }
		public DateTime? Date { get; private set; }
		public int? GrossAvailability { get; private set; }
		public int? AvailabilityByCustomer { get; private set; }
		public int? VenueId { get; private set; }
		public bool? MetCriteria { get; private set; }
		public int? TimeSlotId { get; private set; }
		public string Name { get; private set; }
		public bool? OnSale { get; private set; }
		public int? BusinessUnit { get; private set; }
		public int? ProdSeasonId { get; private set; }
		public bool? NoName { get; private set; }
		public int? ZmapId { get; private set; }
		public DateTime? StartDate { get; private set; }
		public DateTime? EndDate { get; private set; }
		public DateTime? FirstDate { get; private set; }
		public DateTime? LastDate { get; private set; }
		public string VenueName { get; private set; }
		public int? Weight { get; private set; }
		public bool? SuperPackage { get; private set; }
		public bool? FixedSeat { get; private set; }
		public bool? Flex { get; private set; }
		public int? ProdTypeId { get; private set; }
		public string ProdTypeName { get; private set; }
		public short? SeasonId { get; private set; }
		public string SeasonName { get; private set; }
		public int? StatusId { get; private set; }
		public string StatusName { get; private set; }
		public int? Relevance { get; private set; }
		public short? PremiereId { get; private set; }
		public string PremiereName { get; private set; }
		public string TimeSlotName { get; private set; }
		/*public int? OriginalInventoryId { get; private set; }
		public int InventoryId { get; private set; }
		public string InventoryType { get; private set; }
		public int? ContentTypeId { get; private set; }
		public string ContentTypeName { get; private set; }
		public string ContentValue { get; private set; }*/

		public Performance(int? packageId, int? id, string packageCode, string code, DateTime? date,
				int? grossAvailability, int? availabilityByCustomer, int? venueId,
				bool? metCriteria, int? timeSlotId, string name, bool? onSale,
				int? businessUnit, int? prodSeasonId, bool? noName, int? zmapId,
				DateTime? startDate, DateTime? endDate, DateTime? firstDate, DateTime? lastDate,
				string venueName, int? weight, bool? superPackage, bool? fixedSeat,
				bool? flex, int? prodTypeId, string prodTypeName, short? seasonId,
				string seasonName, int? statusId, string statusName, int? relevance,
				short? premiereId, string premiereName, string timeSlotName/*,
				int? originalInventoryId, int inventoryId, string inventoryType, int? contentTypeId,
				string contentTypeName, string contentValue*/)
		{
			PackageId = packageId;
			Id = id;
			PackageCode = packageCode;
			Code = code;
			Date = date;
			GrossAvailability = grossAvailability;
			AvailabilityByCustomer = availabilityByCustomer;
			VenueId = venueId;
			MetCriteria = metCriteria;
			TimeSlotId = timeSlotId;
			Name = name;
			OnSale = onSale;
			BusinessUnit = businessUnit;
			ProdSeasonId = prodSeasonId;
			NoName = noName;
			ZmapId = zmapId;
			StartDate = startDate;
			EndDate = endDate;
			FirstDate = firstDate;
			LastDate = lastDate;
			VenueName = venueName;
			Weight = weight;
			SuperPackage = superPackage;
			FixedSeat = fixedSeat;
			Flex = flex;
			ProdTypeId = prodTypeId;
			ProdTypeName = prodTypeName;
			SeasonId = seasonId;
			SeasonName = seasonName;
			StatusId = statusId;
			StatusName = statusName;
			Relevance = relevance;
			PremiereId = premiereId;
			PremiereName = premiereName;
			TimeSlotName = timeSlotName;
			/*OriginalInventoryId = originalInventoryId;
			InventoryId = inventoryId;
			InventoryType = inventoryType;
			ContentTypeId = contentTypeId;
			ContentTypeName = contentTypeName;
			ContentValue = contentValue;*/
		}

		public override string ToString()
		{
			return Name ?? String.Empty;
		}
	}
}
namespace Ambitus
{
	public class AddressUpdateParams
	{
		public DeletableParam<int[]> PurposeIds { get; private set; }
		public AddressUpdateClearingParams ClearableFields { get; private set; }
		public AddressUpdateNonclearingParams NonclearableFields { get; private set; }

		public AddressUpdateParams()
		{
			PurposeIds = new DeletableParam<int[]>(new int[] { });
			ClearableFields = new AddressUpdateClearingParams();
			NonclearableFields = new AddressUpdateNonclearingParams();
		}
	}
}

namespace Ambitus
{
	public enum CheckoutResult : byte
	{
		Unprocessed,
		Succeeded,
		Invalid,
		TypeMismatch,
		TimedOut,
		NotAuthorized,
		Declined
	}
}

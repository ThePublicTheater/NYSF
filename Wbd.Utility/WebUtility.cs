using System.Web;

namespace Wbd.Utilities
{
	public static class WebUtility
	{
		public static bool SessionJustExpired()
		{
			HttpContext context = HttpContext.Current;
			if (context.Session != null
				&& context.Session.IsNewSession)
			{
				string cookieHeader = context.Request.Headers["Cookie"];
				if (cookieHeader != null
					&& cookieHeader.Contains("ASP.NET_SessionId"))
				{
					context.Session.Clear();
					return true;
				}
			}
			return false;
		}
	}
}

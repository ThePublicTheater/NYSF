using Ambitus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Wbd.Utilities;

namespace Nysf.Web
{
	public static class BrowserUtility
	{
		private const string LastPageSessionPostfix = "_LastPage";

		private static HttpContext Context
		{
			get
			{
				return HttpContext.Current;
			}
		}

		private static ConfigSection Settings
		{
			get
			{
				return ConfigSection.Settings;
			}
		}

		private static StandardPagesElement StandardPages
		{
			get
			{
				return Settings.StandardPages;
			}
		}

		public static NysfSession GetSession()
		{
			NysfSession session = (NysfSession)HttpContext.Current.Session[
					Settings.BrowserSessionKey];
			return session;
		}

		public static string GetLastPage()
		{
			return (string)Context.Session[Settings.BrowserSessionKey + LastPageSessionPostfix];
		}

		public static void SetLastPage(string url)
		{
			Context.Session.Add(Settings.BrowserSessionKey + LastPageSessionPostfix, url);
		}

		public static void MaintainSession(bool allowAnonymous, bool allowAuthenticated,
					bool allowTemporary, bool requireSsl, int? newSessionSourceId, bool setLastPage)
		{
			if (WebUtility.SessionJustExpired())
			{
				// TODO: preserve query-requested source
				Context.Response.Redirect(StandardPages.Timeout);
			}
			if (requireSsl)
			{
				if (!Context.Request.IsSecureConnection
						&& Settings.DebugEnableSsl)
				{
					string unsecureUrl = Context.Request.Url.ToString();
					int protocolIndex = unsecureUrl.IndexOf("http://");
					if (protocolIndex != 0)
					{
						throw new ApplicationException(
								"An unsupported protocol was detected. The URL must start with \"http://\".");
					}
					string secureUrl = unsecureUrl.Insert(protocolIndex + 4, "s");
					Context.Response.Redirect(secureUrl);
				}
			}
			else if (Context.Request.IsSecureConnection)
			{
				string secureUrl = Context.Request.Url.ToString();
				int protocolIndex = secureUrl.IndexOf("https://");
				if (protocolIndex != 0)
				{
					throw new ApplicationException(
							"An unsupported protocol was detected. The URL must start with \"https://\".");
				}
				string unsecureUrl = secureUrl.Remove(4, 1);
				Context.Response.Redirect(unsecureUrl);
			}
			if (setLastPage)
			{
				SetLastPage(Context.Request.Url.ToString());
			}
			int? requestedSourceId = null;
			if (!String.IsNullOrWhiteSpace(
					Context.Request.QueryString[Settings.SourceIdQueryStringKey]))
			{
				string sourceIdQueryValue =
						Context.Request.QueryString[Settings.SourceIdQueryStringKey];
				int convertedSourceId;
				if (Int32.TryParse(sourceIdQueryValue, out convertedSourceId))
				{
					requestedSourceId = convertedSourceId;
				}
			}
			NysfSession session = GetSession();
			if (session == null)
			{
				if (requestedSourceId.HasValue)
				{
					session =
						new NysfSession(Context.Request.UserHostAddress, requestedSourceId.Value);
				}
				else if (newSessionSourceId.HasValue)
				{
					session =
						new NysfSession(Context.Request.UserHostAddress, newSessionSourceId.Value);
				}
				else
				{
					session =
						new NysfSession(Context.Request.UserHostAddress);
				}
				Context.Session.Add(Settings.BrowserSessionKey, session);
				if (!allowAnonymous)
				{
					Context.Response.Redirect(StandardPages.Login);
				}
				return;
			}
			else
			{
				session.UpdateLastAccessTime();
				if (requestedSourceId.HasValue)
				{
					session.UpdateSessionSource(requestedSourceId.Value);
				}
			}
			if (session.CartExpireTime.HasValue && session.CartExpireTime.Value < DateTime.Now
					&& (!session.IsTemporary.HasValue || !session.IsTemporary.Value))
			{
				session.Revive();
				Context.Response.Redirect(StandardPages.Expired);
			}
			if (!allowAnonymous && session.IsAnonymous)
			{
				Context.Response.Redirect(StandardPages.Login);
			}
			if (!allowAuthenticated && !session.IsAnonymous)
			{
				RedirectToLastPage();
			}
			if (!allowTemporary && session.IsTemporary.Value)
			{
				Context.Response.Redirect(StandardPages.Activate);
			}
		}

		public static void RedirectToLastPage()
		{
			string url = GetLastPage();
			if (url == null)
			{
				Context.Response.Redirect("/");
			}
			else
			{
				Context.Response.Redirect(url);
			}
		}

		public static void LogoutAndClearSession()
		{
			NysfSession session = GetSession();
			if (session != null)
			{
				session.Logout();
			}
			Context.Session.Clear();
		}
	}
}
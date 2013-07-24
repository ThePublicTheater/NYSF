using Nysf.TicketScanning.WebReferences.TessWebApi;
using System;
using System.Data;
using System.Web;
using System.Web.SessionState;

namespace Nysf.TicketScanning
{
	public class LoginClient
	{
		private const string userSessKey = "nysf_TicketScanning_User";
		private const string checkDatesModeKey = "nysf_TicketScanning_CheckDatesMode";
		private const string facilitySessKey = "nysf_TicketScanning_Session";
		private const string justLoggedOutCookieKey = "nysf_TicketScanning_JustLoggedOut";

		private readonly static Tessitura tessClient;
		
		private readonly static string tessSessKey;

		private static HttpContext context
		{
			get { return HttpContext.Current; }
		}

		private static HttpSessionState session
		{
			get { return context.Session; }
		}

		static LoginClient()
		{
			tessClient = new Tessitura();
			tessSessKey = tessClient.GetNewSessionKeyEx("0.0.0.0", 1);
		}

		public static string GetUser()
		{
			return (session[userSessKey] == null) ? null
				: session[userSessKey].ToString();
		}

		public static int GetFacilityId()
		{
			return (int)session[facilitySessKey];
		}

		public static bool GetCheckDatesMode()
		{
			return (bool)session[checkDatesModeKey];
		}

		public static bool Login(string username, string password, int facilityId,
			bool checkDatesMode)
		{
			DataSet result = tessClient.ValidateTessituraUserEx(tessSessKey, username, password);
			if (result.Tables["UserInfo"].Rows[0]["userid"] != DBNull.Value)
			{
				session.Add(userSessKey, username);
				session.Add(facilitySessKey, facilityId);
				session.Add(checkDatesModeKey, checkDatesMode);
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void Logout()
		{
			session.Abandon();
			context.Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
		}

		public static void MaintainSession()
		{
			if (session != null
				&& session.IsNewSession
				&& context.Request.Cookies["ASP.NET_SessionId"] != null
				&& context.Request.Cookies["ASP.NET_SessionId"].Value != "")
			{
				session.Clear();
				context.Response.Redirect(context.Request.Url.AbsolutePath);
			}
		}
	}
}
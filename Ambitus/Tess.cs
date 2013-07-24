using Ambitus.Internals;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Ambitus
{
	/// <summary>
	///		A wrapper that simplifies calls to the Tessitura Web API
	/// </summary>
	/// <remarks>
	///		Purposes:
	///		<list type="table">
	///			<listheader>
	///				<term>
	///					Problem
	///				</term>
	///				<description>
	///					Solution
	///				</description>
	///			</listheader>
	///			<item>
	///				<term>
	///					API methods allow a variety of parameters to be supplied or omitted, in
	///					varying combinations. Value-type parameters that may be omitted require
	///					specific and varying omission values to be passed in their place. These
	///					combinations and omission values are poorly documented and difficult to
	///					remember.
	///				</term>
	///				<description>
	///					Provide overloaded methods for each accepted combination of parameters,
	///					and supply the expected omission values automatically, allowing
	///					IntelliSense to present acceptable parameter combinations.
	///				</description>
	///			</item>
	///			<item>
	///				<term>
	///					API method parameters and returned data are named inconsistenly and
	///					inaccurately, and are loosely typed.
	///				</term>
	///				<description>
	///					Standardize a set of names that describe the data accurately, and supply
	///					strongly typed objects as method results.
	///				</description>
	///			</item>
	///			<item>
	///				<term>
	///					API methods take parameters that are unlikely to change for a single
	///					application deployment. These values must be memorized and supplied
	///					manually, repetitively, to each method call.
	///				</term>
	///				<description>
	///					Allow users to set default constant values in a config file, and supply
	///					those constants automatically to API method calls when they are excluded
	///					from the parameter list.
	///				</description>
	///			</item>
	///			<item>
	///				<term>
	///					Certain API methods should operate only over a secure connection, while
	///					others are unlikely to contain sensitive information.
	///				</term>
	///				<description>
	///					Automatically employ a secure connection for transfers that may contain
	///					sensitive information, and otherwise use an unsecure connection to conserve
	///					resources.
	///				</description>
	///			</item>
	///		</list>
	/// </remarks>
	public static class Tess
	{
		#region Fields and Properties

		// Raw Tessitura Web API clients

		/// <summary>
		///		A raw Tessitura Web API client that communicates over a secure connection
		/// </summary>
		public static Internals.TessituraWebApi.Tessitura SecureTess { get; set; }

		/// <summary>
		///		A raw Tessitura Web API client that communicates over an unsecured connection
		/// </summary>
		public static Internals.TessituraWebApi.Tessitura UnsecureTess { get; set; }

		/// <summary>
		///		A convenient reference to the deployment-wide defaults set in the configuration
		/// </summary>
		private static DefaultsElement Defaults
		{
			get
			{
				return ConfigSection.Settings.Defaults;
			}
		}

		/// <summary>
		///		A string expected to be reserved for special purposes in the remote Web API's
		///		configuration
		/// </summary>
		public static string EmptyStringLiteral;

		#endregion

		#region Infrastructural Methods

		/// <summary>
		///		Prepares static fields
		/// </summary>
		static Tess()
		{
			PrepareWebApiClients();
			EmptyStringLiteral = Defaults.EmptyStringLiteral;
		}

		/// <summary>
		///		Prepares the raw clients
		/// </summary>
		/// <remarks>
		///		Intended to be called only from the static constructor
		/// </remarks>
		/// <exception cref="ApplicationException">
		///		Thrown when the configured URL to the web API doesn't start with either "http" or
		///		"https"
		/// </exception>
		private static void PrepareWebApiClients()
		{
			// Instantiate the default Web API client
			Internals.TessituraWebApi.Tessitura defaultClient =
					new Internals.TessituraWebApi.Tessitura();

			// Determine whether configuration allows an unsecured client
			bool allowUnsecure = ConfigSection.Settings.Connection.AllowUnsecure;

			// If the client is secure by default
			if (defaultClient.Url.StartsWith("https://"))
			{
				// Designate it as the SecureTess
				SecureTess = defaultClient;

				// And if an unsecured client is allowed
				if (allowUnsecure)
				{
					// Create a new default client and make its URL unsecure

					UnsecureTess = new Internals.TessituraWebApi.Tessitura();
					UnsecureTess.Url = UnsecureTess.Url.Remove(4, 1);
				}

				// If an unsecured client is not allowed
				else
				{
					// Have SecureTess and UnsecureTess share the same secure client object
					UnsecureTess = defaultClient;
				}
			}

			// If the client is unsecured by default
			else if (defaultClient.Url.StartsWith("http://"))
			{
				// And if an unsecured client is allowed
				if (allowUnsecure)
				{
					// Then designate the default client as UnsecureTess
					UnsecureTess = defaultClient;

					// And assign a new client to SecureTess (to be secured later)
					SecureTess = new Internals.TessituraWebApi.Tessitura();
				}

				// If an unsecured client is not allowed
				else
				{
					// Then SecureTess and UnsecureTess will share the same client object
					SecureTess = UnsecureTess = defaultClient;
				}

				// Secure the client's URL
				SecureTess.Url = SecureTess.Url.Insert(4, "s");
			}
			else
			{
				// Throw an exception when the client's URL format is unrecognized
				throw new ApplicationException(
					"The URL configured for the Tessitura web API is not supported.");
			}
		}

		/// <summary>
		///		Prepares a value to be passed as a string parameter to a web API method
		/// </summary>
		/// <param name="data">A string or null</param>
		/// <returns>
		///		A trimmed string, or an empty string if the value was null
		/// </returns>
		private static string Mask(string data)
		{
			return Mask(data, null);
		}

		/// <summary>
		///		Prepares a value to be passed as a string parameter to a web API method
		/// </summary>
		/// <param name="data">A string or null</param>
		/// <param name="nullReplacement">A value to replace a null (or empty) parameter</param>
		/// <returns>
		///		A trimmed string, or the replacement string if the value was null
		/// </returns>
		private static string Mask(string data, string nullReplacement)
		{
			// If the data is null or empty, replace it
			if (String.IsNullOrWhiteSpace(data))
			{
				// If the replacement string is also null or empty, default to String.Empty
				return String.IsNullOrWhiteSpace(nullReplacement) ?
						String.Empty : Mask(nullReplacement);
			}
			// If the data is not empty, return it trimmed
			else
			{
				return data.Trim();
			}
		}

		/// <summary>
		///		Prepares a performance sort field to be passed as a string parameter to a web API
		///		method
		/// </summary>
		/// <param name="data">A performance sort field to convert</param>
		/// <exception cref="ApiParameterMaskException">
		///		Thrown when an unrecognized PerfSortField was encountered
		/// </exception>
		/// <returns>
		///		The name of a column to sort performances by, or an empty string if the value was
		///		null
		/// </returns>
		private static string Mask(PerfSortField? data)
		{
			// If the sort field is null, return String.Empty
			if (data == null)
			{
				return String.Empty;
			}

			// Otherwise attempt to discern a performance column name from the specified sort field
			switch (data)
			{
				case PerfSortField.PackageId: return "pkg_no";
				case PerfSortField.Id: return "perf_no";
				case PerfSortField.PackageCode: return "pkg_code";
				case PerfSortField.Code: return "perf_code";
				case PerfSortField.Date: return "perf_date";
				case PerfSortField.GrossAvailability: return "gross_availbility";
				case PerfSortField.AvailabilityByCustomer: return "availbility_by_customer";
				case PerfSortField.VenueId: return "facility_no";
				case PerfSortField.MetCriteria: return "met_criteria_in";
				case PerfSortField.TimeSlotId: return "time_slot";
				case PerfSortField.Name: return "description";
				case PerfSortField.OnSale: return "on_sale_ind";
				case PerfSortField.BusinessUnit: return "bu";
				case PerfSortField.ProdSeasonId: return "prod_season_no";
				case PerfSortField.NoName: return "no_name";
				case PerfSortField.ZmapId: return "zmap_no";
				case PerfSortField.StartDate: return "start_dt";
				case PerfSortField.EndDate: return "end_dt";
				case PerfSortField.FirstDate: return "first_dt";
				case PerfSortField.LastDate: return "last_dt";
				case PerfSortField.VenueName: return "facility_desc";
				case PerfSortField.Weight: return "weight";
				case PerfSortField.SuperPackage: return "super_pkg_ind";
				case PerfSortField.FixedSeat: return "fixed_seat_ind";
				case PerfSortField.Flex: return "flex_ind";
				case PerfSortField.ProdTypeId: return "prod_type";
				case PerfSortField.ProdTypeName: return "prod_type_desc";
				case PerfSortField.SeasonId: return "season_no";
				case PerfSortField.SeasonName: return "season_desc";
				case PerfSortField.StatusId: return "perf_status";
				case PerfSortField.StatusName: return "perf_status_desc";
				case PerfSortField.Relevance: return "relevance";
				case PerfSortField.PremiereId: return "premiere_id";
				case PerfSortField.PremiereName: return "premiere_desc";
				case PerfSortField.TimeSlotName: return "time_slot_desc";
			}

			// Throw an exception if the sort field wasn't accounted for in the above list
			throw new ApiParameterMaskException(data.Value.ToString());
		}

		private static string Mask(FullTextSearchSyntaxType? data)
		{
			if (data == null)
			{
				return String.Empty;
			}
			switch (data)
			{
				case FullTextSearchSyntaxType.ContainsTable: return "C";
				case FullTextSearchSyntaxType.FreetextTable: return "F";
			}
			throw new ApiParameterMaskException(data.Value.ToString());
		}

		private static char Mask(OrderCommentTarget data)
		{
			switch (data)
			{
				case OrderCommentTarget.Contribution: return 'C';
				case OrderCommentTarget.LineItem: return 'L';
				case OrderCommentTarget.Order: return 'O';
			}
			throw new ApiParameterMaskException(data.ToString());
		}

		private static string Mask(Dictionary<string, string> data)
		{
			if (data == null || data.Keys.Count == 0)
			{
				return String.Empty;
			}
			List<string> keyValuePairs = (from item in data
										 select item.Key + "=" + item.Value).ToList<string>();
			return String.Join("&", keyValuePairs);
		}

		private static string Mask(ConstituentInfoType[] data)
		{
			if (data == null || data.Length == 0)
			{
				return String.Empty;
			}
			return String.Join(",", (from t in data
									select Mask(t)).ToArray<string>());
		}

		private static string Mask(ConstituentInfoType data)
		{
			switch (data)
			{
				case ConstituentInfoType.Header: return "HD";
				case ConstituentInfoType.Attributes: return "AT";
				case ConstituentInfoType.Constituencies: return "CT";
				case ConstituentInfoType.Contributions: return "CN";
				case ConstituentInfoType.Interests: return "IN";
				case ConstituentInfoType.EmailAddresses: return "EA";
				case ConstituentInfoType.Rankings: return "RN";
				case ConstituentInfoType.Associations: return "AS";
				case ConstituentInfoType.Memberships: return "MB";
				case ConstituentInfoType.PhoneNumbers: return "PH";
				case ConstituentInfoType.Addresses: return "AD";
				case ConstituentInfoType.ProgramListings: return "PR";
			}
			throw new ApiParameterMaskException(data.ToString());
		}

		private static string Mask(SpecialRequests data)
		{
			if (data == null)
			{
				return String.Empty;
			}
			StringBuilder syntax = new StringBuilder();
			bool stringStarted = false;
			string separator = "&";
			if (data.MinContiguousSeats.HasValue)
			{
				syntax.Append("ContiguousSeats=" + data.MinContiguousSeats.Value);
				stringStarted = true;
			}
			if (data.NumOfWheelChairSeats.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("WheelChairSeats=" + data.NumOfWheelChairSeats.Value);
			}
			if (data.EnsureNoStairs.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				char noStairsValue = data.EnsureNoStairs.Value ? 'Y' : 'N';
				syntax.Append("NoStairs=" + noStairsValue);
			}
			if (data.AislePref.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				char aisleValue;
				switch (data.AislePref.Value)
				{
					case AisleSeatPref.ByEitherAisle:
						aisleValue = 'A';
						break;
					case AisleSeatPref.ByLeftAisle:
						aisleValue = 'L';
						break;
					case AisleSeatPref.ByRightAisle:
						aisleValue = 'R';
						break;
					default: // NotByAisle
						aisleValue = 'N';
						break;
				}
				syntax.Append("AisleSeat=" + aisleValue);
			}
			if (data.StartingPrice.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("StartingPrice=" + data.StartingPrice.Value);
			}
			if (data.EndingPrice.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("EndingPrice=" + data.EndingPrice.Value);
			}
			if (!String.IsNullOrWhiteSpace(data.StartingRow))
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("StartingRow=" + data.StartingRow);
			}
			if (!String.IsNullOrWhiteSpace(data.EndingRow))
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("EndingRow=" + data.EndingRow);
			}
			if (!String.IsNullOrWhiteSpace(data.StartingSeat))
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("StartingSeat=" + data.StartingSeat);
			}
			if (!String.IsNullOrWhiteSpace(data.EndingSeat))
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("EndingSeat=" + data.EndingSeat);
			}
			if (data.AllowLeaveSingleSeats.HasValue)
			{
				if (stringStarted)
				{
					syntax.Append(separator);
				}
				else
				{
					stringStarted = true;
				}
				syntax.Append("LeaveSingleSeats=" +
						(data.AllowLeaveSingleSeats.Value ? "Y" : "N"));
			}
			return syntax.ToString();
		}

		#endregion

/*		public class ApiParameterMaskException : ApplicationException
		{
			public ApiParameterMaskException(string value)
				: base("Unable to mask API method parameter of value: " + value) { }
		}*/

		public static string GetApiDiagnostics()
		{
			return UnsecureTess.GetAPIDiagnostics();
		}

		public static string GetNewSessionKey()
		{
			return DoGetNewSessionKey(null, null);
		}

		public static string GetNewSessionKey(string ip)
		{
			return DoGetNewSessionKey(ip, null);
		}

		public static string GetNewSessionKey(int businessUnit)
		{
			return DoGetNewSessionKey(null, businessUnit);
		}

		public static string GetNewSessionKey(string ip, int businessUnit)
		{
			return DoGetNewSessionKey(ip, businessUnit);
		}

		private static string DoGetNewSessionKey(string ip, int? businessUnit)
		{
			return UnsecureTess.GetNewSessionKeyEx(
					sIP: Mask(ip, Defaults.Ip),
					iBusinessUnit: businessUnit ?? Defaults.BusinessUnit);
		}

		public static bool LoggedIn(string sessionKey)
		{
			return UnsecureTess.LoggedIn(Mask(sessionKey));
		}

		public static bool Login(string sessionKey, string username, string password)
		{
			return DoLogin(sessionKey, username, password, null, null, null, null, null, null,
					null);
		}

		public static bool Login(string sessionKey, string username, string password,
				int sourceId)
		{
			return DoLogin(sessionKey, username, password, null, sourceId, null, null, null, null,
					null);
		}

		public static bool Login(string sessionKey, string username, int loginTypeId,
				string password)
		{
			return DoLogin(sessionKey, username, password, loginTypeId, null, null, null, null,
					null, null);
		}

		public static bool Login(string sessionKey, string username, int loginTypeId,
				string password, int sourceId)
		{
			return DoLogin(sessionKey, username, password, loginTypeId, sourceId, null, null, null,
					null, null);
		}

		public static bool Login(string sessionKey, int constituentId, string phone,
				string postalCode)
		{
			return DoLogin(sessionKey, null, null, null, null, null, phone, postalCode,
					constituentId, null);
		}

		public static bool Login(string sessionKey, int constituentId, string phone,
				string postalCode, int sourceId)
		{
			return DoLogin(sessionKey, null, null, null, sourceId, null, phone, postalCode,
					constituentId, null);
		}

		public static bool Login(string sessionKey, int constituentId, int loginTypeId,
				string phone, string postalCode)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, null, null, phone, postalCode,
				constituentId, null);
		}

		public static bool Login(string sessionKey, int constituentId, int loginTypeId,
				string phone, string postalCode, int sourceId)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, sourceId, null, phone, postalCode,
					constituentId, null);
		}

		public static bool LoginWithToken(string sessionKey, string email, string token)
		{
			return DoLogin(sessionKey, null, null, null, null, email, null, null, null, token);
		}

		public static bool LoginWithToken(string sessionKey, string email, int loginTypeId,
				string token)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, null, email, null, null, null,
					token);
		}

		public static bool LoginWithToken(string sessionKey, string email, string token,
				int sourceId)
		{
			return DoLogin(sessionKey, null, null, null, sourceId, email, null, null, null, token);
		}

		public static bool LoginWithToken(string sessionKey, string email, int loginTypeId,
				string token, int sourceId)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, sourceId, email, null, null, null,
					token);
		}

		public static bool LoginWithEmail(string sessionKey, string email, string password)
		{
			return DoLogin(sessionKey, null, password, null, null, email, null, null, null, null);
		}

		public static bool LoginWithEmail(string sessionKey, string email, string password,
				int sourceId)
		{
			return DoLogin(sessionKey, null, password, null, sourceId, email, null, null, null,
					null);
		}

		public static bool LoginWithEmail(string sessionKey, string email, int loginTypeId,
				string password)
		{
			return DoLogin(sessionKey, null, password, loginTypeId, null, email, null, null, null,
					null);
		}

		public static bool LoginWithEmail(string sessionKey, string email, int loginTypeId,
				string password, int sourceId)
		{
			return DoLogin(sessionKey, null, password, loginTypeId, sourceId, email, null, null,
					null, null);
		}

		public static bool LoginWithEmail(string sessionKey, int constituentId, string phone,
				string postalCode, string email)
		{
			return DoLogin(sessionKey, null, null, null, null, email, phone, postalCode,
					constituentId, null);
		}

		public static bool LoginWithEmail(string sessionKey, int constituentId, string phone,
				string postalCode, string email, int sourceId)
		{
			return DoLogin(sessionKey, null, null, null, sourceId, email, phone, postalCode,
					constituentId, null);
		}

		public static bool LoginWithEmail(string sessionKey, int constituentId, int loginTypeId,
				string phone, string postalCode, string email)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, null, email, phone, postalCode,
					constituentId, null);
		}

		public static bool LoginWithEmail(string sessionKey, int constituentId, int loginTypeId,
				string phone, string postalCode, string email, int sourceId)
		{
			return DoLogin(sessionKey, null, null, loginTypeId, sourceId, email, phone, postalCode,
					constituentId, null);
		}

		private static bool DoLogin(string sessionKey, string username, string password,
				int? loginTypeId, int? sourceId, string email, string phone, string postalCode,
				int? constituentId, string token)
		{
			return SecureTess.LoginEx2(
					sSessionKey: Mask(sessionKey),
					sUID: Mask(username),
					sPwd: Mask(password),
					iLoginType: loginTypeId ?? Defaults.LoginTypeId,
					iPromotionCode: sourceId ?? 0,
					sEmail: Mask(email),
					sPhone: Mask(phone),
					sPostalCode: Mask(postalCode),
					iCustomerNo: constituentId ?? 0,
					iN1N2: 0,
					sForgotLoginToken: Mask(token));
		}

		public static Ambitus.LoginInfo GetLoginInfo(string sessionKey)
		{
			return new Ambitus.LoginInfo(UnsecureTess.LoginInfo(Mask(sessionKey)).Tables[0]);
		}

		public static void Logout(string sessionKey)
		{
			UnsecureTess.Logout(Mask(sessionKey));
		}

		public static bool CartExists(string sessionKey)
		{
			return UnsecureTess.OrderExists(Mask(sessionKey));
		}

		public static void UpdateLastAccessTime(string sessionKey)
		{
			DoUpdateLastAccessTime(sessionKey, null);
		}

		public static void UpdateLastAccessTime(string sessionKey, DateTime time)
		{
			DoUpdateLastAccessTime(sessionKey, time);
		}

		private static void DoUpdateLastAccessTime(string sessionKey, DateTime? time)
		{
			UnsecureTess.UpdateLastAccessTime(
					sSessionKey: Mask(sessionKey),
					sNow: time == null ? DateTime.Now.ToString() : time.Value.ToString());
		}

		public static void TransferSession(string sessionKey, string oldSessionKey)
		{
			UnsecureTess.TransferSession(
					sSessionKey: Mask(oldSessionKey),
					sNewKey: Mask(sessionKey));
		}

		public static void TransferCart(string sessionKey)
		{
			UnsecureTess.TransferCart(Mask(sessionKey));
		}

		public static SourceInfo GetSourceInfo(string sessionKey, string promoCode)
		{
			return DoGetPromoCode(sessionKey, promoCode, null);
		}

		public static SourceInfo GetSourceInfo(string sessionKey, int sourceId)
		{
			return DoGetPromoCode(sessionKey, null, sourceId);
		}

		private static SourceInfo DoGetPromoCode(string sessionKey, string promoCode, int? sourceId)
		{
			DataSet results = UnsecureTess.GetPromoCodeEx(
					SessionKey: Mask(sessionKey),
					PromotionCodeString: Mask(promoCode),
					PromotionCode: sourceId ?? 0);
			if (results.Tables[0].Rows.Count == 0)
			{
				return null;
			}
			return new SourceInfo(results.Tables[0]);
		}

		public static void DestroyCache()
		{
			UnsecureTess.DestroyCache();
		}

		public static bool TessituraCreditCardServerListening()
		{
			return UnsecureTess.TessituraCreditCardServerListening();
		}

		public static int GetWebSeatServerConnectionCount()
		{
			return UnsecureTess.WebSeatServerConnectionCount();
		}

		public static bool WebSeatServerListening()
		{
			return UnsecureTess.WebSeatServerListening();
		}

		public static bool TessituraSeatServerListeningViaWebSeatServer()
		{
			return UnsecureTess.TessituraSeatServerListeningViaWebSeatServer();
		}

		public static DateTime GetCartExpiration(string sessionKey)
		{
			return UnsecureTess.GetTicketExpiration(Mask(sessionKey));
		}

		public static void UpdateSessionSource(string sessionKey, int sourceId)
		{
			UnsecureTess.UpdateSourceCode(
					SessionKey: Mask(sessionKey),
					NewPromoCode: sourceId);
		}

		public static RegisterResult Register(string sessionKey, string username, string password,
				string email, string firstName, string lastName)
		{
			return DoRegister(sessionKey, username, password, null, email, firstName, lastName,
					null);
		}

		public static RegisterResult Register(string sessionKey, string username, string password,
				string email, string firstName, string lastName, int sourceId)
		{
			return DoRegister(sessionKey, username, password, null, email, firstName, lastName,
					sourceId);
		}

		public static RegisterResult Register(string sessionKey, string username, int loginTypeId,
				string password, string email, string firstName, string lastName)
		{
			return DoRegister(sessionKey, username, password, loginTypeId, email, firstName,
					lastName, null);
		}

		public static RegisterResult Register(string sessionKey, string username, int loginTypeId,
				string password, string email, string firstName, string lastName, int sourceId)
		{
			return DoRegister(sessionKey, username, password, loginTypeId, email, firstName,
					lastName, sourceId);
		}

		private static RegisterResult DoRegister(string sessionKey, string username,
				string password, int? loginTypeId, string email, string firstName, string lastName,
				int? sourceId)
		{
			RegisterResult result = RegisterResult.Success;
			bool succeeded = true;
			try
			{
				succeeded = SecureTess.RegisterWithPromoCode(
					sSessionKey: Mask(sessionKey),
					sUID: Mask(username),
					sPwd: password,
					iLoginType: loginTypeId ?? Defaults.LoginTypeId,
					sEmail: Mask(email),
					sFName: Mask(firstName),
					sLName: Mask(lastName),
					iPromoCode: sourceId ?? 0);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
				{
					result = RegisterResult.EmailUsed;
				}
				else if (e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION"))
				{
					result = RegisterResult.UsernameTaken;
				}
				else
				{
					throw e;
				}
			}
			if (!succeeded)
			{
				throw new ApplicationException("Registration failed unexpectedly.");
			}
			return result;
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, string email)
		{
			return DoSendCredentials(sessionKey, email, null, null, SendCredentialsAction.UseToken,
					null);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, string email,
				int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, null, null, SendCredentialsAction.UseToken,
					emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, string email,
				bool setTempFlag)
		{
			return DoSendCredentials(sessionKey, email, null, setTempFlag,
					SendCredentialsAction.UseToken, null);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, string email,
				bool setTempFlag, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, null, setTempFlag,
					SendCredentialsAction.UseToken, emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, int loginTypeId,
				string email)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, null,
					SendCredentialsAction.UseToken, null);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, int loginTypeId,
				string email, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, null,
					SendCredentialsAction.UseToken,	emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, int loginTypeId,
				string email, bool setTempFlag)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, setTempFlag,
					SendCredentialsAction.UseToken, null);
		}

		public static SendCredentialsResult SendCredentials(string sessionKey, int loginTypeId,
				string email, bool setTempFlag, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, setTempFlag,
					SendCredentialsAction.UseToken, emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, string email)
		{
			return DoSendCredentials(sessionKey, email, null, null, action, null);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, string email, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, null, null, action, emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, string email, bool setTempFlag)
		{
			return DoSendCredentials(sessionKey, email, null, setTempFlag, action, null);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, string email, bool setTempFlag, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, null, setTempFlag, action, emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, int loginTypeId, string email)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, null, action, null);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, int loginTypeId, string email, int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, null, action, emailTemplateId);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, int loginTypeId, string email, bool setTempFlag)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, setTempFlag, action, null);
		}

		public static SendCredentialsResult SendCredentials(SendCredentialsAction action,
				string sessionKey, int loginTypeId, string email, bool setTempFlag,
				int emailTemplateId)
		{
			return DoSendCredentials(sessionKey, email, loginTypeId, setTempFlag, action,
					emailTemplateId);
		}
		
		private static SendCredentialsResult DoSendCredentials(string sessionKey, string email,
				int? loginTypeId, bool? setTempFlag, SendCredentialsAction action,
				int? emailTemplateId)
		{
			bool makeToken, makeNewPassword;
			switch (action)
			{
				case SendCredentialsAction.UseToken:
					makeToken = true;
					makeNewPassword = false;
					break;
				case SendCredentialsAction.MakeNewPassword:
					makeToken = false;
					makeNewPassword = true;
					break;
				default: // I.E., case SendCredentialsAction.UseOldPassword:
					makeToken = false;
					makeNewPassword = false;
					break;
			}
			SendCredentialsResult result = SendCredentialsResult.Success;
			bool succeeded = true;
			try
			{
				succeeded = UnsecureTess.SendCredentials(
						sSessionKey: Mask(sessionKey),
						sEmail: Mask(email),
						iLoginType: loginTypeId ?? Defaults.LoginTypeId,
						bResetTempFlag: setTempFlag ?? true,
						bUseToken: makeToken,
						bGenerateNewPassword: makeNewPassword,
						iTemplateID: emailTemplateId ?? Defaults.CredentialsEmailTemplateId);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("No login found"))
				{
					result = SendCredentialsResult.NotFound;
				}
				else if (e.Message.Contains("Can not locate"))
				{
					result = SendCredentialsResult.NoActiveLogin;
				}
				else
				{
					throw e;
				}
			}
			if (!succeeded)
			{
				throw new ApplicationException("Account recovery failed unexpectedly.");
			}
			return result;
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				string password, string email, string newUsername)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, null,
					null, null);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newEmail)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, newEmail,
					null, null);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, null,
					null, sourceId);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newEmail, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, newEmail,
					null, sourceId);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, null,
					loginTypeId, null);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername, string newEmail)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, newEmail,
					loginTypeId, null);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, null,
					loginTypeId, sourceId);
		}

		public static UpdateLoginResult UpdateUsernameAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername, string newEmail,
				int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, null, email, newEmail,
					loginTypeId, sourceId);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				string password, string email, string newPassword)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, null,
					null, null);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				string password, string email, string newPassword, string newEmail)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, newEmail,
					null, null);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				string password, string email, string newPassword, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, null,
					null, sourceId);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				string password, string email, string newPassword, string newEmail, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, newEmail,
					null, sourceId);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newPassword)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, null,
					loginTypeId, null);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newPassword, string newEmail)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, newEmail,
					loginTypeId, null);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newPassword, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, null,
					loginTypeId, sourceId);
		}

		public static UpdateLoginResult UpdatePasswordAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newPassword, string newEmail,
				int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, null, password, newPassword, email, newEmail,
					loginTypeId, sourceId);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newPassword)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					null, null, null);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newPassword, string
				newEmail)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					newEmail, null, null);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newPassword, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					null, null, sourceId);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				string password, string email, string newUsername, string newPassword,
				string newEmail, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					newEmail, null, sourceId);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername,
				string newPassword)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					null, loginTypeId, null);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername,
				string newPassword, string newEmail)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					newEmail, loginTypeId, null);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername,
				string newPassword, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					null, loginTypeId, sourceId);
		}

		public static UpdateLoginResult UpdateLoginAndEmail(string sessionKey, string username,
				int loginTypeId, string password, string email, string newUsername,
				string newPassword, string newEmail, int sourceId)
		{
			return DoUpdateLogin(sessionKey, username, newUsername, password, newPassword, email,
					newEmail, loginTypeId, sourceId);
		}

		private static UpdateLoginResult DoUpdateLogin(string sessionKey, string oldUsername,
				string newUsername, string oldPassword, string newPassword, string oldEmail,
				string newEmail, int? loginTypeId, int? sourceId)
		{
			oldUsername = Mask(oldUsername);
			oldEmail = Mask(oldEmail);
			UpdateLoginResult result = UpdateLoginResult.Success;
			bool succeeded = true;
			try
			{
				succeeded = SecureTess.UpdateLoginWithPromoCode(
						sSessionKey: Mask(sessionKey),
						sUID: oldUsername,
						sNewUID: Mask(newUsername, oldUsername),
						sPwd: oldPassword,
						sNewPwd: newPassword ?? oldPassword,
						sEmail: oldEmail,
						sNewEmail: Mask(newEmail, oldEmail),
						iLoginType: loginTypeId ?? Defaults.LoginTypeId,
						iPromotionCode: sourceId ?? 0);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION"))
				{
					result = UpdateLoginResult.UsernameTaken;
				}
				else if (e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
				{
					result = UpdateLoginResult.EmailUsed;
				}
				else
				{
					throw e;
				}
			}
			if (!succeeded)
			{
				result = UpdateLoginResult.LoginInvalid;
			}
			return result;
		}

		public static AccountInfo GetAccountInfo(string sessionKey)
		{
			return new AccountInfo(UnsecureTess.GetAccountInfo(Mask(sessionKey)).Tables[0]);
		}

		public static UpdateAccountInfoResult UpdateAccountInfo(string sessionKey,
				AccountInfoUpdate updates)
		{
			return UpdateAccountInfo(
					sessionKey: sessionKey,
					email: updates.Email.Value,
					phone: updates.Phone,
					streetAddress: updates.StreetAddress.Value,
					subStreetAddress: updates.SubStreetAddress.Value,
					city: updates.City.Value,
					state: updates.State,
					postalCode: updates.PostalCode.Value,
					countryId: updates.CountryId,
					//phone2: updates.Phone2,
					//phone2TypeId: updates.Phone2TypeId,
					fax: updates.Fax,
					firstName: updates.FirstName.Value,
					middleName: updates.MiddleName.Value,
					lastName: updates.LastName.Value,
					prefix: updates.Prefix.Value,
					suffix: updates.Suffix.Value,
					businessTitle: updates.BusinessTitle.Value,
					emailRestrictionId: updates.EmailRestrictionId,
					mailRestrictionId: updates.MailRestrictionId,
					phoneRestrictionId: updates.PhoneRestrictionId,
					acceptsHtmlEmail: updates.AcceptsHtmlEmail,
					gender: updates.Gender.Value,
					gender2: updates.Gender2.Value,
					firstName2: updates.FirstName2.Value,
					middleName2: updates.MiddleName2.Value,
					lastName2: updates.LastName2.Value,
					prefix2: updates.Prefix2.Value,
					suffix2: updates.Suffix2.Value,
					originalSourceId: updates.OriginalSourceId,
					allowChangesToSalutations: updates.AllowChangesToSalutationsAndBusinessTitle,
					addressTypeId: updates.AddressTypeId,
					emailAddressTypeId: updates.EmailAddressTypeId,
					salutationLine1: updates.SalutationLine1.Value,
					salutationLine2: updates.SalutationLine2.Value,
					letterSalutation: updates.LetterSalutation.Value,
					constituentTypeId: updates.ConstituentTypeId,
					nameStatusId: updates.NameStatusId,
					name2StatusId: updates.Name2StatusId);
		}

		private static UpdateAccountInfoResult UpdateAccountInfo(string sessionKey,	string email,
				string phone, string streetAddress, string subStreetAddress, string city,
				string state, string postalCode, int? countryId, //string phone2, int? phone2TypeId,
				string fax, string firstName, string middleName, string lastName, string prefix,
				string suffix, string businessTitle, int? emailRestrictionId,
				int? mailRestrictionId, int? phoneRestrictionId, bool? acceptsHtmlEmail,
				string gender, string gender2, string firstName2, string middleName2,
				string lastName2, string prefix2, string suffix2, int? originalSourceId,
				bool? allowChangesToSalutations, int? addressTypeId, int? emailAddressTypeId,
				string salutationLine1, string salutationLine2, string letterSalutation,
				int? constituentTypeId, int? nameStatusId, int? name2StatusId)
		{
			string sHtmlIndicatorValue;
			if (acceptsHtmlEmail.HasValue)
			{
				sHtmlIndicatorValue = acceptsHtmlEmail.Value ? "Y" : "N";
			}
			else
			{
				sHtmlIndicatorValue = String.Empty;
			}
			UpdateAccountInfoResult result = UpdateAccountInfoResult.Success;
			try
			{
				UnsecureTess.UpdateAccountInfoEx2(
						sSessionKey: Mask(sessionKey),
						sEmail: Mask(email),
						sPhone: Mask(phone),
						sStreet1: Mask(streetAddress),
						sStreet2: Mask(subStreetAddress),
						sCity: Mask(city),
						sStateProv: Mask(state),
						sPostalCode: postalCode ?? String.Empty,
						iCountry: countryId ?? 0,
						sPhone2: String.Empty,//Mask(phone2),
						iPhone2Type: 0,//phone2TypeId ?? 0,
						sFax: Mask(fax),
						sFirstName: Mask(firstName),
						sLastName: Mask(lastName),
						sMiddleName: Mask(middleName),
						sPrefix: Mask(prefix),
						sSuffix: Mask(suffix),
						sBusinessTitle: Mask(businessTitle),
						iEmailIndicator: emailRestrictionId ?? 0,
						iMailIndicator: mailRestrictionId ?? 0,
						iPhoneIndicator: phoneRestrictionId ?? 0,
						sHtmlIndicator: sHtmlIndicatorValue,
						sGender: Mask(gender),
						sGender2: Mask(gender2),
						sFirstName2: Mask(firstName2),
						sMiddleName2: Mask(middleName2),
						sLastName2: Mask(lastName2),
						sPrefix2: Mask(prefix2),
						sSuffix2: Mask(suffix2),
						iOriginalSourceNumber: originalSourceId ?? 0,
						bUpdateSalutation: allowChangesToSalutations ?? true,
						iAddressTypeOverwrite: addressTypeId ?? 0,
						iEaddressTypeOverwrite: emailAddressTypeId ?? 0,
						sEsal1DescriptionOverwrite: Mask(salutationLine1),
						sEsal2DescriptionOverwrite: Mask(salutationLine2),
						sLsalDescriptionOverwrite: Mask(letterSalutation),
						iConstituentTypeOverwrite: constituentTypeId ?? 0,
						iNameStatus: nameStatusId ?? 0,
						iName2Status: name2StatusId ?? 0);
			}
			catch (Exception e)
			{
				if (e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
				{
					result = UpdateAccountInfoResult.EmailUsed;
				}
				else
				{
					throw e;
				}
			}
			return result;
		}

		public static VariableCollection GetSessionVariables(string sessionKey)
		{
			DataSet results = SecureTess.GetVariables(Mask(sessionKey));
			if (results.Tables[0].Rows.Count == 0)
			{
				return null;
			}
			return new VariableCollection(SecureTess.GetVariables(Mask(sessionKey)).Tables[0]);
		}

		public static void SetSessionVariable(string sessionKey, string name, string value)
		{
			DoSetVariable(sessionKey, name, value, null);
		}

		public static void SetSessionVariable(string sessionKey, string name, string value,
				bool allowNewVariableCreation)
		{
			DoSetVariable(sessionKey, name, value, allowNewVariableCreation);
		}

		private static void DoSetVariable(string sessionKey, string name, string value,
				bool? allowNewVariableCreation)
		{
			string sModeValue = null;
			if (allowNewVariableCreation.HasValue)
			{
				sModeValue = allowNewVariableCreation.Value ? String.Empty : "U";
			}
			SecureTess.SetVariable(
					sSessionKey: Mask(sessionKey),
					sName: Mask(name),
					sValue: Mask(value),
					sMode: sModeValue ?? String.Empty);
		}

		public static void DeleteSessionVariable(string sessionKey, string name)
		{
			UnsecureTess.DeleteVariable(
					sSessionKey: sessionKey,
					sName: name);
		}

		public static void DisconnectSessionSeatServer(string sessionKey)
		{
			UnsecureTess.DisconnectSessionSeatServer(Mask(sessionKey));
		}

		public static DateTime AlterCartExpiration(string sessionKey, DateTime absoluteTime)
		{
			return DoAlterTicketExpiration(sessionKey, absoluteTime, null);
		}

		public static DateTime AlterCartExpiration(string sessionKey, TimeSpan offsetTime)
		{
			return DoAlterTicketExpiration(sessionKey, null, offsetTime);
		}

		private static DateTime DoAlterTicketExpiration(string sessionKey, DateTime? absoluteTime,
				TimeSpan? offsetTime)
		{
			int? iOffsetSecondsValue = null;
			if (offsetTime.HasValue)
			{
				iOffsetSecondsValue = Convert.ToInt32(offsetTime.Value.TotalSeconds);
			}
			return UnsecureTess.AlterTicketExpiration(
				sSessionKey: Mask(sessionKey),
				sExpirationTime: absoluteTime.HasValue ?
						absoluteTime.Value.ToString() : String.Empty,
				iOffsetSeconds: iOffsetSecondsValue ?? 0);
		}

		public static CountryCollection GetCountries()
		{
			return new CountryCollection(UnsecureTess.GetCountries().Tables[0]);
		}

		public static SettingCollection GetApiSettings()
		{
			return GetApiSettings(null);
		}

		public static SettingCollection GetApiSettings(string orgName)
		{
			return new SettingCollection(UnsecureTess.GetDefaults(
					sOrganizationName: Mask(orgName, Defaults.OrganizationName)).Tables[0]);
		}

		public static StateProvinceCollection GetStatesProvinces()
		{
			return DoGetStatesProvinces(null, null);
		}

		public static StateProvinceCollection GetStatesProvinces(int countryId)
		{
			return DoGetStatesProvinces(countryId, null);
		}

		public static StateProvinceCollection GetStatesProvinces(int[] countryIds)
		{
			return DoGetStatesProvinces(null, countryIds);
		}

		private static StateProvinceCollection DoGetStatesProvinces(int? countryId,
				int[] countryIds)
		{
			string countryIdsValue = null;
			if (countryId.HasValue)
			{
				countryIdsValue = countryId.Value.ToString();
			}
			else if (countryIds != null)
			{
				countryIdsValue = String.Join(",", countryIds);
			}
			/*else if (countryIds != null && countryIds.Length > 0)
			{
				StringBuilder ids = new StringBuilder();
				ids.Append(countryIds[0].ToString());
				for (int i = 1; i < countryIds.Length; i++)
				{
					ids.Append("," + countryIds[i].ToString());
				}
				countryIdsValue = ids.ToString();
			}*/
			DataSet tessResults = UnsecureTess.GetStateProvinceEx(
					CountryIds: countryIdsValue ?? String.Empty);
			if (tessResults.Tables["StateProvince"].Rows.Count > 0)
			{
				return new StateProvinceCollection(tessResults.Tables["StateProvince"]);
			}
			else
			{
				return null;
			}
		}

		public static ContactRestrictionCollection GetContactRestrictions(string sessionKey)
		{
			return DoGetContactOptions(sessionKey, null);
		}

		public static ContactRestrictionCollection GetContactRestrictions(string sessionKey,
				ContactRestrictionType type)
		{
			return DoGetContactOptions(sessionKey, type);
		}

		private static ContactRestrictionCollection DoGetContactOptions(string sessionKey,
					ContactRestrictionType? type)
		{
			string sRestrictionTypeValue = null;
			if (type.HasValue)
			{
				switch(type.Value)
				{
					case ContactRestrictionType.Mail:
						sRestrictionTypeValue = "M";
						break;
					case ContactRestrictionType.Email:
						sRestrictionTypeValue = "E";
						break;
					case ContactRestrictionType.Phone:
						sRestrictionTypeValue = "P";
						break;
				}
			}
			return new ContactRestrictionCollection(UnsecureTess.GetContactOptions(
					sSessionKey: sessionKey,
					sRestrictionType: sRestrictionTypeValue ?? String.Empty).Tables[0]);
		}

		public static Attribute GetAttribute(string sessionKey, int id)
		{
			AttributeCollection attributes = DoGetAttributes(sessionKey, id);
			if (attributes == null)
			{
				return null;
			}
			return attributes[0];
		}

		public static AttributeCollection GetAttributes(string sessionKey)
		{
			return DoGetAttributes(sessionKey, null);
		}

		private static AttributeCollection DoGetAttributes(string sessionKey, int? id)
		{
			DataSet results = UnsecureTess.GetAttributes(
					sSessionKey: Mask(sessionKey),
					iAttributeID: id ?? 0);
			if (results.Tables["AttributesKeyword"].Rows.Count == 0)
			{
				return null;
			}
			return new AttributeCollection(results);
		}

		public static VenueCollection GetVenues()
		{
			return new VenueCollection(UnsecureTess.GetVenue().Tables[0]);
		}

		public static ModeOfSaleRules GetModeOfSaleRules(string sessionKey)
		{
			return new ModeOfSaleRules(UnsecureTess.GetModeOfSaleRules(Mask(sessionKey)).Tables[0]);
		}

		public static PerfCollection GetPerformances(string sessionKey,
				PerfSearchCriteria criteria)
		{
			return DoGetPerformances(sessionKey, criteria.StartDateTime, criteria.EndDateTime,
					criteria.VenueId, criteria.ModeOfSaleId, criteria.BusinessUnit,
					criteria.SortField, criteria.Keywords, criteria.MatchAllKeywords,
					criteria.ArtistLastName, criteria.ArtistFirstName, criteria.ArtistMiddleName,
					criteria.FullTextSearchCriteria, criteria.FullTextSearchSyntaxType,
					criteria.ContentTypeIds, criteria.Ids, criteria.SeasonIds,
					criteria.ComputeSeatCounts);
		}

		private static PerfCollection DoGetPerformances(string sessionKey,
				DateTime? startDateTime, DateTime? endDateTime, short? venueId, short? modeOfSaleId,
				int? businessUnit, PerfSortField? sortField, string[] keywords,
				bool? matchAllKeywords, string artistLastName, string artistFirstName,
				string artistMiddleName, string fullTextSearchCriteria,
				FullTextSearchSyntaxType? fullTextSearchSyntaxType,
				ContentTypeIdsParam contentTypeIds, int[] perfIds, int[] seasonIds,
				bool? computeSeatCounts)
		{
			string cKeywordAndOrStatementValue;
			if (matchAllKeywords.HasValue)
			{
				cKeywordAndOrStatementValue = matchAllKeywords.Value ? "2" : "1";
			}
			else
			{
				cKeywordAndOrStatementValue = String.Empty;
			}
			string sArtistLastNameValue;
			if (artistLastName == null && artistFirstName == null && artistMiddleName == null)
			{
				sArtistLastNameValue = String.Empty;
			}
			else
			{
				sArtistLastNameValue = Mask(artistLastName) + "/" + Mask(artistFirstName)
						+ "/" + Mask(artistMiddleName);
			}
			DataSet results = UnsecureTess.GetPerformancesEx4(
					sWebSessionId: Mask(sessionKey),
					sStartDate: startDateTime.HasValue ? startDateTime.ToString() : String.Empty,
					sEndDate: endDateTime.HasValue ? endDateTime.ToString() : String.Empty,
					iVenueID: venueId ?? -1,
					iModeOfSale: modeOfSaleId ?? Defaults.ModeOfSaleId,
					iBusinessUnit: businessUnit ?? -1,
					sSortString: Mask(sortField),
					sKeywords: keywords == null ? String.Empty : String.Join(",", keywords),
					cKeywordAndOrStatement: cKeywordAndOrStatementValue,
					sArtistLastName: sArtistLastNameValue,
					sFullText: fullTextSearchCriteria ?? String.Empty,
					sFullTextType: Mask(fullTextSearchSyntaxType),
					sContentType: contentTypeIds == null ? String.Empty : contentTypeIds.ToString(),
					sPerformanceIds: perfIds == null ? String.Empty : String.Join(",", perfIds),
					sSeasonIds: seasonIds == null ? String.Empty : String.Join(",", seasonIds),
					bIncludeSeatCounts: computeSeatCounts ?? false);
			if (results.Tables["Performance"].Rows.Count == 0)
			{
				return null;
			}
			return new PerfCollection(results.Tables["Performance"]);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, null, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, null, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				bool computeSeatCounts)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, null, null,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, null, null,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, null, contentTypeIds, null);
		}
		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, null, contentTypeIds,
					null);
		}
		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				bool computeSeatCounts, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, null, contentTypeIds,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, null, contentTypeIds,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, businessUnit, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, businessUnit, null,
					null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				bool computeSeatCounts, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, businessUnit, null,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, businessUnit, null,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, businessUnit, contentTypeIds,
					null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, businessUnit,
					contentTypeIds, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				bool computeSeatCounts, ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, null, businessUnit, contentTypeIds,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetails(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, ContentTypeIdsParam contentTypeIds,
				int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, null, id, modeOfSaleId, businessUnit, contentTypeIds,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, null, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, null, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				bool computeSeatCounts)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, null, null,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, null, null,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, null, contentTypeIds, null);
		}
		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, null, contentTypeIds,
					null);
		}
		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				bool computeSeatCounts, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, null, contentTypeIds,
					computeSeatCounts);
		}
		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, null, contentTypeIds,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, businessUnit, null, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, businessUnit, null,
					null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				bool computeSeatCounts, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, businessUnit, null,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, businessUnit, null,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, businessUnit, contentTypeIds,
					null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, businessUnit,
					contentTypeIds, null);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				bool computeSeatCounts, ContentTypeIdsParam contentTypeIds, int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, null, businessUnit, contentTypeIds,
					computeSeatCounts);
		}

		public static ProdSeasonDetails GetProdSeasonDetailsForPerf(string sessionKey, int id,
				short modeOfSaleId, bool computeSeatCounts, ContentTypeIdsParam contentTypeIds,
				int businessUnit)
		{
			return DoGetProductionDetails(sessionKey, id, null, modeOfSaleId, businessUnit, contentTypeIds,
					computeSeatCounts);
		}

		private static ProdSeasonDetails DoGetProductionDetails(string sessionKey, int? perfId,
				int? prodSeasonId, short? modeOfSaleId, int? businessUnit,
				ContentTypeIdsParam contentTypeIds, bool? computeSeatCounts)
		{
			string sContentTypesValue = String.Empty;
			string sPerformanceContentTypesValue = String.Empty;
			if (contentTypeIds != null)
			{
				if (prodSeasonId.HasValue)
				{
					sContentTypesValue = contentTypeIds.ToString();
				}
				else if (perfId.HasValue)
				{
					sPerformanceContentTypesValue = contentTypeIds.ToString();
				}
			}
			DataSet results = UnsecureTess.GetProductionDetailEx3(
					SessionKey: Mask(sessionKey),
					iPerf_no: perfId ?? 0,
					iProd_Season_no: prodSeasonId ?? 0,
					iModeOfSale: modeOfSaleId ?? Defaults.ModeOfSaleId,
					iBusinessUnit: businessUnit ?? Defaults.BusinessUnit,
					sContentTypes: sContentTypesValue,
					sPerformanceContentTypes: sPerformanceContentTypesValue,
					bIncludeSeatCounts: computeSeatCounts ?? false);
			if (results.Tables[0].Rows.Count == 0)
			{
				return null;
			}
			return new ProdSeasonDetails(results, prodSeasonId.HasValue);
		}

		public static SeatPricing GetSeatPricing(string sessionKey, int perfId)
		{
			return DoGetPerformanceDetails(sessionKey, perfId, null, null);
		}

		public static SeatPricing GetSeatPricing(string sessionKey, int perfId,
				short modeOfSaleId)
		{
			return DoGetPerformanceDetails(sessionKey, perfId, modeOfSaleId, null);
		}

		public static SeatPricing GetSeatPricing(string sessionKey, int perfId,
				ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetails(sessionKey, perfId, null, contentTypeIds);
		}

		public static SeatPricing GetSeatPricing(string sessionKey, int perfId,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetails(sessionKey, perfId, modeOfSaleId, contentTypeIds);
		}

		private static SeatPricing DoGetPerformanceDetails(string sessionKey, int perfId,
				short? modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return new SeatPricing(UnsecureTess.GetPerformanceDetailWithDiscountingEx(
					SessionKey: sessionKey,
					iPerf_no: perfId,
					iModeOfSale: modeOfSaleId ?? Defaults.ModeOfSaleId,
					sContentType:
						contentTypeIds == null ? String.Empty : contentTypeIds.ToString()));
		}

		public static SeatStatusCollection GetSeatStatuses()
		{
			return new SeatStatusCollection(UnsecureTess.GetSeatStatus().Tables["Status"]);
		}

		public static void AddContributionToFund(string sessionKey, decimal amount, int fundId)
		{
			DoAddContribution(sessionKey, amount, fundId, null, null);
		}

		public static void AddContributionToFund(string sessionKey, decimal amount, int fundId,
				ContributionAction action)
		{
			DoAddContribution(sessionKey, amount, fundId, null, action);
		}

		public static void AddContributionOnAccount(string sessionKey, decimal amount,
				int paymentMethodId)
		{
			DoAddContribution(sessionKey, amount, null, paymentMethodId, null);
		}

		public static void AddContributionOnAccount(string sessionKey, decimal amount,
				int paymentMethodId, ContributionAction action)
		{
			DoAddContribution(sessionKey, amount, null, paymentMethodId, action);
		}

		private static void DoAddContribution(string sessionKey, decimal amount, int? fundId,
				int? paymentMethodId, ContributionAction? action)
		{
			UnsecureTess.AddContribution(
					sWebSessionID: Mask(sessionKey),
					Amount: amount,
					Fund: fundId ?? 0,
					AccountMethod: paymentMethodId ?? 0,
					Upgrade: action.HasValue ? action == ContributionAction.Upgrade : false,
					Renew: action.HasValue ? action == ContributionAction.Renew : false);
		}

		public static Cart GetCart(string sessionKey)
		{
			DataSet results = SecureTess.GetCart(Mask(sessionKey));
			if (results.Tables["Order"].Rows.Count == 0)
			{
				return null;
			}
			return new Cart(results);
		}

		public static void UpdateContributionNote(string sessionKey, int contributionId,
				string note)
		{
			DoAddOrderComment(sessionKey, note, OrderCommentTarget.Contribution, contributionId,
					null, null);
		}

		private static void DoAddOrderComment(string sessionKey, string comment,
				OrderCommentTarget target, int? targetId, int? constituentId, int? categoryId)
		{
			SecureTess.AddOrderCommentsEx2(
					SessionKey: Mask(sessionKey),
					Comment: Mask(comment),
					LineItemID: targetId ?? 0,
					LineItemType: Mask(target),
					CustomerNo: constituentId ?? 0,
					CategoryNo: categoryId ?? 0);
		}

		public static int SetModeOfSaleToDefault(string sessionKey)
		{
			return SetModeOfSale(sessionKey, 0);
		}

		public static int SetModeOfSale(string sessionKey, int modeOfSaleId)
		{
			return UnsecureTess.ChangeModeOfSaleEx(sessionKey, modeOfSaleId);
		}

		public static PaymentMethodCollection GetPaymentMethods(string sessionKey)
		{
			return new PaymentMethodCollection(
					UnsecureTess.GetPaymentMethod(Mask(sessionKey)).Tables["PaymentMethod"]);
		}

		public static ShippingMethodCollection GetShippingMethods(string sessionKey)
		{
			return new ShippingMethodCollection(
					UnsecureTess.GetShippingMethods(Mask(sessionKey)).Tables["ShippingMethods"]);
		}

		public static CheckoutResult CheckoutWithCreditCard(string sessionKey,
				decimal paymentAmount, int paymentMethodId, string cardNumber,
				string cardHolderName, int cardExpMonth, int cardExpYear, string cardSecurityCode)
		{
			return DoCheckout(sessionKey, cardHolderName, cardNumber, paymentMethodId, cardExpMonth,
					cardExpYear, paymentAmount, null, cardSecurityCode, null, null, null, true,
					null, null);
		}

		private static CheckoutResult DoCheckout(string sessionKey, string cardHolderName,
				string cardNumber, int? paymentMethodId, int? cardExpMonth, int? cardExpYear,
				decimal? paymentAmount, bool? allowUnderPayment, string cardSecurityCode,
				int? cardIssueNumber, int? cardStartMonth, int? cardStartYear,
				bool? requiresCardAuthorization, string authorizationCode,
				Dictionary<string,string> threeDSecureValues)
		{
			try
			{
				SecureTess.CheckoutEx3(
						sSessionKey: Mask(sessionKey),
						sCCOwner: Mask(cardHolderName),
						sCCNumber: Mask(cardNumber),
						iCCType: paymentMethodId ?? 0,
						iCCMonth: cardExpMonth ?? 0,
						iCCYear: cardExpYear ?? 0,
						dAmount: paymentAmount ?? 0,
						bAllowUnderPayment: allowUnderPayment ?? false,
						sCardAuthenticationCode: Mask(cardSecurityCode),
						iCCIssueNumber: cardIssueNumber ?? 0,
						iCCStartMonth: cardStartMonth ?? 0,
						iCCStartYear: cardStartYear ?? 0,
						bAuthorize: requiresCardAuthorization ?? true,
						sAuthorizationCode: Mask(authorizationCode),
						s3DSecureValues: Mask(threeDSecureValues));
			}
			catch (Exception e)
			{
				if (e.Message.Contains("CREDITCARD_VALIDATION_EXCEPTION"))
				{
					return CheckoutResult.Invalid;
				}
				else if (e.Message.Contains("CREDITCARD_TYPE_MISMATCH_EXCEPTION"))
				{
					return CheckoutResult.TypeMismatch;
				}
				else if (e.Message.Contains("CCS_TIMEOUT_EXCEPTION")
						|| e.Message.Contains("SEATSERVER_TIMEOUT_EXCEPTION"))
				{
					return CheckoutResult.TimedOut;
				}
				else if (e.Message.Contains("AUTHORIZATION_EXCEPTION"))
				{
					return CheckoutResult.NotAuthorized;
				}
				else if (e.Message.Contains("DECLINATION_EXCEPTION"))
				{
					return CheckoutResult.Declined;
				}
				throw new ApplicationException("Checkout unexpectedly failed.");
			}
			return CheckoutResult.Succeeded;
		}

		public static ConstituentInfo GetConstituentInfo(string sessionKey)
		{
			return GetConstituentInfo(sessionKey, null);
		}

		public static ConstituentInfo GetConstituentInfo(string sessionKey,
				params ConstituentInfoType[] requestedSets)
		{
			return new ConstituentInfo(UnsecureTess.GetConstituentInfoEx(
					SessionKey: Mask(sessionKey),
					TableListTokens: Mask(requestedSets)));
		}

		public static void SetShippingInfo(string sessionKey, int addressId, int methodId)
		{
			UnsecureTess.SetShippingInformation(
					sSessionKey: Mask(sessionKey),
					iAddress_no: addressId,
					iShippingMethod: methodId);
		}

		public static int GetMaxTicketsPerRequest()
		{
			return UnsecureTess.GetMaxTicketAmount();
		}

		public static ConstituentAttributeCollection AddAttribute(string sessionKey,
				int attributeId, string value)
		{
			return DoUpdateConstituentAttributes(sessionKey, 'A', attributeId, null, value, null);
		}

		public static ConstituentAttributeCollection DoUpdateConstituentAttributes(
			string sessionKey, char action, int attributeId, string oldValue, string newValue,
			char? n1n2)
		{
			DataSet results = SecureTess.UpdateConstituentAttributes(
					sWebSessionId: Mask(sessionKey),
					cAction: action,
					iKeywordNumber: attributeId,
					sOldKeyValue: Mask(oldValue),
					sNewKeyValue: Mask(newValue),
					cAccountName: n1n2 ?? '3');
			if (results.Tables.Contains("Table")
					&& results.Tables["Table"].Rows.Count > 0)
			{
				return new ConstituentAttributeCollection(results.Tables["Table"]);
			}
			else
			{
				return null;
			}
		}

		public static void UpdateContributionCustomData(string sessionKey, int contributionId,
				byte fieldIndex, string newValue)
		{
			DoUpdateCustomOrderData(sessionKey, contributionId, fieldIndex, newValue);
		}

		private static void DoUpdateCustomOrderData(string sessionKey, int? contributionId,
				byte fieldIndex, string newValue)
		{
			SecureTess.UpdateCustomOrderData(
					SessionKey:	sessionKey,
					RefNo: contributionId ?? 0,
					FieldIndex: fieldIndex,
					FieldValue: newValue);
		}

		public static void SendOrderConfirmationEmail(string sessionKey, int emailTemplateId)
		{
			DoSendOrderConfirmationEmail(sessionKey, emailTemplateId, null, null);
		}

		public static void SendOrderConfirmationEmail(string sessionKey, int emailTemplateId,
				string subjectOverride)
		{
			DoSendOrderConfirmationEmail(sessionKey, emailTemplateId, null, subjectOverride);
		}

		public static void SendOrderConfirmationEmail(string sessionKey, int orderId,
				int emailTemplateId)
		{
			DoSendOrderConfirmationEmail(sessionKey, emailTemplateId, orderId, null);
		}

		public static void SendOrderConfirmationEmail(string sessionKey, int orderId,
				int emailTemplateId, string subjectOverride)
		{
			DoSendOrderConfirmationEmail(sessionKey, emailTemplateId, orderId, subjectOverride);
		}

		public static void DoSendOrderConfirmationEmail(string sessionKey, int templateId,
				int? orderId, string subjectOverride)
		{
			UnsecureTess.SendOrderConfirmationEmail(
					sSessionKey: Mask(sessionKey),
					iTemplateNo: templateId,
					iOrderNo: orderId ?? 0,
					sSubject: subjectOverride ?? String.Empty);
		}

		public static DataSet ExecuteLocalProcedure(string sessionKey, int procedureId)
		{
			return ExecuteLocalProcedure(sessionKey, procedureId, null);
		}

		public static DataSet ExecuteLocalProcedure(string sessionKey, int procedureId,
				Dictionary<string, string> keyValueParams)
		{
			List<string> paramPairs;
			if (keyValueParams != null && keyValueParams.Keys.Count > 0)
			{
				paramPairs = new List<string>();
				foreach (string key in keyValueParams.Keys)
				{
					paramPairs.Add("@" + MaskProcedureParam(key) + "="
							+ MaskProcedureParam(keyValueParams[key]));
				}
			}
			else
			{
				paramPairs = null;
			}
			DataSet result = SecureTess.ExecuteLocalProcedure(
					SessionKey: sessionKey,
					LocalProcedureId: procedureId,
					LocalProcedureValues: paramPairs == null ? "" : String.Join("&", paramPairs));
			return result;
		}

		private static string MaskProcedureParam(string raw)
		{
			return raw.Replace("=", "\\=").Replace("&", "\\&");
		}

		public static void RemoveContribution(string sessionKey, int contId)
		{
			UnsecureTess.RemoveContribuion(
					sWebSessionID: Mask(sessionKey),
					iLineItemNumber: contId);
		}

		public static ProdSeasonCollection GetProdSeasons()
		{
			return DoGetProductions(null, null, null, null, null, null, null, null, null, null,
					null, null, null, null, null, null, null);
		}

		public static ProdSeasonCollection GetProdSeasons(ProdSeasonSearchCriteria criteria)
		{
			return GetProdSeasons(null, criteria);
		}

		public static ProdSeasonCollection GetProdSeasons(string sessionKey,
				ProdSeasonSearchCriteria criteria)
		{
			return DoGetProductions(sessionKey, criteria.StartDateTime, criteria.EndDateTime,
					criteria.ArtistFirstName, criteria.ArtistMiddleName, criteria.ArtistLastName,
					criteria.VenueId, criteria.Keywords, criteria.ModeOfSaleId,
					criteria.BusinessUnit, criteria.FullTextSearchCriteria,
					criteria.FullTextSearchSyntaxType, criteria.ContentTypeIds,
					criteria.MatchAllKeywords, criteria.PerfIds, criteria.SeasonIds, criteria.Ids);
		}

		private static ProdSeasonCollection DoGetProductions(string sessionKey,
				DateTime? startDateTime, DateTime? endDateTime, string artistFirstName,
				string artistMiddleName, string artistLastName, short? venueId, string[] keywords,
				short? modeOfSaleId, int? businessUnit, string fullTextSearchCriteria,
				FullTextSearchSyntaxType? fullTextSearchSyntaxType,
				ContentTypeIdsParam contentTypeIds, bool? matchAllKeywords, int[] perfIds,
				short[] seasonIds, int[] ids)
		{
			int iKeywordAndOrStatementValue;
			if (matchAllKeywords.HasValue)
			{
				iKeywordAndOrStatementValue = matchAllKeywords.Value ? 2 : 1;
			}
			else
			{
				iKeywordAndOrStatementValue = 0;
			}
			string sArtistValue;
			bool hasLastName = !String.IsNullOrWhiteSpace(artistLastName);
			bool hasFirstName = !String.IsNullOrWhiteSpace(artistFirstName);
			bool hasMiddleName = !String.IsNullOrWhiteSpace(artistMiddleName);
			if (hasLastName || hasFirstName || hasMiddleName)
			{
				StringBuilder syntax = new StringBuilder();
				if (hasLastName)
				{
					syntax.Append(artistLastName.Replace("/",String.Empty));
				}
				if (hasFirstName || hasMiddleName)
				{
					syntax.Append("/");
				}
				if (hasFirstName)
				{
					syntax.Append(artistFirstName.Replace("/", String.Empty));
				}
				if (hasMiddleName)
				{
					syntax.Append("/" + artistMiddleName.Replace("/", String.Empty));
				}
				sArtistValue = syntax.ToString();
			}
			else
			{
				sArtistValue = "";
			}
			DataSet results = UnsecureTess.GetProductionsEx3(
				sSessionKey: Mask(sessionKey),
				sStartDate: startDateTime.HasValue ? startDateTime.ToString() : String.Empty,
				sEndDate: endDateTime.HasValue ? endDateTime.ToString() : String.Empty,
				sPerfType: String.Empty,
				sArtist: sArtistValue,
				iVenueID: venueId ?? -1,
				sKeywords: keywords == null ? String.Empty : String.Join(",", keywords),
				iModeOfSale: modeOfSaleId ?? Defaults.ModeOfSaleId,
				iBusinessUnit: businessUnit ?? -1,
				sFullText: fullTextSearchCriteria ?? String.Empty,
				sFullTextType: Mask(fullTextSearchSyntaxType),
				sContentType: contentTypeIds == null ? String.Empty : contentTypeIds.ToString(),
				iKeywordAndOrStatement: iKeywordAndOrStatementValue,
				sPerformanceIds: perfIds == null ? String.Empty : String.Join(",", perfIds),
				sSeasonIds: seasonIds == null ? String.Empty : String.Join(",", seasonIds),
				sProductionSeasonIds: ids == null ? String.Empty : String.Join(",", ids));
			if (results.Tables["Production"].Rows.Count == 0)
			{
				return null;
			}
			return new ProdSeasonCollection(results.Tables["Production"],
					results.Tables["Credit"], results.Tables["Keyword"]);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId)
		{
			return DoGetPerformanceDetailSyos(null, perfId, null, null);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId,
				ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetailSyos(null, perfId, null, contentTypeIds);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId,
				short modeOfSaleId)
		{
			return DoGetPerformanceDetailSyos(null, perfId, modeOfSaleId, null);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetailSyos(null, perfId, modeOfSaleId, contentTypeIds);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId, string sessionKey)
		{
			return DoGetPerformanceDetailSyos(sessionKey, perfId, null, null);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId, string sessionKey,
				ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetailSyos(sessionKey, perfId, null, contentTypeIds);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId, string sessionKey,
				short modeOfSaleId)
		{
			return DoGetPerformanceDetailSyos(sessionKey, perfId, modeOfSaleId, null);
		}

		public static SyosSeatPricing GetSyosSeatPricing(int perfId, string sessionKey,
				short modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			return DoGetPerformanceDetailSyos(sessionKey, perfId, modeOfSaleId, contentTypeIds);
		}

		private static SyosSeatPricing DoGetPerformanceDetailSyos(string sessionKey,
				int perfId, short? modeOfSaleId, ContentTypeIdsParam contentTypeIds)
		{
			DataSet results = UnsecureTess.GetPerformanceDetailWithDiscountingSYOSDataSet(
				SessionKey: Mask(sessionKey),
				iPerf_no: perfId,
				iModeOfSale: modeOfSaleId ?? Defaults.ModeOfSaleId,
				sContentType: contentTypeIds == null ? String.Empty : contentTypeIds.ToString());
			DataTableCollection tables = results.Tables;
			if (tables["Performance"].Rows.Count == 0)
			{
				return null;
			}
			return new SyosSeatPricing(tables["Performance"], tables["PriceTypes"],
					tables["Section"], tables["SectionPriceTypes"]);
		}

		public static ReserveTicketsResult ReserveTickets(string sessionKey, int perfId,
				int zoneId, SeatsPerPriceTypeCollection seatsPerPriceType)
		{
			return ReserveTickets(sessionKey, perfId, zoneId, seatsPerPriceType, null);
		}

		// TODO: Account for failure due to not enough seats being available vs. other circumstances
		public static ReserveTicketsResult ReserveTickets(string sessionKey, int perfId,
				int zoneId, SeatsPerPriceTypeCollection seatsPerPriceType, SpecialRequests requests)
		{
			List<int> priceTypes = new List<int>();
			if (seatsPerPriceType.PriceTypeCount == 1)
			{
				priceTypes.Add(seatsPerPriceType[0].PriceTypeId);
			}
			else
			{
				foreach (SeatsPerPriceType req in seatsPerPriceType)
				{
					for (byte s = 0; s < req.NumOfSeats; s++)
					{
						priceTypes.Add(req.PriceTypeId);
					}
				}
			}
			string sPriceTypeValue = String.Join(",", priceTypes);
			byte numOfSeats = seatsPerPriceType.TotalSeats;
			int seatsReserved = 0;
			ReserveTicketsResult result = ReserveTicketsResult.Success;
			try
			{
				seatsReserved = UnsecureTess.ReserveTicketsEx(
					sWebSessionID: Mask(sessionKey),
					sPriceType: sPriceTypeValue,
					iPerformanceNumber: perfId,
					iNumberOfSeats: numOfSeats,
					iZone: zoneId,
					sSpecialRequests: Mask(requests));
			}
			catch (Exception e)
			{
				if (e.Message.Contains("Could not find seats"))
				{
					result = ReserveTicketsResult.CriteriaNotMet;
				}
				else
				{
					throw e;
				}
			}
			if (seatsReserved == 0)
			{
				result = ReserveTicketsResult.Failed;
			}
			else if (seatsReserved < numOfSeats)
			{
				throw new ApplicationException(
						"Ticket reservation encountered an unexpected partial failure.");
			}
			return result;
		}

		public static SeatOptions GetSeatsForPackage(string sessionKey, int packageId)
		{
			return DoGetSeats(sessionKey, packageId, null, null);
		}

		public static SeatOptions GetSeatsForPackage(string sessionKey, int packageId,
				SeatOptionRestrictions restrictions)
		{
			return DoGetSeats(sessionKey, packageId, null, restrictions);
		}

		public static SeatOptions GetSeatsForPerf(string sessionKey, int perfId)
		{
			return DoGetSeats(sessionKey, null, perfId, null);
		}

		public static SeatOptions GetSeatsForPerf(string sessionKey, int perfId,
				SeatOptionRestrictions restrictions)
		{
			return DoGetSeats(sessionKey, null, perfId, restrictions);
		}

		private static SeatOptions DoGetSeats(string sessionKey, int? packageId, int? perfId,
				SeatOptionRestrictions restrictions)
		{
			string sCheckPriceTypesValue;
			if (restrictions == null || restrictions.RequiredPriceTypes == null)
			{
				sCheckPriceTypesValue = String.Empty;
			}
			else
			{
				if (restrictions.RequiredPriceTypes.IncludeAllPriceTypes)
				{
					sCheckPriceTypesValue = "All";
				}
				else
				{
					sCheckPriceTypesValue = String.Join(",", restrictions.RequiredPriceTypes);
				}
			}
			DataSet results = UnsecureTess.GetSeatsBrief(
				sSessionKey: Mask(sessionKey),
				iPackageNumber: packageId ?? 0,
				iPerformanceNumber: perfId ?? 0,
				sZoneList: restrictions == null || restrictions.ZoneIds == null ?
						String.Empty : String.Join(",", restrictions.ZoneIds),
				sSectionList: restrictions == null || restrictions.SectionIds == null ?
						String.Empty : String.Join(",", restrictions.SectionIds),
				sScreenList: restrictions == null || restrictions.ScreenIds == null ?
						String.Empty : String.Join(",", restrictions.ScreenIds),
				cSummaryOnly: restrictions != null && restrictions.Summarize == true ? 'Y' : 'N',
				cCalcPackageAlloc: restrictions != null
						&& restrictions.CalculatePackageAllocDetails == true ? 'Y' : 'N',
				sCheckPriceTypes: sCheckPriceTypesValue,
				cReturnNonSeats: restrictions != null
						&& restrictions.GetNonSeats == true ? 'Y' : 'N');
			if (results.Tables["S"].Rows.Count == 0)
			{
				return null;
			}
			return new SeatOptions(results.Tables["S"], results.Tables["Section"],
					results.Tables["SeatType"], results.Tables["Allocation"]);
		}

		public static ReserveTicketsResult ReserveTicketsSyos(string sessionKey, int perfId,
				SyosSeatsPerPriceTypeCollection seatsPerPriceType)
		{
			return ReserveTicketsSyos(sessionKey, perfId, seatsPerPriceType, null);
		}

		// TODO: account for results more robustly (esp. insufficient available seats)
		public static ReserveTicketsResult ReserveTicketsSyos(string sessionKey, int perfId,
				SyosSeatsPerPriceTypeCollection seatsPerPriceType, SpecialRequests requests)
		{
			List<int> priceTypes = new List<int>();
			List<int> seatIds = new List<int>();
			if (seatsPerPriceType.PriceTypeCount == 1)
			{
				priceTypes.Add(seatsPerPriceType[0].PriceTypeId);
				SyosSeatsPerPriceType spt = seatsPerPriceType[0];
				for (int c = 0; c < spt.Count; c++)
				{
					seatIds.Add(spt[c]);
				}
			}
			else
			{
				foreach (SyosSeatsPerPriceType spt in seatsPerPriceType)
				{
					for (int c = 0; c < spt.Count; c++)
					{
						priceTypes.Add(spt.PriceTypeId);
						seatIds.Add(spt[c]);
					}
				}
			}
			ReserveTicketsResult result = ReserveTicketsResult.Success;
			int seatsReserved = 0;
			try
			{
				seatsReserved = UnsecureTess.ReserveTicketsSpecifiedSeats(
					sWebSessionID: Mask(sessionKey),
					sPriceType: String.Join(",", priceTypes),
					iPerformanceNumber: perfId,
					iNumberOfSeats: 0,
					iZone: 0,
					sSpecialRequests: Mask(requests),
					RequestedSeats: String.Join(",", seatIds));
			}
			catch (Exception e)
			{
				if (e.Message.Contains("One or more of the selected seats"))
				{
					result = ReserveTicketsResult.Unavailable;
				}
				else
				{
					throw e;
				}
			}
			if (seatsReserved == 0)
			{
				result = ReserveTicketsResult.Failed;
			}
			else if (seatsReserved < seatIds.Count)
			{
				result = ReserveTicketsResult.PartialFailure;
			}
			return result;
		}

		public static void ReleaseTickets(string sessionKey, int perfId, int lineItemId)
		{
			UnsecureTess.ReleaseTickets(
				sWebSessionID: Mask(sessionKey),
				iPerformanceNumber: perfId,
				iLineItemNumber: lineItemId);
		}

		public static ShippingOptions GetShippingOptions(string sessionKey)
		{
			DataSet results = UnsecureTess.GetAddressAndShippingMethod(sessionKey);
			return new ShippingOptions(results.Tables["Address"], results.Tables["ShippingMethod"]);
		}

		public static UpdateAddressResult CreateAddress(string sessionKey, int countryId,
				string stateId, string city, string streetAddress, bool setToPrimary)
		{
			return CreateAddress(sessionKey, countryId, stateId, city, streetAddress, setToPrimary,
					null);
		}

		public static UpdateAddressResult CreateAddress(string sessionKey, int countryId,
				string stateId, string city, string streetAddress, bool setToPrimary,
				AddressCreationParams optionalParams)
		{
			if (optionalParams == null)
			{
				return DoUpdateAddress(
					sessionKey: sessionKey,
					id: 0,
					streetAddress: streetAddress,
					subStreetAddress: null,
					city: city,
					stateId: stateId,
					postalCode: null,
					countryId: countryId,
					phone1: null,
					phone2: null,
					fax: null,
					addressType: null,
					months: null,
					mailPurposeIds: null,
					setAsPrimary: setToPrimary,
					saveInactiveCopyOnChange: false,
					startDate: null,
					endDate: null,
					setAsInactive: null);
			}
			return DoUpdateAddress(
				sessionKey: sessionKey,
				id: 0,
				streetAddress: streetAddress,
				subStreetAddress: optionalParams.SubStreetAddress,
				city: city,
				stateId: stateId,
				postalCode: optionalParams.PostalCode,
				countryId: countryId,
				phone1: optionalParams.Phone1,
				phone2: optionalParams.Phone2,
				fax: optionalParams.Fax,
				addressType: optionalParams.AddressTypeId,
				months: optionalParams.Months,
				mailPurposeIds: optionalParams.MailPurposeIds,
				setAsPrimary: setToPrimary,
				saveInactiveCopyOnChange: false,
				startDate: optionalParams.StartDate,
				endDate: optionalParams.EndDate,
				setAsInactive: null);
		}

		public static UpdateAddressResult UpdateAddress(string sessionKey, int id,
				int countryId, string stateId, int addressTypeId, bool saveInactiveCopy,
				bool setToPrimary)
		{
			return UpdateAddress(sessionKey, id, countryId, stateId, addressTypeId,
					saveInactiveCopy, setToPrimary, null);
		}

		public static UpdateAddressResult UpdateAddress(string sessionKey, int id,
				int countryId, string stateId, int addressTypeId, bool saveInactiveCopy,
				bool setToPrimary, AddressUpdateParams optionalParams)
		{
			if (optionalParams == null)
			{
				return DoUpdateAddress(
					sessionKey: sessionKey,
					id: id,
					streetAddress: null,
					subStreetAddress: null,
					city: null,
					stateId: stateId,
					postalCode: null,
					countryId: countryId,
					phone1: null,
					phone2: null,
					fax: null,
					addressType: addressTypeId,
					months: null,
					mailPurposeIds: null,
					setAsPrimary: setToPrimary,
					saveInactiveCopyOnChange: saveInactiveCopy,
					startDate: null,
					endDate: null,
					setAsInactive: null);

			}
			return DoUpdateAddress(
				sessionKey: sessionKey,
				id: id,
				streetAddress: optionalParams.ClearableFields.StreetAddress,
				subStreetAddress: optionalParams.NonclearableFields.SubStreetAddress,
				city: optionalParams.ClearableFields.City,
				stateId: stateId,
				postalCode: optionalParams.ClearableFields.PostalCode,
				countryId: countryId,
				phone1: optionalParams.ClearableFields.Phone1,
				phone2: optionalParams.ClearableFields.Phone2,
				fax: optionalParams.ClearableFields.Fax,
				addressType: addressTypeId,
				months: optionalParams.ClearableFields.Months,
				mailPurposeIds: optionalParams.PurposeIds.Value,
				setAsPrimary: setToPrimary,
				saveInactiveCopyOnChange: saveInactiveCopy,
				startDate: optionalParams.NonclearableFields.StartDate,
				endDate: optionalParams.NonclearableFields.EndDate,
				setAsInactive: optionalParams.ClearableFields.SetToInactive);
		}

		private static UpdateAddressResult DoUpdateAddress(string sessionKey, int id,
				string streetAddress, string subStreetAddress, string city, string stateId,
				string postalCode, int countryId, string phone1, string phone2, string fax,
				int? addressType, string months, int[] mailPurposeIds, bool setAsPrimary,
				bool saveInactiveCopyOnChange, DateTime? startDate, DateTime? endDate,
				bool? setAsInactive)
		{
			DataSet results = UnsecureTess.UpdateAddressEx(
				sSessionKey: Mask(sessionKey),
				sPhone: Mask(phone1),
				sStreet1: Mask(streetAddress),
				sStreet2: Mask(subStreetAddress),
				sCity: Mask(city),
				sStateProv: Mask(stateId),
				sPostalCode: Mask(postalCode),
				iCountry: countryId,
				sPhone2: Mask(phone2),
				sFax: Mask(fax),
				iAddressNumber: id,
				iAddressType: addressType ?? 0,
				bPrimary: setAsPrimary,
				sStartDate: startDate == null ? String.Empty : startDate.ToString(),
				sEndDate: endDate == null ? String.Empty : endDate.ToString(),
				sMonths: Mask(months),
				sInActive: setAsInactive.HasValue ?
						(setAsInactive == true ? "Y" : "N") : String.Empty,
				bSaveCopyOnChange: saveInactiveCopyOnChange,
				sMailPurposes: mailPurposeIds == null ? String.Empty :
						(mailPurposeIds.Length == 0 ? Defaults.EmptyStringLiteral
							: String.Join(",", mailPurposeIds)));
			return new UpdateAddressResult(results.Tables["Address"]);
		}

		public static EmailAddress CreateEmail(string sessionKey, string address, int? n1n2,
				int addressTypeId, string months, int[] purposeIds, DateTime? startDate,
				DateTime? endDate, int? salutationId, bool? acceptsHtml, bool? allowMarketing,
				bool? setToPrimary)
		{
			return DoUpdateEAddress(sessionKey, 0, address, n1n2, addressTypeId, months,
					purposeIds, setToPrimary, startDate, endDate, salutationId, acceptsHtml,
					allowMarketing, false);
		}

		public static EmailAddress UpdateEmail(string sessionKey, int id, string address,
				int? n1n2, int addressTypeId, string months, int[] purposeIds, DateTime? startDate,
				DateTime? endDate, int? salutationId, bool? acceptsHtml, bool? allowMarketing,
				bool? setToPrimary, bool setToInactive)
		{
			return DoUpdateEAddress(sessionKey, id, address, n1n2, addressTypeId, months,
					purposeIds, setToPrimary, startDate, endDate, salutationId, acceptsHtml,
					allowMarketing, setToInactive);
		}

		private static EmailAddress DoUpdateEAddress(string sessionKey, int id, string address,
				int? n1n2, int addressType, string months, int[] emailPurposeIds,
				bool? setAsPrimary,	DateTime? startDate, DateTime? endDate, int? salutationId,
				bool? acceptsHtml, bool? allowMarketing, bool setAsInactive)
		{
			DataSet results = UnsecureTess.UpdateEAddress(
					sSessionKey: Mask(sessionKey),
					iAddressNumber: id,
					sEmail: Mask(address),
					sPurposes: emailPurposeIds == null ? String.Empty :
							(emailPurposeIds.Length == 0 ? Defaults.EmptyStringLiteral
									: String.Join(",", emailPurposeIds)),
					iAddressType: addressType,
					bInActive: setAsInactive,
					sStartDate: startDate == null ? String.Empty : startDate.ToString(),
					sEndDate: endDate == null ? String.Empty : startDate.ToString(),
					sMonths: Mask(months),
					sPrimaryInd: setAsPrimary.HasValue ?
							(setAsPrimary == true ? "Y" : "N") : String.Empty,
					sMarketInd: allowMarketing.HasValue ?
							(allowMarketing == true ? "Y" : "N") : String.Empty,
					iDefaultSal: salutationId ?? 0,
					sHtmlInd: acceptsHtml.HasValue ?
							(acceptsHtml == true ? "Y" : "N") : String.Empty,
					iN1N2: n1n2 ?? 3);
			return new EmailAddress(results.Tables["EAddress"]);
		}

		public static TessUserInfo ValidateTessituraUser(string sessionKey, string username, string
				password)
		{
			DataSet results = SecureTess.ValidateTessituraUserEx(
				sSessionKey: Mask(sessionKey),
				sTessituraUsername: Mask(username),
				sTessituraPassword: Mask(password));
			return new TessUserInfo(results);
		}
	}
}
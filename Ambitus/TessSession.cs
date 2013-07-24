using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ambitus.Internals;

namespace Ambitus
{
	public class TessSession
	{
		#region Static members

		private static MD5CryptoServiceProvider EncryptionClient;

		public static TessSession Generic { get; private set; }

		protected static ConfigSection Settings
		{
			get
			{
				return ConfigSection.Settings;
			}
		}

		// TODO: needed?
		protected static bool UseSessionCache
		{
			get
			{
				return Settings.Session.UseSessionCache;
			}
		}

		protected static bool DebugCheck
		{
			get
			{
				return Settings.Session.DebugCheckSession;
			}
		}

		static TessSession()
		{
			EncryptionClient = new MD5CryptoServiceProvider();
			Generic = new TessSession();
		}

		#endregion

		#region Instance members

		// Keeping track of the following:

		public string Key { get; protected set; }
		protected string m_ip; // preserved for new session on expiration / checkout
		protected int? m_businessUnit; // preserved for new session on expiration / checkout
		public bool CartExists { get; protected set; } // determines if cart needs transfer on login
		public DateTime? CartExpireTime { get; protected set; }
		public string Username; // preserved for re-login, used to know if logged in, to
								//         know if cart needs transfer on new user login
		protected string Password; // preserved for re-login after login failure
		protected bool PasswordIsPreEncrypted; // for password preservation
		public int? SourceId; // preserve source on login; helps mark non-generic session
		public bool? IsTemporary;

		protected SessionCache SessCache; // holds AccountInfo, Cart, etc.
		

		// TODO: need property?
		public string Ip
		{
			get
			{
				return (m_ip == null ? Settings.Defaults.Ip : m_ip);
			}

			protected set
			{
				m_ip = value;
			}
		}

		// TODO: need property?
		public int BusinessUnit
		{
			get
			{
				return m_businessUnit ?? Settings.Defaults.BusinessUnit;
			}

			protected set
			{
				m_businessUnit = value;
			}
		}

		public bool LoggedIn
		{
			get
			{
				return Username != null;
			}
		}

		public bool ExpectsDefaults // determines whether methods use cached data
		{
			get
			{
				if (SourceId == Generic.SourceId
					&& BusinessUnit == Generic.BusinessUnit)
				{
					return true;
				}
				return false;
			}
		}

		protected TessSession()
				: this(null, null, null, null, null) { }

		public TessSession(string ip)
				: this(ip, null, null, null, null) { }

		public TessSession(int businessUnit)
				: this(null, businessUnit, null, null, null) { }

		public TessSession(string ip, int businessUnit)
				: this(ip, businessUnit, null, null, null) { }

		public TessSession(string ip, string username, string password)
				: this(ip, null, username, password, null) { }

		public TessSession(string ip, string username, string password, int sourceId)
			: this(ip, null, username, password, sourceId) { }

		public TessSession(string username, string password)
				: this(null, null, username, password, null) { }

		public TessSession(string username, string password, int sourceId)
				: this(null, null, username, password, sourceId) { }

		public TessSession(string ip, int? businessUnit, string username, string password,
				int? sourceId)
		{
			Ip = ip ?? Settings.Defaults.Ip;
			BusinessUnit = businessUnit ?? Settings.Defaults.BusinessUnit;
			CartExists = false;
			//ExpectsDefaults = true;
			Key = Tess.GetNewSessionKey(Ip, BusinessUnit);
			if (!(username == null || password == null))
			{
				LoginResult loginResult;
				if (sourceId.HasValue)
				{
					loginResult = Login(username, password, sourceId.Value);
				}
				else
				{
					loginResult = Login(username, password);
				}
				if (loginResult == LoginResult.BadSourceId)
				{
					throw new ApplicationException(
							"The new session's default source ID was invalid.");
				}
				else if (loginResult == LoginResult.BadCredentials)
				{
					throw new ApplicationException(
							"Unable to authenticated the default user for new session.");
				}
			}
			DebugVerifySession();
		}

		protected string EncryptPassword(string password)
		{
			if (Settings.Session.EnablePasswordEncryption)
			{
				string saltedPass = Settings.Session.PasswordSalt + password;
				byte[] originalBytes = ASCIIEncoding.Default.GetBytes(saltedPass);
				byte[] encodedBytes = EncryptionClient.ComputeHash(originalBytes);
				return BitConverter.ToString(encodedBytes).Replace("-", "");
			}
			return password;
		}

		public void DebugVerifySession()
		{
			if (!DebugCheck)
			{
				return;
			}
			bool loggedIn = Tess.LoggedIn(Key);
			if (LoggedIn != loggedIn)
			{
				throw new SessionDiscrepancyException("LoggedIn", LoggedIn, loggedIn);
			}
			if (!loggedIn) return;
			VariableCollection vars = Tess.GetSessionVariables(Key);
			bool isPermanent = vars["Status"].Trim() == "P";
			if (IsTemporary.HasValue && IsTemporary.Value == isPermanent)
			{
				throw new SessionDiscrepancyException(
						"IsTemporary", IsTemporary.Value, !isPermanent);
			}
			if (SourceId.HasValue)
			{
				LoginInfo info = Tess.GetLoginInfo(Key);
				if (SourceId.Value != info.SourceId)
				{
					throw new SessionDiscrepancyException("SourceId", SourceId.Value,
							info.SourceId);
				}
			}
			bool cartExists = Tess.CartExists(Key);
			if (CartExists != cartExists)
			{
				throw new SessionDiscrepancyException("CartExists", CartExists, cartExists);
			}

			// TODO: why this?
			if (IsTemporary.HasValue && IsTemporary.Value) return;

			string username = vars["UID"];
			if (Username != username)
			{
				throw new SessionDiscrepancyException(
						"Username", Username, username ?? String.Empty);
			}
			// TODO: build in more checks here
		}

		public class SessionDiscrepancyException : ApplicationException
		{
			public SessionDiscrepancyException(string key, object localValue, object apiValue)
				: base("An unexpected session discrepancy was discovered for \""
				+ key + "\": [" + localValue.ToString() + "] vs [" + apiValue.ToString() + "]")
			{ }
		}

		public LoginResult Login(string username, string password)
		{
			return Login(username, password, null, null);
		}

		public LoginResult Login(string username, string password, int sourceId)
		{
			return Login(username, password, sourceId, null);
		}

		public LoginResult Login(string username, string password, string promoCode)
		{
			return Login(username, password, null, promoCode);
		}

		private LoginResult Login(string username, string password, int? sourceId, string promoCode)
		{
			/* API gotchas:
			 * - newlogin doesn't preserve source
			 * - failed newlogin deauthenticates session
			 * - newlogin doesn't automatically associate the session's cart with the new user
			 */

			LoginResult result = LoginResult.Success;
			bool userSwitched = false;
			bool mustEncryptPass = true;
			// If the session was already authenticated
			if (Username != null && Password != null)
			{
				// If no new username/password was supplied
				if (username == null && password == null)
				{
					// Prepare to re-login previous user
					username = Username;
					password = Password;
					if (PasswordIsPreEncrypted)
					{
						mustEncryptPass = false;
					}
				}
				// Otherwise flag whether the login is changing
				else if (username != Username)
				{
					userSwitched = true;
				}
			}
/*			// If a specific source ID is requested
			if (sourceId.HasValue)
			{
				// Make sure the source ID is valid
				// TODO: replace with a cached method:
				SourceInfo source = Tess.GetSourceInfo(Key, sourceId.Value);
				if (source == null)
				{
					// And if it's not, flag it in the result, and clear the requested source ID
					result = LoginResult.BadSourceId;
					sourceId = null;
				}
			}
			// If a promo code was supplied
			else*/ if (promoCode != null)
			{
				// Check it for validity
				// TODO: use cached method
				SourceInfo source = Tess.GetSourceInfo(Key, promoCode);
				if (source == null)
				{
					// And if it's not, flag it in the result
					result = LoginResult.BadPromoCode;
				}
				else
				{
					// Otherwise update the requested source ID
					sourceId = source.SourceId;
				}
			}
			// If no source has been requested or found, preserve the previous source
			if (!sourceId.HasValue && SourceId.HasValue)
			{
				sourceId = SourceId;
			}
			string encryptedPassword = mustEncryptPass ? EncryptPassword(password) : password;
			bool succeeded;

			// Attempt to login with prepared values
			if (sourceId.HasValue)
			{
				succeeded = Tess.Login(Key, username, encryptedPassword, sourceId.Value);
			}
			else
			{
				succeeded = Tess.Login(Key, username, encryptedPassword);
			}

			// If login succeeded
			if (succeeded)
			{
				LoginInfo loginInfo = Tess.GetLoginInfo(Key);
				// Check if source was set, and flag it as otherwise in the results
				if (sourceId.HasValue && loginInfo.SourceId != sourceId)
				{
					result = LoginResult.BadSourceId;
				}
				// Check if the account is temporary
				IsTemporary = !loginInfo.IsPermanent;
				// Update the session's tracking values
				Username = username;
				Password = password;
				SourceId = sourceId;
				PasswordIsPreEncrypted = !mustEncryptPass;
				// TODO: update ExpectDefaults here?
				if (userSwitched && CartExists)
				{
					// And transfer the cart if the session's user changed from a previous one
					Tess.TransferCart(Key);
				}
			}
			// If login failed
			else
			{
				result = LoginResult.BadCredentials;
				bool recovered = RecoverFormerAuthentication();
				if (!recovered)
				{
					throw new ApplicationException(
							"Unable to preserve the session's previous credentials on login failure.");
				}
			}
			DebugVerifySession();
			return result;
		}

		protected bool RecoverFormerAuthentication()
		{
			if (Username != null && Password != null)
			{
				// TODO: fix potential infinite loop
				return Login(null, null, null, null) == LoginResult.Success;
			}
			return true;
		}

		public void UpdateLastAccessTime()
		{
			Tess.UpdateLastAccessTime(Key);
		}

		public void Revive()
		{
			CartExists = false;
			CartExpireTime = null;
			string oldKey = Key;
			Key = Tess.GetNewSessionKey(Ip, BusinessUnit);
			Tess.TransferSession(Key, oldKey);
			DebugVerifySession();
		}

		public RegisterResult Register(string firstName, string lastName, string email,
				string username, string password)
		{
			/* API gotchas:
			 * - register doesn't preserve source
			 * - failed register deauthenticates session
			 * - register doesn't automatically associate the session's cart with the new user
			 */
			string encryptedPassword = EncryptPassword(password);
			RegisterResult result;
			if (SourceId.HasValue)
			{
				result = Tess.Register(Key, username, encryptedPassword, email, firstName,
						lastName, SourceId.Value);
			}
			else
			{
				result = Tess.Register(Key, username, encryptedPassword, email, firstName,
						lastName);
			}
			if (result == RegisterResult.Success)
			{
				// Update the session's tracking values
				Username = username;
				Password = password;
				if (CartExists)
				{
					// Transfer the cart if the session's user changed from a previous one
					Tess.TransferCart(Key);
				}
			}
			else
			{
				bool recovered = RecoverFormerAuthentication();
				if (!recovered)
				{
					throw new ApplicationException(
							"Unable to preserve the session's previous credentials on login failure.");
				}
			}
			DebugVerifySession();
			return result;
		}

		public ConstituentAttributeCollection AddAttributes(
				Dictionary<string, string> attValuePairs)
		{
			// TODO: replace with cached method
			AttributeCollection systemAtts = Tess.GetAttributes(Key);
			ConstituentAttributeCollection results = null;
			foreach (string newAttName in attValuePairs.Keys)
			{
				Attribute newAtt = systemAtts[newAttName];
				if (newAtt == null)
				{
					throw new ApplicationException("Unable to find attribute, \""
							+ newAttName + "\".");
				}
				results = Tess.AddAttribute(Key, newAtt.Id.Value, attValuePairs[newAttName]);
			}
			DebugVerifySession();
			return results;
		}

		public UpdateAccountInfoResult UpdateAccountInfo(AccountInfoUpdate newInfo)
		{
			return Tess.UpdateAccountInfo(Key, newInfo);
		}

		public void SendGeneralAccountEmail(string email, int templateId)
		{
			SendCredentialsResult result = Tess.SendCredentials(
					SendCredentialsAction.UseOldPassword, Key, email, false, templateId);
			if (result != SendCredentialsResult.Success)
			{
				throw new ApplicationException("Unable to send the account email: "
						+ result.ToString());
			}
			DebugVerifySession();
		}

		public void Logout()
		{
			Tess.Logout(Key);
			CartExists = false;
			Username = null;
			Password = null;
			SourceId = null;
			IsTemporary = null;
			CartExpireTime = null;
			PasswordIsPreEncrypted = false;
		}

		public SendCredentialsResult SendAccountRecoveryEmail(string email, int templateId)
		{
			AccountInfo info = Tess.GetAccountInfo(Key);
			if (info.Email == null || info.Email != email)
			{
				return Tess.SendCredentials(Key, email, templateId);
			}
			// TODO: do something better in this situation:
			throw new ApplicationException(
					"Attempted to recover the account currently signed in.");
		}

		public bool LoginWithToken(string email, string token)
		{
			bool succeeded;
			if (SourceId.HasValue)
			{
				succeeded = Tess.LoginWithToken(Key, email, token, SourceId.Value);
			}
			else
			{
				succeeded = Tess.LoginWithToken(Key, email, token);
			}
			// If login succeeded
			if (succeeded)
			{
				VariableCollection vars = Tess.GetSessionVariables(Key);
				Username = vars["UID"];
				Password = vars["PWD"];
				PasswordIsPreEncrypted = true;
				IsTemporary = true;
				if (CartExists)
				{
					Tess.TransferCart(Key);
				}
			}
			// If login failed
			else
			{
				bool recovered = RecoverFormerAuthentication();
				if (!recovered)
				{
					throw new ApplicationException(
							"Unable to preserve the session's previous credentials on login failure.");
				}
			}
			DebugVerifySession();
			return succeeded;
		}

		public void UpdatePassword(string password)
		{
			ConstituentInfo info = Tess.GetConstituentInfo(Key, ConstituentInfoType.EmailAddresses);
			string email = info.EmailAddresses.GetPrimary().Address;
			string oldPass = PasswordIsPreEncrypted ? Password : EncryptPassword(Password);
			string newPass = EncryptPassword(password);
			UpdateLoginResult result;
			if (SourceId.HasValue)
			{
				result = Tess.UpdatePasswordAndEmail(Key, Username, oldPass, email,
						newPass, SourceId.Value);
			}
			else
			{
				result = Tess.UpdatePasswordAndEmail(Key, Username, oldPass, email,
						newPass);
			}
			if (result == UpdateLoginResult.Success)
			{
				if (Username != null && Password != null && CartExists)
				{
					Tess.TransferCart(Key);
				}
				IsTemporary = false;
				Password = newPass;
				PasswordIsPreEncrypted = false;
			}
			else
			{
				throw new ApplicationException("Account activation failed unexpectedly.");
			}
			DebugVerifySession();
		}

		public VariableCollection GetSessionVariables()
		{
			return Tess.GetSessionVariables(Key);
		}

		public void UpdateContributionCustomData(int contributionId, byte fieldIndex, string data)
		{
			Tess.UpdateContributionCustomData(Key, contributionId, fieldIndex, data);
		}

		public void AddContribution(decimal amount, int fundId)
		{
			Tess.AddContributionToFund(Key, amount, fundId);
			CartExists = true;
		}

		public Cart GetCart()
		{
			if (CartExists)
			{
				return Tess.GetCart(Key);
			}
			return null;
		}

		public StateProvinceCollection GetStateProvinces(int countryId)
		{
			// TODO: replace with cached version
			return Tess.GetStatesProvinces(countryId);
		}

		public PaymentMethodCollection GetPaymentMethods()
		{
			return Tess.GetPaymentMethods(Key);
		}

		public CountryCollection GetCountries()
		{
			// TODO: replace with cached version
			return Tess.GetCountries();
		}

		public AccountInfo GetAccountInfo()
		{
			return Tess.GetAccountInfo(Key);
		}

		public CheckoutResult Checkout(decimal balance, int shippingMethodId,
				string cardholderName, string cardNum, int cardTypeId, int cardExpMonth,
				int cardExpYear, string securityCode)
		{
			ConstituentInfo info =
					Tess.GetConstituentInfo(Key, ConstituentInfoType.Addresses);
			Tess.SetShippingInfo(Key, info.Addresses.GetPrimary().Id.Value, shippingMethodId);
			CheckoutResult result = Tess.CheckoutWithCreditCard(Key, balance, cardTypeId, cardNum,
					cardholderName,	cardExpMonth, cardExpYear, securityCode);
			return result;
		}

		public void SendOrderConfirmationEmail(int orderId, int emailTemplateId)
		{
			Tess.SendOrderConfirmationEmail(Key, orderId, emailTemplateId);
		}

		public DataSet ExecuteLocalProcedure(int procedureId)
		{
			return Tess.ExecuteLocalProcedure(Key, procedureId);
		}

		public DataSet ExecuteLocalProcedure(int procedureId,
				Dictionary<string, string> keyValueParams)
		{
			return Tess.ExecuteLocalProcedure(Key, procedureId, keyValueParams);
		}

		public void RemoveContribution(int contId)
		{
			int items = CountCartItems();
			Tess.RemoveContribution(Key, contId);
			if (items == 1)
			{
				// TODO: reconcile cart having zero items vs. cart not being started
				CartExists = false;
			}
		}

		protected int CountCartItems()
		{
			if (!CartExists)
			{
				return 0;
			}
			Cart cart = GetCart();
			int items = 0;
			items += ((cart.Contributions == null || cart.Contributions.Count == 0) ?
					0 : cart.Contributions.Count);
			// TODO: add other cart item types here
			return items;
		}

		public bool UpdateSessionSource(int sourceId)
		{
			if (SourceId == sourceId)
			{
				return true;
			}
			// TODO: replace with cached version
			SourceInfo source = Tess.GetSourceInfo(Key, sourceId);
			if (source == null)
			{
				return false;
			}
			Tess.UpdateSessionSource(Key, sourceId);
			SourceId = sourceId;
			return true;
		}

		#endregion
	}
}

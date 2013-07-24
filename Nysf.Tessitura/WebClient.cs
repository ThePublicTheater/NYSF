using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Text.RegularExpressions;
using Nysf.Types;
using System.Collections;

namespace Nysf.Tessitura
{
    /// <summary>
    ///     Exposes optimized wrappers for API method calls.
    /// </summary>
    /// <remarks>
    ///     Wrappers for API functionality manage server-side considerations more robustly than raw
    ///     API method calls.
    /// </remarks>
    public class WebClient
    {
        #region Constants

        public const string TessSessionKeySessionKey = "nysf_Tessitura_SessionKey";
        public const string UsernameSessionKey = "nysf_Tessitura_Username";
        public const string TempLoginSessionKey = "nysf_Tessitura_LoginIsTemporary";
        //public const string CartExpiredSessionKey = "nysf_Tessitura_CartExpired";
        public const string ModeOfSaleSessionKey = "nysf_Tessitura_ModeOfSale";
        public const string CartExpireTimeSessionKey = "nysf_Tessitura_CartExpireTime";
        public const string SyosReservedSeatsSessionKey = "nysf_Tessitura_SyosReservedSeats";
        public const string AccountInfoCacheKeyBase = "nysf_Tessitura_AccountInfo";
        public const string CountriesCacheKey = "nysf_Tessitura_Countries";
        public const string StatesCacheKey = "nysf_Tessitura_States";
        public const string CountriesWithStatesCacheKey = "nysf_Tessitura_CountriesWithStates";
        public const string CountriesWithoutStatesCacheKey =
            "nysf_Tessitura_CountriesWithoutStates";
        public const string PerformancesCacheKey = "nysf_Tessitura_Perfs";
        public const string VenuesCacheKey = "nysf_Tessitura_Venues";
        public const string ProductionCacheKeyBase = "nysf_Tessitura_Production";
        public const string AttributesCacheKey = "nysf_Tessitura_Attributes";
        public const string MemCreditCardTypesCacheKey = "nysf_Tessitura_MemCreditCardTypes";

        #endregion

        #region Variables


        private static TessituraWebApi.Tessitura client;
        private static TessituraWebApi.Tessitura unsecuredClient;
        private static int defaultBusinessUnit;
        private static int defaultLoginType;
        private static int defaultN1N2;
        private static int defaultPromoCode;
        private static short defaultModeOfSale;
        private static string anonymousUsername;
        private static string anonymousPassword;
        private static bool convertUnobfuscatedPasswords;
        private static List<int> invisibleHoldCodes;
        private static int memberTicketPerfId;
        private static int memberTicketZoneId;
        private static int memberTicketPriceTypeId;
        private static int memberTicketModeOfSale;
        private static decimal membershipPrice;
        private static decimal membershipHandlingFee;

        #endregion

        #region Properties

        public static int DefaultSourceCode
        {
            get { return defaultPromoCode; }
        }

        /// <summary>
        ///     Exposes the client for interaction with the Tessitura Web API.
        /// </summary>
        public static TessituraWebApi.Tessitura RawClient
        {
            get { return client; }
        }

        public static decimal MembershipPrice
        {
            get { return membershipPrice; }
        }

        public static decimal MembershipHandlingFee
        {
            get { return membershipHandlingFee; }
        }

        #endregion

        #region Methods

        static WebClient()
        {
            // Check for required entries in the config file

            string[] requiredSettingKeys =
			{
				"nysf_Tessitura_DefaultBusinessUnit",
				"nysf_Tessitura_DefaultLoginType",
				"nysf_Tessitura_DefaultN1N2",
				"nysf_Tessitura_DefaultPromoCode",
				"nysf_Tessitura_DefaultWebModeOfSale",
				"nysf_Tessitura_AnonymousUsername",
				"nysf_Tessitura_AnonymousPassword",
				"nysf_Tessitura_ConvertUnobfuscatedPasswords",
				"nysf_Tessitura_SeatMapInvisHoldCodes",
				"nysf_Tessitura_CartExpirationMins",
				"nysf_Tessitura_MemTicketPerf",
				"nysf_Tessitura_MemTicketZone",
				"nysf_Tessitura_MemTicketPriceType",
				"nysf_Tessitura_MemTicketMos",
				"nysf_Tessitura_MembershipPrice",
				"nysf_Tessitura_MembershipHandlingFee"
			};
            Utility.VerifyAppSettings(requiredSettingKeys);

            // Initialize the web API client

            client = new TessituraWebApi.Tessitura();

            // Initialize settings variables

            NameValueCollection settings = WebConfigurationManager.AppSettings;
            defaultBusinessUnit = int.Parse(settings["nysf_Tessitura_DefaultBusinessUnit"]);
            defaultLoginType = int.Parse(settings["nysf_Tessitura_DefaultLoginType"]);
            defaultN1N2 = int.Parse(settings["nysf_Tessitura_DefaultN1N2"]);
            defaultPromoCode = int.Parse(settings["nysf_Tessitura_DefaultPromoCode"]);
            defaultModeOfSale = short.Parse(settings["nysf_Tessitura_DefaultWebModeOfSale"]);
            anonymousUsername = settings["nysf_Tessitura_AnonymousUsername"];
            anonymousPassword = settings["nysf_Tessitura_AnonymousPassword"];
            convertUnobfuscatedPasswords =
                Convert.ToBoolean(settings["nysf_Tessitura_ConvertUnobfuscatedPasswords"]);
            memberTicketPerfId = int.Parse(settings["nysf_Tessitura_MemTicketPerf"]);
            memberTicketZoneId = int.Parse(settings["nysf_Tessitura_MemTicketZone"]);
            memberTicketPriceTypeId = int.Parse(settings["nysf_Tessitura_MemTicketPriceType"]);
            memberTicketModeOfSale = int.Parse(settings["nysf_Tessitura_MemTicketMos"]);
            membershipPrice = decimal.Parse(settings["nysf_Tessitura_MembershipPrice"]);
            membershipHandlingFee = decimal.Parse(settings["nysf_Tessitura_MembershipHandlingFee"]);

            string[] invisHoldCodeStrings = Regex.Replace(
                settings["nysf_Tessitura_SeatMapInvisHoldCodes"], "\\s+", "").Split(',');
            invisibleHoldCodes = new List<int>();
            foreach (string hc in invisHoldCodeStrings)
            {
                int newHc = -1;
                if (Int32.TryParse(hc, out newHc) && !invisibleHoldCodes.Contains(newHc))
                    invisibleHoldCodes.Add(newHc);
            }

            // Prepare unsecured vs. secured API URLs

            if (client.Url.StartsWith("https://"))
            {
                unsecuredClient = new TessituraWebApi.Tessitura();
                unsecuredClient.Url = client.Url.Replace("https://", "http://");
            }
            else if (client.Url.StartsWith("http://"))
            {
                unsecuredClient = client;
                client = new TessituraWebApi.Tessitura();
                client.Url = client.Url.Replace("http://", "https://");
            }
            else
            {
                throw new NotSupportedException("The URL to the Tessitura Web API must start with "
                    + "either \"http://\" or \"https://\".");
            }
        }

        public static bool Login(string userIdentifier, string password)
        {
            return Login(userIdentifier, password, defaultPromoCode);
        }

        /// <summary>
        ///     Attempts to authenticate the current Tessitura session.
        /// </summary>
        /// <param name="userIdentifier">
        ///     A System.String object containing the constituent's email address.
        /// </param>
        /// <param name="password">
        ///     A System.String object containing the constituent's password.
        /// </param>
        public static bool Login(string userIdentifier, string password, int sourceCode)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            string tessSessionKey =
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
            DataSet tessSessionVariables = null;
            bool isTemporary = false;

            // Attempt to log into Tessitura using an obfuscated password

            bool loginWasSuccessful =
                client.LoginEx2(
                    tessSessionKey,
                    userIdentifier,
                    Utility.MakeObfuscation(password),
                    defaultLoginType,
                    sourceCode,
                    "",
                    "",
                    "",
                    0,
                    defaultN1N2,
                    "");

            // Attempt to log into Tessitura using a plain password

            if (!loginWasSuccessful)
            {
                loginWasSuccessful = client.LoginEx2(
                    tessSessionKey,
                    userIdentifier,
                    password,
                    defaultLoginType,
                    sourceCode,
                    "",
                    "",
                    "",
                    0,
                    defaultN1N2,
                    "");
                if (loginWasSuccessful && convertUnobfuscatedPasswords)
                {
                    // Updates user's password to obfuscated version

                    string oldUsername = null;
                    string oldPassword = null;
                    string oldEmail = null;
                    int loginType = defaultLoginType;
                    DataSet results = client.GetVariables(tessSessionKey);
                    foreach (DataRow row in results.Tables["SessionVariable"].Rows)
                    {
                        if (row["name"].ToString() == "UID")
                            oldUsername = row["value"].ToString();
                        if (row["name"].ToString() == "PWD")
                            oldPassword = row["value"].ToString();
                        if (row["name"].ToString() == "LoginType")
                            loginType = Convert.ToInt32(row["value"]);
                        if (row["name"].ToString() == "Status")
                            isTemporary = Convert.ToChar(row["value"]) == 'T';
                    }
                    results = unsecuredClient.GetAccountInfo(tessSessionKey);
                    oldEmail = results.Tables[0].Rows[0]["email"].ToString();
                    client.UpdateLogin(
                        tessSessionKey,
                        oldUsername,
                        oldUsername,
                        oldPassword,
                        Utility.MakeObfuscation(oldPassword),
                        oldEmail,
                        oldEmail,
                        loginType);
                }
            }

            if (loginWasSuccessful)
            {
                // TODO: consider: should "username" be stored in a browser session variable?
                //       maybe store boolean "isAuthenticated" instead?
                HttpContext.Current.Session.Add(UsernameSessionKey, userIdentifier);
                // Run TransferCart() to resolve any discrepancy with the cart owner (usually when
                // a cart was started under the anonymous user, and the session has been
                // reauthenticated).

                if (CartHasItems())
                    unsecuredClient.TransferCart(tessSessionKey);

                // Check if account is temporary

                if (tessSessionVariables == null)
                {
                    tessSessionVariables = client.GetVariables(tessSessionKey);
                    foreach (DataRow row in tessSessionVariables.Tables["SessionVariable"].Rows)
                    {
                        if (row["name"].ToString() == "Status")
                            isTemporary = Convert.ToChar(row["value"]) == 'T';
                    }
                }
                if (isTemporary)
                    HttpContext.Current.Session.Add(TempLoginSessionKey, true);
            }
            else
                HttpContext.Current.Session.Remove(UsernameSessionKey);

            return loginWasSuccessful;
        }

        /// <summary>
        ///     Unauthenticates the current Tessitura session and clears relevant browser session
        ///     variables.
        /// </summary>
        public static void Logout()
        {
            
            System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;
            if (session[TessSessionKeySessionKey] != null)
                unsecuredClient.Logout(session[TessSessionKeySessionKey].ToString());
            session.Remove(UsernameSessionKey);
            session.Remove(TempLoginSessionKey);
            //session.Remove(CartExpiredSessionKey);
            session.Remove(CartExpireTimeSessionKey);
            session[ModeOfSaleSessionKey] = defaultModeOfSale;
        }

        public static void MaintainTessSession()
        {
            MaintainTessSession(defaultPromoCode);
        }

        /// <summary>
        ///     Guarantees that a valid Tessitura session is started.
        /// </summary>
        /// <remarks>
        ///     Performs several important checks:
        ///         1. That a session key has been acquired.
        ///         2. That the session's last-access-time is updated.
        ///         3. That the session's cart is not expired.
        /// </remarks>
        public static void MaintainTessSession(int sourceCode)
        {
            bool newSessionKeyRequired = false;
            string oldSessionKey = null;
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                newSessionKeyRequired = true;
            else
            {
                oldSessionKey =
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString();

                // Update the last access time

                unsecuredClient.UpdateLastAccessTime(
                    oldSessionKey,
                    DateTime.Now.ToString());

                // Check if an expired cart exists on the session

                if (HttpContext.Current.Session[UsernameSessionKey] != null
                    && GetCart() == null)
                {
                    //HttpContext.Current.Session.Add(CartExpiredSessionKey, true);
                    HttpContext.Current.Session.Remove(CartExpireTimeSessionKey);
                    newSessionKeyRequired = true;
                }
            }

            // Getting a new key when either none were acquired before, or a previously-acquired key
            // had expired.

            if (newSessionKeyRequired)
            {
                string newSessionKey = unsecuredClient.GetNewSessionKeyEx(
                    HttpContext.Current.Request.UserHostAddress,
                    defaultBusinessUnit);
                HttpContext.Current.Session.Add(TessSessionKeySessionKey, newSessionKey);
                HttpContext.Current.Session.Add(ModeOfSaleSessionKey, defaultModeOfSale);

                // Transfer the session if updating from an old (expired) session

                if (oldSessionKey != null)
                    unsecuredClient.TransferSession(oldSessionKey, newSessionKey);
            }

            // If the session is not authenticated, login as anonymous.

            if (HttpContext.Current.Session[UsernameSessionKey] == null)
            {
                bool anonymousLoginWasSuccessful =
                    Login(anonymousUsername, anonymousPassword, sourceCode);
                if (!anonymousLoginWasSuccessful)
                    throw new ApplicationException("Unable to log into Tessitura with the "
                        + "anonymous username and password specified in web.config.");
            }
        }
        // TODO: consider: is there ever a time to make exceptionOnSessionError false?
        /// <summary>
        ///     Determines whether then cart has any items.
        /// </summary>
        /// <param name="exceptionOnSessionError">
        ///     Whether the method should throw an exception when lacking an authenticated session.
        /// </param>
        /// <remarks>
        ///     This is useful for determining whether or not to call TransferCart() when
        ///     reauthenticating from an anonymous user to a registered constituent.
        /// </remarks>
        public static bool CartHasItems(bool exceptionOnSessionError)
        {
            bool cartHasItems = false;
            if (!MaintainedSessionExists())
            {
                if (exceptionOnSessionError)
                    throw new ApplicationException("CartHasItems() was called out of sequence. A "
                        + "maintained Tessitura session is required. Try calling "
                        + "MaintainTessSession() first.");
            }
            else
            {
                Cart cart = GetCart(exceptionOnSessionError);
                cartHasItems = (cart != null) && (cart.HasItems);
            }
            return cartHasItems;
        }

        /// <summary>
        ///     Determines whether then cart has any items.
        /// </summary>
        /// <remarks>
        ///     This is useful for determining whether or not to call TransferCart() when
        ///     reauthenticating from an anonymous user to a registered constituent.
        /// </remarks>
        public static bool CartHasItems()
        {
            return CartHasItems(true);
        }

        // TODO: consider: is there ever a time to set exceptionOnSessionError to false?
        /// <summary>
        ///     Gets the cart for the current session, or returns null for an expired cart.
        /// </summary>
        /// <param name="exceptionOnSessionError">
        ///     Whether the method should throw an exception when lacking an authenticated session.
        /// </param>
        public static Cart GetCart(bool exceptionOnSessionError)
        {
            DataSet cartResults = null;
            if (!MaintainedSessionExists())
            {
                if (exceptionOnSessionError)
                    // TODO: Check if it's safe to call MaintainTessSession() automatically
                    throw new ApplicationException("GetCart() was called out of sequence. A "
                        + "maintained Tessitura session is required. Try calling "
                        + "MaintainTessSession() first.");
            }
            else
            {
                try
                {
                    cartResults = unsecuredClient.GetCart(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
                }
                catch (Exception exception)
                {
                    if (exception.Message.Contains("TESSITURA_ACCESS_DENIED_EXCEPTION"))
                    {
                        Logout();
                        //HttpContext.Current.Response.Redirect("/");
                    }
                    else if (!exception.Message.Contains("TESSITURA_SEAT_LOCKING_EXCEPTION"))
                        throw exception;
                }
            }
            //            if (cartResults != null && cartResults.Tables["Order"] != null
            //                && cartResults.Tables["LineItem"] != null)
            if (cartResults != null && cartResults.Tables["Order"].Rows.Count > 0)
            {
                Cart cart = new Cart();
                cart.Id = Convert.ToInt32(cartResults.Tables["Order"].Rows[0]["order_no"]);
                cart.SeatGroups = new List<CartSeatGroupItem>();
                DataRowCollection lineItemResults = cartResults.Tables["LineItem"].Rows;
                foreach (DataRow lineItemResult in lineItemResults)
                {
                    CartSeatGroupItem seatGroup = new CartSeatGroupItem();
                    seatGroup.Id = Convert.ToInt32(lineItemResult["li_seq_no"]);
                    seatGroup.Performance = GetPerformance(Convert.ToInt32(lineItemResult["perf_no"]));
                    seatGroup.SeatingZoneId = Convert.ToInt32(lineItemResult["zone_no"]);
                    seatGroup.SeatingZoneName = null;
                    seatGroup.SeatsPerPriceTypes = new List<CartPriceTypeSeats>();
                    DataRowCollection subLineItemResults = cartResults.Tables["SubLineItem"].Rows;
                    foreach (DataRow subLineItemResult in subLineItemResults)
                    {
                        if (subLineItemResult["li_seq_no"].ToString()== lineItemResult["li_seq_no"].ToString())
                        {
                            if (seatGroup.SeatingZoneName == null)
                            {
                                seatGroup.SeatingZoneName =
                                    subLineItemResult["zone_desc"].ToString();
                            }
                            int foundIndex = -1;
                            for (int c = 0; c < seatGroup.SeatsPerPriceTypes.Count; c++)
                            {
                                if (seatGroup.SeatsPerPriceTypes[c].PriceTypeId
                                    == Convert.ToInt32(subLineItemResult["price_type"]))
                                {
                                    foundIndex = c;
                                    break;
                                }
                            }
                            CartPriceTypeSeats priceTypeSeats = new CartPriceTypeSeats();
                            priceTypeSeats.PriceTypeId =Convert.ToInt32(subLineItemResult["price_type"]);
                            foreach (DataRow priceTypeResult in cartResults.Tables["PriceType"].Rows)
                            {
                                if (Convert.ToInt32(priceTypeResult["price_type"])== priceTypeSeats.PriceTypeId)
                                {
                                    priceTypeSeats.PriceTypeName =priceTypeResult["description"].ToString();
                                    priceTypeSeats.PriceTypeIsDefault =Convert.ToChar(priceTypeResult["def_price_type"])== 'Y';
                                    break;
                                }
                            }
                            priceTypeSeats.PricePerSeat = Convert.ToDouble(subLineItemResult["due_amt"]);
                            priceTypeSeats.SeatCount = 1;
                            priceTypeSeats.Seats = new List<CartSeat>();
                            CartSeat newSeat = new CartSeat();
                            newSeat.RowIdentifier = subLineItemResult["seat_row"].ToString();
                            newSeat.SeatNumInRow = Convert.ToInt32(subLineItemResult["seat_num"]);
                            newSeat.Id = Convert.ToInt32(subLineItemResult["seat_no"]);
                            newSeat.Fees = new List<CartFee>();
                            foreach (DataRow feeResult in cartResults.Tables["SubLineItemFee"].Rows)
                            {
                                if (Convert.ToInt32(subLineItemResult["sli_no"])
                                    == Convert.ToInt32(feeResult["sli_no"]))
                                {
                                    newSeat.Fees.Add(new CartFee
                                    {
                                        Amount = Convert.ToDouble(feeResult["fee_amt"]),
                                        Description = feeResult["category_desc"].ToString()
                                    });
                                }
                            }
                            priceTypeSeats.Seats.Add(newSeat);
                            if (foundIndex == -1)
                            {
                                seatGroup.SeatsPerPriceTypes.Add(priceTypeSeats);
                            }
                            else
                            {
                                seatGroup.SeatsPerPriceTypes[foundIndex].Absorb(priceTypeSeats);
                            }
                        }
                    }
                    cart.SeatGroups.Add(seatGroup);
                }
                return cart;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     Gets the cart for the current session, or returns null for an expired cart.
        /// </summary>
        public static Cart GetCart()
        {
            return GetCart(true);
        }



        /// <summary>
        ///     Changes the authenticated user's password and removes temporary status.
        /// </summary>
        /// <param name="inputPassword">
        ///     A System.String object containing the current password.
        /// </param>
        /// <param name="newPassword">
        ///     A System.String object containing the new password to set.
        /// </param>
        public static bool SetNewPassword(string inputPassword, string newPassword)
        {
            if (!IsLoggedIn())
                throw new ApplicationException("SetNewPassword() was called out of sequence. An "
                    + "authenticated Tessitura session is required.");

            System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;

            // Prepare the user's login information to be updated

            string oldUsername = session[UsernameSessionKey].ToString();
            string oldPassword = null;
            string oldEmail = null;
            int loginType = defaultLoginType;
            DataSet results =
                client.GetVariables(session[TessSessionKeySessionKey].ToString());
            foreach (DataRow row in results.Tables["SessionVariable"].Rows)
            {
                if (row["name"].ToString() == "PWD")
                    oldPassword = row["value"].ToString();
                if (row["name"].ToString() == "LoginType")
                    loginType = Convert.ToInt32(row["value"]);
            }

            if (oldPassword != inputPassword)
                return false;

            results = unsecuredClient.GetAccountInfo(session[TessSessionKeySessionKey].ToString());
            oldEmail = results.Tables["AccountInformation"].Rows[0]["email"].ToString();

            // Update the user's login information

            if (convertUnobfuscatedPasswords)
            {
                newPassword = Utility.MakeObfuscation(newPassword);
            }
            client.UpdateLogin(
                session[TessSessionKeySessionKey].ToString(),
                oldUsername,
                oldUsername,
                oldPassword,
                newPassword,
                oldEmail,
                oldEmail,
                loginType);
            session.Remove(TempLoginSessionKey);
            Logout(); // TODO: is this necessary?
            Login(oldUsername, newPassword); // TODO: is this necessary?
            return true;
        }

        /// <summary>
        ///     Sets a new password for the authenticated account and removes temporary flag.
        /// </summary>
        /// <param name="newPassword">
        ///     A System.String object containing the new password to set.
        /// </param>
        /// <remarks>
        ///     This method is to be used for activation of new and recovered accounts, which have
        ///     temporary passwords.
        /// </remarks>
        public static bool SetNewPassword(string newPassword)
        {
            if (!IsLoggedIn())
                throw new ApplicationException("SetNewPassword() was called out of sequence. An "
                    + "authenticated Tessitura session is required.");

            System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;

            // Get the user's password

            string oldPassword = null;
            DataSet results =
                client.GetVariables(session[TessSessionKeySessionKey].ToString());
            foreach (DataRow row in results.Tables["SessionVariable"].Rows)
            {
                if (row["name"].ToString() == "PWD")
                    oldPassword = row["value"].ToString();
            }

            return SetNewPassword(oldPassword, newPassword);
        }

        /// <summary>
        ///     Sets a new email address (and username) for the authenticated account.
        /// </summary>
        /// <param name="newEmail">
        ///     The email address to be set.
        /// </param>
        public static bool SetNewEmail(string newEmail)
        {
            if (!IsLoggedIn())
                throw new ApplicationException("SetNewPassword() was called out of sequence. An "
                    + "authenticated Tessitura session is required.");

            System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;

            // Prepare the user's login information to be updated

            string oldUsername = session[UsernameSessionKey].ToString();
            string oldPassword = null;
            string oldEmail = null;
            int loginType = defaultLoginType;
            DataSet results =
                client.GetVariables(session[TessSessionKeySessionKey].ToString());
            foreach (DataRow row in results.Tables["SessionVariable"].Rows)
            {
                if (row["name"].ToString() == "PWD")
                    oldPassword = row["value"].ToString();
                if (row["name"].ToString() == "LoginType")
                    loginType = Convert.ToInt32(row["value"]);
            }
            results = unsecuredClient.GetAccountInfo(session[TessSessionKeySessionKey].ToString());
            oldEmail = results.Tables["AccountInformation"].Rows[0]["email"].ToString();

            // Update the user's login information

            bool wasSuccessful = false;
            try
            {
                wasSuccessful = client.UpdateLogin(
                session[TessSessionKeySessionKey].ToString(),
                oldUsername,
                newEmail.Trim(),
                oldPassword,
                oldPassword,
                oldEmail,
                newEmail.Trim(),
                loginType);
            }
            catch (Exception e)
            {
                if (!e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION"))
                    throw e;
                else
                {
                    client.Login(
                        session[TessSessionKeySessionKey].ToString(),
                        oldUsername,
                        oldPassword,
                        loginType);
                }
            }
            return wasSuccessful;
        }

        /// <summary>
        ///     Sends an account recovery message with a temporary password to the email address
        ///     specified, or returns false to indicate that the account could not be found.
        /// </summary>
        /// <param name="emailAddress">
        ///     The email address of the account to recover.
        /// </param>
        /// <remarks>
        ///     The Tessitura Web API method, SendCredentials(), logs out the session, so Logout()
        ///     must be called to make the browser session consistent with the API session.
        /// </remarks>
        public static bool SendRecoveryEmail(string emailAddress, int emailTemplateNumber)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            bool wasSuccessful = false;
            try
            {
                wasSuccessful = unsecuredClient.SendCredentials(
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                emailAddress,
                defaultLoginType,
                true,
                false,
                true,
                emailTemplateNumber);
            }
            catch (Exception exception)
            {
                if (!exception.Message.Contains("TESSITURA_LOGIN_NOT_FOUND_EXCEPTION")
                    // Not sure why, but sometimes the API throws this exception:
                    && !exception.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION"))
                    throw exception;
            }
            if (wasSuccessful)
                Logout();
            return wasSuccessful;
        }

        /// <summary>
        ///     Attempts to register a new constituent.
        /// </summary>
        /// <returns>
        ///     Whether or not the new constituent could be registered.
        /// </returns>
        public static bool RegisterNewConstituent(string emailAddress, string givenName,
            string surname, int emailTemplateNumber, Organization sourceOrg)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();

            // Check for suspiciously similar registrations

            DataSet result = unsecuredClient.ExecuteLocalProcedure(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                LocalProcedureId: 8006,
                LocalProcedureValues: "@ip=" + HttpContext.Current.Request.UserHostAddress
                                       + "&@email=" + emailAddress
                                       + "&@firstName=" + givenName
                                       + "&@lastName=" + surname
                                       + "&@daySpan=" + "60"
                                       + "&@sensitivity=" + "2"); // TODO: set from web.config
            if (!Convert.ToBoolean(result.Tables["LocalProcedure"].Rows[0][0]))
            {
                Logout();
                return false;
            }

            bool registrationWasSuccessful = false;
            try
            {
                registrationWasSuccessful =
                    client.RegisterWithPromoCode(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        "temp",
                        defaultLoginType,
                        emailAddress,
                        givenName,
                        surname,
                        defaultPromoCode);
            }
            catch (Exception e)
            {
                // Allow the TESSITURA_DUPLICATE_LOGIN_EXCEPTION to pass without
                // halting the application

                if (!e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION")
                    && !e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
                    throw e;
            }

            if (registrationWasSuccessful)
            {
                string orgAttName;
                switch (sourceOrg)
                {
                    case Organization.PublicTheater:
                        orgAttName = "The Public Theater";
                        break;
                    case Organization.JoesPub:
                        orgAttName = "Joe’s Pub";
                        break;
                    case Organization.ShakespeareInThePark:
                        orgAttName = "Shakespeare in the Park";
                        break;
                    default:
                        orgAttName = "";
                        break;
                }
                Nysf.Tessitura.AttributeCollection attCol = GetAttributes();
                Nysf.Tessitura.Attribute att = attCol.GetAttributeByName(
                    "cp_Em_" + orgAttName);
                AddAttribute(att.Id, 1);

                // Send the account activation email
                if (emailTemplateNumber >= 0)
                {

                    unsecuredClient.SendCredentials(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        defaultLoginType,
                        true,
                        false,
                        true,
                        emailTemplateNumber);

                    unsecuredClient.ExecuteLocalProcedure(
                        SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        LocalProcedureId: 8015,
                        LocalProcedureValues: "@ip=" + "192.168.92.40"
                        // + HttpContext.Current.Request.UserHostAddress
                                               + "&@email=" + emailAddress
                                               + "&@firstName=" + givenName
                                               + "&@lastName=" + surname);
                }
            }

            Logout();
            MaintainTessSession();

            return registrationWasSuccessful;
        }



        public static bool RegisterNewConstituentWithPassword(string emailAddress, string password, string givenName,
           string surname, int emailTemplateNumber, Organization sourceOrg)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            string tessSessionKey =
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
            // Check for suspiciously similar registrations

            DataSet result = unsecuredClient.ExecuteLocalProcedure(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                LocalProcedureId: 8006,
                LocalProcedureValues: "@ip=" + HttpContext.Current.Request.UserHostAddress
                                       + "&@email=" + emailAddress
                                       + "&@firstName=" + givenName
                                       + "&@lastName=" + surname
                                       + "&@daySpan=" + "60"
                                       + "&@sensitivity=" + "2"); // TODO: set from web.config
            if (!Convert.ToBoolean(result.Tables["LocalProcedure"].Rows[0][0]))
            {
                Logout();
                return false;
            }

            bool registrationWasSuccessful = false;
            try
            {
                registrationWasSuccessful =
                    client.RegisterWithPromoCode(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        password,
                        defaultLoginType,
                        emailAddress,
                        givenName,
                        surname,
                        defaultPromoCode);
            }
            catch (Exception e)
            {
                // Allow the TESSITURA_DUPLICATE_LOGIN_EXCEPTION to pass without
                // halting the application

                if (!e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION")
                    && !e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
                    throw e;
            }

            if (registrationWasSuccessful)
            {
                string orgAttName;
                switch (sourceOrg)
                {
                    case Organization.PublicTheater:
                        orgAttName = "The Public Theater";
                        break;
                    case Organization.JoesPub:
                        orgAttName = "Joe’s Pub";
                        break;
                    case Organization.ShakespeareInThePark:
                        orgAttName = "Shakespeare in the Park";
                        break;
                    default:
                        orgAttName = "";
                        break;
                }
                if (orgAttName.Length > 0)
                {
                    Nysf.Tessitura.AttributeCollection attCol = GetAttributes();
                    Nysf.Tessitura.Attribute att = attCol.GetAttributeByName(
                        "cp_Em_" + orgAttName);
                    AddAttribute(att.Id, 1);
                }

                HttpContext.Current.Session.Add(UsernameSessionKey, emailAddress);
                // Run TransferCart() to resolve any discrepancy with the cart owner (usually when
                // a cart was started under the anonymous user, and the session has been
                // reauthenticated).

                if (CartHasItems())
                    unsecuredClient.TransferCart(tessSessionKey);

                // Send the account activation email
                if (emailTemplateNumber >= 0)
                {

                    unsecuredClient.SendCredentials(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        defaultLoginType,
                        true,
                        false,
                        true,
                        emailTemplateNumber);

                    unsecuredClient.ExecuteLocalProcedure(
                        SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        LocalProcedureId: 8015,
                        LocalProcedureValues: "@ip=" + "192.168.92.40"
                        // + HttpContext.Current.Request.UserHostAddress
                                               + "&@email=" + emailAddress
                                               + "&@firstName=" + givenName
                                               + "&@lastName=" + surname);
                }
            }

           
            MaintainTessSession();

            return registrationWasSuccessful;
        }



        // TODO: have this store greetingnames in session variable or cache, or have
        //       GetAccountInformation() store in cache and retrieve from that
        // TODO: make sure this works when first names are empty
        public static string BuildGreetingNames()
        {
            if (!IsLoggedIn())
                throw new ApplicationException("BuildGreetingNames() was called out of "
                    + "sequence. An authenticated Tessitura session is required.");

            StringBuilder greetingNames = new StringBuilder();

            AccountInfo info = GetAccountInformation();
            AccountPerson person1 = info.People.Person1;
            AccountPerson person2 = info.People.Person2;

            bool lastNamesAreSame = (person2 == null || person2.LastName == null) ?
                false : person1.LastName == person2.LastName;

            // Append the salutation name for contact 1

            if (person1 != null)
            {
                bool prefixWasApplied = false; // Will help determine if given names
                // should be applied
                bool properNounWasApplied = false; // Determines if a space should be added
                // before the middle and last names

                // If a last name exists, try to apply prefix

                if (!String.IsNullOrWhiteSpace(person1.LastName))
                {
                    if (!String.IsNullOrWhiteSpace(person1.Prefix))
                    {
                        greetingNames.Append(person1.Prefix + " ");
                        prefixWasApplied = true;
                    }
                    else if (person1.Gender != null)
                    {
                        if (person1.Gender == 'M')
                        {
                            greetingNames.Append("Mr. ");
                            prefixWasApplied = true;
                        }
                        else if (person1.Gender == 'F')
                        {
                            greetingNames.Append("Ms. ");
                            prefixWasApplied = true;
                        }
                    }
                }

                // If no last name exists, or if a prefix couldn't be determined, apply
                // given names

                if (String.IsNullOrWhiteSpace(person1.LastName) || !prefixWasApplied)
                {
                    if (!String.IsNullOrWhiteSpace(person1.FirstName))
                    {
                        greetingNames.Append(person1.FirstName);
                        properNounWasApplied = true;
                    }
                    if (!String.IsNullOrWhiteSpace(person1.MiddleName))
                    {
                        if (properNounWasApplied)
                            greetingNames.Append(" ");
                        greetingNames.Append(person1.MiddleName);
                        properNounWasApplied = true;
                    }
                }

                // If the second contact doesn't exist, or has a different last name,
                // apply last name

                if (!String.IsNullOrWhiteSpace(person1.LastName)
                    && (person2 == null || !lastNamesAreSame))
                {
                    if (properNounWasApplied)
                        greetingNames.Append(" ");
                    greetingNames.Append(person1.LastName);
                }
            }

            // Append "and" between names if necessary
            if (person1 != null && person2 != null)
                greetingNames.Append(" and ");

            // Append the salutation name for contact 2

            if (person2 != null)
            {
                bool prefixWasApplied = false; // Will help determine if given names
                // should be applied
                bool properNounWasApplied = false; // Determines if a space should be added
                // before the middle and last names

                // If a last name exists, try to apply prefix

                if (!String.IsNullOrWhiteSpace(person2.LastName))
                {
                    if (!String.IsNullOrWhiteSpace(person2.Prefix))
                    {
                        greetingNames.Append(person2.Prefix + " ");
                        prefixWasApplied = true;
                    }
                    else if (person2.Gender != null)
                    {
                        if (person2.Gender == 'M')
                        {
                            greetingNames.Append("Mr. ");
                            prefixWasApplied = true;
                        }
                        else if (person2.Gender == 'F')
                        {
                            greetingNames.Append("Ms. ");
                            prefixWasApplied = true;
                        }
                    }
                }

                // If no last name exists, or if a prefix couldn't be determined, apply
                // given names

                if (String.IsNullOrWhiteSpace(person2.LastName) || !prefixWasApplied)
                {
                    if (!String.IsNullOrWhiteSpace(person2.FirstName))
                    {
                        greetingNames.Append(person2.FirstName);
                        properNounWasApplied = true;
                    }
                    if (!String.IsNullOrWhiteSpace(person2.MiddleName))
                    {
                        if (properNounWasApplied)
                            greetingNames.Append(" ");
                        greetingNames.Append(person2.MiddleName);
                        properNounWasApplied = true;
                    }
                }

                // Try to append the last name

                if (!String.IsNullOrWhiteSpace(person2.LastName))
                {
                    if (properNounWasApplied)
                        greetingNames.Append(" ");
                    greetingNames.Append(" " + person2.LastName);
                }
            }
            return greetingNames.ToString();
        }

        public static DataSet GetCountries()
        {
            DataSet countries = (DataSet)HttpContext.Current.Cache[CountriesCacheKey];
            if (countries == null)
            {
                countries = unsecuredClient.GetCountries();
                HttpContext.Current.Cache.Insert(CountriesCacheKey, countries, null,
                    DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return countries;
        }

        public static DataSet GetStates()
        {
            DataSet states = (DataSet)HttpContext.Current.Cache[StatesCacheKey];
            if (states == null)
            {
                states = unsecuredClient.GetStateProvince();
                HttpContext.Current.Cache.Insert(StatesCacheKey, states, null,
                    DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return states; // TODO: spotted the following line, replacing it with the current... okay?
            //return client.GetStateProvince();
        }

        public static DataSet GetStates(int countryId)
        {
            DataSet states = GetStates().Copy();
            for (int i = states.Tables["StateProvince"].Rows.Count - 1; i >= 0; i--)
            {
                if (Convert.ToInt32(states.Tables["StateProvince"].Rows[i]["country"])
                    != countryId)
                {
                    states.Tables["StateProvince"].Rows.RemoveAt(i);
                }
            }
            return states;
        }

        public static DataSet GetCountries(bool haveStates)
        {//TODO: Rewrite this fuckin mess
            DataSet countriesWithStates =(DataSet)HttpContext.Current.Cache[CountriesWithStatesCacheKey];
            DataSet countriesWithoutStates =(DataSet)HttpContext.Current.Cache[CountriesWithoutStatesCacheKey];
            if (countriesWithStates == null || countriesWithoutStates == null)
            {
                DataSet statesResults = GetStates();
                List<int> idsOfcountriesWithStates = new List<int>();
                foreach (DataRow state in statesResults.Tables["StateProvince"].Rows)
                {
                    if (!idsOfcountriesWithStates.Contains(Convert.ToInt32(state["country"])))
                        idsOfcountriesWithStates.Add(Convert.ToInt32(state["country"]));
                }
                countriesWithoutStates = GetCountries();
                countriesWithStates = new DataSet();

                foreach (DataTable table in countriesWithoutStates.Tables)
                {
                    countriesWithStates.Tables.Add(table.TableName);
                    foreach (DataColumn col in table.Columns)
                    {
                        countriesWithStates.Tables[table.TableName].Columns.Add(
                            col.ColumnName, col.DataType);
                    }
                }

                for (int i = countriesWithoutStates.Tables["Country"].Rows.Count - 1; i >= 0; i--)
                {
                    if (idsOfcountriesWithStates.Contains(
                        Convert.ToInt32(countriesWithoutStates.Tables["Country"].Rows[i]["id"])))
                    {
                        countriesWithStates.Tables["Country"].Rows.Add(
                            countriesWithoutStates.Tables["Country"].Rows[i].ItemArray);
                        countriesWithoutStates.Tables["Country"].Rows.RemoveAt(i);
                    }
                }

                string[] cacheKeys = new string[] { CountriesCacheKey, StatesCacheKey };
                CacheDependency dependency1 = new CacheDependency(null, cacheKeys);
                CacheDependency dependency2 = new CacheDependency(null, cacheKeys);
                HttpContext.Current.Cache.Insert(CountriesWithStatesCacheKey, countriesWithStates,
                    dependency1);
                HttpContext.Current.Cache.Insert(CountriesWithoutStatesCacheKey,
                    countriesWithoutStates, dependency2);
            }
            if (haveStates)
                return countriesWithStates;
            else
                return countriesWithoutStates;
        }

        public static bool UpdateAccountInfo(AccountInfo newInfo, bool allowEmailChange)
        {
            if (!IsLoggedIn())
            {
                throw new ApplicationException("UpdateAccountInfo() was called out of "
                    + "sequence. An authenticated Tessitura session is required.");
            }



            // TODO: retrieve original email address and phone numbers for comparisons:

            AccountInfo oldInfo = GetAccountInformation();

            // Check if email has changed (if necessary)

            if (!allowEmailChange && oldInfo.Email != newInfo.Email)
                return false;

            AccountPerson person1 = newInfo.People.Person1;
            AccountPerson person2 = newInfo.People.Person2;
            if (person2 == null)
                person2 = new AccountPerson();

            // Update the constituent

            string sessionKey = HttpContext.Current.Session[TessSessionKeySessionKey].ToString();

            HttpContext.Current.Cache.Remove(AccountInfoCacheKeyBase + "["
                + HttpContext.Current.Session[UsernameSessionKey] + "]");

            try
            {
                unsecuredClient.UpdateAccountInfoEx2(
                    sSessionKey: sessionKey,
                    sEmail: newInfo.Email ?? oldInfo.Email,
                    sPhone: newInfo.Phone ?? "[]",
                    sStreet1: newInfo.Address ?? "[]",
                    sStreet2: newInfo.SubAddress ?? "[]",
                    sCity: newInfo.City ?? "[]",
                    sStateProv: String.IsNullOrWhiteSpace(newInfo.StateId) ? "NY" : newInfo.StateId,
                    // won't delete via API call (API bug)
                    sPostalCode: newInfo.PostalCode ?? "0", // won't delete via API call (API bug)
                    iCountry: newInfo.CountryId ?? 1,
                    sPhone2: newInfo.Phone2 ?? "[]",
                    iPhone2Type: 0,
                    sFax: newInfo.Fax ?? "[]",
                    sFirstName: person1.FirstName ?? "[]",
                    sLastName: person1.LastName,
                    sMiddleName: person1.MiddleName ?? "[]",
                    sPrefix: person1.Prefix ?? "[]",
                    sSuffix: person1.Suffix ?? "[]",
                    sBusinessTitle: newInfo.BusinessTitle ?? "[]",
                    iEmailIndicator: (newInfo.WantsEmail ?? true) ? 3 : 1,
                    iMailIndicator: (newInfo.WantsMail ?? true) ? 3 : 1,
                    iPhoneIndicator: (newInfo.WantsPhone ?? true) ? 3 : 1,
                    sHtmlIndicator: (newInfo.CanReceiveHtmlEmail ?? true) ? "Y" : "N",
                    sGender: person1.Gender == null ? "[]" : person1.Gender.ToString(),
                    sGender2: person2.Gender == null ? "[]" : person2.Gender.ToString(),
                    sFirstName2: person2.FirstName ?? "[]",
                    sMiddleName2: person2.MiddleName ?? "[]",
                    sLastName2: person2.LastName ?? "[]",
                    sPrefix2: person2.Prefix ?? "[]",
                    sSuffix2: person2.Suffix ?? "[]",
                    iOriginalSourceNumber: 0,
                    bUpdateSalutation: true,
                    iAddressTypeOverwrite: newInfo.AddressTypeId ?? 0,
                    iEaddressTypeOverwrite: 0,
                    sEsal1DescriptionOverwrite: "",
                    sEsal2DescriptionOverwrite: "",
                    sLsalDescriptionOverwrite: "",
                    iConstituentTypeOverwrite: 0,
                    iNameStatus: 1,
                    iName2Status: 1);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
                    return false;
                else
                    throw e;
            }

            if (oldInfo.Email != newInfo.Email)
            {
                // TODO: update username with new email
            }

            if (oldInfo.Phone != null && newInfo.Phone == null)
                unsecuredClient.RemovePhone(sessionKey, oldInfo.PhoneId.Value);
            if (oldInfo.Phone2 != null && newInfo.Phone2 == null)
                unsecuredClient.RemovePhone(sessionKey, oldInfo.Phone2Id.Value);
            if (oldInfo.Fax != null && newInfo.Fax == null)
                unsecuredClient.RemovePhone(sessionKey, oldInfo.FaxId.Value);

            return true;
        }

        public static bool UpdateAccountInfo(AccountInfo newInfo)
        {
            return UpdateAccountInfo(newInfo, false);
        }

        public static bool MaintainedSessionExists()
        {
            return HttpContext.Current.Session[TessSessionKeySessionKey] != null
                   && HttpContext.Current.Session[UsernameSessionKey] != null;
        }

        public static bool IsLoggedIn()
        {
            return MaintainedSessionExists()
                   && HttpContext.Current.Session[UsernameSessionKey].ToString()
                       != anonymousUsername;
        }

        public static AccountInfo GetAccountInformation()
        {
            if (!IsLoggedIn())
                throw new ApplicationException("GetAccountInformation() was called out of "
                    + "sequence. An authenticated Tessitura session is required.");
            string cacheKey = AccountInfoCacheKeyBase + "["
                + HttpContext.Current.Session[UsernameSessionKey].ToString() + "]";
            AccountInfo info = (AccountInfo)HttpContext.Current.Cache[cacheKey];
            if (info == null)
            {
                DataSet accountResults = unsecuredClient.GetAccountInfo(
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
                DataRow data = accountResults.Tables["AccountInformation"].Rows[0];
                AccountPerson person1 = null;
                AccountPerson person2 = null;
                if (data["name_status"].ToString() != "Deceased"
                    && data["lname"].ToString() != "")
                {
                    char? gender = null;
                    if (data["gender_1"] != DBNull.Value)
                        gender = Convert.ToChar(data["gender_1"]);
                    person1 = new AccountPerson(
                        data["prefix"].ToString(),
                        data["fname"].ToString(),
                        data["mname"].ToString(),
                        data["lname"].ToString(),
                        data["suffix"].ToString(),
                        gender,
                        false);
                }
                if (data["name2_status"].ToString() != "Deceased"
                    && data["lname2"].ToString() != "")
                {
                    char? gender = null;
                    if (data["gender_2"] != DBNull.Value)
                        gender = Convert.ToChar(data["gender_2"]);
                    person2 = new AccountPerson(
                        data["prefix2"].ToString(),
                        data["fname2"].ToString(),
                        data["mname2"].ToString(),
                        data["lname2"].ToString(),
                        data["suffix2"].ToString(),
                        gender,
                        false);
                }
                AccountPersonPair personPair = null;
                if (person1 != null || person2 != null)
                    personPair = new AccountPersonPair(person1, person2);
                info = new AccountInfo();
                info.CustomerNumber = Convert.ToInt32(data["customer_no"]);
                info.People = personPair;
                info.Address = data["street1"].ToString();
                info.SubAddress = data["street2"].ToString();
                info.City = data["city"].ToString();
                info.StateId = data["state"].ToString();
                info.PostalCode = data["postal_code"].ToString();
                info.SetNewCountryIdWithDesc(
                    Convert.ToInt32(data["country"]), data["country_name"].ToString());
                string phone = data["phone"].ToString();
                info.EmailUnprotected = data["email"].ToString();
                // data["use_avs"]
                string phone2 = data["phone2"].ToString();
                string fax = data["fax_phone"].ToString();
                // data["esal1_desc"]
                // data["esal2_desc"]
                // data["lsal_desc"]
                info.BusinessTitle = data["business_title"].ToString();
                info.CanReceiveHtmlEmail = data["html_ind"].ToString() == "Y";
                // data["original_source"]
                info.WantsMail = data["mail_ind_id"] == DBNull.Value
                                 || Convert.ToInt32(data["mail_ind_id"]) == 3;
                info.WantsPhone = data["phone_ind_id"] == DBNull.Value
                                  || Convert.ToInt32(data["phone_ind_id"]) == 3;
                info.WantsEmail = data["emarket_ind_id"] == DBNull.Value
                                  || Convert.ToInt32(data["emarket_ind_id"]) == 3;
                // data["email_inactive"]
                DataSet constituentResults = unsecuredClient.GetConstituentInfoEx(
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString(), null);
                if (constituentResults.Tables["Addresses"].Rows.Count > 0)
                {
                    foreach (DataRow address in constituentResults.Tables["Addresses"].Rows)
                    {
                        if (address["primary_ind"].ToString() == "Y")
                        {
                            info.SetAddressTypeIdWithDesc(
                                Convert.ToInt32(address["address_type"]),
                                address["address_type_desc"].ToString());
                            break;
                        }
                    }
                }
                int? phoneId = null;
                int? phone2Id = null;
                int? faxId = null;
                if (constituentResults.Tables["Phones"].Rows.Count > 0)
                {
                    foreach (DataRow phoneNumber in constituentResults.Tables["Phones"].Rows)
                    {
                        string number =
                            Regex.Replace(phoneNumber["phone"].ToString(), "[^0-9]", "");
                        if (!String.IsNullOrWhiteSpace(phone) && number == phone)
                            phoneId = Convert.ToInt32(phoneNumber["phone_no"]);
                        if (!String.IsNullOrWhiteSpace(phone2) && number == phone2)
                            phone2Id = Convert.ToInt32(phoneNumber["phone_no"]);
                        if (!String.IsNullOrWhiteSpace(fax) && number == fax)
                            faxId = Convert.ToInt32(phoneNumber["phone_no"]);
                    }
                    if (phoneId != null)
                        info.SetPhoneWithId(phone, phoneId);
                    else
                        info.Phone = phone;
                    if (phone2Id != null)
                        info.SetPhone2WithId(phone2, phone2Id);
                    else
                        info.Phone2 = phone2;
                    if (faxId != null)
                        info.SetFaxWithId(fax, faxId);
                    else
                        info.Fax = fax;
                }
                HttpContext.Current.Cache.Insert(cacheKey, info, null,
                    DateTime.MaxValue, new TimeSpan(0, 10, 0));
            }
            return info;
        }

        public static string EncodeProcedureParam(string param)
        {
            // TODO: test if \@ will properly escape an at-sign for local proc, then implement here:
            return param.Replace("@", "").Replace("&", "\\&").Replace("=", "\\=").Replace("\\", "\\\\");
        }

        public static bool RevalidatePassword(string inputPassword)
        {
            if (!IsLoggedIn())
                throw new ApplicationException("RevalidatePassword() was called out of "
                    + "sequence. An authenticated Tessitura session is required.");
            string currentPassword = null;
            DataSet results =
                client.GetVariables(
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
            foreach (DataRow row in results.Tables["SessionVariable"].Rows)
            {
                if (row["name"].ToString() == "PWD")
                    currentPassword = row["value"].ToString();
            }
            return currentPassword == inputPassword;
        }

        public static Performance[] GetPerformances()
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null || HttpContext.Current.Session[ModeOfSaleSessionKey]==null)
                MaintainTessSession();
            Performance[] perfs = null;
         //   Performance[] perfs =(Performance[])HttpContext.Current.Cache[PerformancesCacheKey];
            if (perfs == null)
            {
                string sessionKey =
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
                short modeOfSale = (short)HttpContext.Current.Session[ModeOfSaleSessionKey];
                DataSet results = unsecuredClient.GetPerformancesEx4(
                    sWebSessionId: sessionKey,
                    sStartDate: DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"),
                    sEndDate: "",
                    iVenueID: (short)-1,
                    iModeOfSale: modeOfSale,
                    iBusinessUnit: defaultBusinessUnit,
                    sSortString: "",
                    sKeywords: "",
                    cKeywordAndOrStatement: "",
                    sArtistLastName: "",
                    sFullText: "",
                    sFullTextType: "",
                    sContentType: "",
                    sPerformanceIds: "",
                    sSeasonIds: "",
                    bIncludeSeatCounts: false);
                DataRowCollection resultRows = results.Tables[0].Rows;
                perfs = new Performance[resultRows.Count];
                for (int i = 0; i < resultRows.Count; i++)
                {
                    perfs[i] = new Performance(
                        Convert.ToInt32(resultRows[i]["perf_no"]),
                        resultRows[i]["description"].ToString(),
                        Convert.ToInt32(resultRows[i]["prod_season_no"]),
                        Convert.ToInt16(resultRows[i]["prod_type"]),
                        Convert.ToDateTime(resultRows[i]["perf_date"]),
                        Convert.ToInt32(resultRows[i]["facility_no"]),
                        results.Tables["WebContent"]);
                }

                HttpContext.Current.Cache.Insert(PerformancesCacheKey, perfs, null,
                    DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }

            return perfs;
        }

        public static Performance[] GetPerformances(DateTime startDate, DateTime endDate)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null || HttpContext.Current.Session[ModeOfSaleSessionKey] == null)
                MaintainTessSession();
            Performance[] perfs = null;
         //  Performance[] perfs= (Performance[])HttpContext.Current.Cache[PerformancesCacheKey];
            if (perfs == null)
            {
                string sessionKey =
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
                short modeOfSale = (short)HttpContext.Current.Session[ModeOfSaleSessionKey];
                DataSet results = unsecuredClient.GetPerformancesEx4(
                    sWebSessionId: sessionKey,
                    sStartDate: startDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    sEndDate: endDate.ToString("yyyy-MM-ddTHH:mm:ss"),
                    iVenueID: (short)-1,
                    iModeOfSale: modeOfSale,
                    iBusinessUnit: defaultBusinessUnit,
                    sSortString: "",
                    sKeywords: "",
                    cKeywordAndOrStatement: "",
                    sArtistLastName: "",
                    sFullText: "",
                    sFullTextType: "",
                    sContentType: "",
                    sPerformanceIds: "",
                    sSeasonIds: "",
                    bIncludeSeatCounts: false);
                DataRowCollection resultRows = results.Tables[0].Rows;
                perfs = new Performance[resultRows.Count];
                for (int i = 0; i < resultRows.Count; i++)
                {
                    perfs[i] = new Performance(
                        Convert.ToInt32(resultRows[i]["perf_no"]),
                        resultRows[i]["description"].ToString(),
                        Convert.ToInt32(resultRows[i]["prod_season_no"]),
                        Convert.ToInt16(resultRows[i]["prod_type"]),
                        Convert.ToDateTime(resultRows[i]["perf_date"]),
                        Convert.ToInt32(resultRows[i]["facility_no"]),
                        results.Tables["WebContent"]);
                }
                HttpContext.Current.Cache.Insert(PerformancesCacheKey, perfs, null,
                    DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return perfs;
        }

        public static Performance[] GetPerformances(DateTime startDate, DateTime endDate, Organization organizationId)
        {
            Performance[] perfs = GetPerformances(startDate, endDate);
            List<Performance> selectedPerfs = new List<Performance>();

            foreach (Performance perf in perfs)
            {
                if (perf.Organization == organizationId)
                    selectedPerfs.Add(perf);
            }
            return selectedPerfs.ToArray();
        }
        public static ProdPerformance[] GetPerformances(int prodId)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            DataRowCollection results = null;
            try
            {
                results = unsecuredClient.GetProductionDetailEx3(
                    SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    iPerf_no: 0,
                    iProd_Season_no: prodId,
                    iModeOfSale: (short)HttpContext.Current.Session[ModeOfSaleSessionKey],
                    iBusinessUnit: defaultBusinessUnit,
                    sContentTypes: "",
                    sPerformanceContentTypes: "",
                    bIncludeSeatCounts: true).Tables["Performance"].Rows;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Invalid Prod Season Number"))
                    return null;
                else
                    throw e;
            }
            // TODO: return only available perfs
            ProdPerformance[] perfs = new ProdPerformance[results.Count];
            for (int c = 0; c < results.Count; c++)
            {
                perfs[c] = new ProdPerformance(
                        Convert.ToInt32(results[c]["perf_no"]),
                        Convert.ToDateTime(results[c]["perf_date"]),
                        results[c]["facility_desc"].ToString(),
                        Convert.ToInt32(results[c]["availbility_by_customer"]),
                        Convert.ToInt32(results[c]["facility_no"]),
                        Convert.ToInt32(results[c]["prod_type"]));
            }
            return perfs;
        }

        public static Venue[] GetVenues()
        {
            Venue[] venues = (Venue[])HttpContext.Current.Cache[VenuesCacheKey];
            if (venues == null)
            {
                DataRowCollection results = unsecuredClient.GetVenue().Tables[0].Rows;
                venues = new Venue[results.Count];
                for (int i = 0; i < results.Count; i++)
                {
                    venues[i] = new Venue(
                        Convert.ToInt32(results[i]["facil_no"]),
                        results[i]["theaterDesc"].ToString());
                }
                HttpContext.Current.Cache.Insert(VenuesCacheKey, venues, null,
                    DateTime.Now.AddMinutes(10), TimeSpan.Zero);
            }
            return venues;
        }

        public static Production GetProduction(int id)
        {
            //if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
                Production prod = null;
              // Production prod= (Production)HttpContext.Current.Cache[ProductionCacheKeyBase + id.ToString()];
            if (prod == null)
            {
                DataSet results = null;
                try
                {
                    results = unsecuredClient.GetProductionDetailEx3(
                        SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        iPerf_no: 0,
                        iProd_Season_no: id,
                        iModeOfSale: (short)HttpContext.Current.Session[ModeOfSaleSessionKey],
                        iBusinessUnit: defaultBusinessUnit,
                        sContentTypes: "",
                        sPerformanceContentTypes: "",
                        bIncludeSeatCounts: true);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Invalid Prod Season Number"))
                        results = null;
                    else
                        throw e;
                }
                if (results != null && results.Tables["Performance"].Rows.Count>0)
                {
                    DataRow nextPerf = results.Tables["Performance"].Rows[0];
                    DataSet nextPerfDetails = unsecuredClient.GetPerformanceDetailEx(
                        iPerf_no: Convert.ToInt32(nextPerf["perf_no"]),
                        iModeOfSale: (short)HttpContext.Current.Session[ModeOfSaleSessionKey],
                        sContentType: "");
                    bool isOnSale = nextPerfDetails.Tables["Price"].Rows.Count > 0;
                    bool hasSeats = false;
                    bool isPreSale=false;
                    foreach (DataRow perfRow in results.Tables["Performance"].Rows)
                    {
                        if (perfRow["availbility_by_customer"] != DBNull.Value
                            && Convert.ToInt32(perfRow["availbility_by_customer"]) > 0)
                        {
                            hasSeats = true;
                            break;
                        }
                    }
                    //foreach (DataRow row in nextPerfDetails.Tables[5].Rows)
                    //{
                    //    if (row[4].ToString().ToUpper().Contains("PRESALE"))
                    //        isPreSale = true;
                    //}
                    DataRow prodInfo = results.Tables["Production"].Rows[0];
                    prod = new Production(
                        Convert.ToInt32(prodInfo["prod_season_no"]),
                        nextPerf["description"].ToString(),
                        prodInfo["synop"].ToString(),
                        isOnSale,!hasSeats);
                    HttpContext.Current.Cache.Insert(ProductionCacheKeyBase + id.ToString(), prod,
                        null, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
                }
            }

            return prod;
        }

        public static SeatingZone[] GetSeatingZonesAndPrices(int perfId)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            DataSet results = unsecuredClient.GetPerformanceDetailWithDiscountingEx(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iPerf_no: perfId,
                iModeOfSale: (short)HttpContext.Current.Session[ModeOfSaleSessionKey],
                sContentType: "");
            //DataSet results2=unsecuredClient.GetSeats(
            //    sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
            //    iPackageNumber: 0,
            //    iPerformanceNumber:perfId,
            //    sZoneList: "",
            //    sSectionList: "",
            //    sScreenList: "",
            //    cSummaryOnly: 'N',
            //    cCalcPackageAlloc:'Y',
            //    sCheckPriceTypes: "",
            //    cReturnNonSeats: 'N');

            //ArrayList seats = new ArrayList();
            //ArrayList statusFiveSeats = new ArrayList();
            //foreach (DataRow row in results2.Tables["Seat"].Rows){
            //    string[] temp=new string[4];
                
            //        temp[0] = row["section"].ToString();
            //        temp[1] = row["seat_row"].ToString();
            //        temp[2] = row["seat_num"].ToString();
            //        temp[3] = row["seat_status"].ToString();
            //        if ((int)row["seat_status"] == 5)
            //            statusFiveSeats.Add(temp);
            //        else
            //            seats.Add(temp);
                
            //}
            DataRowCollection zoneRows = results.Tables["Price"].Rows;
            DataRowCollection priceTypeRows = results.Tables["PriceType"].Rows;
            DataRowCollection allPriceZoneComboRows = results.Tables["AllPrice"].Rows;

            List<SeatingZone> zones = new List<SeatingZone>();
            foreach (DataRow zoneRow in zoneRows)
            {
                int tempSeatCount = 0;
                if (zoneRow["avail_count"] != DBNull.Value)
                    tempSeatCount = Convert.ToInt32(zoneRow["avail_count"]);
                if (tempSeatCount > 0)
                {
                    int tempZoneId = Convert.ToInt32(zoneRow["zone_no"]);
                    string tempZoneName = zoneRow["description"].ToString();
                    List<PriceType> tempPriceTypes = new List<PriceType>();
                    foreach (DataRow priceTypeRow in priceTypeRows)
                    {
                        int tempPriceTypeId = Convert.ToInt32(priceTypeRow["price_type"]);
                        foreach (DataRow allPriceRow in allPriceZoneComboRows)
                        {
                            int allPriceZoneId = Convert.ToInt32(allPriceRow["zone_no"]);
                            int allPricePriceTypeId = Convert.ToInt32(allPriceRow["price_type"]);
                            if (tempZoneId == allPriceZoneId
                                && tempPriceTypeId == allPricePriceTypeId)
                            {
                                string tempPriceTypeName = priceTypeRow["description"].ToString();
                                double tempPrice = Convert.ToDouble(allPriceRow["price"]);
                                bool tempIsDefault =
                                    priceTypeRow["def_price_type"].ToString() == "Y";
                                bool tempIsPromo =
                                    priceTypeRow["promo"].ToString() == "Y";
                                PriceType tempPriceType = new PriceType(tempPriceTypeId,
                                    tempPriceTypeName, tempPrice, tempIsDefault, tempIsPromo);
                                tempPriceTypes.Add(tempPriceType);
                                break;
                            }
                        }
                    }
                    SeatingZone tempZone = new SeatingZone(tempZoneId, tempZoneName, tempSeatCount,
                        tempPriceTypes.ToArray());
                    zones.Add(tempZone);
                }
            }
            return zones.ToArray();
        }

        public static bool EnterPromoCode(string promoCode)
        {
            DataTable results = unsecuredClient.GetPromoCodeEx(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                PromotionCodeString: promoCode,
                PromotionCode: 0).Tables[0];
            if (results.Rows.Count == 0)
                return false;
            else
            {
                //update mode of sale for promotion
                int mos=(int)(results.Rows[0]["mos"]);
                short mosShort=(short)mos;
                if (mosShort != (short)HttpContext.Current.Session[ModeOfSaleSessionKey])
                {                    
                    HttpContext.Current.Session[ModeOfSaleSessionKey] = mosShort;
                    try
                    {
                        unsecuredClient.ChangeModeOfSale(
                            SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                            NewModeOfSale: mos
                        );
                    }
                    catch (Exception e)
                    {
                        EmptyCart();
                        unsecuredClient.ChangeModeOfSale(
                            SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                            NewModeOfSale: mos
                        );
                    }
                }
            }
            int sourceId = Convert.ToInt32(results.Rows[0]["source_no"]);
            try
            {
                unsecuredClient.UpdateSourceCode(
                    SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    NewPromoCode: sourceId);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("Invalid Source_no"))
                    return false;
                throw e;
            }
            return true;
        }

        public static Reservation ReserveBestSeating(Reservation reservation)
        {
            Reservation unreserved = new Reservation(reservation.PerfId);
            foreach (ReservationSeatingZone request in reservation.Sections)
            {
                StringBuilder priceTypeParam = new StringBuilder();
                if (request.PriceTypesSeats.Count == 1)
                    priceTypeParam.Append(request.PriceTypesSeats[0].PriceTypeId);
                else
                {
                    List<int> priceTypes = new List<int>();
                    foreach (PriceTypeSeatsPair seatsPrice in request.PriceTypesSeats)
                    {
                        for (int c = 0; c < seatsPrice.SeatCount; c++)
                        {
                            priceTypes.Add(seatsPrice.PriceTypeId);
                        }
                    }
                    priceTypeParam.Append(priceTypes[0].ToString());
                    for (int c = 1; c < priceTypes.Count; c++)
                    {
                        priceTypeParam.Append("," + priceTypes[c].ToString());
                    }
                }
                int seatsReserved = 0;
                try
                {
                    seatsReserved =
                        unsecuredClient.ReserveTicketsEx(
                            sWebSessionID:
                                HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                            sPriceType: priceTypeParam.ToString(),
                            iPerformanceNumber: reservation.PerfId,
                            iNumberOfSeats: request.NumOfSeats,
                            iZone: request.SectionId,
                            sSpecialRequests: "");
                }
                catch (Exception e)
                {
                    if (!e.Message.Contains("TESSITURA_SEAT_LIMIT_EXCEPTION"))
                        throw e;
                }

                if (seatsReserved < request.NumOfSeats)
                {
                    foreach (PriceTypeSeatsPair priceSeats in request.PriceTypesSeats)
                    {
                        unreserved.AddPriceTypeSeats(request.SectionId, priceSeats.PriceTypeId,
                            priceSeats.SeatCount);
                    }
                }
            }
            if (unreserved.Sections.Count < reservation.Sections.Count)
            {
                checkExpireTime();
            }
            return unreserved;
        }

        public static Performance GetPerformance(int perfId)
        {
            //foreach (Performance perf in GetPerformances())
            //{
            //    if (perf.Id == perfId)
            //        return perf;
            //}

            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null || HttpContext.Current.Session[ModeOfSaleSessionKey] == null)
                MaintainTessSession();
            
                string sessionKey =
                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
                short modeOfSale = (short)HttpContext.Current.Session[ModeOfSaleSessionKey];
                DataSet results = unsecuredClient.GetPerformanceDetail(
                    iPerf_no:perfId,
                    iModeOfSale: modeOfSale);
                DataRowCollection resultRows = results.Tables[0].Rows;
                Performance perf=null;
                if(resultRows.Count>0)
                {
                    perf = new Performance(
                        perfId,
                        resultRows[0]["description"].ToString(),
                        Convert.ToInt32(resultRows[0]["prod_season_no"]),
                        Convert.ToInt16(resultRows[0]["perf_type_id"]),
                        Convert.ToDateTime(resultRows[0]["perf_dt"]),
                        Convert.ToInt32(resultRows[0]["facility_no"]),
                        results.Tables["WebContent"]);
                }
                return perf;
        }

        public static void RemoveSeatGroupFromCart(int perfId, int seatGroupId)
        {
            if (HttpContext.Current.Session[SyosReservedSeatsSessionKey] != null)
            {
                Dictionary<int, List<int>> syosReservedSeats =
                    (Dictionary<int, List<int>>)HttpContext.Current.Session[
                    SyosReservedSeatsSessionKey];
                if (syosReservedSeats.ContainsKey(perfId))
                {
                    List<int> seats = syosReservedSeats[perfId];
                    Cart cart = GetCart();
                    foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                    {
                        if (seatGroup.Id == seatGroupId)
                        {
                            foreach (CartPriceTypeSeats priceTypeSeats
                                in seatGroup.SeatsPerPriceTypes)
                            {
                                foreach (CartSeat seat in priceTypeSeats.Seats)
                                {
                                    if (seats.Contains(seat.Id))
                                        seats.Remove(seat.Id);
                                }
                            }
                            break;
                        }
                    }
                }
            }
            unsecuredClient.ReleaseTickets(
                sWebSessionID: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iPerformanceNumber: perfId,
                iLineItemNumber: seatGroupId);
        }

        public static TimeSpan GetTimeToCartExpiration()
        {
            if (HttpContext.Current.Session[CartExpireTimeSessionKey] != null)
            {
                DateTime expireTime =
                    (DateTime)HttpContext.Current.Session[CartExpireTimeSessionKey];
                TimeSpan timeLeft = expireTime.Subtract(DateTime.Now);
                if (timeLeft.TotalSeconds < 1)
                    timeLeft = new TimeSpan(0, 0, 1);
                return timeLeft;
            }
            return new TimeSpan(0);
        }

        public static Dictionary<int, string> GetCreditCardTypes()
        {
            Dictionary<int, string> cardTypes = new Dictionary<int, string>();
            DataSet results = unsecuredClient.GetPaymentMethod(
                sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString());

            foreach (DataRow cardTypeRow in results.Tables["PaymentMethod"].Rows)
            {
                if (!String.IsNullOrWhiteSpace(cardTypeRow["account_type"].ToString()))
                    cardTypes.Add(Convert.ToInt32(cardTypeRow["id"]),
                        cardTypeRow["account_type"].ToString());
            }
            return cardTypes;
        }

        public static CheckoutResult Checkout(int orderId, decimal amountDue, int shippingMethodId,
            string cardHolderName, string cardNumber, int ccTypeId, int ccExpMonth, int ccExpYear,
            string ccAuthCode)
        {
            CheckoutResult checkoutResult = CheckoutResult.Unprocessed;
            DataSet results = unsecuredClient.GetAddressAndShippingMethod(
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
            int primaryAddressNumber = -1;
            foreach (DataRow addressRow in results.Tables["Address"].Rows)
            {
                if (Convert.ToChar(addressRow["primary"]) == 'Y')
                    primaryAddressNumber = Convert.ToInt32(addressRow["address_no"]);
            }
            unsecuredClient.SetShippingInformation(
                sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iAddress_no: primaryAddressNumber,
                iShippingMethod: shippingMethodId);
            try
            {
                client.CheckoutEx3(
                    sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    sCCOwner: cardHolderName,
                    sCCNumber: cardNumber,
                    iCCType: ccTypeId,
                    iCCMonth: ccExpMonth,
                    iCCYear: ccExpYear,
                    dAmount: amountDue,
                    bAllowUnderPayment: false,
                    sCardAuthenticationCode: ccAuthCode,
                    iCCIssueNumber: 0,
                    iCCStartMonth: 0,
                    iCCStartYear: 0,
                    bAuthorize: true,
                    sAuthorizationCode: "",
                    s3DSecureValues: "");
                checkoutResult = CheckoutResult.Succeeded;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("CREDITCARD_VALIDATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardInvalid;
                else if (e.Message.Contains("CREDITCARD_TYPE_MISMATCH_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardTimeMismatch;
                else if (e.Message.Contains("CCS_TIMEOUT_EXCEPTION")
                    || e.Message.Contains("SEATSERVER_TIMEOUT_EXCEPTION"))
                    checkoutResult = CheckoutResult.ServerTimeout;
                else if (e.Message.Contains("AUTHORIZATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardNotAuthorized;
                else if (e.Message.Contains("DECLINATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardDeclined;
                else
                    throw e;
            }
            if (checkoutResult == CheckoutResult.Succeeded)
                HttpContext.Current.Session.Remove(CartExpireTimeSessionKey);
            return checkoutResult;
        }

        public static bool SendMembershipConfirmationEmail(int orderId)
        {
            return unsecuredClient.SendOrderConfirmationEmail(
                sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iTemplateNo: 64,
                iOrderNo: orderId,
                sSubject: "Membership Email Confirmation");
        }

        public static bool SendOrderConfirmationEmail(int orderId, Organization[] organizations)
        {
            bool joesPubFound = false;
            foreach (Organization org in organizations)
            {
                if (org == Organization.JoesPub)
                {
                    joesPubFound = true;
                    break;
                }
            }

            return unsecuredClient.SendOrderConfirmationEmail(
                sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iTemplateNo: joesPubFound ? 68 : 61,
                iOrderNo: orderId,
                sSubject: "");
        }

        public static bool SendOrderConfirmationEmail(int orderId, string commaSeparatedOrgList)
        {
            string[] parts = commaSeparatedOrgList.Split(',');
            Organization[] orgs = new Organization[parts.Length];
            for (int c = 0; c < parts.Length; c++)
            {
                orgs[c] = (Organization)Enum.Parse(typeof(Organization), parts[c]);
            }
            return SendOrderConfirmationEmail(orderId, orgs);
        }

        public static SyosSeatCollection GetSeatsForSyos(int perfId)
        {
            // Getting result sets from the API

            DataRowCollection seatRows;
            DataRowCollection sectionRows;
            DataRowCollection zoneRows;
            DataRowCollection zonePriceTypeRows;
            DataRowCollection priceTypeRows;

            DataSet results = unsecuredClient.GetSeats(
                sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iPackageNumber: 0,
                iPerformanceNumber: perfId,
                sZoneList: "",
                sSectionList: "",
                sScreenList: "",
                cSummaryOnly: 'N',
                cCalcPackageAlloc: 'N',
                sCheckPriceTypes: "",
                cReturnNonSeats: 'N');
            seatRows = results.Tables["Seat"].Rows;
            sectionRows = results.Tables["Section"].Rows;
            results = unsecuredClient.GetPerformanceDetailWithDiscountingSYOSDataSet(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                iPerf_no: perfId,
                iModeOfSale: (short)HttpContext.Current.Session[ModeOfSaleSessionKey],
                sContentType: "");
            zoneRows = results.Tables["Section"].Rows;
            zonePriceTypeRows = results.Tables["SectionPriceTypes"].Rows;
            priceTypeRows = results.Tables["PriceTypes"].Rows;

            // Parsing results sets

            List<SyosSeatSection> sectionsList = new List<SyosSeatSection>();
            foreach (DataRow sectionRow in sectionRows)
            {
                int newSectionId = Convert.ToInt32(sectionRow["section"]);
                string newSectionName = sectionRow["section_desc"].ToString();

                List<SyosSeatZone> zonesList = new List<SyosSeatZone>();
                foreach (DataRow zoneRow in zoneRows)
                {
                    int newZoneId = Convert.ToInt32(zoneRow["Zone"]);
                    string newZoneName = zoneRow["Description"].ToString();

                    Dictionary<int, List<SyosSeat>> seatGroupList
                        = new Dictionary<int, List<SyosSeat>>();
                    foreach (DataRow seatRow in seatRows)
                    {
                        if (Convert.ToInt32(seatRow["zone_no"]) == newZoneId
                            && Convert.ToInt32(seatRow["section"]) == newSectionId
                            && (seatRow["hc_no"] == DBNull.Value
                            || !invisibleHoldCodes.Contains(Convert.ToInt32(seatRow["hc_no"]))))
                        {
                            SyosSeatStatus newStatus;
                            if (seatRow["hc_no"] != DBNull.Value)
                                newStatus = SyosSeatStatus.Unavailable;
                            else
                            {
                                bool seatFound = false;
                                if (HttpContext.Current.Session[SyosReservedSeatsSessionKey]
                                    != null)
                                {
                                    Dictionary<int, List<int>> syosReservedSeats =
                                        (Dictionary<int, List<int>>)
                                        HttpContext.Current.Session[SyosReservedSeatsSessionKey];
                                    if (syosReservedSeats.ContainsKey(perfId))
                                    {
                                        List<int> seats = syosReservedSeats[perfId];
                                        seatFound = seats.Contains(
                                            Convert.ToInt32(seatRow["seat_no"]));
                                    }
                                }
                                if (seatFound)
                                {
                                    newStatus = SyosSeatStatus.InCart;
                                }
                                else if (Convert.ToInt32(seatRow["seat_status"]) == 0)
                                {
                                    newStatus = SyosSeatStatus.Available;
                                }
                                else
                                {
                                    newStatus = SyosSeatStatus.Unavailable;
                                }
                            }
                            SyosSeat newSeat = new SyosSeat(
                                id: Convert.ToInt32(seatRow["seat_no"]),
                                number: Convert.ToInt32(seatRow["seat_num"]),
                                xposition: Convert.ToInt32(seatRow["xpos"]),
                                yposition: Convert.ToInt32(seatRow["ypos"]),
                                status: newStatus);
                            int groupId = Convert.ToInt32(seatRow["seat_row"]);
                            if (seatGroupList.ContainsKey(groupId))
                            {
                                seatGroupList[groupId].Add(newSeat);
                            }
                            else
                            {
                                List<SyosSeat> seatsList = new List<SyosSeat>();
                                seatsList.Add(newSeat);
                                seatGroupList.Add(groupId, seatsList);
                            }
                        }
                    }

                    if (seatGroupList.Count == 0)
                        continue;

                    List<PriceType> priceTypesList = new List<PriceType>();
                    foreach (DataRow zonePriceTypeRow in zonePriceTypeRows)
                    {
                        if (Convert.ToInt32(zonePriceTypeRow["Zone"]) == newZoneId)
                        {
                            int newPriceTypeId = Convert.ToInt32(zonePriceTypeRow["Id"]);
                            string newPriceTypeName = null;
                            bool newIsPromo = false;
                            foreach (DataRow priceTypeRow in priceTypeRows)
                            {
                                if (Convert.ToInt32(priceTypeRow["Id"]) == newPriceTypeId)
                                {
                                    newPriceTypeName = priceTypeRow["Description"].ToString();
                                    newIsPromo = Convert.ToBoolean(priceTypeRow["Promotion"]);
                                    break;
                                }
                            }
                            PriceType newPriceType = new PriceType(
                                id: newPriceTypeId,
                                name: newPriceTypeName,
                                price: Convert.ToInt32(zonePriceTypeRow["Price"]),
                                isDefault: false,
                                isPromo: newIsPromo);
                            priceTypesList.Add(newPriceType);
                        }
                    }

                    SyosSeatGroup[] seatGroups = new SyosSeatGroup[seatGroupList.Count];
                    int insertIndex = 0;
                    foreach (int groupId in seatGroupList.Keys)
                    {
                        SyosSeatGroup newSeatGroup = new SyosSeatGroup(
                            id: groupId,
                            seats: seatGroupList[groupId].ToArray());
                        seatGroups[insertIndex] = newSeatGroup;
                        insertIndex++;
                    }

                    SyosSeatZone newZone = new SyosSeatZone(
                        id: newZoneId,
                        description: newZoneName,
                        priceTypes: priceTypesList.ToArray(),
                        seatGroups: seatGroups);

                    zonesList.Add(newZone);
                }

                if (zonesList.Count == 0)
                    continue;

                SyosSeatSection newSection = new SyosSeatSection(
                    id: newSectionId,
                    name: newSectionName,
                    zones: zonesList.ToArray());

                sectionsList.Add(newSection);
            }

            if (sectionsList.Count == 0)
                return null;

            SyosSeatCollection newCollection = new SyosSeatCollection(sectionsList.ToArray(),
                Convert.ToInt32(results.Tables["Performance"].Rows[0]["ZoneMap"]));

            return newCollection;
        }

        public static SyosReservation ReserveSyos(SyosReservation reservation)
        {
            SyosReservation unreserved = new SyosReservation(reservation.PerfId);
            foreach (SyosReservationSection section in reservation.Sections)
            {
                foreach (SyosReservationZone zone in section.Zones)
                {
                    if (zone.Seats.Count == 0)
                        continue;
                    StringBuilder priceTypeParam = new StringBuilder();
                    StringBuilder requestedSeatsParam = new StringBuilder();
                    foreach (SyosReservationSeat seat in zone.Seats)
                    {
                        priceTypeParam.Append("," + seat.PriceTypeId.ToString());
                        requestedSeatsParam.Append("," + seat.Id.ToString());
                    }
                    priceTypeParam.Remove(0, 1);
                    requestedSeatsParam.Remove(0, 1);
                    int seatsReserved = 0;
                    try
                    {
                        seatsReserved =
                            unsecuredClient.ReserveTicketsSpecifiedSeats(
                                sWebSessionID:
                                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                                sPriceType: priceTypeParam.ToString(),
                                iPerformanceNumber: reservation.PerfId,
                                iNumberOfSeats: reservation.SeatCount,
                                iZone: zone.Id,
                                sSpecialRequests: "LeaveSingleSeats=Y",
                                RequestedSeats: requestedSeatsParam.ToString());
                        if (seatsReserved > 0)
                        {
                            Dictionary<int, List<int>> syosReservedSeats;
                            if (HttpContext.Current.Session[SyosReservedSeatsSessionKey] == null)
                            {
                                syosReservedSeats = new Dictionary<int, List<int>>();
                            }
                            else
                            {
                                syosReservedSeats =
                                    (Dictionary<int, List<int>>)HttpContext.Current.Session[
                                    SyosReservedSeatsSessionKey];
                            }
                            if (!syosReservedSeats.ContainsKey(reservation.PerfId))
                            {
                                syosReservedSeats.Add(reservation.PerfId, new List<int>());
                            }
                            List<int> seatList = syosReservedSeats[reservation.PerfId];
                            foreach (SyosReservationSeat seat in zone.Seats)
                            {
                                seatList.Add(seat.Id);
                            }
                            HttpContext.Current.Session[SyosReservedSeatsSessionKey] =
                                syosReservedSeats;
                        }
                    }
                    catch (Exception e)
                    {
                        if (!e.Message.Contains("TESSITURA_SEAT_LIMIT_EXCEPTION")
                            && !e.Message.Contains("One or more"))
                            throw e;
                    }

                    if (seatsReserved < zone.Seats.Count)
                    {
                        foreach (SyosReservationSeat seat in zone.Seats)
                        {
                            unreserved.AddSeat(section.Id, zone.Id, seat.PriceTypeId, seat.Id);
                        }
                    }
                }
            }
            if (unreserved.SeatCount < reservation.SeatCount)
            {
                checkExpireTime();
            }
            return unreserved;
        }

        /*		public static bool ReserveSyos(int perfId, SyosSeatRequest[] seatsToReserve)
                {
                    int nextSeatToReserve = 0;
                    while (nextSeatToReserve < seatsToReserve.Length)
                    {
                        SyosSeatRequest seatToReserve = seatsToReserve[nextSeatToReserve];
                        int result = 0;
                        try
                        {
                            result = unsecuredClient.ReserveTicketsSpecifiedSeats(
                                sWebSessionID:
                                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                                sPriceType: seatToReserve.PriceTypeId.ToString(),
                                iPerformanceNumber: perfId,
                                iNumberOfSeats: 1,
                                iZone: seatToReserve.ZoneId,
                                sSpecialRequests: "LeaveSingleSeats=Y",
                                RequestedSeats: seatToReserve.SeatId.ToString());
                        }
                        catch (Exception e)
                        {
                            if (!e.Message.Contains("One or more"))
                                throw e;
                        }
                        if (result == 1)
                            nextSeatToReserve++;
                        else
                            break;
                    }
                    if (nextSeatToReserve > 0)
                    {
                        checkExpireTime();
                    }
                    if (nextSeatToReserve < seatsToReserve.Length)
                    {
                        DataSet cartResults = unsecuredClient.GetCart(
                            HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
                        DataRowCollection subLineItemRows = cartResults.Tables["SubLineItem"].Rows;
                        for (int removeCursor = 0; removeCursor < nextSeatToReserve; removeCursor++)
                        {
                            int lineItemToRemove = -1;
                            foreach (DataRow sliRow in subLineItemRows)
                            {
                                if (Convert.ToInt32(sliRow["seat_no"]) ==
                                    seatsToReserve[removeCursor].SeatId)
                                {
                                    lineItemToRemove = Convert.ToInt32(sliRow["li_seq_no"]);
                                    break;
                                }
                            }
                            unsecuredClient.ReleaseTickets(
                                sWebSessionID:
                                    HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                                iPerformanceNumber: perfId,
                                iLineItemNumber: lineItemToRemove);
                        }
                        return false;
                    }
                    Dictionary<int, List<int>> syosReservedSeats;
                    if (HttpContext.Current.Session[SyosReservedSeatsSessionKey] == null)
                    {
                        syosReservedSeats = new Dictionary<int, List<int>>();
                    }
                    else
                    {
                        syosReservedSeats =
                            (Dictionary<int, List<int>>)HttpContext.Current.Session[
                            SyosReservedSeatsSessionKey];
                    }
                    if (!syosReservedSeats.ContainsKey(perfId))
                    {
                        syosReservedSeats.Add(perfId, new List<int>());
                    }
                    List<int> seatList = syosReservedSeats[perfId];
                    foreach (SyosSeatRequest addedSeat in seatsToReserve)
                    {
                        seatList.Add(addedSeat.SeatId);
                    }
                    HttpContext.Current.Session[SyosReservedSeatsSessionKey] =
                        syosReservedSeats;
                    return true;
                }*/

        private static void checkExpireTime()
        {
            DateTime expireTime = Convert.ToDateTime(unsecuredClient.GetTicketExpiration(
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString()));
            int expirationMins = Int32.Parse(
                WebConfigurationManager.AppSettings["nysf_Tessitura_CartExpirationMins"]);
            if (expireTime < DateTime.Now.AddMinutes(expirationMins))
            {
                expireTime = DateTime.Now.AddMinutes(expirationMins);
                unsecuredClient.AlterTicketExpiration(
                    sSessionKey:
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    sExpirationTime: expireTime.ToString(),
                    iOffsetSeconds: 0);
            }
            HttpContext.Current.Session.Add(CartExpireTimeSessionKey, expireTime);
        }

        public static AttributeCollection GetAttributes()
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();
            AttributeCollection attCol = (AttributeCollection)HttpContext.Current.Cache[
                AttributesCacheKey];
            if (attCol == null)
            {
                DataSet result = unsecuredClient.GetAttributes(
                    sSessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    iAttributeID: 0);
                DataRowCollection attRows = result.Tables["AttributesKeyword"].Rows;
                DataRowCollection optRows = result.Tables["AttributesOption"].Rows;
                List<Attribute> atts = new List<Attribute>();
                foreach (DataRow attRow in attRows)
                {
                    string name = attRow["description"].ToString();
                    int id = Convert.ToInt32(attRow["keyword_no"]);
                    bool allowMult = Convert.ToChar(attRow["multiple_value"]) == 'Y';
                    switch (Convert.ToInt32(attRow["data_type"]))
                    {
                        case 1: // string
                            List<string> strVals = new List<string>();
                            foreach (DataRow optRow in optRows)
                            {
                                if (Convert.ToInt32(optRow["keyword_no"]) == id
                                    && Convert.ToChar(optRow["inactive"]) == 'N')
                                {
                                    strVals.Add(optRow["id_value"].ToString());
                                }
                            }
                            Attribute<string> newStrAtt =
                                new Attribute<string>(name, id, allowMult, strVals.ToArray());
                            atts.Add(newStrAtt);
                            break;
                        case 2: // int
                            List<int> intVals = new List<int>();
                            foreach (DataRow optRow in optRows)
                            {
                                if (Convert.ToInt32(optRow["keyword_no"]) == id
                                    && Convert.ToChar(optRow["inactive"]) == 'N')
                                {
                                    intVals.Add(Convert.ToInt32(optRow["id_value"]));
                                }
                            }
                            Attribute<int> newIntAtt =
                                new Attribute<int>(name, id, allowMult, intVals.ToArray());
                            atts.Add(newIntAtt);
                            break;
                        case 3: // date
                            List<DateTime> dateVals = new List<DateTime>();
                            foreach (DataRow optRow in optRows)
                            {
                                if (Convert.ToInt32(optRow["keyword_no"]) == id
                                    && Convert.ToChar(optRow["inactive"]) == 'N')
                                {
                                    dateVals.Add(Convert.ToDateTime(optRow["id_value"]));
                                }
                            }
                            Attribute<DateTime> newDateAtt =
                                new Attribute<DateTime>(name, id, allowMult, dateVals.ToArray());
                            atts.Add(newDateAtt);
                            break;
                    }
                }
                attCol = new AttributeCollection(atts.ToArray());
                HttpContext.Current.Cache.Insert(AttributesCacheKey, attCol,
                        null, DateTime.Now.AddMinutes(5), TimeSpan.Zero);
            }
            return attCol;
        }

        public static void AddAttribute(int attId, Object newValue, bool doReplace)
        {
            bool valueFound = false;
            if (doReplace)
            {
                ConstituentAttributeCollection acctAtts = GetConstituentAttributes();
                Object[] currentValues = acctAtts.GetValuesById(attId);
                for (int c = 0; c < currentValues.Length; c++)
                {
                    Object currentValue = currentValues[c];
                    if (!valueFound)
                    {
                        if (currentValue.Equals(newValue))
                        {
                            valueFound = true;
                        }
                        else if (c == currentValues.Length - 1)
                        {
                            UpdateAttribute(attId, currentValue, newValue);
                            valueFound = true;
                        }
                        else
                        {
                            DeleteAttribute(attId, currentValue);
                        }
                    }
                    else
                    {
                        DeleteAttribute(attId, currentValue);
                    }
                }
            }
            if (!valueFound)
            {
                unsecuredClient.UpdateConstituentAttributes(
                    sWebSessionId: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                    cAction: 'A',
                    iKeywordNumber: attId,
                    sOldKeyValue: "",
                    sNewKeyValue: newValue.ToString(),
                    cAccountName: '3');
            }
        }

        public static void AddAttribute(int id, Object value)
        {
            AddAttribute(id, value, false);
        }

        public static void UpdateAttribute(int id, Object oldValue, Object newValue)
        {
            unsecuredClient.UpdateConstituentAttributes(
                sWebSessionId: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                cAction: 'U',
                iKeywordNumber: id,
                sOldKeyValue: oldValue.ToString(),
                sNewKeyValue: newValue.ToString(),
                cAccountName: '3');
        }

        public static void DeleteAttribute(int id, Object oldValue)
        {
            unsecuredClient.UpdateConstituentAttributes(
                sWebSessionId: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                cAction: 'D',
                iKeywordNumber: id,
                sOldKeyValue: oldValue.ToString(),
                sNewKeyValue: "",
                cAccountName: '3');
        }

        public static ConstituentAttributeCollection GetConstituentAttributes()
        {
            DataSet result = unsecuredClient.GetConstituentInfoEx(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                TableListTokens: "AT");
            DataRowCollection attRows = result.Tables["ConstituentAttribute"].Rows;
            List<ConstituentAttribute> atts = new List<ConstituentAttribute>();
            foreach (DataRow attRow in attRows)
            {
                string name = attRow["attribute"].ToString();
                int id = Convert.ToInt32(attRow["keyword_no"]);
                switch (Convert.ToInt32(attRow["data_type"]))
                {
                    case 1: // string
                        string strVal = attRow["attribute_value"].ToString();
                        ConstituentAttribute<string> newStrAtt =
                            new ConstituentAttribute<string>(name, id, strVal);
                        atts.Add(newStrAtt);
                        break;
                    case 2: // int
                        int intVal = Convert.ToInt32(attRow["attribute_value"]);
                        ConstituentAttribute<int> newIntAtt =
                            new ConstituentAttribute<int>(name, id, intVal);
                        atts.Add(newIntAtt);
                        break;
                    case 3: // date
                        DateTime dateVal = Convert.ToDateTime(attRow["attribute_value"]);
                        ConstituentAttribute<DateTime> newDateAtt =
                            new ConstituentAttribute<DateTime>(name, id, dateVal);
                        atts.Add(newDateAtt);
                        break;
                }
            }
            return new ConstituentAttributeCollection(atts.ToArray());
        }

        public static bool LoginExists(string email)
        {
            DataSet result = unsecuredClient.ExecuteLocalProcedure(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                LocalProcedureId: 8016,
                LocalProcedureValues: "@email=" + email);
            return Convert.ToBoolean(result.Tables["LocalProcedure"].Rows[0][0]);
        }

        public static CheckoutResult PurchaseMemberships(int numOfMemberships, Decimal addDonation,
            bool optInAutoRenew,
            string cardHolderName, string cardNumber, int ccTypeId, int ccExpMonth, int ccExpYear,
            string ccAuthCode, string confirmationEmailSubject, int emailTemplateId)
        {
            // Make new authenticated session especially for this order

            string username =
                HttpContext.Current.Session[UsernameSessionKey].ToString();
            string password = null;
            DataSet results = client.GetVariables(
                HttpContext.Current.Session[TessSessionKeySessionKey].ToString());
            foreach (DataRow row in results.Tables["SessionVariable"].Rows)
            {
                if (row["name"].ToString() == "PWD")
                {
                    password = row["value"].ToString();
                    break;
                }
            }
            string specialSession = unsecuredClient.GetNewSessionKeyEx(
                sIP: HttpContext.Current.Request.UserHostAddress,
                iBusinessUnit: defaultBusinessUnit);
            client.Login(specialSession, username, password, defaultLoginType);

            // Build special order

            unsecuredClient.ChangeModeOfSaleEx(specialSession, memberTicketModeOfSale);
            // Add membership "tickets"
            int ticketsReserved = unsecuredClient.ReserveTicketsEx(
                sWebSessionID: specialSession,
                sPriceType: memberTicketPriceTypeId.ToString(),
                iPerformanceNumber: memberTicketPerfId,
                iNumberOfSeats: numOfMemberships,
                iZone: memberTicketZoneId,
                sSpecialRequests: "LeaveSingleSeats=Y");
            if (ticketsReserved == 0)
                throw new ApplicationException("Unable to reserve the membership ticket.");

            // Add additional contribution
            if (addDonation > 0)
                unsecuredClient.AddContribution(
                    sWebSessionID: specialSession,
                    Amount: addDonation,
                    Fund: 149,
                    AccountMethod: 0,
                    Upgrade: false,
                    Renew: false);

            // Add order comment (likely for report)
            StringBuilder comment = new StringBuilder("Order Total: $");
            comment.Append(
                (numOfMemberships * (membershipPrice + membershipHandlingFee) + addDonation)
                .ToString("N").Replace(".00", "") + " (");
            comment.Append(numOfMemberships + " membership");
            comment.Append((numOfMemberships > 1 ? "s" : "") + " for $");
            comment.Append((numOfMemberships * (membershipPrice + membershipHandlingFee))
                .ToString("N").Replace(".00", "") + " + $");
            comment.Append(addDonation.ToString("N").Replace(".00", "")
                + " additional contribution)<br>");
            unsecuredClient.AddOrderCommentsEx2(
                SessionKey: specialSession,
                Comment: comment.ToString(),
                LineItemID: 0,
                LineItemType: 'O',
                CustomerNo: 0,
                CategoryNo: 0);

            // Set up generic shipping details for order

            results = unsecuredClient.GetAddressAndShippingMethod(specialSession);
            int primaryAddressNumber = -1;
            foreach (DataRow addressRow in results.Tables["Address"].Rows)
            {
                if (Convert.ToChar(addressRow["primary"]) == 'Y')
                    primaryAddressNumber = Convert.ToInt32(addressRow["address_no"]);
            }
            unsecuredClient.SetShippingInformation(
                sSessionKey: specialSession,
                iAddress_no: primaryAddressNumber,
                iShippingMethod: -1);

            // Attempt checkout with supplied credit card info

            CheckoutResult checkoutResult;
            results = unsecuredClient.GetCart(specialSession); // to get the order number later
            try
            {
                client.CheckoutEx3(
                    sSessionKey: specialSession,
                    sCCOwner: cardHolderName,
                    sCCNumber: cardNumber,
                    iCCType: ccTypeId,
                    iCCMonth: ccExpMonth,
                    iCCYear: ccExpYear,
                    dAmount: Convert.ToDecimal(numOfMemberships * MembershipPrice + 5) + addDonation,
                    bAllowUnderPayment: false,
                    sCardAuthenticationCode: ccAuthCode,
                    iCCIssueNumber: 0,
                    iCCStartMonth: 0,
                    iCCStartYear: 0,
                    bAuthorize: true,
                    sAuthorizationCode: "",
                    s3DSecureValues: "");
                checkoutResult = CheckoutResult.Succeeded;
            }
            catch (Exception e)
            {
                if (e.Message.Contains("CREDITCARD_VALIDATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardInvalid;
                else if (e.Message.Contains("CREDITCARD_TYPE_MISMATCH_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardTimeMismatch;
                else if (e.Message.Contains("CCS_TIMEOUT_EXCEPTION")
                    || e.Message.Contains("SEATSERVER_TIMEOUT_EXCEPTION"))
                    checkoutResult = CheckoutResult.ServerTimeout;
                else if (e.Message.Contains("AUTHORIZATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardNotAuthorized;
                else if (e.Message.Contains("DECLINATION_EXCEPTION"))
                    checkoutResult = CheckoutResult.CreditCardDeclined;
                else
                {
                    // Logout to cancel order and release membership "tickets"
                    unsecuredClient.Logout(specialSession);
                    throw e;
                }
            }
            if (checkoutResult == CheckoutResult.Succeeded)
            {
                /*unsecuredClient.ExecuteLocalProcedure(specialSession, 8027,
                    "@sessionKey=" + specialSession);*/
                string optInVal = optInAutoRenew ? "Yes" : "No";
                try
                {
                    AddAttribute(371, optInVal);
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("attribute already exists"))
                    {
                        ConstituentAttributeCollection currentAtts =
                                WebClient.GetConstituentAttributes();
                        ConstituentAttribute foundAtt = null;
                        foreach (ConstituentAttribute att in currentAtts)
                        {
                            if (att.Id == 371)
                            {
                                foundAtt = att;
                                break;
                            }
                        }
                        UpdateAttribute(371, foundAtt.Value, optInVal);
                    }
                    else throw e;
                }

                int orderNumber = Convert.ToInt32(results.Tables["Order"].Rows[0]["order_no"]);
                unsecuredClient.SendOrderConfirmationEmail(
                                sSessionKey: specialSession,
                                iTemplateNo: emailTemplateId,
                                iOrderNo: orderNumber,
                                sSubject: confirmationEmailSubject);
            }
            unsecuredClient.Logout(specialSession);
            return checkoutResult;
        }

        public static Dictionary<int, string> GetMembershipCreditCardTypes()
        {
            Dictionary<int, string> cardTypes =
                (Dictionary<int, string>)HttpContext.Current.Cache[MemCreditCardTypesCacheKey];
            if (cardTypes == null)
            {
                string tempSessionKey = unsecuredClient.GetNewSessionKeyEx(
                    HttpContext.Current.Request.UserHostAddress,
                    defaultBusinessUnit);
                client.LoginEx2(
                    tempSessionKey,
                    anonymousUsername,
                    Utility.MakeObfuscation(anonymousPassword),
                    defaultLoginType,
                    defaultPromoCode,
                    "",
                    "",
                    "",
                    0,
                    defaultN1N2,
                    "");
                unsecuredClient.ChangeModeOfSale(tempSessionKey, memberTicketModeOfSale);
                int ticketsReserved = unsecuredClient.ReserveTicketsEx(
                    sWebSessionID: tempSessionKey,
                    sPriceType: memberTicketPriceTypeId.ToString(),
                    iPerformanceNumber: memberTicketPerfId,
                    iNumberOfSeats: 1,
                    iZone: memberTicketZoneId,
                    sSpecialRequests: "LeaveSingleSeats=Y");
                if (ticketsReserved == 0)
                    throw new ApplicationException("Unable to reserve the membership ticket.");
                cardTypes = new Dictionary<int, string>();

                DataSet results = unsecuredClient.GetPaymentMethod(
                    sSessionKey: tempSessionKey);
                unsecuredClient.Logout(tempSessionKey);
                foreach (DataRow cardTypeRow in results.Tables["PaymentMethod"].Rows)
                {
                    if (!String.IsNullOrWhiteSpace(cardTypeRow["account_type"].ToString()))
                        cardTypes.Add(Convert.ToInt32(cardTypeRow["id"]),
                            cardTypeRow["account_type"].ToString());
                }
                HttpContext.Current.Cache.Insert(MemCreditCardTypesCacheKey, cardTypes, null,
                    DateTime.Now.AddMinutes(5), TimeSpan.Zero);
            }
            return cardTypes;
        }

        public static void ClearPerfsCache()
        {
            HttpContext.Current.Cache.Remove(PerformancesCacheKey);
        }

        public static bool AddPackage(int packageId, int zoneId, int priceTypeId, int seatsPerPerf)
        {
            return seatsPerPerf == unsecuredClient.AddPackageItemSeated(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                PriceType: priceTypeId.ToString(),
                PackageNumber: packageId,
                NumberOfSeats: seatsPerPerf,
                Zone: zoneId,
                RequestedSeats: "",
                LeaveSingleSeats: true);
        }

        public static void EmptyCart()
        {
            string oldSessionKey = HttpContext.Current.Session[TessSessionKeySessionKey].ToString();
            string newSessionKey = unsecuredClient.GetNewSessionKeyEx(
                sIP: HttpContext.Current.Request.UserHostAddress,
                iBusinessUnit: defaultBusinessUnit);
            HttpContext.Current.Session[TessSessionKeySessionKey] = newSessionKey;
            unsecuredClient.TransferSession(oldSessionKey, newSessionKey);
        }

        /*public static void LogException(Exception ex, bool doRethrow)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(ex);
            if (doRethrow)
                throw ex;
        }

        public static void LogException(Exception ex)
        {
            LogException(ex, true);
        }*/
        public static DataSet ExecuteLocalProcedure(int procNum, string parameters)
        {
            DataSet result = unsecuredClient.ExecuteLocalProcedure(
                SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                LocalProcedureId: procNum,
                LocalProcedureValues: parameters);
            return result;
        }
        public static bool RegisterNewConstituentFacebook(string emailAddress, string givenName,
            string surname, int emailTemplateNumber, Organization sourceOrg, string password)
        {
            if (HttpContext.Current.Session[TessSessionKeySessionKey] == null)
                MaintainTessSession();

            // Check for suspiciously similar registrations



            bool registrationWasSuccessful = false;
            try
            {
                registrationWasSuccessful =
                    client.RegisterWithPromoCode(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        password,
                        defaultLoginType,
                        emailAddress,
                        givenName,
                        surname,
                        defaultPromoCode);
            }
            catch (Exception e)
            {
                // Allow the TESSITURA_DUPLICATE_LOGIN_EXCEPTION to pass without
                // halting the application

                if (!e.Message.Contains("TESSITURA_DUPLICATE_LOGIN_EXCEPTION")
                    && !e.Message.Contains("TESSITURA_DUPLICATE_EMAIL_EXCEPTION"))
                    throw e;
            }

            if (registrationWasSuccessful)
            {
                string orgAttName;
                switch (sourceOrg)
                {
                    case Organization.PublicTheater:
                        orgAttName = "The Public Theater";
                        break;
                    case Organization.JoesPub:
                        orgAttName = "Joe’s Pub";
                        break;
                    case Organization.ShakespeareInThePark:
                        orgAttName = "Shakespeare in the Park";
                        break;
                    default:
                        orgAttName = "";
                        break;
                }
                Nysf.Tessitura.AttributeCollection attCol = GetAttributes();
                Nysf.Tessitura.Attribute att = attCol.GetAttributeByName(
                    "cp_Em_" + orgAttName);
                AddAttribute(att.Id, 1);

                //// Send the account activation email
                if (emailTemplateNumber >= 0)
                {

                    unsecuredClient.SendCredentials(
                        HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        emailAddress,
                        defaultLoginType,
                        true,
                        false,
                        true,
                        emailTemplateNumber);

                    unsecuredClient.ExecuteLocalProcedure(
                        SessionKey: HttpContext.Current.Session[TessSessionKeySessionKey].ToString(),
                        LocalProcedureId: 8015,
                        LocalProcedureValues: "@ip=" + "192.168.92.40"
                        // + HttpContext.Current.Request.UserHostAddress
                                               + "&@email=" + emailAddress
                                               + "&@firstName=" + givenName
                                               + "&@lastName=" + surname);
                    //}
                }
            }
           // Logout();
            MaintainTessSession();

            return registrationWasSuccessful;
        }
        public static bool isLoggedOn()
        {
            System.Web.SessionState.HttpSessionState session = HttpContext.Current.Session;
            return unsecuredClient.LoggedIn(session[TessSessionKeySessionKey].ToString());
        }
        
        #endregion
    }
}

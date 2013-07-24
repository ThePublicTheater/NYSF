using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Caching;
using System.Web.Configuration;
using System.Xml;

namespace Nysf
{
    /// <summary>
    ///     General helper methods for use in NYSF applications.
    /// </summary>
    public static class Utility
    {
        private static MD5CryptoServiceProvider encryptionClient;

        private const string PathToAbbrevData = "~/App_Data/addressAbbreviations.xml";
        private const string RefererSessionKey = "nysf_Utility_Referer";
        private const string StateAbbrevsCacheKey = "Nysf_Utility_StateAbbrevs";
        private const string StreetSuffixAbbrevsCacheKey = "Nysf_Utility_StreetSuffixAbbrevs";
        private const string SecondaryUnitAbbrevsCacheKey = "Nysf_Utility_SecondaryUnitAbbrevs";
        private const string RefererQueryStringKey = "referer";

        static Utility()
        {
            // Verify the required settings in web.config

            string[] requiredSettingKeys =
            {
                "nysf_Utility_AppBase",
                "nysf_Utility_SaltString",
                "nysf_Utility_SmallScreenAgentIdentifiers",
				"nysf_Utility_TimeoutPageUrl"
            };
            VerifyAppSettings(requiredSettingKeys);

            // Verify the required data file

            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(PathToAbbrevData)))
                throw new System.IO.FileNotFoundException("Unable to find the required file: "
                    + PathToAbbrevData);

            // Instantiate the encryption provider

            encryptionClient = new MD5CryptoServiceProvider();
        }

        /// <summary>
        ///     Makes a salted MD5 hash from a string.
        /// </summary>
        /// <param name="originalString">
        ///     A string to obfuscate.
        /// </param>
        public static string MakeObfuscation(string originalString)
        {
            string salt = WebConfigurationManager.AppSettings["nysf_Utility_SaltString"];
            string saltedString = salt + originalString;
            byte[] originalBytes = ASCIIEncoding.Default.GetBytes(saltedString);
            byte[] encodedBytes = encryptionClient.ComputeHash(originalBytes);
            return BitConverter.ToString(encodedBytes).Replace("-","");
        }

        /// <summary>
        ///     Verifies the existence of specified appSettings in web.config and optionally throws
        ///     an exception when any are found to be missing.
        /// </summary>
        /// <param name="requiredSettingKeys">
        ///     A System.String array of required setting keys.
        /// </param>
        /// <param name="throwException">
        ///     A System.Boolean value that determines whether the method throws an exception when
        ///     required settings are found to be missing.
        /// </param>
        public static bool VerifyAppSettings(string[] requiredSettingKeys, bool throwException)
        {
            bool settingsVerified = false;
            NameValueCollection currentSettings = WebConfigurationManager.AppSettings;
            List<string> unsetSettingKeys = new List<string>();
            foreach (string key in requiredSettingKeys)
            {
                if (String.IsNullOrWhiteSpace(currentSettings[key]))
                    unsetSettingKeys.Add(key);
            }
            if (unsetSettingKeys.Count > 0)
            {
                if (throwException)
                {
                    int numOfUnsetSettings = unsetSettingKeys.Count;
                    StringBuilder message = new StringBuilder(
                        "This application requires that the following key");
                    if (numOfUnsetSettings > 1)
                        message.Append('s');
                    message.Append(" be set in web.config under <appSettings>: ");
                    for (int i = 0; i < numOfUnsetSettings; i++)
                    {
                        message.Append("\"" + unsetSettingKeys[i] + "\"");
                        if (i < numOfUnsetSettings - 1)
                            message.Append(", ");
                    }
                    message.Append('.');
                    throw new ApplicationException(message.ToString());
                }
            }
            else
            {
                settingsVerified = true;
            }
            return settingsVerified;
        }

        /// <summary>
        ///     Verifies the existence of a specified appSetting in web.config and optionally throws
        ///     an exception if it is found to be missing.
        /// </summary>
        /// <param name="requiredSettingKey">
        ///     A System.String object representing the required setting key.
        /// </param>
        /// <param name="throwException">
        ///     A System.Boolean value that determines whether the method throws an exception when
        ///     the required setting is found to be missing.
        /// </param>
        public static bool VerifyAppSettings(string requiredSettingKey, bool throwException)
        {
            string[] requiredSettingKeys = { requiredSettingKey };
            return VerifyAppSettings(requiredSettingKeys, throwException);
        }

        /// <summary>
        ///     Verifies the existence of specified appSettings in web.config and throws an
        ///     exception when any are found to be missing.
        /// </summary>
        /// <param name="requiredSettingKeys">
        ///     A System.String array of required setting keys.
        /// </param>
        public static void VerifyAppSettings(string[] requiredSettingKeys)
        {
            VerifyAppSettings(requiredSettingKeys, true);
        }

        /// <summary>
        ///     Verifies the existence of a specified appSetting in web.config and throws an
        ///     exception if it is found to be missing.
        /// </summary>
        /// <param name="requiredSettingKey">
        ///     A System.String object representing the required setting key.
        /// </param>
        public static void VerifyAppSettings(string requiredSettingKey)
        {
            VerifyAppSettings(requiredSettingKey, true);
        }

        /// <summary>
        ///     Determines whether the user agent's screen size is small.
        /// </summary>
        public static bool DetectSmallScreenUserAgent()
        {
            bool smallScreenAgentDetected = false;
            string smallScreenAgentIdentifiersList =
                WebConfigurationManager.AppSettings[
                    "nysf_Utility_SmallScreenAgentIdentifiers"];
            string[] smallScreenAgentIdentifiers = Regex.Replace(Regex.Replace(
                smallScreenAgentIdentifiersList, "\\s+,", ","), ",\\s+", ",").Split(',');
            string currentUserAgent = HttpContext.Current.Request.UserAgent.ToLower();
            foreach (string identifier in smallScreenAgentIdentifiers)
            {
                if (currentUserAgent.Contains(identifier.ToLower()))
                {
                    smallScreenAgentDetected = true;
                    break;
                }
            }
            return smallScreenAgentDetected;
        }

        /// <summary>
        ///     Builds an all-caps name without extra whitespace and non-letters.
        /// </summary>
        /// <param name="name">
        ///     The name to be regularized.
        /// </param>
        public static string RegularizeName(string name)
        {
            string tempName = Regex.Replace(name.Trim().ToUpper(), "[^A-Z\\s]", "");
            tempName = Regex.Replace(tempName, "\\s", " ");
            while (tempName.Contains("  "))
                tempName = tempName.Replace("  ", " ");
            return tempName;
        }

        public static string RegularizeAddress(string rawAddress, bool convertAbbreviations)
        {
            if (convertAbbreviations)
            {
                // TODO: check for addressAbbreviations.xml file
            }
            string modifiedAddress = Regex.Replace(rawAddress, "[^a-zA-Z0-9#]", " ").Trim();
            if (modifiedAddress.Length == 0)
                return "";
            StringBuilder addressWip =
                new StringBuilder(modifiedAddress.TrimEnd('#').TrimStart('#').ToUpper());
            for (int i = addressWip.Length-2; i >= 0; i--)
                if (addressWip[i] == '#' && !Char.IsNumber(addressWip[i + 1]))
                    addressWip.Replace('#',' ', i, 1);
            for (int i = addressWip.ToString().LastIndexOf('#')-1; i >= 0; i--)
                if (addressWip[i] == '#')
                    addressWip.Replace('#',' ', i, 1);
            addressWip.Replace("#", " #");
            while (Regex.IsMatch(addressWip.ToString(), "  "))
                addressWip.Replace("  ", " ");
            if (convertAbbreviations)
            {
                string[] words = addressWip.ToString().Trim().Split(' ');
                bool suffixIsAbbreviated = false;
                bool secondaryUnitIsAbbreviated = false;
                bool changed = false;
                for (int i = words.Length - 1; i >= 0; i--)
                {
                    if (!suffixIsAbbreviated && !secondaryUnitIsAbbreviated)
                    {
                        foreach (string key in SecondaryUnitAbbreviations.Keys)
                        {
                            if (words[i] == key)
                                secondaryUnitIsAbbreviated = true;
                            else
                                foreach (string value in SecondaryUnitAbbreviations[key])
                                {
                                    if (words[i] == value)
                                    {
                                        words[i] = key;
                                        secondaryUnitIsAbbreviated = changed = true;
                                        break;
                                    }
                                }
                            if (secondaryUnitIsAbbreviated)
                                break;
                        }
                        if (secondaryUnitIsAbbreviated)
                            continue;
                    }
                    if (!suffixIsAbbreviated)
                    {
                        foreach (string key in StreetSuffixAbbreviations.Keys)
                        {
                            if (words[i] == key)
                                suffixIsAbbreviated = true;
                            else
                                foreach (string value in StreetSuffixAbbreviations[key])
                                {
                                    if (words[i] == value)
                                    {
                                        words[i] = key;
                                        suffixIsAbbreviated = changed = true;
                                        break;
                                    }
                                }
                            if (suffixIsAbbreviated)
                                break;
                        }
                        if (suffixIsAbbreviated)
                            continue;
                    }
                }
                if (changed)
                {
                    addressWip = new StringBuilder(words[0]);
                    for (int w = 1; w < words.Length; w++)
                        addressWip.Append(" " + words[w]);
                }
            }
            return addressWip.ToString().Trim();
        }

        public static string RegularizeAddress(string rawAddress)
        {
            return RegularizeAddress(rawAddress, true);
        }

        private static Dictionary<string,List<string>> StateAbbreviations
        {
            get
            {
                Dictionary<string, List<string>> stateAbbrevs = (Dictionary<string, List<string>>)
                    HttpContext.Current.Cache[StateAbbrevsCacheKey];
                if (stateAbbrevs != null)
                    return stateAbbrevs;
                string abbrevsDataPath =
                    HttpContext.Current.Server.MapPath(PathToAbbrevData);
                XmlTextReader reader = new XmlTextReader(abbrevsDataPath);
                if (!reader.ReadToFollowing("addressAbbreviations"))
                    throw new XmlException("Can't find <addressAbbreviations> node.");
                if (!reader.ReadToDescendant("states"))
                    throw new XmlException("Can't find <states> node.");
                if (!reader.ReadToDescendant("state"))
                    throw new XmlException("Can't find any <state> nodes.");
                stateAbbrevs = new Dictionary<string, List<string>>();
                do
                {
                    if (!reader.MoveToAttribute("abbrev"))
                        throw new XmlException("Can't find \"abbrev\" attribute for <state>.");
                    string abbrev = reader.Value.Trim().ToUpper();
                    if (!reader.MoveToAttribute("name"))
                        throw new XmlException("Can't find \"name\" attribute for <state>.");
                    string name = reader.Value.Trim().ToUpper();
                    if (stateAbbrevs.ContainsKey(abbrev))
                        stateAbbrevs[abbrev].Add(name);
                    else
                    {
                        List<string> names = new List<string>();
                        names.Add(name);
                        stateAbbrevs.Add(abbrev, names);
                    }
                }
                while (reader.ReadToNextSibling("state"));
                CacheDependency dependency = new CacheDependency(abbrevsDataPath);
                HttpContext.Current.Cache.Insert(StateAbbrevsCacheKey, stateAbbrevs, dependency);
                return stateAbbrevs;
            }
        }

        private static Dictionary<string, List<string>> StreetSuffixAbbreviations
        {
            get
            {
                Dictionary<string, List<string>> suffixAbbrevs = (Dictionary<string, List<string>>)
                    HttpContext.Current.Cache[StreetSuffixAbbrevsCacheKey];
                if (suffixAbbrevs != null)
                    return suffixAbbrevs;
                string abbrevsDataPath =
                    HttpContext.Current.Server.MapPath(PathToAbbrevData);
                XmlTextReader reader = new XmlTextReader(abbrevsDataPath);
                if (!reader.ReadToFollowing("addressAbbreviations"))
                    throw new XmlException("Can't find <addressAbbreviations> node.");
                if (!reader.ReadToDescendant("streetSuffixes"))
                    throw new XmlException("Can't find <streetSuffixes> node.");
                if (!reader.ReadToDescendant("suffix"))
                    throw new XmlException("Can't find any <suffix> nodes.");
                suffixAbbrevs = new Dictionary<string, List<string>>();
                do
                {
                    if (!reader.MoveToAttribute("abbrev"))
                        throw new XmlException("Can't find \"abbrev\" attribute for <suffix>.");
                    string abbrev = reader.Value.Trim().ToUpper();
                    reader.MoveToElement();
                    if (!reader.ReadToDescendant("alt"))
                        throw new XmlException("Can't find any <alt> nodes under <suffix>.");
                    List<string> alts = new List<string>();
                    do
                    {
                        reader.Read();
                        if (String.IsNullOrWhiteSpace(reader.Value))
                            throw new XmlException("Can't find content for <alt> node.");
                        alts.Add(reader.Value.Trim().ToUpper());
                        reader.Read();
                    }
                    while (reader.ReadToNextSibling("alt"));
                    suffixAbbrevs.Add(abbrev, alts);
                }
                while (reader.ReadToNextSibling("suffix"));
                CacheDependency dependency = new CacheDependency(abbrevsDataPath);
                HttpContext.Current.Cache.Insert(
                    StreetSuffixAbbrevsCacheKey, suffixAbbrevs, dependency);
                return suffixAbbrevs;
            }
        }

        private static Dictionary<string, List<string>> SecondaryUnitAbbreviations
        {
            get
            {
                Dictionary<string, List<string>> unitAbbrevs = (Dictionary<string, List<string>>)
                    HttpContext.Current.Cache[SecondaryUnitAbbrevsCacheKey];
                if (unitAbbrevs != null)
                    return unitAbbrevs;
                string abbrevsDataPath =
                    HttpContext.Current.Server.MapPath(PathToAbbrevData);
                XmlTextReader reader = new XmlTextReader(abbrevsDataPath);
                if (!reader.ReadToFollowing("addressAbbreviations"))
                    throw new XmlException("Can't find <addressAbbreviations> node.");
                if (!reader.ReadToDescendant("secondaryUnits"))
                    throw new XmlException("Can't find <secondaryUnits> node.");
                if (!reader.ReadToDescendant("unit"))
                    throw new XmlException("Can't find any <unit> nodes.");
                unitAbbrevs = new Dictionary<string, List<string>>();
                do
                {
                    if (!reader.MoveToAttribute("abbrev"))
                        throw new XmlException("Can't find \"abbrev\" attribute for <unit>.");
                    string abbrev = reader.Value.Trim().ToUpper();
                    if (!reader.MoveToAttribute("name"))
                        throw new XmlException("Can't find \"name\" attribute for <unit>.");
                    string name = reader.Value.Trim().ToUpper();
                    if (unitAbbrevs.ContainsKey(abbrev))
                        unitAbbrevs[abbrev].Add(name);
                    else
                    {
                        List<string> names = new List<string>();
                        names.Add(name);
                        unitAbbrevs.Add(abbrev, names);
                    }
                }
                while (reader.ReadToNextSibling("unit"));
                CacheDependency dependency = new CacheDependency(abbrevsDataPath);
                HttpContext.Current.Cache.Insert(
                    SecondaryUnitAbbrevsCacheKey, unitAbbrevs, dependency);
                return unitAbbrevs;
            }
        }

        public static string MakeInitials(string name, bool keepCapitals)
        {
            StringBuilder initials = new StringBuilder(name);
            for (int c = initials.Length - 1; c > 0; c--)
            {
                if ((!keepCapitals || (keepCapitals && !Char.IsUpper(initials[c])))
                    && Regex.IsMatch(initials[c - 1].ToString(), "[a-zA-Z]"))
                {
                    initials.Remove(c, 1);
                }
            }
            return Regex.Replace(initials.ToString(), "\\s+", "").ToUpper();
        }

        public static string MakeInitials(string name)
        {
            return MakeInitials(name, true);
        }

        public static string GetFullHrefFromSubpath(string subpath)
        {
            string clutteredVirtualPath =
                "/" + WebConfigurationManager.AppSettings["nysf_Utility_AppBase"]
                + "/" + subpath;
            // TODO: add final slash if directory
            return Regex.Replace(clutteredVirtualPath, "/+", "/");
        }

        public static void SetReferer(System.Web.UI.StateBag viewState)
        {
            string referer = null;
            HttpRequest request = HttpContext.Current.Request;

            // If available, set the referer to that specified by the query string

            if (!String.IsNullOrWhiteSpace(request.QueryString[RefererQueryStringKey]))
            {
                referer = request.QueryString[RefererQueryStringKey];
            }
            else
            {
                string httpHost = request.ServerVariables["HTTP_HOST"];

                // Otherwise fall back on the UrlReferrer, as long as it's from the same host

                if (request.UrlReferrer != null
                    && request.UrlReferrer.ToString().Contains(httpHost))
                {
                    referer = request.UrlReferrer.ToString();
                }

            }
            if (referer == null)
                referer = WebConfigurationManager.AppSettings["nysf_Utility_AppBase"];
            viewState.Add(RefererSessionKey, referer);
            
        }

        public static void RedirectToReferer(System.Web.UI.StateBag viewState)
        {
            HttpContext.Current.Response.Redirect(viewState[RefererSessionKey].ToString());
        }

        public static void RedirectWithReferer(string subpath, System.Web.UI.StateBag viewState)
        {
            HttpContext.Current.Response.Redirect(
                GetFullHrefWithReferer(subpath, viewState));
        }

        public static void RedirectWithReferer(string subpath, string customReferer)
        {
            HttpContext.Current.Response.Redirect(GetFullHrefWithReferer(subpath, customReferer));
        }

        public static string GetFullHrefWithReferer(string subpath,
            System.Web.UI.StateBag viewState)
        {
            // TODO: if cleanVirtualPath already has querystring, use & instead of ?
            return GetFullHrefFromSubpath(subpath) + "?" + RefererQueryStringKey + "="
                + HttpContext.Current.Server.UrlEncode(viewState[RefererSessionKey].ToString());
        }

        public static string GetFullHrefWithReferer(string subpath, string customReferer)
        {
            // TODO: if cleanVirtualPath already has querystring, use & instead of ?
            return GetFullHrefFromSubpath(subpath) + "?" + RefererQueryStringKey + "="
                + HttpContext.Current.Server.UrlEncode(customReferer);
        }

        public static string GetReferer(System.Web.UI.StateBag viewState)
        {
            return viewState[RefererSessionKey].ToString();
        }

		public static void CheckBrowserSession()
		{
			if (HttpContext.Current.Session != null
				&& HttpContext.Current.Session.IsNewSession)
			{
				string cookieHeader = HttpContext.Current.Request.Headers["Cookie"];
				if (cookieHeader != null
					&& cookieHeader.Contains("ASP.NET_SessionId"))
				{
					HttpContext.Current.Session.Clear();
					HttpContext.Current.Response.Redirect(
						WebConfigurationManager.AppSettings["nysf_Utility_TimeoutPageUrl"]);
				}
			}
		}
    }
}
// TODO: if ultimately not needed, remove commented-out makeCurrentPage features completely, and
//       comments that mention the feature


using System;
using System.Web.Configuration;
using Nysf.Tessitura;

namespace Nysf.UserControls
{

/// <summary>
///     Protects again session hijacking and fixation, maintains a Tessitura
///     session, tracks the current page, and handles disallowed anonymous
///     or authenticated users.
/// </summary>
public partial class SessionHandler : GenericControl
{
    #region Variables

    // private bool makeCurrentPage;
    private bool allowAnonymous;
    private bool allowAuthenticated;
    private bool allowTemporary;
    private bool enableSmallScreenDetection;
    private bool requireSecureConnection;
	private int sourceCode;

    #endregion

    #region Properties

    public bool RequireSecureConnection
    {
        get { return requireSecureConnection; }
        set { requireSecureConnection = value; }
    }

	public int SourceCode
	{
		get { return sourceCode; }
		set { sourceCode = value; }
	}

    /*/// <summary>
    ///     Whether the page should be designated as the current page.
    /// </summary>
    /// <remarks>
    ///     The current page is set in the session variable, "currentPage".  It
    ///     is mainly used when redirecting from a page that is disallowed.
    /// </remarks>
    public bool MakeCurrentPage
    {
        set
        {
            makeCurrentPage = value;
        }
    }*/

    /// <summary>
    ///     Whether the page may be viewed anonymously.
    /// </summary>
    /// <remarks>
    ///     If false, anonymous users will be redirected to the login page
    ///     specified as "loginPageUrl" in web.config.
    /// </remarks>
    public bool AllowAnonymous
    {
        set
        {
            // Make sure anonymous and authenticated users aren't disallowed
            // simultaneously.
            if (value == false && allowAuthenticated == false)
                throw new ArgumentException(
                    "Cannot disallow anonymous users while " +
                    "simultaneously disallowing authenticated users.");
            allowAnonymous = value;
        }
    }

    /// <summary>
    ///     Whether the page may be viewed by an authenticated user.
    /// </summary>
    /// <remarks>
    ///     If false, authenticated users will be redirected to the last page,
    ///     as specified by the session variable, "currentPage".
    /// </remarks>
    public bool AllowAuthenticated
    {
        set
        {
            // Make sure anonymous and authenticated users aren't disallowed
            // simultaneously.
            if (value == false && allowAnonymous == false)
                throw new ArgumentException(
                    "Cannot disallow authenticated users while " +
                    "simultaneously disallowing anonymous users.");
            allowAuthenticated = value;
        }
    }

    /// <summary>
    ///     Whether the page may be viewed by a temporary user.
    /// </summary>
    /// <remarks>
    ///     This is almost always false, except on the account activation page
    ///     and the logout page.
    /// </remarks>
    public bool AllowTemporary
    {
        set
        {
            allowTemporary = value;
        }
    }

    /// <summary>
    ///     Whether the session handler accounts for small-screen user agents.
    /// </summary>
    public bool EnableSmallScreenDetection
    {
        set
        {
            enableSmallScreenDetection = value;
        }
    }

    #endregion

    #region Methods

    public SessionHandler() : base()
    {
        // Set defaults

        //This line removes SSL security from tessitura connection. Not recommended. Use for expired certificate.
       //System.Net.ServicePointManager.ServerCertificateValidationCallback =((sender, certificate, chain, sslPolicyErrors) => true);
        //makeCurrentPage = true;
        allowAnonymous = true;
        allowAuthenticated = true;
        allowTemporary = false;
        enableSmallScreenDetection = true;
		sourceCode = WebClient.DefaultSourceCode;
    }

	protected void Page_Load(object sender, EventArgs e)
	{
		// Handle expired browser sessions

		Utility.CheckBrowserSession();

        // Redirect to secure page if necessary

        if (RequireSecureConnection)
        {
            if (!Request.IsSecureConnection && Boolean.Parse(
                WebConfigurationManager.AppSettings["nysf_UserControls_EnableRedirectsToHttps"]))
            {
                Response.Redirect(Request.Url.ToString().Replace("http://", "https://"));
            }
        }
        else
        {
            if (Request.IsSecureConnection)
                Response.Redirect(Request.Url.ToString().Replace("https://", "http://"));
        }

        WebClient.MaintainTessSession(sourceCode);

        // Detect the user agent's screen size

        if (enableSmallScreenDetection)
        {
            if (Session[TargetSmallScreenUserAgentSessionKey] == null)
                Session.Add(TargetSmallScreenUserAgentSessionKey, Utility.DetectSmallScreenUserAgent());
        }
        else
            Session.Add(TargetSmallScreenUserAgentSessionKey, false);

        // Redirect temporary user to activation page

        if (!allowTemporary && Session[WebClient.TempLoginSessionKey] != null &&
            (bool)Session[WebClient.TempLoginSessionKey])
        {
            /*if (makeCurrentPage)
                Session.Add("currentPage", Request.Url.ToString());*/
            Response.Redirect(Utility.GetFullHrefFromSubpath(
                WebConfigurationManager.AppSettings["nysf_UserControls_ActivationPageUrl"]));
        }

        // Redirect authenticated user to the last page if necessary

        if (!allowAuthenticated && WebClient.IsLoggedIn())
        {
            // TODO: redirect to querystringed page if submitted, otherwise referer
            Response.Redirect(WebConfigurationManager.AppSettings["nysf_Utility_AppBase"]);
        }

        /*// Designate this page as current

        if (makeCurrentPage)
            Session.Add("currentPage",Request.Url.ToString());*/

        // Redirect anonymous user to the login page if necessary

        if (!allowAnonymous && !WebClient.IsLoggedIn())
        {
            Utility.RedirectWithReferer(
                WebConfigurationManager.AppSettings["nysf_UserControls_LoginPageUrl"],
                Request.Url.ToString());
        }
	}

    #endregion
}

}
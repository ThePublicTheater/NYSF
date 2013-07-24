// TODO: remove references to ForceOrganization

using System;
using System.Web.Configuration;
using Nysf.Types;

namespace Nysf.UserControls
{

/// <summary>
///     Displays a link-reference to a stylesheet based on the designated
///     organization and on the detected screen-size of the user.
/// </summary>
public partial class StyleLinkRenderer : GenericControl
{
    #region Variables

    private Organization organization;
    //private bool forceOrganization;
    private string startOfUrlToStylesheets;

    #endregion

    #region Properties

    /// <summary>
    ///     The organization for which the page should be styled.
    /// </summary>
    public string Organization
    {
        set
        {
            try
            {
                organization =
                    (Organization)Enum.Parse(typeof(Organization), value, true);
            }
            catch (Exception e)
            {
                throw new ArgumentException("The organization must be a " +
                    "member of the Nysf.Types.Organizations " +
                    "enumeration.", e);
            }
        }
    }

    /*/// <summary>
    ///     Whether the organization for this StyleSelector should override the
    ///     previously established organization.
    /// </summary>
    /// <remarks>
    ///     Once established, the organization is stored in the session
    ///     variable, targetOrganization.
    /// </remarks>
    public bool ForceOrganization
    {
        set { forceOrganization = value; }
    }*/

    /// <summary>
    ///     The start of the URL to the set of stylesheets, including the
    ///     style's base name but leaving off postfixes and ".css".
    /// </summary>
    /// <remarks>
    ///     The set of stylesheets follows a naming convention:
    ///     
    ///         basename-org-screensize.css
    ///         
    ///     basename   - a custom name that is common to all of the stylesheets
    ///     org        - an abbreviation of the organization for which the
    ///                      stylesheets is intended ("pt", "jp", "sitp")
    ///     screensize - the size of the screen for which the stylesheet is
    ///                      intended ("largescreen" or "smallscreen")
    /// </remarks>
    public string StartOfUrlToStylesheets
    {
        set { startOfUrlToStylesheets = value; }
    }

    #endregion

    #region Methods

    public StyleLinkRenderer() : base()
    {
        // Set default values (which can be overridden by properties)

        switch (WebConfigurationManager.AppSettings[
                    "nysf_UserControls_DefaultOrganization"].Trim().ToLower())
        {
            case "pt":
                organization = Types.Organization.PublicTheater;
                break;
            case "jp":
                organization = Types.Organization.JoesPub;
                break;
            case "sitp":
                organization = Types.Organization.ShakespeareInThePark;
                break;
            default:
                throw new ApplicationException("An invalid organization was specified in "
                    + "web.config under <appSettings> key, "
                    + "\"nysf_UserControls_DefaultOrganization\". It must be set to \"pt\", "
                    + "\"jp\", or \"sitp\".");
        }
        //forceOrganization = true;
        startOfUrlToStylesheets =
            WebConfigurationManager.AppSettings[
                "nysf_UserControls_DefaultStartOfUrlToStylesheets"].Trim();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[TargetSmallScreenUserAgentSessionKey] == null)
            throw new ApplicationException("The StyleLinkRenderer requires that the user agent's " +
                "screen size have been assigned by the SessionHandler.");
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        bool smallScreenAgentDetected;
        if (Session[TargetSmallScreenUserAgentSessionKey] == null)
            smallScreenAgentDetected = false;
        else
            smallScreenAgentDetected = (bool)Session[TargetSmallScreenUserAgentSessionKey];

        // Construct the URL to the stylesheet

        string organizationPostfix = "";
        string screenSizePostfix = "";
        switch (organization)
        {
            case Types.Organization.PublicTheater:
                organizationPostfix = "pt";
                break;
            case Types.Organization.JoesPub:
                organizationPostfix = "jp";
                break;
            case Types.Organization.ShakespeareInThePark:
                organizationPostfix = "sitp";
                break;
        }
        if(smallScreenAgentDetected)
        {
            screenSizePostfix = "smallscreen";
        }
        else
        {
            screenSizePostfix = "largescreen";
        }
        string urlToStylesheet = startOfUrlToStylesheets + "-" +
                                 organizationPostfix + "-" +
                                 screenSizePostfix + ".css";

        // Append the stylesheet URL to the generated HTML

        LinkToStylesheet.Attributes.Add("href", urlToStylesheet);
    }

    #endregion
}

}
using System;
using System.Web.Configuration;

namespace Nysf.UserControls
{

    /// <summary>
    ///     A link that switches the designated screen size.
    /// </summary>
    public partial class ScreenSizeSwitch : GenericControl
    {
        #region Properties

        public string SwitchToLargeScreenText { get; set; }
        public string SwitchToSmallScreenText { get; set; }

        #endregion

        #region Methods

        public ScreenSizeSwitch() : base()
        {
            SwitchToLargeScreenText = "Switch to largescreen version";
            SwitchToSmallScreenText = "Switch to smallscreen version";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[TargetSmallScreenUserAgentSessionKey] == null)
            {
                throw new ApplicationException("The ScreenSizeSwitch requires that the user "
                    + "agent's screen size have been detected by the SessionHandler control.");
            }
            else
            {
                bool targetSmallScreenUserAgent = false;
                if (String.IsNullOrEmpty(Request.QueryString["targetUserAgent"]))
                {
                    targetSmallScreenUserAgent = 
                        (bool)Session[TargetSmallScreenUserAgentSessionKey];
                }
                else
                {

                    if (Request.QueryString["targetUserAgent"] == "largescreen")
                        targetSmallScreenUserAgent = false;
                    else if (Request.QueryString["targetUserAgent"] == "smallscreen")
                        targetSmallScreenUserAgent = true;
                    Session[TargetSmallScreenUserAgentSessionKey] = targetSmallScreenUserAgent;
                }
                string baseUrl = Request.Path + "?targetUserAgent=";
                if (targetSmallScreenUserAgent)
                {
                    SwitchHyperLink.CssClass = "SwitchToLargeScreenLink";
                    SwitchHyperLink.NavigateUrl = baseUrl + "largescreen";
                    SwitchHyperLink.Text = SwitchToLargeScreenText;
                }
                else
                {
                    SwitchHyperLink.CssClass = "SwitchToSmallScreenLink";
                    SwitchHyperLink.NavigateUrl = baseUrl + "smallscreen";
                    SwitchHyperLink.Text = SwitchToSmallScreenText;
                }
            }
        }

        #endregion
    }

}
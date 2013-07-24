// TODO: remove references to SessionHandler's "current page" feature

using System;
using Nysf.Tessitura;
using System.Web.Configuration;

namespace Nysf.UserControls
{

/// <summary>
///     Logs out the currently authenticated constituent and redirects
///     to the last page or homepage.
/// </summary>
/// <remarks>
///     If placed on a page with a SessionHandler, the SessionHandler should
///     disallow anonymous users and should not make the page current.
/// </remarks>
public partial class LogoutHandler : GenericControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        WebClient.Logout();
        Response.Redirect(WebConfigurationManager.AppSettings["nysf_Utility_AppBase"]);
    }
}

}
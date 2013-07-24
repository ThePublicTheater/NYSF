using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.Apps.OldStyleTickets
{
    public partial class ProdDetails : GenericControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[WebClient.TessSessionKeySessionKey] == null)
                throw new ApplicationException("PerfDetails requires that a Tessitura "
                    + "session have been set by the SessionHandler control.");

            int requestedProd = -1;
            int requestedPerf = -1;

            if (!String.IsNullOrWhiteSpace(Request.QueryString["perf"]))
                Int32.TryParse(Request.QueryString["perf"], out requestedPerf);

            if (String.IsNullOrWhiteSpace(Request.QueryString["prod"])
                || !Int32.TryParse(Request.QueryString["prod"], out requestedProd))
            {
                // Determine the production from the performance ID

                if (requestedPerf >= 0)
                {
                    Performance[] perfs = WebClient.GetPerformances();
                    foreach (Performance perf in perfs)
                    {
                        if (perf.Id == requestedPerf)
                        {
                            requestedProd = perf.ProductionId;
                            break;
                        }
                    }
                }
            }

            if (requestedProd < 0)
            {
                ProductionViews.ActiveViewIndex = 1;
            }

            else
            {
                // Prepare the calendar link

                CalendarLink.NavigateUrl =
                    WebConfigurationManager.AppSettings[
                        "nysf_Apps_Ost_CalendarPageBaseUrl"].ToString();

                // Collect information on the production

                Production prod = WebClient.GetProduction(requestedProd);

                if (prod == null)
                    ProductionViews.ActiveViewIndex = 1;
                else
                {
                    Performance[] perfs = prod.Performances;

                    List<string> venueNames = new List<string>();
                    foreach (Performance perf in perfs)
                    {
                        if (!venueNames.Contains(perf.VenueName))
                        {
                            venueNames.Add(perf.VenueName);
                        }
                    }

                    StringBuilder locations = new StringBuilder();
                    foreach (string name in venueNames)
                    {
                        locations.Append(" / " + name);
                    }
                    if (locations.Length > 0)
                        locations.Remove(0, 3); // remove the starting comma-space

                    // Insert production information into the page

                    TitleBlurb.Text = prod.Title;
                    LocationBlurb.Text = locations.ToString();
                    SynopsisBlurb.Text = prod.Synopsis;



                    // Add the order link if production has perfs on sale with seats available

                    if (prod.IsOnSale && !prod.IsSoldOut)
                    {
                        // TODO: make sure this link is well-formed with both directories and aspx
                        //       files
                        OrderLink.NavigateUrl = WebConfigurationManager.AppSettings[
                            "nysf_Apps_Ost_OrderPageBaseUrl"].ToString()
                                + "/?prod=" + requestedProd.ToString();
                        if (requestedPerf >= 0)
                        {
                            OrderLink.NavigateUrl += "&perf=" + requestedPerf.ToString();
                        }
                    }
                else
                    {
                        OrderLink.Visible = false;
                        if (prod.IsOnSale && prod.IsSoldOut)
                            SoldOutBlurb.Visible = true;
                    }
                }
            }
        }
    }
}
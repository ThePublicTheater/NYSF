using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using System.Web.Configuration;
using System.Text;

namespace tickets.usercontrols
{
    public partial class cartWidget : System.Web.UI.UserControl
    {

        private string loadingImageUrl;
        public string LoadingImageUrl
        {
            get {
                return loadingImageUrl;
            }
            set {
                loadingImageUrl = value;
            }
        }
        private string title;
        public string Title
        {
            get
            {
                return title;
            }
            set
            {
                title = value;
            }
        }

        public cartWidget()
        {
            loadingImageUrl = "";
            title = "Order Summary";
        }
        static Dictionary<int, string> SeatingChartFilenames;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                titleLiteral.Text = title;
                UpdateProgress1.ProgressTemplate = new Nysf.Apps.OldStyleTickets.ProgressTemplate(loadingImageUrl);
            }
            Cart cart = WebClient.GetCart();
            foreach (string key in Request.Form.AllKeys)
            {
                if (!string.IsNullOrEmpty(key))
                {
                    if (key.StartsWith("remove_"))
                    {
                        string[] parts = key.Split('_');
                        WebClient.RemoveSeatGroupFromCart(Int32.Parse(parts[1]), Int32.Parse(parts[2]));

                        cart = WebClient.GetCart();
                        if (!cart.HasItems)
                            Response.Redirect(Request.RawUrl);
                    }
                }
            }
            buildCart(cart);
        }
        public void buildCart(Cart cart)
        {
            if (cart != null && cart.HasItems)
            {
                string html = "";
                orderSummaryDiv.Style["Height"] = (300 + (cart.SeatGroups.Count - 1) * 170).ToString() + "px";
                foreach (CartSeatGroupItem item in cart.SeatGroups)
                {
                    string seatingChartHref = null;
                    seatingChartHref = GetSeatingChartHref(item.Performance.VenueId);

                    html += "<ul>";
                    html += ("<li class=\"perfNameRow\">" + item.Performance.Name + "</li>");
                    html += ("<li><span class=\"orderSumLeftCol\">Date/time: </span><span class=\"orderSumRightCol\">" + item.Performance.StartTime.ToString("MMM d, h:mm tt") + "</span></li>");
                    html += ("<li><span class=\"orderSumLeftCol\">Venue: </span><span class=\"orderSumRightCol\">" + item.Performance.VenueName + "</span></li>");
                    html += ("<li><span class=\"orderSumLeftCol\">Seat" + ((item.SeatCount > 1) ? "s" : "") + ":</span><span class=\"orderSumRightCol\">" + getSeatString(item) + "</span></li>");
                    html += ("<li><span class=\"orderSumLeftCol\">Quantity/Unit Price: </span><span class=\"orderSumRightCol\">" + item.SeatCount.ToString() + "@ " + (item.SubTotal / item.SeatCount).ToString("C") + "</span></li>");
                    html += ("<li><span class=\"orderSumLeftCol\">Price: </span><span class=\"orderSumRightCol\">" + item.SubTotal.ToString("C") + "</span></li>");
                    if (!String.IsNullOrEmpty(seatingChartHref))
                        html += ("<li class=\"alignRight\">" + "<a href=\"" + seatingChartHref + "\" target=\"_blank\" title=\""
                            + item.Performance.VenueName + " seating chart\">Seating "
                            + "chart</a>" + "</li>");
                    html += ("<li class=\"alignRight\"><input type=\"submit\"  class=\"submitLink\" name=\"remove_"
                         + item.Performance.Id.ToString() + "_" + item.Id.ToString() + "\" value=\"Remove\" /></li>");
                    html += "</ul>";

                    html += "<br/>";

                }
                summaryDiv.InnerHtml = html;
                subTotalDiv.InnerHtml = "Subtotal: " + cart.SubTotal.ToString("C") + "<br/>";
                foreach (CartFee fee in cart.FeeSumPerType)
                {
                    subTotalDiv.InnerHtml += (fee.Description + ": ");
                    subTotalDiv.InnerHtml += fee.Amount.ToString("C") + "<br/>"; ;
                }
                summaryFooter.InnerHtml = "Grand Total:" + cart.Total.ToString("C");
            }
            else
            {
                summaryDiv.InnerHtml = "Cart is empty";
                orderSummaryDiv.Style["Height"] = "50px";
                summaryFooter.InnerHtml = "";
            }
            

        }
        private string GetSeatingChartHref(int venueId)
        {
            // TODO: make sure this is actually null by default:
            if (SeatingChartFilenames == null)
            {
                string toParse =
                    WebConfigurationManager.AppSettings["nysf_Apps_Ost_SeatingChartImageNames"];
                string[] venueFileNames = toParse.Split('?');
                SeatingChartFilenames = new Dictionary<int, string>();
                foreach (string venueFileName in venueFileNames)
                {
                    string[] parts = venueFileName.Split(':');
                    SeatingChartFilenames.Add(Int32.Parse(parts[0]), parts[1]);
                }
            }
            if (SeatingChartFilenames.Keys.Contains(venueId))
                return WebConfigurationManager.AppSettings["nysf_Apps_Ost_SeatingChartImageDir"]
                    + "/" + SeatingChartFilenames[venueId];
            else
                return null;
        }
        private string getSeatString(CartSeatGroupItem seatGroup)
        {
            StringBuilder seatText = new StringBuilder("");
            foreach (CartPriceTypeSeats priceTypeSeats in seatGroup.SeatsPerPriceTypes)
            {
                foreach (CartSeat seat in priceTypeSeats.Seats)
                {
                    seatText.Append(" " + seat.RowIdentifier + "-"
                        + seat.SeatNumInRow.ToString() + ",");
                }
            }
            return seatText.ToString().Substring(0, seatText.Length - 1);//to eliminate trailing comma
        }
    }
}
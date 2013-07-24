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
    public partial class CartViewer : GenericControl
    {

        static Dictionary<int, string> SeatingChartFilenames;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                Utility.SetReferer(ViewState);

            // Process cart item removals

            foreach (string key in Request.Form.AllKeys)
            {
                if (key.StartsWith("remove_"))
                {
                    string[] parts = key.Split('_');
                    WebClient.RemoveSeatGroupFromCart(Int32.Parse(parts[1]), Int32.Parse(parts[2]));
                    Response.Redirect(Request.RawUrl);
                }
            }

            Cart cart = WebClient.GetCart();

            if (cart != null && cart.HasItems)
            {
                // Add cart items and totals to table

                int addAtIndex = 2;
                foreach (CartSeatGroupItem seatGroup in cart.SeatGroups)
                {
                    TableRow newRow = new TableRow();
                    TableCell dateCell = new TableCell();
                    dateCell.Text = seatGroup.Performance.StartTime.ToString("MMM dd, h:mm tt");
                    newRow.Cells.Add(dateCell);
                    StringBuilder eventText = new StringBuilder();
                    eventText.AppendLine(seatGroup.Performance.Name + "<br />");
                    eventText.AppendLine(seatGroup.Performance.VenueName + "<br />");
                    eventText.AppendLine(seatGroup.SeatingZoneName + "<br />");
                    StringBuilder seatText = new StringBuilder("Seat");
                    if (seatGroup.SeatCount > 1)
                        seatText.Append('s');
                    seatText.Append(":");
                    foreach (CartPriceTypeSeats priceTypeSeats in seatGroup.SeatsPerPriceTypes)
                    {
                        foreach (CartSeat seat in priceTypeSeats.Seats)
                        {
                            seatText.Append(" " + seat.RowIdentifier + " "
                                + seat.SeatNumInRow.ToString());
                        }
                    }
                    eventText.AppendLine(seatText.ToString());
                    string seatingChartHref = null;
                    seatingChartHref = GetSeatingChartHref(seatGroup.Performance.VenueId);
                    if (!String.IsNullOrEmpty(seatingChartHref))
                        eventText.AppendLine("<br /><a href=\"" + seatingChartHref + "\" title=\""
                            + seatGroup.Performance.VenueName + " seating chart\">Seating "
                            + "chart</a>");
                    TableCell eventCell = new TableCell();
                    eventCell.Text = eventText.ToString();
                    newRow.Cells.Add(eventCell);
                    StringBuilder countPriceText = new StringBuilder();
                    bool firstPassed = false;
                    foreach (CartPriceTypeSeats priceTypeSeats in seatGroup.SeatsPerPriceTypes)
                    {
                        if (firstPassed)
                            countPriceText.AppendLine("<br />");
                        else
                            firstPassed = true;
                        countPriceText.Append(priceTypeSeats.SeatCount.ToString()
                            + " @ " + priceTypeSeats.PricePerSeat.ToString("C"));
                    }
                    TableCell countPriceCell = new TableCell();
                    countPriceCell.Text = countPriceText.ToString();
                    newRow.Cells.Add(countPriceCell);
                    TableCell priceCell = new TableCell();
                    priceCell.Text = seatGroup.SubTotal.ToString("C") + "<br />"
                        + "<input type=\"submit\" name=\"remove_"
                        + seatGroup.Performance.Id.ToString() + "_"
                        + seatGroup.Id.ToString() + "\" value=\"Remove\" />";
                    newRow.Cells.Add(priceCell);
                    CartTable.Rows.AddAt(addAtIndex,newRow);
                    addAtIndex++;
                }

                SubTotalCell.Text = cart.SubTotal.ToString("C");

                addAtIndex++;
                foreach (CartFee fee in cart.FeeSumPerType)
                {
                    TableRow newRow = new TableRow();
                    TableCell descCell = new TableCell();
                    descCell.Text = fee.Description;
                    descCell.ColumnSpan = 3;
                    newRow.Cells.Add(descCell);
                    TableCell amtCell = new TableCell();
                    amtCell.Text = fee.Amount.ToString("C");
                    newRow.Cells.Add(amtCell);
                    CartTable.Rows.AddAt(addAtIndex, newRow);
                    addAtIndex++;
                }

                TotalCell.Text = cart.Total.ToString("C");

                CheckoutLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
                    WebConfigurationManager.AppSettings["nysf_UserControls_CheckOutPageUrl"]);
            }

            else
            {
                CartTable.Visible = false;
                CheckoutLinkLi.Visible = false;
                EmptyCartMessage.Visible = true;
            }

            //KeepShoppingLink.NavigateUrl = Utility. Utility.GetReferer(ViewState);
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
    }
}
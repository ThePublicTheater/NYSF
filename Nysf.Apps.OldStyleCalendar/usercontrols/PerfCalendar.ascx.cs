using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf;
using Nysf.Tessitura;
using Nysf.Types;

namespace Nysf.Apps.OldStyleTickets
{
    public partial class PerfCalendar : GenericControl
    {
        private static ushort maxCalendarYear;
        private static byte maxCalendarMonth;
        private const string calendarRowsCacheKeyBase = "nysf_Apps_Ost_CalendarRows_";
        public static string productionPageBaseUrl;

        static PerfCalendar()
        {
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;

            maxCalendarYear = (ushort)DateTime.Now.Year;
            maxCalendarMonth = (byte)(DateTime.Now.Month
                + Int32.Parse(appSettings["nysf_Apps_Ost_MaxCalendarMonthsAhead"]));
            while (maxCalendarMonth > 12)
            {
                maxCalendarYear++;
                maxCalendarMonth -= 12;
            }

            productionPageBaseUrl = appSettings["nysf_Apps_Ost_ProductionPageBaseUrl"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session[WebClient.TessSessionKeySessionKey] == null)
                throw new ApplicationException("The PerfCalendar requires that a Tessitura "
                    + "session have been set by the SessionHandler control.");

            // Determine the month to display

            byte requestedMonth;
            ushort requestedYear;
            bool couldParseMonth;
            bool couldParseYear;
            bool monthIsFuture;
            bool monthUnderMax;

            couldParseMonth = Byte.TryParse(Request.QueryString["month"], out requestedMonth);
            couldParseYear = ushort.TryParse(Request.QueryString["year"], out requestedYear);

            monthIsFuture = requestedYear > DateTime.Now.Year
                || (requestedYear == DateTime.Now.Year && requestedMonth >= DateTime.Now.Month);

            monthUnderMax = requestedYear < maxCalendarYear
                || (requestedYear == maxCalendarYear && requestedMonth <= maxCalendarMonth);

            if (!(couldParseMonth && couldParseYear && monthIsFuture))
            {
                requestedMonth = (byte)DateTime.Now.Month;
                requestedYear = (ushort)DateTime.Now.Year;
            }
            else if (!monthUnderMax)
            {
                requestedMonth = maxCalendarMonth;
                requestedYear = maxCalendarYear;
            }

            // Build the month & year blurb

            DateTime firstMinOfMonth = new DateTime(requestedYear, requestedMonth, 1);

            MonthYearBlurb.Text = firstMinOfMonth.ToString("MMMM yyyy");

            // Set up (or hide) the previous month link

            if (requestedYear == (ushort)DateTime.Now.Year
                && requestedMonth == (byte)DateTime.Now.Month)
            {
                PrevMonthLink.Visible = false;
            }
            else
            {
                ushort prevYear = requestedYear;
                byte prevMonth = (byte)(requestedMonth-1);
                if (prevMonth == 0)
                {
                    prevYear--;
                    prevMonth = 12;
                }
                PrevMonthLink.NavigateUrl = Request.Url.AbsolutePath
                    + "?year=" + prevYear.ToString()
                    + "&month=" + prevMonth.ToString();
            }

            // Set up (or hide) the next month link

            if (requestedYear == maxCalendarYear && requestedMonth == maxCalendarMonth)
            {
                NextMonthLink.Visible = false;
            }
            else
            {
                ushort nextYear = requestedYear;
                byte nextMonth = (byte)(requestedMonth + 1);
                if (nextMonth == 13)
                {
                    nextYear++;
                    nextMonth = 1;
                }
                NextMonthLink.NavigateUrl = Request.Url.AbsolutePath
                    + "?year=" + nextYear.ToString()
                    + "&month=" + nextMonth.ToString();
            }

            // Construct the rows of the calendar table

            // TODO: format requested month / year as double-digits
            string monthCacheKey = calendarRowsCacheKeyBase + requestedYear.ToString()
                    + requestedMonth.ToString();
            List<TableRow> weeks = null;
                //(List<TableRow>)Cache[monthCacheKey];
            if (weeks == null)
            {
                weeks = new List<TableRow>();

                DateTime lastMinOfMonth = firstMinOfMonth.AddMonths(1);
                lastMinOfMonth = new DateTime(lastMinOfMonth.Year, lastMinOfMonth.Month, 1, 23, 59, 59);
                lastMinOfMonth = lastMinOfMonth.AddDays(-1);

                byte lastInsertedDateCursor = 0;
                byte firstDateDayOfWeek = (byte)firstMinOfMonth.DayOfWeek;
                byte lastDateInMonth = (byte)lastMinOfMonth.Day;

                List<Performance> perfs;

                if (requestedMonth == DateTime.Now.Month && requestedYear == DateTime.Now.Year)
                    perfs = new List<Performance>(
                        WebClient.GetPerformances(DateTime.Now, lastMinOfMonth));
                else
                    perfs = new List<Performance>(
                        WebClient.GetPerformances(firstMinOfMonth, lastMinOfMonth));

                while (lastInsertedDateCursor < lastDateInMonth)
                {
                    TableRow newWeek = new TableRow();
                    byte dayOfWeekCursor = 0;
                    while (dayOfWeekCursor <= 6)
                    {
                        TableCell newDay = new TableCell();
                        if ((lastInsertedDateCursor == 0 && dayOfWeekCursor < firstDateDayOfWeek)
                            || lastInsertedDateCursor == lastDateInMonth)
                        {
                            newDay.Text = "<div class=\"Date\">&nbsp;</div>";
                            newDay.CssClass = "OutOfMonthDay";
                        }
                        else
                        {
                            lastInsertedDateCursor++;
                            StringBuilder dayContent = new StringBuilder();
                            dayContent.AppendLine(
                                "<div class=\"Date\">" + lastInsertedDateCursor.ToString() + "</div>");
                            dayContent.AppendLine("<dl>");
                            if (firstMinOfMonth >= DateTime.Now
                                || lastInsertedDateCursor >= DateTime.Now.Day)
                            {
                                while (perfs.Count > 0)
                                {
                                    if (perfs[0].StartTime.Day > lastInsertedDateCursor)
                                        break;
                                    else
                                    {
                                        string orgClass = // TODO: Fix MakeInitials
                                            Utility.MakeInitials(perfs[0].Organization.ToString());
                                        dayContent.AppendLine("<dt class=\"" + orgClass + "\">"
                                            + "<a href=\"" + productionPageBaseUrl + "/?prod="
                                            + perfs[0].ProductionId.ToString() + "&perf="
                                            + perfs[0].Id.ToString() + "\" title=\"Details About "
                                            + perfs[0].Name + "\">" + perfs[0].Name
                                            + "<span class=\"SelfEvident\"> (" + orgClass
                                            + ")</span></a></dt>");
                                        dayContent.AppendLine("<dd>"
                                            + perfs[0].StartTime.ToString("h:mm tt") + "</dd>");
                                        perfs.RemoveAt(0);
                                    }
                                }
                            }
                            dayContent.AppendLine("</dl>");
                            newDay.Text = dayContent.ToString();
                        }
                        newWeek.Cells.Add(newDay);
                        dayOfWeekCursor++;
                    }
                    weeks.Add(newWeek);
                }
                Cache.Insert(monthCacheKey, weeks, null,
                    DateTime.Now.AddMinutes(5), TimeSpan.Zero);
            }

            foreach (TableRow row in weeks)
                Calendar.Rows.Add(row);
        }
    }
}
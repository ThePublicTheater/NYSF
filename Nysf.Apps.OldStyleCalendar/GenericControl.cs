using System.Web;

namespace Nysf.Apps.OldStyleTickets
{
    public abstract class GenericControl : System.Web.UI.UserControl
    {
        protected static string PathToJoesPubSeatingConditions =
            "~/App_Data/pubSeatingConditions.xml";

        static GenericControl()
        {
            // Check for required entries in the config file

            string[] requiredSettingKeys =
            {
                "nysf_Apps_Ost_MaxCalendarMonthsAhead",
                "nysf_Apps_Ost_ProductionPageBaseUrl",
                "nysf_Apps_Ost_CalendarPageBaseUrl",
                "nysf_Apps_Ost_OrderPageBaseUrl",
                "nysf_Apps_Ost_MaxSeatQuantityChoicePerSection",
                "nysf_UserControls_CartPageUrl",
                "nysf_UserControls_CheckOutPageUrl",
                "nysf_Apps_Ost_SeatingChartImageDir",
                "nysf_Apps_Ost_SeatingChartImageNames",
                "nysf_Apps_Ost_SyosVenueIds"
            };
            Utility.VerifyAppSettings(requiredSettingKeys);

            // Verify the required data file

            if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(
                PathToJoesPubSeatingConditions)))
                throw new System.IO.FileNotFoundException("Unable to find the required file: "
                    + PathToJoesPubSeatingConditions);

        }

        public GenericControl() : base() { }
    }
}
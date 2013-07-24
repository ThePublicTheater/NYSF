using Nysf.Tessitura;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.Apps.VLineEntryForm
{

    public partial class VLineForm : System.Web.UI.UserControl
    {
        protected static byte openHour;
        protected static byte openMin;
        protected static byte closeHour;
        protected static byte closeMin;
        protected static byte pickupStartHour;
        protected static byte pickupStartMin;
        protected static byte pickupDueHour;
        protected static byte pickupDueMin;
        protected static byte startHour;
        protected static byte startMin;
        protected static DateTime debugDate;
        protected static string datePerformanceString;

        static VLineForm()
        {
            string[] requiredSettingKeys =
            {
                "nysf_Apps_Vlef_OpenHour",
                "nysf_Apps_Vlef_OpenMin",
                "nysf_Apps_Vlef_CloseHour",
                "nysf_Apps_Vlef_CloseMin",
                "nysf_Apps_Vlef_PickupStartHour",
                "nysf_Apps_Vlef_PickupStartMin",
                "nysf_Apps_Vlef_PickupDueHour",
                "nysf_Apps_Vlef_PickupDueMin",
                "nysf_Apps_Vlef_StartHour",
                "nysf_Apps_Vlef_StartMin"
            };
            Utility.VerifyAppSettings(requiredSettingKeys);
            NameValueCollection appSettings = WebConfigurationManager.AppSettings;
            openHour = Byte.Parse(appSettings["nysf_Apps_Vlef_OpenHour"]);
            openMin = Byte.Parse(appSettings["nysf_Apps_Vlef_OpenMin"]);
            closeHour = Byte.Parse(appSettings["nysf_Apps_Vlef_CloseHour"]);
            closeMin = Byte.Parse(appSettings["nysf_Apps_Vlef_CloseMin"]);
            pickupStartHour = Byte.Parse(appSettings["nysf_Apps_Vlef_PickupStartHour"]);
            pickupStartMin = Byte.Parse(appSettings["nysf_Apps_Vlef_PickupStartMin"]);
            pickupDueHour = Byte.Parse(appSettings["nysf_Apps_Vlef_PickupDueHour"]);
            pickupDueMin = Byte.Parse(appSettings["nysf_Apps_Vlef_PickupDueMin"]);
            startHour = Byte.Parse(appSettings["nysf_Apps_Vlef_StartHour"]);
            startMin = Byte.Parse(appSettings["nysf_Apps_Vlef_StartMin"]);
            if(!DateTime.TryParse(appSettings["nysf_Apps_Vlef_debugDate"].ToString(),out debugDate))
                debugDate=DateTime.Parse("1/1/1900");

        }

        protected void Page_Load(object sender, EventArgs e)
        {            
            if (!Page.IsPostBack)
            {
                DateTime dtCloseHour = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, closeHour, closeMin, 0);
                LotteryStatus lotteryStatus = GetCurrentLotteryStatus();

                if (lotteryStatus == LotteryStatus.PickupPassed)
                {
                    VLineViews.ActiveViewIndex = 4; // lottery over
                }
                else if (lotteryStatus == LotteryStatus.NoShow)
                {
                    VLineViews.ActiveViewIndex = 7; // no show today
                }
                else if (lotteryStatus == LotteryStatus.YearEnd)
                {
                    VLineViews.ActiveViewIndex = 8; // year over
                }
                else if (lotteryStatus == LotteryStatus.NotYetOpened)
                {
                    VLineViews.ActiveViewIndex = 10; // year over
                }
                else
                {
                    EntryStatus entryStatus = GetEntryStatus();
                    if (lotteryStatus == LotteryStatus.Closed)
                    {
                        switch (entryStatus)
                        {
                            case EntryStatus.NotEntered:
                                VLineViews.ActiveViewIndex = 4; // lottery over
                                break;
                            case EntryStatus.Pending:
                                if (DateTime.Now > dtCloseHour)
                                    VLineViews.ActiveViewIndex = 9;
                                else
                                    VLineViews.ActiveViewIndex = 6; // pending drawing
                                break;
                            case EntryStatus.WonGeneral:
                                VLineViews.ActiveViewIndex = 3; // won blurb
                                break;
                            case EntryStatus.WonDisabled:
                                // DisabledExtraReqBlurb.Visible = true;
                                VLineViews.ActiveViewIndex = 3; // won blurb
                                break;
                            case EntryStatus.WonSenior:
                                SeniorsExtraReqBlurb.Visible = true;
                                VLineViews.ActiveViewIndex = 3; // won blurb
                                break;
                            default: // as in Lost or Disqualified
                                VLineViews.ActiveViewIndex = 5; // lost blurb
                                break;
                        }

                    }
                    else
                    {
                        if (entryStatus == EntryStatus.NotEntered)
                        {
                            VLineViews.ActiveViewIndex = 0; // entry form
                        }
                        else
                        {
                            if (DateTime.Now > dtCloseHour)
                                VLineViews.ActiveViewIndex = 9;
                            else
                                VLineViews.ActiveViewIndex = 6; // pending drawing

                        }
                    }

                }
            }
           // VLineViews.ActiveViewIndex = 2;
        }

        protected void Page_PreRender(object sender, EventArgs e)
        {
            string sessionKey = Session[WebClient.TessSessionKeySessionKey].ToString();
            DataSet result;

            if (VLineViews.ActiveViewIndex < 4)
                NextLotteryOpeningBlurb.Visible = false;
            else if (VLineViews.ActiveViewIndex == 10)
                openTimeBlurb.Text = GetTimeString(openHour, openMin);
            else
            {
                bool skipPerf = DateTime.Now.Hour < startHour
                    || (DateTime.Now.Hour == startHour && DateTime.Now.Minute < startMin);
                result = WebClient.RawClient.ExecuteLocalProcedure(
                        SessionKey: sessionKey,
                        LocalProcedureId: 8011, // GetNextPerf
                        LocalProcedureValues: "@skipTodaysPerf="
                            + Convert.ToByte(skipPerf).ToString());
                if (result.Tables[0].Rows.Count > 0)
                {
                    DateTime nextPerfDateTime = Convert.ToDateTime(result.Tables[0].Rows[0][1]);
                    NextDateBlurb.Text = nextPerfDateTime.ToString("MMMM d");
                    NextLotteryOpeningBlurb.Visible = true;
                }
                else
                    NextLotteryOpeningBlurb.Visible = false;
            }


            result = WebClient.RawClient.ExecuteLocalProcedure(
                            SessionKey: sessionKey,
                            LocalProcedureId: 8011, // GetNextPerf
                            LocalProcedureValues: null);
            datePerformanceString = result.Tables[0].Rows[0][1].ToString() + " performance of " + result.Tables[0].Rows[0][0].ToString() + ".";
            int length = datePerformanceString.Length;
            datePerformanceString = datePerformanceString.Replace(":00 PM", " PM");
            if (length == datePerformanceString.Length) //there was no replacement
                datePerformanceString.Replace(":00 AM", "AM");


            switch (VLineViews.ActiveViewIndex)
            {
                case 0:

                    // Set up the entry form

                    if (!Page.IsPostBack)
                    {
                        

                        PerfTimeBlurb.Text = datePerformanceString;
                        PerfTimeBlurb2.Text = datePerformanceString;
                        
                        result = WebClient.RawClient.ExecuteLocalProcedure(
                            SessionKey: sessionKey,
                            LocalProcedureId: 8014, // GetLineTypes
                            LocalProcedureValues: null);
                        LineTypeField.DataSource = result.Tables[0];
                        LineTypeField.DataValueField = "id";
                        LineTypeField.DataTextField = "name";
                        LineTypeField.DataBind();
                        //AccountInfo account = WebClient.GetAccountInformation();
                        //string nameFieldText = account.People.Person1.FirstName;
                        //if (!String.IsNullOrWhiteSpace(account.People.Person1.MiddleName))
                        //    nameFieldText += " " + account.People.Person1.MiddleName;
                        //if (!String.IsNullOrWhiteSpace(account.People.Person1.LastName))
                        //    nameFieldText += " " + account.People.Person1.LastName;
                        //NameField.Text = nameFieldText;
                        //AddressField.Text = account.Address;
                    }
                    NextLotteryOpeningBlurb.Visible = false;
                    break;

                case 1:

                    // Set up verification screen

                    //if (LineTypeField.SelectedValue == "2")
                    //    DisabledCondition.Visible = true;
                    //else if (LineTypeField.SelectedValue == "3")
                    //    SeniorsCondition.Visible = true;
                    LineNameBlurb.Text = LineTypeField.SelectedItem.Text;
                    NameConfirmBlurb.Text = NameField.Text;
                    AddressConfirmBlurb.Text = AddressField.Text;

                    closingTimeBlurb.Text = GetTimeString(closeHour, closeMin);
                    break;

                case 2:

                    // Set up the confirmation screen

                 //   SeatingBlurb.Text = LineTypeField.SelectedItem.Text;
                    drawingTimeBlurb2.Text = GetTimeString(closeHour, closeMin);
                    break;

                case 3:

                    // Set up the won screen

                    pickupStartTimeBlurb.Text = GetTimeString(pickupStartHour, pickupStartMin);
                    pickupStartEndBlurb.Text = GetTimeString(pickupDueHour, pickupDueMin);
                    pickupStartEndBlurb1.Text = GetTimeString(pickupDueHour, pickupDueMin);
                    result = WebClient.RawClient.ExecuteLocalProcedure(
                        SessionKey: sessionKey,
                        LocalProcedureId: 8011,
                        LocalProcedureValues: null);
                    WinPerfNameBlurb.Text = result.Tables[0].Rows[0][0].ToString();
                    break;

                case 6:

                    // Set up the pending screen

                    PerfTimeBlurb3.Text = datePerformanceString;
                    DrawingTimeBlurb.Text = GetTimeString(closeHour, closeMin);
                    NextLotteryOpeningBlurb.Visible = false;
                    break;

                case 8:

                    // Set up the year over screen

                    NextLotteryOpeningBlurb.Visible = false;
                    break;
              case 10:

                    // Set up the year over screen

                    NextLotteryOpeningBlurb.Visible = false;
                    break;


            }
        }

        protected void ProcessInitialEntry(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                // TODO: set up details confirmation page
                VLineViews.ActiveViewIndex = 1;
                if (LineTypeField.SelectedItem.Text.ToLower().Contains("senior"))
                {
                    senior.Visible = true;
                    nonSenior.Visible = false;
                }
                else
                {
                    senior.Visible = false;
                    nonSenior.Visible = true;
                }
            }

        }

        protected LotteryStatus GetCurrentLotteryStatus()
        {
            LotteryStatus status = new LotteryStatus();

            // Check if there is a performance today

            DataSet result = WebClient.RawClient.ExecuteLocalProcedure(
                        SessionKey: Session[WebClient.TessSessionKeySessionKey].ToString(),
                        LocalProcedureId: 8011, // GetNextPerf
                        LocalProcedureValues: "@skipTodaysPerf=0");
            if (result.Tables[0].Rows.Count > 0)
            {
                DateTime nextPerfDateTime = Convert.ToDateTime(result.Tables[0].Rows[0][1]);
                DateTime compareDate = debugDate;
                if (compareDate == DateTime.Parse("1/1/1900"))//If the debug date was not defined.  
                    compareDate = DateTime.Now.Date;
                if (nextPerfDateTime.Date == compareDate.Date)
                {
                    DateTime nextOpenTime = GetNextMatchingDateTime(openHour, openMin);
                    DateTime nextCloseTime = GetNextMatchingDateTime(closeHour, closeMin);
                    DateTime pickupDueTime = GetNextMatchingDateTime(pickupDueHour, pickupDueMin);
                    if (nextOpenTime < nextCloseTime && nextOpenTime < pickupDueTime)
                    {
                        
                        if (nextOpenTime.Date == DateTime.Now.Date)
                            status = LotteryStatus.NotYetOpened;
                        else
                            status = LotteryStatus.PickupPassed;
                    }
                    else if (nextCloseTime < pickupDueTime)
                        status = LotteryStatus.Opened;
                    else
                        status = LotteryStatus.Closed;
                }
                else
                {
                    status = LotteryStatus.NoShow;
                }
            }
            else
                status = LotteryStatus.YearEnd;

            return status;
        }

        protected DateTime GetNextMatchingDateTime(byte hour, byte min)
        {
            DateTime match = new DateTime(
                DateTime.Now.Year,
                DateTime.Now.Month,
                DateTime.Now.Day,
                Convert.ToInt32(hour),
                Convert.ToInt32(min),
                0);
            if (match < DateTime.Now)
                match = match.AddDays(1);
            return match;
        }

        protected EntryStatus GetEntryStatus()
        {
            EntryStatus status;
            int constituentId = (int)WebClient.GetAccountInformation().CustomerNumber;
            DataSet result =
                WebClient.RawClient.ExecuteLocalProcedure(
                    SessionKey: Session[WebClient.TessSessionKeySessionKey].ToString(),
                    LocalProcedureId: 8009,
                    LocalProcedureValues: "@customer_no=" + constituentId.ToString());
            switch (Convert.ToChar(result.Tables[0].Rows[0][0]))
            {
                case 'd':
                    status = EntryStatus.Disqualified;
                    break;
                case 'p':
                    status = EntryStatus.Pending;
                    break;
                case 'l':
                    status = EntryStatus.Lost;
                    break;
                case 'g':
                    status = EntryStatus.WonGeneral;
                    break;
                case 'h':
                    status = EntryStatus.WonDisabled;
                    break;
                case 's':
                    status = EntryStatus.WonSenior;
                    break;
                default: // as in, case: 'n'
                    status = EntryStatus.NotEntered;
                    break;
            }
            return status;
        }

        protected enum LotteryStatus : byte
        {
            Opened, Closed, PickupPassed, NoShow, YearEnd, NotYetOpened
        }

        protected enum EntryStatus : byte
        {
            NotEntered, Disqualified, Pending, Lost, WonGeneral, WonDisabled, WonSenior
        }

        protected void ProcessSubmission(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                Nysf.Tessitura.AttributeCollection sysAtts = WebClient.GetAttributes();
                ConstituentAttributeCollection acctAtts = WebClient.GetConstituentAttributes();
                if (cbPTEmail.Checked)
                {
                    // if the box is checked

                    Nysf.Tessitura.Attribute attrJP = sysAtts.GetAttributeByName("cp_Em_The Public Theater");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrJP.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrJP.Id, 1, acctAtts.HasAttributeValuePair(attrJP.Id, 0));
                        }
                        catch { }
                    }
                }
                if (cbSitPEmail.Checked)
                {
                    // if the box is checked
                    Nysf.Tessitura.Attribute attrPT = sysAtts.GetAttributeByName("cp_Em_Shakespeare in the Park");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrPT.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrPT.Id, 1, acctAtts.HasAttributeValuePair(attrPT.Id, 0));
                        }
                        catch { }
                    }
                }

                if (cbJP.Checked)
                {
                    // if the box is checked
                    Nysf.Tessitura.Attribute attrPT = sysAtts.GetAttributeByName("cp_Em_Joe’s Pub");
                    // ignore if the value is already set to 1
                    if (!acctAtts.HasAttributeValuePair(attrPT.Id, 1))
                    {
                        try
                        {
                            // if the value is already 0, set doReplace to true
                            WebClient.AddAttribute(attrPT.Id, 1, acctAtts.HasAttributeValuePair(attrPT.Id, 0));
                        }
                        catch { }
                    }
                }
                  
                AccountInfo account = WebClient.GetAccountInformation();
                WebClient.RawClient.ExecuteLocalProcedure(
                    SessionKey: Session[WebClient.TessSessionKeySessionKey].ToString(),
                    LocalProcedureId: 8012, // record entry
                    LocalProcedureValues:
                        "@lineType=" + LineTypeField.SelectedValue.ToString()
                        + "&@customer_no=" + account.CustomerNumber.ToString()
                        + "&@ip=" + "127.0.0.1"//Request.UserHostAddress
                        + "&@inputName=" + WebClient.EncodeProcedureParam(NameField.Text.Trim())
                        + "&@inputAddress="
                            + WebClient.EncodeProcedureParam(AddressField.Text.Trim())
                        + "&@fixedName=" + Utility.RegularizeName(NameField.Text)
                        + "&@fixedAddress=" + Utility.RegularizeAddress(AddressField.Text));
                VLineViews.ActiveViewIndex = 2; // confirmation
            }
        }

        protected void ReturnToForm(object sender, EventArgs e)
        {
            VLineViews.ActiveViewIndex = 0; // entry form
        }

        protected void ValidateAgreement(object sender, ServerValidateEventArgs e)
        {
            if (AgreeField.Checked)
                e.IsValid = true;
            else
                e.IsValid = false;
        }

        protected string GetTimeString(byte hour, byte min)
        {
            return
                ((hour == 0) ? 12 : (hour > 12 ? hour - 12 : hour)).ToString() + ":"
                + min.ToString("D2") + " "
                + ((hour < 12) ? "am" : "pm")
                + ((hour == 12 && min==0) ? " (noon)" : "");
        }
    }
}
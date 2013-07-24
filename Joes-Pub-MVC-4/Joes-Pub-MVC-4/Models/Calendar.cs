using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Joes_Pub_MVC_4.Models
{
    public class CalendarViewModel
    {
        public DateTime WorkingDate = new DateTime();
        public DateTime Today { get { return DateTime.Now; } }
        public DateTime NextMonth = new DateTime();
        public DateTime LastMonth = new DateTime();
        public DateTime SelectedMonth { get; set; }
        public DateTime First = new DateTime();
        public DateTime Last = new DateTime();
        public int FirstDay;
        public int NumDays;
        public int CurDay = 1;
        public Nysf.Tessitura.Performance[] perfs;
        public List<DataRow> TotalPerfs = new List<DataRow>();
        public Nysf.Tessitura.Performance[] pastshows;
        public CalendarViewModel()
        {
        }
        public CalendarViewModel(DateTime Selected)
        {
            SelectedMonth = Selected;
            DataSet tempData = Nysf.Tessitura.WebClient.ExecuteLocalProcedure(8030, "");
            foreach (DataRow temp in tempData.Tables[0].Rows)
            {
                DateTime perfdt = (DateTime)temp.ItemArray[9];
                if (perfdt.Month == Selected.Month && perfdt.Year == Selected.Year)
                {
                    TotalPerfs.Add(temp);
                }
            }
            WorkingDate = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1);
            NextMonth = new DateTime(SelectedMonth.Year, ((SelectedMonth.Month == 12) ? 1 : SelectedMonth.Month + 1 ), 1);
            LastMonth = new DateTime(SelectedMonth.Year, ((SelectedMonth.Month == 1) ? 12 : SelectedMonth.Month - 1 ), 1);
            First = new DateTime(SelectedMonth.Year, SelectedMonth.Month, 1, 0, 0, 1);
            FirstDay = (int)First.DayOfWeek;
            NumDays = DateTime.DaysInMonth(SelectedMonth.Year, SelectedMonth.Month);
            Last = new DateTime(SelectedMonth.Year, SelectedMonth.Month, NumDays,23,59,59);


            if ((SelectedMonth.Month >= Today.Month && SelectedMonth.Year >= Today.Year))
            {
                perfs = Nysf.Tessitura.WebClient.GetPerformances();
                perfs = perfs.Where(s=>s.Organization == Nysf.Types.Organization.JoesPub).ToArray();
                perfs = perfs.Where(s => s.StartTime.Month == DateTime.Now.Month).ToArray();
            }
            else perfs = null;
            if (SelectedMonth.Month <= Today.Month && SelectedMonth.Year <= Today.Year)
            {
                pastshows = Nysf.Tessitura.WebClient.GetPerformances(First,DateTime.Now);
                pastshows = perfs.Where(s => s.Organization == Nysf.Types.Organization.JoesPub).ToArray();
               
            }
            else pastshows = null;
        }
    }
}
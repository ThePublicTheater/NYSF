using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace Nysf.TicketScanning
{
	public class DataClient
	{
		private const string facilitiesCacheKey = "nysf_TicketScanning_Facilities";

		public static FacilityCollection GetFacilities()
		{
			FacilityCollection facilities =
				(FacilityCollection)HttpContext.Current.Cache[facilitiesCacheKey];
			if (facilities == null)
			{
				string query =

@"select distinct d.facil_no,d.description as facil_desc
from tx_sli_ticket a
join T_SUB_LINEITEM b on a.sli_no = b.sli_no
join T_PERF c on b.perf_no = c.perf_no
join T_FACILITY d on c.facility_no = d.facil_no
where c.perf_dt between CONVERT(date,getdate()) and dateadd(day,1,CONVERT(date,getdate()))";

				string connString = ConfigSection.Settings.ConnectionString;
				SqlConnection conn = new SqlConnection(connString);
				SqlCommand cmd = new SqlCommand(query, conn);
				List<Facility> facilityList = new List<Facility>();
				try
				{
					conn.Open();
					SqlDataReader reader = cmd.ExecuteReader();
					while (reader.Read())
					{
						Facility newFacility = new Facility
						{
							Id = reader.GetInt32(0),
							Name = reader.GetString(1)
						};
						facilityList.Add(newFacility);
					}
				}
				finally
				{
					conn.Close();
				}
				if (facilityList.Count > 0)
					facilities = new FacilityCollection(facilityList.ToArray());
				else
					facilities = new FacilityCollection();
				int cacheMins = ConfigSection.Settings.CacheMins;
				HttpContext.Current.Cache.Insert(facilitiesCacheKey, facilities,
						null, DateTime.Now.AddMinutes(cacheMins), TimeSpan.Zero);
			}
			return facilities;
		}

		public static TicketResults CheckTicket(int ticketNum, string username, int facilityId,
			bool checkDates)
		{
			string userIp = HttpContext.Current.Request.UserHostAddress;
			string connString = ConfigSection.Settings.ConnectionString;
			SqlConnection conn = new SqlConnection(connString);
			SqlCommand cmd = new SqlCommand("lp_ticket_scan", conn);
			cmd.CommandType = CommandType.StoredProcedure;
			cmd.Parameters.Add(new SqlParameter("@ticket_no", ticketNum));
			cmd.Parameters.Add(new SqlParameter("@userid", username));
			cmd.Parameters.Add(new SqlParameter("@fac_no", facilityId));
			cmd.Parameters.Add(new SqlParameter("@scanned_ip", userIp));
			cmd.Parameters.Add(new SqlParameter("@late_scans_allowed", !checkDates));
			SqlDataAdapter adapter = new SqlDataAdapter(cmd);
			DataSet sqlResults = new DataSet();
			try
			{
				conn.Open();
				adapter.Fill(sqlResults);
			}
			finally
			{
				conn.Close();
			}
			TicketResults results = new TicketResults();
			if (sqlResults.Tables.Count == 1)
			{
				results.Message = sqlResults.Tables[0].Rows[0][0].ToString();
				return results;
			}
			DataRow partyRow = sqlResults.Tables[0].Rows[0];
			DataRow perfRow = sqlResults.Tables[1].Rows[0];
			results.Row = perfRow["row_num"].ToString();
			results.Seat = perfRow["seat_num"].ToString();
			results.PartyName =
				partyRow["party_fname"].ToString() + " " + partyRow["party_lname"].ToString();
			results.PartyCount = Convert.ToInt32(partyRow["party_count"]);
			results.OrderDate = Convert.ToDateTime(partyRow["party_order_dt"]);
			results.PartyHasMembership = Convert.ToChar(partyRow["party_member_ind"]) != 'N';
			results.PartyHasPartnership = Convert.ToChar(partyRow["party_partner_ind"]) != 'N';
			results.PerfTitle = perfRow["show_title"].ToString();
			results.PerfDate = Convert.ToDateTime(perfRow["show_date"]).Add(
				(TimeSpan)perfRow["show_time"]);
			return results;
		}
	}
}
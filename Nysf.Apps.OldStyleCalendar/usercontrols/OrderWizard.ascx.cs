using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using Nysf.Tessitura;

namespace Nysf.Apps.OldStyleTickets
{
	public partial class OrderWizard : GenericControl
	{
		private static Dictionary<int, string> syosSeatingConditions;

		private bool syosRequestProcessed = false;

		static OrderWizard()
		{
			string conditionsDataPath =
				HttpContext.Current.Server.MapPath(PathToJoesPubSeatingConditions);
			XmlTextReader reader = new XmlTextReader(conditionsDataPath);
			if (!reader.ReadToFollowing("pubSeatingConditions"))
				throw new XmlException("Can't find <pubSeatingConditions> node.");
			if (!reader.ReadToDescendant("section"))
				throw new XmlException("Can't find any <section> nodes.");
			syosSeatingConditions = new Dictionary<int, string>();
			do
			{
				if (!reader.MoveToAttribute("id"))
					throw new XmlException("Can't find \"id\" attribute for <section>.");
				int id = Int32.Parse(reader.Value.Trim());
				reader.MoveToElement();
				string conditions = reader.ReadElementContentAsString();
				syosSeatingConditions.Add(id, conditions);
			}
			while (reader.ReadToNextSibling("section"));
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				if (Session[WebClient.TessSessionKeySessionKey] == null)
					throw new ApplicationException("The OrderWizard requires that a Tessitura "
						+ "session have been set by the SessionHandler control.");
				Utility.SetReferer(ViewState);
				int requestedProd = -1;
				if (!Int32.TryParse(Request.QueryString["prod"], out requestedProd))
				{
					ShowNameHeader.Visible = false;
					OrderWizardViews.SetActiveView(ProdNotFoundView);
				}
				else
				{
					Production prod = WebClient.GetProduction(requestedProd);
					if (prod == null)
					{
						ShowNameHeader.Visible = false;
						OrderWizardViews.SetActiveView(ProdNotFoundView);
					}
					else
					{
						PerfNameBlurb.Text = prod.Title;
						if (!prod.IsOnSale)
						{
							OrderWizardViews.SetActiveView(PerfsNotAvailableView);
						}
						else
						{
							ProdPerformance[] perfs = null;
							perfs = WebClient.GetPerformances(requestedProd);
							ViewState.Add("perfs", perfs);
							if (perfs == null || perfs.Length < 1)
							{
								OrderWizardViews.SetActiveView(PerfsNotAvailableView);
							}
							else if (perfs.Length == 1)
							{
								ListItem newItem = new ListItem("", perfs[0].Id.ToString());
								newItem.Selected = true;
								PerfField.Items.Add(newItem);
								SeatChoiceBackButton.Visible = false;
								SeatChoiceCancelButton.Visible = true;
								OrderWizardViews.SetActiveView(SeatChoiceView);
							}
							else
							{
								OrderWizardViews.SetActiveView(PerfSelectionView);
							}
						}
					}
				}
			}
			else if (OrderWizardViews.GetActiveView() == SeatChoiceView
				&& SelectionModeViews.GetActiveView() == SyosModeView)
			{
				bool syosSeatFound = false;
				foreach (string key in Request.Form.AllKeys)
				{
					if (key.StartsWith("SyosSeat"))
					{
						syosSeatFound = true;
						break;
					}
				}
				if (syosSeatFound)
				{
					AddSeatsToCart(sender, e);
				}
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			if (OrderWizardViews.GetActiveView() == PerfSelectionView && !Page.IsPostBack)
			{
				int requestedPerf = -1;
				Int32.TryParse(Request.QueryString["perf"], out requestedPerf);
				ProdPerformance[] perfs = (ProdPerformance[])ViewState["perfs"];
				List<string> venueNames = new List<string>();
				foreach (ProdPerformance perf in perfs)
				{
					if (!venueNames.Contains(perf.VenueName))
						venueNames.Add(perf.VenueName);
				}
				bool hasMultipleVenues = venueNames.Count > 1;
				if (hasMultipleVenues)
				{
					VenueNamesNote.Visible = true;
				}
				else
				{
					VenueNameBlurb.Text = "<aside>Location: " + venueNames[0] + "</aside>";
				}
				foreach (ProdPerformance perf in perfs)
				{
					ListItem newItem = new ListItem(
						perf.StartTime.ToString("dddd, MMMM d, h:mm tt")
							+ (hasMultipleVenues ? " (" + perf.VenueName + ")" : ""),
						perf.Id.ToString());
					newItem.Selected = perf.Id == requestedPerf;
					PerfField.Items.Add(newItem);
				}
			}
			else if (OrderWizardViews.GetActiveView() == SeatChoiceView)
			{
				ProdPerformance[] perfs = (ProdPerformance[])ViewState["perfs"];
				ProdPerformance chosenPerf = null;
				foreach (ProdPerformance perf in perfs)
				{
					if (perf.Id == Int32.Parse(PerfField.SelectedValue))
					{
						chosenPerf = perf;
						break;
					}
				}

				// Parse SYOS venues from web.config

				string[] syosVenueIdStrings = Regex.Replace(WebConfigurationManager.AppSettings[
					"nysf_Apps_Ost_SyosVenueIds"], "\\s+", "").Split(',');
				List<int> syosVenueIds = new List<int>();
				foreach (string venueIdString in syosVenueIdStrings)
				{
					int parsedVenueId = -1;
					if (Int32.TryParse(venueIdString, out parsedVenueId))
					{
						syosVenueIds.Add(parsedVenueId);
					}
				}

				// Check if venue is a SYOS venue

				bool isSyosVenue = syosVenueIds.Contains(chosenPerf.VenueId);

				SyosSeatCollection seatChoices = null;
				SeatingZone[] seatOptions = null;

				if (isSyosVenue)
					seatChoices = WebClient.GetSeatsForSyos(chosenPerf.Id);
				else
					seatOptions = SeatingZone.GetIdealPricesPerZone(
						WebClient.GetSeatingZonesAndPrices(chosenPerf.Id));

				if ((isSyosVenue && seatChoices == null)
					|| (!isSyosVenue && seatOptions.Length == 0))
					OrderWizardViews.SetActiveView(PerfsNotAvailableView);
				else
				{
					//if (isSyosVenue)
					//    ViewState.Add("seatChoices", seatChoices);
					//else
					//    ViewState.Add("seatOptions", seatOptions);

					if (!isSyosVenue)
						ViewState.Add("seatOptions", seatOptions);

					if (ReservationErrorFlag.Value == "1")
					{
						ReservationErrorMessage.Visible = true;
						ReservationErrorFlag.Value = "0";
					}
					else
					{
						ReservationErrorMessage.Visible = false;
					}

					if (PromoCodeSuccessFlag.Value == "1")
					{
						PromoCodeSuccessMessage.Visible = true;
						PromoCodeEntryPanel.Visible = false;
						PromoCodeSuccessFlag.Value = "0";
					}
					else
					{
						PromoCodeSuccessMessage.Visible = false;
					}

					if (PromoCodeErrorFlag.Value == "1")
					{
						PromoCodeWarning.Visible = true;
						PromoCodeErrorFlag.Value = "0";
					}
					else
					{
						PromoCodeWarning.Visible = false;
					}

					if (SeatAddSuccessFlag.Value == "1")
					{
						SeatAddSuccessMessage.Visible = true;
						SeatAddSuccessFlag.Value = "0";
					}
					else
					{
						SeatAddSuccessMessage.Visible = false;
					}

					if (FailedSeatAddFlag.Value == "1")
					{
						FailedSeatAddMessage.Visible = true;
						FailedSeatAddFlag.Value = "0";
					}
					else
					{
						FailedSeatAddMessage.Visible = false;
					}

					if (SeatsAddSuccessFlag.Value == "1")
					{
						SeatsAddSuccessPanel.Visible = true;
						SeatsAddSuccessFlag.Value = "0";
					}
					else
					{
						SeatsAddSuccessPanel.Visible = false;
					}

					if (FailedSeatsAddFlag.Value == "1")
					{
						FailedSeatsAddPanel.Visible = true;
						FailedSeatsAddFlag.Value = "0";
					}
					else
					{
						FailedSeatsAddPanel.Visible = false;
					}

					DateBlurb.Text = chosenPerf.StartTime.ToString("dddd, MMMM d");
					StartTimeBlurb.Text = chosenPerf.StartTime.ToString("h:mm tt");
					VenueBlurb.Text = chosenPerf.VenueName;

					if (isSyosVenue)
						PrepareSyosMap(seatChoices,
							chosenPerf.ProdTypeId == 4 || chosenPerf.ProdTypeId == 5);
					else
						PrepareBestSeatingSelection(seatOptions);
				}
			}
			else if (OrderWizardViews.GetActiveView() == ConfirmationView)
			{
				ConfirmDateBlurb.Text = DateBlurb.Text;
				ConfirmStartTimeBlurb.Text = StartTimeBlurb.Text;
				ConfirmVenueBlurb.Text = VenueBlurb.Text;
				KeepShoppingLink.NavigateUrl = Utility.GetReferer(ViewState);
				ViewCartLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
					WebConfigurationManager.AppSettings["nysf_UserControls_CartPageUrl"]);
				CheckOutLink.NavigateUrl = Utility.GetFullHrefFromSubpath(
					WebConfigurationManager.AppSettings["nysf_UserControls_CheckOutPageUrl"]);

				//if (SelectionModeViews.GetActiveView() == BestSeatingModeView)
				//{
					SeatingZone[] seatOptions = (SeatingZone[])ViewState["seatOptions"];
					ViewState.Remove("seatOptions");

					Reservation reserved = (Reservation)ViewState["reserved"];
					ViewState.Remove("reserved");
					Cart reservedSummary = Cart.BuildSummaryCart(reserved, seatOptions);
					FillListFromSummaryCart(AddedTicketsList, reservedSummary);

					if (ViewState["unreserved"] != null)
					{
						Reservation unreserved = (Reservation)ViewState["unreserved"];
						ViewState.Remove("unreserved");
						Cart unreservedSummary = Cart.BuildSummaryCart(unreserved, seatOptions);
						FillListFromSummaryCart(FailedTicketsList, unreservedSummary);
					}
				//}

				/*else if (SelectionModeViews.GetActiveView() == SyosModeView)
				{
					SyosSeatCollection seatChoices = (SyosSeatCollection)ViewState["seatChoices"];
					ViewState.Remove("seatChoices");
				}*/
			}
		}

		private void PrepareSyosMap(SyosSeatCollection collection)
		{
			PrepareSyosMap(collection, true);
		}

		private void PrepareSyosMap(SyosSeatCollection collection, bool displayConditions)
		{
			SelectionModeViews.SetActiveView(SyosModeView);
			for (int c = collection.PriceScale.Count-1; c >= 0; c--)
			{
				if (collection.PriceScale[c] < 0)
					continue;
				HtmlGenericControl newColorLi = new HtmlGenericControl("li");
				newColorLi.Attributes.Add("class",
					"SyosPriceScaleX" + c.ToString());
				newColorLi.InnerHtml = collection.PriceScale[c].ToString("C");
				ColorLegend.Controls.AddAt(0, newColorLi);
			}
			SyosSeatOptions.Attributes["class"] =
				"ArrangedByJs SyosMap SyosMapId" + collection.ZoneMapId.ToString();
			foreach (SyosSeatSection section in collection.Sections)
			{
				HtmlGenericControl newSectionDiv = new HtmlGenericControl("div");
				newSectionDiv.Attributes["class"] =
					"SyosSection SyosSectionId" + section.Id.ToString();
				HtmlGenericControl newSectionH = new HtmlGenericControl("h4");
				newSectionH.InnerText = section.Name;
				newSectionDiv.Controls.Add(newSectionH);
				HtmlGenericControl newSectionDisDiv = new HtmlGenericControl("div");
				newSectionDisDiv.Attributes["class"] = "SyosDisclaimers";
				/*HtmlGenericControl newSectionDisH = new HtmlGenericControl("h5");
				newSectionDisH.InnerText = "Conditions";
				newSectionDisDiv.Controls.Add(newSectionDisH);*/
				if (displayConditions && syosSeatingConditions.Keys.Contains(section.Id)
					&& !String.IsNullOrWhiteSpace(syosSeatingConditions[section.Id]))
				{
					newSectionDisDiv.InnerHtml = syosSeatingConditions[section.Id];
				}
				newSectionDiv.Controls.Add(newSectionDisDiv);
				HtmlGenericControl newSectionZonesDiv = new HtmlGenericControl("div");
				newSectionZonesDiv.Attributes["class"] = "SyosZones";
				HtmlGenericControl newSectionZonesH = new HtmlGenericControl("h5");
				newSectionZonesH.InnerText = "Zones";
				newSectionZonesDiv.Controls.Add(newSectionZonesH);
				foreach (SyosSeatZone zone in section.Zones)
				{
					HtmlGenericControl newZoneDiv = new HtmlGenericControl("div");
					newZoneDiv.Attributes["class"] =
						"SyosZone SyosZoneId" + zone.Id.ToString() + " SyosPriceScaleX"
						+ collection.PriceScale.IndexOf(zone.BestPriceType.Price).ToString();
					HtmlGenericControl newZoneH = new HtmlGenericControl("h6");
					newZoneH.InnerText = zone.Description + " ("
						+ zone.BestPriceType.Price.ToString("C") + ")";
					newZoneDiv.Controls.Add(newZoneH);
					HtmlGenericControl newGroupsDl = new HtmlGenericControl("dl");
					foreach (SyosSeatGroup group in zone.SeatGroups)
					{
						HtmlGenericControl newGroupDt = new HtmlGenericControl("dt");
						newGroupDt.InnerText = "Seat group " + group.Id.ToString();
						newGroupsDl.Controls.Add(newGroupDt);
						foreach (SyosSeat seat in group.Seats)
						{
							string availClass;
							switch(seat.Status)
							{
								case SyosSeatStatus.Available:
									availClass = "SyosSeatAvail";
									break;
								case SyosSeatStatus.InCart:
									availClass = "SyosSeatInCart";
									break;
								default:
									availClass = "SyosSeatTaken";
									break;
							}
							HtmlGenericControl newSeatDd = new HtmlGenericControl("dd");
							newSeatDd.Attributes["class"] = "SyosSeat " + availClass + " "
								+ "SyosSeatLocation" + seat.LocationId.ToString("D6");
							string newSeatCheckBox;
							string newSeatLabel;
							switch (seat.Status)
							{
								case SyosSeatStatus.Available:
									newSeatCheckBox =
										"<input type=\"checkbox\" id=\"" + "SyosSeat"
										+ seat.Id.ToString()
										+ "\" name=\"SyosSeat_s_" + section.Id.ToString()
										+ "_z_" + zone.Id.ToString()
										+ "_p_" + zone.BestPriceType.Id.ToString()
										+ "_s_" + seat.Id.ToString() + "\" />";
									newSeatLabel = "<label for=\"" + seat.Id.ToString() + "\">"
										+ "#" + seat.Number.ToString() + "</label>";
									break;
								case SyosSeatStatus.InCart:
									newSeatCheckBox =
										"<input type=\"checkbox\" disabled=\"disabled\" />";
									newSeatLabel = "<label>in your cart</label>";
									break;
								default:
									newSeatCheckBox =
										"<input type=\"checkbox\" disabled=\"disabled\" />";
									newSeatLabel = "<label>unavailable</label>";
									break;
							}
							newSeatDd.InnerHtml = newSeatCheckBox + newSeatLabel;
							newGroupsDl.Controls.Add(newSeatDd);
						}
					}
					newZoneDiv.Controls.Add(newGroupsDl);
					newSectionZonesDiv.Controls.Add(newZoneDiv);
				}
				newSectionDiv.Controls.Add(newSectionZonesDiv);
				SyosSeatOptions.Controls.Add(newSectionDiv);
			}
		}

		private void PrepareBestSeatingSelection(SeatingZone[] seatOptions)
		{
			SelectionModeViews.SetActiveView(BestSeatingModeView);

			int seatChoiceMax = Int32.Parse(WebConfigurationManager.AppSettings[
				"nysf_Apps_Ost_MaxSeatQuantityChoicePerSection"]);
			StringBuilder seatQuantityOptions = new StringBuilder();
			seatQuantityOptions.AppendLine(
				"<option value=\"0\" selected=\"selected\">0</option>");
			for (int c = 1; c <= seatChoiceMax; c++)
			{
				seatQuantityOptions.AppendLine(
					"<option value=\"" + c.ToString() + "\">" + c.ToString() + "</option>");
			}

			StringBuilder seatingTableRows = new StringBuilder();
			foreach (SeatingZone section in seatOptions)
			{
				seatingTableRows.AppendLine("<tr>");
				seatingTableRows.AppendLine("<td>" + section.Name + "</td>");
				seatingTableRows.AppendLine("<td>"
					+ section.PriceTypes[0].Price.ToString("C") + "</td>");
				seatingTableRows.AppendLine("<td>");
				seatingTableRows.AppendLine("<select name=\"seats_" + section.Id.ToString()
					+ "_" + section.PriceTypes[0].Id.ToString() + "\">");
				seatingTableRows.Append(seatQuantityOptions.ToString());
				seatingTableRows.AppendLine("</select>");
				seatingTableRows.AppendLine("</td>");
				seatingTableRows.AppendLine("</tr>");
			}

			SeatingRowsOutput.Text = seatingTableRows.ToString();
		}

		protected void ShowZonePriceView(object sender, EventArgs e)
		{
			OrderWizardViews.SetActiveView(SeatChoiceView);
		}

		protected void UpdatePrices(object sender, EventArgs e)
		{
			if (WebClient.EnterPromoCode(PromoCodeField.Text))
			{
				PromoCodeSuccessFlag.Value = "1";
			}
			else
			{
				PromoCodeErrorFlag.Value = "1";
			}
		}

		protected void ReturnToReferer(object sender, EventArgs e)
		{
			Utility.RedirectToReferer(ViewState);
		}

		protected void AddSeatsToCart(object sender, EventArgs e)
		{
			if (SelectionModeViews.GetActiveView() == BestSeatingModeView)
			{
				Reservation res = new Reservation(Int32.Parse(PerfField.SelectedValue));
				foreach (string key in Request.Form.AllKeys)
				{
					if (key.StartsWith("seats_"))
					{
						string[] queryKeyParts = key.Split('_');
						int sectionId = Int32.Parse(queryKeyParts[1]);
						int priceTypeId = Int32.Parse(queryKeyParts[2]);
						int numOfSeats = Int32.Parse(Request.Form[key]);
						if (numOfSeats > 0)
						{
							res.AddPriceTypeSeats(sectionId, priceTypeId, numOfSeats);
						}
					}
				}
				Reservation unreserved = WebClient.ReserveBestSeating(res);
				if (unreserved.Sections.Count == res.Sections.Count)
				{
					ReservationErrorFlag.Value = "1";
				}
				else
				{
					ViewState.Remove("perfs");
					if (unreserved.Sections.Count == 0)
					{
						FailedResPanel.Visible = false;
					}
					else
					{
						ViewState.Add("unreserved", unreserved);
						res.RemoveSections(unreserved.Sections);
					}
					ViewState.Add("reserved", res);
					OrderWizardViews.SetActiveView(ConfirmationView);
				}
			}
			else if (SelectionModeViews.GetActiveView() == SyosModeView)
			{
				if (syosRequestProcessed)
					return;
				SyosReservation res = new SyosReservation(Int32.Parse(PerfField.SelectedValue));
				foreach (string key in Request.Form.AllKeys)
				{
					if (key.StartsWith("SyosSeat_"))
					{
						string[] queryKeyParts = key.Split('_');
						int sectionId = Int32.Parse(queryKeyParts[2]);
						int zoneId = Int32.Parse(queryKeyParts[4]);
						int priceTypeId = Int32.Parse(queryKeyParts[6]);
						int seatId = Int32.Parse(queryKeyParts[8]);
						res.AddSeat(sectionId, zoneId, priceTypeId, seatId);
					}
				}
				SyosReservation unreserved = WebClient.ReserveSyos(res);
				if (res.SeatCount == 0)
				{
					ReservationErrorFlag.Value = "1";
				}
				else if (res.SeatCount == 1)
				{
					if (unreserved.SeatCount == 1)
						FailedSeatAddFlag.Value = "1";
					else
						SeatAddSuccessFlag.Value = "1";
				}
				else
				{
					if (unreserved.SeatCount == res.SeatCount)
						ReservationErrorFlag.Value = "1";
					else
					{
						SeatsAddSuccessFlag.Value = "1";
						// TODO: add successful reservations to list
						if (unreserved.SeatCount > 0)
						{
							FailedSeatsAddFlag.Value = "1";
							// TODO: add failed reservations to list
						}
					}
				}
				syosRequestProcessed = true;
			}
		}

		protected void BackToPerfSelection(object sender, EventArgs e)
		{
			OrderWizardViews.SetActiveView(PerfSelectionView);
		}

		private void FillListFromSummaryCart(BulletedList list, Cart summaryCart)
		{
			foreach (CartSeatGroupItem seatGroup in summaryCart.SeatGroups)
			{
				if (seatGroup.SeatsPerPriceTypes.Count == 1)
				{
					list.Items.Add(seatGroup.SeatingZoneName + ", "
						+ seatGroup.SeatsPerPriceTypes[0].SeatCount.ToString() + " seat"
						+ (seatGroup.SeatsPerPriceTypes[0].SeatCount > 1 ? "s" : ""));
				}
				else
				{
					foreach (CartPriceTypeSeats seatsPerPriceType in seatGroup.SeatsPerPriceTypes)
					{
						list.Items.Add(seatGroup.SeatingZoneName + ", "
							+ seatsPerPriceType.SeatCount.ToString() + " seat"
							+ (seatsPerPriceType.SeatCount > 1 ? "s" : "")
							+ " (" + seatsPerPriceType.PriceTypeName + ")");
					}
				}
			}
		}

		protected void GoToCart(object sender, EventArgs e)
		{
			Response.Redirect(Utility.GetFullHrefFromSubpath(
				WebConfigurationManager.AppSettings["nysf_UserControls_CartPageUrl"]));
		}
	}
}
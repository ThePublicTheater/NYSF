using Ambitus;
using Nysf.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.Web.UserControls
{
	public partial class CartForm : Nysf.Web.UserControls.UserControl
	{
		protected void Page_Load(object sender, EventArgs e)
		{
			NysfSession session = BrowserUtility.GetSession();
			ProcessRemovals(session);
			if (!session.CartExists)
			{
				Response.Redirect("/");
			}
			StringBuilder output = new StringBuilder();
			Cart cart = session.GetCart();
			OutputContributions(output, cart);
			OutputLiteral.Text = output.ToString();
		}

		protected void ProcessRemovals(NysfSession session)
		{
			foreach (string formItem in Request.Form.AllKeys)
			{
				if (formItem.StartsWith("remove_cont_"))
				{
					int contId = Int32.Parse((formItem.Split('_'))[2]);
					session.RemoveContribution(contId);
					break;
				}
			}
		}

		protected void OutputContributions(StringBuilder output, Cart cart)
		{
			if (cart.Contributions == null || cart.Contributions.Count == 0)
			{
				return;
			}
			output.AppendLine("<div id=\"ContribSection\" class=\"CartSection\">");
			output.AppendLine("<h3>Contributions</h3>");
			output.AppendLine("<ul>");
			foreach (Contribution cont in cart.Contributions)
			{
				output.AppendLine("<li>");
				output.AppendLine("<div class=\"CartItemDesc\">");
				if (String.IsNullOrWhiteSpace(cont.Custom0))
				{
					output.AppendLine("donation");
				}
				else
				{
					ContributionCustomData data = new ContributionCustomData(cont.Custom0);
					output.Append((data.ProgName ?? String.Empty) + " - "
							+ (data.LevelName ?? String.Empty) + System.Environment.NewLine);
				}
				output.AppendLine("</div>");
				output.AppendLine("<div class=\"CartItemValue\">");
				output.AppendLine(cont.Amount.Value.ToString("C"));
				output.AppendLine("</div>");
				output.AppendLine("<div class=\"CartItemControls\">");
				output.AppendLine("<input type=\"submit\" value=\"Remove\" name=\"remove_cont_"
						+ cont.Id.Value + "\"/>");
				output.AppendLine("</div>");
				output.AppendLine("</li>");
			}
			output.AppendLine("</ul>");
			output.AppendLine("</div>");
		}

		protected void Do_RedirectCheckout(object sender, EventArgs e)
		{
			Response.Redirect(ConfigSection.Settings.StandardPages.Checkout);
		}
	}
}
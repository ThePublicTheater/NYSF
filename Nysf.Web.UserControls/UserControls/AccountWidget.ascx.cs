using Nysf.Web;
using System;

namespace Nysf.Web.UserControls
{
	public partial class AccountWidget : UserControl
	{
		public bool ShowPromoLink { get; set; }
		public bool ShowRegisterLink { get; set; }
		public bool ShowCartLink { get; set; }
		public bool ShowCheckoutLink { get; set; }
		public bool ShowAccountLink { get; set; }

		public AccountWidget()
		{
			SetDefaults();
		}

		public void SetDefaults()
		{
			ShowPromoLink = ShowRegisterLink = ShowCartLink = ShowCheckoutLink = ShowAccountLink
					= true;
		}

		protected void Page_Load(object sender, EventArgs e)
		{
			if (!Page.IsPostBack)
			{
				StandardPagesElement pages = ConfigSection.Settings.StandardPages;
				Label_PromoLink.NavigateUrl = pages.EnterPromo;
				Label_RegisterLink.NavigateUrl = pages.Register;
				Label_CartLink.NavigateUrl = pages.Cart;
				Label_CheckoutLink.NavigateUrl = pages.Checkout;
				Label_AccountLink.NavigateUrl = pages.Account;
				Label_LoginLink.NavigateUrl = pages.Login;
				Label_LogoutLink.NavigateUrl = pages.Logout;
				PromoLink.Visible = ShowPromoLink;
			}
		}

		protected void Page_PreRender(object sender, EventArgs e)
		{
			NysfSession session = BrowserUtility.GetSession();
			if (session.CartExists)
			{
				CheckoutLink.Visible = ShowCheckoutLink;
				CartLink.Visible = ShowCartLink;
			}
			else
			{
				CheckoutLink.Visible = CartLink.Visible = false;
			}
			if (session.IsAnonymous)
			{
				Display_CurrentAccount.Visible = false;
				RegisterLink.Visible = ShowRegisterLink;
				LoginLink.Visible = true;
				AccountLink.Visible = LogoutLink.Visible = false;
			}
			else
			{
				Display_CurrentAccount.Visible = true;
				Display_Username.Text = session.Username;
				RegisterLink.Visible = LoginLink.Visible = false;
				AccountLink.Visible = ShowAccountLink;
				LogoutLink.Visible = true;
			}
		}
	}
}
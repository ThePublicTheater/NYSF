using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;

namespace Nysf.Apps.OldStyleTickets
{
    public partial class CartCountdown : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            TimeSpan timeLeft = WebClient.GetTimeToCartExpiration();
            if (timeLeft.TotalSeconds > 0)
                CountdownSeconds.Text = ((int)timeLeft.TotalSeconds).ToString();
            else
            {
                this.Visible = false;
                WebClient.EmptyCart();
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Nysf.UserControls
{
    public partial class DebugDisplay : GenericControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            OutputLiteral.Text = Debug.GetSituationPrintout();
        }
    }
}
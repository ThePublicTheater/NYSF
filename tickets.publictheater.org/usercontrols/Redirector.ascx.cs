using System;

namespace Nysf.UserControls
{
    public partial class Redirector : GenericControl
    {
        public string TargetUrl { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(TargetUrl.Trim()))
            {
                throw new ApplicationException(
                    "The Redirector control requires that its TargetUrl property be set.");
            }
            Response.Redirect(TargetUrl);
        }
    }
}
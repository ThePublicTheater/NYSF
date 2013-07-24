using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Nysf.Tessitura;
using System.Web.Configuration;
using System.Text;

namespace Nysf.Apps.OldStyleTickets
{
    public class ProgressTemplate : ITemplate
    {
        Image img;
        public ProgressTemplate(string imageUrl)
        {
            img = new Image();
            img.ImageUrl = imageUrl;
        }
        public void InstantiateIn(Control container)
        {

            container.Controls.Add(img);
        }

    } 
}
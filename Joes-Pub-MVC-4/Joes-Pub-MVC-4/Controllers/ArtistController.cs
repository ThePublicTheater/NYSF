using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Joes_Pub_MVC_4.Controllers
{
    public class ArtistController : Controller
    {
        //
        // GET: /Artist/

        public ActionResult Index(int id, int? page)
        {
            Models.ArtistViewModel ViewMod = new Models.ArtistViewModel();
            var result = ViewMod.ArtDB.MasterArtistList.Where(s => s.JoomlaID == id).ToList();
            ViewMod._artist = result[0];
            if (page != null)
            {
                ViewMod.commPage = page.Value;
            }
            else ViewMod.commPage = 1;

            if (ViewMod._artist != null)
            {
                return View(ViewMod);
            }
            else return Redirect(Request.UrlReferrer.PathAndQuery);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Joes_Pub_MVC_4.Models;
namespace Joes_Pub_MVC_4.Controllers
{
    public class StaffPicksController : Controller
    {
        //
        // GET: /StaffPicks/
        public ActionResult Index()
        {
            List<StaffPick> picks = Utilities.StaffPickDB.Picks.ToList();
            List<SPViewModel> pickmods = new List<SPViewModel>();
            foreach (var item in picks)
            {
                pickmods.Add(new SPViewModel(item, null));
            }
            SPlistViewModel Mod = new SPlistViewModel(pickmods, null);
            string part = ControllerContext.RenderPartialViewToString("_listings", Mod._PickPages);
            return View((object)part);
        }

        public ActionResult SearchSP(string keywords)
        {
            List<StaffPick> picks = Utilities.StaffPickDB.Picks.ToList();
            string[] keys = keywords.Split(' ');
            picks = picks.Where(s => keys.Any(a => s.Caption.Contains(a)) || keys.Any(a => s.Description.Contains(a)) || keys.Any(a => s.Title.Contains(a)) || keys.Any(a => s.SubTitle.Contains(a))).ToList();
            List<SPViewModel> pickmods = new List<SPViewModel>();
            foreach (var item in picks)
            {
                pickmods.Add(new SPViewModel(item, null));
            }
            SPlistViewModel Mod = new SPlistViewModel(pickmods, null);
            string part = ControllerContext.RenderPartialViewToString("_listings", Mod._PickPages);
            return View("Index",(object)part);
        }

        public ActionResult SearchCatsSP(string cat)
        {
            int catid = -1;
            if (cat == "show")
            {
                catid = 1;
            }
            if (cat == "artist")
            {
                catid = 2;
            }
            if (cat == "venue")
            {
                catid = 3;
            }
            List<StaffPick> picks = Utilities.StaffPickDB.Picks.Where(s=>s.Catagory == catid).ToList();
            List<SPViewModel> pickmods = new List<SPViewModel>();
            foreach (var item in picks)
            {
                pickmods.Add(new SPViewModel(item, null));
            }
            SPlistViewModel Mod = new SPlistViewModel(pickmods, null);
            string part = ControllerContext.RenderPartialViewToString("_listings", Mod._PickPages);
            return Content(part);
        }

        public ActionResult ChangeSPpage(int topage)
        {
            List<StaffPick> picks = Utilities.StaffPickDB.Picks.ToList();
            List<SPViewModel> pickmods = new List<SPViewModel>();
            foreach (var item in picks)
            {
                pickmods.Add(new SPViewModel(item, null));
            }
            SPlistViewModel Mod = new SPlistViewModel(pickmods, topage);
            string part = ControllerContext.RenderPartialViewToString("_listings", Mod._PickPages);
            return Content(part);
        }

        public ActionResult Pick(int id)
        {
            StaffPick temppick = Utilities.StaffPickDB.Picks.Find(id);
            if (temppick == null)
            {
                return RedirectToAction("Index", "StaffPicks");
            }
            else
            {
                SPViewModel Mod = new SPViewModel(temppick, null);
                return View(Mod);
            }
        }

        public  ActionResult UpdatePickComments(StaffPickComments comm)
        {
                comm.PostDate = DateTime.Now;
                Utilities.StaffPickDB.PickComments.Add(comm);
                Utilities.StaffPickDB.SaveChanges();
                return RedirectToAction("Pick", "StaffPicks", new { @id = comm.PickID });
                
        }
    }
}

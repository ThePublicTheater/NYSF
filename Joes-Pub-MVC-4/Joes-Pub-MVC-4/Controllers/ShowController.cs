using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Nysf.Tessitura;
namespace Joes_Pub_MVC_4.Controllers
{
    public class ShowController : Controller
    {
        //
        // GET: /Show/
        public ActionResult Index(int id, int? page, int? id2)
        {
            Models.ShowViewModel ViewMod = new Models.ShowViewModel();
            if (id != -1)
            {
                List<Models.Show> result = ViewMod.ShowDB.MasterShowList.Where(s => s.TessProdID ==(id - 1) || s.TessProdID == id).ToList();
                //List<Models.Show> result = ViewMod.ShowDB.MasterShowList.ToList();
               // List<Nysf.Tessitura.Performance> PerfList = new List<Nysf.Tessitura.Performance>(Nysf.Tessitura.WebClient.GetPerformances(DateTime.Now, DateTime.Now.AddDays(7), Nysf.Types.Organization.JoesPub));
                Performance p = WebClient.GetPerformance(id);
                //Models.Show s = new Models.Show();
                
                ViewMod._show = result[0];
            }
            else
            {
                List<Models.Show> result = ViewMod.ShowDB.MasterShowList.Where(s => s.ID == id2).ToList();
                ViewMod._show = result[0];
            }
            if (page != null)
            {
                ViewMod.commpage = page.Value;
            }
            else ViewMod.commpage = 1;
            return View(ViewMod);
        }
        public ActionResult UpdateShowComments(Models.ShowComment Comm)
        {
            Comm.PostDate = DateTime.Now;
            Models.ProfilesDBContext ProfDB = new Models.ProfilesDBContext();
            List<Models.Profiles> Prof = ProfDB.ProfileList.Where(s => s.UserName == User.Identity.Name).ToList();
            Comm.Name = Prof[0].FirstName + " " + Prof[0].LastName;
            if (ModelState.IsValid)
            {
                Models.ShowDBContext DB = new Models.ShowDBContext();
                DB.MasterShowCommentList.Add(Comm);
                DB.SaveChanges();
                DB.Dispose();

                Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
                Mod.ShowComms = Models.Utilities.ShowDB.MasterShowCommentList.Where(s => s.UserID == Mod.CurUser.ID).ToList();
                List<string> temp = new List<string>();
                for (int i = 1; i < Mod.ShowCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_ShowComms.cshtml", Mod.ShowComms.ToPagedList(i, 5)));
                }
                Mod.CurUserCache.showcommentcache = temp;
                Models.Utilities.CacheDB.Entry(Mod.CurUserCache).State = System.Data.EntityState.Modified;
                Models.Utilities.CacheDB.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

        public ActionResult ShowList(string keyword, string genre, string artist, DateTime from_date, DateTime to_date, int? page, int? perfpage, int? numperpage)
        {
            if (keyword == "Enter Keyword")
                keyword = "";
            if (artist == "Enter Artist")
                artist = "";
            if (genre == "Select Genre")
                genre = "";
            Joes_Pub_MVC_4.Models.ShowListViewModel Mod = new Models.ShowListViewModel(from_date,to_date,keyword,artist,genre,page ?? 1, perfpage ?? 1, numperpage ?? 15);
            return View(Mod);
        }
        public PartialViewResult UpdatePerfShowLists(Models.ShowListViewModel Mod)
        {

            return PartialView();
        }
    }
}

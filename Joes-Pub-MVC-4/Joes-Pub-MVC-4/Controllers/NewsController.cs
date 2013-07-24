using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace Joes_Pub_MVC_4.Controllers
{
    public class NewsController : Controller
    {
        //
        // GET: /News/
        public ActionResult Index(int id, int? page)
        {
            Models.NewsDBcontext DB = new Models.NewsDBcontext();
            Models.News SelectedArticle = DB.MasterNewsListing.Find(id);
            DB.Dispose();
            Models.NewsViewModel Mod = new Models.NewsViewModel(SelectedArticle, page ?? 1);
            return View(Mod);
        }
        
        public ActionResult NewsList(string Key, string Key2, string Key3, int? pagenum)
        {
            if (Key == null)
            {
                Key = "";
            }
            if (Key2 == null)
            {
                Key2 = "2000-01-01";
            }
            if (Key3 == null)
            {
                Key3 = DateTime.Now.ToString();
            }
            Models.NewsListViewModel Mod = new Models.NewsListViewModel(pagenum ?? 1, 10, DateTime.Parse(Key2), DateTime.Parse(Key3), Key);
            return View(Mod);
        }

        [HttpPost]
        public ActionResult UpdateNewsList(string Key, string Key2, string Key3, int? pagenum)
        {
            if (Key == "Keyword Search")
            {
                Key = "";
            }
            Models.NewsListViewModel Mod = new Models.NewsListViewModel(pagenum ?? 1, 10, DateTime.Parse(Key2), DateTime.Parse(Key3), Key);
            return PartialView("_newslistpartial", Mod);
        }
        [HttpPost]
        public ActionResult UpdateNewsListPage(int pagenum, string Key, string Key2, string Key3)
        {
            if (Key == "Keyword Search")
            {
                Key = "";
            }
            Models.NewsListViewModel Mod = new Models.NewsListViewModel(pagenum, 10, DateTime.Parse(Key2), DateTime.Parse(Key3), Key);
            return PartialView("_newslistpartial", Mod);
        }

        public ActionResult UpdateNewsComment(Models.NewsComments Comm)
        {
            Comm.PostDate = DateTime.Now;
            if (ModelState.IsValid)
            {
                Models.NewsDBcontext DB = new Models.NewsDBcontext();
                DB.MasterCommentsListing.Add(Comm);
                DB.SaveChanges();
                DB.Dispose();

                Models.ProfileViewModel Mod = new Models.ProfileViewModel(User.Identity.Name);
                Mod.NewsComms = Models.Utilities.NewsDB.MasterCommentsListing.Where(s=> s.UserId == Mod.CurUser.ID).ToList();
                List<string> temp = new List<string>();
                for (int i = 1; i < Mod.NewsCommPages.PageCount; i++)
                {
                    temp.Add(ControllerContext.RenderPartialViewToString("~/Views/Account/_NewsComms.cshtml", Mod.NewsComms.ToPagedList(i, 5)));
                }
                Mod.CurUserCache.newscommentcache = temp;
                Models.Utilities.CacheDB.Entry(Mod.CurUserCache).State = System.Data.EntityState.Modified;
                Models.Utilities.CacheDB.SaveChanges();
            }
            return Redirect(Request.UrlReferrer.PathAndQuery);
        }

    }
}

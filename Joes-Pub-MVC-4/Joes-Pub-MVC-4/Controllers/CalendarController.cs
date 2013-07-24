using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Joes_Pub_MVC_4.Models;
using System.IO;
namespace Joes_Pub_MVC_4.Controllers
{
    public static class HtmlExtensions
    {
        public static string RenderPartialViewToString(this ControllerContext context, string viewName, object model)
        {
            if (string.IsNullOrEmpty(viewName))
            {
                viewName = context.RouteData.GetRequiredString("action");
            }

            context.Controller.ViewData.Model = model;

            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(context, viewName);
                var viewContext = new ViewContext(context, viewResult.View, context.Controller.ViewData, context.Controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);

                return sw.GetStringBuilder().ToString();
            }
        }
    }
    public class CalendarController : Controller
    {
        Models.CacheDBContext Cache = new CacheDBContext();
        [HttpGet]
        public ActionResult Calendar()
        {
            return View();
        }
        [HttpPost]
        public string CalendarGet(DateTime Date)
        {
            Date = new DateTime(Date.Year, Date.Month, 1, 12, 0, 0);
            string Mod = null;
            bool foundcache = false;
            var curmonth = Cache.CalendarCaches.Where(s => s.CachedMonth.Year == Date.Year && s.CachedMonth.Month == Date.Month);
            foreach (var item in curmonth)
            {
                if (item.DeleteDate > Date && !foundcache)
                {
                    foundcache = true;
                    Mod = item.cachedpage;
                }
                else
                {
                    Cache.CalendarCaches.Remove(item);
                    //Cache.SaveChanges();
                }
            }
            if (!foundcache)
            {
                CalendarViewModel Mod2 = new CalendarViewModel(Date);
                CalendarCache newcache = new CalendarCache();
                newcache.CachedDate = DateTime.Now;
                newcache.DeleteDate = DateTime.Now.AddDays(7);
                newcache.CachedMonth = Date;
                newcache.cachedpage = ControllerContext.RenderPartialViewToString("~/Views/Calendar/_CalPartial.cshtml",Mod2);
                Mod = newcache.cachedpage;
                if(Cache.CalendarCaches.Where(s=>s.CachedMonth == Date).Count() == 0)
                {
                    Cache.CalendarCaches.Add(newcache);
                    Cache.SaveChanges();
                }
            }        
            return Mod;
        }
    }
}
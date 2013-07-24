using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Joes_Pub_MVC_4.Models;

namespace Joes_Pub_MVC_4.Controllers
{
    public class ProfilesController : Controller
    {
        private ProfilesDBContext db = new ProfilesDBContext();

        //
        // GET: /Profiles/

        public ActionResult Index()
        {
            return View(db.ProfileList.ToList());
        }

        //
        // GET: /Profiles/Details/5

        public ActionResult Details(int id = 0)
        {
            Profiles profiles = db.ProfileList.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // GET: /Profiles/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Profiles/Create

        [HttpPost]
        public ActionResult Create(Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                db.ProfileList.Add(profiles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(profiles);
        }

        //
        // GET: /Profiles/Edit/5

        public ActionResult Edit(int id = 0)
        {
            Profiles profiles = db.ProfileList.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // POST: /Profiles/Edit/5

        [HttpPost]
        public ActionResult Edit(Profiles profiles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(profiles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Manage","Account");
            }
            return View(profiles);
        }

        //
        // GET: /Profiles/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Profiles profiles = db.ProfileList.Find(id);
            if (profiles == null)
            {
                return HttpNotFound();
            }
            return View(profiles);
        }

        //
        // POST: /Profiles/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            Profiles profiles = db.ProfileList.Find(id);
            db.ProfileList.Remove(profiles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
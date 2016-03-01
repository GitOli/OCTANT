using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Framework;
using IdentitySample.Models;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Expert")]
    public class FrameworksController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Frameworks
        public ActionResult Index()
        {
            var frameworks = db.Frameworks.Include(f => f.ApplicationUsers).Include(f => f.Standard);
            return View(frameworks.ToList());
        }

        // GET: Frameworks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Framework.Framework framework = db.Frameworks.Find(id);
            if (framework == null)
            {
                return HttpNotFound();
            }
            return View(framework);
        }

        // GET: Frameworks/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName");
            ViewBag.IdStandard = new SelectList(db.Standards, "IdStandard", "Name");
            return View();
        }

        // POST: Frameworks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFramework,Name,Description,Status,Id,IdStandard")] Framework.Framework framework)
        {
            if (ModelState.IsValid)
            {
                db.Frameworks.Add(framework);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "FullName", framework.Id);
            ViewBag.IdStandard = new SelectList(db.Standards, "IdStandard", "Name", framework.IdStandard);
            return View(framework);
        }

        // GET: Frameworks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Framework.Framework framework = db.Frameworks.Find(id);
            if (framework == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName", framework.Id);
            ViewBag.IdStandard = new SelectList(db.Standards, "IdStandard", "Name", framework.IdStandard);
            return View(framework);
        }

        // POST: Frameworks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFramework,Name,Description,Status,Id,IdStandard")] Framework.Framework framework)
        {
            if (ModelState.IsValid)
            {
                db.Entry(framework).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FullName", framework.Id);
            ViewBag.IdStandard = new SelectList(db.Standards, "IdStandard", "Name", framework.IdStandard);
            return View(framework);
        }

        // GET: Frameworks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Framework.Framework framework = db.Frameworks.Find(id);
            if (framework == null)
            {
                return HttpNotFound();
            }
            return View(framework);
        }

        // POST: Frameworks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Framework.Framework framework = db.Frameworks.Find(id);
            db.Frameworks.Remove(framework);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

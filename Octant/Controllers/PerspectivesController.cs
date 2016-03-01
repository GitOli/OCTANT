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
    public class PerspectivesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Perspectives
        public ActionResult Index()
        {
            var perspectives = db.Perspectives.Include(p => p.Framework);
            return View(perspectives.ToList());
        }

        // GET: Perspectives/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perspective perspective = db.Perspectives.Find(id);
            if (perspective == null)
            {
                return HttpNotFound();
            }
            return View(perspective);
        }

        // GET: Perspectives/Create
        public ActionResult Create()
        {
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name");
            return View();
        }

        // POST: Perspectives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdPerspective,Description,Name,IsDefault,Weight,IdFramework")] Perspective perspective)
        {
            if (ModelState.IsValid)
            {
                db.Perspectives.Add(perspective);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", perspective.IdFramework);
            return View(perspective);
        }

        // GET: Perspectives/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perspective perspective = db.Perspectives.Find(id);
            if (perspective == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", perspective.IdFramework);
            return View(perspective);
        }

        // POST: Perspectives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdPerspective,Description,Name,IsDefault,Weight,IdFramework")] Perspective perspective)
        {
            if (ModelState.IsValid)
            {
                db.Entry(perspective).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", perspective.IdFramework);
            return View(perspective);
        }

        // GET: Perspectives/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Perspective perspective = db.Perspectives.Find(id);
            if (perspective == null)
            {
                return HttpNotFound();
            }
            return View(perspective);
        }

        // POST: Perspectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Perspective perspective = db.Perspectives.Find(id);
            db.Perspectives.Remove(perspective);
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

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
    public class PropositionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Propositions
        public ActionResult Index()
        {
            var propositions = db.Propositions.Include(p => p.Question);
            return View(propositions.ToList());
        }

        // GET: Propositions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposition proposition = db.Propositions.Find(id);
            if (proposition == null)
            {
                return HttpNotFound();
            }
            return View(proposition);
        }

        // GET: Propositions/Create
        public ActionResult Create()
        {
            ViewBag.IdQuestion = new SelectList(db.Questions, "IdQuestion", "Name");
            return View();
        }

        // POST: Propositions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdProposition,Value,Description,IdQuestion")] Proposition proposition)
        {
            if (ModelState.IsValid)
            {
                db.Propositions.Add(proposition);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdQuestion = new SelectList(db.Questions, "IdQuestion", "Name", proposition.IdQuestion);
            return View(proposition);
        }

        // GET: Propositions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposition proposition = db.Propositions.Find(id);
            if (proposition == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdQuestion = new SelectList(db.Questions, "IdQuestion", "Name", proposition.IdQuestion);
            return View(proposition);
        }

        // POST: Propositions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdProposition,Value,Description,IdQuestion")] Proposition proposition)
        {
            if (ModelState.IsValid)
            {
                db.Entry(proposition).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdQuestion = new SelectList(db.Questions, "IdQuestion", "Name", proposition.IdQuestion);
            return View(proposition);
        }

        // GET: Propositions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Proposition proposition = db.Propositions.Find(id);
            if (proposition == null)
            {
                return HttpNotFound();
            }
            return View(proposition);
        }

        // POST: Propositions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Proposition proposition = db.Propositions.Find(id);
            db.Propositions.Remove(proposition);
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

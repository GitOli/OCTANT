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
    [Authorize(Roles = "Admin,Consultant,Manager")]
    public class CustomScoresController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CustomScores
        public ActionResult Index()
        {
            var customScores = db.CustomScores.Include(c => c.ApplicationUsers).Include(c => c.Node);
            return View(customScores.ToList());
        }

        // GET: CustomScores/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomScore customScore = db.CustomScores.Find(id);
            if (customScore == null)
            {
                return HttpNotFound();
            }
            return View(customScore);
        }

        // GET: CustomScores/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name");
            return View();
        }

        // POST: CustomScores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdNode,Value,Comment")] CustomScore customScore)
        {
            if (ModelState.IsValid)
            {
                db.CustomScores.Add(customScore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", customScore.Id);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", customScore.IdNode);
            return View(customScore);
        }

        // GET: CustomScores/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomScore customScore = db.CustomScores.Find(id);
            if (customScore == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", customScore.Id);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", customScore.IdNode);
            return View(customScore);
        }

        // POST: CustomScores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdNode,Value,Comment")] CustomScore customScore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customScore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", customScore.Id);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", customScore.IdNode);
            return View(customScore);
        }

        // GET: CustomScores/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CustomScore customScore = db.CustomScores.Find(id);
            if (customScore == null)
            {
                return HttpNotFound();
            }
            return View(customScore);
        }

        // POST: CustomScores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            CustomScore customScore = db.CustomScores.Find(id);
            db.CustomScores.Remove(customScore);
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

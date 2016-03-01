using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IdentitySample.Models;
using Interview;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Consultant,Manager")]
    public class IntervieweesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Interviewees
        public ActionResult Index()
        {
            var interviewees = db.Interviewees.Include(i => i.Candidate).Include(i => i.Interview);
            return View(interviewees.ToList());
        }

        // GET: Interviewees/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviewee interviewee = db.Interviewees.Find(id);
            if (interviewee == null)
            {
                return HttpNotFound();
            }
            return View(interviewee);
        }

        // GET: Interviewees/Create
        public ActionResult Create()
        {
            ViewBag.IdCandidate = new SelectList(db.Candidates, "IdCandidate", "FirstName");
            ViewBag.IdInterview = new SelectList(db.Interviews, "IdInterview", "Name");
            return View();
        }

        // POST: Interviewees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCandidate,IdInterview")] Interviewee interviewee)
        {
            if (ModelState.IsValid)
            {
                db.Interviewees.Add(interviewee);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdCandidate = new SelectList(db.Candidates, "IdCandidate", "FirstName", interviewee.IdCandidate);
            ViewBag.IdInterview = new SelectList(db.Interviews, "IdInterview", "Name", interviewee.IdInterview);
            return View(interviewee);
        }

        // GET: Interviewees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviewee interviewee = db.Interviewees.Find(id);
            if (interviewee == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdCandidate = new SelectList(db.Candidates, "IdCandidate", "FirstName", interviewee.IdCandidate);
            ViewBag.IdInterview = new SelectList(db.Interviews, "IdInterview", "Name", interviewee.IdInterview);
            return View(interviewee);
        }

        // POST: Interviewees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCandidate,IdInterview")] Interviewee interviewee)
        {
            if (ModelState.IsValid)
            {
                db.Entry(interviewee).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdCandidate = new SelectList(db.Candidates, "IdCandidate", "FirstName", interviewee.IdCandidate);
            ViewBag.IdInterview = new SelectList(db.Interviews, "IdInterview", "Name", interviewee.IdInterview);
            return View(interviewee);
        }

        // GET: Interviewees/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Interviewee interviewee = db.Interviewees.Find(id);
            if (interviewee == null)
            {
                return HttpNotFound();
            }
            return View(interviewee);
        }

        // POST: Interviewees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Interviewee interviewee = db.Interviewees.Find(id);
            db.Interviewees.Remove(interviewee);
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

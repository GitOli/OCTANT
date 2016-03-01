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
using Microsoft.AspNet.Identity;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class CandidatesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Candidates
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var idfirm = db.Users.Find(userid).IdFirm;
            var candidates = db.Candidates.Include(c => c.Customer).Where(c => c.Customer.IdFirm == idfirm).OrderBy(c => c.IdCustomer);
            return View(candidates.ToList());
        }

        // GET: Candidates/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // GET: Candidates/Create
        public ActionResult Create()
        {
            var userid = User.Identity.GetUserId();
            var userfirm = db.Users.Find(userid).IdFirm;
            ViewBag.IdCustomer = new SelectList(db.Customers.Where(c => c.IdFirm == userfirm), "IdCustomer", "Name");
            ViewBag.IdGroup = new SelectList(db.Groups, "IdGroup", "Name");
            return View();
        }

        // POST: Candidates/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCandidate,FirstName,LastName,PhoneNumber,EmailAddress,IdCustomer,Function,Comment")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                candidate.Comment = "";
                db.Candidates.Add(candidate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var userid = User.Identity.GetUserId();
            var userfirm = db.Users.Find(userid).IdFirm;
            ViewBag.IdCustomer = new SelectList(db.Customers.Where(c => c.IdFirm == userfirm), "IdCustomer", "Name");
            return View(candidate);
        }

        // GET: Candidates/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            var userfirm = db.Users.Find(userid).IdFirm;
            var cust = candidate.IdCustomer;
            ViewBag.IdCustomer = new SelectList(db.Customers.Where(c => c.IdFirm == userfirm), "IdCustomer", "Name", cust);
            return View(candidate);
        }

        // POST: Candidates/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCandidate,FirstName,LastName,PhoneNumber,EmailAddress,IdCustomer,IdGroup,Function,Comment")] Candidate candidate)
        {
            if (ModelState.IsValid)
            {
                db.Entry(candidate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var userid = User.Identity.GetUserId();
            var userfirm = db.Users.Find(userid).IdFirm;
            var cust = candidate.IdCustomer;
            ViewBag.IdCustomer = new SelectList(db.Customers.Where(c => c.IdFirm == userfirm), "IdCustomer", "Name", cust);
            return View(candidate);
        }

        // GET: Candidates/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Candidate candidate = db.Candidates.Find(id);
            if (candidate == null)
            {
                return HttpNotFound();
            }
            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Candidate candidate = db.Candidates.Find(id);
            db.Candidates.Remove(candidate);
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

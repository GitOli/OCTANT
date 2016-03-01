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
    [Authorize(Roles = "Admin,Manager")]
    public class ConsultantCampaignsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ConsultantCampaigns
        public ActionResult Index()
        {
            var consultantCampaigns = db.ConsultantCampaigns.Include(c => c.ApplicationUsers).Include(c => c.Campaign);
            return View(consultantCampaigns.ToList());
        }

        // GET: ConsultantCampaigns/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantCampaign consultantCampaign = db.ConsultantCampaigns.Find(id);
            if (consultantCampaign == null)
            {
                return HttpNotFound();
            }
            return View(consultantCampaign);
        }

        // GET: ConsultantCampaigns/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName");
            ViewBag.IdCampaign = new SelectList(db.Campaigns, "IdCampaign", "Name");
            return View();
        }

        // POST: ConsultantCampaigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,IdCampaign,Comment")] ConsultantCampaign consultantCampaign)
        {
            if (ModelState.IsValid)
            {
                db.ConsultantCampaigns.Add(consultantCampaign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", consultantCampaign.Id);
            ViewBag.IdCampaign = new SelectList(db.Campaigns, "IdCampaign", "Name", consultantCampaign.IdCampaign);
            return View(consultantCampaign);
        }

        // GET: ConsultantCampaigns/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantCampaign consultantCampaign = db.ConsultantCampaigns.Find(id);
            if (consultantCampaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", consultantCampaign.Id);
            ViewBag.IdCampaign = new SelectList(db.Campaigns, "IdCampaign", "Name", consultantCampaign.IdCampaign);
            return View(consultantCampaign);
        }

        // POST: ConsultantCampaigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,IdCampaign,Comment")] ConsultantCampaign consultantCampaign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(consultantCampaign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.Users, "Id", "FirstName", consultantCampaign.Id);
            ViewBag.IdCampaign = new SelectList(db.Campaigns, "IdCampaign", "Name", consultantCampaign.IdCampaign);
            return View(consultantCampaign);
        }

        // GET: ConsultantCampaigns/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ConsultantCampaign consultantCampaign = db.ConsultantCampaigns.Find(id);
            if (consultantCampaign == null)
            {
                return HttpNotFound();
            }
            return View(consultantCampaign);
        }

        // POST: ConsultantCampaigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ConsultantCampaign consultantCampaign = db.ConsultantCampaigns.Find(id);
            db.ConsultantCampaigns.Remove(consultantCampaign);
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

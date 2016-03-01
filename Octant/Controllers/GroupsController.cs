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
    public class GroupsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Groups
        public ActionResult Index()
        {
            var groups = db.Groups.Include(g => g.Campaign).ToList();
            return View(groups);
        }

        // GET: Groups/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // GET: Groups/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {
            var userid = User.Identity.GetUserId();
            var idfirm = db.Users.Find(userid).IdFirm;
            var campaigns = db.Campaigns.Include(c => c.ApplicationUsers).Where(c => c.Deleted == false).Where(c => c.ApplicationUsers.IdFirm == idfirm);
            ViewBag.IdCampaign = new SelectList(campaigns, "IdCampaign", "Name");

            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create([Bind(Include = "IdGroup,Name,Description,Comment, IdCampaign")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Groups.Add(group);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var userid = User.Identity.GetUserId();
            var campaigns = db.Campaigns.Where(c => c.Id == userid).Where(c => !c.Deleted);
            ViewBag.IdCampaign = new SelectList(campaigns, "IdCampaign", "Name");

            return View(group);
        }

        // GET: Groups/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            var userid = User.Identity.GetUserId();
            var campaigns = db.ConsultantCampaigns.Where(cc => cc.Id == userid)
                .Select(cc => cc.Campaign).Where(c => !c.Deleted);
            ViewBag.IdCampaign = new SelectList(campaigns, "IdCampaign", "Name", group.IdCampaign);

            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit([Bind(Include = "IdGroup,Name,Description,Comment,IdCampaign")] Group group)
        {
            if (ModelState.IsValid)
            {
                db.Entry(group).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var userid = User.Identity.GetUserId();
            var campaigns = db.ConsultantCampaigns.Where(cc => cc.Id == userid)
                .Select(cc => cc.Campaign).Where(c => !c.Deleted);
            ViewBag.IdCampaign = new SelectList(campaigns, "IdCampaign", "Name", group.IdCampaign);

            return View(group);
        }

        // GET: Groups/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Group group = db.Groups.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }
            return View(group);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DeleteConfirmed(int id)
        {
            Group group = db.Groups.Find(id);
            db.Groups.Remove(group);
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

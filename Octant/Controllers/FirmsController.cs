using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Octant.Models;
using Octant.Models.Interview;
using IdentitySample.Models;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class FirmsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Firms
        public ActionResult Index()
        {
            ApplicationUser actualuser = db.Users.Find(User.Identity.GetUserId());
            List<Firm> firms = new List<Firm>();
            if (User.IsInRole("Admin"))
            {
                firms = db.Firms.ToList();
            }
            else
            {
                firms = db.Firms.Where(x => x.IdFirm == actualuser.IdFirm).ToList();
            }
            var listFirms = (from item in firms
                                 let image = ImageController.GetImage("Firms", item.IdFirm.ToString())
                                 select Tuple.Create(item, image)).ToList();
            return View(listFirms);
        }

        // GET: Firms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firm firm = db.Firms.Find(id);
            if (firm == null)
            {
                return HttpNotFound();
            }
            return View(firm);
        }

        // GET: Firms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Firms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdFirm,Name,Description,Comment")] Firm firm, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                db.Firms.Add(firm);
                db.SaveChanges();
                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Firms", firm.IdFirm.ToString());
                }
                return RedirectToAction("Index");
            }

            return View(firm);
        }

        // GET: Firms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firm firm = db.Firms.Find(id);
            if (firm == null)
            {
                return HttpNotFound();
            }
            var image = ImageController.GetImage("Firms", firm.IdFirm.ToString());
            if (image != "")
            {
                ViewData["image"] = image;
            }
            else
            {
                ViewData["image"] = ImageController.GetImage("Firms", "Default");
            }
            return View(firm);
        }

        // POST: Firms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdFirm,Name,Description,Comment")] Firm firm, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(firm).State = EntityState.Modified;
                db.SaveChanges();
                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Firms", firm.IdFirm.ToString());
                }
                return RedirectToAction("Index");
            }
            return View(firm);
        }

        // GET: Firms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Firm firm = db.Firms.Find(id);
            if (firm == null)
            {
                return HttpNotFound();
            }
            return View(firm);
        }

        // POST: Firms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Firm firm = db.Firms.Find(id);
            db.Firms.Remove(firm);
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

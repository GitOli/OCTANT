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
    public class StandardsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Standards
        public ActionResult Index()
        {
            var standards = db.Standards.ToList();
            var listStandards = (from item in standards 
                                 let image = ImageController.GetImage("Standards", item.IdStandard.ToString()) 
                                 select Tuple.Create(item, image)).ToList();
            return View(listStandards);
        }

        // GET: Standards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var standard = db.Standards.Find(id);
            if (standard == null)
            {
                return HttpNotFound();
            }
            return View(standard);
        }

        // GET: Standards/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Standards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdStandard,Name,Description,Version")] Standard standard, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                db.Standards.Add(standard);
                db.SaveChanges();
                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Standards", standard.IdStandard.ToString());
                }
                ViewBag.ImgTitle = "Upload standard logo";
                return RedirectToAction("Index");
            }

            return View(standard);
        }

        // GET: Standards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var standard = db.Standards.Find(id);
            if (standard == null)
            {
                return HttpNotFound();
            }
            var image = ImageController.GetImage("Standards", standard.IdStandard.ToString());
            if (image != "")
            {
                ViewData["image"] = image;
            }
            else
            {
                ViewData["image"] = ImageController.GetImage("Users", "Default");
            }
            return View(standard);
        }

        // POST: Standards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdStandard,Name,Description,Version")] Framework.Standard standard, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(standard).State = EntityState.Modified;
                db.SaveChanges();
                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Standards", standard.IdStandard.ToString());
                }
                return RedirectToAction("Index");
            }
            return View(standard);
        }

        // GET: Standards/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var standard = db.Standards.Find(id);
            if (standard == null)
            {
                return HttpNotFound();
            }
            return View(standard);
        }

        // POST: Standards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var standard = db.Standards.Find(id);
            db.Standards.Remove(standard);
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

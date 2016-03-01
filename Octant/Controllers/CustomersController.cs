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
    public class CustomersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Customers
        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var userfirm = db.Users.Where(u => u.Id == userid).Select(u => u.IdFirm).FirstOrDefault().GetValueOrDefault();
            var customers = db.Customers.Include(c => c.Industry).Where(c => c.IdFirm == userfirm);
            var listCustomers = new List<Tuple<Customer, string>>();
            foreach (var item in customers)
            {
                var image = ImageController.GetImage("Customers", item.IdCustomer.ToString());
                listCustomers.Add(Tuple.Create(item, image));
            } return View(listCustomers);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.IdIndustry = new SelectList(db.Industries, "IdIndustry", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCustomer,Name,Description,Comment,IdIndustry")] Customer customer, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var firm = db.Users.Find(userid).IdFirm.Value;
                customer.IdFirm = firm;
                db.Customers.Add(customer);
                db.SaveChanges();
                TempData["Msg"] = "Data has been saved successfully";

                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Customers", customer.IdCustomer.ToString());
                }
                ViewBag.ImgTitle = "Upload customer logo";

                return RedirectToAction("Index");
            }

            ViewBag.IdIndustry = new SelectList(db.Industries, "IdIndustry", "Name", customer.IdIndustry);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            var image = ImageController.GetImage("Customers", customer.IdCustomer.ToString());
            if (image != "")
            {
                ViewData["image"] = image;
            }
            else
            {
                ViewData["image"] = ImageController.GetImage("Users", "Default");
            }

            ViewBag.IdIndustry = new SelectList(db.Industries, "IdIndustry", "Name", customer.IdIndustry);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCustomer,Name,Description,Comment,IdIndustry")] Customer customer, HttpPostedFileBase fileBase)
        {
            if (ModelState.IsValid)
            {
                var userid = User.Identity.GetUserId();
                var firm = db.Users.Find(userid).IdFirm.Value;
                customer.IdFirm = firm;
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Customers", customer.IdCustomer.ToString());
                }

                return RedirectToAction("Index");
            }
            ViewBag.IdIndustry = new SelectList(db.Industries, "IdIndustry", "Name", customer.IdIndustry);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer = db.Customers.Find(id);
            var campaigns = db.Campaigns.Where(c => c.IdCustomer == id);
            if (campaigns.Any())
            {
                // To do : message telling that the customer cannot be deleted
                return RedirectToAction("Index");
            }
            db.Customers.Remove(customer);
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

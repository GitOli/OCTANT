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
    public class NodesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Nodes
        public ActionResult Index()
        {
            var nodes = db.Nodes.Include(n => n.ParentNode).Include(n => n.Perspective);
            return View(nodes.ToList());
        }

        // GET: Nodes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Node node = db.Nodes.Find(id);
            if (node == null)
            {
                return HttpNotFound();
            }
            return View(node);
        }

        // GET: Nodes/Create
        public ActionResult Create()
        {
            ViewBag.IdParentNode = new SelectList(db.Nodes, "IdNode", "Name");
            ViewBag.IdPerspective = new SelectList(db.Perspectives, "IdPerspective", "Name");
            return View();
        }

        // POST: Nodes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdNode,Name,Description,Weight,IsRoot,Equation,IdPerspective,IdParentNode")] Node node)
        {
            if (ModelState.IsValid)
            {
                db.Nodes.Add(node);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdParentNode = new SelectList(db.Nodes, "IdNode", "Name", node.IdParentNode);
            ViewBag.IdPerspective = new SelectList(db.Perspectives, "IdPerspective", "Name", node.IdPerspective);
            return View(node);
        }

        // GET: Nodes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Node node = db.Nodes.Find(id);
            if (node == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdParentNode = new SelectList(db.Nodes, "IdNode", "Name", node.IdParentNode);
            ViewBag.IdPerspective = new SelectList(db.Perspectives, "IdPerspective", "Name", node.IdPerspective);
            return View(node);
        }

        // POST: Nodes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdNode,Name,Description,Weight,IsRoot,Equation,IdPerspective,IdParentNode")] Node node)
        {
            if (ModelState.IsValid)
            {
                db.Entry(node).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdParentNode = new SelectList(db.Nodes, "IdNode", "Name", node.IdParentNode);
            ViewBag.IdPerspective = new SelectList(db.Perspectives, "IdPerspective", "Name", node.IdPerspective);
            return View(node);
        }

        // GET: Nodes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Node node = db.Nodes.Find(id);
            if (node == null)
            {
                return HttpNotFound();
            }
            return View(node);
        }

        // POST: Nodes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Node node = db.Nodes.Find(id);
            db.Nodes.Remove(node);
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

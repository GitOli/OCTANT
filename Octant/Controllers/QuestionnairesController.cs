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
using Interview;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Expert,Manager")]
    public class QuestionnairesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET: Questionnaires
        public ActionResult Index()
        {
            var standards = db.Standards.Include(s => s.Frameworks);
            var listStandards = new List<Tuple<Standard, string>>();
            foreach (var item in standards)
            {
                var image = ImageController.GetImage("Standards", item.IdStandard.ToString());
                listStandards.Add(Tuple.Create(item, image));
            }

            return View(listStandards);
        }

        public ActionResult List()
        {
            var questionnaires = db.Questionnaires.Include(q => q.Framework);
            return View(questionnaires.ToList());
        }

        // GET: Questionnaires/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin,Expert")]
        // GET: Questionnaires/Create
        public ActionResult Create()
        {
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name");
            return View();
        }
        [Authorize(Roles = "Admin,Expert")]
        // POST: Questionnaires/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdQuestionnaire,IsDraft,Name,Description,Language,IdFramework,AutoScore")] Questionnaire questionnaire)
        {
            if (ModelState.IsValid)
            {
                db.Questionnaires.Add(questionnaire);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", questionnaire.IdFramework);
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin,Expert")]
        // GET: Questionnaires/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", questionnaire.IdFramework);
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin,Expert")]
        // POST: Questionnaires/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdQuestionnaire,IsDraft,Name,Description,Language,IdFramework,AutoScore")] Questionnaire questionnaire)
        {
            if (ModelState.IsValid)
            {
                db.Entry(questionnaire).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name", questionnaire.IdFramework);
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin,Expert")]
        // GET: Questionnaires/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin,Expert")]
        // POST: Questionnaires/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            db.Questionnaires.Remove(questionnaire);
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

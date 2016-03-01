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
    public class QuestionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Questions
        public ActionResult Index()
        {
            var questions = db.Questions.Include(q => q.Node).Include(q => q.Role).Include(q => q.Questionnaire);
            return View(questions.ToList());
        }

        // GET: Questions/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // GET: Questions/Create
        public ActionResult Create()
        {
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name");
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name");
            ViewBag.IdRole = new SelectList(db.Roles1, "IdRole", "Name");
            return View();
        }

        // POST: Questions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdQuestion,Name,Description,IsFactor,IsRelevant,AnswerType,IdRole,IdQuestionnaire,IdNode,PossibleAnswer")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Questions.Add(question);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", question.IdQuestionnaire);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", question.IdNode);
            ViewBag.IdRole = new SelectList(db.Roles1, "IdRole", "Name", question.IdRole);
            return View(question);
        }

        // GET: Questions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", question.IdQuestionnaire);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", question.IdNode);
            ViewBag.IdRole = new SelectList(db.Roles1, "IdRole", "Name", question.IdRole);
            return View(question);
        }

        // POST: Questions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdQuestion,Name,Description,IsFactor,IsRelevant,AnswerType,IdRole,IdNode,IdQuestionnaire,PossibleAnswer")] Question question)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", question.IdQuestionnaire);
            ViewBag.IdNode = new SelectList(db.Nodes, "IdNode", "Name", question.IdNode);
            ViewBag.IdRole = new SelectList(db.Roles1, "IdRole", "Name", question.IdRole);
            return View(question);
        }

        // GET: Questions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question question = db.Questions.Find(id);
            if (question == null)
            {
                return HttpNotFound();
            }
            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question question = db.Questions.Find(id);
            db.Questions.Remove(question);
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

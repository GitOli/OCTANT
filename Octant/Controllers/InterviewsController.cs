using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Interview;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Consultant,Manager")]
    public class InterviewsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Interviews
        public ActionResult Index()
        {
            var interviews = db.Interviews.Include(i => i.ApplicationUsers).Include(i => i.Campaign).Include(i => i.Campaign.Questionnaire).Where(i => i.Deleted == false);

            var idfirm = db.Users.Find(User.Identity.GetUserId()).IdFirm;
            if ((idfirm != null) && !(User.IsInRole("Admin")))
            {
                interviews = interviews.Where(x => x.Campaign.ApplicationUsers.IdFirm == idfirm);
                if (User.IsInRole("Consultant"))
                {
                    var userid = User.Identity.GetUserId();
                    interviews = interviews.Where(i => i.Id == userid);
                }
            }

            return View(interviews.ToList());
        }

        // GET: Interviews/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // GET: Interviews/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create(int? idcampaign)
        {
            int year = (DateTime.Now.Year) - 1;
            ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;
            var lastId = 0;
            int? firm;
            if (idcampaign != null)
            {
                firm = db.Campaigns.Include(c => c.ApplicationUsers)
                        .Where(c => c.IdCampaign == idcampaign)
                        .Select(c => c.ApplicationUsers.IdFirm).FirstOrDefault();
                ViewBag.Consultant = new SelectList(db.ConsultantCampaigns.Include(cc => cc.ApplicationUsers).Where(cc => cc.IdCampaign == idcampaign).Select(cc => cc.ApplicationUsers).ToList(), "Id", "FullName");
            }
            else
            {
                var userid = User.Identity.GetUserId();
                firm = db.Users.Find(userid).IdFirm;
                ViewBag.Consultant = new SelectList(db.Users.Where(u => u.IdFirm == firm), "Id", "FullName");
            }

            //ViewBag.Id = new SelectList(db.Users.Where(u => u.IdFirm == firm), "Id", "FullName");

            if (TempData.ContainsKey("lastId"))
            {
                lastId = (int)TempData["lastId"];
            }

            ViewBag.IdCampaign = idcampaign != null ? new SelectList(db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.Deleted == false && c.ApplicationUsers.IdFirm == firm), "IdCampaign", "Name", idcampaign)
                : new SelectList(db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.Deleted == false && c.ApplicationUsers.IdFirm == firm), "IdCampaign", "Name", lastId);

            if (idcampaign != null)
            {
                ViewBag.IdCamp = idcampaign;
                ViewBag.IdCampaign = new SelectList(db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.Deleted == false && c.ApplicationUsers.IdFirm == firm), "IdCampaign", "Name",
                    idcampaign);

                ViewBag.Candidates =
                    new MultiSelectList(
                        db.CandidateCampaigns.Where(c => c.IdCampaign == idcampaign)
                            .Include(c => c.Candidate)
                            .OrderBy(c => c.Candidate.LastName).ToList().Select(c => new
                            {
                                IdCandidate = c.IdCandidate,
                                FullName = c.Candidate.FullName
                            }), "IdCandidate", "FullName");
            }
            else
            {
                ViewBag.IdCamp = null;
                ViewBag.IdCampaign = new SelectList(db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.Deleted == false && c.ApplicationUsers.IdFirm == firm), "IdCampaign", "Name", lastId);

                ViewBag.Candidates = new MultiSelectList("");
            }

            ViewBag.HasCampaign = idcampaign ?? 0;
            ViewBag.NameCampaign = idcampaign != null ? db.Campaigns.Find(idcampaign).Name : "";

            return View();
        }

        // POST: Interviews/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdInterview,Date,Name,Comment,Canceled,Suspended,Description,Completion,Id,IdCampaign")] Interview.Interview interview, string Command)
        {
            var idcampaign = interview.IdCampaign;
            if (ModelState.IsValid)
            {
                var c = Request.Form["Candidates"];
                if (c != null)
                {
                    Array candidats = c.Split(new[] { ',' });
                    foreach (var i in from string s in candidats select new Interviewee { IdInterview = interview.IdInterview, IdCandidate = int.Parse(s) })
                    {
                        db.Interviewees.Add(i);
                    }
                }
                interview.Completion = 0;
                interview.Suspended = false;
                interview.Canceled = false;
                db.Interviews.Add(interview);
                db.SaveChanges();
                //idcampaign = db.Interviews.Find(interview.IdInterview).IdCampaign;
                ConductInterviewController cic = new ConductInterviewController();
                cic.SaveCoverageCompletion(idcampaign);

                return Command == "Create" ? RedirectToAction("Details", "Campaigns", new { id = idcampaign })
                    : RedirectToAction("Index", "ConductInterview", new { idinterview = interview.IdInterview });
            }
            ViewBag.Consultant = new SelectList(db.ConsultantCampaigns.Include(cc => cc.ApplicationUsers)
                .Where(cc => cc.IdCampaign == idcampaign)
                .Select(cc => cc.ApplicationUsers).ToList(), "Id", "FullName");
            ViewBag.IdCampaign = new SelectList(db.Campaigns.Where(c => c.Deleted == false), "IdCampaign", "Name", interview.IdCampaign);

            var candidates = from cc in db.CandidateCampaigns
                             join c in db.Candidates on cc.IdCandidate equals c.IdCandidate
                             where cc.IdCampaign == idcampaign
                             orderby c.LastName
                             select new { IdCandidate = c.IdCandidate, FullName = c.FirstName + " " + c.LastName + " - " + c.Function };

            ViewBag.Candidates = new MultiSelectList(candidates, "IdCandidate", "FullName");
            int year = (DateTime.Now.Year) - 1;
            ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;

            return View(interview);
        }

        // GET: Interviews/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var interview = db.Interviews.Where(i => i.IdInterview == id).Include(i => i.Campaign).FirstOrDefault();
            if (interview == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Consultant = new SelectList(db.Users.Where(u => u.IdFirm == interview.Campaign.ApplicationUsers.IdFirm), "Id", "FullName", interview.Id);
            var idcamp = interview.IdCampaign;
            var idfirm = db.Campaigns.Find(idcamp).ApplicationUsers.IdFirm;
            ViewBag.Consultant = new SelectList(db.Users
                .Where(u => u.IdFirm == idfirm)
                .Where(z => z.Roles.Select(y => y.RoleId)
                    .Contains("1f468580-90bc-4e5b-b959-73082975919e") || z.Roles.Select(y => y.RoleId)
                    .Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")), "Id", "FullName", interview.Id);
            //ViewBag.Consultant = new SelectList(db.ConsultantCampaigns.Include(cc => cc.ApplicationUsers).Where(cc => cc.IdCampaign == idcamp).Select(cc => cc.ApplicationUsers), "Id", "FullName", interview.Id);
            ViewBag.IdCampaign = new SelectList(db.Campaigns
                .Where(c => c.Deleted == false 
                    && c.IdCustomer == interview.Campaign.IdCustomer), "IdCampaign", "Name", interview.IdCampaign);
            //ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", interview.Campaign.IdQuestionnaire);
            int year = (DateTime.Now.Year) - 1;
            ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;

            return View(interview);
        }

        // POST: Interviews/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdInterview,Date,Name,Comment,Canceled,Suspended,Description,Completion,IdCampaign,Id")] Interview.Interview interview)
        {
            var b = Request.Form["Consultant"];
            interview.Id = b;
            if (ModelState.IsValid)
            {
                db.Entry(interview).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var idcamp = interview.IdCampaign;

            var consultants =
                db.ConsultantCampaigns.Include(cc => cc.ApplicationUsers)
                    .Where(cc => cc.IdCampaign == idcamp)
                    .Where(z => z.ApplicationUsers.Roles.Select(y => y.RoleId)
                        .Contains("1f468580-90bc-4e5b-b959-73082975919e") ||
                                z.ApplicationUsers.Roles.Select(y => y.RoleId)
                                    .Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf"))
                                    .Select(x => new { Id = x.Id, FullName = x.ApplicationUsers.FirstName + " " + x.ApplicationUsers.LastName }); ;
            ViewBag.Consultant = new SelectList(consultants, "Id", "FullName", interview.Id);

            ViewBag.IdCampaign = db.Campaigns.Single(c => c.IdCampaign == idcamp).Name;

            int year = (DateTime.Now.Year) - 1;
            ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;

            return View(interview);
        }

        // GET: Interviews/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var interview = db.Interviews.Find(id);
            if (interview == null)
            {
                return HttpNotFound();
            }
            return View(interview);
        }

        // POST: Interviews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            //foreach (Answer a in db.Answers.Where(x => x.IdInterview == id)) // Soft delete
            //{
            //    db.Answers.Remove(a); 
            //}
            Interview.Interview interview = db.Interviews.Find(id);
            var idcampaign = interview.IdCampaign;
            //db.Interviews.Remove(interview); // Soft delete
            interview.Deleted = true;
            db.SaveChanges();
            var cic = new ConductInterviewController();
            cic.SaveCoverageCompletion(idcampaign);

            return RedirectToAction("Details", "Campaigns", new { id = interview.IdCampaign });
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

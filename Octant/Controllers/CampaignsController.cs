using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Framework;
using IdentitySample.Models;
using Interview;
using Microsoft.AspNet.Identity;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Manager,Consultant")]
    public class CampaignsController : Controller
    {
        public readonly ApplicationDbContext db = new ApplicationDbContext();
        public readonly ApplicationDbContext dbcs = new ApplicationDbContext();
        public int lastId;

        // GET: Campaigns
        public ActionResult Index()
        {
            var idfirm = db.Users.Find(User.Identity.GetUserId()).IdFirm;
            var campaigns = db.Campaigns.Include(c => c.ApplicationUsers).Include(c => c.Customer).Include(c => c.Questionnaire).Where(c => c.Deleted == false);
            if ((idfirm != null) && !(User.IsInRole("Admin")))
            {
                campaigns = db.Campaigns.Include(c => c.ApplicationUsers).Include(c => c.Customer).Include(c => c.Questionnaire).Where(c => c.Deleted == false).Where(c => c.ApplicationUsers.IdFirm == idfirm);
            }

            var interviews = db.Interviews.ToList();
            ViewBag.Interviews = interviews;

            ViewBag.Campaign1 = db.Campaigns.Count(x => x.Coverage == 0);
            ViewBag.Campaign2 = db.Campaigns.Count(x => (x.Coverage > 0) && (x.Coverage < 100));
            ViewBag.Campaign3 = db.Campaigns.Count(x => x.Coverage == 100);

            return View(campaigns);
        }

        // GET: Campaigns/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }

            var interviews = db.Interviews.Where(x => x.IdCampaign == id).Where(x => x.Deleted == false).ToList();
            ViewBag.Interviews = interviews;

            var candidatescampaign = db.CandidateCampaigns.Where(x => x.IdCampaign == id).Include(x => x.Candidate).Include(x => x.Group);
            var candidates = candidatescampaign.Select(cc => cc.Candidate).ToList();
            ViewBag.Candidates = candidates;

            var consultantscampaign = db.ConsultantCampaigns.Where(x => x.IdCampaign == id).Include(x => x.ApplicationUsers);
            var consultants = consultantscampaign.Select(ccc => ccc.ApplicationUsers).ToList();
            ViewBag.Consultants = consultants;

            // Lists for the selects to add consultant and candidates to a campaign
            if (User.IsInRole("Admin"))
            {
                ViewBag.FullConsultants = new List<ApplicationUser>(db.Users.ToList());
            }
            else
            {
                var idfirm = db.Users.Find(User.Identity.GetUserId()).IdFirm;
                ViewBag.FullConsultants = idfirm != null ? new List<ApplicationUser>(db.Users.Where(x => x.IdFirm == idfirm).ToList()) : new List<ApplicationUser>();
            }
            ViewBag.FullCandidats = db.Candidates.Where(c => c.IdCustomer == campaign.IdCustomer);
            var grps = db.Groups.Where(g => g.IdCampaign == id).ToList();
            ViewBag.FullGroups = grps;

            ViewBag.UserImage = ImageController.GetImage("Users", campaign.Id);
            ViewBag.CustomerImage = ImageController.GetImage("Customers", campaign.IdCustomer.ToString());

            return View(campaign);
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public string AddConsultant(int idCampaign, string idConsultant)
        {
            try
            {
                var cc = new ConsultantCampaign { IdCampaign = idCampaign, Id = idConsultant };
                db.ConsultantCampaigns.Add(cc);
                db.SaveChanges();
                return "success";
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public string AddCandidat(int idCampaign, int idCandidat, int idGroup)
        {
            try
            {
                var ca = new CandidateCampaign { IdCampaign = idCampaign, IdCandidate = idCandidat, IdGroup = idGroup };
                db.CandidateCampaigns.Add(ca);
                db.SaveChanges();
                return "success";
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public ActionResult AddCandidatAjax(string[] candidate)
        {
            try
            {
                var ca = new Candidate
                {
                    FirstName = candidate[0],
                    LastName = candidate[1],
                    PhoneNumber = candidate[2],
                    EmailAddress = candidate[3],
                    IdCustomer = Convert.ToInt32(candidate[4]),
                    Comment = candidate[6],
                    Function = candidate[7]
                };
                db.Candidates.Add(ca);
                db.SaveChanges();
                return Json(new { id = ca.IdCandidate, name = ca.FullName, message = "success" });
            }
            catch (Exception e)
            {
                return Json(new { message = "error" });
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public ActionResult GetCandidates(int idCustomer)
        {
            try
            {
                var candidates = new JavaScriptSerializer().Serialize(new List<Candidate>(db.Candidates.Where(c => c.IdCustomer == idCustomer).ToList()));
                return Json(candidates);
            }
            catch (Exception e)
            {
                return Json(new { message = "error" });
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public ActionResult GetChartData(int idnode, int idcampaign)
        {
            var pnodes = db.Nodes.Where(n => n.IdParentNode == idnode).ToList();
            
            var radar = new List<Tuple<string, string, double>>(); // Group Name, Node name, Score
            var list = new List<Tuple<string, string, double, int>>(); // Groupe name, Node name, Total, Times

            var interviewees = db.Interviewees.Include(i => i.Interview).Where(i => i.Interview.IdCampaign == idcampaign && !i.Interview.Deleted).ToList();

            foreach (var intrvee in interviewees)
            {
                var candi = db.CandidateCampaigns.Include(c => c.Group).FirstOrDefault(cc => cc.IdCandidate == intrvee.IdCandidate && cc.IdCampaign == idcampaign);
                if (candi == null) continue;
                var group = candi.Group;
                
                foreach (var node in pnodes)
                {
                    var nodename = node.Name;
                    if (nodename.Length > 25)
                    {
                        nodename = nodename.Substring(0, 25);
                        nodename += "...";
                    }
                    var score = db.CustomScores.FirstOrDefault(c => c.IdNode == node.IdNode && c.IdInterview == intrvee.IdInterview);
                    var average = 0.0;
                    if (score != null) average = score.Value;
                    var count = 1;

                    if (list.Any())
                    {
                        var cur = list.Find(x => x.Item1 == @group.Name && x.Item2 == node.Name);
                        if (cur != null)
                        {
                            var value = 0;
                            if (score != null)
                            {
                                value = score.Value;
                            }
                            average = cur.Item3 + value;
                            count += cur.Item4;
                            list.Remove(cur);
                        }
                    }

                    list.Add(new Tuple<string, string, double, int>(@group.Name, nodename, average, count));
                }
            }

            foreach (var item in list)
            {
                var tmp = item.Item3;
                if (item.Item4 > 0)
                {
                    tmp = item.Item3 / item.Item4;
                }
                radar.Add(new Tuple<string, string, double>(item.Item1, item.Item2, Convert.ToInt32(tmp)));
            }
            radar.Sort((x, y) => string.Compare(y.Item1, x.Item1, StringComparison.Ordinal));
            var rad = radar.OrderBy(r => r.Item1);
            try
            {
                return Json(new JavaScriptSerializer().Serialize(rad.ToList()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { message = e.Message });
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public string DelConsultant(int idCampaign, string idConsultant)
        {
            try
            {
                var ca = db.ConsultantCampaigns.FirstOrDefault(x => (x.IdCampaign == idCampaign) && (x.Id == idConsultant));
                if (ca == null)
                {
                    return "error";
                }
                db.ConsultantCampaigns.Remove(ca);
                db.SaveChanges();
                Campaign campaign = db.Campaigns.Find(idCampaign);
                ViewBag.FullCandidats = new List<Candidate>(db.Candidates.Where(x => x.IdCustomer == campaign.IdCustomer).ToList());

                return "success";
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        public string DelCandidat(int idCampaign, int idCandidat)
        {
            try
            {
                var ca = db.CandidateCampaigns.FirstOrDefault(x => (x.IdCampaign == idCampaign) && (x.IdCandidate == idCandidat));
                if (ca == null)
                {
                    return "error";
                }
                db.CandidateCampaigns.Remove(ca);
                db.SaveChanges();
                return "success";
            }
            catch (Exception e)
            {
                return "error";
            }
        }

        // GET: Campaigns/Create
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Create()
        {
            var idcurrentuser = User.Identity.GetUserId();
            var firm = db.Users.Where(u => u.Id == idcurrentuser).Select(u => u.IdFirm).FirstOrDefault();
            ViewBag.Id = new SelectList(db.Users.Where(u => u.IdFirm == firm), "Id", "FullName", idcurrentuser);
            ViewBag.IdCustomer = User.IsInRole("Admin") ? new SelectList(db.Customers, "IdCustomer", "Name") : new SelectList(db.Customers.Where(c => c.IdFirm == firm), "IdCustomer", "Name");
            ViewBag.IdGroup = new SelectList(db.Groups, "IdGroup", "Name");
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name");
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name");
            //ViewBag.Candidates = new MultiSelectList(db.Candidates.Include(c => c.Customer).Where(c => c.Customer.IdFirm == firm), "IdCandidate", "FullNameAndFunction");
            ViewBag.ConsultantUsers = new MultiSelectList(db.Users.Where(cu => cu.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1f468580-90bc-4e5b-b959-73082975919e")), "Id", "FullName");
            return View();
        }

        // POST: Campaigns/Create
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IdCampaign,Name,StartDate,EndDate,Description,Status,Comment,Coverage,Id,IdCustomer,IdFramework,IdQuestionnaire")] Campaign campaign)
        {
            if (campaign.StartDate.Value.Date > campaign.EndDate.Value.Date)
            {
                int year = (DateTime.Now.Year) - 1;
                ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;
                ModelState.AddModelError("", "Error on the date.");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    db.Campaigns.Add(campaign);
                    campaign.Coverage = 0;
                    campaign.Completion = 0;
                    db.SaveChanges();

                    var u = Request.Form["ConsultantUsers"];
                    if (u != null)
                    {
                        Array consultants = u.Split(new[] { ',' });
                        foreach (var co in from string us in consultants select new ConsultantCampaign { IdCampaign = campaign.IdCampaign, Id = us })
                        {
                            db.ConsultantCampaigns.Add(co);
                            db.SaveChanges();
                        }
                    }
                    lastId = campaign.IdCampaign;
                    TempData["lastId"] = lastId;

                    return RedirectToAction("Index", "Home");
                }
            }
            var idcurrentuser = User.Identity.GetUserId();
            var firm = db.Users.Where(u => u.Id == idcurrentuser).Select(u => u.IdFirm).FirstOrDefault();
            ViewBag.Id = new SelectList(db.Users.Where(u => u.IdFirm == firm), "Id", "FullName", idcurrentuser);
            ViewBag.IdCustomer = User.IsInRole("Admin") ? new SelectList(db.Customers, "IdCustomer", "Name") : new SelectList(db.Customers.Where(c => c.IdFirm == firm), "IdCustomer", "Name");
            ViewBag.IdGroup = new SelectList(db.Groups, "IdGroup", "Name");
            ViewBag.IdFramework = new SelectList(db.Frameworks, "IdFramework", "Name");
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name");
            //ViewBag.Candidates = new MultiSelectList(db.Candidates.Include(c => c.Customer).Where(c => c.Customer.IdFirm == firm), "IdCandidate", "FullNameAndFunction");
            ViewBag.ConsultantUsers = new MultiSelectList(db.Users.Where(cu => cu.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1f468580-90bc-4e5b-b959-73082975919e")), "Id", "FullName");

            return View(campaign);
        }

        // GET: Campaigns/Edit/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            var idcurrentuser = User.Identity.GetUserId();
            var firm =
                db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.IdCampaign == id)
                    .Select(c => c.ApplicationUsers.IdFirm)
                    .FirstOrDefault();
            if ((firm != null)) // && !(User.IsInRole("Admin"))
            {
                ViewBag.Manager = new SelectList(db.Users.Where(u => u.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")), "Id", "FullName", campaign.Id);
            }
            //else
            //{
            //    ViewBag.Manager = new SelectList(db.Users.Where(z => z.Roles.Select(y => y.RoleId).Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")), "Id", "FullName", campaign.Id);
            //}
            //ViewBag.IdCustomer = User.IsInRole("Admin") ? new SelectList(db.Customers, "IdCustomer", "Name", campaign.IdCustomer) : new SelectList(db.Customers.Where(c => c.IdFirm == firm), "IdCustomer", "Name", campaign.IdCustomer);
            ViewBag.IdCustomer = new SelectList(db.Customers.Where(c => c.IdFirm == firm), "IdCustomer", "Name", campaign.IdCustomer);
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", campaign.IdQuestionnaire);
            ViewBag.IdGroup = new SelectList(db.Groups, "IdGroup", "Name");
            //ViewBag.Candidates = new MultiSelectList(db.Candidates.Include(c => c.Customer).Where(c => c.Customer.IdFirm == firm && c.IdCustomer == campaign.IdCustomer), "IdCandidate", "FullNameAndFunction");
            //ViewBag.CandidatesSelected = new JavaScriptSerializer().Serialize(new List<int>(db.CandidateCampaigns.Where(ca => ca.IdCampaign == campaign.IdCampaign).Select(ca => ca.IdCandidate).ToList()));
            ViewBag.ConsultantUsers = new MultiSelectList(db.Users.Where(cu => cu.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1f468580-90bc-4e5b-b959-73082975919e")), "Id", "FullName");
            ViewBag.ConsultantUsersSelected = new JavaScriptSerializer().Serialize(new List<string>(db.ConsultantCampaigns.Where(co => co.IdCampaign == campaign.IdCampaign).Select(co => co.Id).ToList()));
            int year = (DateTime.Now.Year) - 1;
            ViewBag.MinDateDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;

            return View(campaign);
        }

        // POST: Campaigns/Edit/5
        // Afin de déjouer les attaques par sur-validation, activez les propriétés spécifiques que vous voulez lier. Pour 
        // plus de détails, voir  http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IdCampaign,Name,StartDate,EndDate,Description,Status,Comment,Coverage,IdCustomer,IdFramework,IdQuestionnaire,Completion")] Campaign campaign)
        {
            if (campaign.StartDate.Value.Date > campaign.EndDate.Value.Date)
            {
                int year = (DateTime.Now.Year) - 1;
                ViewBag.DateOfDay = "" + DateTime.Now.Day + "/" + DateTime.Now.Month + "/" + year;
                ModelState.AddModelError("", "Error on the date.");
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var b = Request.Form["Manager"];
                    campaign.Id = b;

                    var c = Request.Form["ConsultantUsers"];
                    if (c != null)
                    {
                        Array consultants = c.Split(',');

                        foreach (var y in from string consu in consultants select new ConsultantCampaign { IdCampaign = campaign.IdCampaign, Id = consu })
                        {
                            foreach (var consutodel in db.ConsultantCampaigns.Where(cc => cc.IdCampaign == campaign.IdCampaign))
                            {
                                db.Entry(consutodel).State = EntityState.Deleted;
                            }
                            db.Entry(y).State = EntityState.Added;
                        }
                        db.SaveChanges();
                    }
                    else
                    {
                        foreach (var consutodel in db.ConsultantCampaigns.Where(cc => cc.IdCampaign == campaign.IdCampaign))
                        {
                            db.Entry(consutodel).State = EntityState.Deleted;
                        }
                    }

                    db.Entry(campaign).State = EntityState.Modified;
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }
            }

            var id = campaign.IdCampaign;
            var firm =
                db.Campaigns.Include(c => c.ApplicationUsers)
                    .Where(c => c.IdCampaign == id)
                    .Select(c => c.ApplicationUsers.IdFirm)
                    .FirstOrDefault();
            if ((firm != null) && !(User.IsInRole("Admin")))
            {
                ViewBag.Manager = new SelectList(db.Users.Where(u => u.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")), "Id", "FullName", campaign.Id);
            }
            else
            {
                ViewBag.Manager = new SelectList(db.Users.Where(z => z.Roles.Select(y => y.RoleId).Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")), "Id", "FullName", campaign.Id);
            }
            ViewBag.IdCustomer = User.IsInRole("Admin") ? new SelectList(db.Customers, "IdCustomer", "Name", campaign.IdCustomer) : new SelectList(db.Customers.Where(c => c.IdFirm == firm), "IdCustomer", "Name", campaign.IdCustomer);
            ViewBag.IdQuestionnaire = new SelectList(db.Questionnaires, "IdQuestionnaire", "Name", campaign.IdQuestionnaire);
            ViewBag.IdGroup = new SelectList(db.Groups, "IdGroup", "Name");
            //ViewBag.Candidates = new MultiSelectList(db.Candidates.Include(c => c.Customer).Where(c => c.Customer.IdFirm == firm && c.IdCustomer == campaign.IdCustomer), "IdCandidate", "FullNameAndFunction");
            //ViewBag.CandidatesSelected = new JavaScriptSerializer().Serialize(new List<int>(db.CandidateCampaigns.Where(ca => ca.IdCampaign == campaign.IdCampaign).Select(ca => ca.IdCandidate).ToList()));
            ViewBag.ConsultantUsers = new MultiSelectList(db.Users.Where(cu => cu.IdFirm == firm).Where(z => z.Roles.Select(y => y.RoleId).Contains("1f468580-90bc-4e5b-b959-73082975919e")), "Id", "FullName");
            ViewBag.ConsultantUsersSelected = new JavaScriptSerializer().Serialize(new List<string>(db.ConsultantCampaigns.Where(co => co.IdCampaign == campaign.IdCampaign).Select(co => co.Id).ToList()));

            return View(campaign);
        }

        // GET: Campaigns/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        // POST: Campaigns/Delete/5
        [Authorize(Roles = "Admin,Manager")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var campaign = db.Campaigns.Find(id);
            //db.Campaigns.Remove(campaign); // Soft Delete
            campaign.Deleted = true;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public double GetWeightedAverage(int idinterview)
        {
            float result = 0;
            var cpt = 0;
            foreach (var item in dbcs.CustomScores.Include(c => c.Node).Where(c => c.IdInterview == idinterview))
            {
                result += item.Value * (item.Node.Weight / 10);
                cpt++;
            }
            if (cpt > 0)
            {
                result = result / cpt;
            }
            return Math.Floor(result);
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Saved(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var campaign = db.Campaigns.Include(c => c.ApplicationUsers).Include(a => a.Customer).FirstOrDefault(c => c.IdCampaign == id);
            if (campaign != null)
            {
                ViewBag.Customer = db.Customers.Find(campaign.IdCustomer).Name;
            }

            var interviews = db.Interviews.Where(o => o.IdCampaign == id).Where(o => !o.Deleted);
            ViewBag.Interviews = interviews.ToList();
            //ViewBag.Questions = db.Questions.Where(w => w.IdQuestionnaire == campaign.IdQuestionnaire).ToList();
            var autoscore =
                db.Questionnaires.Where(q => q.IdQuestionnaire == campaign.IdQuestionnaire)
                    .Select(q => q.AutoScore)
                    .FirstOrDefault();

            if (autoscore)
            {
                var interviewsscores = interviews.ToList().Select(i => new Tuple<Interview.Interview, double>(i, Convert.ToDouble(i.Score)));
                ViewBag.InterviewsScores = interviewsscores;
            }
            else
            {
                var listinterviewsscores = interviews.ToList().Select(i => new Tuple<Interview.Interview, double>(i, GetWeightedAverage(i.IdInterview)));
                ViewBag.InterviewsScores = listinterviewsscores;
            }

            // Get all nodes of the campaign's questionnaire
            var fullnodes = db.Nodes.Where(x => x.IdQuestionnaire == campaign.IdQuestionnaire).ToList();
            // Get Sub-nodes
            List<Node> nodes = null;
            // Get Principal nodes
            List<Node> pnodes = null;
            ViewBag.lvl = false;
            if (fullnodes.Any())
            {
                var root = fullnodes.FirstOrDefault(r => r.IsRoot);
                if (root != null)
                {
                    nodes = fullnodes.Where(n => !n.IsRoot && n.IdParentNode != root.IdNode).ToList();
                    pnodes = fullnodes.FindAll(p => p.IdParentNode == root.IdNode).ToList();
                    ViewBag.RootNode = root.IdNode;
                }
                if (nodes != null && nodes.Any())
                {
                    ViewBag.lvl = true;
                }
            }

            ViewBag.Nodes = nodes;
            ViewBag.PNodes = pnodes;

            /* Get chart data and building a list (List<Tuple>) that will be inject the data into the chart, also update scores */

            /* Variables declaration */
            var dbfu = new ApplicationDbContext();

            var groups =
                dbfu.CandidateCampaigns.Where(cc => cc.IdCampaign == campaign.IdCampaign).Select(cc => cc.Group).Distinct().ToList();
            ViewBag.Groups = groups.OrderBy(g => g.Name).ToList();
            var list = new List<Tuple<string, string, double, int>>(); // Groupe name, Node name, Total, Times
            var radarList = new List<Tuple<string, string, double>>(); // Group name, Node name, Score
            var nodeList = new List<Node>();
            var average = 0.0;
            var total = 0.0;

            /* Build chart data list (List<Tuple>) */
            foreach (var group in groups)
            {
                var tmp = dbfu.Interviewees.Include(i => i.Interview)
                    .Where(i => i.Interview.IdCampaign == id)
                    .Join(dbfu.CandidateCampaigns.Where(cc => cc.IdGroup == group.IdGroup), i => i.IdCandidate, cc => cc.IdCandidate, (interviewee, candidateCampaign) => interviewee.Interview)
                    .Distinct().ToList();
                var intrv = tmp.Where(i => !i.Deleted);
                foreach (var interview in intrv)
                {
                    List<Tuple<int, double>> sc = null;
                    if (autoscore)
                    {
                        sc = GetNodesScore(interview.IdInterview);
                    }

                    foreach (var node in pnodes)
                    {
                        /* Update scores or Add if not exists */
                        var current = sc.Find(x => x.Item1 == node.IdNode);
                        if (autoscore && sc != null && current != null)
                        {
                            var cs = new CustomScore
                            {
                                Id = User.Identity.GetUserId(),
                                IdNode = node.IdNode,
                                IdInterview = interview.IdInterview
                            };

                            var val = Convert.ToInt32(current.Item2);
                            cs.Value = val;

                            var param = new object[3];
                            param[0] = cs.Id;
                            param[1] = cs.IdInterview;
                            param[2] = cs.IdNode;
                            var currentcs = db.CustomScores.Find(param);

                            if (currentcs != null)
                            {
                                currentcs.Value = cs.Value;
                            }
                            else
                            {
                                dbfu.CustomScores.Add(cs);
                            }
                            
                            dbfu.SaveChanges();
                        }

                        /* Get scores by group for the chart */
                        var score = dbfu.CustomScores.FirstOrDefault(c => c.IdNode == node.IdNode && c.IdInterview == interview.IdInterview);
                        if (score != null)
                        {
                            var candi = dbfu.Interviewees.Where(i => i.IdInterview == interview.IdInterview).Select(i => i.Candidate).FirstOrDefault();
                            if (candi != null)
                            {
                                var idgroup =
                                dbfu.CandidateCampaigns.Where(
                                    cc => cc.IdCampaign == interview.IdCampaign && cc.IdCandidate == candi.IdCandidate).Select(cc => cc.IdGroup).FirstOrDefault();
                                var count = 1;
                                if (idgroup == group.IdGroup)
                                {
                                    average = score.Value;
                                    if (list.Any())
                                    {
                                        var cur = list.Find(x => x.Item1 == group.Name && x.Item2 == node.Name);
                                        if (cur != null)
                                        {
                                            average = cur.Item3 + score.Value;
                                            count += cur.Item4;
                                            list.Remove(cur);
                                        }
                                    }
                                    var nodename = node.Name;
                                    if (nodename.Length > 25)
                                    {
                                        nodename = nodename.Substring(0, 25);
                                        nodename += "...";
                                    }
                                    list.Add(new Tuple<string, string, double, int>(group.Name, nodename, average, count));
                                    if (!nodeList.Contains(node))
                                    {
                                        nodeList.Add(node);
                                    }
                                }
                            }
                        }
                    }
                }
                total += average;
            }
            foreach (var item in list)
            {
                var tmp = item.Item3;
                if (item.Item4 > 0)
                {
                    tmp = item.Item3 / item.Item4;
                }
                radarList.Add(new Tuple<string, string, double>(item.Item1, item.Item2, tmp));
            }
            //radarList = list.Select(item => new Tuple<string, string, double>(item.Item1, item.Item2, item.Item3 / item.Item4)).ToList();
            //radarList.Sort((x, y) => string.Compare(y.Item1, x.Item1, StringComparison.Ordinal));
            var radar = radarList.OrderBy(r => r.Item1).ThenBy(r => r.Item2).ToList();
            ViewBag.Radar = radar;

            /* Average from CustomScores of nodes */
            var nodescore = (from n in pnodes
                             let cs = db.CustomScores.Include(i => i.Interview)
                .Where(x => x.IdNode == n.IdNode && x.Interview.IdCampaign == id && !x.Interview.Deleted).ToList()
                             where cs.Count > 0
                             select Tuple.Create(n.Name, Math.Round(cs.Average(x => x.Value), 2))).ToList();

            var snodescore = (from n in nodes
                              let cs = db.CustomScores.Include(i => i.Interview)
                                  .Where(x => x.IdNode == n.IdNode && x.Interview.IdCampaign == id && !x.Interview.Deleted).ToList()
                              where cs.Count > 0
                              select Tuple.Create(n.Name, Math.Round(cs.Average(x => x.Value), 2))).ToList();

            nodescore.AddRange(snodescore);
            //var part = 0.0;
            //var ci = new ConductInterviewController();
            //foreach (var interview in interviews)
            //{
            //    part += ci.GetInterviewScore(interview.IdInterview);
            //}
            //ViewBag.Part = (part/interviews.Count()).ToString(CultureInfo.InvariantCulture);
            ViewBag.Nodescore = nodescore;
            ViewBag.Questionnaire = db.Campaigns.Include(c => c.Questionnaire).Where(c => c.IdCampaign == id).Select(c => c.Questionnaire.Name).Single();
            ViewBag.NodeList = new SelectList(nodeList.ToList(), "IdNode", "Name");
            return View(campaign);
        }

        public double GetSubNodeScore(int? idinterview, int idnode)
        {
            var dbs = new ApplicationDbContext();
            var scores = dbs.Answers.Include(a => a.Question)
                .Where(a => a.IdInterview == idinterview && a.Question.IdNode == idnode)
                .Select(a => a.IntervieweeAnswer).ToList();
            var nodescore = scores.Aggregate(0.0, (current, score) => current + Convert.ToInt32(score));
            if (scores.Any())
            {
                return nodescore / scores.Count;
            }
            return nodescore;
        }

        public List<Tuple<int, double>> GetNodesScore(int? idinterview)
        {
            var dbctx = new ApplicationDbContext();
            var idquest = dbctx.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == idinterview)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            var tmp = dbctx.Nodes.Where(n => n.IdQuestionnaire == idquest);
            var rootnode = tmp.FirstOrDefault(i => i.IsRoot);
            var pnodes = tmp.Where(t => t.IdParentNode == rootnode.IdNode);
            var score = 0.0;
            var cpt = 0;
            var listscore = new List<Tuple<int, double>>(); // idnode, score
            foreach (var node in pnodes)
            {
                var dbu = new ApplicationDbContext();
                var snodes = dbu.Nodes.Where(n => n.IdQuestionnaire == idquest
                    && n.IdParentNode == node.IdNode).ToList();
                if (snodes.Count == 0)
                {
                    score += GetSubNodeScore(idinterview, node.IdNode);
                    cpt++;
                }
                else
                {
                    foreach (var snode in snodes)
                    {
                        score += GetSubNodeScore(idinterview, snode.IdNode);
                        cpt++;
                    }
                }
                if (cpt > 0)
                {
                    score = score / cpt;
                    listscore.Add(new Tuple<int, double>(node.IdNode, score));
                }

                cpt = 0;
                score = 0.0;
                dbu.Dispose();
            }

            return listscore;
        }

        public int GetAutoScore(int? idinterview)
        {
            var finalscore = 0.0;
            var scores = GetNodesScore(idinterview);
            var cpt = 0;
            foreach (var score in scores.Where(score => score.Item1 != null))
            {
                finalscore += score.Item2;
                cpt++;
            }
            if (cpt > 0)
            {
                finalscore = finalscore / cpt;
            }

            return Convert.ToInt32(finalscore);
        }



        [Authorize(Roles = "Admin,Manager")]
        public ActionResult Pdf(int? id, bool isfirm)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var campaign = db.Campaigns.Include(c => c.ApplicationUsers).Include(a => a.Customer).FirstOrDefault(c => c.IdCampaign == id);

            ViewBag.Interviews = db.Interviews.Where(o => o.IdCampaign == id).Where(o => o.Deleted == false).ToList();
            ViewBag.Questions = db.Questions.Where(w => w.IdQuestionnaire == campaign.IdQuestionnaire).OrderBy(x => x.IdNode).ToList();
            ViewBag.FirmImage = ImageController.GetImage("Firms", campaign.ApplicationUsers.IdFirm.ToString());
            ViewBag.CustomerImage = ImageController.GetImage("Customers", campaign.IdCustomer.ToString());

            List<Tuple<global::Interview.Interview, double>> listinterviewsscores = new List<Tuple<global::Interview.Interview, double>>();
            foreach (var i in db.Interviews.Where(o => o.IdCampaign == id).Where(o => o.Deleted == false))
            {
                listinterviewsscores.Add(new Tuple<global::Interview.Interview, double>(i, GetWeightedAverage(i.IdInterview)));
            }
            ViewBag.InterviewsScores = listinterviewsscores;

            ViewBag.IsFirm = isfirm;

            return View(campaign);
        }

        [Authorize(Roles = "Admin,Manager")]
        public ActionResult DownloadPDF(int? id, bool isfirm)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var cookieCollection = Request.Cookies.AllKeys.ToDictionary(key => key, key => Request.Cookies.Get(key).Value);

            var campaign = db.Campaigns.Find(id);

            return new Rotativa.ActionAsPdf("Pdf", new { id = id, isfirm = isfirm }) { FileName = "Campaign_" + campaign.Name + (isfirm ? "_Firm.pdf" : "_Customer.pdf"), Cookies = cookieCollection };
        }

        //TO DELETE (this put data in custom scores table for testing purpose)
        public void UpdateCs()
        {
            var customscores = db.CustomScores.Where(cs => cs.IdInterview == 38 || cs.IdInterview == 2094 || cs.IdInterview == 2095);
            var r = new Random();
            foreach (var score in customscores)
            {
                score.Value = r.Next(5);
            }
            var campaign = db.Campaigns.Find(db.Interviews.Find(38).IdCampaign);
            var avg = customscores.Average(cs => cs.Value);
            campaign.Score = (int)avg;
            db.SaveChanges();
        }
    }
}
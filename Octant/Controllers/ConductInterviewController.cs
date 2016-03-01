using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Framework;
using IdentitySample.Models;
using Interview;
using Microsoft.AspNet.Identity;

//using Syncfusion.Olap.Reports;
//using Interview = Interview.Interview;
using Octant.Migrations;
using Syncfusion.Linq;

namespace Octant.Controllers
{
    [Authorize(Roles = "Admin,Consultant,Manager")]
    public class ConductInterviewController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();
        private readonly ApplicationDbContext dbcs = new ApplicationDbContext();
        private DbSet<Answer> answers;

        private List<Tuple<int, int, string, string, bool>> comments;
        // idquestion, idrole, comment, consultantcomment, isrelevant?

        private int iditv;

        private List<Tuple<int, int>> list;
        private DbSet<Node> nodes;
        private List<Tuple<int, string, string, string, string, int, bool>> order;
        private IQueryable<Question> questions;
        private Node rootnode;

        // Recursive function to display questions and nodes in right HTML order
        private String createTree(int i, bool edit)
        {
            if (list.Count == 0)
                return "";
            if (i >= list.Count)
            {
                return "";
            }
            var html = "";
            var node = nodes.Find(list[i].Item2);
            
            var yCpt = list[i].Item2;
            var cpt = questions.Count(x => x.IdNode == yCpt && x.IsRelevant);
            if ((list[i].Item1 == list[i].Item2) || (list[i].Item1 == rootnode.IdNode))
            {
                html += "<tr class=\"node node1 toClose\"><td><i class=\"ion ion-arrow-down-b\"></i></td><td>" + node.Name + " <sub>(" + cpt + "questions)</sub>" + "</td><td class=\"node-progression completion\">0%</td></tr>";
            }
            else
            {
                html += "<tr class=\"node node2\"><td><i class=\"ion ion-ios-arrow-down\"></i></td><td>" + node.Name + " <sub>(" + cpt + "questions)</sub>" + "</td><td class=\"node-progression completion\">0%</td></tr>";
            }

            if ((list[i].Item1 == list[i].Item2) || (list[i].Item1 == rootnode.IdNode))
            {
                html += "<tr class=\"node node1 toClose\"><td><i class=\"ion ion-arrow-down-b\"></i></td><td>" + node.Name + "</td><td class=\"node-progression completion\">0%</td></tr>";
            }
            else
            {
                html += "<tr class=\"node node2\"><td><i class=\"ion ion-ios-arrow-down\"></i></td><td>" + node.Name + "</td><td class=\"node-progression completion\">0%</td></tr>";
            }

            List<Answer> ans = null;
            if (edit && (iditv != 0))
            {
                ans = answers.Where(x => x.IdInterview == iditv).ToList();
            }

            var y = list[i].Item2;
            foreach (var question in questions.Where(x => x.IdNode == y && x.IsRelevant))
            {
                var answer = "";
                var multi = false;

                switch (question.AnswerType)
                {
                    case AnswerType.Text:
                        answer = "Text";
                        break;
                    case AnswerType.Integer:
                        answer = "Integer";
                        break;
                    case AnswerType.SingleChoice:
                        answer = question.PossibleAnswer;
                        break;
                    case AnswerType.MultipleChoice:
                        answer = question.PossibleAnswer;
                        multi = true;
                        break;
                    case AnswerType.Boolean:
                        answer = "Boolean";
                        break;
                    case AnswerType.Real:
                        answer = "Real";
                        break;
                    default:
                        break;
                }

                Answer a = null;
                if (edit && (ans != null) && (ans.Count > 0))
                {
                    a = ans.FirstOrDefault(x => x.IdQuestion == question.IdQuestion);
                    order.Add(Tuple.Create(question.IdQuestion, question.Name, question.Description, answer,
                        a.IntervieweeAnswer, question.IdRole, multi));
                    comments.Add(Tuple.Create(question.IdQuestion, question.IdRole, a.Comment, a.ConsultantComment,
                        (a.Value != 0)));
                }
                else
                {
                    order.Add(Tuple.Create(question.IdQuestion, question.Name, question.Description, answer, "",
                        question.IdRole, multi));
                    comments.Add(Tuple.Create(question.IdQuestion, question.IdRole, "", "", question.IsRelevant));
                }

                html += "<tr class=\"question\" ng-click=\"setQuestion(" + question.IdQuestion + ")\">";
                html += "<td><i class=\"ion ion-ios-arrow-thin-right\"></i></td><td>" + question.Name + "</td>";

                if (edit && (ans != null))
                {
                    if ((a != null) && (a.Value == 0))
                    {
                        html += "<td class=\"question-validate\">";
                        html += "<i class=\"ion ion-ios-checkmark-empty text-success\"></i>";
                        html += "<i class=\"ion ion-ios-close-empty text-danger\" style=\"display: block;\"></i>";
                        html += "</td>";
                    }
                    else
                    {
                        if ((a != null) && (!string.IsNullOrEmpty(a.IntervieweeAnswer)))
                        {
                            html += "<td class=\"question-validate\">";
                            html += "<i class=\"ion ion-ios-checkmark-empty text-success\" style=\"display: block;\"></i>";
                            html += "<i class=\"ion ion-ios-close-empty text-danger\"></i>";
                            html += "</td>";
                        }
                        else
                        {
                            html += "<td class=\"question-validate\">";
                            html += "<i class=\"ion ion-ios-checkmark-empty text-success\"></i>";
                            html += "<i class=\"ion ion-ios-close-empty text-danger\"></i>";
                            html += "</td>";
                        }
                    }
                }
                else
                {
                    html += "<td class=\"question-validate\">";
                    html += "<i class=\"ion ion-ios-checkmark-empty text-success\"></i>";
                    html += "<i class=\"ion ion-ios-close-empty text-danger\"></i>";
                    html += "</td>";
                }
                html += "</tr>";
            }

            return html + createTree(++i, edit);
        }

        public string createRolePanel(int framework)
        {
            var roles = db.Roles1.Where(x => x.IdFramework == framework);
            string rolesList = "";
            foreach (var role in roles)
            {
                rolesList +=
                    "<div class=\"checkbox\"><label><input class=\"\" type=\"checkbox\" ng-model=\"rolesModel.id" +
                    role.IdRole + "\" ng-init=\"rolesModel.id" +
                    role.IdRole + " = true\" ng-change=\"updateRoles()\">" + role.Name + "</label></div>";
            }

            return rolesList;

            //return Enumerable.Aggregate(roles, "",
            //    (current, role) =>
            //        current +
            //        ("<div class=\"checkbox\"><label><input class=\"\" type=\"checkbox\" ng-model=\"rolesModel.id" +
            //         role.IdRole + "\" ng-change=\"updateRoles()\">" + role.Name + "</label></div>"));
        }

        // GET: ConductInterview
        public ActionResult Index(int idinterview)
        {
            //if (idinterview == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            // Initialisation
            nodes = db.Nodes;
            var idquest = db.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == idinterview)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            questions = db.Questions.Where(q => q.IdQuestionnaire == idquest);
            var inodes = nodes.Where(n => n.IdQuestionnaire == idquest);
            rootnode = inodes.FirstOrDefault(i => i.IsRoot);
            //questions = db.Questions;
            list = new List<Tuple<int, int>>();
            order = new List<Tuple<int, string, string, string, string, int, bool>>();
            comments = new List<Tuple<int, int, string, string, bool>>();

            // Fill list to store nodes and their parents
            foreach (var node in inodes.Where(node => !node.IsRoot))
            {
                if ((node.IdParentNode != null) && (rootnode != null) && (node.IdParentNode != rootnode.IdNode))
                {
                    var idpn = (int)node.IdParentNode;
                    list.Add(Tuple.Create(idpn, node.IdNode));
                }
                else
                {
                    list.Add(Tuple.Create(node.IdNode, node.IdNode));
                }
            }

            // Sort list to display in right order
            list.Sort(Comparer<Tuple<int, int>>.Create((x, y) =>
            {
                var result = x.Item1.CompareTo(y.Item1);
                return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
            }));

            // Put RootNode name in ViewBag (not displayed for now)
            ViewBag.RootNode = rootnode;
            // Put the HTML string (created from recursive function) to ViewBag
            ViewBag.Tree = createTree(0, false);
            // Put the HTML string of roles checkboxes to ViewBag
            var idframework = questions.Select(q => q.Questionnaire.IdFramework).FirstOrDefault();
            ViewBag.Roles = createRolePanel(idframework);

            // Put list that orders questions to ViewBag for angular
            ViewBag.List = new JavaScriptSerializer().Serialize(order);
            // Put the list that will contain the comments to ViewBag
            ViewBag.Comments = new JavaScriptSerializer().Serialize(comments);

            var campaign = db.Campaigns.Find(db.Interviews.Find(idinterview).IdCampaign);
            ViewBag.InterviewName = db.Interviews.Find(idinterview).Name;
            ViewBag.InterviewCampaign = campaign.Name;
            ViewBag.InterviewIdCampaign = db.Interviews.Find(idinterview).IdCampaign;
            var candi = db.Interviewees.Include(i => i.Candidate).Where(i => i.IdInterview == idinterview).Select(i => i.Candidate.FirstName + " " + i.Candidate.LastName + " - " + i.Candidate.Function).ToList();
            ViewBag.Candidate = candi;
            ViewBag.CustomerImage = ImageController.GetImage("Customers", campaign.IdCustomer.ToString());
            ViewBag.CustomerName = campaign.Customer.Name;
            ViewBag.CustomerIndustry = campaign.Customer.Industry.Name;

            return View(idinterview);
        }

        [HttpPost]
        public string Save(List<CIResults> tosave, int? idinterview, List<CIComments> comments)
        {
            if (idinterview == null)
            {
                return "0";
            }
            if ((tosave != null) && (tosave.Count > 0) && (comments != null) && (comments.Count > 0))
            {
                for (var i = 0; i < tosave.Count; i++)
                {
                    var c = tosave[i];
                    var d = comments[i];
                    Answer a = null;
                    var idq = tosave[i].Item1;

                    a = db.Answers.Any(an => an.IdInterview == idinterview && an.IdQuestion == idq)
                        ? db.Answers.FirstOrDefault(an => an.IdInterview == idinterview && an.IdQuestion == idq) : new Answer();

                    a.Value = (d.Item5 ? 1 : 0); // used for isRelevant
                    a.IntervieweeAnswer = c.Item5;
                    a.Comment = d.Item3;
                    a.Description = ""; // ????
                    a.ConsultantComment = d.Item4;
                    a.IdInterview = idinterview;
                    a.IdQuestion = c.Item1;

                    if (db.Answers.Any(an => an.IdInterview == idinterview && an.IdQuestion == idq))
                    {
                        db.Entry(a).State = EntityState.Modified;
                    }
                    else
                    {
                        db.Answers.Add(a);
                    }
                }
                var idquest =
                    db.Interviews.Include(i => i.Campaign).Where(i => i.IdInterview == idinterview).Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
                var autoscore = db.Questionnaires.Where(q => q.IdQuestionnaire == idquest).Select(q => q.AutoScore).FirstOrDefault();
                if (autoscore)
                {
                    var itv = db.Interviews.Find(idinterview);
                    itv.Score = GetAutoScore(idinterview);
                }

                db.SaveChanges();

                return idinterview.ToString();
            }
            return "0";
        }

        public ActionResult Saved(int id)
        {
            var interviews = db.Interviews.Include(c => c.ApplicationUsers)
                                .Include(c => c.Candidates)
                                .SingleOrDefault(x => x.IdInterview == id);

            if (db.Interviews.Find(id) == null || interviews == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var answers = db.Answers.Where(x => x.IdInterview == id).Include(a => a.Interview).Include(a => a.Question).Include(a => a.Question.Node).OrderBy(a => a.Question.Role.IdRole);

            ViewBag.InterviewId = id;
            var interv = db.Interviews.Find(id);
            ViewBag.InterviewName = interv.Name;
            ViewBag.InterviewDate = interv.Date.ToString("MM/dd/yyyy");
            ViewBag.InterviewCampaign = db.Campaigns.Find(interv.IdCampaign).Name;
            ViewBag.InterviewIdCampaign = interv.IdCampaign;
            ViewBag.InterviewDescription = interv.Description;
            ViewBag.InterviewComment = interv.Comment;
            ViewBag.InterviewInterviewer = interviews.ApplicationUsers.FullName;
            ViewBag.InterviewInterviewee = "";
            var b = false;
            foreach (var ie in db.Interviewees.Where(x => x.IdInterview == id).Include(c => c.Candidate))
            {
                if (b) { ViewBag.InterviewInterviewee += ", "; } else { b = true; }
                ViewBag.InterviewInterviewee += ie.Candidate.FullName;
            }

            ViewBag.CustomScores = db.CustomScores.Where(cs => cs.IdInterview == id).ToList();

            // TO FIX : filter by IdInterview (id)

            var idquest = db.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == id)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            //nodes = db.Nodes;
            //var inodes = nodes.Where(n => n.IdQuestionnaire == idquest);
            //rootnode = inodes.FirstOrDefault(n => n.IsRoot);
            list = new List<Tuple<int, int>>();

            var fullnodes = db.Nodes.Where(x => x.IdQuestionnaire == idquest).ToList();
            List<Node> nodes = null;
            List<Node> pnodes = null;
            Node root = null;
            if (fullnodes.Any())
            {
                root = fullnodes.Single(r => r.IsRoot);
                if (root != null)
                {
                    nodes = fullnodes.Where(n => !n.IsRoot && n.IdParentNode != root.IdNode).ToList();
                    pnodes = fullnodes.FindAll(p => p.IdParentNode == root.IdNode).ToList();
                    ViewBag.RootNode = root.IdNode;
                }
            }
            ViewBag.FullNodes = fullnodes;
            ViewBag.Nodes = nodes;
            ViewBag.PNodes = pnodes;

            if (pnodes != null)
            {
                foreach (var node in pnodes)
                {
                    var subnodes = fullnodes.FindAll(s => s.IdParentNode == node.IdNode);
                    if (subnodes.Any())
                    {
                        foreach (var idpn in subnodes.Select(snode => (int) snode.IdParentNode))
                        {
                            list.Add(Tuple.Create(idpn, node.IdNode));
                        }
                    }
                    else
                    {
                        list.Add(Tuple.Create(node.IdNode, node.IdNode));
                    }
                }
            }
            list.Sort(Comparer<Tuple<int, int>>.Create((x, y) =>
            {
                var result = x.Item1.CompareTo(y.Item1);
                return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
            }));

            var nodeslist = new List<Tuple<string, string, double, int, string>>(); // id, name, customscore, idnode, comment
            var interscore = 0.0;
            if (db.Questionnaires.Find(idquest).AutoScore)
            {
                var nodescores = GetNodesScore(id);
                var cpt = 0;
                foreach (var ns in nodescores)
                {
                    //var value = Convert.ToInt32(Math.Floor(ns.Item2));
                    var value = ns.Item2;
                    if (ns.Item1 == rootnode.IdNode)
                    {
                        nodeslist.Add(Tuple.Create("node0", db.Nodes.Find(ns.Item1).Name, 0.0, ns.Item1, ""));
                    }
                    else
                    {
                        nodeslist.Add(ns.Item1 == ns.Item2
                            ? Tuple.Create("node1", db.Nodes.Find(ns.Item1).Name, value, ns.Item1, "")
                            : Tuple.Create("node2", db.Nodes.Find(ns.Item1).Name, value, ns.Item1, ""));
                    }
                    interscore += value;
                    cpt++;
                }
                if (cpt > 0)
                {
                    interscore = interscore / cpt;
                }
            }
            else
            {
                foreach (var n in list)
                {
                    var customscore =
                        db.CustomScores.FirstOrDefault(cs => cs.IdNode == n.Item2 && cs.IdInterview == id);
                    var value = 0.0;
                    var comment = "";
                    if (customscore != null)
                    {
                        value = customscore.Value;
                        comment = customscore.Comment;
                    }
                    if (n.Item1 == rootnode.IdNode)
                    {
                        nodeslist.Add(Tuple.Create("node0", db.Nodes.Find(n.Item2).Name, 0.0, n.Item2, ""));
                    }
                    else
                    {
                        nodeslist.Add(n.Item1 == n.Item2
                            ? Tuple.Create("node1", db.Nodes.Find(n.Item2).Name, value, n.Item2, comment)
                            : Tuple.Create("node2", db.Nodes.Find(n.Item2).Name, value, n.Item2, comment));
                    }
                }
            }
            
            ViewBag.Nodes = nodeslist;
            if (db.Questionnaires.Find(idquest).AutoScore)
            {
                var interview = db.Interviews.Find(id);
                var score = GetInterviewScore(id);
                interview.Score = Convert.ToInt32(score);
                db.Interviews.Attach(interview);
                var entry = db.Entry(interview);
                entry.Property(e => e.Score).IsModified = true;
                db.SaveChanges();
                //ViewBag.Part = db.Interviews.Find(id).Score;
                ViewBag.Part = score;
            }
            else
            {
                ViewBag.Part = GetWeightedAverage(id).ToString(CultureInfo.InvariantCulture);
            }
            ViewBag.Total = "5";

            var nodescore = (from n in pnodes
                             let cs = db.CustomScores.Include(i => i.Interview)
                                .Where(x => x.IdNode == n.IdNode && x.IdInterview == id && !x.Interview.Deleted).ToList()
                             where cs.Count > 0
                             select Tuple.Create(n.Name, Math.Round(cs.Average(x => x.Value), 2))).ToList();

            var snodescore = (from n in nodes
                              let cs = db.CustomScores.Include(i => i.Interview)
                                  .Where(x => x.IdNode == n.IdNode && x.IdInterview == id && !x.Interview.Deleted).ToList()
                              where cs.Count > 0
                              select Tuple.Create(n.Name, Math.Round(cs.Average(x => x.Value), 2))).ToList();

            nodescore.AddRange(snodescore);

            ViewBag.Nodescore = nodescore;

            return View(answers.ToList());
        }

        public ActionResult Edit(int idinterview)
        {
            //if (idinterview == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            iditv = idinterview;
            // Initialisation
            nodes = db.Nodes;
            var idquest = db.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == idinterview)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            questions = db.Questions.Where(q => q.IdQuestionnaire == idquest);
            var inodes = nodes.Where(n => n.IdQuestionnaire == idquest);
            //questions = db.Questions;
            answers = db.Answers;
            list = new List<Tuple<int, int>>();
            order = new List<Tuple<int, string, string, string, string, int, bool>>();
            comments = new List<Tuple<int, int, string, string, bool>>();
            rootnode = inodes.FirstOrDefault(i => i.IsRoot);
            // Fill list to store nodes and their parents
            foreach (var node in inodes.Where(node => !node.IsRoot))
            {
                if ((node.IdParentNode != null) && (rootnode != null) && (node.IdParentNode != rootnode.IdNode))
                {
                    var idpn = (int)node.IdParentNode;
                    list.Add(Tuple.Create(idpn, node.IdNode));
                }
                else
                {
                    list.Add(Tuple.Create(node.IdNode, node.IdNode));
                }
            }

            // Sort list to display in right order
            list.Sort(Comparer<Tuple<int, int>>.Create((x, y) =>
            {
                var result = x.Item1.CompareTo(y.Item1);
                return result == 0 ? x.Item2.CompareTo(y.Item2) : result;
            }));

            // Put RootNode name in ViewBag (not displayed for now)
            ViewBag.RootNode = rootnode;
            // Put the HTML string (created from recursive function) to ViewBag
            ViewBag.Tree = createTree(0, true);
            // Put the HTML string of roles checkboxes to ViewBag
            var idframework = questions.Select(q => q.Questionnaire.IdFramework).FirstOrDefault();
            ViewBag.Roles = createRolePanel(idframework);

            // Put list that orders questions to ViewBag for angular
            ViewBag.List = new JavaScriptSerializer().Serialize(order);
            // Put the list that will contain the comments to ViewBag
            ViewBag.Comments = new JavaScriptSerializer().Serialize(comments);

            var campaign = db.Campaigns.Find(db.Interviews.Find(idinterview).IdCampaign);
            ViewBag.InterviewName = db.Interviews.Find(idinterview).Name;
            ViewBag.InterviewCampaign = campaign.Name;
            ViewBag.InterviewIdCampaign = db.Interviews.Find(idinterview).IdCampaign;
            ViewBag.Questionnaire = db.Questionnaires.Find(idquest).Name;
            ViewBag.Candidate = db.Interviewees.Include(i => i.Candidate).Where(i => i.IdInterview == idinterview).Select(i => i.Candidate.FirstName + " " + i.Candidate.LastName + " - " + i.Candidate.Function).ToList();
            ViewBag.CustomerImage = ImageController.GetImage("Customers", campaign.IdCustomer.ToString());
            ViewBag.CustomerName = campaign.Customer.Name;
            ViewBag.CustomerIndustry = campaign.Customer.Industry.Name;

            return View("Index", idinterview);
        }

        [HttpPost]
        public ActionResult SaveCustomScores(string[] customScores)
        {
            if (customScores == null) return Json(new { message = "" });
            var idinterview = Convert.ToInt32(customScores.GetValue(2));
            var idcampaign = db.Interviews.Find(Convert.ToInt32(idinterview)).IdCampaign;
            var interviews = db.Interviews.Where(i => i.IdCampaign == idcampaign && i.Deleted == false);
            var campaignscore = 0.0;
            var cs = new CustomScore();
            var iduser = User.Identity.GetUserId();
            var iscomment = customScores.GetValue(3).ToString();
            var idnode = Convert.ToInt32(customScores.GetValue(0));
            var value = customScores.GetValue(1);
            var param = new object[3];
            param[0] = iduser;
            param[1] = idinterview;
            param[2] = idnode;
            var found = db.CustomScores.Find(param);
            if (found != null)
            {
                if (iscomment == "False" && !string.IsNullOrEmpty(value.ToString())) // Score
                {
                    found.Value = Convert.ToInt32(value);
                }
                else if (!string.IsNullOrEmpty(value.ToString())) // Comment
                {
                    found.Comment = value.ToString();
                }
            }
            else
            {
                cs.IdNode = idnode;
                cs.Id = iduser;
                cs.IdInterview = idinterview;
                if (iscomment == "False") // Score
                {
                    cs.Value = Convert.ToInt32(value);
                }
                else // Comment
                {
                    cs.Comment = value.ToString();
                }
                db.CustomScores.Add(cs);
            }
            db.SaveChanges();

            // Campaign Score
            foreach (var interview in interviews)
            {
                campaignscore += GetWeightedAverage(interview.IdInterview);
            }
            if (interviews.Any())
            {
                campaignscore = campaignscore / interviews.Count();
                db.Campaigns.Find(idcampaign).Score = Convert.ToInt32(campaignscore);
                db.SaveChanges();
            }

            return
                Json(new { average = GetWeightedAverage(idinterview), message = "Custom scores successfully saved !!!" });
        }

        public double GetWeightedAverage(int idinterview)
        {
            float result = 0;
            var cs = dbcs.CustomScores.Include(c => c.Node).Where(c => c.IdInterview == idinterview);
            if (cs.Any())
            {
                foreach (var item in cs)
                {
                    result += item.Value * (item.Node.Weight / 10);
                }
                result = result / cs.Count();
            }
            return Math.Floor(result);
        }

        [HttpPost]
        public ActionResult SaveCompletion(string[] completion)
        {
            if (completion == null) return Json(new { message = "" });
            var interview = db.Interviews.Find(Convert.ToInt32(completion[1]));
            var idcampaign = interview.IdCampaign;
            interview.Completion = Convert.ToInt32(Math.Floor(Convert.ToDouble(completion[0])));
            db.SaveChanges();
            if (interview.Completion == 100)
            {
                var interviews = db.Interviews.Where(i => i.IdCampaign == idcampaign);
                var end = false;
                foreach (var i in interviews)
                {
                    end = i.Completion == 100;
                }
                if (end)
                {
                    var campaign = db.Campaigns.Find(idcampaign);
                    campaign.Status = CStatus.Completed;
                    db.SaveChanges();
                }
            }
            SaveCoverageCompletion(idcampaign.GetValueOrDefault());
            return Json(new { message = "Completion saved !!!" });
        }

        public void SaveCoverageCompletion(int? idcampaign)
        {
            if (idcampaign == null) return;
            var campaign = db.Campaigns.Find(idcampaign);
            var interviews = db.Interviews.Where(i => i.IdCampaign == idcampaign && i.Deleted == false);
            var cpt = 0;
            var result = 0;
            var nbquestions = db.Questions.Count(q => q.IdQuestionnaire == campaign.IdQuestionnaire && q.IsRelevant);
            var nbanswers = db.Answers.Include(a => a.Interview).Include(a => a.Question)
                                .Where(
                                    a =>
                                        a.Interview.IdCampaign == idcampaign &&
                                        a.Interview.Deleted == false &&
                                        a.IntervieweeAnswer != null &&
                                        a.Question.IsRelevant).Select(a => a.IdQuestion).Distinct().Count();

            if (nbquestions > 0)
            {
                campaign.Coverage = Convert.ToInt32((nbanswers * 100) / nbquestions);
            }
            foreach (var item in interviews)
            {
                result += item.Completion;
                cpt++;
            }
            if (cpt > 0)
            {
                campaign.Completion = Convert.ToInt32(result / cpt);
            }
            db.SaveChanges();
        }

        public double GetInterviewScore(int idinterview)
        {
            var idquest = db.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == idinterview)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            var fullnodes = db.Nodes.Where(x => x.IdQuestionnaire == idquest).ToList();
            List<Node> nodes = null;
            Node root;
            var score = 0.0;
            if (fullnodes.Any())
            {
                root = fullnodes.Single(r => r.IsRoot);
                if (root != null)
                {
                    nodes = fullnodes.FindAll(p => p.IdParentNode == root.IdNode).ToList();
                }
            }
            if (nodes == null) return score;
            score = nodes.Sum(node => GetPNodeScore(node.IdNode, idinterview));
            return score / nodes.Count;
        }

        public double GetSubNodeScore(int? idinterview, int idnode)
        {
            var dbs = new ApplicationDbContext();
            var scores = dbs.Answers.Include(a => a.Question)
                .Where(a => a.IdInterview == idinterview && a.Question.IdNode == idnode)
                .Select(a => a.IntervieweeAnswer);
            var nodescore = 0.0;
            var i = 0;
            foreach (var score in scores)
            {
                nodescore += Convert.ToInt32(score);
                i++;
            }
            if (i > 0)
            {
                return nodescore / i;
            }
            return nodescore;
        }

        public List<Tuple<int, double>> GetNodesScore(int? idinterview)
        {
            var idquest = db.Interviews.Include(i => i.Campaign)
                    .Where(i => i.IdInterview == idinterview)
                    .Select(i => i.Campaign.IdQuestionnaire).FirstOrDefault();
            var tmp = db.Nodes.Where(n => n.IdQuestionnaire == idquest);
            rootnode = tmp.FirstOrDefault(i => i.IsRoot);
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

        public double GetPNodeScore(int idpnode, int idinterview)
        {
            var dbns = new ApplicationDbContext();
            var snodes = db.Nodes.Where(n => n.IdParentNode == idpnode).ToList();
            var pnodescore = 0.0;

            if (snodes.Any())
            {
                pnodescore = snodes.Aggregate(pnodescore, (current, snode) => current + dbns.CustomScores.Where(cs => cs.IdNode == snode.IdNode && cs.IdInterview == idinterview).Select(pn => pn.Value).FirstOrDefault());
                pnodescore = pnodescore/snodes.Count;
            }
            return Math.Round(pnodescore, 2);
        }

        //public bool SetPNodeScore(int idpnode)
        //{
            
        //}

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

        public class CIComments
        {
            public int Item1 { get; set; } //idquestion
            public int Item2 { get; set; } //idrole
            public string Item3 { get; set; } //comment
            public string Item4 { get; set; } //consultantcomment
            public bool Item5 { get; set; } //isrelevant
        }

        public class CIResults
        {
            public int Item1 { get; set; } //idquestion
            //public string Item2 { get; set; } //name
            //public string Item3 { get; set; } //description
            //public string Item4 { get; set; } //answertype
            public string Item5 { get; set; } //answer
            //public int Item6 { get; set; } //idrole
            //public bool Item7 { get; set; } //multi
        }

        [Authorize(Roles = "Admin,Manager,Consultant")]
        public ActionResult Pdf(int? id, bool isfirm)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var interview = db.Interviews.Where(f => f.IdInterview == id).Include(c => c.ApplicationUsers).Include(c => c.Campaign).FirstOrDefault();

            ViewBag.Nodes = db.Nodes.Where(x => x.IsRoot == false).ToList();
            ViewBag.Questions = db.Questions.Where(w => w.IdQuestionnaire == interview.Campaign.IdQuestionnaire).OrderBy(x => x.IdNode).ToList();
            ViewBag.FirmImage = ImageController.GetImage("Firms", interview.Campaign.ApplicationUsers.IdFirm.ToString());
            ViewBag.CustomerImage = ImageController.GetImage("Customers", interview.Campaign.IdCustomer.ToString());
            ViewBag.ConsultantImage = ImageController.GetImage("Users", interview.ApplicationUsers.Id.ToString());
            ViewBag.OverallScore = GetWeightedAverage((int)id).ToString();

            ViewBag.InterviewInterviewee = "";
            var b = false;
            foreach (var ie in db.Interviewees.Where(x => x.IdInterview == id).Include(c => c.Candidate))
            {
                if (b) { ViewBag.InterviewInterviewee += ", "; } else { b = true; }
                ViewBag.InterviewInterviewee += ie.Candidate.FullName;
            }

            ViewBag.IsFirm = isfirm;

            return View(interview);
        }

        [Authorize(Roles = "Admin,Manager,Consultant")]
        public ActionResult DownloadPDF(int? id, bool isfirm)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Dictionary<string, string> cookieCollection = new Dictionary<string, string>();
            foreach (var key in Request.Cookies.AllKeys)
            {
                cookieCollection.Add(key, Request.Cookies.Get(key).Value);
            }

            var interview = db.Interviews.Find(id);

            return new Rotativa.ActionAsPdf("Pdf", new { id = id, isfirm = isfirm }) { FileName = "Interview_" + interview.Name + (isfirm ? "_Firm.pdf" : "_Customer.pdf"), Cookies = cookieCollection };
        }
    }
}
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
using System.Web.Script.Serialization;

namespace Octant.Controllers
{
    [Authorize]
    public class CalendarController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        private List<string> eventscolors = new List<string> { "#3A87AD", "#762E7E", "#F39C12", "##00C0EF", "#DD4B39" };

        // GET: Calendar
        public ActionResult Index()
        {
            ApplicationUser actualuser = db.Users.Find(User.Identity.GetUserId());
            List<Interview.Interview> interviews = null;
            if (!User.IsInRole("Admin"))
            {
                if (User.IsInRole("Manager"))
                {
                    interviews = db.Interviews.Where(x => x.Deleted == false
                                                        && x.Campaign.Deleted == false
                                                        && x.Campaign.ApplicationUsers.IdFirm == actualuser.IdFirm).ToList();
                }
                else if (User.IsInRole("Consultant"))
                {
                    interviews =
                        db.Interviews.Where(
                            i => i.Deleted == false && i.Campaign.Deleted == false && i.Id == actualuser.Id).ToList();
                }
            }
            else
                interviews = db.Interviews.Where(x => x.Deleted == false).Where(x => x.Campaign.Deleted == false).OrderBy(x => x.IdCampaign).ToList();
            List<EventsSource> events = new List<EventsSource>();
            int actualidcampaign = (interviews != null && interviews.Count > 0 ? (int)interviews.First().IdCampaign : 0);
            int colorscounter = 0;
            foreach (Interview.Interview i in interviews)
            {
                if (i == null) return View("Index", "Home");
                if (i.IdCampaign != actualidcampaign)
                {
                    colorscounter = (colorscounter + 1) % (eventscolors.Count);
                    actualidcampaign = (int)i.IdCampaign;
                }
                events.Add(new EventsSource(i.Name, true, i.Date.ToString("yyyy-MM-dd"), i.IdInterview, eventscolors[colorscounter]));
            }
            ViewBag.Interviews = new JavaScriptSerializer().Serialize(events);

            //List<Interview.Campaign> campaigns = db.Campaigns.Where(x => x.Deleted == false).Where(x => x.ApplicationUsers.IdFirm == actualuser.IdFirm).ToList();
            //if (User.IsInRole("Admin"))
            //    campaigns = db.Campaigns.Where(x => x.Deleted == false).ToList();
            //ViewBag.campaign = new SelectList(campaigns, "IdCampaign", "Name");

            //List<ApplicationUser> consultants = db.Users.Where(x => x.IdFirm == actualuser.IdFirm).ToList();
            //if (User.IsInRole("Admin"))
            //    consultants = db.Users.ToList();
            //ViewBag.consultant = new SelectList(consultants, "Id", "FullName");

            return View();
        }


        [HttpPost]
        public ActionResult GetInterview(int idInterview)
        {
            try
            {
                Interview.Interview i = db.Interviews.Include(x => x.Campaign).Include(x => x.Candidates).Where(x => x.IdInterview == idInterview).FirstOrDefault();
                if (i != null)
                {
                    return Json(new
                    {
                        msg = "success",
                        name = i.Name,
                        consultant = i.ApplicationUsers.FullName,
                        candidats = string.Join(", ", db.Interviewees.Include(x => x.Candidate).Where(x => x.IdInterview == idInterview).Select(x => x.Candidate.FirstName + " " + x.Candidate.LastName).ToList()),
                        date = i.Date.ToString("dd/MM/yyyy"),
                        campaign = i.Campaign.Name,
                        link = Url.Action("Details", "Campaigns", new { id = i.IdCampaign })
                    });
                }
                return Json(new { msg = "error" });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Exception : " + e.Message);
                return Json(new { msg = "error" });
            }
        }


        public class EventsSource
        {
            public string title { get; set; }
            public bool allDay { get; set; }
            public string start { get; set; }
            public int idInterview { get; set; }
            public string backgroundColor { get; set; }
            public string borderColor { get; set; }

            public EventsSource(string t, bool a, string s, int i, string c)
            {
                title = t;
                allDay = a;
                start = s;
                idInterview = i;
                backgroundColor = c;
                borderColor = c;
            }
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

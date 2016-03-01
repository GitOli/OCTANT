using System.Collections.Generic;
using IdentitySample.Models;
using System.Web.Mvc;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Net;
using System.Web;
using System;
using Interview;

namespace IdentitySample.Controllers
{
    public class HomeController : Controller
    {
        public readonly ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public ActionResult Index()
        {
            IQueryable<Campaign> campaigns = null;
            var idfirm = db.Users.Find(User.Identity.GetUserId()).IdFirm;
            if ((idfirm != null) && !(User.IsInRole("Admin")))
            {
                if (User.IsInRole("Manager"))
                {
                    campaigns = db.Campaigns.Include(c => c.ApplicationUsers).Include(c => c.Customer).Include(c => c.Questionnaire).Where(c => c.Deleted == false).Where(c => c.ApplicationUsers.IdFirm == idfirm);
                }
                else if (User.IsInRole("Consultant"))
                {
                    var iduser = User.Identity.GetUserId();
                    campaigns =
                        db.ConsultantCampaigns.Include(cc => cc.Campaign.Customer)
                            .Include(cc => cc.Campaign.Questionnaire)
                            .Where(cc => cc.Campaign.Deleted == false && cc.ApplicationUsers.Id == iduser)
                            .Select(cc => cc.Campaign);
                }
            }
            else
            {
                campaigns = db.Campaigns.Include(c => c.ApplicationUsers).Include(c => c.Customer).Include(c => c.Questionnaire).Where(c => c.Deleted == false);
            }

            var interviews = new List<Interview.Interview>();
            foreach (Interview.Interview i in db.Interviews)
            {
                interviews.Add(i);
            }
            ViewBag.Interviews = interviews;
            if (User.IsInRole("Admin"))
            {
                ViewBag.Campaign1 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion == 0)).Count();
                ViewBag.Campaign2 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion > 0) && (x.Completion < 100)).Count();
                ViewBag.Campaign3 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion == 100)).Count();
                ViewBag.Customers = new List<Interview.Customer>(db.Customers.ToList());
            }
            else
            {
                ViewBag.Campaign1 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion == 0) && (x.ApplicationUsers.IdFirm == idfirm)).Count();
                ViewBag.Campaign2 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion > 0) && (x.Completion < 100) && (x.ApplicationUsers.IdFirm == idfirm)).Count();
                ViewBag.Campaign3 = db.Campaigns.Where(x => (x.Deleted == false) && (x.Completion == 100) && (x.ApplicationUsers.IdFirm == idfirm)).Count();
                ViewBag.Customers = new List<Interview.Customer>(db.Customers.Where(c => c.IdFirm == idfirm).ToList());
            }
            
            return View(campaigns);
        }

        [Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

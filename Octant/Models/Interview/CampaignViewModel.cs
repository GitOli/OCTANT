using System;
using System.ComponentModel.DataAnnotations;
using Octant.Models.Framework;
using IdentitySample.Models;
using Interview;

namespace Octant.Models.Interview
{
    public class CampaignViewModel
    {
        public int IdCampaign { get; set; }
        public string Name { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public CStatus Status { get; set; }

        //[UIHint("Customer")]
        public CustomerViewModel Customer { get; set; }
        public int? IdCustomer { get; set; }

        //[UIHint("Framework")]
        public FrameworkViewModel Framework { get; set; }
        public virtual int? IdFramework { get; set; }

        public string Id { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IdentitySample.Models;
using Interview;

namespace Octant.Models.Interview
{
    class CampaignScore
    {
        [Key, Column(Order = 0)]
        [Required, Display(Name = "Manager")]
        public virtual string Id
        {
            get;
            set;
        }

        [Key, Column(Order = 1)]
        [Required, Display(Name = "Campaign")]
        public virtual int IdCampaign
        {
            get;
            set;
        }

        public virtual int Value
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }

        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public Campaign Campaign { get; set; }
    }
}

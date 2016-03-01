using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview
{
    public class CandidateCampaign
    {
        [Key, Column(Order = 0)]
        public virtual int IdCandidate
        {
            get;
            set;
        }
        [Key, Column(Order = 1)]
        public virtual int IdCampaign
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }

        [ForeignKey("IdGroup")]
        public Group Group { get; set; }
        [Required, Display(Name = "Group")]
        public int? IdGroup { get; set; }

        public virtual Candidate Candidate { get; set; }
        public virtual Campaign Campaign { get; set; }
    }
}


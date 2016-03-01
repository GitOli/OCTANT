using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octant.Models.Interview
{
    public class Firm
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int IdFirm
        {
            get;
            set;
        }
        [Required, Display(Name = "Firm")]
        public virtual string Name
        {
            get;
            set;
        }

        public virtual string Description
        {
            get;
            set;
        }

        public virtual string Comment
        {
            get;
            set;
        }
        public virtual bool Deleted { get; set; }
    }
}

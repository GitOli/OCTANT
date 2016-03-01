using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Octant.Models.Interview
{
    public class InterviewViewModel
    {
        public int IdInterview
        {
            get;
            set;
        }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date
        {
            get;
            set;
        }
        public string Name
        {
            get;
            set;
        }
        public string Comment
        {
            get;
            set;
        }
        public bool Canceled
        {
            get;
            set;
        }
        public bool Suspended
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public int Completion
        {
            get;
            set;
        }
        public int IdCampaign { get; set; }
        public int IdQuestionnaire { get; set; }
        public string Id { get; set; }
    }
}

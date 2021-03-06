﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using Foolproof;
using IdentitySample.Models;

namespace Interview
{
	using Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

    public class Campaign
    {
        [Key]
        [ScaffoldColumn(false)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual int IdCampaign { get; set; }

        [Required, Display(Name = "Campaign")]
        public virtual string Name { get; set; }

        [Display(Name = "Start Date"), DataType(DataType.Date)]
        public virtual DateTime? StartDate { get; set; }

        [Display(Name = "End Date"), DataType(DataType.Date)]
        public virtual DateTime? EndDate { get; set; }

        public virtual string Description { get; set; }

        public virtual CStatus Status { get; set; }

        public virtual string Comment { get; set; }
        [DefaultValue(0)]
        public virtual int? Coverage { get; set; }
        [DefaultValue(0)]
        public virtual int? Completion { get; set; }

        [DefaultValue(false)]
        public virtual bool Deleted { get; set; }
        [DefaultValue(null)]
        public virtual int? Score
        {
            get;
            set;
        }

        [Display(Name = "Manager")]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUsers { get; set; }

        [Required, Display(Name = "Customer")]
        public virtual int IdCustomer { get; set; }
        //[UIHint("CustomerEditor")]
        public virtual Customer Customer { get; set; }

        [Required, Display(Name = "Questionnaire")]
        public int IdQuestionnaire { get; set; }
        [ForeignKey("IdQuestionnaire")]
        public Questionnaire Questionnaire { get; set; }
        public ICollection<Candidate> Candidates { get; set; }
        public ICollection<ApplicationUser> ConsultantUsers { get; set; }    
	}
}


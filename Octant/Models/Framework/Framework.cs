﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentitySample.Models;

namespace Framework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Framework
	{
        [Key]
		public virtual int IdFramework
		{
			get;
			set;
		}
        [Display(Name = "Framework")]
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

		public virtual FWStatus Status
		{
			get;
			set;
		}

        [Display(Name = "Expert")]
        public string Id { get; set; }
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUsers { get; set; }
        [Required, Display(Name = "Standard")]
        public int? IdStandard { get; set; }
        public Standard Standard { get; set; }
	}
}

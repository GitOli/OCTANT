﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentitySample.Models;

namespace Interview
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class ConsultantCampaign
	{
        [Key, Column(Order = 0)]
		public virtual string Id
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
        [ForeignKey("Id")]
        public virtual ApplicationUser ApplicationUsers { get; set; }
        public Campaign Campaign { get; set; }
	}
}

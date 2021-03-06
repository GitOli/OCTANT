﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Interview
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Industry
	{
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int IdIndustry
		{
			get;
			set;
		}
        [Required, Display(Name = "Industry")]
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

        public virtual ICollection<Customer> Customers { get; set; }
	}
}


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré par un outil
//     Les modifications apportées à ce fichier seront perdues si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

using System.ComponentModel.DataAnnotations;

namespace Framework
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Role
	{
        [Key]
		public virtual int IdRole
		{
			get;
			set;
		}

		public virtual string Description
		{
			get;
			set;
		}
        [Display(Name = "Role")]
		public virtual string Name
		{
			get;
			set;
		}
        [Required, Display(Name = "Framework")]
        public int? IdFramework { get; set; }
        public Framework Framework { get; set; }
	}
}

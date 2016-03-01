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
	using Framework;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;

	public class Candidate
	{
        [Key]
        //[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public virtual int IdCandidate
		{
			get;
			set;
		}
        [Required, Display(Name = "First Name")]
		public virtual string FirstName
		{
			get;
			set;
		}
        [Required, Display(Name = "Last Name")]
		public virtual string LastName
		{
			get;
			set;
		}
        [Phone]
        [Display(Name = "Phone Number")]
		public virtual string PhoneNumber
		{
			get;
			set;
		}
        [EmailAddress]
        [Display(Name = "Email Address")]
		public virtual string EmailAddress
		{
			get;
			set;
		}
        public string FullName
        {
            get
            {
                var dspFirstName = string.IsNullOrWhiteSpace(FirstName) ? "" : FirstName;
                var dspLastName = string.IsNullOrWhiteSpace(LastName) ? "" : LastName;

                return string.Format("{0} {1}", dspFirstName, dspLastName);
            }
        }

        public virtual string Function { get; set; }

        public string FullNameAndFunction
        {
            get
            {
                var dspFirstName = string.IsNullOrWhiteSpace(FirstName) ? "" : FirstName;
                var dspLastName = string.IsNullOrWhiteSpace(LastName) ? "" : LastName;
                var dspFunction = string.IsNullOrWhiteSpace(Function) ? "" : " - " + Function;

                return string.Format("{0} {1} {2}", dspFirstName, dspLastName, dspFunction);
            }
        }

        public virtual string Comment { get; set; }

        [ForeignKey("IdCustomer")]
        public Customer Customer { get; set; }
        [Required, Display(Name = "Customer")]
        public int? IdCustomer { get; set; }

        //[ForeignKey("IdGroup")]
        //public Group Group { get; set; }
        //[Required, Display(Name = "Group")]
        //public int? IdGroup { get; set; }

        //[ForeignKey("IdRole")]
        //public Role Role { get; set; }
        //[Required, Display(Name = "Role")]
        //public int? IdRole { get; set; }
	}
}

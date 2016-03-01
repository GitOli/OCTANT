using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Octant.Models;
using Octant.Models.Interview;

namespace IdentitySample.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        [Display(Name = "RoleName")]
        public string Name { get; set; }
        public string Description { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "E-mail")]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public bool IsActive { get; set; }
        public virtual Firm Firm { get; set; }
        [Display(Name = "Firm")]
        public virtual int? IdFirm { get; set; }

        public IEnumerable<SelectListItem> RolesList { get; set; }
        public IEnumerable<SelectListItem> FirmsList { get; set; }
    }
}
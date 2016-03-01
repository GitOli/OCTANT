using System.ComponentModel.DataAnnotations.Schema;
using Interview;
using Octant.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Octant.Models.Interview;

namespace IdentitySample.Models
{
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> 
            GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }

        [Display(Name = "First Name")]
        public virtual string FirstName
        {
            get;
            set;
        }
        [Display(Name = "Last Name")]
        public virtual string LastName
        {
            get;
            set;
        }
        [Display(Name = "Is Active")]
        public virtual bool IsActive
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
        public virtual Firm Firm { get; set; }
        [Display(Name = "Firm")]
        public virtual int? IdFirm { get; set; }
    }

    public class ApplicationRole : IdentityRole
    {
        public ApplicationRole() : base() { }
        public ApplicationRole(string name) : base(name) { }
        public string Description { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("OctantDB", throwIfV1Schema: false)
        {
        }

        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<IdentityUserLogin>()
        //        .HasKey(x => new { x.LoginProvider, x.ProviderKey, x.UserId });

        //    modelBuilder.Entity<IdentityUserRole>()
        //        .HasKey(x => new { x.UserId, x.RoleId });

        //    modelBuilder.Entity<Candidate>()
        //         .Property(x => x.IdCandidate)
        //         .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

        //    modelBuilder.Entity<CandidateCampaign>()
        //         .HasKey(x => new { x.IdCandidate, x.IdCampaign })
        //         .HasRequired(b => b.Candidate);
        //}
        public DbSet<Interview.Answer> Answers { get; set; }

        public DbSet<Interview.Interview> Interviews { get; set; }

        public DbSet<Framework.Question> Questions { get; set; }

        public DbSet<Interview.Campaign> Campaigns { get; set; }

        public DbSet<Interview.Customer> Customers { get; set; }

        public DbSet<Interview.Candidate> Candidates { get; set; }

        public DbSet<Interview.Group> Groups { get; set; }

        public DbSet<CandidateCampaign> CandidateCampaigns { get; set; }

        public DbSet<Interview.ConsultantCampaign> ConsultantCampaigns { get; set; }

        public DbSet<Interview.Industry> Industries { get; set; }

        public DbSet<Framework.Questionnaire> Questionnaires { get; set; }

        public DbSet<Interview.Interviewee> Interviewees { get; set; }

        public System.Data.Entity.DbSet<Octant.Models.Interview.Firm> Firms { get; set; }

        public System.Data.Entity.DbSet<Framework.CustomScore> CustomScores { get; set; }

        public System.Data.Entity.DbSet<Framework.Node> Nodes { get; set; }

        public System.Data.Entity.DbSet<Framework.Framework> Frameworks { get; set; }

        public System.Data.Entity.DbSet<Framework.Standard> Standards { get; set; }

        public System.Data.Entity.DbSet<Framework.Perspective> Perspectives { get; set; }

        public System.Data.Entity.DbSet<Framework.Proposition> Propositions { get; set; }

        public System.Data.Entity.DbSet<Framework.Role> Roles1 { get; set; }

        public System.Data.Entity.DbSet<IdentitySample.Models.EditUserViewModel> EditUserViewModels { get; set; }
    }
}
using System.Web;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Octant.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "IdentitySample.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
            //var roleStore = new RoleStore<IdentityRole>(context);
            //var roleManager = new RoleManager<IdentityRole>(roleStore);
            ////var userStore = new UserStore<ApplicationUser>(context);
            ////var userManager = new UserManager<ApplicationUser>(userStore);
            //var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(context));
            //var user = new ApplicationUser { UserName = "admin@octant.com", Email = "admin@octant.com" };

            //userManager.Create(user, "Admin@123456");
            //roleManager.Create(new IdentityRole { Name = "Admin" });
            //userManager.AddToRole(user.Id, "Admin");
        }
    }
}

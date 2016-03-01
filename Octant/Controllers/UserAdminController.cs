using Octant.Controllers;
using Octant.Models;
using Octant.Migrations;
using System.Security.Claims;
using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Interview;

namespace IdentitySample.Controllers
{
    [Authorize(Roles = "Admin, Manager")]
    public class UsersAdminController : Controller
    {
        public UsersAdminController()
        {
        }

        public UsersAdminController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }
        private ApplicationDbContext db = new ApplicationDbContext();
        private ApplicationUserManager _userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private ApplicationRoleManager _roleManager;
        public ApplicationRoleManager RoleManager
        {
            get
            {
                return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            }
            private set
            {
                _roleManager = value;
            }
        }

        //
        // GET: /Users/
        public async Task<ActionResult> Index()
        {
            ApplicationUser actualuser = db.Users.Find(User.Identity.GetUserId());
            List<ApplicationUser> users = new List<ApplicationUser>();
            if (User.IsInRole("Admin"))
            {
                users = await db.Users.Include(c => c.Firm).ToListAsync();
            }
            else
            {
                users = await db.Users.Where(c => c.IdFirm == actualuser.IdFirm).Include(c => c.Firm)
                    .Where(z => z.Roles.Select(y => y.RoleId)
                        .Contains("1f468580-90bc-4e5b-b959-73082975919e") ||
                                z.Roles.Select(y => y.RoleId)
                                    .Contains("1ede4f9a-6f89-4a52-b81d-9ab127beaccf")).ToListAsync();
            }

            var listUsers = new List<Tuple<ApplicationUser, string>>();
            foreach (var item in users)
            {
                var image = ImageController.GetImage("Users", item.Id);
                listUsers.Add(Tuple.Create(item, image));
            }
            return View(listUsers);
        }

        //
        // GET: /Users/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);

            ViewBag.RoleNames = await UserManager.GetRolesAsync(user.Id);

            return View(user);
        }

        //
        // GET: /Users/Create
        public async Task<ActionResult> Create()
        {
            var defaultRole = RoleManager.FindByName("Consultant");
            if (User.IsInRole("Admin"))
            {
                var roles = await RoleManager.Roles.ToListAsync();
                ViewBag.RoleId = new SelectList(roles, "Name", "Name", defaultRole.Name);
            }
            else
            {
                var roles = await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync();
                ViewBag.RoleId = new SelectList(roles, "Name", "Name", defaultRole.Name);
            }
            ViewBag.IdFirm = new SelectList(db.Firms, "IdFirm", "Name");
            return View();
        }

        //
        // POST: /Users/Create
        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel userViewModel, HttpPostedFileBase fileBase, params string[] selectedRoles)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = userViewModel.Email,
                    Email = userViewModel.Email,
                    FirstName = userViewModel.FirstName,
                    LastName = userViewModel.LastName,
                    IsActive = userViewModel.IsActive,
                    IdFirm = userViewModel.IdFirm
                };

                ViewBag.IdFirm = new SelectList(db.Firms, "IdFirm", "Name", userViewModel.IdFirm);
                var adminresult = await UserManager.CreateAsync(user, userViewModel.Password);
                //Add User to the selected Roles 
                if (adminresult.Succeeded)
                {
                    if (selectedRoles != null)
                    {
                        var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles);
                         
                        if (!result.Succeeded)
                        {
                            ModelState.AddModelError("", result.Errors.First());
                            if (User.IsInRole("Admin"))
                            {
                                ViewBag.RoleId = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);
                            }
                            else
                            {
                                ViewBag.RoleId = new SelectList(await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name", selectedRoles);
                            }
                            
                            return View();
                        }

                        if (fileBase != null)
                        {
                            var imgctrl = new ImageController();
                            imgctrl.SetImage(fileBase, "Users", user.Id);
                        }
                        ViewBag.ImgTitle = "Upload user profile photo";
                    }
                }
                else
                {
                    ModelState.AddModelError("", adminresult.Errors.First());
                    if (User.IsInRole("Admin"))
                    {
                        ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);
                    }
                    else
                    {
                        ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name", selectedRoles);
                    }
                    return View();
                }
                return RedirectToAction("Index");
            }

            ViewBag.IdFirm = new SelectList(db.Firms, "IdFirm", "Name", userViewModel.IdFirm);
            if (User.IsInRole("Admin"))
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);
            }
            else
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name", selectedRoles);
            }
            return View();
        }

        //
        // GET: /Users/Edit/1
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            var selectedRole = await UserManager.GetRolesAsync(user.Id);
            if (User.IsInRole("Admin"))
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name", selectedRole);
            }
            else
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name", selectedRole);
            }

            var userFirms = await db.Firms.FindAsync(user.IdFirm);
            ViewBag.IdFirm = new SelectList(db.Firms, "IdFirm", "Name", userFirms.IdFirm);

            var image = ImageController.GetImage("Users", user.Id);
            if (image != "")
            {
                ViewData["image"] = image;
            }
            else
            {
                ViewData["image"] = ImageController.GetImage("Users", "Default");
            }

            ViewBag.FirmName = user.Firm.Name;

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                IdFirm = user.IdFirm,
                FirmsList = new SelectList(db.Firms, "IdFirm", "Name")
            });
        }

        //
        // POST: /Users/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Email,Id,FirstName,LastName,IsActive,IdFirm")] EditUserViewModel editUser, HttpPostedFileBase fileBase, params string[] selectedRoles)
        {
            var user = await UserManager.FindByIdAsync(editUser.Id);
            if (user == null)
            {
                return HttpNotFound();
            }

            user.FirstName = editUser.FirstName;
            user.LastName = editUser.LastName;
            user.IsActive = editUser.IsActive;
            user.IdFirm = editUser.IdFirm;

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            selectedRoles = selectedRoles ?? new string[] { };

            if (ModelState.IsValid)
            {
                var result = await UserManager.AddToRolesAsync(user.Id, selectedRoles.Except(userRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(selectedRoles).ToArray<string>());

                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }

                if (fileBase != null)
                {
                    var imgctrl = new ImageController();
                    imgctrl.SetImage(fileBase, "Users", user.Id);
                }

                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Something failed.");
            var userFirms = await db.Firms.FindAsync(user.IdFirm);
            ViewBag.IdFirm = new SelectList(db.Firms, "IdFirm", "Name", userFirms.IdFirm);
            ViewBag.FirmName = userFirms.Name;
            var image = ImageController.GetImage("Users", user.Id);

            if (image != "")
            {
                ViewData["image"] = image;
            }
            else
            {
                ViewData["image"] = ImageController.GetImage("Users", "Default");
            }

            if (User.IsInRole("Admin"))
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name", selectedRoles);
            }
            else
            {
                ViewBag.RoleId = new MultiSelectList(await RoleManager.Roles.Where(x => x.Name != "Admin").ToListAsync(), "Name", "Name", selectedRoles);
            }

            return View(new EditUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                IdFirm = user.IdFirm,
                FirmsList = new SelectList(db.Firms, "IdFirm", "Name")
            });
        }

        //
        // GET: /Users/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        //
        // POST: /Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (ModelState.IsValid)
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    return HttpNotFound();
                }
                var result = await UserManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    return View();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}

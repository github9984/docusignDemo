using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using docusignDemo.Models;
using Microsoft.AspNet.Identity.Owin;

namespace docusignDemo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext _db = new ApplicationDbContext();

        private ApplicationRoleManager _roleManager;

        private ApplicationUserManager _userManager;

        public UsersController()
        {
        }

        public UsersController(ApplicationUserManager userManager, ApplicationRoleManager roleManager)
        {
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
            private set { _userManager = value; }
        }

        public ApplicationRoleManager RoleManager
        {
            get { return _roleManager ?? HttpContext.GetOwinContext().Get<ApplicationRoleManager>(); }
            private set { _roleManager = value; }
        }


        public async Task<ActionResult> Index()
        {
            //do not allow edit - Administrator account
            return
                View(
                    (await UserManager.Users.ToListAsync()).Where(
                            x => (x.FirstName != "Administrator") && (x.LastName != "Administrator"))
                        .OrderBy(x => x.FirstName));
        }


        public async Task<ActionResult> Create()
        {
            var viewModel = new RegisterViewModel
            {
                UserRoles = await GetUserRolesChecklistItems()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<ActionResult> Create(RegisterViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var newUser = new ApplicationUser
                {
                    UserName = viewModel.Email,
                    Email = viewModel.Email,
                    FirstName = viewModel.FirstName,
                    LastName = viewModel.LastName
                };
                var createUser = await UserManager.CreateAsync(newUser, viewModel.Password);

                if (createUser.Succeeded)
                {
                    // check if any User Roles were selected.
                    if (viewModel.UserRoles.All(x => x.IsChecked != true)) return RedirectToAction("Index");
                    // Add user to Roles.
                    var selectedRoles = (from ur in viewModel.UserRoles
                        where ur.IsChecked
                        select ur.RoleName).ToArray();

                    var addToRoles = await UserManager.AddToRolesAsync(newUser.Id, selectedRoles);

                    if (addToRoles.Succeeded) return RedirectToAction("Index");
                }

                ModelState.AddModelError("", createUser.Errors.First());
                viewModel.UserRoles = await GetUserRolesChecklistItems();
                return View(viewModel);
            }
            viewModel.UserRoles = await GetUserRolesChecklistItems();

            return View(viewModel);
        }

        /// <summary>
        /// Gets the user roles in a checklist.
        /// </summary>
        /// <returns> List of UserRolesChecklistItems </returns>
        private async Task<List<UserRolesChecklistItem>> GetUserRolesChecklistItems()
        {
            var userRolesChecklistItems = from r in await RoleManager.Roles.ToListAsync()
                select new UserRolesChecklistItem
                {
                    IsChecked = false,
                    RoleName = r.Name
                };
            return new List<UserRolesChecklistItem>(userRolesChecklistItems);
        }

        private async Task<List<UserRolesChecklistItem>> GetUserRolesChecklistItems(IEnumerable<string> roles)
        {
            var userRolesChecklistItems = from r in await RoleManager.Roles.ToListAsync()
                select new UserRolesChecklistItem
                {
                    IsChecked = roles.Contains(r.Name) ? true : false,
                    RoleName = r.Name
                };

            return new List<UserRolesChecklistItem>(userRolesChecklistItems);
        }

        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var userRoles = await UserManager.GetRolesAsync(user.Id);

            var viewModel = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                //UserRoles = new SelectList(await RoleManager.Roles.ToListAsync(), "Name", "Name"),
                UserRoles = await GetUserRolesChecklistItems(userRoles),
                //SelectedUserRole = userRoles.FirstOrDefault(),
                LockUser = user.LockoutEndDateUtc > DateTime.UtcNow ? true : false,
                LockoutEndDateUtc = user.LockoutEndDateUtc
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindByIdAsync(viewModel.Id);
                if (user == null)
                    return HttpNotFound();

                user.UserName = viewModel.Email;
                user.Email = viewModel.Email;
                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;

                if (viewModel.LockUser)
                    user.LockoutEndDateUtc = new DateTime(2999, 1, 1);
                else
                    user.LockoutEndDateUtc = null;


                //update user details
                var result = await UserManager.UpdateAsync(user);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    viewModel.UserRoles = await GetUserRolesChecklistItems();

                    return View(viewModel);
                }


                var userRoles = await UserManager.GetRolesAsync(user.Id);
                //var selectedUserRole = new List<string> { viewModel.SelectedUserRole };

                var selectedUserRoles = from ur in viewModel.UserRoles
                    where ur.IsChecked == true
                    select ur.RoleName;

                var enumerable = selectedUserRoles as IList<string> ?? selectedUserRoles.ToList();

                //add to roles
                result = await UserManager.AddToRolesAsync(user.Id, enumerable.Except(userRoles).ToArray());
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", result.Errors.First());
                    viewModel.UserRoles = await GetUserRolesChecklistItems();

                    return View(viewModel);
                }

                //remove from roles
                result = await UserManager.RemoveFromRolesAsync(user.Id, userRoles.Except(enumerable).ToArray());
                if (result.Succeeded) return RedirectToAction("Index");

                ModelState.AddModelError("", result.Errors.First());
                viewModel.UserRoles = await GetUserRolesChecklistItems();
                return View(viewModel);
            }

            viewModel.UserRoles = await GetUserRolesChecklistItems();
            return View(viewModel);
        }


        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            var viewModel = new DeleteUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserRoles = await UserManager.GetRolesAsync(user.Id)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
                return HttpNotFound();

            if (ModelState.IsValid)
            {
                var result = await UserManager.DeleteAsync(user);
                if (result.Succeeded)
                    return RedirectToAction("Index");
                else
                    ModelState.AddModelError("", result.Errors.First());
            }

            var viewModel = new DeleteUserViewModel()
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserRoles = await UserManager.GetRolesAsync(user.Id)
            };

            return View(viewModel);
        }
    }
}
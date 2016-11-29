using docusignDemo.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using WebGrease.Css.Extensions;

namespace docusignDemo.Extensions
{
    public static class GenericPrincipalExtensions
    {
        public static string GiveName(this IPrincipal user) //claims stored in IdentityModels.cs
        {
            if (user.Identity.IsAuthenticated)
            {
                var claimsIdentity = user.Identity as ClaimsIdentity;
                if (claimsIdentity == null)
                    return string.Empty;

                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.GivenName)

                        if (user.IsInRole("Admin"))
                            return claim.Value + " (admin)";
                        else
                            return claim.Value;
                }
                return string.Empty;
            }
            else
                return string.Empty;
        }

        public static string FullName(this IPrincipal user) //claims stored in IdentityModels.cs
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.Name)
                        return claim.Value;
                }
                return "";
            }
            else
                return "";
        }

        public static string Email(this IPrincipal user) //claims stored in IdentityModels.cs
        {
            if (user.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = user.Identity as ClaimsIdentity;
                foreach (var claim in claimsIdentity.Claims)
                {
                    if (claim.Type == ClaimTypes.Email)
                        return claim.Value;
                }
                return "";
            }
            else
                return "";
        }


       


        public static bool IsAdmin(ApplicationUser user)
        {
            bool retval = false;

            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var adminRole = roleManager.FindByName("admin");

            bool exist = adminRole.Users.Any(x => x.UserId.Contains(user.Id));
            if (exist)
            {
                retval = true;
            }

            return retval;
        }

        public static string GetRoleName(string roleId)
        {
            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();
            var role = roleManager.FindById(roleId);
            
            return role.Name;
        }
      

        public static List<IdentityRole> GetUserRoles(IPrincipal user)
        {
            var userManager = HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var userRoleNames = userManager.GetRoles(user.Identity.GetUserId());

            var roleManager = HttpContext.Current.GetOwinContext().Get<ApplicationRoleManager>();

            return userRoleNames.Select(roleName => roleManager.FindByName(roleName)).ToList();
        }
    }
}
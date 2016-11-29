using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using docusignDemo.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace docusignDemo.Models
{
    public class RoleViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Role Name")]
        public string Name { get; set; }


        public List<ApplicationUser> UsersInRole { get; set; }
        public int UserCountInRole { get; set; }
        public List<TemplateRole> TemplatesInRole { get; set; }
        public int TemplateCountInRole { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool LockUser { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }


        //[Required(ErrorMessage = "The User Role field is required.")]
        //public string SelectedUserRole { get; set; }


        //public IEnumerable<SelectListItem> UserRoles { get; set; }
        public List<UserRolesChecklistItem> UserRoles { get; set; }

    }

    public class DeleteUserViewModel
    {
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public IList<string> UserRoles { get; set; }

    }
    public class UserRolesChecklistItem
    {
        public string RoleName { get; set; }
        public bool IsChecked { get; set; }
    }

    //public class TemplateRolesViewModel
    //{
    //    public string TemplateId { get; set; }
    //    public string TemplateName { get; set; }

    //    public List<RoleViewModel> RolesInTemplate { get; set; }

    //}
    public class TemplateViewModel
    {
        public string TemplateId { get; set; }
        public string TemplateName { get; set; }
    }

    public class TemplateRolesViewModel
    {
        public string TemplateId { get; set; }
        [Display(Name ="Template Name")]
        public string TemplateName { get; set; }
        public List<TemplateRolesList> Roles { get; set; }
    }

    public class TemplateRolesList
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }

        public bool IsChecked { get; set; }
    }
}
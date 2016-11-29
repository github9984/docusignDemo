using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DocuSign.eSign.Api;
using docusignDemo.Data;
using docusignDemo.Data.Entities;
using docusignDemo.Services;
using docusignDemo.Models;
using Microsoft.AspNet.Identity.Owin;

namespace docusignDemo.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TemplatesController : Controller
    {
        private readonly UnitOfWork _unitOfWork;


        public TemplatesController()
        {
            _unitOfWork = new UnitOfWork();
        }


        [HttpGet]
        public ActionResult Sync()
        {
            var templateSvc = new Templates();
            var docuSignTemplates = templateSvc.GetList();
            var templates = _unitOfWork.TemplatesRepository.Get().ToList();

            var missing = docuSignTemplates.Except(templates);
            foreach (var item in missing)
            {
                var existing = templates.Find(x => x.TemplateId == item.TemplateId);
                // Add if not found.
                if (existing == null)
                {
                    _unitOfWork.TemplatesRepository.Add(item);
                }
                // Otherwise update existing template.
                else
                {
                    existing.TemplateName = item.TemplateName;
                    _unitOfWork.TemplatesRepository.Update(existing);
                }

                _unitOfWork.Save();
                if (!_unitOfWork.HasError()) continue;
                ModelState.AddModelError("", _unitOfWork.GetError());
            }

            return RedirectToAction("Index");
        }


        public ActionResult Index()
        {
            var templates = _unitOfWork.TemplatesRepository.Get();

            var viewModel = from tr in templates
                            select new TemplateViewModel
                            {
                                TemplateId = tr.TemplateId,
                                TemplateName = tr.TemplateName
                            };

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var template = _unitOfWork.TemplatesRepository.GetFirst(x => x.TemplateId == id);
            if (template == null)
                return HttpNotFound();


            var templateRoles = _unitOfWork.TemplateRolesRepository.Get(x => x.TemplateId == template.TemplateId);
            var roleManager = HttpContext.GetOwinContext().Get<ApplicationRoleManager>();
            var roles = (from r in roleManager.Roles.AsEnumerable()
                         select new TemplateRolesList
                         {
                             RoleId = r.Id,
                             RoleName = r.Name,
                             IsChecked = templateRoles.Any(x => x.RoleId == r.Id) ? true : false
                         }).ToList();


            var viewModel = new TemplateRolesViewModel
            {
                TemplateId = template.TemplateId,
                TemplateName = template.TemplateName,
                Roles = roles
            };


            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(TemplateRolesViewModel viewModel)
        {
            var templateRoles =
                _unitOfWork.TemplateRolesRepository.Get(x => x.TemplateId == viewModel.TemplateId);
            
            // Delete all existing Roles
            foreach (var templateRole in templateRoles)
            {
                _unitOfWork.TemplateRolesRepository.Delete(templateRole);
            }
            _unitOfWork.Save();
            if (_unitOfWork.HasError())
                ModelState.AddModelError("", _unitOfWork.GetError());


            // Add all Roles that are selected.
            foreach (var role in viewModel.Roles.Where(x => x.IsChecked))
            {
                var newRole = new TemplateRole
                {
                    TemplateId = viewModel.TemplateId,
                    RoleId = role.RoleId
                };

                _unitOfWork.TemplateRolesRepository.Add(newRole);
            }
            _unitOfWork.Save();
            if (_unitOfWork.HasError())
                ModelState.AddModelError("", _unitOfWork.GetError());



            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var template = _unitOfWork.TemplatesRepository.GetFirst(x => x.TemplateId == id);
            if (template == null)
                return HttpNotFound();


            return View(template);
        }

        //
        // POST: /Roles/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var template = _unitOfWork.TemplatesRepository.GetFirst(x => x.TemplateId == id);
            if (template == null)
                return HttpNotFound();

            if (!ModelState.IsValid) return View(template);

            // Remove all Roles in Template.
            var rolesInTemplate = _unitOfWork.TemplateRolesRepository.Get(x => x.TemplateId == template.TemplateId);
            foreach (var role in rolesInTemplate)
            {
                _unitOfWork.TemplateRolesRepository.Delete(role);
            }
            _unitOfWork.Save();
            if (_unitOfWork.HasError())
            {
                ModelState.AddModelError("", _unitOfWork.GetError());
                return View(template);
            }

            // Remove Template.
            _unitOfWork.TemplatesRepository.Delete(template);

            _unitOfWork.Save();
            if (!_unitOfWork.HasError()) return RedirectToAction("Index");
            ModelState.AddModelError("", _unitOfWork.GetError());
            return View(template);
        }
    }
}
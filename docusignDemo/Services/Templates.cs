using System.Collections.Generic;
using DocuSign.eSign.Api;
using DocuSign.eSign.Model;
using docusignDemo.Data.Entities;

namespace docusignDemo.Services
{
    public class Templates
    {
        private readonly TemplatesApi _templatesApi;

        public Templates()
        {
            _templatesApi = new TemplatesApi();
        }

        /// <summary>
        ///     Gets the list.
        /// </summary>
        /// <returns>List of Templates against your account</returns>
        public List<Template> GetList()
        {
            var templates = new List<Template>();
            var envTemplateResults = _templatesApi.ListTemplates(Account.AccountId);

            envTemplateResults.EnvelopeTemplates.ForEach(
                e => templates.Add(new Template {TemplateId = e.TemplateId, TemplateName = e.Name}));
            return templates;
        }

        /// <summary>
        ///     Gets the specified template identifier.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <returns>EnvelopeTemplate.</returns>
        public EnvelopeTemplate Get(string templateId)
        {
            var envTemplate = _templatesApi.Get(Account.AccountId, templateId);

            return envTemplate;
        }
    }
}
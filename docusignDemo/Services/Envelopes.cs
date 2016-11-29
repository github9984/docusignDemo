using System.Collections.Generic;
using DocuSign.eSign.Api;
using DocuSign.eSign.Model;

namespace docusignDemo.Services
{
    public class Envelopes
    {
        private readonly EnvelopesApi _envelopesApi;
        public Envelopes()
        {
            _envelopesApi = new EnvelopesApi();
        }
        /// <summary>
        /// Creates the env definition.
        /// </summary>
        /// <param name="templateId">The template identifier.</param>
        /// <param name="templateRolesList">The template roles list.</param>
        /// <param name="envTemplate">The env template.</param>
        /// <returns>EnvelopeDefinition.</returns>
        public EnvelopeDefinition CreateEnvDef(string templateId, List<TemplateRole> templateRolesList, EnvelopeTemplate envTemplate)
        {
            var envDef = new EnvelopeDefinition
            {
                TemplateId = templateId,
                TemplateRoles = templateRolesList,
                EmailSubject = envTemplate.EmailSubject,
                Status = "sent"
            };
            return envDef;
        }


        /// <summary>
        /// Lists the recipients.
        /// </summary>
        /// <param name="envelopeId">The envelope identifier.</param>
        /// <returns>DocuSign.eSign.Model.Recipients.</returns>
        public global::DocuSign.eSign.Model.Recipients ListRecipients(string envelopeId)
        {
            var recipients = _envelopesApi.ListRecipients(Account.AccountId, envelopeId);
            return recipients;
        }
        /// <summary>
        /// Creates the envelope.
        /// </summary>
        /// <param name="envDef">The env definition.</param>
        /// <returns>EnvelopeSummary.</returns>
        public EnvelopeSummary CreateEnvelope(EnvelopeDefinition envDef)
        {
            var envSummary = _envelopesApi.CreateEnvelope(Account.AccountId, envDef);
            return envSummary;
        }

        /// <summary>
        /// Creates the recipient view.
        /// </summary>
        /// <param name="envelopeId">The envelope identifier.</param>
        /// <param name="viewOptions">The view options.</param>
        /// <returns>ViewUrl.</returns>
        public ViewUrl CreateRecipientView(string envelopeId, RecipientViewRequest viewOptions)
        {
            var view = _envelopesApi.CreateRecipientView(Account.AccountId, envelopeId, viewOptions);
            return view;
        }



    }
}
using DocuSign.eSign.Model;
using docusignDemo.Properties;

namespace docusignDemo.Services
{
    public class Recipients
    {
        /// <summary>
        /// Views the request.
        /// </summary>
        /// <param name="returnUrl">The return URL.</param>
        /// <param name="clientUserId">The client user identifier.</param>
        /// <param name="userName">Name of the user.</param>
        /// <param name="email">The email.</param>
        /// <returns>RecipientViewRequest.</returns>
        public RecipientViewRequest ViewRequest(string returnUrl, string clientUserId, string userName, string email)
        {
            var agentViewOptions = new RecipientViewRequest
            {
                ReturnUrl = $"{Settings.Default.BaseUrl}{returnUrl}",
                AuthenticationMethod = "email",
                ClientUserId = clientUserId, // must match clientUserId set in Roles
                UserName = userName,
                Email = email
            };
            return agentViewOptions;
        }
 
    }
}
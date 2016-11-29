using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using docusignDemo.Properties;

namespace docusignDemo.Services
{
  
    public static class Account
    {
        private static string _accountId;

        public static string AccountId
        {
            get
            {
                if (_accountId == null)
                    Login();

                return _accountId;
            }
            set { _accountId = value; }
        }

        /// <summary>
        /// Login this instance.
        /// </summary>
        public static void Login()
        {
            var username = Settings.Default.Username;
            var password = Settings.Default.Password;
            var integratorKey = Settings.Default.IntegratorKey;
            var restUrl = Settings.Default.RestUrl;

            // initialize client for desired environment (for production change to www)
            var apiClient = new ApiClient(restUrl);
            Configuration.Default.ApiClient = apiClient;

            // configure 'X-DocuSign-Authentication' header
            var authHeader = "{\"Username\":\"" + username + "\", \"Password\":\"" + password +
                             "\", \"IntegratorKey\":\"" + integratorKey + "\"}";
            Configuration.Default.AddDefaultHeader("X-DocuSign-Authentication", authHeader);

            // we will retrieve this from the login API call
            string accountId = null;

            /////////////////////////////////////////////////////////////////
            // STEP 1: LOGIN API        
            /////////////////////////////////////////////////////////////////

            // login call is available in the authentication api 
            var authApi = new AuthenticationApi();
            var loginInfo = authApi.Login();

            // parse the first account ID that is returned (user might belong to multiple accounts)
            accountId = loginInfo.LoginAccounts[0].AccountId;

            AccountId = accountId;
        }
    }
}
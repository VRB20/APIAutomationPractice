using CoreFramework.Extensions;
using CoreFramework.Models;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Formatting;
using System.Text;
using System.Threading.Tasks;
using System;

namespace CoreFramework.Helpers
{
    public static class ApiHelperHttpClient
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly AppHelperConfig Config = new AppHelperConfig();
        public static async Task<SampleResponse1> CallAPI(string thePayload, bool throwOnNonSuccessStatusCode = true)
        {
            ConfigureHttpClient();
                        
            var content = new StringContent(thePayload, Encoding.UTF8, "application.json");
            //var requestUri = Config.Uri;
            var requestUri = "https://reqres.in/api/users";
            var response = await Client.PostAsync(requestUri, content).ConfigureAwait(false);

            if (throwOnNonSuccessStatusCode && !response.IsSuccessStatusCode)
                throw new Exception($@"'{nameof(CallAPI)}' Api Call Failed.
Request:
{thePayload}
Response:
{response.ToJson()}
");
            return await response.Content.ReadAsAsync<SampleResponse1>().ConfigureAwait(false);
        }

        public static void ConfigureHttpClient()
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Config.AuthToken);
        }
    }
}

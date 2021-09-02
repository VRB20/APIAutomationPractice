using CoreFramework.Models;
using RestSharp;
using System;
using System.Collections.Generic;

namespace CoreFramework.Helpers
{
    public static class ApiHelperRestSharp
    {
        private static RestClient _restClient;
        private static RestRequest _restRequest;
        private static IRestResponse _restResponse;

        public static void SetUpBaseUri (this string baseUri)
        {
           _restClient = new RestClient(baseUri);
        }

        public static string MakePostCall(this string resource, string jsonPayload)
        {
            _restRequest = new RestRequest(resource, Method.POST);
            _restRequest.AddJsonBody(jsonPayload);

            _restResponse = _restClient.Execute(_restRequest);
            Console.WriteLine("Printing results for fun : " + _restResponse.Content);
            return _restResponse.Content;
        }
    }
}

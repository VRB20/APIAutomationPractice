using CoreFramework.Extensions;
using CoreFramework.Helpers;
using CoreFramework.Models;
using CoreFramework.ObjectGraphBatchValidation;
using CoreFramework.ObjectGraphBatchValidation.Models;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static CoreFramework.Services.StepWrappingExtensions;

namespace CoreFramework.Services
{
    public static class TestCaseProcessingService
    {
        private static readonly Func<Func<Task<SampleResponse1>>, FuncExecuteExtensionHook<Task<SampleResponse1>>> Execute = StepWrappingExtensions.Execute;

        private static SampleRequest1 CreateRequest(ApiTestCase apiTestCase)
        {
            var result = SampleRequest1Factory.CreateRequest(apiTestCase.ConstructionBatch);
            return result;
        }

        private static async Task<SampleResponse1> CallAPIUsingRestSharp <T> (T thePayload, bool throwOnNonSuccessStatusCode = true)
        {
            var payload = thePayload.ToJson(SerializationExtensions.CamelCaseSerializerSettingsExcludeNulls);
            //TODO Read from config file
            var baseURI = "https://reqres.in/";
            baseURI.SetUpBaseUri();
            var response = "api/users".MakePostCall(payload);
            SampleResponse1 resp = JsonConvert.DeserializeObject<SampleResponse1>(response);
            return resp;
        }

        private static async Task<SampleResponse1> CallAPIUsingHttpClientp<T>(T thePayload, bool throwOnNonSuccessStatusCode = true)
        {
            var payload = thePayload.ToJson(SerializationExtensions.CamelCaseSerializerSettingsExcludeNulls);
            payload.Log($@"Request-{TestContext.CurrentContext.Test.Name}:{Environment.NewLine}");

            return await ApiHelperHttpClient.CallAPI(payload, throwOnNonSuccessStatusCode).ConfigureAwait(false);
        }

        private static async Task<SampleResponse1> CreateResponse(SampleRequest1 request)
        {
            var actionResponse = await CallAPIUsingRestSharp(request).ConfigureAwait(false);
            actionResponse.Log($@"Response-{TestContext.CurrentContext.Test.Name}:{Environment.NewLine}");

            return actionResponse;
        }
        public static async Task<ValidationErrors> ProcessTestCaseWithSingleFactory(ApiTestCase apiTestCase)
        {
            var request = CreateRequest(apiTestCase);

            var response = await CreateResponse(request).ConfigureAwait(false);

            return response.PerformResponseBatchValidation (apiTestCase.ValidationBatch);
        }
    }
}

using CoreFramework.Extensions;
using CoreFramework.Infrastructure;
using CoreFramework.Models;
using CoreFramework.ObjectGraphBatchValidation.Models;
using CoreFramework.Services;
using FluentAssertions;
using NUnit.Framework;
using System.Threading.Tasks;

namespace ApiFunctionalTests.Nunit_Tests
{
    [Category("API Tests")]
    [TestFixture]
    public class AutoRunnerScenarios : ApiAutoRunnerBase
    {
        [Test]
        [TestCaseSource(nameof(TestCaseSource), new object[] {@"Api Input\ReqRes"})]
        public async Task ProcessTestCases(ApiTestCase apiTestCase)
        {
            TestContext.CurrentContext.Test.Name.Log();
            apiTestCase.ValidationBatch = apiTestCase.ValidationBatch.FilterBatchByTheFollowingKeys(
                "TestFilter");
            var errors = await TestCaseProcessingService.ProcessTestCaseWithSingleFactory(apiTestCase).ConfigureAwait(false);
            errors.Should().BeEmpty();
        }

    }
}

using CoreFramework.Infrastructure;
using CoreFramework.Extensions;
using NUnit.Framework;
using FluentAssertions;
using CoreFramework.ObjectGraphBatchValidation;
using System.Reflection;

namespace CoreFramework.UnitTests
{
    public class ApiAutoRunnerTests
    {
        [Test]
        [Explicit]
        public void TestCaseSourceGenerator_runs_without_error()
        {
            //var testDataFolder = typeof(ApiAutoRunnerBase).Assembly.GetArtifactsFolder(@"TestData\API Input");
            var testDataFolder = Assembly.GetExecutingAssembly().GetFolder(@"TestData\API Input");
            var artifactsFolder = typeof(ApiAutoRunnerBase).Assembly.GetArtifactsFolder("src");

            var testCaseSourceGenerator = ApiAutoRunnerBase.TestCaseSourceGenerator(testDataFolder, artifactsFolder);

            testCaseSourceGenerator.Should().NotBeEmpty();

            var testCase = testCaseSourceGenerator[0];

            var constructionBatch = testCase.ConstructionBatch;
            var rows = constructionBatch.ToDataTableRows();
        }

        [Test]
        [Explicit]
        public void TestCaseSource_runs_without_error()
        {

            var testCaseSourceGenerator = ApiAutoRunnerBase.TestCaseSource(@"API Input\ReqRes");

        }
    }
}

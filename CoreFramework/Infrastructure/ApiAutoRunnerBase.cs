using NUnit.Framework;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Reflection;
using CoreFramework.Extensions;
using System.IO;
using CoreFramework.Models;
using CoreFramework.Services;
using CoreFramework.Infrastructure.Extensions;
using System.Linq;

namespace CoreFramework.Infrastructure
{
    public class ApiAutoRunnerBase
    {
        private static readonly object Padlock = new object();
        private static readonly object Padlock2 = new object();
        private static readonly object Padlock3 = new object();
        private static List<TestCaseData> testCaseData;
        private static List<TestCaseData> testCaseDataForFilesWithIssues;
        private static List<TestCaseData> testCaseDataForScenarioId;

        public static readonly string TestCaseFilter = "";

        private static readonly IDirectoryInfo GetTestDataFolder = typeof(ApiAutoRunnerBase).Assembly.GetArtifactsFolder(@"TestData\Test");
        private static readonly IDirectoryInfo GetTestDataFolderForFileWithIssues = typeof(ApiAutoRunnerBase).Assembly.GetArtifactsFolder(@"TestData\Test\TestData-with-issues");
        private static readonly IDirectoryInfo GetTestDataFolderForScenarioID = typeof(ApiAutoRunnerBase).Assembly.GetArtifactsFolder(@"TestData\Test\Progression\ScenarioID");
        private static readonly IDirectoryInfo ArtifactsFolder = typeof(ApiAutoRunnerBase).Assembly.GetSolutionFolder();

        public static List<TestCaseData> TestCaseSource(string FolderPath)
        {
            testCaseData = null;
            List<TestCaseData> TheFunc() =>
                TestCaseSourceGenerator(
                    Path.Combine(GetTestDataFolder.FullName,FolderPath).AsDirectoryInfo(),
                    ArtifactsFolder)
                .Select(x => new TestCaseData(x) { TestName = x.TestName })
                .Where(x => string.IsNullOrWhiteSpace(TestCaseFilter) || x.TestName.StartsWith(TestCaseFilter))
                .OrderByDescending(x => x.TestName)
                .ToList();

            lock (Padlock)
            {
                if (testCaseData == null) testCaseData = TheFunc();
            }
            return testCaseData;
        }

        public static List<TestCaseData> TestCaseSourceForFilesWithIssues(string FolderPath)
        {
            testCaseDataForScenarioId = null;
            List<TestCaseData> TheFunc() =>
                TestCaseSourceGenerator(
                    GetTestDataFolderForScenarioID,
                    ArtifactsFolder,
                    SearchOption.AllDirectories)                    
                .Select(x => new TestCaseData(x) { TestName = x.TestName })
                .Where(x => string.IsNullOrWhiteSpace(TestCaseFilter) || x.TestName.StartsWith(TestCaseFilter))
                .OrderByDescending(x => x.TestName)
                .ToList();

            lock (Padlock3)
            {
                if (testCaseDataForScenarioId == null) testCaseDataForScenarioId = TheFunc();
            }
            return testCaseDataForScenarioId;
        }

        public static List<TestCaseData> TestCaseSourceForScenarioID(string FolderPath)
        {
            testCaseDataForFilesWithIssues = null;
            List<TestCaseData> TheFunc() =>
                TestCaseSourceGenerator(
                    GetTestDataFolderForFileWithIssues,
                    ArtifactsFolder,
                    SearchOption.AllDirectories)
                .Select(x => new TestCaseData(x) { TestName = x.TestName })
                .Where(x => string.IsNullOrWhiteSpace(TestCaseFilter) || x.TestName.StartsWith(TestCaseFilter))
                .OrderByDescending(x => x.TestName)
                .ToList();

            lock (Padlock2)
            {
                if (testCaseDataForFilesWithIssues == null) testCaseDataForFilesWithIssues = TheFunc();
            }
            return testCaseDataForFilesWithIssues;
        }

        public static List<ApiTestCase> TestCaseSourceGenerator(IDirectoryInfo testDataFolder, IDirectoryInfo artifactsFolder, SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var inputFiles = new InputFileService().GetInputFiles(testDataFolder, searchOption);
            var featureFiles = new FeatureFileService().GetFeatureFiles(artifactsFolder);
            var allScenarioDetails = new SpecFlowMetadataService().ExtractSpecFlowMetadata(featureFiles);

            return inputFiles
                .ToTestCases(allScenarioDetails)
                .OrderBy(x => x.TestName)
                .ToList();
        }
    }
}

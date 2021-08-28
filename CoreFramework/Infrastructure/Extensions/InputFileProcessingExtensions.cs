using CoreFramework.Models;
using CoreFramework.ObjectGraphBatchValidation;
using CoreFramework.ObjectGraphBatchValidation.Models;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using TechTalk.SpecFlow.Tracing;

namespace CoreFramework.Infrastructure.Extensions
{
    public static class InputFileProcessingExtensions
    {
        public static List<ApiTestCase> ToTestCases(
            this List<IFileInfo> inputFiles,
            List<ApiTestCase.TestCaseMetadata> allScenarioDetails) => inputFiles
            .SelectMany(inputFile => ToTestCases(inputFile, allScenarioDetails))
            .ToList();

        public static List<ApiTestCase> ToTestCases(
            this IFileInfo excelInputFile,
            List<ApiTestCase.TestCaseMetadata> allScenarioDetails)
        {
            var extractCsvData = excelInputFile.ToInputModelOfValidationBatchItems();


            var testCases = extractCsvData.Data.Select(x => x["TestCase"]).Distinct();
            var testCaseBatches = testCases.ToDictionary(
                x => x,
                testCaseName => extractCsvData.Data.Where(batchItem => batchItem["TestCase"].Equals(testCaseName)).ToBatch());

            var matchedData = testCaseBatches
                .Select(testCaseBatch =>
                {
                    var scenarioDetails = allScenarioDetails.FirstOrDefault(scenario =>
                                                scenario.ScenarioName.Equals(testCaseBatch.Key, StringComparison.InvariantCultureIgnoreCase) &&
                                                scenario.ExcelFileName.Equals(excelInputFile.Name, StringComparison.InvariantCultureIgnoreCase))
                                            ?? ApiTestCase.TestCaseMetadata.Default;

                    var (constructionBatch, validationBatch) = testCaseBatch.Value.SeparateConstructionFromValidationBatches();

                    return new ApiTestCase
                    {
                        Batch = testCaseBatch.Value,
                        ConstructionBatch = constructionBatch,
                        ValidationBatch = validationBatch,
                        Metadata = scenarioDetails,
                        InputFile = excelInputFile,
                        TestName = $"{scenarioDetails.FeatureName}-{testCaseBatch.Key}-{excelInputFile.Name}".ToIdentifierPart()
                    };
                })
                .Where(x => x.Metadata != null)
                .ToList();

            return matchedData
                .OrderBy(x => x.TestName)
                .ToList();

        }
    }
}

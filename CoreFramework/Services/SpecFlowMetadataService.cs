using CoreFramework.Models;
using Gherkin;
using Gherkin.Ast;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;

namespace CoreFramework.Services
{
    public class SpecFlowMetadataService
    {
        private const string ExcelSearchText = "I want to construct request payload from the 'source' file'";

        private static List<ApiTestCase.TestCaseMetadata> ConstructFeatureDetails(GherkinDocument document, IFileInfo featureFile) => document.Feature.Children
            .OfType<Scenario>()
            .Select(scenario =>
            {
                var steps = scenario.Steps.Select(x => x.Text).ToList();
                var referencedExcelFile = steps.Single(x => x.StartsWith(ExcelSearchText))
                    .Replace(ExcelSearchText, string.Empty)
                    .Replace("", string.Empty);

                return new ApiTestCase.TestCaseMetadata
                {
                    FeatureFile = featureFile,
                    FeatureName = document.Feature.Name,
                    FeatureTags = document.Feature.Tags.Select(x => x.Name.Replace("@", string.Empty)).ToList(),
                    ScenarioName = scenario.Name,
                    Tags = scenario.Tags.Select(x => x.Name.Replace("@", string.Empty)).ToList(),
                    Steps = steps,
                    HasExamples = scenario.Examples.Any(),
                    ExcelFileName = referencedExcelFile
                };
            }).ToList();

        public List<ApiTestCase.TestCaseMetadata> ExtractSpecFlowMetadata(List<IFileInfo> featureFiles)
        {
            return featureFiles
                .SelectMany(featureFile =>
                {
                    //TODO: Poly
                    GherkinDocument gherkinDocument;
                    try
                    {
                        gherkinDocument = new Parser().Parse(featureFile.FullName);
                    }
                    catch
                    {
                        Thread.Sleep(500);
                        gherkinDocument = new Parser().Parse(featureFile.FullName);
                    }

                    return ConstructFeatureDetails(gherkinDocument, featureFile);
                })
                .ToList();
        }
    }
}

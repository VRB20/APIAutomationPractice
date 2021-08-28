using CoreFramework.ObjectGraphBatchValidation.Models;
using System.Collections.Generic;
using System.IO.Abstractions;

namespace CoreFramework.Models
{
    public class ApiTestCase
    {
        public Batch Batch { get; set; }
        public Batch ConstructionBatch { get; set; }
        public Batch ValidationBatch { get; set; }
        public TestCaseMetadata Metadata { get; set; }
        public string TestName { get; set; }
        public ValidationErrors ValidationErrors { get; set; }
        public IFileInfo InputFile { get; set; }

        public class TestCaseMetadata
        {
            public string ScenarioName { get; set; }
            public List<string> Tags { get; set; }
            public List<string> Steps { get; set; }
            public bool HasExamples { get; set; }
            public string ExcelFileName { get; set; }
            public string FeatureName { get; set; }
            public IFileInfo FeatureFile { get; set; }
            public List<string> FeatureTags { get; set; } = new List<string>();

            public static TestCaseMetadata Default => new TestCaseMetadata
            {
                FeatureName = "APIAutoRunner",
                ScenarioName = "APIAutoRunner"
            };
        }
    }
}

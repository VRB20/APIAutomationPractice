using CoreFramework.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreFramework.Services
{
    public class FeatureFileService
    {
        private static readonly List<string> PathsToExclude = new List<string>
        {
            "Initial1.Testing",
            "Genereal.Testing.C"
        };

        public List<IFileInfo> GetFeatureFiles(IDirectoryInfo featureFileRoot)
        {
            featureFileRoot = featureFileRoot ?? throw new ArgumentNullException(nameof(featureFileRoot));
            featureFileRoot.FullName.Log("Feature File Root Folder: ");

            return featureFileRoot
                .GetFiles("*.feature", SearchOption.AllDirectories)
                .Where(x => !PathsToExclude.Any(excludedPath => x.FullName.Contains(excludedPath)))
                .ToList();
        }
    }
}

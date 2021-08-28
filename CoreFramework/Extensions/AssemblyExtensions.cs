using System;
using System.IO.Abstractions;
using System.Reflection;

namespace CoreFramework.Extensions
{
    public static class AssemblyExtensions
    {
        private static readonly IFileSystem FileSystem = new FileSystem();

        public static IDirectoryInfo GetArtifactsFolder(this Assembly executingAssembly, string foldername = ".build")
        {
            var testArtifactsFolder = FileSystem.Path.Combine(GetRepositoryRootFolder(executingAssembly).FullName, foldername);
            FileSystem.Directory.CreateDirectory(testArtifactsFolder);

            return FileSystem.DirectoryInfo.FromDirectoryName(testArtifactsFolder);
        }
        public static IDirectoryInfo GetLocation(this Assembly executingAssembly)
        {
            executingAssembly = executingAssembly ?? Assembly.GetExecutingAssembly();

            var codeBase = executingAssembly.CodeBase;
            var uri = new UriBuilder(codeBase);
            var path = Uri.UnescapeDataString(uri.Path);
            var location = FileSystem.Path.GetDirectoryName(path);

            var message = $@"{executingAssembly.FullName}'s location is '{location}')";
            Console.WriteLine(message);

            return FileSystem.DirectoryInfo.FromDirectoryName(location);
        }

        public static IDirectoryInfo GetFolder(this Assembly executingAssembly, string folderName)
        {
            var folder = FileSystem.Path.Combine(executingAssembly.GetLocation().FullName, folderName);
            //FileSystem.Directory.CreateDirectory(folder);

            return FileSystem.DirectoryInfo.FromDirectoryName(folder);
        }
        public static IDirectoryInfo GetRepositoryRootFolder(this Assembly executingAssembly)
        {
            var location = executingAssembly.GetLocation();

            IDirectoryInfo updatedLocation = null;
            if (location.Parent.Name.Equals("bin", StringComparison.InvariantCultureIgnoreCase))
                updatedLocation = location.Parent.Parent.Parent.Parent;

            if (location.Parent.Parent.Name.Equals("bin", StringComparison.InvariantCultureIgnoreCase))
                updatedLocation = location.Parent.Parent.Parent.Parent.Parent;

            return updatedLocation ?? location;
        }

        public static IDirectoryInfo GetSolutionFolder(this Assembly executingAssembly) => FileSystem.DirectoryInfo.FromDirectoryName(
           FileSystem.Path.Combine(GetRepositoryRootFolder(executingAssembly).FullName, "src"));

    }
}

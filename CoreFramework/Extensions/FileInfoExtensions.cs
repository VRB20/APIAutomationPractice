using System.IO;
using System.IO.Abstractions;

namespace CoreFramework.Extensions
{
    public static class FileInfoExtensions
    {
        private static readonly IFileSystem FileSystem = new FileSystem();
        public static IDirectoryInfo AsDirectoryInfo(this string theFilePath) => FileSystem.DirectoryInfo.FromDirectoryName(theFilePath);

        public static IFileInfo AsFileInfo(this string theFilePath) => FileSystem.FileInfo.FromFileName(theFilePath);

        public static FileInfo NameWithoutExtension(this FileInfo theFile) => new FileInfo(
            FileSystem.Path.Combine(
                theFile.Directory.FullName,
                theFile.Name.Replace(theFile.Extension, string.Empty)));
    }
}

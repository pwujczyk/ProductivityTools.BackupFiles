using ProductivityTools.BackupFiles.Logic.Modes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductivityTools.BackupFiles.Logic.Actions.RedundantItems
{
    [Action((int)RedundantItemsMode.Remove)]
    class Remove : RedundantItemsBase
    {
        public override void Process(string masterSourcePath, string masterDestinationPath, string directory)
        {
            string endPath = directory.Substring(masterSourcePath.Length);
            string destination = Path.Combine(masterDestinationPath, endPath);
            // string destinationDirectory = Path.GetDirectoryName(destination);

            ProcessFiles(directory, destination);
            ProcessDirectories(directory, destination);
        }

        private static void ProcessDirectories(string directory, string destination)
        {
            var sourceDirectories = Directory.GetDirectories(directory);
            var destinationDirectoreis = Directory.GetDirectories(destination);
            var sourcePaths = sourceDirectories.Select(x => new DirectoryInfo(x));
            var destinationPaths = destinationDirectoreis.Select(x => new DirectoryInfo(x));
            var differences = destinationPaths.Except(sourcePaths, new DirectoryComparer());
            foreach (var difference in differences)
            {
                Directory.Delete(difference.FullName, true);
            }
        }

        private static void ProcessFiles(string directory, string destination)
        {
            var sourceFiles = Directory.GetFiles(directory);
            var destinationFiles = Directory.GetFiles(destination);
            var sourcePaths = sourceFiles.Select(x => new FileInfo(x));
            var destinationPaths = destinationFiles.Select(x => new FileInfo(x));
            var differences = destinationPaths.Except(sourcePaths, new FileComparer());
            foreach (var difference in differences)
            {
                File.Delete(difference.FullName);
            }
        }
    }

    public class FileComparer : IEqualityComparer<FileInfo>
    {
        bool IEqualityComparer<FileInfo>.Equals(FileInfo x, FileInfo y)
        {
            return x.Name == y.Name;
        }

        int IEqualityComparer<FileInfo>.GetHashCode(FileInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }

    public class DirectoryComparer : IEqualityComparer<DirectoryInfo>
    {
        bool IEqualityComparer<DirectoryInfo>.Equals(DirectoryInfo x, DirectoryInfo y)
        {
            return x.Name == y.Name;
        }

        int IEqualityComparer<DirectoryInfo>.GetHashCode(DirectoryInfo obj)
        {
            return obj.Name.GetHashCode();
        }
    }
}

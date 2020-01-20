using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Actions
{
    [Action(BackupMode.DoNothing)]
    class DoNothing : BaseAction
    {
        public DoNothing() : base(null) { }

        public override void Process(string masterSourcePath, string masterDestinationPath, string directory)
        {
            ProcessFilesInDirectory(masterSourcePath, masterDestinationPath, directory);
        }

        private void ProcessFilesInDirectory(string masterSourcePath, string masterDestinationPath, string directory)
        {
            //Console.WriteLine($"Processing directory {directory}");
            //string[] filePaths = Directory.GetFiles(directory);
            //string endPath = directory.Substring(masterSourcePath.Length);
            //var destination = Path.Combine(masterDestinationPath, endPath);
            //if (endPath.Length > 0)
            //{
            //    Directory.CreateDirectory(destination);
            //}
            //foreach (var file in filePaths)
            //{
            //    FileInfo f = new FileInfo(file);
            //    var fileDestination = Path.Combine(destination, f.Name);
            //    //Console.WriteLine($"Copying file from {file} to {fileDestination}");
            //    File.Copy(file, fileDestination);
            //}
        }
    }
}

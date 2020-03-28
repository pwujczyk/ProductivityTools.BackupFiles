using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductivityTools.PSBackupFiles.Verbose;

namespace ProductivityTools.BackupFiles.Logic.Actions
{
    [Action((int)BackupMode.CopyRecursively)]
    class CopyFilesRecursive : BaseAction
    {
        public CopyFilesRecursive() : base()
        { }

        public override void Process(string masterSourcePath, string masterDestinationPath, string directory)
        {
            ProcessFilesInDirectory(masterSourcePath, masterDestinationPath, directory);
            base.ManageRedundantItems(masterSourcePath, masterDestinationPath, directory);
        }

        private void ProcessFilesInDirectory(string masterSourcePath, string masterDestinationPath, string directory)
        {
            VerboseHelper.WriteVerbose(VerbosityLevel.Detailed, $"Processing directory {directory}");
            string[] filePaths = Directory.GetFiles(directory);
            string endPath = directory.Substring(masterSourcePath.Length).Trim('\\');
            var destination = Path.Combine(masterDestinationPath, endPath);
            if (endPath.Length > 0)
            {
                Directory.CreateDirectory(destination);
            }
            foreach (var file in filePaths)
            {
                FileInfo f = new FileInfo(file);
                var fileDestination = Path.Combine(destination, f.Name);
                VerboseHelper.WriteVerbose(VerbosityLevel.Detailed, $"Copying file from {file} to {fileDestination}");
                copyStrategy.Copy(file, fileDestination);
            }
        }
    }
}

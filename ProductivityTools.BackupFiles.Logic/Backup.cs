using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.PSBackupFiles.Verbose;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public class Backup
    {
        private readonly string MasterSourcePath;
        private readonly string MasterDestinationPath;
        ActionList ActionList = new ActionList();
        BackupFile BackupFile = new BackupFile();

        public Backup(string masterSourcePath, string masterDestinationPath)
        {
            this.MasterSourcePath = masterSourcePath;
            this.MasterDestinationPath = masterDestinationPath;
        }

        public void PerformBackup()
        {
            ValidateDestinationDirectory();
            ProcessDirectories(this.MasterSourcePath, 0);
        }

        private void ValidateDestinationDirectory()
        {
            System.IO.Directory.CreateDirectory(MasterDestinationPath);
        }

        private void ProcessDirectories(string directory, int depth)
        {
            if (depth < 4)
            {
                VerboseHelper.WriteVerboseStatic($"Looking for powershellconfig in {directory}");
            }
            if (directory.StartsWith(this.MasterDestinationPath, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (Access(directory))
            {
                var config = BackupFile.GetBackupConfig(directory);
                if (config != null)
                {
                    ActionList.Add(directory, config);
                }
                ProcessDirectory(directory, depth);
                GetDirectories(directory, depth);
            }
        }

        private void ProcessDirectory(string directory, int depth)
        {
            ActionList.InvokeForPath(this.MasterSourcePath, this.MasterDestinationPath, directory);
        }

        private void GetDirectories(string directory, int depth)
        {
            string[] filePaths = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                ProcessDirectories(filePath, depth + 1);
            }
        }

        private bool Access(string directory)
        {
            try
            {
                Directory.GetAccessControl(directory);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}

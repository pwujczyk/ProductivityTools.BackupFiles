using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.Tools;
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

        public Backup(string masterSourcePath)
        {
            this.MasterSourcePath = masterSourcePath;
        }

        public Backup(string masterSourcePath, string masterDestinationPath) : this(masterSourcePath)
        {
            this.MasterDestinationPath = masterDestinationPath;
        }

        public void PerformBackup()
        {
            ValidateDestinationDirectory();
            ProcessDirectories(this.MasterSourcePath, 0, true);
        }

        public IEnumerable<string> FindBackupDirectories()
        {
            ProcessDirectories(this.MasterSourcePath, 0, false);
            return this.ActionList.BackupFiles;
        }

        private void ValidateDestinationDirectory()
        {
            System.IO.Directory.CreateDirectory(MasterDestinationPath);
        }

        private void ProcessDirectories(string directory, int depth, bool process)
        {
            WriteVerbose.Write(depth, $"Looking for powershellconfig in {directory}");

            if (process && directory.StartsWith(this.MasterDestinationPath, StringComparison.OrdinalIgnoreCase))
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
                if (process)
                {
                    ProcessDirectory(directory, depth);
                }
                GetDirectories(directory, depth, process);
            }
        }

        private void ProcessDirectory(string directory, int depth)
        {
            ActionList.InvokeForPath(this.MasterSourcePath, this.MasterDestinationPath, directory, depth);
        }

        private void GetDirectories(string directory, int depth, bool process)
        {
            string[] filePaths = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                ProcessDirectories(filePath, depth + 1, process);
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

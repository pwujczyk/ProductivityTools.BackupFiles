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

        public Backup(string masterSourcePath, string masterDestinationPath)
        {
            this.MasterSourcePath = masterSourcePath;
            this.MasterDestinationPath = masterDestinationPath;
        }

        public void FindBackupDirectories()
        {
            FindBackupDirectories(this.MasterSourcePath, 0);
        }

        public void FindBackupDirectories(string directory, int depth)
        {
            if (depth < 4)
            {
                Console.WriteLine($"Looking for powershellconfig in {directory}");
            }
            if (directory.StartsWith(this.MasterDestinationPath,StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            if (Access(directory))
            {
                if (GetFile(directory))
                {
                    ActionList.Add(directory, ProcessFilesInDirectory);
                    ProcessDirectory(directory, depth);
                }
                else
                {
                    if (ActionList.Contains(directory))
                    {
                        ProcessFilesInDirectory(directory);
                    }
                    GetDirectories(directory, depth);
                }
            }
        }

        private void ProcessDirectory(string directory, int depth)
        {
            ActionList.InvokeForPath(directory);
            GetDirectories(directory, depth);
        }

        private void ProcessFilesInDirectory(string directory)
        {
            Console.WriteLine($"Processing directory {directory}");
            string[] filePaths = Directory.GetFiles(directory);
            string endPath = directory.Substring(this.MasterSourcePath.Length);
            var destination = Path.Combine(this.MasterDestinationPath, endPath);
            if (endPath.Length > 0)
            {
                Directory.CreateDirectory(destination);
            }
            foreach (var file in filePaths)
            {
                FileInfo f = new FileInfo(file);
                var fileDestination = Path.Combine(destination, f.Name);
                //Console.WriteLine($"Copying file from {file} to {fileDestination}");
                File.Copy(file, fileDestination);
            }
        }

        private void GetDirectories(string directory, int depth)
        {
            string[] filePaths = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                FindBackupDirectories(filePath, depth + 1);
            }
        }

        private bool GetFile(string directory)
        {
            var x = Directory.GetFiles(directory, ".powershell", SearchOption.TopDirectoryOnly);
            foreach (var xxx in x)
            {
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" + xxx);
            }
            if (x.Count() > 0)
            {
                return true;
            }
            else
            {
                return false;
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

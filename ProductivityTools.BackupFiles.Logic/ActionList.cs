using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class ActionList
    {
        private Dictionary<string, BackupConfig> CurrentWork = new Dictionary<string, BackupConfig>();

        public void Add(string key, BackupConfig backupConfig)
        {
            this.CurrentWork.Add(key, backupConfig);
        }

        public bool Contains(string path)
        {
            foreach (var item in this.CurrentWork)
            {
                if (path.StartsWith(item.Key))
                {
                    return true;
                }
            }
            return false;
        }

        public void InvokeForPath(string masterSourcePath, string masterDestinationPath, string directory)
        {
            var workOrdered = this.CurrentWork.OrderByDescending(x => x.Key.Length);
            foreach (var item in workOrdered)
            {
                if (directory.StartsWith(item.Key))
                {
                    //improve this
                    BackupWorker worker = new BackupWorker(masterSourcePath, masterDestinationPath);
                    worker.Process(directory,item.Value);


                    //item.Value.Mode.Process(masterSourcePath, masterDestinationPath, directory);
                    return;
                }
            }
        }
    }
}


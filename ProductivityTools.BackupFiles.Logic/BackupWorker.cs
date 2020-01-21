using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class BackupWorker
    {
        public string MasterSourcePath { get; set; }
        public string MasterDestPath { get; set; }

        public BackupWorker(string source, string dest)
        {
            this.MasterSourcePath = source;
            this.MasterDestPath = dest;
        }

        public void Process(string processingDirectory, BackupConfig config)
        {
            var x=ReflectionTools.CreateInstanceOfActionFromEnum(config.Mode);
            x.SetCopyStrategy(new OverrideIfNewer());
            x.Process(this.MasterSourcePath, this.MasterDestPath, processingDirectory);
        }
    }
}

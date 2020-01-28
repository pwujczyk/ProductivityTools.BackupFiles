using ProductivityTools.BackupFiles.Logic.Actions.RedundantItems;
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
            var backupMode=ReflectionTools.CreateInstanceOfActionFromEnum((int)config.Mode);
            var copyStrategyMode = ReflectionTools.CreateInstanceOfActionFromEnum<CopyStrategyBase>((int)config.CopyStrategy);
            backupMode.SetCopyStrategy(copyStrategyMode);
            var redundantMode= ReflectionTools.CreateInstanceOfActionFromEnum<RedundantItemsBase>((int)config.RedundantItems);
            backupMode.SetRedundantItemsMode(new Remove());
            backupMode.Process(this.MasterSourcePath, this.MasterDestPath, processingDirectory);
        }
    }


   
}

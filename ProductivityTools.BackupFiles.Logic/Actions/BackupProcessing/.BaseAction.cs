using ProductivityTools.BackupFiles.Logic.Actions.RedundantItems;
using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using ProductivityTools.BackupFiles.Logic.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Actions
{
    abstract class BaseAction
    {
        protected CopyStrategyBase copyStrategy;
        protected RedundantItemsBase redundantItems;

        public BaseAction(){}

        public abstract void Process(string masterSourcePath, string masterDestinationPath, string directory);

        public void SetCopyStrategy(CopyStrategyBase copyStrategy)
        {
            this.copyStrategy = copyStrategy;
        }

        public void SetRedundantItemsMode(RedundantItemsBase redundantItems)
        {
            this.redundantItems = redundantItems;
        }

        protected void ManageRedundantItems(string masterSourcePath, string masterDestinationPath, string directory)
        {
            this.redundantItems.Process(masterSourcePath, masterDestinationPath, directory);
        }

    }
}

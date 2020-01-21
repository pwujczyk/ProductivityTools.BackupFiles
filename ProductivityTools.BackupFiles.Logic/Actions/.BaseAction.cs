using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Actions
{
    abstract class BaseAction
    {
        protected CopyStrategy.CopyStrategyBase copyStrategy;

        public BaseAction()
        {
            
        }

        public abstract void Process(string masterSourcePath, string masterDestinationPath, string directory);

        public void SetCopyStrategy(CopyStrategyBase copyStrategy)
        {
            this.copyStrategy = copyStrategy;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Actions.RedundantItems
{
    abstract class RedundantItemsBase
    {
        public abstract void Process(string masterSourcePath, string masterDestinationPath, string directory);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Actions
{
    abstract class BaseAction
    {
        public abstract void Process(string masterSourcePath, string masterDestinationPath, string directory);
    }
}

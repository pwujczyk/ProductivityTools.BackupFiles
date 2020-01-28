using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.CopyStrategy
{
    abstract class CopyStrategyBase
    {
        public abstract void Copy(string source, string destination);
    }
}

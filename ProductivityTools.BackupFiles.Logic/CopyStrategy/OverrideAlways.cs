using ProductivityTools.BackupFiles.Logic.Modes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.CopyStrategy
{
    [Action((int)CopyStrategyMode.OverrideAlways)]
    class OverrideAlways : CopyStrategyBase
    {
        public override void Copy(string source, string destination)
        {
            File.Copy(source, destination, true);
        }
    }
}

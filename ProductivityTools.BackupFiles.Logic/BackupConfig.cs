using ProductivityTools.BackupFiles.Logic.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public class BackupConfig
    {
        public BackupMode Mode { get; set; }
        public CopyStrategyMode CopyStrategy { get; set; }
        public RedundantItemsMode RedundantItems { get; set; }
    }
}

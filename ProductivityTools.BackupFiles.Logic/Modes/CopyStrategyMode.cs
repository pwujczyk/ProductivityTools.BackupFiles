using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Modes
{
    [Description("It tells what to do if file in the target location will be found")]
    public enum CopyStrategyMode
    {
        NotDefined,
        OverrideAlways,
        OvverideIfModyficationDateIsNewer,
        BreakWhenFound,
        Omit,
    }
}

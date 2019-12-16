using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Modes
{
    public enum CopyStrategyMode
    {
        NotDefined,
        OverrideAlways,
        OvverideIfModyficationDateIsNewer
        BreakWhenFound,
        Omit,
        
    
    }
}

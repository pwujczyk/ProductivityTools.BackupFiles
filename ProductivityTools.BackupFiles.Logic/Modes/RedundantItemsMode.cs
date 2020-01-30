using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Modes
{
    [Description("It defines how to behave when in targed directory additional files or directories will be found. This situation will occur for example when in source you will move files.")]
    public enum RedundantItemsMode
    {
        NotDefined,
        Remove,
        Move,
        Leave
    }
}

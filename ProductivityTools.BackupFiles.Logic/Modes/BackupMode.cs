using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public enum BackupMode
    {
        NotDefined,
        DoNothing,
        [Description("It will copy files and directories recursively, until it will find another file which will override action")]
        CopyRecursively,
        CopyJustFiles
    }
}

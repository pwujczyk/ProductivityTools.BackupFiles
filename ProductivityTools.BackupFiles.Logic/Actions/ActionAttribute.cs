using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public class ActionAttribute : Attribute
    {
        public BackupMode BackupMode { get; set; }

        public ActionAttribute(BackupMode backupMode)
        {
            this.BackupMode = backupMode;
        }
    }
}

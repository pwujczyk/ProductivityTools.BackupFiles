using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public class ActionAttribute : Attribute
    {
        public int BackupMode { get; set; }

        public ActionAttribute(int backupMode)
        {
            this.BackupMode = backupMode;
        }
    }
}

using ProductivityTools.BackupFiles.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles
{
    [Cmdlet(VerbsCommon.New, "Backup")]
    public class BackupFilesCmdlet : ProductivityTools.PSCmdlet.PSCmdletPT
    {
        public BackupFilesCmdlet()
        {
        }

        protected override void ProcessRecord()
        {
            Backup b = new Backup(@"d:\", @"D:\trash\X1");
            b.FindBackupDirectories();
            base.ProcessRecord();

        }
    }
}

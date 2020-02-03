using ProductivityTools.BackupFiles.Logic;
using ProductivityTools.PSBackupFiles.Verbose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles
{
   [Cmdlet(VerbsCommon.Find, "BackupFiles")]
    public class FindBackupFilesCmdlet : ProductivityTools.PSCmdlet.PSCmdletPT
    {
        [Parameter(Position = 0)]
        public string Source { get; set; }

        [Parameter(Position = 1)]
        public VerbosityLevel VerbosityLevel { get; set; }

        public FindBackupFilesCmdlet()
        {
        }

        protected override void ProcessRecord()
        {
            VerboseHelper.SetVerbose(this.MyInvocation.BoundParameters.ContainsKey("Verbose"));
            VerboseHelper.SetVerbosityOutput(WriteVerbose);
            VerboseHelper.Level = VerbosityLevel;

            if (string.IsNullOrEmpty(Source))
            {
                Source = CurrentProviderLocation("FileSystem").ProviderPath;
            }

            //Backup b = new Backup(@"d:\BackupTest\", @"D:\trash\X1");
            Backup b = new Backup(Source);
            var directories=b.FindBackupDirectories();
            foreach(var directory in directories)
            {
                Console.WriteLine(directory);
            }
            base.ProcessRecord();
        }
    }
}

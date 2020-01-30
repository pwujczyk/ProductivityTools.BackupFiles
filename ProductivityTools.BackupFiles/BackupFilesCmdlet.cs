using ProductivityTools.BackupFiles.Logic;
using ProductivityTools.BackupFiles.Logic.Modes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ProductivityTools.PSBackupFiles.Verbose;

namespace ProductivityTools.BackupFiles
{
    [Cmdlet(VerbsCommon.New, "Backup")]
    public class BackupFilesCmdlet : ProductivityTools.PSCmdlet.PSCmdletPT
    {
        [Parameter(Position = 0)]
        public string Source { get; set; }

        [Parameter(Position = 1,Mandatory =true)]
        public string Destination { get; set; }

        public BackupFilesCmdlet()
        {

        }

        protected override void ProcessRecord()
        {
            VerboseHelper.SetVerbose(this.MyInvocation.BoundParameters.ContainsKey("Verbose"));
            VerboseHelper.WriteVerboseStatic = WriteVerbose;

            if (string.IsNullOrEmpty(Source))
            {
                Source = CurrentProviderLocation("FileSystem").ProviderPath; 
            }

            //Backup b = new Backup(@"d:\BackupTest\", @"D:\trash\X1");
            Backup b = new Backup(Source, Destination);
            b.PerformBackup();
            base.ProcessRecord();

        }
    }
}

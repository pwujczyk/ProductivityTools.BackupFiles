using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.CopyStrategy
{
    class OverrideIfNewer: CopyStrategyBase
    {
        public override void Copy(string source, string destination)
        {
            if (File.Exists(destination))
            {
                FileInfo fileinfoSource = new FileInfo(source);
                FileInfo fileinfodestination = new FileInfo(destination);
                if (fileinfoSource.LastWriteTimeUtc>fileinfodestination.LastWriteTimeUtc)
                {
                    File.Copy(source, destination);
                }
            }
            else
            {
                File.Copy(source, destination);
            }
        }
    }
}

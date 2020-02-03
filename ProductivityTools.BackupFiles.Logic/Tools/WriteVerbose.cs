using ProductivityTools.PSBackupFiles.Verbose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic.Tools
{
    static class WriteVerbose
    {
        internal static void Write(int depth, string s)
        {
            if (depth < 2)
            {
                VerboseHelper.WriteVerbose(VerbosityLevel.General, s);
            }
            else
            {
                VerboseHelper.WriteVerbose(VerbosityLevel.Detailed, s);
            }
        }
    }
}

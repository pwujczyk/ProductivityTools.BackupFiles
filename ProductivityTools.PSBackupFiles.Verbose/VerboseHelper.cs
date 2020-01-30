using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.PSBackupFiles.Verbose
{
    public class VerboseHelper
    {
        static VerboseHelper()
        {
            WriteVerboseStatic = (s) =>
            {
                if (IsVerbose)
                {
                    WriteVerboseStatic(s);
                }
            };
        }

        public static Action<String> WriteVerboseStatic;

        public static bool IsVerbose { get; set; }

        public static void SetVerbose(bool b)
        {
            IsVerbose = b;
        }
    }
}

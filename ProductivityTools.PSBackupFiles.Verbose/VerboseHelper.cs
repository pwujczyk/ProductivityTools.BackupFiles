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
            //WriteVerboseStatic = (s) =>
            //{
            //    if (IsVerbose)
            //    {
            //        WriteVerboseStatic(s);
            //    }
            //};
            Level = VerbosityLevel.Detailed;
            WriteVerboseStatic = (s) => Console.WriteLine(s);
        }

        private static Action<String> WriteVerboseStatic;
        public static VerbosityLevel Level;

        public static void WriteVerbose(VerbosityLevel level, string s)
        {
            if (IsVerbose && ((int)level)<=((int)Level))
            {
                WriteVerboseStatic(s);
            }
        }

        public static bool IsVerbose { get; set; }

        public static void SetVerbose(bool b)
        {
            IsVerbose = b;
        }

        public static void SetVerbosityOutput(Action<String> a)
        {
            WriteVerboseStatic = a;
        }

        public static void SetVerboseLevel(VerbosityLevel level)
        {
            Level = level;
        }
    }
}

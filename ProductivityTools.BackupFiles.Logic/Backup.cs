using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    public class Backup
    {
        private void GetDirectories(string directory)
        {
            string[] filePaths = Directory.GetDirectories(directory, "*", SearchOption.TopDirectoryOnly);
            foreach (var filePath in filePaths)
            {
                FindBackupDirectories(filePath);
            }
        }

        private void GetFile(string directory)
        {
            var x=Directory.GetFiles(directory, ".powershell", SearchOption.TopDirectoryOnly);
            foreach(var xxx in x)
            {
                Console.WriteLine("XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX" + xxx);
            }
        }

        public void FindBackupDirectories(string directory)
        {
            // Console.WriteLine(directory);


            
            //System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(directory);
            //if ((di.Attributes & FileAttributes.System) == FileAttributes.System
            //    && di.FullName != di.Root.FullName
            //    )
            //{
            //     return;
            //}
            if (Access(directory))
            {
                GetDirectories(directory);
                GetFile(directory);
            }
            //var x2 = di.GetAccessControl(AccessControlSections.Access);
            //System.Security.AccessControl.DirectorySecurity ds = Directory.GetAccessControl(directory);



            //var collection = ds.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
            //foreach (FileSystemAccessRule rule in collection)
            //{
            //    if (rule.AccessControlType == AccessControlType.Allow)
            //    {
                    
            //        break;
            //    }
            //}



        }

        private bool Access(string directory)
        {
            try
            {
                Directory.GetAccessControl(directory);
            }
            catch (Exception)
            {

                return false;
            }
            return true;
        }
    }
}

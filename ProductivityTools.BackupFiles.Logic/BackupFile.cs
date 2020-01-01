using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static ProductivityTools.BackupFiles.Logic.ReflectionTools;

namespace ProductivityTools.BackupFiles.Logic
{
    public class BackupFile
    {
        private const string FileName = ".Backup";

        public object Direct { get; private set; }
        private string NodeNameBackup = "Backup";
        private string NodeNameMode = "Mode";
        private string NodeCopyStrategy = "CopyStrategy";

        public void CreateBackupFile(string directory)
        {
            Create(directory);
        }

        private void Create(string directory)
        {
            var properties=typeof(BackupConfig).GetProperties();
            foreach(var property in properties)
            {
               var s= ReflectionTools.GetEnumDescription(property);
            }

            IEnumerable<Attribute> attribs = ActionDescription.GetActionAttribute();

            var document = new XDocument();
            var mainNode = new XElement(NodeNameBackup);
            document.Add(mainNode);

            var modeElement = new XElement(NodeNameMode, "notDefined");
            mainNode.Add(modeElement);

            var comment = new XComment("Mode defines how copying files is performed. Possible options:");
            mainNode.Add(comment);

            foreach (ActionAttribute item in attribs)
            {
               // comment = new XComment($"{item.BackupMode} - {item.Description}");
                mainNode.Add(comment);
            }

            var copyStrategyElement = new XElement(NodeCopyStrategy, "notDefined");
            mainNode.Add(modeElement);


            string targetFile = Path.Combine(directory, FileName);
            document.Save(targetFile);
        }

        public BackupMode GetBackupMode(string directory)
        {
            var x = Directory.GetFiles(directory, FileName, SearchOption.TopDirectoryOnly).SingleOrDefault();
            if (x == null)
            {
                return BackupMode.NotDefined;
            }
            else
            {
                XDocument xdoc = XDocument.Load(x);
                string backupMode = (from mode in xdoc.Descendants(NodeNameMode)
                                     select mode.Value).SingleOrDefault();
                if (string.IsNullOrEmpty(backupMode))
                {
                    return BackupMode.NotDefined;
                }
                BackupMode modeEnum = (BackupMode)Enum.Parse(typeof(BackupMode), backupMode);
                return modeEnum;
            }
        }
    }
}

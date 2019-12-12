using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
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

        public void CreateBackupFile()
        {
            Create();
        }

        private void Create()
        {
            IEnumerable<Attribute> attribs=ActionDescription.GetActionAttribute();

            var document = new XDocument();
            var mainNode = new XElement("Backup");
            document.Add(mainNode);

            var modeElement = new XElement("Mode", "notDefined");
            mainNode.Add(modeElement);

            var comment = new XComment("Mode defines how copying files is performed. Possible options:");
            mainNode.Add(comment);

            foreach (ActionAttribute item in attribs)
            {
                comment=new XComment($"{item.BackupMode} - {item.Description}");
                mainNode.Add(comment);
            }

            document.Save(FileName);
        }
    }
}

using ProductivityTools.BackupFiles.Logic.Modes;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace ProductivityTools.BackupFiles.Logic
{
    public class BackupFile
    {
        private const string FileName = ".Backup";

        public object Direct { get; private set; }
        private string NodeNameBackup = "Backup";
        private string NodeNameMode = "Mode";
        private string NodeNameCopyStrategy = "CopyStrategy";
        private string NodeRedundantItems = "RedundantItems";
        

        public void CreateBackupFile(string directory)
        {
            Create(directory);
        }

        private void Create(string directory)
        {
            XComment comment;
            var document = new XDocument();
            var mainNode = new XElement(NodeNameBackup);
            document.Add(mainNode);


            var properties = typeof(BackupConfig).GetProperties();
            foreach (var property in properties)
            {
                var s1 = $"{property.Name} - {property.GetPropertyDescription()}";
                comment = new XComment(s1);
                mainNode.Add(comment);

                comment = new XComment("Options:");
                mainNode.Add(comment);

                var enums = property.PropertyType.GetEnumValues();
                string defaultValue = string.Empty;
                foreach (var @enum in enums)
                {
                    var s = ReflectionTools.GetEnumDescription(@enum);
                    

                    if (ReflectionTools.GetDefault(@enum))
                    {
                        defaultValue = @enum.ToString();
                        comment = new XComment($"{@enum} - {s} [Default]");
                    }
                    else
                    {
                        comment = new XComment($"{@enum} - {s}");
                    }

                    
                    mainNode.Add(comment);
                }

                var modeElement2 = new XElement(property.Name, defaultValue);
                mainNode.Add(modeElement2);

            }

            string targetFile = Path.Combine(directory, FileName);
            document.Save(targetFile);

            IEnumerable<Attribute> attribs = ActionDescription.GetActionAttribute();
        }

        //public BackupMode GetBackupMode(string directory)
        //{
        //    var x = Directory.GetFiles(directory, FileName, SearchOption.TopDirectoryOnly).SingleOrDefault();
        //    if (x == null)
        //    {
        //        return BackupMode.NotDefined;
        //    }
        //    else
        //    {
        //        XDocument xdoc = XDocument.Load(x);
        //        string backupMode = (from mode in xdoc.Descendants(NodeNameMode)
        //                             select mode.Value).SingleOrDefault();
        //        if (string.IsNullOrEmpty(backupMode))
        //        {
        //            return BackupMode.NotDefined;
        //        }
        //        BackupMode modeEnum = (BackupMode)Enum.Parse(typeof(BackupMode), backupMode);
        //        return modeEnum;
        //    }
        //}

        private T ParseEnum<T>(XDocument xdoc, string nodeName) 
        {
            T result = default(T);
            string backupMode = (from mode in xdoc.Descendants(nodeName)
                                 select mode.Value).SingleOrDefault();
            if (!string.IsNullOrEmpty(backupMode))
            {
                    result = (T)Enum.Parse(typeof(T), backupMode);
                
            }
            return result;
        }

        public BackupConfig GetBackupConfig(string directory)
        {
            var result = new BackupConfig();
            var x = Directory.GetFiles(directory, FileName, SearchOption.TopDirectoryOnly).SingleOrDefault();
            if (x == null)
            {
                return null;
            }
            else
            {
                try
                {
                    XDocument xdoc = XDocument.Load(x);
                    result.Mode = ParseEnum<BackupMode>(xdoc, NodeNameMode);
                    result.CopyStrategy = ParseEnum<CopyStrategyMode>(xdoc, NodeNameCopyStrategy);
                    result.RedundantItems = ParseEnum<RedundantItemsMode>(xdoc, NodeRedundantItems);

                }
                catch (Exception)
                {
                    Console.WriteLine($"File {x} cound not be parsed correctly");
                }
                
                //string backupMode = (from mode in xdoc.Descendants(NodeNameMode)
                //                     select mode.Value).SingleOrDefault();
                //if (!string.IsNullOrEmpty(backupMode))
                //{
                //    result.Mode = (BackupMode)Enum.Parse(typeof(BackupMode), backupMode);
                //}
            }
            return result;
        }
    }
}

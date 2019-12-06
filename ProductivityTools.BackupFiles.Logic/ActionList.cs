using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class ActionList
    {
        private Dictionary<string, Action<string>> CurrentWork = new Dictionary<string, Action<string>>();

        public void Add(string key, Action<string> a)
        {
            this.CurrentWork.Add(key, a);
        }

        public bool Contains(string path)
        {
            foreach (var item in this.CurrentWork)
            {
                if (path.StartsWith(item.Key))
                {
                    return true;
                }
            }
            return false;
        }

        public void InvokeForPath(string path)
        {
            foreach (var item in this.CurrentWork)
            {
                if (path.StartsWith(item.Key))
                {
                    item.Value.Invoke(path);
                }
            }
        }
    }
}

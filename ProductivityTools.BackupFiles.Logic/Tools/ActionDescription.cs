using ProductivityTools.BackupFiles.Logic.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ProductivityTools.BackupFiles.Logic.ReflectionTools;

namespace ProductivityTools.BackupFiles.Logic.Tools
{
    public class ActionDescription
    {
        public static List<ActionAttribute> GetActionAttribute()
        {
            List<ActionAttribute> result = new List<ActionAttribute>();

            IEnumerable<Type> actions = GetEnumerableOfType<BaseAction>();
            foreach (Type type in actions)
            {
                var attribute = GetActionAttribute(type);
                if (attribute != null)
                {
                    result.Add(attribute);
                }
            }
            return result;
        }

        public static ActionAttribute GetActionAttribute(Type t)
        {
            var dnAttribute = t.GetCustomAttributes(typeof(ActionAttribute), true).FirstOrDefault() as ActionAttribute;
            if (dnAttribute != null)
            {
                return dnAttribute;
            }
            return null;
        }
    }
}

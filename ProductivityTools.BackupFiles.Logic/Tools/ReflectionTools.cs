using ProductivityTools.BackupFiles.Logic.Actions;
using ProductivityTools.BackupFiles.Logic.CopyStrategy;
using ProductivityTools.BackupFiles.Logic.Tools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class ReflectionTools
    {
        public static IEnumerable<Type> GetEnumerableOfType<T>() where T : class
        {
            List<Type> objects = new List<Type>();
            foreach (Type type in
                Assembly.GetAssembly(typeof(T)).GetTypes()
                .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(T))))
            {
                objects.Add(type);
            }
            return objects;
        }

        public static BaseAction CreateInstanceOfActionFromEnum(BackupMode mode, CopyStrategyBase copyStrategy)
        {
            IEnumerable<Type> actions = GetEnumerableOfType<BaseAction>();
            foreach (Type type in actions)
            {
                var attribute = ActionDescription.GetActionAttribute(type);
                if (attribute != null)
                {
                    if (attribute.BackupMode == mode)
                    {
                        var action = (BaseAction)Activator.CreateInstance(type, copyStrategy);
                        return action;
                    }
                }
            }
            return null;
        }

        public static string GetEnumDescription<T>(T value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}

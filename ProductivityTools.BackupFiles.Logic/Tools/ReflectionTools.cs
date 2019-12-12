﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ProductivityTools.BackupFiles.Logic
{
    class ReflectionTools
    {
        public static class ReflectiveEnumerator
        {
            static ReflectiveEnumerator() { }

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
        }
    }
}

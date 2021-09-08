using System;
using System.Collections.Generic;
using System.Linq;
namespace ETModel
{
    static class ReflectionHelper
    {
        public static IEnumerable<T> CreateAllInstancesOf<T>()
        { 
            return typeof(ReflectionHelper).Assembly.GetTypes()
                .Where(t => typeof(T).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract && t.IsClass)
                .Select(t => (T)Activator.CreateInstance(t));
        }


        public static IEnumerable<string>CreateAllIOperateName()
        {
            var types = typeof(ReflectionHelper).Assembly.GetTypes();
            List<string> result = new List<string>();
            foreach (Type item in types)
            {
                var attributes = item.GetCustomAttributes(typeof(OperateNameAttribute), false);
                if (attributes.Length == 0)
                    continue; 
                result.Add(attributes.OfType<OperateNameAttribute>().FirstOrDefault().OperateName);
            }
            return result;
        }
    }
}
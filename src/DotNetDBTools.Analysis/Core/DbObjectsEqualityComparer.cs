using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNetDBTools.Models.Core;

namespace DotNetDBTools.Analysis.Core
{
    public class DbObjectsEqualityComparer : IEqualityComparer<DBObjectInfo>
    {
        public bool Equals(DBObjectInfo x, DBObjectInfo y)
        {
            return !HaveDifferentPropertiesRecursive(x, y);
        }

        public int GetHashCode(DBObjectInfo obj)
        {
            return obj.GetHashCode();
        }

        private static bool HaveDifferentPropertiesRecursive<T>(T first, T second)
            where T : class
        {
            if (first is null && second is null)
                return false;
            else if (first is null || second is null)
                return true;
            Type parentType = first?.GetType() ?? second.GetType();

            bool res = false;
            foreach (PropertyInfo property in parentType
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x => x.CanRead && x.Name != "SyncRoot" && x.GetIndexParameters().Length == 0))
            {
                object value1 = property.GetValue(first, null);
                object value2 = property.GetValue(second, null);

                if (property.PropertyType == typeof(string) ||
                    property.PropertyType == typeof(Guid) ||
                    property.PropertyType.IsValueType ||
                    property.PropertyType.IsPrimitive)
                {
                    res = res || ObjectsAreDifferent(value1, value2);
                }
                else
                {
                    if (value1 is null && value2 is null)
                        continue;
                    res = res || HaveDifferentPropertiesRecursive(value1, value2);
                }
            }
            return res;

            bool ObjectsAreDifferent(object obj1, object obj2)
            {
                if (obj1 is null && obj2 is null)
                    return false;
                else if (obj1 is null || obj2 is null)
                    return true;
                else
                    return !obj1.Equals(obj2);
            }
        }
    }
}

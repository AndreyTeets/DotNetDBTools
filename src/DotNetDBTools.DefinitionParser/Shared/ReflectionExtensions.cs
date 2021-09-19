using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.DefinitionParser.Shared
{
    public static class ReflectionExtensions
    {
        public static IEnumerable<MemberInfo> GetPropertyOrFieldMembers(this Type type)
        {
            return type.GetFields()
                .Cast<MemberInfo>()
                .Concat(type.GetProperties());
        }

        public static Type GetPropertyOrFieldType(this MemberInfo memberInfo)
        {
            if (memberInfo is FieldInfo fi)
                return fi.FieldType;
            if (memberInfo is PropertyInfo pi)
                return pi.PropertyType;
            throw new InvalidOperationException($"Invalid {nameof(memberInfo)} type: {memberInfo.GetType()}");
        }

        public static object GetPropertyOrFieldValue(this MemberInfo memberInfo, object obj)
        {
            if (memberInfo is FieldInfo fi)
                return fi.GetValue(obj);
            if (memberInfo is PropertyInfo pi)
                return pi.GetValue(obj);
            throw new InvalidOperationException($"Invalid {nameof(memberInfo)} type: {memberInfo.GetType()}");
        }
    }
}

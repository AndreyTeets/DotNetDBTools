using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotNetDBTools.Deploy.Core
{
    internal static class InstanceCreator
    {
        public static T Create<T>(params object[] args)
        {
            ConstructorInfo constructor = typeof(T).GetConstructors().Single();
            int optionalParametersCount = constructor.GetParameters().Length - args.Length;
            if (optionalParametersCount > 0)
            {
                List<object> argsList = args.ToList();
                for (int i = 0; i < optionalParametersCount; i++)
                    argsList.Add(Type.Missing);
                args = argsList.ToArray();
            }
            return (T)constructor.Invoke(args);
        }
    }
}

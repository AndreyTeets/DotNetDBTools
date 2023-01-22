using System;
using System.Collections.Generic;
using System.Reflection;

namespace DotNetDBTools.Models;

public static class ExtensionMethods
{
    private static readonly MethodInfo s_cloneMethod = typeof(object).GetMethod(
        "MemberwiseClone",
        BindingFlags.NonPublic | BindingFlags.Instance);

    /// <summary>
    /// Creates a deep copy of the provided object.
    /// </summary>
    public static T CopyModel<T>(this T original)
    {
        return (T)CopyModel((object)original);
    }

    private static object CopyModel(this object originalObject)
    {
        return InternalCopy(originalObject, new Dictionary<object, object>(new ReferenceEqualityComparer()));
    }

    private static object InternalCopy(object originalObject, IDictionary<object, object> visited)
    {
        if (originalObject is null)
            return null;
        Type typeToReflect = originalObject.GetType();
        if (IsPrimitive(typeToReflect))
            return originalObject;
        if (visited.ContainsKey(originalObject))
            return visited[originalObject];
        if (typeof(Delegate).IsAssignableFrom(typeToReflect))
            return null;

        object cloneObject = s_cloneMethod.Invoke(originalObject, null);
        if (typeToReflect.IsArray)
        {
            Type arrayType = typeToReflect.GetElementType();
            if (!IsPrimitive(arrayType))
            {
                Array clonedArray = (Array)cloneObject;
                clonedArray.ForEach((array, indices) => array.SetValue(
                    InternalCopy(clonedArray.GetValue(indices), visited),
                    indices));
            }
        }
        visited.Add(originalObject, cloneObject);
        CopyFields(originalObject, visited, cloneObject, typeToReflect);
        RecursiveCopyBaseTypePrivateFields(originalObject, visited, cloneObject, typeToReflect);
        return cloneObject;
    }

    private static void ForEach(this Array array, Action<Array, int[]> action)
    {
        if (array.LongLength == 0)
            return;
        ArrayWalker arrayWalker = new(array);
        do
            action(array, arrayWalker.Position);
        while (arrayWalker.Step());
    }

    private static void RecursiveCopyBaseTypePrivateFields(
        object originalObject,
        IDictionary<object, object> visited,
        object cloneObject,
        Type typeToReflect)
    {
        if (typeToReflect.BaseType is not null)
        {
            RecursiveCopyBaseTypePrivateFields(
                originalObject,
                visited,
                cloneObject,
                typeToReflect.BaseType);
            CopyFields(
                originalObject,
                visited,
                cloneObject,
                typeToReflect.BaseType,
                BindingFlags.Instance | BindingFlags.NonPublic,
                info => info.IsPrivate);
        }
    }

    private static void CopyFields(
        object originalObject,
        IDictionary<object, object> visited,
        object cloneObject,
        Type typeToReflect,
        BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.FlattenHierarchy,
        Func<FieldInfo, bool> filter = null)
    {
        foreach (FieldInfo fieldInfo in typeToReflect.GetFields(bindingFlags))
        {
            if (filter is not null && filter(fieldInfo) == false)
                continue;
            if (IsPrimitive(fieldInfo.FieldType))
                continue;
            object originalFieldValue = fieldInfo.GetValue(originalObject);
            object clonedFieldValue = InternalCopy(originalFieldValue, visited);
            fieldInfo.SetValue(cloneObject, clonedFieldValue);
        }
    }

    private static bool IsPrimitive(this Type type)
    {
        if (type == typeof(string))
            return true;
        return type.IsValueType & type.IsPrimitive;
    }

    private class ReferenceEqualityComparer : EqualityComparer<object>
    {
        public override bool Equals(object x, object y)
        {
            return ReferenceEquals(x, y);
        }
        public override int GetHashCode(object obj)
        {
            if (obj is null)
                return 0;
            return obj.GetHashCode();
        }
    }

    private class ArrayWalker
    {
        public int[] Position;
        private readonly int[] _maxLengths;

        public ArrayWalker(Array array)
        {
            _maxLengths = new int[array.Rank];
            for (int i = 0; i < array.Rank; ++i)
                _maxLengths[i] = array.GetLength(i) - 1;
            Position = new int[array.Rank];
        }

        public bool Step()
        {
            for (int i = 0; i < Position.Length; ++i)
            {
                if (Position[i] < _maxLengths[i])
                {
                    Position[i]++;
                    for (int j = 0; j < i; j++)
                        Position[j] = 0;
                    return true;
                }
            }
            return false;
        }
    }
}

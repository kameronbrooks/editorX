using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace EditorX
{
    public static class TypeUtility
    {
        public static System.Type GetTypeByName(string typeName)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i += 1)
            {
                System.Type type = assemblies[i].GetType(typeName);
                if (type != null) return type;
            }
            return null;
        }

        public static System.Type GetArrayType(System.Type type)
        {
            return GetArrayOfType(type, 0).GetType();
        }

        public static System.Type GetArrayType(string typeName)
        {
            return GetArrayOfType(typeName, 0).GetType();
        }

        public static IList GetArrayOfType(System.Type type, int length = 0)
        {
            return System.Array.CreateInstance(type, length);
        }

        public static IList GetArrayOfType(string typeName, int length = 0)
        {
            System.Type type = GetTypeByName(typeName);
            if (type == null) throw new System.Exception("No type found: " + typeName);
            return System.Array.CreateInstance(type, length);
        }
    }
}
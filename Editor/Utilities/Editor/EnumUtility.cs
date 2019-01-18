using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace EditorX
{
    public static class EnumUtility
    {
        public static System.Type GetEnumType(string enumName)
        {
            Assembly[] assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            for (int i = 0; i < assemblies.Length; i += 1)
            {
                System.Type type = assemblies[i].GetType(enumName);
                if (type != null) return type;
            }
            return null;
        }
        public static System.Enum GetEnumObject(System.Type enumType, string name)
        {
            return (System.Enum)System.Enum.Parse(enumType, name);
        }
        public static System.Enum GetDefaultEnum(System.Type enumType)
        {
            string[] names =  System.Enum.GetNames(enumType);
            return (System.Enum)System.Enum.Parse(enumType, names[0]);
        }
    }
}

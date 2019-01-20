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
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public static class VectorUtility
    {
        public static Vector2 ReadVector2(string text)
        {
            text = text.Replace(", ", " ");
            string[] elements = text.Split(',', ' ');

            if (elements == null || elements.Length < 2) throw new System.Exception("Cannot read vector from text");
            return new Vector2(float.Parse(elements[0]), float.Parse(elements[1]));
        }
        public static Vector3 ReadVector3(string text)
        {
            text = text.Replace(", ", " ");
            string[] elements = text.Split(',', ' ');

            if (elements == null || elements.Length < 3) throw new System.Exception("Cannot read vector from text");
            return new Vector3(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]));
        }
        public static Vector4 ReadVector4(string text)
        {
            text = text.Replace(", ", " ");
            string[] elements = text.Split(',', ' ');

            if (elements == null || elements.Length < 4) throw new System.Exception("Cannot read vector from text");
            return new Vector4(float.Parse(elements[0]), float.Parse(elements[1]), float.Parse(elements[2]), float.Parse(elements[3]));
        }
    }
}

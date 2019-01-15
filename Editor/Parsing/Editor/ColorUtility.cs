using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace EditorX
{
    public static class ColorUtility
    {

        public static Color ReadColor(string src)
        {
            if (src[0] == '#')
            {
                int ri = int.Parse(src.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                int gi = int.Parse(src.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                int bi = int.Parse(src.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                int ai = 255;
                if (src.Length > 8)
                {
                    ai = int.Parse(src.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                }

                return new Color((float)ri / 255.0f, (float)gi / 255.0f, (float)bi / 255.0f, (float)ai / 255.0f);
            }
            PropertyInfo prop = typeof(Color).GetProperty(src);

            if(prop != null)
            {
                return (Color)prop.GetValue(null, null);
            }

            Debug.LogWarning("no color can be read from data: " + src);
            return Color.black;
        }
    }

}
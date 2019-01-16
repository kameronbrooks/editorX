using UnityEngine;

namespace EditorX
{
    public static class RectUtility
    {
        public static RectOffset ReadRectOffset(string src)
        {
            string[] elements = src.Split(' ', ',');
            int[] ints = new int[4];
            for (int i = 0; i < ints.Length; i += 1)
            {
                if (i < elements.Length) ints[i] = int.Parse(elements[i]);
                else ints[i] = 0;
            }

            return new RectOffset(ints[0], ints[1], ints[2], ints[3]);
        }

        public static Rect ReadRect(string src)
        {
            string[] elements = src.Split(' ', ',');
            float[] ints = new float[4];
            for (int i = 0; i < ints.Length; i += 1)
            {
                if (i < elements.Length) ints[i] = float.Parse(elements[i]);
                else ints[i] = 0;
            }

            return new Rect(ints[0], ints[1], ints[2], ints[3]);
        }
    }
}
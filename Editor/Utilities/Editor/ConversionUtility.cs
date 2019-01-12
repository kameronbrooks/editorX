using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public static class ConversionUtility
    {
        public static TextAnchor GetTextAnchor(string str)
        {
            switch (str)
            {
                case "lower-center":
                    return TextAnchor.LowerCenter;
                case "lower-left":
                    return TextAnchor.LowerLeft;
                case "lower-right":
                    return TextAnchor.LowerRight;
                case "middle-center":
                    return TextAnchor.MiddleCenter;
                case "middle-left":
                    return TextAnchor.MiddleLeft;
                case "middle-right":
                    return TextAnchor.MiddleRight;
                case "upper-center":
                    return TextAnchor.UpperCenter;
                case "upper-left":
                    return TextAnchor.UpperLeft;
                case "upper-right":
                    return TextAnchor.UpperRight;
                default:
                    throw new System.Exception("The string " + str + " is not the name of a text anchor");
            }
        }
        public static ImagePosition GetImagePosition(string str)
        {
            switch (str)
            {
                case "above":
                    return ImagePosition.ImageAbove;
                case "left":
                    return ImagePosition.ImageLeft;
                case "only":
                    return ImagePosition.ImageOnly;
                case "text":
                    return ImagePosition.TextOnly;
                default:
                    throw new System.Exception("The string " + str + " is not the name of an image position");
            }
        }
    }
}

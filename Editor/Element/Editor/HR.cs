using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class HR : Element
    {
        protected override void InitializeGUIStyle()
        {
            if(style.guistyle == null)
            {
                style.guistyle = GUI.skin.label;
            }
        }
        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            _rect = EditorGUILayout.GetControlRect(GUILayout.ExpandWidth(true), GUILayout.Height(1));
            EditorGUI.DrawRect(_rect, style.color);
        }
        protected override void PostGUI()
        {

        }
        public override Element AddChild(Element child)
        {
            Debug.LogWarning("Cannot add children to elements of type " + tag);
            return null;
        }
    }
}

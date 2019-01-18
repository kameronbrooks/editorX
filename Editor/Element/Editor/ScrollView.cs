using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class ScrollView : Element
    {
        [SerializeField]
        Vector2 _scrollPos;

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) style.guistyle = new GUIStyle(GUI.skin.scrollView);
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, style.guistyle, style.layoutOptions);

            DrawChildren();

            EditorGUILayout.EndScrollView();
        }
        protected override void PostGUI()
        {

        }
    }

}
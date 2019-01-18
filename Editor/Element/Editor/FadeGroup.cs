using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

namespace EditorX
{
    public class FadeGroup : Element
    {
        AnimBool _animBool;
        protected override void InitializeGUIStyle()
        {

        }

        protected override void OnGUI()
        {

            if (_animBool == null)
            {
                _animBool = new AnimBool();
                _animBool.valueChanged.AddListener(RequestRepaint);
            }

            _rect = EditorGUILayout.BeginVertical();
            if (style.backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(_rect, style.backgroundColor);
            }

            _animBool.target = EditorGUILayout.ToggleLeft("show", _animBool.target);
            if(EditorGUILayout.BeginFadeGroup(_animBool.faded))
            {
                DrawChildren();
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
        }

        protected override void PostGUI()
        {

        }

        protected override void PreGUI()
        {

        }
    }
}

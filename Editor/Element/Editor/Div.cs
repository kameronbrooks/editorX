﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public enum LayoutType { Horizontal, Vertical }

    [System.Serializable]
    public class Div : Element
    {
        [SerializeField]
        LayoutType _layoutType;


        public override string tag
        {
            get
            {
                return "div";
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = GUI.skin.box;
        }

        protected override void PreGUI()
        {
            Debug.Log("");
        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            if (_layoutType == LayoutType.Horizontal)
            {
                _rect = EditorGUILayout.BeginHorizontal(style.layoutOptions);
            } else
            {
                _rect = EditorGUILayout.BeginVertical(style.layoutOptions);
            }

            if(_backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(_rect, _backgroundColor);
            }
            DrawChildren();

            if (_layoutType == LayoutType.Horizontal)
            {
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.EndVertical();
            }

        }
        protected override void PostGUI()
        {

        }

    }

}
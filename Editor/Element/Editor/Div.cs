using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public enum LayoutType { Horizontal, Vertical }

    public class Div : Element
    {
        LayoutType _layoutType;


        public override string tag
        {
            get
            {
                return "div";
            }
        }

        public Div(string name, LayoutType layoutType = LayoutType.Horizontal, Style style = null) : base(name, style)
        {
            _layoutType = layoutType;
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
            GUI.SetNextControlName(_name);
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

        protected override SerializedElement ToSerialized()
        {
            return null;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            return;
        }
    }

}
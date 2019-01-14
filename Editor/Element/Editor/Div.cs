using System.Collections;
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
        }
        protected override void OnGUI()
        {
            if(name != null && name != "") GUI.SetNextControlName(name);
            if (_layoutType == LayoutType.Horizontal)
            {
                _rect = EditorGUILayout.BeginHorizontal(style.layoutOptions);
            } else
            {
                _rect = EditorGUILayout.BeginVertical(style.layoutOptions);
            }

            if(style.backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(_rect, style.backgroundColor);
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

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch(name)
            {
                case "layout-type":
                case "layouttype":
                    if(value.GetType() == typeof(LayoutType))
                    {
                        _layoutType = (LayoutType)value;
                    }
                    else
                    {
                        _layoutType = (LayoutType)System.Enum.Parse(typeof(LayoutType), value.ToString(), true);
                    }
                    
                    return true;

                default:
                    return false;

            }
        }
        public override object GetProperty(string name)
        {
            object result = base.GetProperty(name);

            if (result != null) return result;

            switch(name)
            {
                case "layout-type":
                case "layouttype":
                    result = _layoutType;
                    break;
                default:
                    break;
            }

            return result;
        }

    }

}
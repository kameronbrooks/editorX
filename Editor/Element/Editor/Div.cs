using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public enum LayoutType { Horizontal, Vertical }

    [System.Serializable]
    public class Div : Element
    {
        [SerializeField]
        private LayoutType _layoutType;

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                this.style.guistyle = new GUIStyle(GUI.skin.box);
                this.style.guistyle.normal.background = null;
                if (parent != null) this.style.CopyTextProperties(parent.style);
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            //if (name != null && name != "") GUI.SetNextControlName(name);
            if (style.position == PositionType.Layout)
            {
                if (_layoutType == LayoutType.Horizontal)
                {
                    _rect = EditorGUILayout.BeginHorizontal(style.guistyle, style.layoutOptions);
                }
                else
                {
                    _rect = EditorGUILayout.BeginVertical(style.guistyle, style.layoutOptions);
                }

                if (style.backgroundColor != Color.clear)
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
            else
            {
                _rect = style.GetRect(parent.rect);
                GUI.BeginGroup(_rect, style.guistyle);
                if (style.backgroundColor != Color.clear)
                {
                    EditorGUI.DrawRect(_rect, style.backgroundColor);
                }
                DrawChildren();

                GUI.EndGroup();
            }
            
            
        }


        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "layout-type":
                case "layouttype":
                    if (value.GetType() == typeof(LayoutType))
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

            switch (name)
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
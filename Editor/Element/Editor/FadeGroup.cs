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

        [SerializeField]
        protected GUIContent _label;

        public void SetLabel(GUIContent content)
        {
            _label = content;
        }

        public void SetLabel(string content)
        {
            if (content == "" || content == null)
            {
                _label = null;
            }
            else
            {
                _label = new GUIContent(content);
            }
        }

        public void SetLabel(string content, string tooltip)
        {
            if (content == "" || content == null)
            {
                _label = null;
            }
            else
            {
                _label = new GUIContent(content, tooltip);
            }
        }

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
            if (_label == null) _label = GUIContent.none;

            _rect = EditorGUILayout.BeginVertical();
            if (style.backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(_rect, style.backgroundColor);
            }

            _animBool.target = EditorGUILayout.ToggleLeft(_label, _animBool.target);
            if(EditorGUILayout.BeginFadeGroup(_animBool.faded))
            {
                DrawChildren();
            }
            EditorGUILayout.EndFadeGroup();
            EditorGUILayout.EndVertical();
        }

        protected override void PreGUI()
        {

        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "label":
                    SetLabel(value.ToString());
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
                case "label":
                    result = _label;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}

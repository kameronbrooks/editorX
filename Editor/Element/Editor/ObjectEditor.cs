using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    [System.Serializable]
    public class ObjectEditor : ObjectField
    {
        [SerializeField]
        Editor _editor;
        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                this.style.guistyle = new GUIStyle(GUI.skin.box);
                this.style.guistyle.normal.background = null;
                if (parent != null) this.style.CopyTextProperties(parent.style);
            }
        }

        protected override void OnChange()
        {
            base.OnChange();
            if(_value == null)
            {
                _editor = null;
            } else
            {
                _editor = Editor.CreateEditor(_value);
            }
        }

        protected override void OnGUI()
        {
            EditorGUILayout.BeginVertical(style.layoutOptions);
            base.OnGUI();
            if(_editor != null)
            {
                _editor.DrawDefaultInspector();
            }
            EditorGUILayout.EndVertical();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public abstract class ValueElement : Element
    {
        [SerializeField]
        protected GUIContent _label;

        public GUIContent label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public void SetLabel(GUIContent content)
        {
            _label = content;
        }
        public void SetLabel(string content)
        {
            if(content == "" || content == null)
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

        public abstract T GetValue<T>();

        public abstract object GetValue();

        public abstract void SetValue(object val);

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = new GUIStyle(GUI.skin.textField);
        }
    }
}

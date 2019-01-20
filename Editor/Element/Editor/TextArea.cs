using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TextArea : ValueElement
    {
        [SerializeField]
        string _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override T GetValue<T>()
        {
            return (T)(object)_value;
        }

        public override object GetValue()
        {
            return _value;
        }

        public override void SetValue(object val)
        {
            _value = (string)val;
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) style.guistyle = GUI.skin.textArea;
        }

        protected override void PreGUI()
        {

        }

        protected override void OnGUI()
        {
            Color oldColor = GUI.skin.settings.cursorColor;
            GUI.skin.settings.cursorColor = Color.clear;
            if (name != null && name != "") GUI.SetNextControlName(name);
            string temp = EditorGUILayout.TextArea(_value, style.guistyle, style.layoutOptions);


            GUI.skin.settings.cursorColor = oldColor;
            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":
                case "innertext":
                    _value = value.ToString();
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
                case "value":
                    result = _value;
                    break;

                default:
                    break;
            }

            return result;
        }

        public override Element AddChild(Element child)
        {
            if(child.tag == "textnode")
            {
                _value = ((TextNode)child).text;
                return this;
            }
            else
            {
                return base.AddChild(child);
            }
        }
    }

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class Toggle : ValueElement
    {
        [SerializeField]
        bool _value;
        [SerializeField]
        bool _left;

        public override System.Type valueType
        {
            get
            {
                return typeof(bool);
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) style.guistyle = GUI.skin.toggle;
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
            _value = (bool)val;
        }

        protected override void PreGUI()
        {

        }

        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            bool temp = (_label != null) ?
                    ((_left) ? EditorGUILayout.ToggleLeft(_label, _value, style.guistyle, style.layoutOptions) : EditorGUILayout.Toggle(_label, _value, style.guistyle, style.layoutOptions)) :
                    EditorGUILayout.Toggle(_value, style.guistyle, style.layoutOptions);

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
                    _value = (value.GetType() == typeof(bool)) ? (bool)value : bool.Parse(value.ToString());
                    return true;
                case "left":
                    _left = (value.GetType() == typeof(bool)) ? (bool)value : bool.Parse(value.ToString());
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
                case "left":
                    result = _left;
                    break;

                default:
                    break;
            }

            return result;
        }
    }

}
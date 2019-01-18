using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TextField : ValueElement
    {
        [SerializeField]
        string _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(float);
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

        protected override void PreGUI()
        {

        }

        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            string temp = (_label != null) ?
                EditorGUILayout.TextField(_label, _value, style.guistyle, style.layoutOptions) :
                EditorGUILayout.TextField(_value, style.guistyle, style.layoutOptions);
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
    }

}
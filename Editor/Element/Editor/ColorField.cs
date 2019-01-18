using System;
using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class ColorField : ValueElement
    {
        [SerializeField]
        private Color _value;

        public override Type valueType
        {
            get
            {
                return typeof(Color);
            }
        }

        public override string tag
        {
            get
            {
                return "color";
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            Color temp = (_label != null) ?
                EditorGUILayout.ColorField(_label, _value, style.layoutOptions) :
                EditorGUILayout.ColorField(_value, style.layoutOptions);
            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
        }

        protected override void PostGUI()
        {
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":
                    _value = (value.GetType() == typeof(Color)) ? (Color)value : ColorUtility.ReadColor(value.ToString());
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
            _value = (Color)val;
        }
    }
}
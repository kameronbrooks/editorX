using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class FloatField : ValueElement
    {
        [SerializeField]
        private float _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(float);
            }
        }

        public static FloatField Create(string name, float value = 0)
        {
            FloatField field = ScriptableObject.CreateInstance<FloatField>();
            field.name = name;
            field._value = value;

            return field;
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            float temp = (_label != null) ?
                EditorGUILayout.FloatField(_label, _value, style.guistyle, style.layoutOptions) :
                EditorGUILayout.FloatField(_value, style.guistyle, style.layoutOptions);
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
                    _value = (value.GetType() == typeof(float)) ? (float)value : float.Parse(value.ToString());
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
            _value = (float)val;
        }
    }
}
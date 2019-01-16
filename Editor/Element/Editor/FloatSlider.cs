using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class FloatSlider : IntField
    {
        [SerializeField]
        private float _value;

        [SerializeField]
        private float _lvalue;

        [SerializeField]
        private float _rvalue;

        public static FloatSlider Create(string name, float value = 0, float lval = 0, float rval = 10)
        {
            FloatSlider field = ScriptableObject.CreateInstance<FloatSlider>();
            field.name = name;
            field._value = value;
            field._lvalue = lval;
            field._rvalue = rval;

            return field;
        }

        public override string tag
        {
            get
            {
                return "intslider";
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            float temp = (_label != null) ?
                EditorGUILayout.Slider(_label, _value, _lvalue, _rvalue, style.layoutOptions) :
                EditorGUILayout.Slider(_value, _lvalue, _rvalue, style.layoutOptions);
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

                case "rvalue":
                case "right-value":
                    _rvalue = (value.GetType() == typeof(float)) ? (float)value : float.Parse(value.ToString());
                    return true;

                case "lvalue":
                case "left-value":
                    _lvalue = (value.GetType() == typeof(float)) ? (float)value : float.Parse(value.ToString());
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

                case "rvalue":
                case "right-value":
                    result = _rvalue;
                    break;

                case "lvalue":
                case "left-value":
                    result = _lvalue;
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
            _value = (int)val;
        }
    }
}
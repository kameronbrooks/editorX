using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class FloatSlider : FloatField
    {
        [SerializeField]
        private float _lvalue;

        [SerializeField]
        private float _rvalue;

        public override string tag
        {
            get
            {
                return "floatslider";
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
                case "min":
                    _rvalue = (value.GetType() == typeof(float)) ? (float)value : float.Parse(value.ToString());
                    return true;

                case "lvalue":
                case "left-value":
                case "max":
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
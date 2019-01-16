using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class IntSlider : IntField
    {
        [SerializeField]
        private int _value;

        [SerializeField]
        private int _lvalue;

        [SerializeField]
        private int _rvalue;

        public static IntSlider Create(string name, int value = 0, int lval = 0, int rval = 10)
        {
            IntSlider field = ScriptableObject.CreateInstance<IntSlider>();
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
            int temp = (_label != null) ?
                EditorGUILayout.IntSlider(_label, _value, _lvalue, _rvalue, style.layoutOptions) :
                EditorGUILayout.IntSlider(_value, _lvalue, _rvalue, style.layoutOptions);
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
                    _value = (value.GetType() == typeof(int)) ? (int)value : int.Parse(value.ToString());
                    return true;

                case "rvalue":
                case "right-value":
                    _rvalue = (value.GetType() == typeof(int)) ? (int)value : int.Parse(value.ToString());
                    return true;

                case "lvalue":
                case "left-value":
                    _lvalue = (value.GetType() == typeof(int)) ? (int)value : int.Parse(value.ToString());
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
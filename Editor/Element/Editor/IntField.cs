using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class IntField : ValueElement
    {
        [SerializeField]
        protected int _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(int);
            }
        }

        public static IntField Create(string name, int value = 0)
        {
            IntField field = ScriptableObject.CreateInstance<IntField>();
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
            int temp = (_label != null) ?
                EditorGUILayout.IntField(_label, _value, style.guistyle, style.layoutOptions) :
                EditorGUILayout.IntField(_value, style.guistyle, style.layoutOptions);
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
                    _value = (value.GetType() == typeof(int)) ? (int)value : int.Parse(value.ToString());
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
            _value = (int)val;
        }
    }
}
using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class Vector2Field : ValueElement
    {
        [SerializeField]
        private Vector2 _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(float);
            }
        }

        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            Vector2 temp = (_label != null) ?
                EditorGUILayout.Vector2Field(_label, _value, style.layoutOptions) :
                EditorGUILayout.Vector2Field(GUIContent.none, _value, style.layoutOptions);
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
                    _value = (value.GetType() == typeof(Vector2)) ? (Vector2)value : VectorUtility.ReadVector2(value.ToString());
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
            _value = (Vector2)val;
        }
    }
}
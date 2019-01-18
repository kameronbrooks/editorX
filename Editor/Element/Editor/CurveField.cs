using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class CurveField : ValueElement
    {
        [SerializeField]
        private AnimationCurve _value;

        public override System.Type valueType
        {
            get
            {
                return typeof(AnimationCurve);
            }
        }

        public static CurveField Create(string name, AnimationCurve value = null)
        {
            CurveField field = ScriptableObject.CreateInstance<CurveField>();
            field.name = name;
            if(value != null) field._value = value;

            return field;
        }


        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {
            if (_value == null) _value = new AnimationCurve();
            if (name != null && name != "") GUI.SetNextControlName(name);
            AnimationCurve temp = (_label != null) ?
                EditorGUILayout.CurveField(_label, _value, style.layoutOptions) :
                EditorGUILayout.CurveField(_value, style.layoutOptions);
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
            _value = (AnimationCurve)val;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class FloatField : ValueElement
    {
        [SerializeField]
        float _value;

        public System.Type valueType
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

        

        public override string tag
        {
            get
            {
                return "float";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
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
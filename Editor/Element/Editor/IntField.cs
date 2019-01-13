using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class IntField : ValueElement
    {
        [SerializeField]
        int _value;

        public System.Type valueType
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



        public override string tag
        {
            get
            {
                return "int";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            if (name != null && name != "") GUI.SetNextControlName(name);
            int temp = (_label != null) ?
                EditorGUILayout.IntField(_label, _value, style.guistyle, style.layoutOptions):
                EditorGUILayout.IntField(_value, style.guistyle, style.layoutOptions);
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
            _value = (int)val;
        }

    }

}
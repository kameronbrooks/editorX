using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class IntSliderField : IntField
    {
        [SerializeField]
        int _value;
        [SerializeField]
        int _lvalue;
        [SerializeField]
        int _rvalue;


        public static IntSliderField Create(string name, int value = 0, int lval = 0, int rval = 10)
        {
            IntSliderField field = ScriptableObject.CreateInstance<IntSliderField>();
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
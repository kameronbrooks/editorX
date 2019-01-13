using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorX
{
    public class ColorField : ValueElement
    {
        Color _value;

        public Type valueType
        {
            get
            {
                return typeof(Color);
            }
        }

        public override string tag
        {
            get
            {
                return "color";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            Color temp = EditorGUILayout.ColorField(_value, style.layoutOptions);
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
            _value = (Color)val;
        }


    }
}
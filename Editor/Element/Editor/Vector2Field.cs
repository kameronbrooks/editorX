using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorX
{
    public class Vector2Field : Element, IValueElement
    {
        Vector2 _value;
        string _label;

        public Type valueType
        {
            get
            {
                return typeof(Vector2);
            }
        }

        public override string tag
        {
            get
            {
                return "vector2";
            }
        }

        public Vector2Field(string name, string label, Style style = null) : base(name, style)
        {
            _label = label;
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            Vector2 temp = EditorGUILayout.Vector2Field(_label, _value, style.layoutOptions);
            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
        }
        protected override void PostGUI()
        {

        }

        public T GetValue<T>()
        {
            return (T)(object)_value;
        }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object val)
        {
            _value = (Vector2)val;
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddValue(new SerializedElement.SerializedValue("val", _value));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._value = serial.GetValue("val").GetObject<Vector2>();
        }
    }
}

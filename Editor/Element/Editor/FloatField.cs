using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class FloatField : Element, IValueElement
    {
        float _value;

        public System.Type valueType
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
                return "float";
            }
        }

        public FloatField(string name, Style style = null) : base(name, style)
        {

        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            float temp = EditorGUILayout.FloatField(_value, style.layoutOptions);
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
            _value = (float)val;
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddValue(new SerializedElement.SerializedValue("val", _value));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._value = serial.GetValue("val").GetFloat();
        }
    }

}
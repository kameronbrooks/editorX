using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class IntField : Element, IValueElement
    {
        int _value;

        public System.Type valueType
        {
            get
            {
                return typeof(Color);
            }
        }

        public IntField(string name, Style style = null) : base(name, style)
        {
            
        }

        public override string tag
        {
            get
            {
                return "img";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            int temp = EditorGUILayout.IntField(_value, style.layoutOptions);
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
            _value = (int)val;
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddValue(new SerializedElement.SerializedValue("val", _value));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._value = serial.GetValue("val").GetInt();
        }
    }

}
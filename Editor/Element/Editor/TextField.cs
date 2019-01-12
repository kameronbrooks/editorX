using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorX
{
    [System.Serializable]
    public class TextField : Element, IValueElement
    {
        [SerializeField]
        string _value;

        public Type valueType
        {
            get
            {
                return typeof(string);
            }
        }

        public TextField(string name, Style style) : base(name, style)
        {

        }

        public override string tag
        {
            get
            {
                return "input";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            string temp = EditorGUILayout.TextField(_value, style.guistyle, style.layoutOptions);
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
            _value = (string)val;
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            if(_value != null && _value != "")
            {
                serial.AddValue(new SerializedElement.SerializedValue("val", _value));
            }          
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            var serialValue = serial.GetValue("val");
            if(serialValue != null)
            {
                this._value = serialValue.GetString();
            }
            
        }
    }

}
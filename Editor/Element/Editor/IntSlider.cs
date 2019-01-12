using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class IntSlider : Element, IValueElement
    {
        int _value;
        int _leftValue;
        int _rightValue;

        public System.Type valueType
        {
            get
            {
                return typeof(int);
            }
        }

        public override string tag
        {
            get
            {
                return "intslider";
            }
        }

        public int leftValue
        {
            get
            {
                return _leftValue;
            }

            set
            {
                _leftValue = value;
            }
        }

        public int rightValue
        {
            get
            {
                return _rightValue;
            }

            set
            {
                _rightValue = value;
            }
        }

        public IntSlider(string name, int lValue, int rValue, Style style = null) : base(name, style)
        {
            _leftValue = lValue;
            _rightValue = rValue;
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            int temp = EditorGUILayout.IntSlider(_value, _leftValue, _rightValue, style.layoutOptions);
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
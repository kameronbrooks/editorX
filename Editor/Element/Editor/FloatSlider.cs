using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class FloatSlider : Element, IValueElement
    {
        float _value;
        float _leftValue;
        float _rightValue;

        public System.Type valueType
        {
            get
            {
                return typeof(float);
            }
        }

        public override string tag
        {
            get
            {
                return "slider";
            }
        }

        public float leftValue
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

        public float rightValue
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

        public FloatSlider(string name, float lValue, float rValue, Style style = null) : base(name, style)
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
            float temp = EditorGUILayout.Slider(_value, _leftValue, _rightValue, style.layoutOptions);
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

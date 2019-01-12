using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class Vector3Field : Element, IValueElement
    {
        Vector3 _value;
        string _label;

        public System.Type valueType
        {
            get
            {
                return typeof(Vector3);
            }
        }

        public Vector3Field(string name, string label, Style style = null) : base(name, style)
        {
            _label = label;
        }

        public override string tag
        {
            get
            {
                return "vector3";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            Vector3 temp = EditorGUILayout.Vector3Field(_label, _value, style.layoutOptions);
            if (temp != _value)
            {
                _value = temp;
                CallEvent(ElementEvents.Change);
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
            _value = (Vector3)val;
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddValue(new SerializedElement.SerializedValue("val", _value));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._value = serial.GetValue("val").GetObject<Vector3>();
        }
    }
}

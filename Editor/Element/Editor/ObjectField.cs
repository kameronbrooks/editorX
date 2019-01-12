using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorX
{
    [System.Serializable]
    public class ObjectField<T> : Element, IValueElement where T : UnityEngine.Object
    {

        bool _allowSceneObjects = true;
        
        T _value;

        public Type valueType
        {
            get
            {
                return typeof(T);
            }
        }

        public override string tag
        {
            get
            {
                return "object";
            }
        }

        public ObjectField(string name, Style style = null) : base(name, style)
        {

        }
        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = GUI.skin.textField;
        }

        protected override void PreGUI()
        {
            InitializeGUIStyle();
        }
        protected override void OnGUI()
        {
            GUI.SetNextControlName(_name);
            T temp = (T)EditorGUILayout.ObjectField(_value, typeof(T), _allowSceneObjects, style.layoutOptions);
            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
        }
        protected override void PostGUI()
        {

        }

        public T1 GetValue<T1>()
        {
            return (T1)(object)_value;
        }

        public object GetValue()
        {
            return _value;
        }

        public void SetValue(object val)
        {
            _value = (T)val;
        }
        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddReference(new SerializedElement.SerializedObject("val", _value));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._value = (T)serial.GetReference("val");
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class ObjectField : ValueElement
    {
        [SerializeField]
        UnityEngine.Object _value;
        [SerializeField]
        string _typeName;
        System.Type _type;
        [SerializeField]
        bool _allowSceneObjects = true;

        public override string tag
        {
            get
            {
                return "object";
            }
        }

        public bool allowSceneObjects
        {
            get
            {
                return _allowSceneObjects;
            }
            set
            {
                _allowSceneObjects = value;
            }
        }

        public static ObjectField Create(string name, System.Type type)
        {
            ObjectField field = ScriptableObject.CreateInstance<ObjectField>();
            field.name = name;
            field._type = type;

            return field;
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
            _value = (UnityEngine.Object)val;
        }

        protected override void PreGUI()
        {

        }

        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            UnityEngine.Object temp = (_label != null) ?
                EditorGUILayout.ObjectField(_label, _value, _type, _allowSceneObjects, style.layoutOptions) :
                EditorGUILayout.ObjectField( _value, _type, _allowSceneObjects, style.layoutOptions);

            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
        }

        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            _typeName = _type.AssemblyQualifiedName;
        }
        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            _type = System.Type.GetType(_typeName);
        }
    }
}

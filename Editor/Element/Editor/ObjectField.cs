using System;
using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class ObjectField : ValueElement
    {
        [SerializeField]
        protected UnityEngine.Object _value;

        [SerializeField]
        protected string _typeName;

        private System.Type _type;

        [SerializeField]
        private bool _allowSceneObjects = true;

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

        public override Type valueType
        {
            get
            {
                return typeof(UnityEngine.Object);
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
                EditorGUILayout.ObjectField(_value, _type, _allowSceneObjects, style.layoutOptions);

            if (temp != _value)
            {
                _value = temp;
                OnChange();
            }
        }

        protected virtual void OnChange()
        {
            CallEvent("change");
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":
                case "path":
                    if (value as UnityEngine.Object)
                    {
                        _value = (UnityEngine.Object)value;
                    }
                    else
                    {
                        UnityEngine.Object temp = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(value.ToString());
                        if (temp != null)
                        {
                            _value = temp;
                        }
                        else
                        {
                            Debug.LogError("EditorX failed to load "+_type.Name+": No object located at " + value.ToString());
                        }
                    }
                    return true;
                case "type":
                    _type = (value.GetType() == typeof(System.Type)) ? (System.Type)value : TypeUtility.GetTypeByName(value.ToString());
                    return true;
                case "allowSceneObjects":
                    _allowSceneObjects = (value.GetType() == typeof(bool)) ? (bool)value : bool.Parse(value.ToString());
                    return true;
                default:
                    return false;
            }
        }

        public override object GetProperty(string name)
        {
            object result = base.GetProperty(name);

            if (result != null) return result;

            switch (name)
            {
                case "value":
                    result = _value;
                    break;
                case "type":
                    result = _type;
                    break;
                case "allowSceneObjects":
                    result = _allowSceneObjects;
                    break;
                default:
                    break;
            }

            return result;
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
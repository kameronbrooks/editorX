using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class ObjectField : ValueElement
    {
        [SerializeField]
        private UnityEngine.Object _value;

        [SerializeField]
        private string _typeName;

        private System.Type _type;

        [SerializeField]
        private bool _allowSceneObjects = true;

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
                EditorGUILayout.ObjectField(_value, _type, _allowSceneObjects, style.layoutOptions);

            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
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
                        Object temp = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(value.ToString());
                        if (temp != null)
                        {
                            _value = temp;
                        }
                        else
                        {
                            Debug.LogError("EditorX failed to load image: No object located at " + value.ToString());
                        }
                    }
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
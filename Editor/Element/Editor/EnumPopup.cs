using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class EnumPopup : ValueElement
    {
        [SerializeField]
        protected System.Enum _value;
        [SerializeField]
        protected string _enumTypeName;


        System.Type _enumType;
        System.Type enumType
        {
            get
            {
                if(_enumType == null || _enumType.Name != _enumTypeName)
                {
                    _enumType = EnumUtility.GetEnumType(_enumTypeName);


                   _value = EnumUtility.GetDefaultEnum(_enumType);
                }
                return _enumType;
            }
        }

        public override System.Type valueType
        {
            get
            {
                return enumType;
            }
        }

        protected override void HandleEvents()
        {

        }
        protected override void PreGUI()
        {
        }

        protected override void OnGUI()
        {

            if (name != null && name != "") GUI.SetNextControlName(name);
            
            System.Enum temp = (_label != null) ?
                EditorGUILayout.EnumPopup(_label, _value,  style.layoutOptions) :
                EditorGUILayout.EnumPopup(_value,  style.layoutOptions);
                
            if (temp != _value)
            {
                _value = temp;
                CallEvent("change");
            }
            
        }

        protected override void PostGUI()
        {
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":
                    _value = (value.GetType().IsEnum) ? (System.Enum)value : (System.Enum)System.Enum.Parse(enumType, value.ToString());
                    return true;
                case "typename":
                case "type":
                    _enumTypeName = value.ToString();
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
                case "typename":
                case "type":
                    result = _enumTypeName;
                    break;
                default:
                    break;
            }

            return result;
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
            _value = (System.Enum)val;
        }
    }
}
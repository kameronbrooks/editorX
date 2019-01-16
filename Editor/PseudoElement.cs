using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class PseudoElement<T>
    {
        private MethodInfo _method;
        private string _name;
        private Rect _rect;
        private T _value;
        private GUILayoutOption[] _guiLayoutOptions;
        private object[] _argumentBuffer;
        private GUIContent _label;

        public delegate void Callback(T val);

        public event Callback onChange;

        public PseudoElement(string elementName, string methodName, GUIContent label = null, params GUILayoutOption[] layoutOptions)
        {
            _method = typeof(EditorGUILayout).GetMethod(methodName);
            _name = elementName;
            _guiLayoutOptions = layoutOptions;
        }

        public object Draw()
        {
            _rect = EditorGUILayout.GetControlRect(_guiLayoutOptions);
            T temp = (T)_method.Invoke(null, _argumentBuffer);

            if (!temp.Equals(_value))
            {
                _value = temp;
                onChange(_value);
            }
            return _value;
        }
    }
}
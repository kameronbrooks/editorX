using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace EditorX
{
    public class PseudoElement<T>
    {
        MethodInfo _method;
        string _name;
        Rect _rect;
        T _value;
        GUILayoutOption[] _guiLayoutOptions;
        object[] _argumentBuffer;
        GUIContent _label;
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

            if(!temp.Equals(_value))
            {
                _value = temp;
                onChange(_value);
            }
            return _value;
        }

    }

}
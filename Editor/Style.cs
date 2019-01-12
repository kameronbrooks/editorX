using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class Style
    {
        Dictionary<string, object> _data;
        List<GUILayoutOption> _layoutOptionsList;
        GUILayoutOption[] _layoutOptions;
        bool _isDirty = false;
        GUIStyle _guiStyle;
        Color _bgColor;

        public Color backgroundColor
        {
            get
            {
                return _bgColor;
            }
            set
            {
                _bgColor = value;
            }
        }

        public Color color
        {
            get
            {
                return _guiStyle.normal.textColor;
            }
            set
            {
                _guiStyle.normal.textColor = value;
                _isDirty = true;
            }
        }

        public GUIStyle guistyle
        {
            get
            {
                return _guiStyle;
            }
            set
            {
                _guiStyle = value;
                _isDirty = true;
            }
        }

        public Style()
        {
            _data = new Dictionary<string, object>();
            _layoutOptionsList = new List<GUILayoutOption>();
            _guiStyle = null;
        }

        public Style(Style other)
        {
            _data = new Dictionary<string, object>(other._data);
            _layoutOptionsList = new List<GUILayoutOption>();
            _isDirty = true;
            if (other._guiStyle != null)
            {
                _guiStyle = new GUIStyle(other._guiStyle);
            }
            
        }
        public Style(Dictionary<string, object> values)
        {
            _data = new Dictionary<string, object>(values);
            _layoutOptionsList = new List<GUILayoutOption>();
            _guiStyle = null;
            _isDirty = true;

        }
        public Style(params object[] stylePairs)
        {
            _data = new Dictionary<string, object>();
            _layoutOptionsList = new List<GUILayoutOption>();
            _guiStyle = null;
            _isDirty = true;

            for(int i = 0; i < stylePairs.Length; i += 2)
            {
                _data.Add((string)stylePairs[i], stylePairs[i + 1]);
            }
        }

        public object this[string property]
        {
            get
            {
                property = property.ToLower();
                object value = null;
                _data.TryGetValue(property, out value);
                return value;
            }
            set
            {
                property = property.ToLower();
                if (_data.ContainsKey(property))
                {
                    //if (_data[property].Equals(value)) return;
                    _data[property] = value;
                }
                else
                {
                    _data.Add(property, value);
                }
                _isDirty = true;
            }
        }

        protected virtual void Update()
        {
            _layoutOptionsList.Clear();
            object temp = null;
            if (_data.TryGetValue("width", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.Width(val));
            }
            temp = null;
            if (_data.TryGetValue("height", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.Height(val));
            }
            temp = null;
            if (_data.TryGetValue("background-color", out temp) || _data.TryGetValue("backgroundcolor", out temp))
            {
                backgroundColor = (Color)temp;
            }
            temp = null;
            if (_data.TryGetValue("font", out temp))
            {
                _guiStyle.font = (Font)temp;
            }
            temp = null;
            if (_data.TryGetValue("font-style", out temp) || _data.TryGetValue("fontstyle", out temp))
            {
                _guiStyle.fontStyle = (FontStyle)temp;
            }
            temp = null;
            if (_data.TryGetValue("font-size", out temp) || _data.TryGetValue("fontsize", out temp))
            {
                _guiStyle.fontSize = (int)temp;
            }
            temp = null;
            if (_data.TryGetValue("alignment", out temp))
            {
                TextAnchor anchor = (temp.GetType() == typeof(string)) ? ConversionUtility.GetTextAnchor((string)temp) : (TextAnchor)temp;
                _guiStyle.alignment = anchor;
            }
            temp = null;
            if (_data.TryGetValue("image-position", out temp))
            {
                ImagePosition impos = (temp.GetType() == typeof(string)) ? ConversionUtility.GetImagePosition((string)temp) : (ImagePosition)temp;
                _guiStyle.imagePosition = impos;
            }
            temp = null;
            if (_data.TryGetValue("margin", out temp))
            {
                _guiStyle.margin = (RectOffset)temp;
            }
            temp = null;
            if (_data.TryGetValue("padding", out temp))
            {
                _guiStyle.padding = (RectOffset)temp;
            }
            temp = null;
            if (_data.TryGetValue("color", out temp))
            {
                color = (Color)temp;
            }
            temp = null;
            if (_data.TryGetValue("expand-width", out temp))
            {
                _layoutOptionsList.Add(GUILayout.ExpandWidth((bool)temp));
            }
            temp = null;
            if (_data.TryGetValue("expand-height", out temp))
            {
                _layoutOptionsList.Add(GUILayout.ExpandHeight((bool)temp));
            }
            temp = null;
            if (_data.TryGetValue("min-width", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.MinWidth(val));
            }
            temp = null;
            if (_data.TryGetValue("max-width", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.MaxWidth(val));
            }
            temp = null;
            if (_data.TryGetValue("min-height", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.MinHeight(val));
            }
            temp = null;
            if (_data.TryGetValue("max-height", out temp))
            {
                float val = (temp.GetType() == typeof(float)) ? (float)temp : (float)(int)temp;
                _layoutOptionsList.Add(GUILayout.MaxHeight(val));
            }
            _layoutOptions = _layoutOptionsList.ToArray();
            _isDirty = false;
        }
        public GUILayoutOption[] layoutOptions
        {
            get
            {

                if (_isDirty)
                {
                    Update();
                }
                return _layoutOptions;
            }
        }

    }
}

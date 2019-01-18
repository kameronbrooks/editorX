using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public class Style : ISerializationCallbackReceiver
    {
        private Dictionary<string, object> _data;

        [SerializeField]
        private byte[] _serializedData;

        [SerializeField]
        private List<UnityEngine.Object> _serializedObjects;

        [SerializeField]
        private List<GUILayoutOption> _layoutOptionsList;

        [SerializeField]
        private GUILayoutOption[] _layoutOptions;

        private bool _isDirty = false;

        [SerializeField]
        private GUIStyle _guiStyle;

        [SerializeField]
        private Color _bgColor;

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

            for (int i = 0; i < stylePairs.Length; i += 2)
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
                if (value != null && value.GetType() == typeof(int) && property != "font-size") value = (float)(int)value;
                if (_data.ContainsKey(property))
                {
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
                TextAnchor anchor = (temp.GetType() == typeof(string)) ? (TextAnchor)EnumUtility.GetEnumObject(typeof(TextAnchor),(string)temp) : (TextAnchor)temp;
                _guiStyle.alignment = anchor;
            }
            temp = null;
            if (_data.TryGetValue("image-position", out temp))
            {
                ImagePosition impos = (temp.GetType() == typeof(string)) ? (ImagePosition)EnumUtility.GetEnumObject(typeof(ImagePosition), (string)temp) : (ImagePosition)temp;
                _guiStyle.imagePosition = impos;
            }
            temp = null;
            if (_data.TryGetValue("margin", out temp))
            {
                _guiStyle.margin = RectUtility.ReadRectOffset(temp.ToString());
            }
            temp = null;
            if (_data.TryGetValue("padding", out temp))
            {
                _guiStyle.padding = RectUtility.ReadRectOffset(temp.ToString());
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

        protected void SerializeProperty(string name, object property, List<byte> buffer)
        {
            buffer.AddRange(BinarySerializer.GetBytes(name));
            int objectIndex = -1;
            switch (name)
            {
                case "font":
                    _serializedObjects.Add((UnityEngine.Object)property);
                    objectIndex = _serializedObjects.Count - 1;
                    break;

                case "background":
                    _serializedObjects.Add((UnityEngine.Object)property);
                    objectIndex = _serializedObjects.Count - 1;
                    break;

                default:
                    break;
            }
            if (objectIndex >= 0)
            {
                buffer.AddRange(BinarySerializer.GetBytes(objectIndex));
            }
            else
            {
                buffer.AddRange(BinarySerializer.GetBytes(property));
            }
        }

        protected void DeserializeProperty(byte[] bytes, ref int index)
        {
            string propname = BinarySerializer.GetString(bytes, ref index);
            object value = null;

            switch (propname)
            {
                case "width":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                case "height":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                case "background-color":
                    value = BinarySerializer.GetColor(bytes, ref index);
                    break;

                case "font":
                    value = _serializedObjects[BinarySerializer.GetInt(bytes, ref index)];
                    break;

                case "font-size":
                    value = BinarySerializer.GetInt(bytes, ref index);
                    break;

                case "font-style":
                    value = (FontStyle)BinarySerializer.GetEnum(bytes, typeof(FontStyle), ref index);
                    break;

                case "alignment":
                    value = (TextAlignment)BinarySerializer.GetEnum(bytes, typeof(TextAlignment), ref index);
                    break;

                case "image-position":
                    value = (ImagePosition)BinarySerializer.GetEnum(bytes, typeof(ImagePosition), ref index);
                    break;

                case "margin":
                    value = BinarySerializer.GetString(bytes, ref index);
                    break;

                case "padding":
                    value = BinarySerializer.GetString(bytes, ref index);
                    break;

                case "color":
                    value = BinarySerializer.GetColor(bytes, ref index);
                    break;

                case "expand-width":
                    value = BinarySerializer.GetBool(bytes, ref index);
                    break;

                case "expand-height":
                    value = BinarySerializer.GetBool(bytes, ref index);
                    break;

                case "min-width":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                case "min-height":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                case "max-width":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                case "max-height":
                    value = BinarySerializer.GetFloat(bytes, ref index);
                    break;

                default:
                    throw new System.Exception(propname + " is not a supported property name, cannot deserialize");
            }
            _data.Add(propname, value);
        }

        public void OnBeforeSerialize()
        {
            List<byte> buffer = new List<byte>();
            _serializedObjects = new List<Object>();

            var keys = _data.Keys;
            int i = 0;
            foreach (var key in keys)
            {
                SerializeProperty(key, _data[key], buffer);
                i++;
            }
            _serializedData = buffer.ToArray();
        }

        public void OnAfterDeserialize()
        {
            _data = new Dictionary<string, object>();
            if (_serializedData == null) return;
            int index = 0;

            while (index < _serializedData.Length)
            {
                DeserializeProperty(_serializedData, ref index);
            }
            _serializedData = null;
            _isDirty = true;
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
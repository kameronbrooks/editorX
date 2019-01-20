using UnityEngine;

namespace EditorX
{
    public abstract class ValueElement : Element
    {
        [SerializeField]
        protected GUIContent _label;

        public GUIContent label
        {
            get
            {
                return _label;
            }
            set
            {
                _label = value;
            }
        }

        public abstract System.Type valueType
        {
            get;
        }

        public void SetLabel(GUIContent content)
        {
            _label = content;
        }

        public void SetLabel(string content)
        {
            if (content == "" || content == null)
            {
                _label = null;
            }
            else
            {
                _label = new GUIContent(content);
            }
        }

        public void SetLabel(string content, string tooltip)
        {
            if (content == "" || content == null)
            {
                _label = null;
            }
            else
            {
                _label = new GUIContent(content, tooltip);
            }
        }

        public abstract T GetValue<T>();

        public abstract object GetValue();

        public abstract void SetValue(object val);

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = new GUIStyle(GUI.skin.textField);
        }

        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            if (_label != null && _label.text == "")
            {
                _label = null;
            }
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "label":
                    SetLabel(value.ToString());
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
                case "label":
                    result = _label;
                    break;

                default:
                    break;
            }

            return result;
        }

        public override Element AddChild(Element child)
        {
            Debug.LogWarning("Cannot add children to elements of type " + tag);
            return null;
        }
    }
}
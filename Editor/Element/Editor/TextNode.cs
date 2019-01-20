using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class TextNode : Element
    {
        [SerializeField]
        protected string _text;
        [SerializeField]
        protected bool _selectable = true;

        public static TextNode Create(string text)
        {
            TextNode node = ScriptableObject.CreateInstance<TextNode>();
            node._text = text;

            return node;
        }

        public string text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                this.style.guistyle = new GUIStyle(GUI.skin.label);
                if (parent != null) this.style.CopyTextProperties(parent.style);
            }
        }

        protected override void OnGUI()
        {
            object fontSize = style["font-size"];
            if (fontSize != null)
            {
                _rect.height = Mathf.Max(_rect.height, (float)((int)fontSize) * 1.5f);
            }
            if (_selectable)
            {
                EditorGUI.SelectableLabel(_rect, _text, style.guistyle);
            } else
            {
                EditorGUI.LabelField(_rect, _text, style.guistyle);
            }
            
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":
                case "innerText":

                    _text = value.ToString();
                    return true;
                case "selectable":
                    _selectable = (_selectable.GetType() == typeof(bool)) ? (bool)value : bool.Parse(value.ToString());
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
                    result = _text;
                    break;
                case "selectable":
                    result = _selectable;
                    break;
                default:
                    break;
            }

            return result;
        }
    }
}
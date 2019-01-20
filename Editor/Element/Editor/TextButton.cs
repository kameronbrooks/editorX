using UnityEditor;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public class TextButton : Element
    {
        [SerializeField]
        string _text;

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = GUI.skin.button;
        }

        protected override void OnGUI()
        {
            Event e = Event.current;

            if (name != null && name != "") GUI.SetNextControlName(name);

            if (GUI.Button(_rect, _text, style.guistyle))
            {
                CallEvent("click");
            }
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "text":
                case "value":
                    _text = value.ToString();
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
                case "text":
                case "value":
                    result = _text;
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
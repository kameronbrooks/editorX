using UnityEditor;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public class ImageButton : Element
    {
        [SerializeField]
        private Texture _img;

        public Texture img
        {
            get
            {
                return _img;
            }
            set
            {
                _img = value;
            }
        }

        public override string tag
        {
            get
            {
                return "imgbutton";
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) this.style.guistyle = GUI.skin.button;
        }

        protected override void OnGUI()
        {
            Event e = Event.current;

            if (name != null && name != "") GUI.SetNextControlName(name);

            if (GUI.Button(_rect, _img, style.guistyle))
            {
                CallEvent("click");
            }
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "texture":
                case "img":
                    if (value as Texture != null)
                    {
                        _img = (Texture)value;
                    }
                    else
                    {
                        Texture temp = AssetDatabase.LoadAssetAtPath<Texture>(value.ToString());
                        if (temp != null)
                        {
                            _img = temp;
                        }
                        else
                        {
                            Debug.LogError("EditorX failed to load image: No texture located at " + value.ToString());
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
                case "texture":
                case "img":
                    result = _img;
                    break;

                default:
                    break;
            }

            return result;
        }
    }
}
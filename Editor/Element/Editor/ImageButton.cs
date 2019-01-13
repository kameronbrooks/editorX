using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    [System.Serializable]
    public class ImageButton : Element
    {
        [SerializeField]
        Texture _img;

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

    }
}

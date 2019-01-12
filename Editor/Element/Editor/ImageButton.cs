using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class ImageButton : Element
    {
        Texture _img;
        public ImageButton(string name, Texture img, Style style = null) : base(name, style)
        {
            _img = img;
        }

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

            GUI.SetNextControlName(_name);

            if (GUI.Button(_rect, _img, style.guistyle))
            {
                CallEvent("click");
            }


        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddReference(new SerializedElement.SerializedObject("tex", _img));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._img = (Texture)serial.GetReference("tex");
        }


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TextButton : Element
    {
        static readonly Style DefaultStyle = new Style( new Dictionary<string, object>()
        {
            {"width", 100}
        });
        
        string _text;
        public TextButton(string name, string text, Style style = null) : base(name, style)
        {           
            _text = text;
        }
        public override string tag
        {
            get
            {
                return "button";
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

            if (GUI.Button(_rect, _text, style.guistyle))
            {
                CallEvent(ElementEvents.Click);
            }
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddValue(new SerializedElement.SerializedValue("text", _text));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._text = serial.GetValue("text").GetString();
        }
    }
}

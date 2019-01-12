using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TextNode : Element
    {
        
        protected string _text;

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

        public override string tag
        {
            get
            {
                return "span";
            }
        }


        public TextNode(string text, string name = "textnode", Style style = null) : base(name, style)
        {
            _text = text;
        }

        protected override void OnGUI()
        {
           
            EditorGUI.LabelField(_rect, _text, style.guistyle);
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

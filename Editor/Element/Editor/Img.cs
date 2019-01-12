using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class Img : Element
    {
        Texture _texture;
        ScaleMode _scaleMode;

        public Texture texture
        {
            get
            {
                return _texture;
            }
            set
            {
                _texture = value;
            }
        }
        public Img(string name, Texture tex, Style style = null) : base(name, style)
        {
            _texture = tex;
            _scaleMode = ScaleMode.ScaleAndCrop;
        }

        public ScaleMode scaleMode
        {
            get
            {
                return _scaleMode;
            }
            set
            {
                _scaleMode = value;
            }
        }

        public override string tag
        {
            get
            {
                return "img";
            }
        }

        protected override void OnGUI()
        {
            if(_texture != null) EditorGUI.DrawTextureTransparent(_rect, _texture, _scaleMode);
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            serial.AddReference(new SerializedElement.SerializedObject("tex",_texture));
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            this._texture = (Texture)serial.GetReference("tex");
        }
    }
}

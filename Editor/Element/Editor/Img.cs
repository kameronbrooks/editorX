using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class Img : Element
    {
        [SerializeField]
        Texture _texture;
        [SerializeField]
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


    }
}

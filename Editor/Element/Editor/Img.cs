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

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "texture":
                    if (value as Texture != null)
                    {
                        _texture = (Texture)value;
                    }
                    else
                    {
                        Texture temp = AssetDatabase.LoadAssetAtPath<Texture>(value.ToString());
                        if (temp != null)
                        {
                            _texture = temp;
                        } else
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
                    result = _texture;
                    break;
                default:
                    break;
            }

            return result;
        }


    }
}

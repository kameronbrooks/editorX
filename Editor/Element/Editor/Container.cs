using UnityEditor;
using UnityEngine;

namespace EditorX
{
    public class Container : Element
    {
        private Window _window;

        public Window window
        {
            get
            {
                return _window;
            }
            set
            {
                _window = value;
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                style.guistyle = GUI.skin.box;
                style.guistyle.normal.background = null;
            }
        }

        protected override void PreGUI()
        {
            _rect = window.position;
            _rect.x = 0;
            _rect.y = 0;
        }



        protected override void OnGUI()
        {
            if (style.isDirty) style.Update();
            Texture texture = style.guistyle.normal.background;
            if (texture != null)
            {
                EditorGUI.DrawTextureTransparent(_rect, texture, ScaleMode.ScaleAndCrop);
            }
            else if (style.backgroundColor != null && style.backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(_rect, style.backgroundColor);
            }
            
            DrawChildren();
        }

        public override void RequestRepaint()
        {
            window.Repaint();
        }
    }
}
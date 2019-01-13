using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public class Container : Element
    {
        Window _window;

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

        public override string tag
        {
            get
            {
                return "container";
            }
        }

        protected override void PreGUI()
        {

        }
        protected override void OnGUI()
        {
            DrawChildren();
        }

    }
}

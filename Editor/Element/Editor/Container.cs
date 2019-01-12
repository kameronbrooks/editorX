using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public class Container : Element
    {
        Window _window;
        public Container(string name, Window window) : base(name, null)
        {
            _window = window;
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

        protected override SerializedElement ToSerialized()
        {
            return null;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            return;
        }
    }
}

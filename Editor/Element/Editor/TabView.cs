using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TabView : Element
    {
        [SerializeField]
        string _tabName;

        protected bool isInTabGroup()
        {
            return (_parent != null && _parent as TabGroup != null);
        }
        protected override void PreGUI()
        {

        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                this.style.guistyle = new GUIStyle(GUI.skin.box);
                this.style.guistyle.normal.background = null;
                if (parent != null) this.style.CopyTextProperties(parent.style);
            }
        }
        protected override void OnGUI()
        {
            DrawChildren();
        }
    }
}

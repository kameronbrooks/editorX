using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class TabGroup : Element
    {
        [SerializeField]
        float _tabBarHeight = 30;
        [SerializeField]
        int _selectedTab = 0;
        [SerializeField]
        Color _nonfocusedColor;
        [SerializeField]
        bool _expandTabWidth = false;

        GUIContent[] _tabNames;

        protected void UpdateTabNames()
        {
            int count = children.Count;
            _tabNames = new GUIContent[count];
            for(int i = 0; i < count; i += 1)
            {
                object tabName = children[i].GetProperty("tab-name");
                _tabNames[i] = new GUIContent((tabName != null) ? (string)tabName : children[i].name);
            }

        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null)
            {
                this.style.guistyle = new GUIStyle();
                this.style.guistyle.normal.background = null;
                if (parent != null) this.style.CopyTextProperties(parent.style);
            }
        }

        protected override void PreGUI()
        {
            if(_nonfocusedColor == Color.clear)
            {
                _nonfocusedColor = style.backgroundColor * 0.8f;
            }
            if(_tabNames == null || _children.Count != _tabNames.Length)
            {
                UpdateTabNames();
            }
        }

        protected void DrawTabs()
        {
            Event e = Event.current;
             
            for(int i = 0; i < _tabNames.Length; i += 1)
            {
                Rect tabRect = (_expandTabWidth) ? GUILayoutUtility.GetRect(_tabNames[i], style.guistyle, 
                    GUILayout.ExpandHeight(true), 
                    GUILayout.MinWidth(style.guistyle.CalcSize(_tabNames[i]).x)) 
                    :
                    GUILayoutUtility.GetRect(_tabNames[i], style.guistyle,
                    GUILayout.ExpandHeight(true),
                    GUILayout.Width(style.guistyle.CalcSize(_tabNames[i]).x))
                    ;
                bool containsMouse = (tabRect.Contains(e.mousePosition));
                Color color;
                if (i == _selectedTab)
                {
                    color = style.backgroundColor;
                } else
                {
                    color = _nonfocusedColor;
                }

                if(containsMouse)
                {
                    //color = color * 1.1f;
                    
                    if (e.type == EventType.MouseDown)
                    {
                        if(i != _selectedTab)
                        {
                            _selectedTab = i;
                            RequestRepaint();
                            CallEvent("change");
                        }
                        
                    }
                }

                EditorGUI.DrawRect(tabRect, color);
                
                EditorGUI.LabelField(tabRect, _tabNames[i], style.guistyle);

                
                
            }
        }
        protected override void OnGUI()
        {
            _rect = EditorGUILayout.BeginVertical(style.layoutOptions);
            Rect tabBarRect = EditorGUILayout.BeginHorizontal(GUILayout.Height(_tabBarHeight), GUILayout.ExpandWidth(true));
            DrawTabs();
            EditorGUILayout.EndHorizontal();

            Rect tabPageRect = EditorGUILayout.BeginHorizontal(style.guistyle, GUILayout.ExpandWidth(true), GUILayout.MinHeight(30));
            EditorGUI.DrawRect(tabPageRect, style.backgroundColor);
            children[_selectedTab].Draw();
            EditorGUILayout.EndHorizontal();


            EditorGUILayout.EndVertical();
        }
    }

}
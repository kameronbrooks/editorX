using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using System;

namespace EditorX
{
    [System.Serializable]
    public class TextBox : Element, IValueElement
    {
        [SerializeField]
        string _content;

        Vector2 _cursorPos;
        int _cursorIndex;
        int _selectIndex;
        TextEditor _textEditor;
        bool _isInFocus;
        EditorWindow _window;

        public bool isInFocus
        {
            get
            {
                return _isInFocus;
            }

            set
            {
                _isInFocus = value;
            }
        }

        public Type valueType
        {
            get
            {
                return typeof(string);
            }
        }

        public override string tag
        {
            get
            {
                return "textarea";
            }
        }

        public TextBox(string name, EditorWindow window, Style style = null) : base (name, style)
        {
            _name = name;
            _window = window;
        }
        private void SaveTextEditorState()
        {
            if (_textEditor != null)
            {
                Debug.Log("Saving State");
                _cursorIndex = _textEditor.cursorIndex;
                _selectIndex = _textEditor.selectIndex;
            }
        }
        private void LoadTextEditorState()
        {
            if (_textEditor != null)
            {
                Debug.Log("Loading State");
                _textEditor.cursorIndex = _cursorIndex;
                MemberInfo[] members =  typeof(TextEditor).GetMembers(BindingFlags.NonPublic);

                for(int i = 0; i < members.Length; i ++)
                {
                    Debug.Log(members[i].Name);
                }
            }
        }
        private void ClearTextEditorState()
        {
            if (_textEditor != null)
            {
                Debug.Log("Clearing State");
                _textEditor.cursorIndex = 0;
                _textEditor.SelectNone();
            }
        }
        private TextEditor GetTextEditor()
        {
            return typeof(EditorGUI).GetField("activeEditor", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null) as TextEditor;
        }

        private void HandleKeyEvent()
        {
            if(Event.current.type == EventType.KeyDown)
            {
                SaveTextEditorState();
            }
        }

        protected override void InitializeGUIStyle()
        {

            if (style.guistyle == null)
            {
                style.guistyle = GUI.skin.textArea;
            }
        }
        protected override void OnGUI()
        {
            Event e = Event.current;
            GUI.SetNextControlName(_name);
            
            bool isClick = e.type == EventType.MouseDown;
            bool isOverElement = rect.Contains(e.mousePosition);
            bool isSelected = false;

            HandleKeyEvent();
            Color oldCursorColor = GUI.skin.settings.cursorColor;
            if (isClick && isOverElement && !_isInFocus)
            {
                GUI.skin.settings.cursorColor = Color.clear;
            }
            Debug.Log("Content(s) = " + _content);
            string temp = EditorGUI.TextArea(_rect, _content, style.guistyle);
            Debug.Log("After Temp Content(s) = " + _content);
            if (isClick && isOverElement && !_isInFocus)
            {
                GUI.skin.settings.cursorColor = oldCursorColor;
            }

            if (GUI.GetNameOfFocusedControl() == _name)
            {
                _textEditor = GetTextEditor();
                isSelected = true;
            }
            else
            {
                isSelected = false;
            }



            if (isClick)
            {
                if (isOverElement)
                {
                    if(!_isInFocus)
                    {
                        //LoadTextEditorState();
                        _isInFocus = isSelected;
                    }
                }
                else
                {
                    Debug.Log("On Deselect" + _cursorIndex);
                    _isInFocus = false;
                }
            }

            

            if (temp != _content)
            {
                _content = temp;
                CallEvent(ElementEvents.Change);
            }

            
        }

        public override void OnWindowLostFocus()
        {
            _isInFocus = false;
        }

        public T GetValue<T>()
        {
            return (T)(object)_content;
        }

        public object GetValue()
        {
            return _content;
        }

        public void SetValue(object val)
        {
            _content = (string)val;
            
        }

        protected override SerializedElement ToSerialized()
        {
            SerializedElement serial = new SerializedElement();
            if(_content != null && _content != "")
            {
                Debug.Log("ToSerialized: " + _content);
                serial.AddValue(new SerializedElement.SerializedValue("val", _content));
            }
            
            return serial;
        }

        protected override void FromSerialized(SerializedElement serial)
        {
            //Debug.Log("");
            Debug.Log("Deserializing! ");
            SerializedElement.SerializedValue serializedText = serial.GetValue("val");
            Debug.Log(serializedText);
            if(serializedText != null)
            {
                this._content = serializedText.GetString();
                Debug.Log(this._content);
            }
        }
    }
}
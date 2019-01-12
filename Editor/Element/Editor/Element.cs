using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace EditorX
{
    public abstract class Element : IElement
    {
        protected IElement _parent;
        protected List<IElement> _children;

        protected string _name;
        protected Rect _rect;
        protected Dictionary<string, object> _styleData;
        protected Color _backgroundColor;
        private Dictionary<string, EventCallback> _eventHandlers;
        private Style _style;
        public IElement parent
        {
            get
            {
                return _parent;
            }
            set
            {
                _parent = value;
            }
        }

        public List<IElement> children
        {
            get
            {
                return _children;
            }

        }

        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }
        public Rect rect
        {
            get
            {
                return _rect;
            }
        }

        public Style style
        {
            get
            {
                return _style;
            }
            set
            {
                _style = value;
            }
        }
        public abstract string tag
        {
            get;
        }


        protected bool CallEvent(string eventName)
        {
            EventCallback handler = null;
            if (_eventHandlers.TryGetValue(eventName.ToLower(), out handler))
            {
                handler(this, Event.current);
                return true;
            }
            return false;
        }
        

        protected Element(string name, Style style = null)
        {
            _name = name;
            _style = (style != null) ? new Style(style) : new Style();
            _children = new List<IElement>();
            _rect = new Rect();
            _parent = null;
            _eventHandlers = new Dictionary<string, EventCallback>();
            
        }

        protected virtual void InitializeGUIStyle()
        {

        }

        protected virtual void PreGUI()
        {
            InitializeGUIStyle();
            
            _rect = EditorGUILayout.GetControlRect(_style.layoutOptions);
        }
        protected abstract void OnGUI();

        protected virtual void PostGUI()
        {

        }

        public virtual void AddChild(IElement child)
        {
            _children.Add(child);
            child.parent = this;

        }
        public virtual void AddText(string text)
        {
            IElement child = new TextNode(text);
            _children.Add(child);
            child.parent = this;

            if (style != null)
            {
                child.style = style;
            }

        }

        public virtual IElement GetChildById(string name)
        {
            for(int i = 0; i < _children.Count; i++)
            {
                if (_children[i].name == name) return _children[i];
            }
            for (int i = 0; i < _children.Count; i++)
            {
                IElement grandChild = _children[i].GetChildById(name);
                if (grandChild != null) return grandChild;
            }
            return null;
        }
        protected void DrawChildren()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Draw();
            }
        }
        protected virtual void HandleEvents()
        {
            Event e = Event.current;
            if (e.type == EventType.Used) return;
            if (_rect == null) return;
            
            if (e.type == EventType.KeyUp) CallEvent("keyup");
            if (e.type == EventType.KeyDown) CallEvent("keydown");

            if (!_rect.Contains(e.mousePosition)) return;
            if (e.type == EventType.MouseDown) CallEvent("mousedown");
            if (e.type == EventType.MouseUp) CallEvent("mouseup");

        }

        public virtual void Draw()
        {
            PreGUI();
            OnGUI();
            PostGUI();
        }

        public void AddEventListener(string eventType, EventCallback callback)
        {
            eventType = eventType.ToLower();
            if(_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] += callback;
            }
            else
            {
                _eventHandlers[eventType] = callback;
            }
        }
        public void RemoveEventListener(string eventType, EventCallback callback)
        {
            eventType = eventType.ToLower();
            if (_eventHandlers.ContainsKey(eventType))
            {
                _eventHandlers[eventType] -= callback;
            }
        }

        public virtual void OnWindowLostFocus()
        {
            for(int i = 0; i < _children.Count; i += 1)
            {
                _children[i].OnWindowLostFocus();
            }
        }

        public virtual void OnWindowFocus()
        {
            for (int i = 0; i < _children.Count; i += 1)
            {
                _children[i].OnWindowFocus();
            }
        }

        protected abstract SerializedElement ToSerialized();
        protected abstract void FromSerialized(SerializedElement serial);

        public void OnSerialize(SerializedElementQueue list)
        {
            SerializedElement serial = ToSerialized();
            list.Enqueue(serial);
            for (int i = 0; i < _children.Count; i += 1)
            {
                _children[i].OnSerialize(list);
            }
        }
        public void OnDeserialize(SerializedElementQueue list)
        {
            Debug.Log("OnDeserialize(" + name + ")");
            SerializedElement serial = list.Dequeue();
            if(serial != null)
            {
                Debug.Log("OnDeserialize(" + name + "): " + serial.ToString());
                FromSerialized(serial);
            }
            for (int i = 0; i < _children.Count; i += 1)
            {
                _children[i].OnDeserialize(list);
            }
        }
    }
}

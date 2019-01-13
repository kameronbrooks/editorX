using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Events;

namespace EditorX
{
    public delegate void EventCallback(Element elem, Event evnt);

    public abstract class Element : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        protected Element _parent;
        [SerializeField]
        protected List<Element> _children;
        protected Rect _rect;
        protected Dictionary<string, object> _styleData;
        protected Color _backgroundColor;
        [System.NonSerialized]
        private Dictionary<string, EventCallback> _eventHandlers;
        [SerializeField]
        List<DelegateSerialization.SerializedDelegate> _serializedDelegates;
        [SerializeField]
        private Style _style;
        public Element parent
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

        public List<Element> children
        {
            get
            {
                return _children;
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
        
        public void OnEnable()
        {
            if (_style == null) _style = new Style();
            if(_children == null) _children = new List<Element>();
            if(_rect == null) _rect = new Rect();
            if(_parent == null) _parent = null;
            if(_eventHandlers == null) _eventHandlers = new Dictionary<string, EventCallback>();
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

        public virtual Element AddChild(Element child)
        {
            if (child == null)
            {
                Debug.LogWarning("No child passed to element");
                return null;
            }
            _children.Add(child);
            child.parent = this;
            return child;
        }

        public virtual void Unload()
        {
            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Unload();
            }

            DestroyImmediate(this);
        }
        
        public virtual TextNode AddText(string text)
        {
            TextNode child = TextNode.Create(text);
            _children.Add(child);
            child.parent = this;

            if (style["color"] != null)
            {
                child.style["color"] = style["color"];
            }
            return child;
        }
        
        public virtual Element GetChildById(string name)
        {
            for(int i = 0; i < _children.Count; i++)
            {
                if (_children[i].name == name) return _children[i];
            }
            for (int i = 0; i < _children.Count; i++)
            {
                Element grandChild = _children[i].GetChildById(name);
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
        public static T Create<T>() where T : Element
        {
            T elem = ScriptableObject.CreateInstance<T>();
            return elem;
        }
        public static T Create<T>(string name) where T : Element
        {
            T elem = ScriptableObject.CreateInstance<T>();
            elem.name = name;
            return elem;
        }

        protected void SerializeEventHandlers()
        {
            _serializedDelegates = new List<DelegateSerialization.SerializedDelegate>();
            var keys = _eventHandlers.Keys;
            foreach (var key in keys)
            {
                _serializedDelegates.Add(DelegateSerialization.SerializeDelegate(_eventHandlers[key], key));
            }
        }
        protected void DeserializeEventListeners()
        {
            _eventHandlers = new Dictionary<string, EventCallback>();
            for (int i = 0; i < _serializedDelegates.Count; i += 1)
            {
                _eventHandlers.Add(_serializedDelegates[i].eventname, DelegateSerialization.DeserializeDelegate(_serializedDelegates[i]));
            }
        }
        public virtual void OnBeforeSerialize()
        {
            SerializeEventHandlers();
        }

        public virtual void OnAfterDeserialize()
        {
            DeserializeEventListeners();
        }
    }
}

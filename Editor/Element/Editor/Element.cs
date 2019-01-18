using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        [SerializeField]
        private bool _visible = true;

        [SerializeField]
        private bool _isLoaded = false;

        [System.NonSerialized]
        private Dictionary<string, EventCallback> _eventHandlers;

        [SerializeField]
        private List<DelegateSerialization.SerializedDelegate> _serializedDelegates;

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

        public virtual string tag
        {
            get
            {
                return this.GetType().Name.ToLower();
            }
        }

        protected bool visible
        {
            get
            {
                return _visible;
            }

            set
            {
                _visible = value;
            }
        }

        protected bool hidden
        {
            get
            {
                return !_visible;
            }

            set
            {
                _visible = !value;
            }
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
            if (_children == null) _children = new List<Element>();
            if (_rect == null) _rect = new Rect();
            if (_parent == null) _parent = null;
            if (_eventHandlers == null) _eventHandlers = new Dictionary<string, EventCallback>();

            CallEvent("assemblyReload");
        }

        protected virtual void InitializeGUIStyle()
        {
            style.guistyle = new GUIStyle();
        }

        protected virtual void PreGUI()
        {
            _rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight, _style.guistyle, _style.layoutOptions);
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

        public virtual void Load()
        {
            _isLoaded = true;
            CallEvent("load");

            for (int i = 0; i < _children.Count; i++)
            {
                _children[i].Load();
            }
        }

        public virtual void Unload()
        {
            _isLoaded = false;
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
            for (int i = 0; i < _children.Count; i++)
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
            if (_visible)
            {
                InitializeGUIStyle();
                PreGUI();
                OnGUI();
                PostGUI();
            }
        }

        public void AddEventListener(string eventType, EventCallback callback)
        {
            if (callback == null)
            {
                Debug.LogWarning("Cannot add null callback");
                return;
            }
            // Make sure that delegate target is a UnityEngine.Object
            UnityEngine.Object targ = callback.Target as UnityEngine.Object;
            if (targ == null) Debug.LogWarning("[" + callback.Method.Name + "] Callback target type is not derived from UnityEngine.Object, callback will not be serialized. This callback will be lost when assembly reloads.");

            eventType = eventType.ToLower();
            if (_eventHandlers.ContainsKey(eventType))
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
            for (int i = 0; i < _children.Count; i += 1)
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

        public static Element Create(System.Type type)
        {
            Element elem = (Element)ScriptableObject.CreateInstance(type);
            return elem;
        }

        public static Element Create(System.Type type, string name)
        {
            Element elem = (Element)ScriptableObject.CreateInstance(type);
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

        public virtual bool SetProperty(string name, object value)
        {
            switch (name)
            {
                case "visible":
                    _visible = (value.GetType() == typeof(bool)) ? (bool)value : System.Boolean.Parse(value.ToString());
                    return true;

                case "hidden":
                    _visible = (value.GetType() == typeof(bool)) ? !(bool)value : !System.Boolean.Parse(value.ToString());
                    return true;

                default:
                    return false;
            }
        }

        public virtual object GetProperty(string name)
        {
            switch (name)
            {
                case "visible":
                    return _visible;

                case "hidden":
                    return !_visible;

                default:
                    return null;
            }
        }

        public virtual void OnEditorUpdate()
        {
            for (int i = 0; i < _children.Count; i += 1)
            {
                _children[i].OnEditorUpdate();
            }
        }
        public virtual void RequestRepaint()
        {
            if (_parent != null) _parent.RequestRepaint();
        }
    }
}
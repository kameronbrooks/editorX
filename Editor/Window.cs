using UnityEditor;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public abstract class Window : EditorWindow, ISerializationCallbackReceiver
    {
        [SerializeField]
        private Element _body;

        [SerializeField]
        private bool _isLoaded = false;

        [SerializeField]
        private bool _reloadOnAssemblyReload = false;

        protected abstract void OnOpen();

        protected abstract void OnClose();

        //protected abstract void Draw();
        protected virtual void PreGUI()
        {
        }

        protected virtual void PostGUI()
        {
        }

        protected virtual void OnEditorUpdate()
        {
        }

        public abstract void OnLoadWindow();

        protected void Load()
        {
            if (!_isLoaded || _body == null)
            {
                _body = NewElement<Container>("body");
                ((Container)_body).window = this;
                OnLoadWindow();
                _isLoaded = true;
            }
        }

        public void Unload()
        {
            if (_body != null) _body.Unload();
            _isLoaded = false;
        }

        protected Element body
        {
            get
            {
                return _body;
            }
        }

        public bool reloadOnAssemblyReload
        {
            get
            {
                return _reloadOnAssemblyReload;
            }

            set
            {
                _reloadOnAssemblyReload = value;
            }
        }

        public void Open()
        {
            this.Show();
            EditorApplication.update += OnEditorUpdate;
            OnOpen();
        }

        protected virtual void OnGUI()
        {
            Load();
            PreGUI();
            if (_body != null)
            {
                _body.Draw();
            }
            PostGUI();
        }

        public void OnEnable()
        {
        }

        public void OnLostFocus()
        {
            if (_body != null) _body.OnWindowLostFocus();
        }

        public void OnFocus()
        {
            if (_body != null) _body.OnWindowFocus();
        }

        public new void Close()
        {
            EditorApplication.update -= OnEditorUpdate;
            Unload();
            OnClose();
            base.Close();
        }

        public void OnBeforeSerialize()
        {
            if (_reloadOnAssemblyReload) _isLoaded = false;
        }

        public void LoadFromMarkup(string markup, UnityEngine.Object callbackTarget = null)
        {
            EditorXParser parser = new EditorXParser();
            parser.callbackTarget = (callbackTarget != null) ? callbackTarget : this;
            parser.Initialize();
            Element[] elements = parser.BuildUI(markup);
            for (int i = 0; i < elements.Length; i += 1)
            {
                body.AddChild(elements[i]);
            }
        }

        public void OnAfterDeserialize()
        {
        }

        public static T NewElement<T>() where T : Element
        {
            T elem = Element.Create<T>();
            return elem;
        }

        public static T NewElement<T>(string name) where T : Element
        {
            T elem = Element.Create<T>(name);
            return elem;
        }
    }
}
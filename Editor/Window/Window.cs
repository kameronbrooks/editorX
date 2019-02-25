using UnityEditor;
using UnityEngine;
using System.IO;

namespace EditorX
{
    [System.Serializable]
    public abstract class Window : EditorWindow, ISerializationCallbackReceiver
    {
        [SerializeField]
        private GUISkin _skin;
        [SerializeField]
        private Element _body;

        [SerializeField]
        private bool _isLoaded = false;

        [SerializeField]
        private bool _reloadOnAssemblyReload;
        [SerializeField]
        protected bool _wantsMouseMove;
        protected bool _wasAssemblyReloaded;

        public new bool wantsMouseMove
        {
            get
            {
                return base.wantsMouseMove;

            }
            set
            {
                base.wantsMouseMove = _wantsMouseMove = value;
            }
        }

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

        protected virtual void EditorUpdate()
        {
            OnEditorUpdate();
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

                _body.Load();
            }
        }

        public void Unload()
        {
            if (_body != null) _body.Unload();
            _isLoaded = false;
        }

        public Element body
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

        public GUISkin skin
        {
            get
            {
                return _skin;
            }

            set
            {
                _skin = value;
            }
        }

        public void Open()
        {
            this.Show();

            OnOpen();
        }

        protected virtual void OnGUI()
        {
            Load();

            if ( _wasAssemblyReloaded )
            {
                OnAssemblyReload();
                _wasAssemblyReloaded = false;
            }
            PreGUI();
            GUISkin tempSkin = GUI.skin;
            if (_skin == null) GUI.skin = _skin;

            if (_body != null)
            {
                _body.Draw();
            }
            GUI.skin = tempSkin;
            PostGUI();
        }

        public void OnEnable()
        {
            EditorApplication.update += EditorUpdate;
        }
        public void OnDestroy()
        {
            EditorApplication.update -= EditorUpdate;
            Unload();
            OnClose();
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
            
            Unload();
            OnClose();
            base.Close();
        }

        public void OnBeforeSerialize()
        {
            if (_reloadOnAssemblyReload) _isLoaded = false;
            OnSerialize();
        }

        public void LoadFromMarkup(string markup, UnityEngine.Object callbackTarget = null)
        {
            EditorXParser parser = new EditorXParser(this);
            parser.callbackTarget = (callbackTarget != null) ? callbackTarget : this;
            parser.Initialize();
            Element[] elements = parser.BuildUI(markup);
            for (int i = 0; i < elements.Length; i += 1)
            {
                body.AddChild(elements[i]);
            }
        }
        public void LoadFromFile(string unityPath, UnityEngine.Object callbackTarget = null)
        {
            string fullPath = Application.dataPath + "/" + unityPath;
            if (!File.Exists(fullPath))
            {
                throw new System.IO.FileNotFoundException("No file found at " + fullPath);
            }

            string markup = File.ReadAllText(fullPath);

            LoadFromMarkup(markup, callbackTarget);
        }

        public Element GetElementByID(string name)
        {
            return _body.GetChildById(name);
        }

        public void OnAfterDeserialize()
        {
            base.wantsMouseMove = _wantsMouseMove;
            _wasAssemblyReloaded = true;
            OnDeserialize();
        }

        protected virtual void OnSerialize()
        {

        }
        protected virtual void OnDeserialize()
        {

        }
        /// <summary>
        /// This will be called after an assembly reload, use it to bind you non-serialized data back to its element after it is lost in the reload
        /// </summary>
        protected abstract void OnAssemblyReload();


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
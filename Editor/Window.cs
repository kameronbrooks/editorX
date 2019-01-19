﻿using UnityEditor;
using UnityEngine;

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

        public void OnAfterDeserialize()
        {
            base.wantsMouseMove = _wantsMouseMove;
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
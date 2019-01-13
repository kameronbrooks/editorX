using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    [System.Serializable]
    public abstract class Window : EditorWindow, ISerializationCallbackReceiver
    {

        [SerializeField]
        Element _body;
        [SerializeField]
        private bool _isLoaded = false;
        protected abstract void OnOpen();
        protected virtual void OnClose()
        {
            Unload();
        }
        //protected abstract void Draw();
        protected virtual void PreGUI()
        {

        }
        protected virtual void PostGUI()
        {
            
        }

        public abstract void OnLoadWindow();

        protected void Load()
        {
            if (!_isLoaded)
            {
                _body = NewElement<Container>("body");
                ((Container)_body).window = this;
                OnLoadWindow();
                _isLoaded = true;
            }   
            
        }
        public void Unload()
        {
            _body.Unload();
            _isLoaded = false;
        }
        protected Element body
        {
            get
            {
                return _body;
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
            if (_body != null)
            {
                _body.Draw();
            }
            PostGUI();
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
            OnClose();
            base.Close();
        }

        public void OnBeforeSerialize()
        {

        }


        public void OnAfterDeserialize()
        {
            
        }

        public static T NewElement<T>() where T:Element
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
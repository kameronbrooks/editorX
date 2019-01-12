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
        private SerializedElementQueue _serializedElements;

        IElement _body;
        private bool _isLoaded = false;
        protected abstract void OnOpen();
        protected abstract void OnClose();
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
                _body = new Container("body", this);
                OnLoadWindow();
                _isLoaded = true;

                Debug.Log(_serializedElements);
                if (_serializedElements != null && _serializedElements.Count > 0)
                {
                    if (_serializedElements != null) Debug.Log("Load(): serialized elements  " + _serializedElements.Count);
                    _body.OnDeserialize(_serializedElements);
                }
            }   
            
        }
        public void Unload()
        {
            _isLoaded = false;
        }


        protected IElement body
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
            Debug.Log("Serializing  ");
            if (_serializedElements == null) _serializedElements = new SerializedElementQueue();
            _serializedElements.Clear();
            _body.OnSerialize(_serializedElements);
            Debug.Log("OnBeforeSerialize: " + _serializedElements.Count + " Elements");
        }


        public void OnAfterDeserialize()
        {
            Debug.Log("OnDeserialize  " + _serializedElements.ToString());
            _isLoaded = false;
            
        }
    }

}
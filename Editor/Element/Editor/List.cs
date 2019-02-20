using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public class List : ValueElement
    {
        [SerializeField]
        Vector2 _scrollPos;

        System.Type _dataType;
        System.Type _arrayType;

        [SerializeField]
        protected string _dataTypeName;
        IList _list;

        [SerializeField]
        UnityEngine.Object[] _serializedReference;
        [SerializeField]
        byte[] _serializedBytes;

        [SerializeField]
        ListItem _listItem;
        public ListItem listItem
        {
            get
            {
                if(_listItem == null)
                {
                    for(int i = 0; i < children.Count; i += 1)
                    {
                        Debug.Log(children[i].GetType().Name);
                        if((children[i] as ListItem) != null)
                        {
                            _listItem = children[i] as ListItem;
                            break;
                        }
                    }
                }
                return _listItem;
            }
        }

        public System.Type dataType
        {
            get
            {
                if (_dataType == null || _dataType.Name != _dataTypeName) _dataType = TypeUtility.GetTypeByName(_dataTypeName);
                return _dataType;
            }
        }
        public override Type valueType
        {
            get
            {
                if(_arrayType == null)
                {
                    _arrayType = TypeUtility.GetArrayType(dataType);
                }
                return _arrayType;
            }
        }

        protected override void InitializeGUIStyle()
        {
            if (style.guistyle == null) style.guistyle = new GUIStyle(GUI.skin.scrollView);
        }

        protected override void PreGUI()
        {

        }

        
        protected override void OnGUI()
        {
            GUI.SetNextControlName(name);
            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos, style.guistyle, style.layoutOptions);

            Rect bgRect = EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (style.backgroundColor != Color.clear)
            {
                EditorGUI.DrawRect(bgRect, style.backgroundColor);
            }
            if (listItem == null) throw new Exception("No List item template found in list children");
            if (_list != null)
            {
                listItem.SetList(_list);
                for (int i = 0; i < _list.Count; i += 1)
                {
                    listItem.SetTargetIndex(i);
                    listItem.Draw();
                }
            }
            
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        public override T GetValue<T>()
        {
            return (T)(object)_list;
        }

        public override object GetValue()
        {
            return _list;
        }

        public override void SetValue(object val)
        {
            _list = (IList)val;
        }

        public void AddItem(object ob)
        {
            if (_list == null) _list = TypeUtility.GetArrayOfType(dataType, 0);
            IList temp = _list;

            _list = TypeUtility.GetArrayOfType(dataType, temp.Count + 1);
            for (int i = 0; i < temp.Count; i += 1)
            {
                _list[i] = temp[i];
            }
            _list[_list.Count - 1] = ob;
        }
        public void RemoveItem(int index)
        {
            if (_list == null || _list.Count < 1) return;
            IList temp = _list;
            
            _list = TypeUtility.GetArrayOfType(dataType, temp.Count - 1);
            int j = 0;
            for (int i = 0; i < temp.Count; i += 1)
            {
                if (i == index) continue;
                _list[j] = temp[i];
                j++;
            }
        }
        protected void ResizeArray(int newSize)
        {
            Debug.Log("resizing array");
            if (_list == null) {
                _list = TypeUtility.GetArrayOfType(dataType, newSize);
                return;
            }
            if(newSize > _list.Count)
            {
                IList temp = _list;
                _list = TypeUtility.GetArrayOfType(dataType, newSize);
                for(int i = 0; i < temp.Count; i += 1)
                {
                    _list[i] = temp[i];
                }
            } 
            else if(newSize < _list.Count)
            {
                IList temp = _list;
                _list = TypeUtility.GetArrayOfType(dataType, newSize);
                for (int i = 0; i < _list.Count; i += 1)
                {
                    _list[i] = temp[i];
                }
            }
            else
            {
                // do nothing, already correct size
            }
        }

        public override bool SetProperty(string name, object value)
        {
            if (base.SetProperty(name, value)) return true;

            switch (name)
            {
                case "value":

                    return true;
                case "count":
                    int val = (value.GetType() == typeof(string)) ? Int32.Parse((string)value) : (int)value;
                    if (_list == null || _list.Count != val)
                    {
                        ResizeArray(val);
                    }
                    return true;
                case "typename":
                case "type":
                    _dataTypeName = value.ToString();
                    return true;

                default:
                    return false;
            }
        }

        public override object GetProperty(string name)
        {
            object result = base.GetProperty(name);

            if (result != null) return result;

            switch (name)
            {
                case "value":
                    result = _list;
                    break;
                case "count":
                    result = _list.Count;
                    break;
                case "typename":
                case "type":
                    result = _dataTypeName;
                    break;
                default:
                    break;
            }

            return result;
        }

        public override Element AddChild(Element child)
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

        public override void OnBeforeSerialize()
        {
            base.OnBeforeSerialize();
            if (dataType.IsInstanceOfType(typeof(UnityEngine.Object)))
            {
                _serializedReference = new UnityEngine.Object[_list.Count];
                for(int i = 0; i < _list.Count; i += 1)
                {
                    _serializedReference[i] = (UnityEngine.Object)_list[i];
                }
            }
            else
            {
                _serializedBytes = BinarySerializer.GetBytes(_list);
            }           
        }
        public override void OnAfterDeserialize()
        {
            base.OnAfterDeserialize();
            
            if (dataType.IsInstanceOfType(typeof(UnityEngine.Object)))
            {
                _list = TypeUtility.GetArrayOfType(dataType, _serializedReference.Length);
                for (int i = 0; i < _list.Count; i += 1)
                {
                    _list[i] = _serializedReference[i];
                }
            }
            else
            {
                int i = 0;
                _list = BinarySerializer.GetList(_serializedBytes, ref i, dataType);
            }
        }
    }
}

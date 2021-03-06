﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

namespace EditorX
{
    public class ListItem : Element
    {
        IList _listTarget;
        int _index;
        object target
        {
            get
            {
                if (_listTarget == null || _index < 0 || _index >= _listTarget.Count) return null;
                return _listTarget[_index];
            }set
            {
                _listTarget[_index] = value;
            }
            
        }
        System.Type _targetType;

        object[] emptyList = new object[0];

        public void SetList(IList list)
        {
            _listTarget = list;
        }
        public void SetTargetIndex(int i)
        {
            _index = i;
            if (target == null) throw new System.ArgumentNullException();
            _targetType = target.GetType();
        }

        protected void UpdateChild(Element elem)
        {
            if (target == null) return;
            if (elem.children.Count > 0)
            {
                for(int i = 0; i < elem.children.Count; i += 1)
                {
                    UpdateChild(elem.children[i]);
                }
                return;
            }
            string name = elem.name;
            PropertyInfo prop;
            FieldInfo field;
            MethodInfo method;
            if ((prop = _targetType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static))!= null)
            {
                elem.SetProperty("value", prop.GetValue(target, emptyList));
                return;
            }
            else if ((field = _targetType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) != null)
            {
                object val = field.GetValue(target);
                //Debug.Log("setting field value = " + val.ToString());
                elem.SetProperty("value", val);
                //Debug.Log("just set field value to = " + children[i].GetProperty("value"));
                return;
            }
            else if ((method = _targetType.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) != null)
            {
                VoidCallback callback;
                if (method.IsStatic)
                {
                    callback = (VoidCallback)System.Delegate.CreateDelegate(typeof(VoidCallback), method);
                }
                else
                {
                    callback = (VoidCallback)System.Delegate.CreateDelegate(typeof(VoidCallback), target, method);
                }

                elem.SetProperty("delegate", callback);
            }

        }
        protected void UpdateTargetField(Element elem)
        {
            if (elem.children.Count > 0)
            {
                for (int i = 0; i < elem.children.Count; i += 1)
                {
                    UpdateTargetField(elem.children[i]);
                }
                return;
            }
            object childValue = elem.GetProperty("value");
            string name = elem.name;
            object targ = target;
            PropertyInfo prop;
            FieldInfo field;
            if (( prop = _targetType.GetProperty(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) != null)
            {
                if (prop.GetValue(targ, emptyList) != childValue)
                {
                    prop.SetValue(targ, childValue, emptyList);
                }
            }
            else if ((field = _targetType.GetField(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static)) != null)
            {
                field.SetValue(targ, childValue);
            } else
            {
                // Do nothing, not a field or prop
            }
            target = targ;

        }
        protected void DrawChild(Element elem)
        {
            UpdateChild(elem);
            elem.Draw();
            UpdateTargetField(elem);
        }
        protected override void PreGUI()
        {
            
        }
        protected override void OnGUI()
        {
            if (target == null) return;
            
            for(int i = 0; i < children.Count; i += 1)
            {
                DrawChild(children[i]);
            }
            
        }
        protected override void PostGUI()
        {
            base.PostGUI();
        }
    }
}

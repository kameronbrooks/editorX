using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public abstract class ValueElement : Element
    {

        public abstract T GetValue<T>();

        public abstract object GetValue();

        public abstract void SetValue(object val);
    }
}

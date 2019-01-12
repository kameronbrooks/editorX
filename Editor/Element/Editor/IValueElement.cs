using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    public interface IValueElement : IElement
    {
        T GetValue<T>();
        object GetValue();

        void SetValue(object val);
        System.Type valueType { get; }
    }

}
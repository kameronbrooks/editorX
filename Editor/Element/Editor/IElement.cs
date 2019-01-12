using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorX
{
    public delegate void EventCallback(IElement elem, Event e);
    
    public interface IElement
    {

        void Draw();
        void AddChild(IElement child);
        void AddText(string text);

        IElement GetChildById(string name);

        string name { get; set; }
        IElement parent { get; set; }
        List<IElement> children { get; }
        Rect rect { get; }
        string tag { get; }

        Style style { get; set; }

        void OnWindowLostFocus();
        void OnWindowFocus();
        void AddEventListener(string eventType, EventCallback callback);
        void RemoveEventListener(string eventType, EventCallback callback);

        void OnSerialize(SerializedElementQueue list);
        void OnDeserialize(SerializedElementQueue list);

    }
}

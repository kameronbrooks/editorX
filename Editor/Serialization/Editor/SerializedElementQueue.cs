using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public class SerializedElementQueue
    {
        [SerializeField]
        private List<SerializedElement> _data;

        [SerializeField]
        private int _startIndex;

        public SerializedElementQueue()
        {
            _data = new List<SerializedElement>();
            _startIndex = 0;
        }

        public int Count
        {
            get
            {
                return _data.Count;
            }
        }

        public void Clear()
        {
            _data.Clear();
            _startIndex = 0;
        }

        public void Enqueue(SerializedElement serial)
        {
            _data.Add(serial);
        }

        public SerializedElement Dequeue()
        {
            return _data[_startIndex++];
        }

        public bool isEmpty()
        {
            return _startIndex >= _data.Count;
        }
    }
}
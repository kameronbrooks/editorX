using System;
using System.Collections.Generic;
using UnityEngine;

namespace EditorX
{
    [System.Serializable]
    public class SerializedElement
    {
        [System.Serializable]
        public class SerializedObject
        {
            [SerializeField]
            private string _name;

            [SerializeField]
            private UnityEngine.Object _reference;

            public string name
            {
                get
                {
                    return _name;
                }
            }

            public UnityEngine.Object reference
            {
                get
                {
                    return _reference;
                }
            }

            public SerializedObject(string name, UnityEngine.Object ob)
            {
                _name = name;
                _reference = ob;
            }
        }

        [System.Serializable]
        public partial class SerializedValue
        {
            [SerializeField]
            private string _fieldName;

            [SerializeField]
            private byte[] _bytes;

            public string name
            {
                get
                {
                    return _fieldName;
                }
            }

            public SerializedValue(string name)
            {
                _fieldName = name;
            }

            public SerializedValue(string name, bool val)
            {
                _fieldName = name;
                _bytes = BitConverter.GetBytes(val);
            }

            public SerializedValue(string name, int val)
            {
                _fieldName = name;
                _bytes = BitConverter.GetBytes(val);
            }

            public SerializedValue(string name, float val)
            {
                _fieldName = name;
                _bytes = BitConverter.GetBytes(val);
            }

            public SerializedValue(string name, string val)
            {
                _fieldName = name;
                _bytes = System.Text.Encoding.Unicode.GetBytes(val);
            }

            public SerializedValue(string name, object val)
            {
                _fieldName = name;
                _bytes = BinarySerializer.GetBytes(val);
            }

            public T GetObject<T>() where T : new()
            {
                return BinarySerializer.GetClass<T>(_bytes);
            }

            public bool GetBool(int index = 0)
            {
                return BitConverter.ToBoolean(_bytes, index);
            }

            public int GetInt(int index = 0)
            {
                return BitConverter.ToInt32(_bytes, index);
            }

            public uint GetUInt(int index = 0)
            {
                return BitConverter.ToUInt32(_bytes, index);
            }

            public long GetLong(int index = 0)
            {
                return BitConverter.ToInt64(_bytes, index);
            }

            public ulong GetUlong(int index = 0)
            {
                return BitConverter.ToUInt64(_bytes, index);
            }

            public float GetFloat(int index = 0)
            {
                return BitConverter.ToSingle(_bytes, index);
            }

            public double GetDouble(int index = 0)
            {
                return BitConverter.ToDouble(_bytes, index);
            }

            public string GetString(int index = 0)
            {
                return (string)BinarySerializer.GetPrimitive(_bytes, typeof(string), ref index);
            }
 
            public T[] GetArray<T>(int index = 0)
            {
                return (T[])BinarySerializer.GetPrimitive(_bytes, typeof(T[]), ref index);
            }
        }

        [SerializeField]
        private List<SerializedValue> _values;

        [SerializeField]
        private List<SerializedObject> _objects;

        public SerializedElement()
        {
            _values = new List<SerializedValue>();
            _objects = new List<SerializedObject>();
        }

        public void AddValue(SerializedValue serial)
        {
            _values.Add(serial);
        }

        public void AddReference(SerializedObject serial)
        {
            _objects.Add(serial);
        }

        public SerializedValue GetValue(string name)
        {
            for (int i = 0; i < _values.Count; i += 1)
            {
                if (_values[i].name == name) return _values[i];
            }
            return null;
        }

        public UnityEngine.Object GetReference(string name)
        {
            for (int i = 0; i < _values.Count; i += 1)
            {
                if (_objects[i].name == name) return _objects[i].reference;
            }
            return null;
        }
    }
}
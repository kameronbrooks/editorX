using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

namespace EditorX
{
    public static class BinarySerializer
    {
        public static byte[] GetBytes(bool val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(byte val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(short val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(ushort val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(int val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(uint val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(float val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(double val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(long val)
        {
            return BitConverter.GetBytes(val);
        }
        public static byte[] GetBytes(string val)
        {
            byte[] buffer = new byte[sizeof(int) + val.Length * sizeof(char)];
            Array.Copy(BitConverter.GetBytes(val.Length), buffer, sizeof(int));
            byte[] buffer2 = System.Text.Encoding.Unicode.GetBytes(val);
            Array.Copy(buffer2, 0, buffer, sizeof(int), buffer2.Length);
            return buffer;
        }
        public static byte[] GetBytes(IList list)
        {
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < list.Count; i += 1)
            {
                bytes.AddRange(GetBytes(list[i]));
            }
            return bytes.ToArray();
        }
        public static byte[] GetBytes(Vector2 val)
        {
            byte[] buffer = new byte[sizeof(float) * 2];
            Array.Copy(GetBytes(val.x), 0, buffer, 0, sizeof(float));
            Array.Copy(GetBytes(val.y), sizeof(float), buffer, 0, sizeof(float));
            return buffer;
        }
        public static byte[] GetBytes(Vector3 val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.x));
            buffer.AddRange(GetBytes(val.y));
            buffer.AddRange(GetBytes(val.z));
            return buffer.ToArray();
        }
        public static byte[] GetBytes(Vector4 val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.x));
            buffer.AddRange(GetBytes(val.y));
            buffer.AddRange(GetBytes(val.z));
            buffer.AddRange(GetBytes(val.w));
            return buffer.ToArray();
        }
        public static byte[] GetBytes(Color val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.r));
            buffer.AddRange(GetBytes(val.g));
            buffer.AddRange(GetBytes(val.b));
            buffer.AddRange(GetBytes(val.a));
            return buffer.ToArray();
        }
        public static byte[] GetBytes(Rect val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.x));
            buffer.AddRange(GetBytes(val.y));
            buffer.AddRange(GetBytes(val.width));
            buffer.AddRange(GetBytes(val.height));
            return buffer.ToArray();
        }
        public static byte[] GetBytes(Quaternion val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.x));
            buffer.AddRange(GetBytes(val.y));
            buffer.AddRange(GetBytes(val.z));
            buffer.AddRange(GetBytes(val.w));
            return buffer.ToArray();
        }
        public static byte[] GetBytes(object ob)
        {
            Type type = ob.GetType();
            if (type == typeof(bool)) return GetBytes((bool)ob);
            if (type == typeof(int)) return GetBytes((int)ob);
            if (type == typeof(uint)) return GetBytes((uint)ob);
            if (type == typeof(long)) return GetBytes((long)ob);
            if (type == typeof(ulong)) return GetBytes((ulong)ob);
            if (type == typeof(float)) return GetBytes((float)ob);
            if (type == typeof(double)) return GetBytes((double)ob);
            if (type == typeof(string)) return GetBytes((string)ob);
            if (type == typeof(Vector2)) return GetBytes((Vector2)ob);
            if (type == typeof(Vector3)) return GetBytes((Vector3)ob);
            if (type == typeof(Vector4)) return GetBytes((Vector4)ob);
            if (type == typeof(Color)) return GetBytes((Color)ob);
            if (type == typeof(Rect)) return GetBytes((Rect)ob);
            if (type == typeof(Quaternion)) return GetBytes((Quaternion)ob);


            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public);
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < fields.Length; i += 1)
            {
                bytes.AddRange(GetBytes(fields[i].GetValue(ob)));
            }
            return bytes.ToArray();
        }

        
        public static object GetPrimitive(byte[] data, Type type, ref int index)
        {
            if (type == typeof(bool)) {
                object result = BitConverter.ToBoolean(data, index);
                index += sizeof(bool);
                return result;
            }
            if (type == typeof(int))
            {
                object result = BitConverter.ToInt32(data, index);
                index += sizeof(int);
                return result;
            }
            if (type == typeof(uint))
            {
                object result = BitConverter.ToUInt32(data, index);
                index += sizeof(uint);
                return result;
            }
            if (type == typeof(long))
            {
                object result = BitConverter.ToInt64(data, index);
                index += sizeof(long);
                return result;
            }
            if (type == typeof(ulong))
            {
                object result = BitConverter.ToUInt64(data, index);
                index += sizeof(ulong);
                return result;
            }
            if (type == typeof(float))
            {
                object result = BitConverter.ToSingle(data, index);
                index += sizeof(float);
                return result;
            }
            if (type == typeof(double))
            {
                object result = BitConverter.ToDouble(data, index);
                index += sizeof(double);
                return result;
            }
            if (type == typeof(string))
            {
                int length = BitConverter.ToInt32(data, index);
                Debug.Log("String: length = " + length);
                index += sizeof(int);
                Debug.Log("String: bytes = " + data.Length);
                object result = BitConverter.ToString(data, index, length * sizeof(char));
                index += length * sizeof(char);
                return result;
            }
            if (type.IsArray)
            {
                int length = BitConverter.ToInt32(data, index);
                index += sizeof(int);
                IList list = Array.CreateInstance(type.GetElementType(), length);
                for(int i = 0; i < length; i += 1)
                {
                    list[i] = GetPrimitive(data, type.GetElementType(), ref index);
                }
                return list;
            }
            
            throw new Exception(type.Name + " is not a supported primitive");
        }

        public static T GetObject<T>(byte[] bytes, ref T t)
        {
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public);
            int index = 0;
            for (int i = 0; i < fields.Length; i += 1)
            {
                Type fieldType = fields[i].FieldType;
                fields[i].SetValue(t, GetPrimitive(bytes, fieldType, ref index));
            }

            return t;
        }
        public static T GetObject<T>(byte[] bytes) where T:new()
        {
            T t = new T();
            FieldInfo[] fields = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Public);
            int index = 0;
            for (int i = 0; i < fields.Length; i += 1)
            {
                Type fieldType = fields[i].FieldType;
                fields[i].SetValue(t, GetPrimitive(bytes, fieldType, ref index));
            }

            return t;
        }

    }

}
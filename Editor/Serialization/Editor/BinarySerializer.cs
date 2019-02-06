using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

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

        public static byte[] GetBytes(RectOffset val)
        {
            List<byte> buffer = new List<byte>();
            buffer.AddRange(GetBytes(val.left));
            buffer.AddRange(GetBytes(val.right));
            buffer.AddRange(GetBytes(val.top));
            buffer.AddRange(GetBytes(val.bottom));
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

        public static byte[] GetBytes(System.Enum val, Type enumType)
        {
            return GetBytes((int)System.Enum.ToObject(enumType, val));
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
            if (type == typeof(RectOffset)) return GetBytes((RectOffset)ob);
            if (type == typeof(Quaternion)) return GetBytes((Quaternion)ob);
            if (type.IsEnum) return GetBytes((int)ob);

            FieldInfo[] fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Public);
            List<byte> bytes = new List<byte>();
            for (int i = 0; i < fields.Length; i += 1)
            {
                bytes.AddRange(GetBytes(fields[i].GetValue(ob)));
            }
            return bytes.ToArray();
        }

        public static bool GetBool(byte[] data, ref int index)
        {
            bool result = BitConverter.ToBoolean(data, index);
            index += sizeof(bool);
            return result;
        }

        public static char GetChar(byte[] data, ref int index)
        {
            char result = BitConverter.ToChar(data, index);
            index += sizeof(char);
            return result;
        }

        public static short GetShort(byte[] data, ref int index)
        {
            short result = BitConverter.ToInt16(data, index);
            index += sizeof(short);
            return result;
        }

        public static ushort GetUShort(byte[] data, ref int index)
        {
            ushort result = BitConverter.ToUInt16(data, index);
            index += sizeof(ushort);
            return result;
        }

        public static int GetInt(byte[] data, ref int index)
        {
            int result = BitConverter.ToInt32(data, index);
            index += sizeof(int);
            return result;
        }

        public static System.Enum GetEnum(byte[] data, Type enumType, ref int index)
        {
            int result = BitConverter.ToInt32(data, index);
            Debug.Log("enum result =" + result);
            index += sizeof(int);

            return (System.Enum)System.Enum.ToObject(enumType, result);
        }

        public static uint GetUInt(byte[] data, ref int index)
        {
            uint result = BitConverter.ToUInt32(data, index);
            index += sizeof(uint);
            return result;
        }

        public static long GetLong(byte[] data, ref int index)
        {
            long result = BitConverter.ToInt64(data, index);
            index += sizeof(long);
            return result;
        }

        public static ulong GetULong(byte[] data, ref int index)
        {
            ulong result = BitConverter.ToUInt64(data, index);
            index += sizeof(ulong);
            return result;
        }

        public static float GetFloat(byte[] data, ref int index)
        {
            float result = BitConverter.ToSingle(data, index);
            index += sizeof(float);
            return result;
        }

        public static double GetDouble(byte[] data, ref int index)
        {
            double result = BitConverter.ToDouble(data, index);
            index += sizeof(double);
            return result;
        }

        public static string GetString(byte[] data, ref int index)
        {
            int length = BitConverter.ToInt32(data, index);
            index += sizeof(int);

            string result = System.Text.Encoding.Unicode.GetString(data, index, length * sizeof(char));
            index += length * sizeof(char);
            return result;
        }

        public static Vector2 GetVector2(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            return new Vector2(x, y);
        }

        public static Vector3 GetVector3(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            float z = GetFloat(data, ref index);
            return new Vector3(x, y, z);
        }

        public static Vector4 GetVector4(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            float z = GetFloat(data, ref index);
            float w = GetFloat(data, ref index);
            return new Vector4(x, y, z, w);
        }

        public static Color GetColor(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            float z = GetFloat(data, ref index);
            float w = GetFloat(data, ref index);
            return new Color(x, y, z, w);
        }

        public static Rect GetRect(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            float z = GetFloat(data, ref index);
            float w = GetFloat(data, ref index);
            return new Rect(x, y, z, w);
        }

        public static RectOffset GetRectOffset(byte[] data, ref int index)
        {
            int x = GetInt(data, ref index);
            int y = GetInt(data, ref index);
            int z = GetInt(data, ref index);
            int w = GetInt(data, ref index);
            return new RectOffset(x, y, z, w);
        }

        public static Quaternion GetQuaternion(byte[] data, ref int index)
        {
            float x = GetFloat(data, ref index);
            float y = GetFloat(data, ref index);
            float z = GetFloat(data, ref index);
            float w = GetFloat(data, ref index);
            return new Quaternion(x, y, z, w);
        }

        public static object GetPrimitive(byte[] data, Type type, ref int index)
        {
            if (type == typeof(bool)) return GetBool(data, ref index);
            if (type == typeof(char)) return GetBool(data, ref index);
            if (type == typeof(short)) return GetShort(data, ref index);
            if (type == typeof(ushort)) return GetUShort(data, ref index);
            if (type == typeof(int)) return GetInt(data, ref index);
            if (type == typeof(uint)) return GetUInt(data, ref index);
            if (type == typeof(long)) return GetLong(data, ref index);
            if (type == typeof(ulong)) return GetULong(data, ref index);
            if (type == typeof(float)) return GetFloat(data, ref index);
            if (type == typeof(double)) return GetDouble(data, ref index);
            if (type == typeof(string)) return GetString(data, ref index);
            if (type == typeof(Vector2))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                return new Vector2(x, y);
            }
            if (type == typeof(Vector3))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                float z = GetFloat(data, ref index);
                return new Vector3(x, y, z);
            }
            if (type == typeof(Vector4))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                float z = GetFloat(data, ref index);
                float w = GetFloat(data, ref index);
                return new Vector4(x, y, z, w);
            }
            if (type == typeof(Color))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                float z = GetFloat(data, ref index);
                float w = GetFloat(data, ref index);
                return new Color(x, y, z, w);
            }
            if (type == typeof(Rect))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                float z = GetFloat(data, ref index);
                float w = GetFloat(data, ref index);
                return new Rect(x, y, z, w);
            }
            if (type == typeof(RectOffset)) return GetRectOffset(data, ref index);
            if (type == typeof(Quaternion))
            {
                float x = GetFloat(data, ref index);
                float y = GetFloat(data, ref index);
                float z = GetFloat(data, ref index);
                float w = GetFloat(data, ref index);
                return new Quaternion(x, y, z, w);
            }
            if (type.IsArray)
            {
                int length = GetInt(data, ref index);
                IList list = Array.CreateInstance(type.GetElementType(), length);
                for (int i = 0; i < length; i += 1)
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

        public static T GetObject<T>(byte[] bytes) where T : new()
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
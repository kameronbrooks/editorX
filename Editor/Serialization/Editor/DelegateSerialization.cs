using UnityEngine;

namespace EditorX
{
    public static class DelegateSerialization
    {
        [System.Serializable]
        public class SerializedDelegate
        {
            [SerializeField]
            public string eventname;

            [SerializeField]
            public UnityEngine.Object[] targets;

            [SerializeField]
            public string[] methods;
        }

        public static SerializedDelegate SerializeDelegate(EventCallback callback, string eventname)
        {
            System.Delegate[] delegates = callback.GetInvocationList();
            SerializedDelegate serial = new SerializedDelegate()
            {
                eventname = eventname,
                targets = new Object[delegates.Length],
                methods = new string[delegates.Length]
            };
            for (int i = 0; i < delegates.Length; i += 1)
            {
                serial.targets[i] = (UnityEngine.Object)delegates[i].Target;
                serial.methods[i] = delegates[i].Method.Name;
            }

            return serial;
        }

        public static EventCallback DeserializeDelegate(SerializedDelegate serial)
        {
            EventCallback callback = null;

            for (int i = 0; i < serial.methods.Length; i += 1)
            {
                EventCallback subCallback = (EventCallback)System.Delegate.CreateDelegate(typeof(EventCallback), serial.targets[i], serial.methods[i]);
                callback += subCallback;
            }
            return callback;
        }
    }
}
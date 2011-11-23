using System;
using System.Collections.Generic;

namespace org.vxwo.JsonCsharp
{
    internal enum JsonType
    {
        None,
        Boolean,
        Int,
        Long,
        Double,
        String,
        Object,
        Array,
        Null,
    }

    public class JsonValue
    {
        internal JsonType type;
        internal object store;

        public JsonValue()
        {
            type = JsonType.None;
            store = null;
        }

        internal JsonValue(JsonType type, object obj)
        {
            this.type = type;
            this.store = obj;
        }

        public void Clear()
        {
            type = JsonType.None;
            store = null;
        }

        #region Object Methods

        private Dictionary<string, JsonValue> EnsureObject()
        {
            if (type != JsonType.None && type != JsonType.Object)
                throw new Exception("JsonValue not a object");

            if (type == JsonType.None)
            {
                type = JsonType.Object;
                store = new Dictionary<string, JsonValue>();
            }
            return store as Dictionary<string, JsonValue>;
        }

        public JsonValue this[string name]
        {
            get
            {
                JsonValue result;
                Dictionary<string, JsonValue> obj = EnsureObject();
                if (!obj.TryGetValue(name, out result))
                {
                    result = new JsonValue();
                    obj.Add(name, result);
                }
                return result;
            }

            set
            {
                EnsureObject().Add(name, value);
            }
        }

        public bool IsMember(string name)
        {
            return EnsureObject().ContainsKey(name);
        }

        public void RemoveMember(string name)
        {
            EnsureObject().Remove(name);
        }

        #endregion

        #region Array Methods

        private List<JsonValue> EnsureArray()
        {
            if (type != JsonType.None && type != JsonType.Array)
                throw new Exception("JsonValue not a array");

            if (type == JsonType.None)
            {
                type = JsonType.Array;
                store = new List<JsonValue>();
            }
            return store as List<JsonValue>;
        }

        public int Count()
        {
            return EnsureArray().Count;
        }

        public JsonValue GetAt(int index)
        {
            return EnsureArray()[index];
        }

        public JsonValue Append(JsonValue value)
        {
            EnsureArray().Add(value);
            return this;
        }

        #endregion

        #region Read Methods

        public bool IsBoolean()
        {
            return type == JsonType.Boolean;
        }

        public bool IsInt()
        {
            return type == JsonType.Int;
        }

        public bool IsLong()
        {
            return type == JsonType.Long;
        }

        public bool IsDouble()
        {
            return type == JsonType.Double;
        }

        public bool IsString()
        {
            return type == JsonType.String;
        }

        public bool AsBoolean()
        {
            return IsString() ? bool.Parse((string)store) : (bool)store;
        }

        public int AsInt()
        {
            return IsString() ? int.Parse((string)store) : (int)store;
        }

        public long AsLong()
        {
            return IsString() ? long.Parse((string)store) : (long)store;
        }

        public double AsDouble()
        {
            return IsString() ? double.Parse((string)store) : (double)store;
        }

        public string AsString()
        {
            return IsString() ? (string)store : Convert.ToString(store);
        }

        #endregion

        #region operator Methods

        public static implicit operator JsonValue(bool value)
        {
            return new JsonValue(JsonType.Boolean, value);
        }

        public static implicit operator JsonValue(int value)
        {
            return new JsonValue(JsonType.Int, value);
        }

        public static implicit operator JsonValue(long value)
        {
            return new JsonValue(JsonType.Long, value);
        }

        public static implicit operator JsonValue(double value)
        {
            return new JsonValue(JsonType.Double, value);
        }

        public static implicit operator JsonValue(string value)
        {
            return new JsonValue(JsonType.String, value);
        }

        #endregion

    }
}

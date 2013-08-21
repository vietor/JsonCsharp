﻿using System;
using System.Collections.Generic;

namespace JsonCsharp
{
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
            if (this.type == JsonType.Array && obj == null)
                this.store = new List<JsonValue>();
            else
                this.store = obj;
        }
		
		internal bool IsZero()
		{
            if (IsNumberic())
                return Convert.ToUInt64(store) == 0;
			return false;
		}

        public void Clear()
        {
            type = JsonType.None;
            store = null;
        }

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
                if(value == null)
                    value = new JsonValue(JsonType.Null, null);
                EnsureObject().Add(name, value);
            }
        }

        public JsonValue this[int index]
        {
            get
            {
                return GetAt(index);
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

        public int Count
        {
            get
            {
                if (type != JsonType.Array)
                    return 0;
                return EnsureArray().Count;
            }
        }

        internal JsonValue GetAt(int index)
        {
            return EnsureArray()[index];
        }

        public JsonValue Append(JsonValue value)
        {
            if(value != null)
            	EnsureArray().Add(value);
            return this;
        }
        
        public bool IsNull()
        {
            return type == JsonType.Null || type == JsonType.None;
        }
	
        public bool IsBoolean()
        {
            return type == JsonType.Boolean;
        }

        public bool IsNumberic()
        {
            return type == JsonType.Int || type == JsonType.UInt
                || type == JsonType.Long || type == JsonType.ULong
                || type == JsonType.Double;
        }

        public bool IsString()
        {
            return type == JsonType.String;
        }

        public bool AsBoolean()
        {
            bool value = false;
            if (IsBoolean())
                value = (bool)store;
            else if (IsString())
                value = bool.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a bool");
            return value;
        }

        public int AsInt()
        {
            int value = 0;
            if (IsNumberic())
                value = Convert.ToInt32(store);
            else if (IsString())
                value = int.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a int");
            return value;
        }

        public uint AsUInt()
        {
            uint value = 0;
            if (IsNumberic())
                value = Convert.ToUInt32(store);
            else if (IsString())
                value = uint.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a uint");
            return value;
        }

        public long AsLong()
        {
            long value = 0;
            if (IsNumberic())
                value = Convert.ToInt64(store);
            else if (IsString())
                value = long.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a long");
            return value;
        }

        public ulong AsULong()
        {
            ulong value = 0;
            if (IsNumberic())
                value = Convert.ToUInt64(store);
            else if (IsString())
                value = ulong.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a ulong");
            return value;
        }

        public double AsDouble()
        {
            double value = 0;
            if (IsNumberic())
                value = Convert.ToDouble(store);
            else if (IsString())
                value = double.Parse((string)store);
            else
                throw new Exception("JsonValue cannot convert to a double");
            return value;
        }

        public string AsString()
        {
            return IsString() ? (string)store : Convert.ToString(store);
        }

        public static implicit operator JsonValue(bool value)
        {
            return new JsonValue(JsonType.Boolean, value);
        }

        public static implicit operator JsonValue(int value)
        {
            return new JsonValue(JsonType.Int, value);
        }

        public static implicit operator JsonValue(uint value)
        {
            return new JsonValue(JsonType.UInt, value);
        }

        public static implicit operator JsonValue(long value)
        {
            return new JsonValue(JsonType.Long, value);
        }

        public static implicit operator JsonValue(ulong value)
        {
            return new JsonValue(JsonType.ULong, value);
        }

        public static implicit operator JsonValue(double value)
        {
            return new JsonValue(JsonType.Double, value);
        }

        public static implicit operator JsonValue(string value)
        {
            return new JsonValue(JsonType.String, value);
        }

    }
}

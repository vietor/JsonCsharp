
namespace JsonCsharp
{
    public class JsonMapper
    {
        public static JsonValue ToValue(object obj, bool ignoreAttribute = true)
        {
            if (obj is string)
                return new JsonParser((string)obj).Decode();
            else
                return new JsonObjParser(obj, ignoreAttribute).Decode();
        }

        public static T ToObject<T>(string json, bool ignoreAttribute = true)
        {
            return ToObject<T>(ToValue(json), ignoreAttribute);
        }

        public static T ToObject<T>(JsonValue value, bool ignoreAttribute = true)
        {
            return (T)new JsonObjSerializer(ignoreAttribute).ConvertToObject(typeof(T), value);
        }

        public static string ToJson(object obj, bool ignoreAttribute = true)
        {
            JsonValue value;
            if (obj is JsonValue)
                value = (JsonValue)obj;
            else
                value = ToValue(obj, ignoreAttribute);
            return new JsonSerializer(true, true).ConvertToJSON(value);
        }

        public static string ToJsonShrink(object obj, bool ignoreAttribute = true)
        {
            JsonValue value;
            if (obj is JsonValue)
                value = (JsonValue)obj;
            else
                value = ToValue(obj, ignoreAttribute);
            return new JsonSerializer(false, false).ConvertToJSON(value);
        }
    }
}


namespace JsonCsharp
{
    public class JsonMapper
    {
        public static T ToObject<T>(string json, bool ignoreAttribute = true)
        {
            return JsonWriter.WriteObject<T>(JsonReader.Read(json), ignoreAttribute);
        }

        public static T ToObject<T>(JsonValue value, bool ignoreAttribute = true)
        {
            return JsonWriter.WriteObject<T>(value, ignoreAttribute);
        }

        public static string ToJson(object obj, bool ignoreAttribute = true)
        {
            if (obj is JsonValue)
                return JsonWriter.Write((JsonValue)obj);
            return JsonWriter.Write(JsonReader.ReadObject(obj, ignoreAttribute));
        }

        public static string ToJsonShrink(object obj, bool ignoreAttribute = true)
        {
            if (obj is JsonValue)
                return JsonWriter.WriteShrink((JsonValue)obj);
            return JsonWriter.WriteShrink(JsonReader.ReadObject(obj, ignoreAttribute));
        }
    }
}

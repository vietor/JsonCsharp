
namespace JsonCsharp
{
    public class JsonMapper
    {
        public static T ToObject<T>(string json)
        {
            return JsonWriter.Write<T>(JsonReader.Read(json));
        }

        public static T ToObject<T>(JsonValue value)
        {
            return JsonWriter.Write<T>(value);
        }

        public static string ToJson(object obj)
        {
            if (obj is JsonValue)
                return JsonWriter.Write((JsonValue)obj);
            return JsonWriter.Write(JsonReader.Read(obj));
        }

        public static string ToJsonShrink(object obj)
        {
            if (obj is JsonValue)
                return JsonWriter.WriteShrink((JsonValue)obj);
            return JsonWriter.WriteShrink(JsonReader.Read(obj));
        }
    }
}

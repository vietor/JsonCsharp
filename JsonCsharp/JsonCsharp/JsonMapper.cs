
namespace org.vxwo.csharp.json
{
    public class JsonMapper
    {
        public static T ToObject<T>(string json)
        {
            return JsonWriter.Write<T>(JsonReader.Read(json));
        }

        public static string ToJson(object obj)
        {
            return JsonWriter.Write(JsonReader.Read(obj));
        }

        public static string ToJsonShrink(object obj)
        {
            return JsonWriter.WriteShrink(JsonReader.Read(obj));
        }
    }
}

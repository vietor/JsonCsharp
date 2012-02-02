
namespace org.vxwo.csharp.json
{
    public class JsonWriter
    {
        public static string Write(JsonValue obj)
        {
            return new JsonSerializer(true, false).ConvertToJSON(obj);
        }
    }
}

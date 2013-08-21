using System;

namespace JsonCsharp
{
    public class JsonWriter
    {
        public static string Write(JsonValue obj)
        {
            return new JsonSerializer(true, true).ConvertToJSON(obj);
        }
		
		public static string WriteShrink(JsonValue obj)
        {
            return new JsonSerializer(false, false).ConvertToJSON(obj);
        }

        public static T WriteObject<T>(JsonValue obj, bool ignoreAttribute)
        {
            return (T)new JsonObjSerializer(ignoreAttribute).ConvertToObject(typeof(T), obj);
        }
    }
}

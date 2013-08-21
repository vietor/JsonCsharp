
namespace JsonCsharp
{
	public class JsonReader
	{
		public static JsonValue Read (string json)
		{
			return new JsonParser (json).Decode ();
		}

        public static JsonValue ReadObject(object obj, bool ignoreAttribute)
		{
            return new JsonObjParser(obj, ignoreAttribute).Decode();
		}
	}
}

using System;
using System.Reflection;

namespace org.vxwo.csharp.json
{
	public class JsonObjSerializer
	{
		internal JsonObjSerializer ()
		{
		}
		
		internal T ConvertToObject<T> (JsonValue obj)
		{
			return (T)WriteObject (typeof(T), obj);
		}
		
		private object WriteObject (Type type, JsonValue obj)
		{
			if (type.IsArray) {
				if (obj.Count < 1 || !type.HasElementType)
					return null;
				Type etype = type.GetElementType ();
				Array array = Array.CreateInstance (etype, obj.Count);
				for (int i=0; i< obj.Count; i++) {
					array.SetValue (WriteObject (etype, obj.GetAt (i)), i);
				}
				return array;
			}
			
			if (type.Name.Equals ("Boolean"))
				return obj.AsBoolean ();
			if (type.Name.Equals ("Int32"))
				return obj.AsInt ();
			if (type.Name.Equals ("Int64"))
				return obj.AsLong ();
			if (type.Name.Equals ("Single"))
				return (float)obj.AsDouble ();
			if (type.Name.Equals ("Double"))
				return obj.AsDouble ();
			if (type.Name.Equals ("String"))
				return obj.AsString ();
			
			if (type.IsEnum)
				return Enum.Parse (type, obj.AsString ());
			
			if (type.IsClass) {
			
				object result = Activator.CreateInstance (type);
				foreach (FieldInfo info in type.GetFields()) {
					if (obj.IsMember (info.Name))
						info.SetValue (result, WriteObject (info.GetType (), obj [info.Name]));
				}
				return result;
			}
			
			throw new JsonException ("JsonObjSerializer not support type: " + type.Name);
		}
	}
}


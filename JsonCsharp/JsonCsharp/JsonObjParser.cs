using System;
using System.Reflection;
using System.Collections;

namespace org.vxwo.csharp.json
{
	public class JsonObjParser
	{		
		private object root;
		
		internal JsonObjParser (object obj)
		{
			this.root = obj;
		}

		internal JsonValue Decode ()
		{
			return ParseValue (root);
		}
		
		private JsonValue ParseValue (object obj)
		{
			if (obj == null)
				return new JsonValue (JsonType.Null, null);
			
			Type type = obj.GetType ();
			
			if (type.IsArray) {
				JsonValue child, result = new JsonValue ();
				foreach (object sub in (Array)obj) {
					child = ParseValue (sub);
					if (child != null)
						result.Append (child);
				}
				return result;
			}
			
			if (type.Name.Equals ("Boolean"))
				return new JsonValue (JsonType.Boolean, obj);
			if (type.Name.Equals ("Int32"))
				return new JsonValue (JsonType.Int, obj);
			if (type.Name.Equals ("Int64"))
				return new JsonValue (JsonType.Long, obj);
			if (type.Name.Equals ("Single"))
				return new JsonValue (JsonType.Double, (double)(float)obj);
			if (type.Name.Equals ("Double"))
				return new JsonValue (JsonType.Double, obj);
			if (type.Name.Equals ("String"))
				return new JsonValue (JsonType.String, obj);
			
			if (type.IsEnum)
				return new JsonValue (JsonType.Int, Convert.ToInt32 (obj));
			
			if (type.Name.Equals("List`1")) {
				JsonValue child, result = new JsonValue ();
				foreach (object sub in (IList)obj) {
					child = ParseValue (sub);
					if (child != null)
						result.Append (child);
				}
				return result;
			}
			
			if (type.IsClass) {
				JsonValue child, result = new JsonValue ();
				foreach (FieldInfo info in type.GetFields()) {
					bool ignore = false;
					string name = info.Name;
					foreach(Attribute attr in info.GetCustomAttributes(true))
					{
						if(attr is JsonIgnore)
						{
							ignore=true;
							break;
						}
						else if(attr is JsonName)
							name = ((JsonName)attr).GetName();
					}
					if(ignore)
						continue;
					child = ParseValue (info.GetValue (obj));
					if (child != null)
						result [name] = child;
				}
				return result;
			}
			
			throw new JsonException ("JsonObjParser not support type: " + type.Name);
		}
	}
}
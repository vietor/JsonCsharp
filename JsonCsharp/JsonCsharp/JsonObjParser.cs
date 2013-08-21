using System;
using System.Reflection;
using System.Collections;

namespace JsonCsharp
{
	class JsonObjParser
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
			
			if (type.Name.Equals ("Char"))
				return new JsonValue (JsonType.Int, Convert.ToInt32(obj));
			if (type.Name.Equals ("Byte"))
				return new JsonValue (JsonType.Int, Convert.ToInt32(obj));
			if (type.Name.Equals ("Boolean"))
				return new JsonValue (JsonType.Boolean, obj);
			if (type.Name.Equals ("Int32"))
				return new JsonValue (JsonType.Int, obj);
            if (type.Name.Equals("UInt32"))
                return new JsonValue(JsonType.UInt, obj);
			if (type.Name.Equals ("Int64"))
				return new JsonValue (JsonType.Long, obj);
            if (type.Name.Equals("UInt64"))
                return new JsonValue(JsonType.ULong, obj);
			if (type.Name.Equals ("Single"))
				return new JsonValue (JsonType.Double, (double)obj);
			if (type.Name.Equals ("Double"))
				return new JsonValue (JsonType.Double, obj);
			if (type.Name.Equals ("String"))
				return new JsonValue (JsonType.String, obj);
			if (type.Name.Equals ("DateTime"))
				return new JsonValue (JsonType.String, string.Format("{0:yyyy-MM-dd HH:mm:ss}",(DateTime)obj));
            if (type.Name.Equals("List`1"))
            {
                JsonValue child, result = new JsonValue(JsonType.Array, null);
                foreach (object sub in (IList)obj)
                {
                    child = ParseValue(sub);
                    if (child != null)
                        result.Append(child);
                }
                return result;
            }

            if (type.IsArray)
            {
                JsonValue child, result = new JsonValue(JsonType.Array, null);
                foreach (object sub in (Array)obj)
                {
                    child = ParseValue(sub);
                    if (child != null)
                        result.Append(child);
                }
                return result;
            }
			
			if (type.IsEnum)
				return new JsonValue (JsonType.Int, Convert.ToInt32 (obj));
			
			if (type.IsClass) {
                string name;
                object[] attrs;
				JsonValue child, result = new JsonValue ();
				foreach(PropertyInfo info in type.GetProperties())
				{
					if(!info.CanRead)
						continue;
                    if (info.IsDefined(typeof(JsonIgnore), false))
                        continue;
					attrs = info.GetCustomAttributes(typeof(JsonName), false);
                    if(attrs != null && attrs.Length > 0)
                        name = ((JsonName)attrs[0]).GetName();
                    else
                        name = info.Name;
					child = ParseValue (info.GetValue(obj, null));
					if (child != null)
						result [name] = child;
				}
				foreach (FieldInfo info in type.GetFields()) {
					if(!info.IsPublic || info.IsLiteral)
						continue;
                    if (info.IsDefined(typeof(JsonIgnore), false))
                        continue;
                    attrs = info.GetCustomAttributes(typeof(JsonName), false);
                    if (attrs != null && attrs.Length > 0)
                        name = ((JsonName)attrs[0]).GetName();
                    else
                        name = info.Name;
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

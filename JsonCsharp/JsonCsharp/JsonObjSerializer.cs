using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

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
				for (int i=0; i< obj.Count; i++)
					array.SetValue (WriteObject (etype, obj.GetAt (i)), i);
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
			
			if (type.Name.Equals("List`1")) {
				Type[] types = type.GetGenericArguments();
				if(types == null || types.Length<1)
					return null;
				Type etype = types[0];
				IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(etype));
				for (int i=0; i< obj.Count; i++)
					list.Add(WriteObject (etype, obj.GetAt (i)));
				return list;
			}
			
			if (type.IsClass) {			
				object result = Activator.CreateInstance (type);
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
					if (obj.IsMember (info.Name))	
						info.SetValue (result, WriteObject (info.FieldType, obj [info.Name]));
				}
				return result;
			}
			
			throw new JsonException ("JsonObjSerializer not support type: " + type.Name);
		}
	}
}


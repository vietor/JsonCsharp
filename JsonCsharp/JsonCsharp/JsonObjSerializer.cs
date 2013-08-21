using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

namespace JsonCsharp
{
	class JsonObjSerializer
	{
        private bool ignoreAttribute;

		internal JsonObjSerializer (bool ignoreAttribute)
		{
            this.ignoreAttribute = ignoreAttribute;
		}
		
		internal object ConvertToObject (Type type, JsonValue obj)
		{
			return WriteObject (type, obj);
		}
		
		private object WriteObject (Type type, JsonValue obj)
		{
			if(obj == null || obj.IsNull())
				return null;
			
			if (type.Name.Equals ("Char"))
				return Convert.ToChar(obj.AsInt ());
			if (type.Name.Equals ("Byte"))
				return Convert.ToByte(obj.AsInt ());
			if (type.Name.Equals ("Boolean"))
				return obj.AsBoolean ();
			if (type.Name.Equals ("Int32"))
				return obj.AsInt ();
            if (type.Name.Equals("UInt32"))
                return obj.AsUInt();
			if (type.Name.Equals ("Int64"))
				return obj.AsLong ();
            if (type.Name.Equals("UInt64"))
                return obj.AsULong();
			if (type.Name.Equals ("Single"))
				return (float)obj.AsDouble ();
			if (type.Name.Equals ("Double"))
				return obj.AsDouble ();
			if (type.Name.Equals ("String"))
				return obj.AsString ();
			if (type.Name.Equals ("DateTime"))
			{
				try{
					return DateTime.Parse(obj.AsString());	
				}catch(FormatException){
					return null;
				}
			}
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

            if (type.IsArray)
            {
                if (!type.HasElementType)
                    return null;
                Type etype = type.GetElementType();
                Array array = Array.CreateInstance(etype, obj.Count);
                for (int i = 0; i < obj.Count; i++)
                    array.SetValue(WriteObject(etype, obj.GetAt(i)), i);
                return array;
            }

            if (type.IsEnum)
                return Enum.Parse(type, obj.AsString());
			
			if (type.IsClass) {
                string name;
                object[] attrs;
				object result = Activator.CreateInstance (type);
				foreach (PropertyInfo info in type.GetProperties()) {
					if(!info.CanWrite)
						continue;
                    if (ignoreAttribute)
                        name = info.Name;
                    else
                    {
                        if (info.IsDefined(typeof(JsonIgnore), false))
                            continue;
                        attrs = info.GetCustomAttributes(typeof(JsonName), false);
                        if (attrs != null && attrs.Length > 0)
                            name = ((JsonName)attrs[0]).GetName();
                        else
                            name = info.Name;
                    }
					if (obj.IsMember (name))
						info.SetValue (result, WriteObject (info.PropertyType, obj [name]), null);
				}
				foreach (FieldInfo info in type.GetFields()) {
					if(!info.IsPublic || info.IsLiteral)
						continue;
                    if (ignoreAttribute)
                        name = info.Name;
                    else
                    {
                        if (info.IsDefined(typeof(JsonIgnore), false))
                            continue;
                        attrs = info.GetCustomAttributes(typeof(JsonName), false);
                        if (attrs != null && attrs.Length > 0)
                            name = ((JsonName)attrs[0]).GetName();
                        else
                            name = info.Name;
                    }
					if (obj.IsMember (name))	
						info.SetValue (result, WriteObject (info.FieldType, obj [name]));
				}
				return result;
			}
			
			throw new JsonException ("JsonObjSerializer not support type: " + type.Name);
		}
	}
}


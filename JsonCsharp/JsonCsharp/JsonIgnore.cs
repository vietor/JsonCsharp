using System;

namespace JsonCsharp
{
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = false)]
	
	public sealed class JsonIgnore: Attribute
	{
		public JsonIgnore ()
		{
		}
	}
}

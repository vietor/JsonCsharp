using System;

namespace org.vxwo.csharp.json
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	
	public sealed class JsonIgnore: Attribute
	{
		public JsonIgnore ()
		{
		}
	}
}

using System;

namespace org.vxwo.csharp.json
{
	[AttributeUsage(AttributeTargets.Field|AttributeTargets.Property, AllowMultiple = true)]
	
	public sealed class JsonAlias: Attribute
	{
		private string name;
		
		public JsonAlias ()
		{
		}
		
		public JsonAlias (string name)
		{
			this.name = name;
		}
		
		public string GetName()
		{
			return name;
		}
	}
}


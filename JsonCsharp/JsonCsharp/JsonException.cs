using System;

namespace JsonCsharp
{
	public class JsonException: Exception
	{
		public JsonException (string message)
			: base(message)
		{			
		}
	}
}


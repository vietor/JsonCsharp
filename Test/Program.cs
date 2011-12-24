using System;
using System.Collections.Generic;
using System.Text;
using org.vxwo.csharp.json;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            JsonValue value = new JsonValue();
            value["id"] = (JsonValue)"32";
            value["ids"].Append(value["id"]);
            value["ids"].Append(value["id"]);
            value.RemoveMember("id");
            string json = JsonWriter.Write(value);

            Console.ReadKey();
        }
    }
}

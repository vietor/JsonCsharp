using System;
using System.Collections.Generic;
using System.Text;
using org.vxwo.csharp.json;

namespace Test
{
    class Program
    {
		public class TA{
			public int c= 100;
			public  int d= 1000;
		}
		class A {
			public int a;
			public int B;
			public int[] cs = new int[3];
			public List<TA> tttt = new List<TA>();
			
			public A()
			{
				TA ta= new TA();
				tttt.Add(ta);
				tttt.Add(ta);
			}
		};
		
		public enum C {
			CA = 0,
			CB,
		};
		
        static void Main(string[] args)
        {
            JsonValue value = new JsonValue();
            value["id"] = (JsonValue)"32";
            value["ids"].Append(value["id"]);
            value["ids"].Append(value["id"]);
            value.RemoveMember("id");
            string json = JsonWriter.Write(value);
           	Console.WriteLine(json);
			
			A a = new A();			
			value = JsonReader.Read(a);
			json = JsonWriter.Write(value);
           	Console.WriteLine(json);
        }
    }
}

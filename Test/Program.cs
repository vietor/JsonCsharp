using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using JsonCsharp;

namespace Test
{
	class Program
	{
		public enum C
		{
			CA = 0,
			CB,
		};
		
		public class TA
		{
			public int c = 100;
			public uint d = 1000;
		}

		class A
		{
			[JsonName("1")]
			public int a = 1;
			
			[JsonName("2")]
			public int B = 3;
			
			[JsonIgnore]
			public C c = C.CB;
			
			public int[] cs = new int[3];
			public List<TA> tttt = new List<TA>();
			
			public uint[] cs1 = null;
			public List<TA> tttt1 = null;
			
			public DateTime tt = DateTime.Now;
			public string tt2 = null;
			
			public int TQ;
			
			private int dtq=1;
			
			public const int dc = 12;
			
			[JsonName("tq1")]
			public int TQ1 {get {return dtq;} set {dtq = value;}}
			
			public char Car = (char)11;
			public byte Bar = (byte)12;
			public char[] Carr= new char[3];
			public byte[] Barr= new byte[3];
			
			public int q1=0;
			public int q2=0;
			
			public A ()
			{
				TA ta= new TA();
				tttt.Add(ta);
				tttt.Add(ta);
			}
		};
		
		static void Test1()
		{
			Console.WriteLine ("== Basic ==");
			JsonValue value = new JsonValue ();
			value ["id"] = (JsonValue)"32";
			value ["ids"].Append (value ["id"]);
			value ["ids"].Append (value ["id"]);
			value.RemoveMember ("id");
			value["ttt"] = null;
			string json = JsonWriter.Write (value);
			Console.WriteLine (json);
			
			Console.WriteLine ("== Object to Json ==");
			A a = new A ();	
			a.a = 11;
			a.B = 22;
			value = JsonReader.Read (a);
			json = JsonWriter.Write (value);
			Console.WriteLine (json);
			
			Console.WriteLine ("== Object to Json2 ==");
            json = JsonMapper.ToJson(JsonMapper.ToObject<A>(json));
			Console.WriteLine (json);
			
			Console.WriteLine ("== Json to Object ==");
			a = JsonWriter.Write<A>(value);
			a.a = 33;
			a.B = 44;
			a.c = C.CA;
			a.cs[1]=55;
			a.cs1 = new uint[0];
			a.tttt1 = new List<TA>();
			a.TQ = 0;
			a.TQ1 = 999999;
			value = JsonReader.Read (a);
			json = JsonWriter.Write (value);
			Console.WriteLine (json);
						
			Console.WriteLine ("== Json to Object2 (Shrink) ==");
            json = JsonMapper.ToJsonShrink(JsonMapper.ToObject<A>(json));
			Console.WriteLine (json);
			Console.WriteLine ("== Json to Object2 (Normal) ==");
            json = JsonMapper.ToJson(JsonMapper.ToObject<A>(json));
			Console.WriteLine (json);

            Console.ReadKey();
		}


        static void Test2()
        {
            A a = new A();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            for (int i = 0; i < 20000; ++i)
            {
                JsonValue value = JsonReader.Read(a);
                String json = JsonWriter.Write(value);
            }
            sw.Stop();

            Console.WriteLine(String.Format("times: {0}ms", sw.ElapsedMilliseconds));
        }

        static void Main(string[] args)
        {
            Test2();

            Console.ReadKey();
        }
	}
}

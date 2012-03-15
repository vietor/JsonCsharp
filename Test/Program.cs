﻿using System;
using System.Collections.Generic;
using System.Text;
using org.vxwo.csharp.json;

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
			public  int d = 1000;
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
			
			public A ()
			{
				TA ta= new TA();
				tttt.Add(ta);
				tttt.Add(ta);
			}
		};
		
		static void Main (string[] args)
		{
			Console.WriteLine ("== Basic ==");
			JsonValue value = new JsonValue ();
			value ["id"] = (JsonValue)"32";
			value ["ids"].Append (value ["id"]);
			value ["ids"].Append (value ["id"]);
			value.RemoveMember ("id");
			string json = JsonWriter.Write (value);
			Console.WriteLine (json);
			
			Console.WriteLine ("== Object to Json ==");
			A a = new A ();	
			a.a = 11;
			a.B = 22;
			value = JsonReader.Read (a);
			json = JsonWriter.Write (value);
			Console.WriteLine (json);
			
			Console.WriteLine ("== Json to Object ==");
			a = JsonWriter.Write<A>(value);
			a.a = 33;
			a.B = 44;
			a.c = C.CA;
			a.cs[1]=55;
			value = JsonReader.Read (a);
			json = JsonWriter.Write (value);
			Console.WriteLine (json);
		}
	}
}

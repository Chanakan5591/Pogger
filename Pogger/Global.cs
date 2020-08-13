using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using Discord;

namespace Pogger
{
	public static class Global
	{
		public static string Token { get; set; }
		public static string Prefix { get; set; }
		public static String Status { get; set; }
		public static ulong ModLogChannel { get; set; }

		public static void ReadConfig()
		{
			dynamic data = JsonConvert.DeserializeObject(File.ReadAllText($"{Environment.CurrentDirectory}/data.json"));
			Token = data.Token;
			ModLogChannel = data.ModLogID;
			Prefix = data.Prefix;
			Status = data.Status;
		}

	}

}
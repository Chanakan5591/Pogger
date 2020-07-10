using Newtonsoft.Json;
using System;
using System.IO;
using Discord.WebSocket;

public class Global
{
	public static string Token { get; set; }
	public static string Prefix { get; set; }

	public static DiscordSocketClient Client { get; set; }

	public static String Status { get; set; }
	public static void ReadConfig()
    {
		dynamic data = JsonConvert.DeserializeObject(File.ReadAllText($"{Environment.CurrentDirectory}/data.json"));
		Token = data.Token;
		Prefix = data.Prefix;
		Status = data.Status;
    }
	
}


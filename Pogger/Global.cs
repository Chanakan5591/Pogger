using Newtonsoft.Json;
using System;
using System.IO;

public class Global
{
	public static string Token { get; set; }
	
	public static void ReadConfig()
    {
		dynamic data = JsonConvert.DeserializeObject(File.ReadAllText($"{Environment.CurrentDirectory}/data.json"));
		Token = data.Token;
    }
	
}


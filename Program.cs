using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace ConsoleTests
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            string keyPath = "api_key.json";
            string json = File.ReadAllText(keyPath);
            var jsonObject = JsonSerializer.Deserialize<GetAPI>(json);
            string QUERY_URL = "https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=IBM&apikey=demo";

            using (HttpClient client = new HttpClient())
            {
                // Use GetStringAsync and await its result.
                string? jsonString = await client.GetStringAsync(QUERY_URL);

                // Specify the correct namespace if both Newtonsoft.Json and System.Text.Json are referenced.
                dynamic? json_data = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, dynamic>>(jsonString);

                // do something with the json_data
            }
        }
    }
    public class GetAPI
    {
        public string? API_KEY { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Linq;

string keyPath = "api_key.json";
string json = File.ReadAllText(keyPath);
var jsonObject = JsonSerializer.Deserialize<GetAPI>(json);
string QUERY_URL = $"https://www.alphavantage.co/query?function=TIME_SERIES_DAILY&symbol=SPY&apikey={jsonObject?.api_key}";

using (HttpClient client = new HttpClient())
{
    // Use GetStringAsync and await its result.
    string? jsonString = await client.GetStringAsync(QUERY_URL);

    // Deserialize the JSON response into a StockData object.
    var stockData = JsonSerializer.Deserialize<StockData>(jsonString);

    List<decimal> closingPrices = new List<decimal>();

    // Loop through the daily quotes and print out the closing price for each day.
    if (stockData != null && stockData.TimeSeriesDaily != null)
    {
        foreach (var entry in stockData.TimeSeriesDaily.Take(20))
        {
            if (decimal.TryParse(entry.Value.Close, out decimal closePrice))
            {
                closingPrices.Add(closePrice);
            }
            else
            {
                // Handle the case where the string could not be parsed to a decimal
            }
        }
    }
}

public class GetAPI
{
    public string? api_key { get; set; }
}
public class StockData
{
    [JsonPropertyName("Meta Data")]
    public MetaData? MetaData { get; set; }

    [JsonPropertyName("Time Series (Daily)")]
    public Dictionary<string, DailyQuote>? TimeSeriesDaily { get; set; }
}
public class MetaData
{
    [JsonPropertyName("1. Information")]
    public string? Information { get; set; }

    [JsonPropertyName("2. Symbol")]
    public string? Symbol { get; set; }

    [JsonPropertyName("3. Last Refreshed")]
    public string? LastRefreshed { get; set; }

    [JsonPropertyName("4. Output Size")]
    public string? OutputSize { get; set; }

    [JsonPropertyName("5. Time Zone")]
    public string? TimeZone { get; set; }
}

public class DailyQuote
{
    [JsonPropertyName("1. open")]
    public string? Open { get; set; }

    [JsonPropertyName("2. high")]
    public string? High { get; set; }

    [JsonPropertyName("3. low")]
    public string? Low { get; set; }

    [JsonPropertyName("4. close")]
    public string? Close { get; set; }

    [JsonPropertyName("5. volume")]
    public string? Volume { get; set; }
}

using System;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace email_app_api.Core
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Topic
    {
        Weather,
        Sport,
        Stops
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SportOptions
    {
        PremierLeague,
        EFL
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum WeatherOptions
    {
        London,
        Minsk,
        Prague
    }

    public class CustomHttpRequestMessage : HttpRequestMessage
    {
        public CustomHttpRequestMessage(Models.Task task, HttpRequestMessageData httpRequestMessageData)
        {
            //validate if such Topic exists in dictionary
            Method = HttpMethod.Get;
            var uri = string.Format(httpRequestMessageData.RequestUrl, task.Option);
            RequestUri = new Uri(uri);
            Headers.Add("X-RapidAPI-Key", "82ae6d5081msh502b62fd786c6e5p138581jsn5887b17fe4a1");
            Headers.Add("X-RapidAPI-Host", httpRequestMessageData.Host);
        }
    }
}

﻿using System;
using System.IO;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Http;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Converters;

namespace email_app_api.Services
{
    public class ApiEmailService
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public enum Topic
        {
            Weather,
            Languages,
            Stops
        }

        private HttpRequestMessage CreateWeatherRequest() => new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://weatherapi-com.p.rapidapi.com/forecast.json?q=London&days=3"),
            Headers =
            {
                { "X-RapidAPI-Key", "82ae6d5081msh502b62fd786c6e5p138581jsn5887b17fe4a1" },
                { "X-RapidAPI-Host", "weatherapi-com.p.rapidapi.com" },
            },
        };

        private HttpRequestMessage CreateLanguagesRequest() => new HttpRequestMessage
        {
            RequestUri = new Uri("https://google-translate1.p.rapidapi.com/language/translate/v2/languages?target=en"),
	        Headers =
	        {
		        { "X-RapidAPI-Key", "82ae6d5081msh502b62fd786c6e5p138581jsn5887b17fe4a1" },
		        { "X-RapidAPI-Host", "google-translate1.p.rapidapi.com" },
	        },
        };

        private HttpRequestMessage CreateStopsRequest() => new HttpRequestMessage
        {
	        Method = HttpMethod.Get,
	        RequestUri = new Uri("https://transloc-api-1-2.p.rapidapi.com/stops.json?agencies=12%2C16&geo_area=35.80176%2C-78.64347%7C35.78061%2C-78.68218&callback=call"),
	        Headers =
	        {
		        { "X-RapidAPI-Key", "82ae6d5081msh502b62fd786c6e5p138581jsn5887b17fe4a1" },
		        { "X-RapidAPI-Host", "transloc-api-1-2.p.rapidapi.com" },
	        },
        };

        public async Task SendEmailAsync(string email, Topic api)
        {
            HttpRequestMessage request = api switch
            {
                Topic.Weather => CreateWeatherRequest(),
                Topic.Languages => CreateLanguagesRequest(),
                Topic.Stops =>  CreateStopsRequest(),
                _ => null
            };
            HttpClient client = new HttpClient();
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                Stream data = response.Content.ReadAsStream();

                await SendEmailAsync(email, "", "Hey, have a good day!", data);
            }
        }

        private async Task SendEmailAsync(string email, string subject, string message, Stream file)
        {
            MimeMessage emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Email service", "tanjamaltzevatanja@yandex.ru"));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;

            TextPart body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = message
            };

            MimePart attachment = new MimePart("text/csv", "csv")
            {
                Content = new MimeContent(file),
                ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                ContentTransferEncoding = ContentEncoding.Base64,
                FileName = "requested data.csv"
            };

            Multipart multipart = new Multipart("mixed");
            multipart.Add(body);
            multipart.Add(attachment);
            emailMessage.Body = multipart;

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync("smtp.yandex.ru", 25, false);
                await client.AuthenticateAsync("tanjamaltzevatanja", "skjdivnskjvb");
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}

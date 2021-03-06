using System;
using System.IO;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Http;
using email_app_api.Core;
using Microsoft.Extensions.Options;
using Microsoft.Data.Sqlite;
using Aspose.Cells;
using Aspose.Cells.Utility;

namespace email_app_api.Services
{
    public class ApiEmailService
    {
        private readonly string connectionString;

        public ApiEmailService(IOptions<EmailAppDbOptions> dbOptions)
        {
            connectionString = dbOptions.Value.ConnectionString;
        }

        public void SendEmail(string email, Models.Task task)
        {
            HttpRequestMessage request = new CustomHttpRequestMessage(
                task,
                GetApi((Topic)Enum.Parse(typeof(Topic), task.Topic, true))
            );

            HttpClient client = new HttpClient();
            using var response = client.Send(request);
            response.EnsureSuccessStatusCode();
            MemoryStream convertedResponseStream = ConvertJsonToCsv(response.Content.ReadAsStream());
            SendEmail(email, "", "Hey, have a good day!", convertedResponseStream);
        }

        private MemoryStream ConvertJsonToCsv(Stream responseStream)
        {
            var workbook = new Workbook();
            var worksheet = workbook.Worksheets[0];
            var layoutOptions = new JsonLayoutOptions();
            layoutOptions.ArrayAsTable = true;
            StreamReader reader = new StreamReader(responseStream);
            string text = reader.ReadToEnd();
            JsonUtility.ImportData(text, worksheet.Cells, 0, 0, layoutOptions);
            return workbook.SaveToStream();
        }

        private HttpRequestMessageData GetApi(Topic topic)
        {
            string sqlExpression = $"SELECT * FROM HttpRequestMessageData Where Topic = \"{topic}\"";
            using var connection = new SqliteConnection(connectionString);
            connection.Open();
            SqliteCommand command = new SqliteCommand(sqlExpression, connection);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    reader.Read();
                    return new HttpRequestMessageData()
                    {
                        Topic = reader.GetString(0),
                        RequestUrl = reader.GetString(1),
                        Host = reader.GetString(2)
                    };
                }
            return null;
            }
        }

        private void SendEmail(string email, string subject, string message, Stream file)
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
                client.Connect("smtp.yandex.ru", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
                client.Authenticate("tanjamaltzevatanja", "skjdivnskjvb");
                client.Send(emailMessage);
                client.Disconnect(true);
            }
        }
    }
}

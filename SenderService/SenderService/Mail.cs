using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Build.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using SendGrid;
using SendGrid.Helpers.Mail;
using Task = System.Threading.Tasks.Task;

namespace SenderService
{
    public class Mail
    {
        public async Task CreateMailAsync(string toEmail, string fromEmail, string mailName, string subject, string plainText, string content)
        {
            var apiKey = "klucz";
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, mailName);
            var to = new EmailAddress(toEmail);
            //    var plainTextContent = Regex.Replace("sadasd", "<[^>]*>", "");
            var plainTextContent = plainText;
            var htmlContent = content;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var asd = client.SendEmailAsync(msg).Result;
        }

        public static async System.Threading.Tasks.Task<List<string>> GetMailListAsync()
        {
            var list = new List<string>();

            var acc = new CloudStorageAccount(
                new StorageCredentials("warsztaty", "klucz"), false);
            var tableClient = acc.CreateCloudTableClient();
            var table = tableClient.GetTableReference("peopleTable");
            TableQuery<EmailEntity> query = new TableQuery<EmailEntity>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, "Smith"));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<EmailEntity> resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                foreach (EmailEntity entity in resultSegment.Results)
                {
                    list.Add(entity.PartitionKey);
                }
            } while (token != null);

            return list;
        }
    }
    public class EmailEntity : TableEntity
    {
        public EmailEntity(string email)
        {
            this.PartitionKey = email;
            this.RowKey = email;
        }

        public EmailEntity() { }
    }
}
//https://docs.microsoft.com/pl-pl/azure/visual-studio/vs-storage-aspnet5-getting-started-tables
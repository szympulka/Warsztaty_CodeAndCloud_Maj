using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageService
{
    public class StorageService
    {
        private CloudStorageAccount storageAccount;
        private CloudTableClient tableClient;
        private CloudTable table;
        public StorageService(string accountName, string key)
        {
            storageAccount = new CloudStorageAccount(
                new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                    accountName, key), true);
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference("peopleTable");
            var res = table.CreateIfNotExistsAsync().Result;
        }

        public async Task AddAsync(EmailEntity entity)
        {
            TableOperation insertOperation = TableOperation.Insert(entity);
            await table.ExecuteAsync(insertOperation);
        }

        public async Task RemoveAsync(string email)
        {
            TableOperation retrieveOperation = TableOperation.Retrieve<EmailEntity>(email, email);

            TableResult retrievedResult = await table.ExecuteAsync(retrieveOperation);

            EmailEntity deleteEntity = (EmailEntity)retrievedResult.Result;

            TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

            await table.ExecuteAsync(deleteOperation);
        }
    }
}

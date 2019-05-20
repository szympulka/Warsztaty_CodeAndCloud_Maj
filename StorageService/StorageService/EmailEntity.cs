using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace StorageService
{
    public class EmailEntity: TableEntity
    {
        public EmailEntity(string email)
        {
            this.PartitionKey = email;
            this.RowKey = email;
        }

        public EmailEntity() { }
    }
}


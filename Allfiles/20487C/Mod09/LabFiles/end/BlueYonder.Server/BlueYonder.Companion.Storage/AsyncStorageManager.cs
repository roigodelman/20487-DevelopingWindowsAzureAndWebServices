using BlueYonder.Companion.Storage.TableEntities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Storage
{
    public class AsyncStorageManager
    {
        private CloudStorageAccount _account;
        private static readonly string MetadataTable = "FilesMetadata";

        public Uri BlobAddress
        {
            get { return _account.BlobEndpoint; }
        }

        public AsyncStorageManager()
        {
            _account = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("BlueYonderStore"));

        }

        public async Task<Uri> UploadStreamAsync(string containerName, string fileName, Stream data, bool isPublic)
        {
            CloudBlockBlob blob = GetBlob(containerName, fileName, isPublic);
            // Note: The Task.Factory.FromAsync allows us to create a Task that represent the async operation
            // that is implemented using the Asynchronous Programming Model. this will allow any code calling 
            // the UploadPublicStreamAsync to use the await keyword and a more readable asynchronous code
            await Task.Factory.FromAsync(blob.BeginUploadFromStream,
                                         blob.EndUploadFromStream,
                                         data,
                                         null,
                                         TaskCreationOptions.None);
            return blob.Uri;
        }

        public async Task SaveMetadataAsync(FileEntity fileData)
        {
            CloudTable table = GetTable();

            // TODO: Lab 9, Exercise 2: Task 1.3: use a CloudTable and TableOperation to add the object
            TableOperation insert = TableOperation.Insert(fileData);
            await table.ExecuteAsync(insert);
        }

        private CloudTable GetTable()
        {
            CloudTableClient client = _account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference(MetadataTable);
            table.CreateIfNotExists();
            return table;
        }

        public IEnumerable<FileEntity> GetLocationMetadata(string locationId)
        {
            CloudTable table = GetTable();
            TableQuery<FileEntity> query = new TableQuery<FileEntity>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, locationId));
            return table.ExecuteQuery(query);
        }

        public IEnumerable<FileEntity> GetFilesMetadata(IEnumerable<string> rowKeys)
        {

            CloudTable table = GetTable();
            
            foreach (var rowKey in rowKeys)
            {
                TableQuery<FileEntity> query = new TableQuery<FileEntity>().Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
                yield return table.ExecuteQuery(query).Single();
            }

        }      

        public string CreateSharedAccessSignature(string containerName)
        {
            SharedAccessBlobPolicy policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.Now.AddMinutes(1)
            };
            CloudBlobContainer container = GetContainer(containerName);
            var permissions = container.GetPermissions();
            permissions.SharedAccessPolicies.Add("blueyonder", policy);
            container.SetPermissions(permissions);
            return container.GetSharedAccessSignature(policy);
        }

        public IEnumerable<Uri> GetFileUris(string containerName)
        {
            var container = GetContainer(containerName);
            return container.ListBlobs().Select(item => item.Uri);
        }

        private CloudBlobContainer GetContainer(string containerName)
        {
            CloudBlobClient client = _account.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference(containerName);
            container.CreateIfNotExists();
            return container;
        }

        public CloudBlockBlob GetBlob(string containerName, string fileName, bool isPublic)
        {
            CloudBlobContainer container = GetContainer(containerName);
            if (isPublic)
            {
                BlobContainerPermissions permissions = new BlobContainerPermissions();
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                container.SetPermissions(permissions);
            }
            return container.GetBlockBlobReference(fileName);
        }
    }
}
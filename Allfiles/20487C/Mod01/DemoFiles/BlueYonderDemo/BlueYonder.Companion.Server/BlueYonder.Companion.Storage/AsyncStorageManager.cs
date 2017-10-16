using BlueYonder.Companion.Storage.TableEntities;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Configuration;
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
            // TODO: Exercise 1: Task 2a: Create the _account member from a configuration 
            var conStr = ConfigurationManager.AppSettings["BlueYonderStore"];
            var a = CloudStorageAccount.TryParse(conStr, out _account);
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
            TableServiceContext tableContext = GetTableContext();

            // TODO: Exercise 1: Task 5c: use a TableServiceContext to add the object
            tableContext.AddObject(MetadataTable, fileData);

            await Task.Factory.FromAsync<DataServiceResponse>(tableContext.BeginSaveChanges,
                                                              tableContext.EndSaveChanges,
                                                              null,
                                                              TaskCreationOptions.None);
        }

        private TableServiceContext GetTableContext()
        {
            // TODO: Exercise 1: Task 5b: Use the _account member to get a new CloudTableClient,
            // verify that the table exists, and then return a TableServiceContext instance
            CloudTableClient tableClient = _account.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(MetadataTable);
            table.CreateIfNotExists();
            TableServiceContext tableContext = tableClient.GetTableServiceContext();
            return tableContext;
        }

        public IEnumerable<FileEntity> GetLocationMetadata(string locationId)
        {
            // TODO: Exercise 2: Task 2a: Get the table context and then use it 
            // to query the table for a specific partition according to the location
            TableServiceContext tableContext = GetTableContext();

            var query = from file in tableContext.CreateQuery<FileEntity>(MetadataTable)
                        where file.PartitionKey == locationId
                        select file;

            return query.ToList();
        }

        public IEnumerable<FileEntity> GetFilesMetadata(IEnumerable<string> rowKeys)
        {
            // TODO: Exercise 2: Task 2b: Get the table context and then use it 
            // to query the table for each row key. Use yield return to 
            // return each file entity you find in the table

            TableServiceContext tableContext = GetTableContext();
            foreach (var rowKey in rowKeys)
            {
                var fileEntity = (from file in tableContext.CreateQuery<FileEntity>(MetadataTable)
                                  where file.RowKey == rowKey
                                  select file).Single();

                yield return fileEntity;
            }
        }

        public string CreateSharedAccessSignature(string containerName)
        {
            // TODO: Exercise 3: Task 2a: Create a new SharedAccessBlobPolicy with Read permissions and a one hour expiration
            // then apply the policy to the container by using the container's SetPermissions method
            // Return a shared access signature for the 
            var policy = new SharedAccessBlobPolicy()
            {
                Permissions = SharedAccessBlobPermissions.Read,
                SharedAccessExpiryTime = DateTime.UtcNow.AddHours(1)
            };

            var container = GetContainer(containerName);
            BlobContainerPermissions blobPermissions = new BlobContainerPermissions();
            blobPermissions.SharedAccessPolicies.Add("blueyonder", policy);
            container.SetPermissions(blobPermissions);

            return container.GetSharedAccessSignature(policy);
        }

        public IEnumerable<Uri> GetFileUris(string containerName)
        {
            // TODO: Exercise 1: Task 3c: Use the GetContainer to get a CloudBlobContainer,
            // and then use the ListBlobs to get a list of blob items. Return the URI of each blob item
            var container = GetContainer(containerName);

            return container.ListBlobs().Select(item => item.Uri);
        }

        private CloudBlobContainer GetContainer(string containerName)
        {
            // TODO: Exercise 1: Task 3b: Get a container reference and make sure it is created 
            var blobClient = _account.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(containerName);
            container.CreateIfNotExists();
            return container;
        }

        private CloudBlockBlob GetBlob(string containerName, string fileName, bool isPublic)
        {
            // TODO: Exercise 1: Task 3d: Use the GetContainer to get a CloudBlobContainer,
            // make the blob public if requested, and use the container to get a CloudBlob reference 
            CloudBlobContainer container = GetContainer(containerName);
            if (isPublic)
            {
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            }
            return container.GetBlockBlobReference(fileName);

        }
    }
}
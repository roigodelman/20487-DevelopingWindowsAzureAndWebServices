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
            throw new NotImplementedException();
        }

        private CloudTable GetTable()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileEntity> GetLocationMetadata(string locationId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FileEntity> GetFilesMetadata(IEnumerable<string> rowKeys)
        {

            CloudTable table = GetTable();
            return null;
//            foreach (var rowKey in rowKeys)
//            {
//                TableQuery<FileEntity> query = new TableQuery<FileEntity>();
//                    .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, rowKey));
//                yield return table.ExecuteQuery(query).Single();
//            }

        }      

        public string CreateSharedAccessSignature(string containerName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Uri> GetFileUris(string containerName)
        {
            var container = GetContainer(containerName);
            return container.ListBlobs().Select(item => item.Uri);
        }

        private CloudBlobContainer GetContainer(string containerName)
        {
            throw new NotImplementedException();
        }

        public CloudBlockBlob GetBlob(string containerName, string fileName, bool isPublic)
        {
            throw new NotImplementedException();
        }
    }
}
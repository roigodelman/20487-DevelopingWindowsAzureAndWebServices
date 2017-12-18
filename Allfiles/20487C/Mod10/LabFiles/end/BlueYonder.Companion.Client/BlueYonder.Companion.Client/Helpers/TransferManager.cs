using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Shared;
using Windows.Networking.BackgroundTransfer;
using Windows.Storage;

namespace BlueYonder.Companion.Client.Helpers
{
    public class TransferManager
    {
        private readonly DataManager _data = new DataManager();

        public async Task UploadAsync(StorageFile sourceFile, int travelerId, int reservationId, bool isPrivate, int locationId)
        {
            var address = isPrivate ? Addresses.UploadPrivateFileUri : Addresses.UploadPublicFileUri;
            Uri destinationUri = new Uri(string.Format(address, reservationId));

            BackgroundUploader uploader = new BackgroundUploader();
            uploader.SetRequestHeader("Filename", sourceFile.Name);
            UploadOperation upload = uploader.CreateUpload(destinationUri, sourceFile);
            await upload.StartAsync();

            var fileAddress = upload.GetResponseInformation().Headers["Location"];

            await _data.CreateAzureStorageFileMetadata(new Uri(fileAddress), sourceFile.Name, reservationId, isPrivate, travelerId, locationId);
        }

        public async Task DownloadAsync(Uri sourceUri, StorageFile destinationFile)
        {
            BackgroundDownloader downloader = new BackgroundDownloader();
            DownloadOperation download = downloader.CreateDownload(sourceUri, destinationFile);
            await download.StartAsync();
        }

        public async Task ResumeDownloadsAsync()
        {
            var downloads = await BackgroundDownloader.GetCurrentDownloadsAsync();
            if (downloads.Count > 0)
            {
                foreach (DownloadOperation download in downloads)
                {
                    if (download.Progress.Status != BackgroundTransferStatus.Running)
                    {
                        download.Resume();
                    }
                }
            }
        }
    }
}
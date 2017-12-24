using BlueYonder.Companion.Controllers.DataTransferObjects;
using BlueYonder.Companion.Storage;
using BlueYonder.Companion.Storage.TableEntities;
using BlueYonder.DataAccess.Interfaces;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace BlueYonder.Companion.Controllers
{
    [RoutePrefix("Files")]
    public class FilesController : ApiController
    {
        public IReservationRepository Reservations { get; set; }

        public FilesController(IReservationRepository reservations)
        {
            Reservations = reservations;
        }


        [HttpPost]
        [Route("Public")]
        public async Task<HttpResponseMessage> Public([FromUri] int tripId)
        {
            HttpResponseMessage response = await UploadFile(tripId, false);
            return response;
        }

        [HttpPost]
        [Route("Private")]
        public async Task<HttpResponseMessage> Private([FromUri]int tripId)
        {
            HttpResponseMessage response = await UploadFile(tripId, true);
            return response;
        }

        [HttpPost]
        [Route("Metadata")]
        public async Task Metadata(FileDto fileData)
        {
            var storageManager = new AsyncStorageManager();

            int locationId = Reservations.GetTrip(fileData.TripId).FlightInfo.Flight.Destination.LocationId;
            FileEntity entity = CreateFileEntity(fileData, locationId);
            await storageManager.SaveMetadataAsync(entity);
        }

        private FileEntity CreateFileEntity(FileDto fileData, int locationId)
        {
            
            var entity = new FileEntity
            {
                TripId = fileData.TripId,
                Uri = fileData.Uri,
                Type = (int)fileData.Type,
                Description = fileData.Description,
                IsPrivate = fileData.IsPrivate,
                UserId = fileData.UserId,
                FileName = fileData.FileName
            };

            // TODO: Lab 9, Exercise 2, Task 1.4: Set the entity's partition key and row key
            return entity;

        }

        [HttpGet]
        [Route("TripMetadata/{id}")]
        public List<FileDto> TripMetadata(int id)
        {
            var storageManager = new AsyncStorageManager();
            var publicUris = storageManager.GetFileUris(GetContainer(id, false));
            // TODO: Lab 9, Exercise 3, Task 1.4: get a list of files in the trip's private folder 

            var allKeys = publicUris.Select(u => HttpUtility.UrlEncode(u.ToString()));
            var result = from f in storageManager.GetFilesMetadata(allKeys)
                         select ToFileDto(storageManager, f);

            return result.ToList();
        }

        [HttpGet]
        [Route("LocationMetadata/{id}")]
        public List<FileDto> LocationMetadata(int id)
        {
            var storageManager = new AsyncStorageManager();

            var result = from f in storageManager.GetLocationMetadata(id.ToString())
                         where !f.IsPrivate
                         select ToFileDto(storageManager, f);

            return result.ToList();

        }

        public static FileDto ToFileDto(AsyncStorageManager storageManager, FileEntity file)
        {

            FileDto result = new FileDto
            {
                Description = file.Description,
                FileName = file.FileName,

                //LocationId = int.Parse(file.PartitionKey),

                TripId = file.TripId,
                Type = (FileType)file.Type,
                IsPrivate = file.IsPrivate,
            };
            string containerName = GetContainer(result.TripId, result.IsPrivate);
            CloudBlob blob = storageManager.GetBlob(containerName, file.FileName, file.IsPrivate);
            if (result.IsPrivate)
            {
                result.Uri = new Uri(blob.Uri, storageManager.CreateSharedAccessSignature(containerName));
            }
            else
            {
                
                result.Uri = blob.Uri;
            }

            return result;
        }
     
        private static string GetContainer(int tripId, bool IsPrivate)
        {
            if (IsPrivate)
                return tripId + "-private";
            else
                return tripId + "-public";
        }

        private async Task<HttpResponseMessage> UploadFile(int tripId, bool isPrivate)
        {
            // Extracting the filename and content from the HttpRequestMessage
            var stream = await Request.Content.ReadAsStreamAsync();
            var filename = Request.Headers.Where(kv => kv.Key == "Filename").First().Value.First();

            var storageManager = new AsyncStorageManager();

            var uri = await storageManager.UploadStreamAsync(GetContainer(tripId, isPrivate), filename, stream, !isPrivate);

            var response = new HttpResponseMessage(HttpStatusCode.Created);
            response.Headers.Location = uri;
            return response;
        }
    }
}
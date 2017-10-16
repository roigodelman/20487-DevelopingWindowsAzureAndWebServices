using BlueYonder.Companion.Controllers.DataTransferObjects;
using BlueYonder.Companion.Storage;
using BlueYonder.Companion.Storage.TableEntities;
using BlueYonder.DataAccess.Interfaces;
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
    public class FilesController : ApiController
    {
        public IReservationRepository Reservations { get; set; }

        public FilesController(IReservationRepository reservations)
        {
            Reservations = reservations;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Public([FromUri]int tripId)
        {
            HttpResponseMessage response = await UploadFile(tripId, false);
            return response;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Private([FromUri]int tripId)
        {
            HttpResponseMessage response = await UploadFile(tripId, true);
            return response;
        }

        [HttpPost]
        public async Task Metadata(FileDto fileData)
        {
            var storageManager = new AsyncStorageManager();

            FileEntity entity = CreateFileEntity(fileData);

            // TODO: Exercise 1: Task 5e: Call the storageManagers 
            await storageManager.SaveMetadataAsync(entity);
        }

        private FileEntity CreateFileEntity(FileDto fileData)
        {
            // TODO: Exercise 1: Task 5d: create a new FileEntity based on the file data
            var entity = new FileEntity
            {
                RowKey = HttpUtility.UrlEncode(fileData.Uri.ToString()),
                PartitionKey = fileData.LocationId.ToString(),
                TripId = fileData.TripId,
                Uri = fileData.Uri,
                Type = (int)fileData.Type,
                Description = fileData.Description,
                IsPrivate = fileData.IsPrivate,
                UserId = fileData.UserId,
                FileName = fileData.FileName
            };

            return entity;
        }

        [HttpGet]
        public List<FileDto> TripMetadata(int id)
        {
            var storageManager = new AsyncStorageManager();
            var publicUris = storageManager.GetFileUris(GetContainer(id, false));

            // TODO: Exercise 3, Task 1a: get a list of files in the trip's private folder     
            var privateUris = storageManager.GetFileUris(GetContainer(id, true));
            var allUris = publicUris.Union(privateUris);
            var allKeys = allUris.Select(u => HttpUtility.UrlEncode(u.ToString()));

            var result = from f in storageManager.GetFilesMetadata(allKeys)
                         orderby f.Timestamp descending
                         select ToFileDto(storageManager, f);

            return result.ToList();
        }

        [HttpGet]
        public List<FileDto> LocationMetadata(int id, int count)
        {
            var storageManager = new AsyncStorageManager();

            var result = from f in storageManager.GetLocationMetadata(id.ToString())
                         orderby f.Timestamp descending
                         where !f.IsPrivate
                         select ToFileDto(storageManager, f);

            return result.Take(count).ToList();
        }

        public static FileDto ToFileDto(AsyncStorageManager storageManager, FileEntity file)
        {
            FileDto result = new FileDto
            {
                Description = file.Description,
                FileName = file.FileName,
                LocationId = int.Parse(file.PartitionKey),
                TripId = file.TripId,
                Type = (FileType)file.Type,
                IsPrivate = file.IsPrivate,
            };

            if (result.IsPrivate)
            {
                string containerName = GetContainer(result.TripId, result.IsPrivate);
                result.Uri = new Uri(file.Uri, storageManager.CreateSharedAccessSignature(containerName));
            }
            else
            {
                result.Uri = file.Uri;
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
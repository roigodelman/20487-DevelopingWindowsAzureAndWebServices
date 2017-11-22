using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Client.DataTransferObjects;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Client.Helpers
{
    public class DataManager
    {
        public const string BaseUri = "http://10.10.0.10/BlueYonder.Companion.Host/";

        private const string _getLocationsUri = BaseUri + "Locations";
        private const string _getLocationsWithQueryUri = _getLocationsUri + "?City={0}";
        private const string _getFlightsUri = BaseUri + "Flights?source={0}&destination={1}&date={2}";
        private const string _getFlightByIdUri = BaseUri + "Flights?id={0}";
        private const string _addReservation = BaseUri + "Reservations";
        private const string _updateReservation = BaseUri + "Reservations/{0}";
        private const string _getReservationsByTravelerUri = BaseUri + "travelers/{0}/reservations";
        private const string _getReservationByIdUri = BaseUri + "Reservations/{0}";
        private const string _getTravelerByIdentityUri = BaseUri + "Travelers/{0}";
        private const string _createTravelerUri = BaseUri + "Travelers";
        private const string _updateTravelerUri = BaseUri + "Travelers/{0}";
        private const string _getWeatherUri = BaseUri + "locations/{0}/weather?date={1}";
        private const string _registerNotificationsUri = BaseUri + "Notifications/Register";
        private const string _getFilesMetadataUri = BaseUri + "Files/Metadata?tripId={0}";

        /// <summary>
        /// Make sure Internet Connection avilable and perform HttpClient request.
        /// </summary>
        /// <param name="uri">Request Target</param>
        /// <returns>Json Response</returns>
        private async Task<Response> GetAsync(Uri uri)
        {
            var client = new HttpClient();
            var request = client.GetAsync(uri);
            return await SendRequestAsync(request);
        }

        private async Task<Response> PostAsync(Uri uri, string json)
        {
            var client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var request = client.PostAsync(uri, content);
            return await SendRequestAsync(request);
        }

        private async Task<Response> PutAsync(Uri uri, string json)
        {
            var client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var request = client.PutAsync(uri, content);
            return await SendRequestAsync(request);
        }

        private async Task<Response> DeleteAsync(Uri uri)
        {
            var client = new HttpClient();
            var request = client.DeleteAsync(uri);
            return await SendRequestAsync(request);
        }
      
        private static async Task<Response> SendRequestAsync(Task<HttpResponseMessage> request)
        {
            if (!await NetworkManager.CheckInternetConnection(true, ""))
                return null;

            try
            {
                HttpResponseMessage responseMessage = await request;
                var response = new Response()
                {
                    Content = await responseMessage.Content.ReadAsStringAsync(),
                    Success = responseMessage.IsSuccessStatusCode
                };
                return response;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("GetAsync", ex);
            }
        }

        /// <summary>
        /// Get list of Locations from Server
        /// </summary>
        /// <returns>List of locations</returns>
        public async Task<IEnumerable<Location>> GetLocationsAsync(string query)
        {
            string uri;
            if (query == null)
            {
                uri = _getLocationsUri;
            }
            else
            {
                uri = string.Format(_getLocationsWithQueryUri, query);
            }
            var response = await GetAsync(new Uri(uri));
            var locationDTOs = JsonSerializerHelper.Deserialize<IEnumerable<LocationDTO>>(response.Content);
            return locationDTOs.Select(dto => dto.ToObject()).ToArray();
        }

        /// <summary>
        /// Get the traveler associated with this machine
        /// </summary>
        /// <returns></returns>
        public async Task<Traveler> GetTravelerAsync()
        {
            // TODO: Lab 03 Exercise 2: Task 1.3: Implement the GetTravelelrAsync method         
            var hardwareId = GetHardwareId();
            HttpClient client = new HttpClient();

            var travelersUri = string.Format("{0}travelers/{1}", BaseUri, hardwareId);

            HttpResponseMessage response = await client.GetAsync(new Uri(travelersUri));
            if (response.IsSuccessStatusCode)
            {
                string resultJson = await response.Content.ReadAsStringAsync();
                return await JsonConvert.DeserializeObjectAsync<Traveler>(resultJson);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Modify the traveler details
        /// </summary>
        /// <param name="traveler">The Traveler</param>
        /// <returns></returns>
        
        // TODO: Lab 03 Exercise 2: Task 1.8: Review the UpdateTravelerAsync method
        public async Task UpdateTravelerAsync(Traveler traveler)
        {
            if (!await NetworkManager.CheckInternetConnection(true, ""))
                return;
            
            var dto = traveler.ToDTO();
            dto.TravelerUserIdentity = GetHardwareId();
            string json = JsonConvert.SerializeObject(dto);
            
            HttpClient client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var travelerUri = string.Format("{0}/travelers/{1}", BaseUri, dto.TravelerUserIdentity);
            await client.PutAsync(new Uri(travelerUri), content);
        }

        // TODO: Lab 03 Exercise 2: Task 1.6: Review the UpdateTravelerAsync method
        public async Task<Traveler> CreateTravelerAsync()
        {
            var dto = new TravelerDTO()
            {
                TravelerUserIdentity = GetHardwareId()
            };
            string json = JsonConvert.SerializeObject(dto);
            HttpClient client = new HttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(new Uri(_createTravelerUri), content);

            var resultJson = await response.Content.ReadAsStringAsync();
            return await JsonConvert.DeserializeObjectAsync<Traveler>(resultJson);


        }

        /// <summary>
        /// Get reservations by traveler id
        /// </summary>
        /// <param name="travelerId">Traveler Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Reservation>> GetReservationsAsync(int travelerId)
        {
            var uri = new Uri(string.Format(_getReservationsByTravelerUri, GetHardwareId()));
            var response = await GetAsync(uri);
            var reservations = JsonSerializerHelper.Deserialize<IEnumerable<Reservation>>(response.Content);
            return reservations.OrderBy(t => t.DepartureFlight.FlightInfo.Departure.Value);
        }

        /// <summary>
        /// Get reservation by id
        /// </summary>
        /// <param name="reservationId">Reservation Id</param>
        /// <returns></returns>
        public async Task<Reservation> GetReservationAsync(int reservationId)
        {
            var uri = new Uri(string.Format(_getReservationByIdUri, reservationId));
            var response = await GetAsync(uri);
            var reservation = JsonSerializerHelper.Deserialize<Reservation>(response.Content);
            return reservation;
        }

        /// <summary>
        /// Get list of flights
        /// </summary>
        /// <param name="source">Source Country Id</param>
        /// <param name="destination">Destination Country Id</param>
        /// <param name="startDate">Start Date</param>
        /// <returns></returns>
        public async Task<IEnumerable<Flight>> GetFlightsAsync(int source, int destination, DateTime? startDate)
        {
            var uri = new Uri(string.Format(_getFlightsUri, source, destination, startDate));
            var response = await GetAsync(uri);
            return JsonSerializerHelper.Deserialize<IEnumerable<Flight>>(response.Content);
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            var uri = new Uri(string.Format(_getFlightByIdUri, flightId));
            var response = await GetAsync(uri);
            return JsonSerializerHelper.Deserialize<Flight>(response.Content);
        }

        public async Task<Reservation> CreateNewReservationAsync(Reservation reservation)
        {
            var json = JsonSerializerHelper.Serialize(reservation);
            var uri = new Uri(_addReservation);
            var response = await PostAsync(uri, json);
            return JsonSerializerHelper.Deserialize<Reservation>(response.Content);
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var uri = new Uri(string.Format(_updateReservation, reservationId));
            await DeleteAsync(uri);
        }

        public async Task<string> UpdateNewReservationAsync(Reservation reservation, int reservationId)
        {
            var json = JsonSerializerHelper.Serialize(reservation);
            var uri = new Uri(string.Format(_updateReservation, reservationId));
            var response = await PostAsync(uri, json);
            return response.Content;
        }

        /// <summary>
        /// Get Weather Forecast by id
        /// </summary>
        /// <param name="locationId">Location Id</param>
        /// <param name="date">Weather Forecast Date</param>
        /// <returns></returns>
        public async Task<WeatherForecast> GetWeatherForecastByIdAsync(int locationId, DateTime? date)
        {
            var formattedDate = String.Format("{0:MM/dd/yyyy}", date);
            var uri = new Uri(string.Format(_getWeatherUri, locationId, formattedDate));
            var response = await GetAsync(uri);
            return JsonSerializerHelper.Deserialize<WeatherForecast>(response.Content);
        }

        private static string GetHardwareId()
        {
            return new Windows.Security.ExchangeActiveSyncProvisioning.EasClientDeviceInformation().Id.ToString();
        }

        public async Task<bool> RegisterNotificationsChannel(string channelUri)
        {
            var dto = new RegisterNotificationsRequestDTO
            {
                DeviceID = GetHardwareId(),
                DeviceURI = channelUri
            };
            var json = JsonSerializerHelper.Serialize(dto);
            var uri = new Uri(_registerNotificationsUri);
            var response = await PostAsync(uri, json);
            return response.Success;
        }

        public async Task<IEnumerable<Uri>> GetAzureStorageFileUris(int reservationId)
        {
            var uri = new Uri(string.Format(_getFilesMetadataUri, reservationId));
            var response = await GetAsync(uri);
            var fileDtos = JsonSerializerHelper.Deserialize<IEnumerable<FileDTO>>(response.Content);
            var fileUris = fileDtos.Select(fileDto => fileDto.Uri);
            return fileUris;
        }
    }
}

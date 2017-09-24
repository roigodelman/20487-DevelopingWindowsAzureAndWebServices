using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Client.DataTransferObjects;
using BlueYonder.Companion.Shared;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Client.Helpers
{
    public class DataManager
    {
        /// <summary>
        /// Make sure Internet Connection avilable and perform HttpClient request.
        /// </summary>
        /// <param name="uri">Request Target</param>
        /// <returns>Json Response</returns>
        private async Task<Response> GetAsync(Uri uri)
        {
            var client = CreateHttpClient();
            var request = client.GetAsync(uri);
            return await SendRequestAsync(request);
        }

        private async Task<Response> PostAsync(Uri uri, string json)
        {
            var client = CreateHttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var request = client.PostAsync(uri, content);
            return await SendRequestAsync(request);
        }

        private async Task<Response> PutAsync(Uri uri, string json)
        {
            var client = CreateHttpClient();
            var content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var request = client.PutAsync(uri, content);
            return await SendRequestAsync(request);
        }

        private async Task<Response> DeleteAsync(Uri uri)
        {
            var client = CreateHttpClient();
            var request = client.DeleteAsync(uri);
            return await SendRequestAsync(request);
        }

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            return httpClient;
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
                throw new InvalidOperationException("SendRequestAsync", ex);
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
                uri = Addresses.GetLocationsUri;
            }
            else
            {
                uri = string.Format(Addresses.GetLocationsWithQueryUri, query);
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
            var hardwareId = GetHardwareId();
            var uri = new Uri(string.Format(Addresses.GetTravelerByIdentityUri, hardwareId));
            var response = await GetAsync(uri);
            Traveler traveler = null;
            if (response.Success)
            {
                traveler = JsonSerializerHelper.Deserialize<TravelerDTO>(response.Content).ToObject();
            }
            return traveler;
        }

        /// <summary>
        /// Modify the traveler details
        /// </summary>
        /// <param name="traveler">The Traveler</param>
        /// <returns></returns>
        public async Task UpdateTravelerAsync(Traveler traveler)
        {
            var dto = traveler.ToDTO();
            dto.TravelerUserIdentity = GetHardwareId();
            var serializedTraveler = JsonSerializerHelper.Serialize(dto);
            var uri = new Uri(string.Format(Addresses.UpdateTravelerUri, traveler.TravelerId));
            await PutAsync(uri, serializedTraveler);
        }

        public async Task<Traveler> CreateTravelerAsync()
        {
            var dto = new TravelerDTO()
            {
                TravelerUserIdentity = GetHardwareId()
            };
            var json = JsonSerializerHelper.Serialize(dto);
            var uri = new Uri(Addresses.CreateTravelerUri);
            var response = await PostAsync(uri, json);
            return JsonSerializerHelper.Deserialize<TravelerDTO>(response.Content).ToObject();
        }

        /// <summary>
        /// Get reservations by traveler id
        /// </summary>
        /// <param name="travelerId">Traveler Id</param>
        /// <returns></returns>
        public async Task<IEnumerable<Reservation>> GetReservationsAsync(int travelerId)
        {
            var uri = new Uri(string.Format(Addresses.GetReservationsByTravelerUri, travelerId));
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
            var uri = new Uri(string.Format(Addresses.GetReservationByIdUri, reservationId));
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
            var uri = new Uri(string.Format(Addresses.GetFlightsUri, source, destination, startDate));
            var response = await GetAsync(uri);
            return JsonSerializerHelper.Deserialize<IEnumerable<Flight>>(response.Content);
        }

        public async Task<Flight> GetFlightByIdAsync(int flightId)
        {
            var uri = new Uri(string.Format(Addresses.GetFlightByIdUri, flightId));
            var response = await GetAsync(uri);
            return JsonSerializerHelper.Deserialize<Flight>(response.Content);
        }

        public async Task<Reservation> CreateNewReservationAsync(Reservation reservation)
        {
            var json = JsonSerializerHelper.Serialize(reservation);
            var uri = new Uri(Addresses.AddReservationUri);
            var response = await PostAsync(uri, json);
            return JsonSerializerHelper.Deserialize<Reservation>(response.Content);
        }

        public async Task DeleteReservationAsync(int reservationId)
        {
            var uri = new Uri(string.Format(Addresses.UpdateReservationUri, reservationId));
            await DeleteAsync(uri);
        }

        public async Task<string> UpdateNewReservationAsync(Reservation reservation, int reservationId)
        {
            var json = JsonSerializerHelper.Serialize(reservation);
            var uri = new Uri(string.Format(Addresses.UpdateReservationUri, reservationId));
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
            var uri = new Uri(string.Format(Addresses.GetWeatherUri, locationId, date));
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
            var uri = new Uri(Addresses.RegisterNotificationsUri);
            var response = await PostAsync(uri, json);
            return response.Success;
        }

        public async Task<IEnumerable<Uri>> GetAzureStorageFileUris(int reservationId)
        {
            var uri = new Uri(string.Format(Addresses.GetFilesMetadataUri, reservationId));
            var response = await GetAsync(uri);
            var fileDtos = JsonSerializerHelper.Deserialize<IEnumerable<FileDTO>>(response.Content);
            var fileUris = fileDtos.Select(fileDto => fileDto.Uri);
            return fileUris;
        }
    }
}

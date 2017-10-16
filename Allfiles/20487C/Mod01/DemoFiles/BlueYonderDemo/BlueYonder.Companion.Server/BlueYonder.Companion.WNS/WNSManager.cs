using BlueYonder.Companion.WNS.Notifications;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace BlueYonder.Companion.WNS
{
    /// <summary>
    /// Provides authentication and notifications broadcast services using WNS.
    /// </summary>
    public class WNSManager
    {
        private static OAuthToken _accessToken = null;
        private static ManualResetEvent _waitHandle = new ManualResetEvent(false);
        private static ConcurrentDictionary<int, string> _subscribersMap = new ConcurrentDictionary<int, string>();

        public static void Authenticate()
        {
            _accessToken = null;

            IsAuthenticated = false;

            _waitHandle.Reset();

            string clientSecret = ConfigurationManager.AppSettings.Get("ClientSecret");

            string packageSID = ConfigurationManager.AppSettings.Get("PackageSID");

            if (!string.IsNullOrEmpty(clientSecret) && !string.IsNullOrEmpty(packageSID))
            {
                _accessToken = GetAccessToken(clientSecret, packageSID);
                IsAuthenticated = true;
            }
            else
            {
                throw new ConfigurationErrorsException
                    ("ClientSecret or PackageSID keys are either missing on the application configuration file or contains an empty string");
            }
        }



        public static void RegisterDevice(int travelerId, string deviceURL)
        {
            byte[] encodedDataAsBytes = System.Convert.FromBase64String(deviceURL);
            string decoded = System.Text.Encoding.UTF8.GetString(encodedDataAsBytes);

            _subscribersMap[travelerId] = decoded;
        }

        public static void UnregisterDevice(int travelerId, string deviceURL)
        {
            string removedValue;
            _subscribersMap.TryRemove(travelerId, out removedValue);
        }

        /// <summary>
        /// dispatch the notifications to the subscribers.
        /// </summary>
        public static void DispatchNotification(ToastNotificationBase notification)
        {
            Parallel.ForEach<int>
                (notification.TargetClientDevices,
                (travelerId) => DispatchNotification(travelerId, notification));
        }

        private static void DispatchNotification(int travelerId, ToastNotificationBase notification)
        {
            string clientURL;

            if (_subscribersMap.TryGetValue(travelerId, out clientURL))
            {
                var url = clientURL;
                try
                {
                    using (var client = new HttpClient())
                    {
                        var request = (HttpWebRequest)WebRequest.Create(url);
                        request.Method = "POST";
                        request.ContentType = "text/xml";
                        request.Headers = new WebHeaderCollection();
                        request.Headers.Add("X-WNS-Type", "wns/toast");
                        request.Headers.Add("Authorization", "Bearer " + _accessToken.AccessToken);

                        string data = notification.GetNotificationXML();
                        byte[] notificationMessage = Encoding.Default.GetBytes(data);
                        request.ContentLength = notificationMessage.Length;

                        using (Stream requestStream = request.GetRequestStream())
                        {
                            requestStream.Write(notificationMessage, 0, notificationMessage.Length);
                        }

                        var response = (HttpWebResponse)request.GetResponse();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private static OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        private static OAuthToken GetAccessToken(string secret, string sid)
        {
            try
            {
                var urlEncodedSecret = HttpUtility.UrlEncode(secret);
                var urlEncodedSid = HttpUtility.UrlEncode(sid);

                var body =
                  String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com", urlEncodedSid, urlEncodedSecret);

                string response;
                using (var client = new WebClient())
                {
                    client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                    response = client.UploadString("https://login.live.com/accesstoken.srf", body);
                }

                return GetOAuthTokenFromJson(response);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool IsAuthenticated { get; private set; }
    }
}

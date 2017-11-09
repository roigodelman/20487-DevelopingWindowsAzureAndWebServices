using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using Windows.Security.Cryptography;

namespace BlueYonder.Companion.Client.Helpers
{
    class WnsManager
    {
        private async Task<bool> SendChannelToServer(string channelUri)
        {
            var dataManager = new DataManager();
            return await dataManager.RegisterNotificationsChannelAsync(channelUri);
        }

        private string EncodeChannelUri(string uri)
        {
            var channelBuffer = CryptographicBuffer.ConvertStringToBinary(uri, BinaryStringEncoding.Utf8);
            return CryptographicBuffer.EncodeToBase64String(channelBuffer);
        }

        public async Task<bool> Register()
        {
            if (!NetworkManager.IsNetworkAvailable)
            {
                return false;
            }

            // Create a push notifications channel
            var channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            // Encode the channel uri
            var encodedChannelUri = EncodeChannelUri(channel.Uri);

            // Send the encoded channel uri to the server
            var success = await SendChannelToServer(encodedChannelUri);
            return success;
        }
    }
}

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
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;

namespace BlueYonder.Companion.WNS
{
    /// <summary>
    /// Provides authentication and notifications broadcast services using WNS.
    /// </summary>
    public class WNSManager
    {
        private static NotificationHubClient client;

        static WNSManager()
        {
            var connString = Environment.GetEnvironmentVariable("NotificationHubConnectionString", EnvironmentVariableTarget.Process);
            client = NotificationHubClient.CreateClientFromConnectionString(connString, "BlueYonderHub");
        }

        /// <summary>
        /// dispatch the notifications to the subscribers.
        /// The NotificationHub takes care of all the plumbing that has to do with authenticating the client, managing subscriptions and so on.
        /// </summary>
        public static void DispatchNotification(ToastNotificationBase notification)
        {

            Parallel.ForEach<int>
                (notification.TargetClientDevices,
                (travelerId) => client.SendWindowsNativeNotificationAsync(notification.GetNotificationXML(), $"user-{travelerId}");
        }
    }
}

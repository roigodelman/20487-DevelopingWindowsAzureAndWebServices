# Module 7: Windows Azure Service Bus


# Lab: Microsoft Azure Service Bus

#### Scenario

The IT team at Blue Yonder Airlines complained that from the time the company opened its booking services to other companies, they had to make many changes to the network&#39;s firewall in order to open inbound ports. This, of course, limits their ability to secure the company&#39;s internal network properly. To resolve this issue, the company has decided to change the way external applications, including the Travel Companion back-end services, connect to the WCF booking service.  Now all the communication with the on-premises service will be directed through a Azure Service Bus Relay. In this lab, you will create a Service Bus Relay in Azure, configure the WCF booking service to register with the Service Bus, and update the Travel Companion back-end service to send messages to the WCF service through the relay.

In addition, Blue Yonder Airlines wishes to improve the service offered to Travel Companion users by sending users&#39; updated information about changes made to their booked flights directly to their client app. To provide immediate feedback to the end user who updates the flight schedules, it was decided that the notifications will not be sent during the update process but rather be sent by a background process. To interact with the background process, the ASP.NET Web API service will use Service Bus Queues, and the background process itself will run in an Azure Function. For the notifications, the azure function will use Windows Push Notification Services (WNS).

In this lab, you will update the ASP.NET Web API services to use Azure Service Bus Queues, and create a new Azure Function to perform background processing.

#### Objectives

After completing this lab, you will be able to:

- Use Microsoft Azure Service Bus Relays.
- Use Azure Service Bus Queues.

### Exercise 1: Using a Service Bus Relay for the WCF Booking Service

#### Scenario

In this exercise, you will create a Service Bus namespace, configure the on-premises WCF Booking service to use a Service Bus Relay, and then configure the ASP.NET Web API services running in Azure to communicate with the on-premises services by using the newly created relay.

The main tasks for this exercise are as follows:

1. Create the Service Bus namespace by using the Azure Portal.

2. Add a new WCF Endpoint with a relay binding.

3. Configure the ASP.NET Web API back-end service to use the new relay endpoint.

4. Test the WCF service.

#### Task 1: Create the Service Bus Relay by using the Azure Portal

1. In the **20487B-SEA-DEV-A** virtual machine, run the **Setup.cmd** file from **D:\AllFiles\Mod07\LabFiles\Setup**, and then note the name of the cloud service created by the script.

   >**Note:** You might see warnings in yellow indicating a mismatch in the versions and setting deprecations. These warnings may appear if there is a newer version of the Azure PowerShell cmdlets. If this message is followed by a red error message, please inform the instructor, otherwise you can ignore the warnings.

2. Open the Microsoft Azure portal (**http://portal.azure.com**)
3. Create a new Azure Service Relay named **BlueYonderServerLab07YourInitialsRelay** (Replace _YourInitials_ with your initials).

    - Select a region closest to your location.
    - Select the newly created Service Bus Relay, click **Shared access policies**, click the **RootManageSharedAccessKey** policy and then copy the primary key to the clipboard.

#### Task 2: Add a new WCF Endpoint with a relay binding

1. Open the **BlueYonder.Server** solution file from the **D:\AllFiles\Mod07\LabFiles\begin\BlueYonder.Server** folder.
2. Use the Package Manager Console window to install the **4.1.3** version of the **WindowsAzure.ServiceBus** NuGet package in the **BlueYonder.Server.Booking.WebHost** project.

   >**Note:** Other projects in the solution already reference a specific version of the **WindowsAzure.ServiceBus** NuGet package. Therefore, you are required to use the **Package Manager Console** to install the same version of the NuGet package.

3. In the **BlueYonder.Server.Booking.WebHost** project, change the endpoint configuration of the booking service:

    - Open the **Web.config** file
    - Locate the endpoint of the service named **BookingTcp**, and then change its **binding** attribute to **netTcpRelayBinding**.
    - Add an address attribute with the following value: **sb://BlueYonderServerLab07YourInitialsRelay.servicebus.windows.net/booking** (Replace _YourInitials_ with your initials).

4. Add a new endpoint behavior named **sbTokenProvider** to the endpoint behaviors configuration.

    - Add a new **&lt;endpointBehaviors&gt;** element to the **&lt;behaviors&gt;** element that is under the **&lt;system.serviceModel&gt;** section.
    - In the new **&lt;endpointBehaviors&gt;** element, add a new **&lt;behavior&gt;** element, and set its **name** attribute to **sbTokenProvider**.
    - In the new **&lt;behavior&gt;** element, add a **&lt;transportClientEndpointBehavior&gt;** behavior element to the configuration.
    - In the behavior element add a **&lt;tokenProvider&gt;** element, and in it, add a **&lt;sharedAccessSignature&gt;** element with the **keyName** attribute set to **RootManageSharedAccessKey** and the **key** attribute set to the access key of the new Service Bus you created.

   >**Note:** Visual Studio Intellisense uses built-in schemas to perform validations. Therefore, it will not recognize the **transportClientEndpointBehavior** behavior extension, and will display a warning. Disregard this warning. ** **

5. Locate the endpoint of the service, and then set it to use the new endpoint behavior.

    - Use the **behaviorConfiguration** attribute to connect the endpoint to the endpoint behavior named **sbTokenProvider**.

6. Add an **&lt;applicationInitialization&gt;** element to **&lt;system.webServer&gt;** section group, and then set the initialization page to **/Booking.svc**.

   >**Note:** Application initialization automatically sends requests to specified addresses after the Web application loads. Sending the request to the service will make the service host load and initiate the Service Bus connection.

#### Task 3: Configure the ASP.NET Web API back-end service to use the new relay endpoint

1. Open **BlueYonder.Companion.sln** from **D:\AllFiles\Mod07\LabFiles\begin\BlueYonder.Server** in a new Visual Studio 2017 instance.
2. Use the **Package Manager Console** window to install the **3.1.1** version of the **WindowsAzure.ServiceBus** NuGet package in the **BlueYonder.Companion.Host** project.
3. Configure the **BlueYonder.Companion.Host** project to consume the new relay endpoint.

    - In the **Web.config** file, locate the **&lt;client&gt;** section within the **&lt;system.serviceModel&gt;** section group.
    - Change the client endpoint configuration to use the **netTcpRelayBinding**
    - Change the client endpoint configuration to use the **netTcpRelayBinding**
    - Set the value of the **address** attribute to the value **sb://BlueYonderServerLab07YourInitialsRelay.servicebus.windows.net/booking** (Replace _YourInitials_ with your initials).

4. Add a new endpoint behavior to the endpoint behaviors configuration.

    - Add a new **&lt;behaviors&gt;** element to the **&lt;system.serviceModel&gt;** section.
    - Add a new **&lt;endpointBehaviors&gt;** element to the **&lt;behaviors&gt;** element.
    - In the new **&lt;endpointBehaviors&gt;**, element add a new **&lt;behavior&gt;** element, and in it, add a **&lt;transportClientEndpointBehavior&gt;** behavior element.
    - In the behavior element add a **&lt;tokenProvider&gt;** element, and in it, add a **&lt;sharedAccessSignate&gt;** element with the **keyName** attribute set to **RootManageSharedAccessKey** and the **key** attribute set to the access key of the new Service Bus you created.

   >**Note:** Visual Studio Intellisense uses built-in schemas to perform validations. Therefore, it will not recognize **transportClientEndpointBehavior** behavior extension, and will display a warning. Disregard this warning.

#### Task 4: Test the WCF service

1. Open the Azure portal (**http://portal.azure.com**)
2. Locate the **BlueYonderServerLab07YourInitialsRelay** (Replace _YourInitials_ with your initials) Service Bus namespace, and then verify that it contains the **booking** relay.
3. In the **BlueYonder.Companion** solution, bring back the call to the WCF service from the reservation controller.

    - In the **BlueYonder.Companion.Controllers** project, open **ReservationControll.cs** , and then locate the following comment.

      **// TODO: Lab 07, Exercise 1: Task 4.3: Bring back the call to the backend WCF service.**

     You can use the Task List window to locate the **TODO** comment.

    - Uncomment the call to the **CreateReservationOnBackendSystem** method.

4. Publish the **BlueYonder.Companion.Host** project:
    - Right click **BlueYonder.Companion.Host** and click **Publish**
    - Select **Microsoft Azure App Service** and select **Create New**
    - Click **Publish**
    - In the **Create App Service** window, under **Resource Group** select an existing group if you have one, otherwise create a new Resource Group.
    - Under **App Service Plan** select a plan if you have one, otherwise create a new service plan.
    - Click **Create**
5. In the **BlueYonder.Server** solution, open the **BookingService.cs** file from the **BlueYonder.BookingService.Implementation** project.
6. Place a breakpoint at the beginning of the **CreateReservation** method, and then start debugging the web application.
7. Open the **D:\AllFiles\Mod07\LabFiles\begin\BlueYonder.Companion.Client\BlueYonder.Companion.Client.sln** solution file.
9. In the **BlueYonder.Companion.Shared** project, open the **Addresses** class, and then set the **BaseUri** property to the Azure Cloud Service name you noted at the beginning of this lab.
10. Start the client application without debugging, and purchase a new trip to New York.

    - Use **Search** and start typing **New**.
    - Wait for the app to show list of flights from Seattle to New York.
    - Fill in the reservation form and click **Purchase**.

>**Results**: After completing this exercise, you should have successfully run the client app to book a flight, and have the ASP.NET Web API services, running in the Azure Web Role, communicate with the on-premises WCF services by using Azure Service Bus Relays.

### Exercise 2: Publishing Flight Updates to Clients by using Azure Service Bus Queues

#### Scenario

The Flight Manager Web application supports updating flight schedules with new departure times. In this exercise, you will add the push notifications feature to send flight updates directly to the clients who booked the flight being updated. Sending push notifications to multiple clients can take some time, so the notification part will be decoupled from the ASP.NET Web API service by using Service Bus Queues. In this exercise, you will create a Service Bus Queue and send two types of messages to it from the ASP.NET Web API service: client notification subscription, and flight update messages. In addition, you will create a background process running in an Azure Function, which will receive messages from the queue and act according to each message type by either registering the client with the push notification server, or by sending flight update push notifications to registered clients.

The main tasks for this exercise are as follows:

1. Send flight update messages to the Service Bus Queue.

2. Create a Azure Function that receives messages from a Service Bus Queue.

3. Configure and use Azure NotificationHub to manage subscriptions and distribute notifications.

4. Test the Service Bus Queue with flight update messages.

#### Task 1: Send flight update messages to the Service Bus Queue

1. Open the Microsoft Azure portal (**http://portal.azure.com**)

    a. Create a new Service Bus named **BlueYonderServerLab07YourInitialsBus** (replace _YourInitials_ with your initials)
    b. Open the newly created service bus and Click **Shared Access Policies**.
    c. Click the **RootManageSharedAccessKey** policy and then Copy the value of the **Connection string–Primary key** text box.

2. Return to the **BlueYonder.Companion** solution in Visual Studio 2017, and then add the **Connection string** that you copied in the previous step to the **web.config** file.

    - Open **web.config** in located in **BlueYonder.Companion.Host**
    - Add a new entry under **AppSettings**
    - <add key="ServiceBusConnectionString" value="[ConnectionString from previous step]" />

3. Open the **ServiceBusQueueHelper** class located in the **BlueYonder.Companion.Controllers** project, and then implement the **ConnectToQueue** method.

    a. Create a Service Bus namespace manager object by using the connection string of the Service Bus.
    b. Use the **ConfigurationManager.AppSettings** to retrieve the connection string.
    c. To create the namespace manager object, use the **CreateFromConnectionString** method of the **NamespaceManager** class.    
    d. Check if the Queue exists and create it by using the **CreateQueue** API if necessary.    
    e. Return a new **QueueClient** object for the queue by using the **CreateFromConnectionString** method of the **QueueClient** class.

   >**Note:** The Queue name is stored in a static variable named **QueueName**, and has the value of **FlightUpdatesQueue**

4. In the **FlightsController** class, add a static field for the **QueueClient** object.

    - Call the method you previously created in a static constructor to set the object.

5. In the **Put** method, after saving the changes made to the flight schedule, set the **FlightId** property of the **updatedSchedule** variable to the **id** parameter containing the updated flight id.

    - Create a new **BrokeredMessage** object with the updated schedule as the message body, set the **ContentType** property of the message to **UpdatedSchedule**, and then send the message to the queue.

#### Task 2: Create a Notification Hub and configure it to use WNS
We want to be able to notify the clients at real time when a flight changes. To do this, we will use WNS (Windows Notification Service) to send push notifications from the server to the clients.
Before we can send notifications to the clients, we need to know what clients are expecting notifications. Thankfully, we don't have to manage this ourselves and Microsoft Azure provides a service that handles exactly that - Notification Hub.
The Notification Hub itself supports sending push notifications to all kind of platforms, Android, iOS, UWP applications and so on. We will only use its WNS support to send notifications to UWP applications.


1. Open the Microsoft Azure portal (**http://portal.azure.com**)
2. On the left side of the window, click the **+ New** button.
3. Click **Web + Mobile**
4. Click **Notification Hub**
5. In the **Notification Hub** field, enter **BlueYonderHub-YourInitials** (e.g. BlueYonderHub-bg)
6. In the **Create a new namespace** field, enter **BlueYonderHubNS-YourInitials**
7. In **Location** select the location that is closest to you.
8. In ResourceGroup, create a new one if doesn't exist or select one of your existing groups.
9. Click **Create**
10. Once the Notification Hub has been created, access it through the **All Resources** pane on your Azure dashboard.
11. Under **Manage** click **Notification Services**
12. Click **Windows (WNS)**
13. In **Package SID** field, enter "ms-app://s-1-15-2-1943958206-46993560-2941426192-3413777431-2244800519-641792188-4204354165"
14. In **Security Key** field, enter "/A6E8J9JmfDFZMX5lhuCJdTeI32zcXMM"
15. Click **Save**

#### Task 3: Create an Azure Function that activates when it receives messages from a Service Bus Queue and use the Notification Hub to send notifications

We are going to create a new **Azure Functions** project. Azure Functions will let us invoke custom logic based on messages that are transferred through the service bus relay we created earlier. We will use WNS to send notifications through the Notification Hub we created earlier.

First we will add a WNS project bundled with the exercise:
1. Right Click the **BlueYonder.Companion** solution.
2. Double click **BlueYonder.Companion.WNS** folder.
3. Select **BlueYonder.Companion.WNS.csproj** and click Ok.

Next we will add an Azure Functions project and connect it to the WNS project.
1. Right click the **BlueYonder.Companion** solution.
2. Point to **Add** and click **New Project**
3. Expand **Installed** and then expand **Visual C#**
4. Click **Cloud**
5. In the list of project types, select **Azure Functions**
6. Name your project "**BlueYonder.Companion.Functions**"
7. Click **OK**
8. Right click the new project you just created.
9. Point to **Add** and click **New Item**
10. Select **Azure Function**
11. Name the function "**PublishFlightUpdate**"
12. Click **OK**
13. A "**New Azure Function - PublishFlightUpdate**" window will open
14. In the list on the left, select **Service Bus Queue trigger**
15. In **Access Rights** select **Manage**
16. In **Connection** enter **ServiceBusConnectionString**
17. In **Queue name** enter "**FlightUpdatesQueue**"
18. Click **Ok**
19. A new file **PublishFlightUpdates.cs** will be added to the **BlueYonder.Companion.Functions** project.
20. Next, point to **BlueYonder.Companion.Functions** project and right click it.
21. Point to **Add** and click **Existing Item**
22. Select **MessageHandler.cs** that is located in **D:\AllFiles\Mod07\LabFiles\Assets\MessageHandler.cs** and click **Ok**
23. Under **BlueYonder.Companion.Functions**, right click **References** and select **Add Reference**
24. Click **Solution** on the left.
25. Check **BlueYonder.Companion.WNS** and click **Ok**

#### Task 4: Publish the new Azure Functions project and configure it
We will first have to publish our Azure Functions project and once it is published, we will need to provide further configuration through the Azure portal, configurations such as various connection strings (Azure SQL, Service Bus, Notification Hub)

First we will publish the Azure Functions project
1. Right click the **BlueYonder.Companion.Functions** project and click **Publish**
2. Select **Azure Functions App**, make sure **Create New** is selected and click **Ok**
3. A new window **Create App Service** will open.
4. In the **App Name** field, enter **BlueYonderFunctions-YourInitials**
5. In the **Resource Group** field, if nothing is populated, click **New**, give a name of your liking and click **Ok**
6. In **App Service Plan** field, if nothing is populated, click **New**:
    1. In **App Service Plan** field, enter **BlueYonderFunctions-YourInitials**
    2. In **Location** field, pick the location closest to you.
    3. Click **Ok**
7. Click **Create**

Next, we will go to Azure portal and add connection strings to our Azure Functions application.
We will need to add connections strings for our Azure SQL server, Azure Notification Hub and Azure Service Bus.
1. Open the Microsoft Azure portal (**http://portal.azure.com**)
2. Locate the newly created Azure Functions resource and click it.
3. Select the Azure Function App you created via the Publish you performed in the previous step (**BlueYonderFunctions-YourInitials**)
4. Click **Application settings**
5. Under the **Application settings** section add the following entries:
    1. Key: "ServiceBusConnectionString", Value: The primary connection string of the service bus you created earlier for the FlightUpdatesQueue
    2. Key: "TravelCompanion", Value: The connection string to the Azure SQL database, as configured in the **web.config** file in the **BlueYonder.Companion.Host** project.
    3. Key: "NotificationHubConnectionString", value: The primary connection string to the Notification Hub you created earlier.
6. Click **Save**

#### Task 4: Test the Service Bus Queue with flight update messages

1. Open the **BlueYonder.Companion.Client** solution from **D:\AllFiles\Mod07\LabFiles\begin\BlueYonder.Client**.
3. Run the application.
4. Open the **BlueYonder.Companion.FlightsManager** solution file from the **D:\AllFiles\Mod07\LabFiles\begin\BlueYonder.Server** folder in a new Visual Studio 2012 instance.
5. Open the **web.config** file from the **BlueYonder.FlightsManager** project, in the **&lt;appSettings&gt;** section, locate the **webapi:BlueYonderCompanionService** key, and then update the **{CloudService}** string to the Azure Cloud Service name you noted at the beginning of the lab.
6. Run the **BlueYonder.FlightsManager** web application, find the flights from Seattle to New York, and change the departure time of your purchased trip to **9:00 AM**.

    - A notification should appear on your machine.

>**Results**: After completing this exercise, you should have run the Flight Manager Web application, updated the flight departure time of a flight you booked in advance in your client app, and received Windows push notifications directly to your computer.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

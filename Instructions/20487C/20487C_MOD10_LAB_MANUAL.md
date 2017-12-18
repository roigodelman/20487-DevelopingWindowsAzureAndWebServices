# Module 10: Monitoring and Diagnostics

> Wherever you see a path to file starting at [repository root], replace it with the absolute path to the directory in which the 20487 repository resides. 
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487, then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

# Lab: Monitoring and Diagnostics

#### Scenario

The helpdesk team at Blue Yonder Airlines has complained that ever since the ASP.NET Web API services have been deployed to Microsoft Azure, they cannot gather statistical information about the services. In addition, the helpdesk team has complained that in the past, when there were problems with the WCF booking service, they had a difficult time understanding what caused the problem. In this lab, you will help the helpdesk team by configuring the WCF booking service to output trace and message logging. You will also configure the Microsoft Azure App Service running the ASP.NET Web API service to output diagnostics so it can be collected and analyzed.

#### Objectives

After completing this lab, you will be able to:

- Configure WCF tracing and message logging.
- Configure Azure Diagnostics and view the collected information.

#### Lab Setup

Estimated setup time: 15 minutes.

Verify that you received the credentials to sign in to the Azure portal from your training provider. These credentials and the Azure account will be used throughout the labs of this course.

> **Important!** make sure you are logged in to **Visual Studio 2017** with your **Microsoft Azure** account before starting this lab!

1. On the **Start** menu, search for **Windows Powershell**, right click it and click **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. Go to **[repository root]\AllFiles\20487C\Mod10\LabFiles\Setup**
4. Run the following command:
```batch
ps createAzureServices.ps1
```
5. Follow the on-screen instructions.

### Exercise 1: Configuring Message Logging

#### Scenario

WCF supports message logs and activity (trace) logs. To provide the helpdesk the information they require, you will start by turning on both types of logs. Because the WCF messages are encrypted, you will log messages at the service level, which will log the messages in their decrypted form, instead of at the transport level, where messages are logged in the encrypted form.

The main tasks for this exercise are as follows:

1. Open the WCF service configuration editor.

2. Configure WCF message logging.

#### Task 1: Open the WCF service configuration editor

1. Open the **BlueYonder.Server.sln** solution from the **D:\AllFiles\Mod10\LabFiles\begin\BlueYonder.Server** folder.
3. Open the **Web.config** file from the **BlueYonder.Server.Booking.WebHost** project with the **WCF Configuration Editor**. When prompted that the **Microsoft.ServiceBus** assembly could not be found, click **Yes**, select the **Microsoft.ServiceBus.dll** assembly from web project&#39;s **bin** folder, and approve the warning message that appears next.

   >**Note:** To open the **WCF Configuration Editor** , in Solution Explorer, right-click the **Web.config** file, and then click **Edit WCF Configuration**.

#### Task 2: Configure WCF message logging

1. In the **Diagnostics** configuration, enable **Message Logging** and enable **Log Auto Flush**.
2. Expand the **Diagnostics** configuration node, and in the **Message Logging** configuration, set **LogEntireMessage** to **True**.
3. Save the changes to the configuration and close the Service Configuration Editor window.

>**Results**: You can test your changes at the end of the lab.

### Exercise 2: Configuring Azure Diagnostics

#### Scenario

To provide the helpdesk with the required information about the Azure deployment, you will need to provide user information from the app service logs and statistical information about purchases. In this exercise, you will add trace logging to log every purchase performed in the service and configure the Azure deployment to collect those trace files. In addition, you will collect general app service logs from the deployment, which will assist the helpdesk to calculate the user activity statistics.

The main tasks for this exercise are as follows:

1. Add trace messages to the ASP.NET Web API service.

2. Configure Diagnostics logs for the Web API App Service.

3. Deploy the ASP.NET Web API Application to Microsoft Azure.

4. Run the client app to create logs.

5. View the collected diagnostics data.

#### Task 1: Add trace messages to the ASP.NET Web API service and deploy the application

1. Open the **BlueYonder.Companion.sln** solution from the **D:\AllFiles\Mod10\LabFiles\begin\BlueYonder.Server** folder in a new instance of Visual Studio 2017.
2. Open the **TraceWriter.cs** file from the **BlueYonder.Companion.Host** project and implement the **Trace** method. Use .NET Diagnostics tracing to write the trace messages.

   >**Note:** You can see an example for implementing the **ITraceWriter** interface in lesson 2, &quot;Configuring Service Diagnostics&quot;.

3. In the **BlueYonder.Companion.Host** project, under the **App\_Start** folder, open the **WebApiConfig.cs** file, and add the code to replace the default trace writer service of ASP.NET Web API with a new instance of the **TraceWriter** class.

   >**Note:** You can see an example for registering the **TraceWriter** class in lesson 2, &quot;Configuring Service Diagnostics&quot;.

4. In the **BlueYonder.Companion.Controllers** project, open the **ReservationsController.cs** file, locate the **Post** method, and after the call to the **Save** method, add code to trace an information message.

    - Add a using directive for the **System.Web.Http.Tracing**, to get the tracing extension methods
    - Use the **Configuration.Services.GetTraceWriter** method to get an instance of the trace writer.
    - Use the **Info** extension method to write the trace message. Set the category to **ReservationController**, and include the reservation&#39;s confirmation code in the trace message.

   >**Note:** You can see an example for tracing messages with the **TraceWriter** class in lesson 2, &quot;Configuring Service Diagnostics&quot;.
5. Use Visual Studio 2017 to publish the **BlueYonder.Companion.Host** project to the **blueyonder-companion-10-_yourinitials_** app service.

#### Task 2: Configure Windows Azure Diagnostics for the App Service

1. Open a browser and navigate to the Azure portal.
2. Open the App Service named **blueyonder-companion-10-_yourinitials_** (replace _yourinitials_ with your intials)
3. In the App Service's **Diagnostics logs** blade, activate **Application Logging (Blob)** and set the **Level**  to **Verbose**.
4. Use the **blueyonder10_yourinitials_** account to store the logs.
5. Store the logs in a container named **logs**.
6. Set **Retention Period** to 1 day.
6. Save the logging configuration.
7. Go to the **Overview** blade and copy the app service **URL**, you will need it for the next task.

#### Task 3: Run the client app to create logs

2. Open the **BlueYonder.Companion.Client.sln** solution from the **[repository root]\AllFiles\20487C\Mod10\LabFiles\begin\BlueYonder.Companion.Client** folder.
3. In the **Addresses** class of the **BlueYonder.Companion.Shared** project, set the **BaseUri** property to the Azure cloud service name you noted in the beginning of this lab.

    - Replace the **{appServiceUrl}** string with the URL you copied in the previous task.

4. Run the client app, search for **New**, and purchase a flight from _Seattle_ to _New-York_. Close the app after you purchase the trip.

#### Task 5: View the collected diagnostics data

1. Return to the Visual Studio instance with the **BlueYonder.Companion** solution open.
2. In Cloud Explorer, locate the Azure Storage account you have configured for the app servuce **Diagnostics logs**.
3. Expand the Azure Storage account in Cloud Explorer and explore the logs in the **logs** container. Search the container for a file from the current year, month and day. Inside you should look for a message starting with **Info;ReservationsController;**.

   >**Note:** In addition to the trace message your code writes to the log, ASP.NET Web API writes several other infrastructure trace messages.

4. In the same log file, verify that you see the requests for the Travelers, Locations, Flights, and Reservations controllers.

5. Open the WCF message log located in the **[repository root]\AllFiles\20487C\Mod10\LabFiles\begin\BlueYonder.Server\BlueYonder.Server.Booking.WebHost** folder using the Service Trace Viewer. Review the booking service&#39;s CreateReservation request and response messages.

   >**Note:** You can view the messages by clicking the **Message** tab in the left pane, selecting the message to view (either the **http://blueyonder.server.interfaces/IBookingService/CreateReservation** or **http://blueyonder.server.interfaces/IBookingService/CreateReservationResponse** message), and then clicking the **Message** tab in the bottom-right pane.

>**Results**: After you complete the exercise, you will be able to use the client App to purchase a trip, and then view the created log files, for both the Azure deployment and the on-premises WCF service.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

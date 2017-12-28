# Module 10: Monitoring and Diagnostics

# Lab: Monitoring and Diagnostics

#### Scenario

The helpdesk team at Blue Yonder Airlines has complained that ever since the ASP.NET Web API services have been deployed to Microsoft Azure, they cannot gather statistical information about the services. In addition, the helpdesk team has complained that in the past, when there were problems with the WCF booking service, they had a difficult time understanding what caused the problem. In this lab, you will help the helpdesk team by configuring the WCF booking service to output trace and message logging. You will also configure the Microsoft Azure Web Role running the ASP.NET Web API service to output diagnostics so it can be collected and analyzed.

#### Objectives

After completing this lab, you will be able to:

- Configure WCF tracing and message logging.
- Configure Azure Diagnostics and view the collected information.

#### Lab Setup

Estimated Time: **45 minutes**

Virtual machine: **20487B-SEA-DEV-A**, **20487B-SEA-DEV-C**

User name: **Administrator**, **Admin**

Password: **Pa$$w0rd**, **Pa$$w0rd**

For this lab, you will use the available virtual machine environment. Before you begin this lab, you must complete the following steps:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
2. In Hyper-V Manager, click **MSL-TMG1**, and in the **Actions** pane, click **Start**.
3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Actions** pane, click **Start**.
4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
5. Sign in using the following credentials:

    - User name: **Administrator**
    - Password: **Pa$$w0rd**

6. Return to Hyper-V Manager, click **20487B-SEA-DEV-C**, and in the Action pane, click **Start**.
7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
8. Sign in using the following credentials:

    - User name: **Admin**
    - Password: **Pa$$w0rd**

9. Verify that you received credentials to log in to the Azure portal from you training provider, these credentials and the Azure account will be used throughout the labs of this course.

### Exercise 1: Configuring Message Logging

#### Scenario

WCF supports message logs and activity (trace) logs. To provide the helpdesk the information they require, you will start by turning on both types of logs. Because the WCF messages are encrypted, you will log messages at the service level, which will log the messages in their decrypted form, instead of at the transport level, where messages are logged in the encrypted form.

The main tasks for this exercise are as follows:

1. Open the WCF service configuration editor.

2. Configure WCF message logging.

#### Task 1: Open the WCF service configuration editor

1. In the **20487B-SEA-DEV-A** virtual machine, run the **setup.cmd** file from **D:\AllFiles\Mod10\LabFiles\Setup**. Provide the information according to the instructions, and write down the name of the Azure Cloud Service.

   >**Note:** You might see warnings in yellow indicating a mismatch in the versions and the obsolete settings. These warnings might appear if there are newer versions of Azure PowerShell cmdlets. If these warnings are followed by a red error message, please inform the instructor, otherwise you can ignore them.

2. Open the **BlueYonder.Server.sln** solution from the **D:\AllFiles\Mod10\LabFiles\begin\BlueYonder.Server** folder **.**
3. Open the **Web.config** file from the **BlueYonder.Server.Booking.WebHost** project with the **WCF Configuration Editor**. When prompted that the **Microsoft.ServiceBus** assembly could not be found, click **Yes**, select the **Microsoft.ServiceBus.dll** assembly from web project&#39;s **bin** folder, and approve the warning message that appears next.

   >**Note:** To open the **WCF Configuration Editor** , in Solution Explorer, right-click the **Web.config** file, and then click **Edit WCF Configuration**.

#### Task 2: Configure WCF message logging

1. In the **Diagnostics** configuration, enable **Message Logging**.
2. Expand the **Diagnostics** configuration node, and in the **Message Logging** configuration, set **LogEntireMessage** and **LogMessageAtServiceLevel** to **True** , and **LogMessageAtTransportLevel** to **False**.
3. Save the changes to the configuration and close the Service Configuration Editor window.

>**Results**: You can test your changes at the end of the lab.

### Exercise 2: Configuring Azure Diagnostics

#### Scenario

To provide the helpdesk with the required information about the Azure deployment, you will need to provide user information from IIS logs and statistical information about purchases. In this exercise, you will add trace logging to log every purchase performed in the service and configure the Azure deployment to collect those trace files. In addition, you will collect IIS logs from the deployment, which will assist the helpdesk to calculate the user activity statistics.

The main tasks for this exercise are as follows:

1. Add trace messages to the ASP.NET Web API service.

2. Configure Windows Azure Diagnostics for the Web Role.

3. Deploy the ASP.NET Web API Application to Windows Azure.

4. Run the client app to create logs.

5. View the collected diagnostics data.

#### Task 1: Add trace messages to the ASP.NET Web API service

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

#### Task 2: Configure Windows Azure Diagnostics for the Web Role

1. In the **BlueYonder.Companion.Host.Azure** project, open the **BlueYonder.Companion.Host** role&#39;s **Properties** window and set the role to use custom diagnostics.

    - On the **Configuration** tab, select **Custom plan**.
    - Click **Edit** to open the diagnostics configuration.

2. On the **Application logs** tab, change the **Log level** from **Error** to **Verbose**.
3. On the **Log directories** tab, set the **Transfer period** to **1 minute** , the **Buffer size** to **1024MB**, and set the **IIS logs** to a **1024MB** quota.
4. Click **OK** to close the dialog box, and save the changes to the role&#39;s properties.

#### Task 3: Deploy the ASP.NET Web API Application to Windows Azure

1. Use Visual Studio 2017 to publish the **BlueYonder.Companion.Host.Azure** project.

    - If you did not import your Azure subscription information yet, download your Azure credentials, and import the downloaded publish settings file in the **Publish Windows Azure Application** dialog box.

2. Select the cloud service that matches the cloud service name you wrote down in the beginning of the lab, while running the setup script.
3. Finish the deployment process by clicking **Publish**. The publish process might take several minutes to complete.

#### Task 4: Run the client app to create logs

1. Sign in to the virtual machine **20487B-SEA-DEV-C** as **Admin** with the password **Pa$$w0rd**.
2. Open the **BlueYonder.Companion.Client.sln** solution from the **D:\AllFiles\Mod10\LabFiles\begin\BlueYonder.Companion.Client** folder.
3. In the **Addresses** class of the **BlueYonder.Companion.Shared** project, set the **BaseUri** property to the Azure cloud service name you noted in the beginning of this lab.

    - Replace the **{CloudService}** string with the cloud service name.

4. Run the client app, search for **New**, and purchase a flight from _Seattle_ to _New-York_. Close the app after you purchase the trip.

#### Task 5: View the collected diagnostics data

1. Return to the **BlueYonder.Companion** solution in the **20487B-SEA-DEV-A** virtual machine, open the **Publish Windows Azure Application** dialog box, and note the name of the storage account.
2. In Server Explorer, add the Azure Storage account you have used for the publish process.
3. Open the Azure Storage account in Server Explorer and explore the logs in the **WADLogsTable** table. Search the table for a message starting with **ReservationsController** and double-click it to view the message.

   >**Note:** In addition to the trace message your code writes to the log, ASP.NET Web API writes several other infrastructure trace messages.

4. Open the **wad-iis-logfiels** blob container in Server Explorer and explore the log files. Verify that you see the requests for the Travelers, Locations, Flights, and Reservations controllers.

   >**Note:** It is possible it will take more than a minute from the time the request is sent and until it is logged by IIS. If you do not yet see any logs, or the requests are missing from the log, wait for another minute, refresh the blob container, and then download the log again.

5. Open the WCF message log located in the **D:\AllFiles\Mod10\LabFiles\begin\BlueYonder.Server\blueyonder.server.booking.webhost** folder using the Service Trace Viewer. Review the booking service&#39;s CreateReservation request and response messages.

   >**Note:** You can view the messages by clicking the **Message** tab in the left pane, selecting the message to view (either the **http://blueyonder.server.interfaces/IBookingService/CreateReservation** or **http://blueyonder.server.interfaces/IBookingService/CreateReservationResponse** message), and then clicking the **Message** tab in the bottom-right pane.

>**Results**: After you complete the exercise, you will be able to use the client App to purchase a trip, and then view the created log files, for both the Azure deployment and the on-premises WCF service.

Perform the following steps to apply the **StartingImage** snapshot:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
2. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Snapshots** pane, right-click **StartingImage**, and then click **Apply**.
3. In the **Apply Snapshot** dialog box, click **Apply**.
4. Repeat Step 2 for all the virtual machines that you used in this lab. (excluding **MSL-TMG1**).

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

# Module 6: Hosting Services

> Wherever  you see a path to file starting at *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487,
then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

## Lab: Hosting Services

#### Scenario

Blue Yonder Airlines&#39; estimated research results show that in the next six months, more than 100,000 people will start to use their new Travel Companion app. Because the company does not yet have a web data center, they decided not to buy additional servers to support the new user load. Instead, they are moving their external service layer to Azure. By using Azure, they can scale-out as required, as more people start to use the new app. In this lab, you move the WCF booking service from its simple console host to IIS, and move the ASP.NET Web API services from their on-premises IIS hosting environment to a web role in Azure.

In addition, Blue Yonder Airlines wants to separate the deployment of the flights management web application from the Travel Companion back-end service. Because the web application is a small application and does not require many resources, it was decided that the application should be deployed to an Azure Web Site. In this lab, you deploy the booking management web application to an Azure Web Site.

#### Objectives

After you complete this lab, you will be able to:

- Host the WCF booking service in IIS.
- Host the destinations, flight schedules, and booking ASP.NET Web API services in an Azure web role.
- Host the booking management web application in an Azure Web Site.

#### Lab Setup

You will need to activate IIS on your machine. The instructions below are for Windows 10 only.

1. Open **Search Windows** by clicking the magnifying glass next to the **Start** menu.
2. Enter **Turn windows features on or off** and press **Enter**
3. Check **Internet Information Services** once, a black square should appear within the white check box.
4. Expand **Internet Information Services** and then expand **World Wide Web Services**
5. Check **Application Development Features**
6. We will also need to activate WCF support before we can deploy WCF services to IIS. To do this:
	- In the **Windows Features** window, expand **.NET Framework 4.7 Advances Services**
	- Expand **WCF Services** and check all the checkboxes inside it (**HTTP Activation**, **Message Queuing (MSMQ) Activation**, etc) 
6. Click **Ok**, Windows will proceed to install the required components, when it is done, IIS will be on.
7. Verify that IIS works:
	- Open a browser.
	- Navigate to **http://localhost/**
	- An **Internet Information Services** page should show up.

### Exercise 1: Hosting the WCF Services in IIS

#### Scenario

To host the WCF services in IIS, you will start by creating a web application. After you create the web application and add the proper references, you will configure the web application to have the same service configuration as the self-hosted services. After you finish preparing the web application, you will host it in IIS and configure IIS to support both HTTP and NET.TCP endpoints.

The main tasks for this exercise are as follows:

1. Create a web application project

2. Configure the web application project to use IIS

3. Configure the web applications to support NET.TCP

#### Task 1: Create a web application project

1. Open the **BlueYonder.Server** solution file from the **[repository root]\AllFiles\20487C\Mod06\LabFiles\begin\BlueYonder.Server** folder.
2. To the **BlueYonder.Server** solution, add a new **ASP.NET Web Application (.NET Framework)** project named  **BlueYonder.Server.Booking.WebHost**.
3. In the **New ASP.NET Web Application** window, select **Empty** and click **Ok**.
4. Use the **Package Manager Console** window to install version **6.2.0** of the **EntityFramework** NuGet package, in the  **BlueYonder.Server.Booking.WebHost** project.


  >**Note:** Other projects in this solution use version 6.2.0 of the EntityFramework NuGet package. To prevent assembly load failures, you are required to use the **Package Manager Console** , to install that specific package version. 

5. In the **BlueYonder.Server.Booking.WebHost** project, add references to the **System.ServiceModel** assembly and to the following projects:

  - **BlueYonder.BookingService.Contracts**
  - **BlueYonder.BookingService.Implementation**
  - **BlueYonder.DataAccess**
  - **BlueYonder.Entities**

6. Copy the **FlightScheduleDatabaseInitializer.cs** file from the booking service self-host project to the new web application project. Change the namespace from **BlueYonder.BookingService.Host** to **BlueYonder.Server.Booking.WebHost**
7. Add a **Global.asax** file to the new web application, and in the **Application\_Start** method, add the required database initialization code.

    a. Initialize the database by using the database initializer you copied from the self-host project.    
    b. Refer to the database initialization code in the **Program.cs** file that is in the **BlueYonder.Server.Booking.Host** project.

#### Task 2: Configure the web application project to use IIS

1. Copy the entire content of the **App.config** file from the **BlueYonder.Server.Booking.Host** project and paste it over the content of the **Web.config** file in the **BlueYonder.Server.Booking.WebHost** project.

2. Within the **&lt;system.serviceModel&gt;** section group, add a **&lt;serviceHostingEnvironment&gt;** element with service activation configuration for the Booking service, and use the relative address **Booking.svc**.

    a. Declare a **&lt;serviceActivations&gt;** section inside the **&lt;serviceHostingEnvironment&gt;** section.    
    b. Inside the **&lt;serviceActivations&gt;** section, add an **&lt;add&gt;** tag with the following parameters.

   | Attribute | Value |
   | --- | --- |
   | service | BlueYonder.BookingService.Implementation.BookingService |
   | relativeAddress | Booking.svc |

  >**Note:** You can also refer to Lesson 1, &quot;Hosting Service On-Premises&quot;, Topic 2, &quot;Hosting WCF Service in IIS&quot; in this module, for a code example. ** **

3. Add a **&lt;system.web&gt;** element to the **&lt;configuration&gt;** section, and in it add a **&lt;compilation&gt;** element and a **&lt;httpRuntime&gt;** element.

    a. In the **&lt;compilation&gt;** element, set the debug attribute to **true** and the **targetFramework** attribute to **4.5**.  
    b. In the **&lt;httpRuntime&gt;** element, set the **targetFramework** attribute to **4.5**.
    

  >**Note:**.NET 4 and 4.5 use the same .NET Framework version for the IIS application pool. If you do not specify the target framework of your Web application in the **Web.config** file, the default version will be .NET 4.

4. Remove the addresses used in the service metadata behavior and the service endpoint.

    a. Remove the **httpGetUrl** attribute from the **&lt;serviceMetadata&gt;** element.    
    b. Remove the **address** attribute from the **&lt;endpoint&gt;** element.

  >**Note:** IIS uses the address of the web application to create the service metadata address and the service endpoint address.

5. In the **BlueYonder.Server.Booking.WebHost** project&#39;s properties, on the **Web** tab, change the server from IIS Express to the local IIS Web server, and then build the solution.

#### Task 3: Configure the web applications to support NET.TCP

1. Open IIS Manager from the Start screen and select the **Default Web Site** from the **Connections** pane. Verify that the  **net.tcp** binding is listed in the **Bindings** list.

  >**Note:** The site bindings configure which protocols are supported by the IIS Web Site and which port, host name, and IP address are used with each protocol.

2. Open the **Advanced Settings** dialog box of the **BlueYonder.Server.Booking.WebHost** web application, and set both to enable the **http** and **net.tcp** protocols. Use a comma to separate the values.

  >**Note:** In addition to adding net.tcp to the site bindings list, you also need to enable net.tcp for each Web application you host in IIS. By enabling net.tcp, WCF will automatically create an endpoint with NetTcpBinding.

3. Connect to the WCF service through the WCF Test Client application, and verify if the application is able to retrieve metadata from both services.

    a. Open **Search Windows** by clicking the magnifying glass next to the **Start** menu.
    b. Search for **Developer Command Line for VS 2017** and open it.
    c. In the command line that opened, enter **wcftestclient** and press **enter**.
    d. Connect to the address **http://localhost/BlueYonder.Server.Booking.WebHost/Booking.svc**    
    e. Wait until you see the service and endpoints tree in the pane to the left.  

>**Results**: You will be able to run the WCF Test Client application and verify if the services are running properly in IIS.

### Exercise 2: Hosting the ASP.NET Web API Services in a new Azure Web App

#### Scenario

Before you deploy the ASP.NET Web API services to Azure, you need to create a cloud service where you will deploy the web application and an SQL Database server for the application to use. After you create the cloud service, you will upload several certificates to it, so the web application can use these certificates when calling the on-premises WCF service.

After you create the components in Azure, you will create the cloud project in Visual Studio 2012, and configure it to deploy the ASP.NET Web API web application to an Azure web role. Before deploying the application to Azure, you will test it locally with the Azure compute emulator, and after you verify it is running properly, you will deploy it to Azure.

The main tasks for this exercise are as follows:

1. Create a new SQL database

2. Publish the Companion Web API project to Azure

4.  Test the Companion client against the Azure hosted Companion Web API

#### Task 1: Create a new SQL database

1. Open the Microsoft Azure portal ( **http://portal.azure.com** )

2. Create a new **SQL Database** Server.

    a. Open the **New** blade.
    b. Start a **SQL Database** journey.
    c. In the **SQL Database** blade, enter the following details:
      - Database name: **blueyonder**
      - Resource Group: Select **Create new** and enter **BlueYonder.Lab.06**
      - Server: Click on **Configure required settings** and the **Server** and **New server** blades will open. Enter the following details in the **New Server** blade:
        - Server name: **blueyonder-lab-06-dbserver-*YourInitials*** (replace *YourInitials* with your initials, e.g. - John Doe will become jd)
        - Server admin login: **BlueYonderAdmin**
        - Password: **Pa$$w0rd**
        - Confirm password: **Pa$$w0rd**
        - Location: Pick the closest location to you
        - Click **Select**
      - Click **Create**


3. Open the **BlueYonder.Companion.sln** solution file from the **[repository root]\AllFiles\20487C\Mod06\LabFiles\begin\BlueYonder.Server** folder in a new Visual Studio 2017 instance.
4. In the **BlueYonder.Companion.Host** project, edit the connection string within the **Web.config** file, and update the SQL Database server name.

    a. Locate the two **{ServerName}** placeholders in the **connectionString** attribute  
    b. Replace the placeholders with the SQL database server name from the portal.

5. Locate the client endpoint configuration for the Booking service, and change its address to point to the web-hosted service. Use the address: **net.tcp://localhost/BlueYonder.Server.Booking.WebHost/Booking.svc**.

#### Task 2: Publish the Companion Web API project to Azure

1. On the **View** menu, open the **Task List** window.

2. Switch to **Comments** and search for TO-DO items. Double-click each comment and look at the disabled WCF calls:

   - **UpdateReservationOnBackendSystem**
   - **CreateReservationOnBackendSystem**
   

  >**Note:** Prior to the deployment of the cloud project to Azure, all the on-premises WCF calls were disabled.  
  These include calls from the Reservation Controller class and the Trips Controller class.  
  After you deploy the ASP.NET Web API project to Azure, it cannot call the on-premises WCF service, so for now, the WCF Service calls are disabled. In Module 7, &quot;Windows Azure Service Bus&quot; in Course 20487, you will learn how a cloud application can connect to an on-premises service.

3. Use Visual Studio 2017 to publish the **BlueYonder.Companion.Host** project as a new Web App.

    a. Right click **BlueYonder.Companion.Host** project, and click **Publish**.
    b. Select **Microsoft Azure App Service** and select **Create New**
    c. Click **Publish**, a **Create App Service** window will open.
    d. Fill in the following details:
      - App Name: **BlueYonder-Companion-*YourInitials*** (replace *YourInitials* with your initials)
      - Resource Group: **BlueYonder.Lab.06**
      - App Service Plan:
        - Click **New**
        - App Service Plan: **BlueYonder_Companion_*yourInitials*** (replace *yourInitials* with your initials)
        - Location: Choose the location closest to you
        - Click **Ok**
    e. Click **Create** to start deployment.

#### Task 3:  Test the Companion client against the Azure hosted Companion Web API

1. Open a new instance of Visual Studio 2017.
2. Open the **BlueYonder.Companion.Client** solution at **[Repository root]\AllFiles\20487C\Mod06\LabFiles\BlueYonder.Companion.Client**
2. Replace the address used to communicate with the server:

    a. In the **BlueYonder.Companion.Shared** project, open the **Addresses** class and in the **BaseUri** property, replace the address of the emulator with the cloud service address you created earlier.  
    b. Use the form **https://blueyondercompanionYourInitials.cloudapp.net/** (replace _YourInitials_ with your initials).

3. Run the client app and search for flights to New York. Verify the client application is able to connect to the ASP.NET Web API Web application hosted in Windows Azure and retrieve the list of flights.

>**Results**: You will verify the application works on Azure.

### Exercise 3: Hosting the Flights Management Web Application in an existing Azure Web App

#### Scenario

In this exercise, you will deploy the flights manager web application to an Azure Web App. The first step will be to create an Azure Web App. After you create the Web App, you will download the publish settings file, which you will then use in Visual Studio 2012 to populate all the publish settings required by the publish process (such as destination server, user name, and password).

The main tasks for this exercise are as follows:

1. Create new Web App in Azure

2. Publish the flight manager web application to the Web App created in the first task.

#### Task 1: Create new Web App in Azure

1. Go back to the Azure portal.

2. Create a new Web App named: **BlueYonderCompanion-FlightManager-YourInitials** (_YourInitials_ contains your initials).

    a. Click **New** to open the **New** blade.
    b. Select **Web App** to start the Web App creation **journey**.
    c. Fill in the following values:
      - App name: **BlueYonderCompanion-FlightManager-YourInitials** (_YourInitials_ contains your initials).
      - Resource Group: **BlueYonder.Lab.06**
      - OS: **Windows**
      - App service plan: You should have the same app plan that was created in the previous step, if not:
        - Click **App ServicePlan**
        - Click **Create New**
        - Enter the following values:
          - App service plan: "BlueYonder-Lab-06"
          - Location: The location closest to you.
        - Click **Create**
    d. Click **Create** and wait until the new Web App has been created.

  > Result: You have created a new web app through the Azure portal.

#### Task 2: Upload the Flights Management web application to the new Web App by using the Microsoft Azure portal

1. Open the **BlueYonder.Companion.FlightsManager** solution file from the **[repository root]\AllFiles\20487C\Mod06\LabFiles\begin\BlueYonder.Server** folder in a new Visual Studio 2017 instance.

2. In the **BlueYonder.FlightsManager** project, open the **web.config** file, and edit the **&lt;appSettings&gt;** section to substitute the **{YourInitials}** placeholder with the initials you have used when you published the Companion web API project to a new Web App in the previous exercise.
3. Publish the **BlueYonder.FlightsManager** project.

   - Right click the **BlueYonder.FlightManager** project.
   - Click **Publish**.
   - Select **Microsoft Azure App Service** and then select "**Select Existing**"
   - You will see a list of **Resource Groups**. Expand the **BlueYonder.Lab.06** Resource Group.
   - Select the **BlueYonderCompanion-FlightManager-*YourInitials*** and click **Ok**.
   - Deployment will start automatically and when it is done, the browser will open.

4. Verify that you can see flight schedules from **Paris** to **Rome** , indicating that the application was able to retrieve information from the web role.

>**Results**: After you publish the flights manager web application, you will open the web application in a browser and verify if it is working properly and is able to communicate with the web role you deployed in the previous exercise.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

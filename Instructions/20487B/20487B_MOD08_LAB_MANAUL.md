# Module 8: Deploying Services

## Lab: Deploying Services

#### Scenario

Blue Yonder Airlines deployed its services to Azure and they are running successfully. Now they want to add a new weather forecast service. Before they deploy the new service to production, they want the service to be deployed to a staging environment for testing. In this lab, you will deploy a new version of the service to Azure, first to the staging environment, and then swap between staging and production environments.

In addition to the weather forecast service, Blue Yonder Airlines wants to scale its WCF booking service and the frequent flyer service to another server to increase booking service&#39;s durability. To meet this need, in this lab, you will create an IIS deployment package and deploy it to another server.

#### Objectives

After you complete this lab, you will be able to:

- Deploy a web application to an Azure cloud service staging environment, and perform a VIP Swap.
- Create an IIS deployment package, and install it on a different server.

#### Lab Setup

Estimated Time: **45 minutes**

Virtual Machine: **20487B-SEA-DEV-A**, **20487B-SEA-DEV-B**, and **20487B-SEA-DEV-C**

User name: **Administrator**, **Administrator**, and **Admin**

Password: **Pa$$w0rd**, **Pa$$w0rd**, and **Pa$$w0rd**

For this lab, you will use the available virtual machine environment. Before you begin this lab, you must complete the following steps:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.

2. In Hyper-V Manager, click **MSL-TMG1**, and in the **Actions** pane, click **Start**.
3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Actions** pane, click **Start**.
4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
5. Sign in by using the following credentials:

   - User name: **Administrator**
   - Password: **Pa$$w0rd**

6. Return to Hyper-V Manager, click **20487B-SEA-DEV-B**, and in the **Actions** pane, click **Start**.
7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
8. Sign in by using the following credentials:

   - User name: **Administrator**
   - Password: **Pa$$w0rd**

9. Return to Hyper-V Manager, click **20487B-SEA-DEV-C**, and in the **Actions** pane, click **Start**.

Verify that you received the credentials to sign in to the Azure portal from your training provider. These credentials and the Azure account will be used throughout the labs of this course.

### Exercise 1: Deploying an Updated Service to Azure

#### Scenario

Start by adding the code to implement the weather forecast service. Then, create an Azure application package, and deploy it to a staging deployment. After the staging and production deployments are online, test both with the client app to verify that the staging deployment is updated with the new service. The last step is to perform a VIP Swap of the two deployments, and verify that the weather forecast service is working in the production deployment.

The main tasks for this exercise are as follows:

1. Add the new weather updates service to the ASP.NET Web API project

2. Deploy the updated project to the staging environment by using the Azure portal

3. Test the client app with the production and staging deployments

4. Perform a VIP Swap by using the Azure portal and retest the client app

#### Task 1: Add the new weather updates service to the ASP.NET Web API project

1. On the **20487B-SEA-DEV-A** virtual machine, run the **setup.cmd** file from **D:\AllFiles\Mod08\LabFiles\Setup**.

   - Write down the names of the Azure Service Bus namespace and Azure cloud service.

2. Open the solution **[repository root]\AllFiles\20487C\Mod08\LabFiles\begin\BlueYonder.Server\BlueYonder.Companion.sln**.
3. Create a cloud deployment package for the cloud project.
4. Right-click the **BlueYonder.Companion.Host.Azure** project, and then click **Package**.
5. Create a package for the **Cloud** service configuration with the **Debug** build configuration.
6. Deploy the package to the production environment of your cloud service.
7. Open the Azure portal at http://manage.windowsazure.com and locate the cloud service that was created during the setup process.
8. Select the production environment for the cloud service and then click **Update** or **Upload** at the bottom of the page (only one of the buttons should be visible).
9. Use **Lab08** as the deployment label, and select the path to the package file, which should be located in the **bin\debug** folder of the begin solution. The file is **BlueYonder.Companion.Host.Azure.cspkg**.
10. Select the path of the **ServiceConfiguration.Cloud.cscfg** configuration file .The file is located in the same folder as the package file you previously selected.
11. Select the **Deploy even if one or more roles contain a single instance** check box and approve.
12. In the **INSTANCES** tab, verify that the instance is running.
13. In the **BlueYonder.Companion.Controllers** project, implement the **GetWeather** method of the **LocationController** class.
14. Create a new instance of the **WeatherService** class that is part of the **BlueYonder.Companion.Controllers** project.
15. Use the **Locations.GetSingle** method to get the **Location** object according to the **locationId** parameter.
16. Call the **GetWeather** method of the **WeatherService** class to get the **WeatherForecast** object.

   >**Note:** The Begin Solution already contains the **WeatherService** class. The class uses the **WeatherForecast** class and the **WeatherCondition** enum.  
   >Expand the **DataTransferObjects** folder to review the files.

17. In the **BlueYonder.Companion.Host** project, under the **App\_Start** folder, open the **WebApiConfig.cs** file, and add an additional HTTP route, before the other routes. Use the following settings:

| Key | Value |
| --- | --- |
| name | **LocationWeatherApi** |
| routeTemplate | **locations/{locationId}/weather** |
| defaults | Create a new anonymous type by using the following code.                                                                ```cs New {   controller = ";locations",   action = "GetWeather"```    } |
| constraints | Create a new anonymous type nu using the following code.                                                            ```cs new{   httpMethod = new HttpMethodConstraint(HttpMethod.Get)}```  |

#### Task 2: Deploy the updated project to the staging environment by using the Azure portal

1. Create a new package for the **BlueYonder.Companion.Host.Azure** project. Use the same procedure as in the previous task.

2. Deploy the package to the **Staging** environment of your cloud service.

   >**Note:** You are performing the same procedure as you did in Task 1 of this exercise, with one difference: you are deploying to the **Staging** environment and not to the **Production** environment.

#### Task 3: Test the client app with the production and staging deployments

1. On the **20487B-SEA-DEV-C** virtual machine, open the client solution from  **D:\AllFiles\Mod08\LabFiles\begin\BlueYonder.Companion.Client**.

2. In the **Addresses** class of the **BlueYonder.Companion.Shared** project, set the **BaseUri** property to the name of the Azure cloud service you wrote down at the beginning of this lab.
3. Run the client app without debugging, purchase a trip from Seattle to New York, and verify that the weather forecast for the current trip is missing the temperature.

   - The temperature text should show only the Fahrenheit sign.
   - Close the client app after you verify that the temperature is not shown.

4. In the **Addresses** class, duplicate the **BaseUri** property, and set one of the implementations to the staging deployment URL. Place the second implementation in comment, because you will need it in the next task.

   - In the Azure portal, open the configuration of your cloud service, and then copy the staging deployment URL to the **BaseUri**  property.

5. Run the client app again, verify that the weather forecast appears for the current trip, and then close the client app.

   >**Note:** The staging and the production deployments share their databases, which is why the current trip, which you created with the production deployment, appears when connect to the staging deployment. ** **

#### Task 4: Perform a VIP Swap by using the Azure portal and retest the client app

1. Return to the Azure portal and perform a **VIP Swap** between the staging and production deployments. Return to Visual Studio 2012 when the swap completes.

2. Set the service URL in the **BaseUri** property back to the URL of the production deployment, run the client app without debugging, and verify that the weather forecast appears.
3. Return to the Azure portal and delete the staging deployment.

   >**Note:** After you ensure that the production deployment is running successfully, we recommend that you delete the staging deployment to reduce the compute-hour charges.

   >**Results**: After you complete this exercise, the client app will retrieve weather forecast information from the production deployment in Azure.

### Exercise 2: Exporting and Importing an IIS Deployment Package

#### Scenario

To scale out the WCF services from the first server to the additional server, you must create a Web Deploy package that synchronizes everything the services will need to work correctly on the new server. This includes their files, the certificates they use, and IIS-related configuration.

After you create the deployment package, you will copy it to the remote server, sign in to that server, and deploy the package locally.

The main tasks for this exercise are as follows:

1. Export the web applications containing the WCF booking and frequent flyer services

2. Import the deployment package to a second server

### Task 1: Export the web applications containing the WCF booking and frequent flyer services

1. On the **20487B-SEA-DEV-A** virtual machine, open **IIS Manager**, select the **Default Web Site**, and then open the **Export Application Package** dialog box.

2. In the **Export Application Package** dialog box, open **Management Components**, and then clear the list of components.
3. Add two **appHostConfig** providers to synchronize the **Default Web Site/BlueYonder.Server.Booking.WebHost** and **Default Web Site/BlueYonder.Server. FrequentFlyer.WebHost** web applications.
4. Add an **appPoolConfig** provider to synchronize the **DefaultAppPool** application pool.
5. Store the package in **C:\backup.zip**. Complete the package creation and close IIS Manager.
6. Copy the **backup.zip** file from **C:\ to \\10.10.0.11\c$**.

#### Task 2: Import the deployment package to a second server

1. On the **20487B-SEA-DEV-B** virtual machine, open **IIS Manager**, select the **Default Web Site**, and then open the **Import Application Package** dialog box.

2. Import the package from **C:\backup.zip** by using the following settings.

   - Physical path of the Booking service: **C:\Services\BlueYonder.Server.Booking.WebHost**
   - Physical path of the Frequent Flyer service: **C:\Services\BlueYonder.Server.FrequentFlyer.WebHost**

3. Close IIS Manager, open the Azure portal(http://manage.windowsazure.com), and verify that there are two listeners for the  **booking** relay.
4. In the Azure portal, locate the Service Bus namespace you wrote down at the beginning of this lab
5. Open the Service Bus configuration, click the **RELAYS** tab, and verify that there are two listeners for the **booking** relay.

   >**Results**: As soon as both the servers are online, they will listen to the same Service Bus relay and will be load balanced. You will verify that both servers are listening by checking the Service Bus relay listener&#39;s information supplied by Service Bus in the Azure portal.

Perform the following steps to apply the **StartingImage** snapshot:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
2. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Snapshots** pane, right-click **StartingImage**, and then click **Apply**.
3. In the **Apply Snapshot** dialog box, click **Apply**.
4. Repeat Step 2 for all the virtual machines that you used in this lab. (excluding **MSL-TMG1**).

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

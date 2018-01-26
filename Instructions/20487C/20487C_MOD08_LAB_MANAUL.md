# Module 8: Deploying Services

> Wherever you see a path to file starting at [repository root], replace it with the absolute path to the directory in which the 20487 repository resides. 
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487, then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

## Lab: Deploying Services

#### Scenario

Blue Yonder Airlines deployed its services to Azure and they are running successfully. Now they want to add a new weather forecast service. Before they deploy the new service to production, they want the service to be deployed to a staging environment for testing. In this lab, you will deploy a new version of the service to Azure, first to the staging environment, and then swap between staging and production environments.

In addition to the weather forecast service, Blue Yonder Airlines wants to scale its WCF booking service and the frequent flyer service to another server to increase booking service&#39;s durability. To meet this need, in this lab, you will create an IIS deployment package and deploy it to another server.

#### Objectives

After you complete this lab, you will be able to:

- Deploy a web application to an Azure cloud service staging environment, and perform a VIP Swap.
- Create an IIS deployment package, and install it on a different server.

#### Lab Setup

Estimated setup time: 15 minutes.

Verify that you received the credentials to sign in to the Azure portal from your training provider. These credentials and the Azure account will be used throughout the labs of this course.

> **Important!** make sure you are logged in to Visual Studio 2017 with your Microsoft account before starting this lab!

1. On the **Start** menu, search for **Command Prompt**, right click it and click **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. Go to **[repository root]\AllFiles\20487C\Mod08\LabFiles\Setup**
4. Run the following command:
```batch
ps createAzureServices.ps1
```
5. Follow the on-screen instructions.

### Exercise 1: Deploying an Updated Service to Azure

#### Scenario

Start by adding the code to implement the weather forecast service. Then, create an Azure application package, and deploy it to a staging deployment. After the staging and production deployments are online, test both with the client app to verify that the staging deployment is updated with the new service. The last step is to perform a VIP Swap of the two deployments, and verify that the weather forecast service is working in the production deployment.

The main tasks for this exercise are as follows:

1. Add the new weather updates service to the ASP.NET Web API project

2. Deploy the updated project to an App Service staging deployment slot.

3. Test the client app with the production and staging deployments

4. Perform a Swap by using the Azure portal and retest the client app

#### Task 1: Add the new weather updates service to the ASP.NET Web API project

1. Open the solution **[repository root]\AllFiles\20487C\Mod08\LabFiles\begin\BlueYonder.Server\BlueYonder.Companion.sln**.
2. Publish the **BlueYonder.Companion.Host** project to the **blueyonder-companion-08-_yourinitials_** (_yourinitials_ are your initials, e.g. John Doe -> jd) app service.
   > The mentioned app service should have been created during the lab setup.
3. In the **BlueYonder.Companion.Controllers** project, implement the **GetWeather** method of the **LocationController** class.
4. Create a new instance of the **WeatherService** class that is part of the **BlueYonder.Companion.Controllers** project.
5. Use the **Locations.GetSingle** method to get the **Location** object according to the **locationId** parameter.
6. Call the **GetWeather** method of the **WeatherService** class to get the **WeatherForecast** object.

   >**Note:** The Begin Solution already contains the **WeatherService** class. The class uses the **WeatherForecast** class and the **WeatherCondition** enum.  
   >Expand the **DataTransferObjects** folder to review the files.

7. In the **BlueYonder.Companion.Host** project, under the **App\_Start** folder, open the **WebApiConfig.cs** file, and add an additional HTTP route, before the other routes. Use the following settings:

| Key | Value |
| --- | --- |
| name | **LocationWeatherApi** |
| routeTemplate | **locations/{locationId}/weather** |
| defaults | Create a new anonymous type by using the following code.                                                                ```cs New {   controller = ";locations",   action = "GetWeather"```    } |
| constraints | Create a new anonymous type nu using the following code.                                                            ```cs new{   httpMethod = new HttpMethodConstraint(HttpMethod.Get)}```  |


#### Task 2: Deploy the updated project to an App Service staging deployment slot

1. Create an **App Service** in Microsoft Azure, named **blueyonder-companion-08-_yourinitials_** under the **BlueYonder.Lab.08** resource group.
2. Open the **blueyonder-companion-08-_yourinitials_** App Service.
3. Go to **Deployment slots** and add a **Staging** slot.
4. In **Visual Studio 2017**, open the publish page for the **BlueYonder.Companion.Host** project
5. Publish the **BlueYonder.Companion.Host** project to the app service you just created:
6. In the publish page select **Azure App Service** and select "**Select Existing**"
7. In the **App Service** modal, expand the **BlueYonder.Lab.08** node
8. Expand **blueyonder-companion-08-_yourinitials_** node
9. Expand the **Deployment Slots** node
10. Select **Staging**.
11. Click **Ok**.

   >**Note:** You are performing the same procedure as you did in Task 1 of this exercise, with one difference: you are deploying to the **Staging** environment and not to the **Production** environment.

#### Task 3: Test the client app with the production and staging deployments

1. Open the client solution from  **[repository root]\AllFiles\20487C\Mod08\LabFiles\begin\BlueYonder.Companion.Client**.

2. In the **Addresses** class of the **BlueYonder.Companion.Shared** project, set the **BaseUri** property to the name of the Azure App Service that was created during the lab setup (**blueyonder-companion-08-_yourinitials_**).
3. Run the client app without debugging, purchase a trip from Seattle to New York, and verify that the weather forecast for the current trip is missing the temperature.

   - The temperature text should show only the Fahrenheit sign.
   - Close the client app after you verify that the temperature is not shown.

4. In the **Addresses** class, duplicate the **BaseUri** property, and set one of the implementations to the staging deployment URL. Place the second implementation in comment, because you will need it in the next task.

   - In the Azure portal, open the configuration of your app service, and then copy the staging deployment slot URL to the **BaseUri**  property.

5. Run the client app again, verify that the weather forecast appears for the current trip, and then close the client app.

   >**Note:** The staging and the production deployments share their databases, which is why the current trip, which you created with the production deployment, appears when connect to the staging deployment. ** **

#### Task 4: Perform a Swap by using the Azure portal and retest the client app

1. Return to the Azure portal and perform a **Swap** between the staging and production slots. Return to Visual Studio 2017 when the swap completes.

2. Set the service URL in the **BaseUri** property back to the URL of the production deployment slot, run the client app without debugging, and verify that the weather forecast appears.
3. Return to the Azure portal and delete the staging deployment.

   >**Note:** After you ensure that the production deployment is running successfully, we recommend that you delete the staging deployment to reduce the compute-hour charges.

   >**Results**: After you complete this exercise, the client app will retrieve weather forecast information from the production deployment in Azure.

### Exercise 2: Exporting and Importing an IIS Deployment Package.

#### Scenario

To prepare for a potential system failure, you will need to back up the WCF services, you must create a Web Deploy package that contains everything the services will need to work correctly after a system restore. This includes their files, the certificates they use, and IIS-related configuration.

After you create the deployment package, you will delete the existing application, and then re-deploy the application using the Web Deploy package.

The main tasks for this exercise are as follows:

1. Deploy the Booking and FrequentFlyer web applications to IIS.

2. Export the web applications containing the WCF booking and frequent flyer services

3. Delete the web applications and import the deployment package to restore them.

### Task 1: Deploy the Booking and FrequentFlyer web applications to IIS.

1. Open the solution **[repository root]\AllFiles\20487C\Mod08\LabFiles\begin\BlueYonder.Server\BlueYonder.Server.sln** as an Administrator.
2. Publish the **BlueYonder.Server.Booking.WebHost** to IIS with the following values:
    - Server: **localhost**
    - Site name: **Default Web Site/BlueYonder.Server.Booking.WebHost**
3. Publish the **BlueYonder.Server.FrequentFlyer.WebHost** to IIS with the following values:
    - Server: **localhost**
    - Site name: **Default Web Site/BlueYonder.Server.FrequentFlyer.WebHost**

### Task 2: Export the web applications containing the WCF booking and frequent flyer services

1. Open **IIS Manager**, select the **Default Web Site**, and then open the **Export Application Package** dialog box.
2. In the **Export Application Package** dialog box, open **Management Components**, and then clear the list of components.
3. Add two **appHostConfig** providers to synchronize the **Default Web Site/BlueYonder.Server.Booking.WebHost** and **Default Web Site/BlueYonder.Server. FrequentFlyer.WebHost** web applications.
4. Add an **appPoolConfig** provider to synchronize the **DefaultAppPool** application pool.
5. Store the package in **[Path to your user folder]\Downloads\backup.zip**. Complete the package creation and close IIS Manager.

#### Task 3: Delete the web applications and import the deployment package to restore them

1. Open **IIS Manager** and remove the **BlueYonder.Server.Booking.WebHost** and **BlueYonder.Server.FrequentFlyer.WebHost** applications.

2. Import the package from **[Path to your user folder]\Downloads\backup.zip**.

3. Close IIS Manager, open the Azure portal(http://portal.azure.com), and verify that there is a listener for the  **booking** relay.
4. In the Azure portal, locate the Service Bus Relay you wrote down at the beginning of this lab
5. Open the Service Bus Relay configuration, click the **Relays** tab, and verify that there is a listener for the **booking** relay.

   >**Results**: After a "system failure", you've restored the Booking and FrequentFlyer services by redeploying using a backup package, thus they are back online.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

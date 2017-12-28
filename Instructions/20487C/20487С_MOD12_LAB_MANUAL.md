# Module 12: Scaling Services

# Lab: Scalability

#### Scenario

The final task that you need to perform in the Blue Yonder Airlines application is to reduce the ASP.NET Web API back-end service database load by storing the static data that was fetched from the database in a distributed cache. In this lab, you will add a distributed caching mechanism to the ASP.NET Web API service.

#### Objectives

After completing this lab, you will be able to:

- Use Windows Azure Caching with Web applications.

#### Lab Setup

Estimated Time: **30 Minutes**.

Virtual Machine: **20487B-SEA-DEV-A, 20487B-SEA-DEV-C**

User name: **Administrator, Admin**

Password: **Pa$$w0rd, Pa$$w0rd**

For this lab, you will use the available virtual machine environment. Before you begin this lab, you must complete the following steps:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
2. In Hyper-V Manager, click **MSL-TMG1**, and in the **Actions** pane, click **Start**.
3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Actions** pane, click **Start**.
4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
5. Sign in using the following credentials:

	- User name: **Administrator**
	- Password: **Pa$$w0rd**

6. Return to Hyper-V Manager, click **20487B-SEA-DEV-C**, and in the **Actions** pane, click **Start**.
7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
8. Sign in using the following credentials:

	- User name: **Admin**
	- Password: **Pa$$w0rd**

9. Verify that you received credentials to sign in to the Windows Azure portal from you training provider, these credentials and the Windows Azure account will be used throughout the labs of this course.

In this lab, you will install NuGet packages. It is possible that some NuGet packages will have newer versions than those used when developing this course. If your code does not compile, and you identify the cause to be a breaking change in a NuGet package, you should uninstall the NuGet package and instead, install the old version by using Visual Studio&#39;s Package Manager Console window:

1. In Visual Studio, on the **Tools** menu, point to **Library Package Manager**, and then click **Package Manager Console**.
2. In **Package Manager Console**, type the following command, and then press Enter.

  ```cs
		install-package PackageName -version PackageVersion -ProjectName ProjectName
```
(The project name is the name of the Visual Studio project that is written in the step where you were instructed to add the NuGet package).

3. Wait until **Package Manager Console** finishes downloading and adding the package.

The following table details the compatible versions of the packages used in the lab:

| Package name | Package version |
| --- | --- |
| Microsoft.WindowsAzure.Caching | 2.9.5.0 |

### Exercise 1: Use Microsoft Azure Caching

#### Scenario

To reduce the number of requests sent to the database from the ASP.NET Web API back-end service, you will add a distributed cache. Each time a user requests information regarding certain flights, the result of querying the database is stored in the distributed cache. If another user requests the same information, that information will be retrieved from the cache instead of from the database.

In this exercise, you will add a Microsoft Azure Cache worker role, and configure the flights API controller to first check

The main tasks for this exercise are as follows:

1. Add a Caching Worker Role to the Cloud Project.

2. Add the Microsoft Azure Caching NuGet Package to the ASP.NET Web API Project.

3. Add Code to Cache the List of Searched Locations.

4. Debug Using the Client Application.

#### Task 1: Add a Caching Worker Role to the Cloud Project

1. In the **20487B-SEA-DEV-A** virtual machine, run the **setup.cmd** script from **D:\AllFiles\Mod12\LabFiles\Setup**. Provide the information according to the instructions.

   >**Note:** You might see warnings in yellow indicating a mismatch in the versions and obsolete settings. These warnings might appear if there are newer versions of Azure PowerShell cmdlets. If these warnings are followed by a red error message, please inform the instructor, otherwise you can ignore them.

2. Open the **D:\Allfiles\Mod12\LabFiles\begin\BlueYonder.Server\BlueYonder.Companion** solution file.
3. Add a Cache Worker Role named **BlueYonder.Companion.CacheWorkerRole** to the **BlueYonder.Companion.Host.Azure** project and then build the solution.

#### Task 2: Add the Microsoft Azure Caching NuGet Package to the ASP.NET Web API Project

1. Install version 2.9.5.0 of the **Microsoft.WindowsAzure.Caching** NuGet package in the **BlueYonder.Companion.Host** project.

	- To install a specific version of a NuGet package, first open the **Packager Manager Console** window (on the **View** menu, under **Other Windows** ).
	- In **Package Manager Console**, type the following command and then press Enter:

  ```cs
		install-package Microsoft.WindowsAzure.Caching -version 2.9.5.0 -ProjectName BlueYonder.Companion.Controllers
```
2. Install version 2.9.5.0 of the **Microsoft.WindowsAzure.Caching** NuGet package in the **BlueYonder.Companion.Host** project.

 	- To install a specific version of a NuGet package, first open the **Packager Manager Console** window (on the **View** menu, under **Other Windows** ).
 	- In **Package Manager Console**, type the following command and then press Enter:

  ```cs
		install-package Microsoft.WindowsAzure.Caching -version 2.9.5.0 -ProjectName BlueYonder.Companion.Host
```
    >**Note:** You might see error messages in red in the console window that say **Exception calling &quot;Item&quot; with &quot;1&quot; argument(s)**. Those messages can be ignored. If you see any other error messages, please inform the instructor.

3. Open the **Web.config** file from the **BlueYonder.Companion.Host** project, and then in the **&lt;dataCacheClients&gt;** section, replace the **[cache cluster role name]** string with **BlueYonder.Companion.CacheWorkerRole**.

#### Task 3: Add Code to Cache the List of Searched Locations.

1. In the **BlueYonder.Companion.Controllers** project, open the **FlightsController** class, and locate the **Get** method that receives three parameters. In the method locate the comment _// TODO: Place cache initialization here_, and after it add the code to create a **DataCache** object for the default cache. Use the **DataCacheFactory.GetDefaultCache** method to get the data cache object.
2. Still in the **Get** method, locate the comment _// TODO: Place cache check here_, and after it add the code to check whether the cache already contains the requested list of flight schedules. Use the **DataCache.Get** method to get the cached item, and store the result in the **routesWithSchedules** variable. For the key, use a semicolon-separated string containing the **source**, **destination** and **date** parameters of the method.

   >**Note:** The **date** parameter of the **Get** method is a nullable DateTime. You should make sure the cache key you create will not get set to null if the **date** parameter is null.

3. Still in the **Get** method, locate the comment _// TODO: Insert into cache here_, and after it add the code to store the **routesWithSchedules** variable in the cache by using the **DataCache.Put** method. Use the cache key as you created it before for the **DataCache.Get** method call.

#### Task 4: Debug Using the Client Application

1. Still in the **Get** method, place a breakpoint in the beginning of the method, set the **BlueYonder.Companion.Host.Azure** project as the startup project, and then run the project in debug.
2. Switch into the **20487B-SEA-DEV-C** virtual machine.
3. Open the **BlueYonder.Companion.Client** solution file from the **D:\AllFiles\Mod12\LabFiles\begin\BlueYonder.Companion.Client** folder.
4. Run the client without debugging, search for a destination that contains the letter **N**, and go back to **20487B-SEA-DEV-A** virtual machine to see the code execution breaks. Debug the code in the Flights controller to verify it retrieves data from the database.

   >**Note:** Normally, the Azure Emulator is not accessible from other computers on the network. For the purpose of testing this lab from a Windows 8 client, a routing module was installed on the server&#39;s IIS, routing the incoming traffic to the emulator.

5. Close the client app, re-open it without debugging, search again for a destination that contains the letter **N**, and verify the Flights controller retrieves data from the cache.

  >**Results**: After completing this exercise, you should have successfully added a caching worker role to the Cloud project, and implemented other Microsoft Azure caching features.

Perform the following steps to apply the **StartingImage** snapshot:

1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
2. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the **Snapshots** pane, right-click **StartingImage**, and then click **Apply**.
3. In the **Apply Snapshot** dialog box, click **Apply**.
4. Repeat Step 2 for all the virtual machines that you used in this lab. (excluding **MSL-TMG1**).

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

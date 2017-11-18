# Module 1: Overview of Service and Cloud Technologies

# Lab: Exploring the Work Environment

#### Scenario

In this lab you will explore several frameworks and platforms, such as Entity Framework, ASP.NET Web API, and Azure, that are used for creating distributed applications.

#### Objectives

After completing this lab, you will be able to:

- Create an entity data model by using the data model wizard.
- Create an ASP.NET Web API service.
- Create an Azure SQL database.
- Deploy a web application to an Azure website.

### Exercise 1: Creating an Azure SQL Database

#### Scenario

In this exercise, you will use Azure portal and create a new Azure SQL Database server. You will connect to the new SQL Database server from SQL Server Management Studio and create a BlueYonder database from the .bacpac file.

The main tasks for this exercise are as follows:

1. Create a new Azure SQL Database server

2. Manage the Azure SQL Database server from SQL Server Management Studio

#### Task 1: Create a new Azure SQL Database server

1. Open a browser and navigate to the Azure portal at **https://portal.azure.com**.
2. On the left side navigation pane, click the **+ New** button.
3. In the search box, enter **SQL Server**, then select **SQL Server (logical server)** from the results.
4. Enter the following values:
  - Server name: **my-first-azure-sql-database-*YourInitials*** (replace *YourInitials* with your initials, e.g. - John Doe -> jd)
  - Server admin login: **SQLAdmin**
  - Password: **Pa$$w0rd**
  - Confirm Password: **Pa$$w0rd**
  - Resource Group: Select **Create new**.
  - Location: Select the location closest to you.
5. Click **Create** and wait until the server is created.
You can track its progress by hovering over the bell icon on the top bar.

#### Task 2: Manage the Azure SQL Database server from SQL Server Management Studio

1. Open the SQL server you created in the previous step by clicking on it in the **All Resources** pane on the Azure portal.
2. Click on **Firewall/Virtual Networks**, then click **Add client IP** and then click **Save**
3. Open Microsoft SQL Server Management Studio 2017 and connect to the new server. Use the server name  **SQLServerName.database.windows.net**, and the sign-in name and the password that you used in the previous task (Replace **SQLServerName** with the server name that you wrote down in the previous task).
4. In Object Explorer, right-click the **Databases** node, and then click **Import Data Tier Application**.
5. Import the **BlueYonder.bacpac** file located in the **[Repository root]\AllFiles\20487C\Mod01\LabFiles\Assets** folder.
6. Verify that the **BlueYonder** database is created.

   
   >**Results** : After completing this exercise, you should have created an Azure SQL database in your Azure account.

### Exercise 2: Creating an Entity Data Model

#### Scenario

In this exercise, you will create a new Class Library project and use Entity Framework in it, to connect to the Azure SQL database. You will use the Entity Framework database-first development approach to generate classes for the **BlueYonder** database tables.

The main tasks for this exercise are as follows:

1. Create an Entity Framework Model

#### Task 1: Create an Entity Framework Model

1. Open Visual Studio 2017 and create a new **Class Library** project. Name the project **BlueYonder.Model**.

2. Add an ADO.NET Entity Data Model to the project.

    a. Connect to the **SQLServerName.database.windows.net** server with the sign-in name and the password that you used in the previous task (replace **SQLServerName** with the server name you have written down in the previous exercise), and then select the **BlueYonder** database.

    b. Make sure to select the option to include the database password in the connection string.
    c. Import the **Locations** and **Travelers** tables.
    d. Save the EDMX file after it opens, and then close it.

   
   >**Results** : After completing this exercise, you should have created Entity Framework wrappers for the **BlueYonder** database.

### Exercise 3: Managing the Entity Framework Model with an ASP.NET Web API Project

#### Scenario

Create a new ASP.NET web application that exposes the Web API for CRUD operations on the BlueYonder database, by using the Entity Framework model.

The main tasks for this exercise are as follows:

1. Create an ASP.NET Web API Project

2. Add a Web API Controller with CRUD Actions by using the Add Controller Wizard

#### Task 1: Create an ASP.NET Web API Project

- Add a new **ASP.NET Web Application (.NET Framework)** project named **BlueYonder.MVC** using the Web API template.

#### Task 2: Add a Web API Controller with CRUD Actions by using the Add Controller Wizard

1. In the **BlueYonder.MVC** project, add a reference to the **BlueYonder.Model** project.

2. Copy the connection string from the **BlueYonder.Model** project to the **BlueYonder.MVC** project (from the **App.config** file to the **Web.config** file).
3. Build the solution, open Server Explorer, and then refresh the **Data Connections**.
4. In Solution Explorer, in the **BlueYonder.MVC** project, right-click the **Controllers** folder, and then add a new controller named **LocationsController** , by using the following steps:

5. Create the new controller by using the **Web API 2 Controller with actions, using Entity Framework** template.

6. Select the **Location** model class.
7. Select the **BlueYonderEntities** data context class.

   >**Note:** You now have a Web API controller for the Location model.

8. Run the **BlueYonder.MVC** web application and in the web browser, append the **api/locations** string to the URL to download the list of locations. Open the downloaded file and verify that you see a JSON formatted list of locations.

   
   >**Results** : After completing this exercise, you should have created a web app that exposes the Web API for CRUD operations on the BlueYonder database.

### Exercise 4: Deploying a Web App to Azure

#### Scenario

In this exercise, you will create an Azure web app to host the ASP.NET MVC 4 web application.

The main tasks for this exercise are as follows:

1. Create a New Azure Web App

2. Deploy the web application to the Azure web app

3. Delete the Azure SQL Database Server

#### Task 1: Create a New Azure Web App

1. Open Azure portal at **https://portal.azure.com**.
2. On the left side navigation pane, click **+ New** and then click **Web App**.
3. Name the web app **BlueYonderWebSite_YourInitials_** (Replace **_YourInitials_** with your initials), create a new App Service plan, and then select the region that is closest to your location.
4. Click **Create** and wait until the new web app is ready.

#### Task 2: Deploy the web application to the Azure web app

1. In Visual Studio 2017, right-click the **BlueYonder.MVC** project in Solution Explorer, and then click **Publish**.
2. Select **Microsoft Azure App Service** and make sure **Select Existing** is selected.
3. Click **Publish**.
4. An **App Service** window will open. If you are logged in to Visual Studio, then your account may appear on the top right. If it doesn't, click **Add an Account** dropdown, then click the **Add an account** item and proceed through the sign in process.
5. After you've signed in with your Azure account, the subscription field will be populated automatically and the **BlueYonderWebsite_yourInitials_** (replace _yourInitials_ with your initials) folder will appear below the group of fields.
6. Expand **BlueYonderWebsite_yourInitials_** folder and select the **BlueYonderWebsite_yourInitials_** web app.
7. Click **Ok** and the publishing process will start, it may take a few minutes.
8. After the publish has finished, open a browser and navigate to **http://blueyonderwebsite_yourInitials_.azurewebsites.net/api/locations**
4. An XML or JSON describing a list of locations will appear. (On Chrome - XML, Edge and Firefox - JSON)

#### Task 3: Delete the Azure SQL Database Server

1. Open a browser and navigate to the Azure portal at **https://portal.azure.com**.
2. Open the SQL Server you created in the previous step by opening it through the **All Resources** pane.
3. Click **Delete** and then enter the server name and then **Delete** to start the deletion process.

   >**Note:** Azure free subscriptions have a resource limitation and a restriction on the total working hours. To avoid exceeding those limitations, you must delete the Azure SQL Database resources.

   
  
  >**Results** : After completing this exercise, you should have ensured that all your products are hosted on the Microsoft Azure cloud by using SQL Databases and Azure Web Apps.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

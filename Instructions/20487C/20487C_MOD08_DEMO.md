# Module 8: Deploying Services

> Wherever  you see a path to file starting at *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.

> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487,
then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

## Setup

For this demo you will need IIS activated on your machine.
To activate IIS on Windows 10, perform the following steps:
1. Open the **Start** menu and search for **Turn windows features on or off** and click it.
2. In the **Windows Features** window, check **Internet Information Services**.
3. Expand **Internet Information Services**.
4. Expand **World Wide Web Services**.
5. Check **Application Development Features**.
6. Click **OK**.

# Lesson 1: Web Deployment with Visual Studio 2012

### Demonstration: Deploying a Web Application by Using Visual Studio

#### Demonstration Steps

1. Open **Visual Studio 2017** as administrator:
    - Open the **Start** menu.
    - Look for **Visual Studio 2017**
    - Right click **Visual Studio 2017**
    - Click **Run as Administrator**
    - In the **User Account Control** dialog, click **Yes**.
2. On the **File** menu, point to **New**, and then click **Project**.
3. In the **New Project** dialog box, in the left pane, expand the **Installed** node, expand the **Installed** node, expand the **Visual C#** node, click the **Web** node, and then select **ASP.NET Web Application (.NET Framework)** from the list of templates.
4. In the **Name** box, type **MyApp**.
5. In the **Location** box, type **[repository root]\Allfiles\Mod08\DemoFiles\DeployWebApp\begin**.
6. Clear the **Create directory for solution** check box, and then click **OK**.
7. In the **New ASP.NET Web Application** dialog box, select **Web API**, and then click **OK**.
8. In Solution Explorer, right-click the **MyApp** project, and then click **Publish**.
9. In the **Publish** page, select **IIS, FTP, etc** and click **Publish**
10. In the **Publish** dialog box, verify that the selected publish method is **Web Deploy**, and set the rest of the fields by using the following values:

    - Server: **localhost**
    - Site Name: **Default Web Site/MyApp**
    - Destination URL: **http://localhost/MyApp/api/values**

12. Click **Validate Connection**, and wait for the green check mark to appear.
13. Click **Next**, click **Next** again, and then click **Save**.
14. Wait until the publishing completes and the browser opens. You will see either a XML or a JSON output depending on your browser.
15. Return to Visual Studio 2017, and in Solution Explorer, under the **MyApp** project, expand the **Controllers**  folder, and then double-click **ValuesController.cs**.
16. Locate the parameterless **Get** method, and change its code as follows:

  ```cs
		return new string[] { "value1", "value2", "value3" };
```
17. To save changes that you made to the code file, press Ctrl+S.
18. In Solution Explorer, right-click the **MyApp** project, and then click **Publish**.
19. In the **Publish** page, click **Publish**.
20. Wait until the publishing completes and the browser opens. Verify that you see three values.

# Lesson 2: Creating and Deploying Web Application Packages

### Demonstration: Exporting and Importing Web Deploy Packages by Using IIS Manager

#### Preparation Steps

> This demonstration is based off Lesson 1, make sure to complete it before starting this lesson.

If you are using Windows 10, Windows Server 2016 (or later), application package export and import feature is not available in the default IIS configuration and you will need to enable it.

First, check that you have the application package export/import available:
1. In the **Start** menu, search for **IIS** and click **Internet Information Services (IIS) Manager**.
2. In the **Internet Information Services (IIS) Manager** window, expand the **_yourcomputername_** node. (_yourcomputername_ is your computer name, you will most likely have only one node under connections)
3. Expand the **Sites** node, then expand the **Default Web Site** node and then click the **MyApp** web application node.
4. On the right hand side of the window, you should see either **Actions**, **Deploy** or both.
5. If you have Application Export/Import feature enabled, then you should see two items under either **Actions** or **Deploy**:
    - Export Application Package
    - Import Application Package

In the case the above feature isn't enabled and you have Windows 10 or Windows Server 2016 or later:
First, you will have to replace the existing web deploy supplied by Visual Studio 2017 as you will be unable to modify it.
1. In the **Start** menu, search for **Add or Remove Programs** and click it.
2. In the **Apps & Features** pane, in the search box, enter **Microsoft Web Deploy 3.6**.
3. Click the result with the name **Microsoft Web Deploy 3.6** and click **Uninstall**, follow the on-screen instructions.
4. Once done with uninstalling **Microsoft Web Deploy 3.6**, open a browser and go to **https://www.microsoft.com/en-us/download/details.aspx?id=43717**, then click the **Download** button.
5. When the download finishes, open the downloaded file.
6. Click **Next**, then check the **I accept the terms...** checkbox and click **Next**.
7. In the **Choose Setup Type**, click **Complete** and click **Next** and **Next** again.
8. Once the setup is done, click **Finish**.

To verify that the installation worked, follow the first set of instructions above.

#### Demonstration Steps

1. In the **Start** menu, search for **Internet Information Services (IIS)** and click it.
2. In the **Connections** pane, expand the **_yourcomputername_** node. (_yourcomputername_ is your computer name, you will most likely have only one node under connections)
3. If you receive a message that asks whether you want to get started with Microsoft Web Platform, click **No**. 
4. While still in the **Connections** pane, expand the **Sites** node, then expand the **Default Web Site** node, and then click the  **MyApp** web application node.
5. In the **Actions** pane, under **Deploy**, click **Export Application**.
6. In the **Export Application Package**, dialog box, click **Next**, and then click **Next** again.
7. In the **Package path** box, browse to the **Downloads** folder and in the **File name** field, type **MyApp.zip**, click **Ok** and then click **Next**.
8. Wait for the export to complete, and then click **Finish**.
9. On the left hand side of the window, right click **MyApp** and click **Remove**.
10. Click **Yes**.
11. Open a browser and navigate to **http://localhost/MyApp/api/values**, you should get a 404 error.
11. Click the **Default Web Site** node.
12. In the **Actions** pane, under **Deploy**, click **Import Application**.
13. In the **Package path** box of the **Import Application Package**, dialog box, browse to the **Downloads** folder and select **MyApp.zip**, and then click **Next**.
14. Click **Next** again, and verify that in the **Enter Application Package Information** step of the wizard, the **Application Path** box is set to **MyApp. Click Next**
15. In the **Overwrite Existing Files** step, select **Yes** and click **Next**.
16. Wait for the import to complete, and then click **Finish**.
17. Open a browser and navigate to **http://localhost/MyApp/api/values**, you should see either XML or JSON output, depending on your browser.

# Lesson 3: Command-Line Tools for Web Deploy

### Demonstration: Using Windows PowerShell Cmdlets

#### Preparation Steps

> This lesson is based off Lesson 2, make sure to complete lesson 2 before starting this lesson.

#### Demonstration Steps

1. Open the **Start** menu and search for **Windows Powershell**.
2. Right click **Windows Powershell** and click **Run as Administrator**, then click **Yes**.
3. In the Administrator: Windows PowerShell window, type the following command and press Enter.

  ```cs
		Add-PSSnapin WDeploySnapin3.0
```
4. To create a publish settings file, type the following command and press Enter.

  ```cs
		New-WDPublishSettings -ComputerName 127.0.0.1 -AgentType MSDepSvc -FileName "C:\[path to downloads folder]\Server.publishsettings"
```
5. To synchronize the web application to the remote server, type the following command and press Enter.

  ```cs
		Sync-WDApp "Default Web Site/MyApp" "Default Web Site/MyAppDeployedWithPowerShell" -DestinationPublishSettings "C:\[path to downloads folder]\Server.publishsettings"
```
6. Open a browser and navigate to **http://localhost/MyAppDeployedWithPowerShell/api/values**.
7. Wait until the response is returned, you should see 3 values in the form of either XML or JSON, depending on your browser.

# Lesson 5: Continuous Delivery with TFS and Git

### Demonstration: Continuous Delivery with Visual Studio Team Services

#### Preparation Steps
  
To present this demonstration, you must have a Microsoft Account. If you have not created a Microsoft Account before, create one before you start the demonstration.

> **Important!** make sure you are connected to your Microsoft account in Visual Studio 2017 before starting this demonstration!

#### Demonstration Steps

1. On the **Start** screen, click the **Internet Explorer** tile.
2. Go to **https://www.visualstudio.com/team-services/** and click **Get started for free**.
3. On the **Sign in** page, type your **Microsoft account** email address and click **Next**.
   > If instead of a **Sign in** page, you see a **Pick an account** page, pick your account and click **Next**.
4. On the **Enter password** page, enter your password, and then click **Sign in**.
5. If this is the first time you logged in with your account to VSTS, follow the next steps, else skip to step 14.

6. In the **Host my projects at** page, enter a unique name. (We will relate to that unique name as _youraccount_ from now on)
7. Under **Manage code using**, select **Team Foundation Version Control**.
8. Click **Create Account**.
9. Wait until the account creation is done and you are redirected to **MyFirstProject** page.
10. In the **MyFirstProject** page, point to the **settings icon** on the top menu bar and click **Account settings**.
11. Under **Projects**, point to **MyFirstProject** and click the **three dots** next to it.
12. Click **Delete**, in the textbox enter **MyFirstProject** and click **Delete Project**.
13. Click on the VSTS logo on the top left.

14. If you already have projects in your account, click **New Project**, else skip to the next step.

15. In the **Create new project** page, enter the following details:
    - **Project name**: MyApp
    - **Version control**: Team Foundation Version Control
16. Click **Create** 
17. Wait for the project to be created.
18. Open **Visual Studio 2017**.
19. On the **View** menu, click **Team Explorer**.
20. In the Team Explorer window, click on the **Manage Connections** icon, and then click the **Manage Connections** link, then click **Connect to a Project**.
21. In the **Connect to a Project** dialog box, expand the **https://_youraccount_.visualstudio.com** node, expand **MyApp** and click **$/MyApp**, then click **Connect**.
22. In the **Team Explorer** window, under **Solutions**, click **New**.
23. In the **New Project** dialog box, in the left pane, expand the **Installed** node, expand the  **Visual C#** node, click the **Web** node, and then select **ASP.NET Web Application (.NET Framework)** from the list of templates.
24. In the **Name** box, type **MyTFSWebApp** and click **OK**.
25. In the **New ASP.NET Web Application** dialog box, select **Web API**, and then click **OK**.
26. In Solution Explorer, right-click the **Solution &#39;MyTFSWebApp&#39;** node, and then click **Check In**.
27. In Team Explorer, click **Check In**.
28. If you are prompted by a **Check-in Confirmation** dialog box, click **Yes**. Wait for the check in to complete.
29. Open the **Team Explorer** window and click **Source Control Explorer**.
30. Under **Folders**, right click **MyTFSWebApp**, point to **Branching and Merging** and click **Convert to Branch**.
31. Return to the browser window and go to **https://portal.azure.com**.
32. If a page appears, prompting for your email address, type your email address, and then click **Continue**. Wait for the sign-in page to appear, type your email address and password, and then click **Sign In.**

   >**Note:** During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account that you previously used, and then continue to provide your credentials.

33. If the **Windows Azure Tour** dialog box appears, click close (the **X** button).
34. In the navigation blade, click **App Services**, click **Add**, click **Web App** and then click **Create**.
35. In the **App name** box, type a unique name.
36. Click **App service plan/Location**, click **Create new**.
37. From the **Location** drop-down list, select the region that is closest to your location.
38. In the **App Service plan** input, enter **MyAppService** and click **OK**.
39. Click **Create**. Wait for the web app to be created. Click the newly created web app.
40. In the newly created webapp blade, click **Continuous Delivery** in the **Deployment** section.
41. In the **Continuous Delivery** blade, click **Configure**.
42. Click **Source Code** and fill the following values:
    - Code repository: **Visual Studio Team Service**
    - Visual Studio Team Service Account: **_youraccount_**
    - Project: **MyApp**
    - Repository: **$/MyApp**
    - Branch: **MyTFSWebApp**
43. Click **OK**
44. Click **Build** and in the **Web application framework** dropdown, select **ASP.NET**, click **OK**.
45. Click **OK** and wait for the continuous delivery configuration to finish.
46. A new build will automatically start and should take about 2 minutes. 
47. After 2 minutes, click **Refresh logs**, you should see **Deployed successfully to Production**. If you don't wait another minute and refresh the logs again.
48. Open a browser tab and navigate to the web app at url: **http://[yourwebapp].azurewebsites.net/api/values** and you should see 2 values in a form of XML or JSON.
49. Return to Visual Studio 2017, and on the **View** menu, click **Solution Explorer**.
50. In Solution Explorer, expand the **Controllers** folder, right-click the **ValuesController.cs** file, and then select **Check Out for Edit**.
44. In the **Check Out** dialog box, click **Check Out**.
45. In Solution Explorer, double-click **ValuesController.cs**, locate the **Get** method that is without parameters, and replace the  **return** line of the method with the following code:

  ```cs
		return new string[] { "value1", "value2", "value3" };
```
46. To save the file, press Ctrl+S.
47. In Solution Explorer, right-click **ValuesController.cs**, and then click **Check In**.
48. In Team Explorer, click **Check In**.
49. If you are prompted by a **Check-in Confirmation** dialog box, click **Yes**. Wait for the check-in to complete.
50. Click the **Pending Changes | MyApp** title in Team Explorer, and then click **Builds** from the drop-down list.
51. Double-click the build definition that appears under the **My Builds** group.
52. A browser will open in the MSTS build page where you can see the progress of the build.
53. When the build is done, return to the Azure portal, click **Refresh logs** and review the last deployment.
54. Open a browser tab and navigate to the web app at url: **http://[yourwebapp].azurewebsites.net/api/values** and you should see 3 values in a form of XML or JSON.


# Lesson 6: Best Practices for Production Deployment

### Demonstration: Transforming Web.config Files

#### Demonstration Steps

1. Open **Visual Studio 2017**
2. On the **File** menu, point to **New**, and then click **Project**.
3. In the **New Project** dialog box, in the navigation pane, expand the **Installed** node, expand the **Visual C#** node, click the **Web** node, and then select **ASP.NET Web Application (.NET Framework)** from the list of templates.
4. In the **Name** box, type **WebConfigTransformations**.
5. In the **Location** box, type **[repository root]\Mod08\DemoFiles\WebConfigTransformations\begin**.
6. Clear the **Create directory for solution** check box, and then click **OK**.
7. In the **New ASP.NET Web Application** dialog box, select **Web API**, and then click **OK**.
8. Right click the **WebConfigTransformations** project and click **Manage NuGet Packages**.
9. In the **NuGet: WebConfigTransformations** tab, click **Browse**.
10. In the **Search** box, enter **EntityFramework**.
11. Click the **EntityFramework** search result and click **Install**.
12. In the **Preview Changes** modal, click **OK**.
13. In the **License Acceptance** modal, click **I Accept**.
8. In Solution Explorer, double-click **Web.config**.
9. Add a connnection string:
    ```xml
    <connectionStrings>
        <add name="DefaultConnection" connectionString="Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=MyAppDB;Integrated Security=SSPI;AttachDBFilename=|DataDirectory|\db.mdf">
    </connectionStrings>
    ```
10. Review the **compilation** configuration section under the **system.web** configuration group. Note the existence of the **debug** attribute.
11. In Solution Explorer, expand **Web.config**, and double-click **Web.Release.config**.
12. Review the **compilation** configuration section under the **system.web** configuration group. Observe the **RemoveAttribute**  transform attribute.
13. In the **configuration** root element, add the following **connectionStrings** section:

  ```cs
		<connectionStrings>
          <add name="DefaultConnection"
             connectionString="Data Source=ProductionSQLServer;Initial Catalog=MyAppProductionDB;Integrated Security=True"
          xdt:Transform="SetAttributes" xdt:Locator="Match(name)"/>
        </connectionStrings>
```
14. On the **File** menu, click **Save Web.Release.config**.
15. In Solution Explorer, right-click the **WebConfigTransformations** project, and then click **Publish**.
16. In the **Publish Web** dialog box, click the publish profile drop-down list, and then click **New**.
17. In the **New Profile** dialog box, type **Production**, and then click **OK**.
18. Verify that the selected publish method is **Web Deploy**, and set the rest of the fields to the following values:

    - Server: **localhost**
    - Site Name: **Default Web Site/MyProductionApp**
    - Destination URL: **http://localhost/MyProductionApp**

19. Click **Validate Connection** , and wait for the green check mark to appear.
20. Click **Next** , and select **Release** from the **Configuration** drop-down list. Click **Next**, and then click **Publish**.  
    Wait until the publishing completes and the browser opens.
21. Close the browser and return to Visual Studio 2017.
22. On the **File** menu, point to **Open**, and then click **Web Site**.
23. Click **Local IIS** in the navigation pane.
24. Expand the **IIS Sites** node in the websites tree, expand the **Default Web Site** node, select the **MyProductionApp** node, and then click **Open**.
25. In Solution Explorer, expand the **http://localhost/MyProductionApp** website, and double-click the **Web.config** file.
26. Review the connection string named **DefaultConnection** in the **&lt;connectionStrings&gt;** section. Note that it now uses the  **ProductionSQLServer** database.
27. Show students the **&lt;compilation&gt;** configuration section under the **&lt;system.web&gt;** configuration group. Point out the missing **debug** attribute.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

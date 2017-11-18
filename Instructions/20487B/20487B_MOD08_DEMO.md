# Module 8: Deploying Services

# Lesson 1: Web Deployment with Visual Studio 2012

### Demonstration: Deploying a Web Application by Using Visual Studio

#### Preparation Steps

For this demonstration, you will use the available virtual machine environment. Before you begin this demonstration, you must complete the following steps:
 1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2. In Hyper-V Manager, click **MSL-TMG1**, and in the Actions pane, click **Start**.
 3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Actions pane, click **Start**.
 4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

 6. Return to Hyper-V Manager, click **20487B-SEA-DEV-B**, and in the **Actions** pane, click **Start**.
 7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 8. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the **Start** screen, click the **Visual Studio 2012** tile.
2. On the **File** menu, point to **New**, and then click **Project**.
3. In the **New Project** dialog box, in the left pane, expand the **Installed** node, expand the **Templates** node, expand the **Visual C#** node, click the **Web** node, and then select **ASP.NET MVC 4 Web Application** from the list of templates.
4. In the **Name** box, type **MyApp**.
5. In the **Location** box, type **D:\Allfiles\Mod08\DemoFiles\DeployWebApp\begin**.
6. Clear the **Create directory for solution** check box, and then click **OK**.
7. In the **New ASP.NET MVC 4 Project** dialog box, select **Web API**, and then click **OK**.
8. In Solution Explorer, right-click the **MyApp** project, and then click **Publish**.
9. In the **Publish Web** dialog box, from the **Publish Profile** list, select **New Profile**.
10. In the **New Profile** dialog box, type **RemoteServer**, and then click **OK**.
11. Verify that the selected publish method is **Web Deploy**, and set the rest of the fields by using the following values:

    - Server: **http://10.10.0.11/msdeployagentservice**
    - Site Name: **Default Web Site/MyApp**
    - User name: **Administrator**
    - Password: **Pa$$w0rd**
    - Save password: **Checked**
    - Destination URL: **http://10.10.0.11/MyApp/api/values**
-
12. Click **Validate Connection**, and wait for the green check mark to appear.
13. Click **Next**, click **Next** again, and then click **Publish**.
14. Wait until the publishing completes and the browser opens. Click **Open** at the bottom of the browser, and then click **Notepad**. Verify that you see two values.

   >**Note** : If you get a message that says &quot;Windows can&#39;t open this type of file (.json)&quot;, click **Try an app on this PC** , and then click **Notepad**.

15. Close Notepad, return to Visual Studio 2012, and in Solution Explorer, under the **MyApp** project, expand the **Controllers**  folder, and then double-click **ValuesController.cs**.
16. Locate the parameterless **Get** method, and change its code as follows:

  ```cs
		return new string[] { "value1", "value2", "value3" };
```
17. To save changes that you made to the code file, press Ctrl+S.
18. In Solution Explorer, right-click the **MyApp** project, and then click **Publish**.
19. In the **Publish Web** dialog box, click **Publish**.
20. Wait until the publishing completes and the browser opens. Click **Open** at the bottom of the browser, and select **Notepad** from the list of available programs. Verify that you see three values.

# Lesson 2: Creating and Deploying Web Application Packages

### Demonstration: Exporting and Importing Web Deploy Packages by Using IIS Manager

#### Preparation Steps

For this demonstration, you will use the available virtual machine environment. Before you begin this demonstration, you must complete the following steps:
 1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2. In Hyper-V Manager, click **MSL-TMG1**, and in the Actions pane, click **Start**.
 3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Actions pane, click **Start**.
 4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

 6. Return to Hyper-V Manager, click **20487B-SEA-DEV-B**, and in the **Actions pane**, click **Start**.
 7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 8. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**
 
 9. Make sure that the MyApp web application is deployed to the 20487B-SEA-DEV-B server.


#### Demonstration Steps

1. On the **20487B-SEA-DEV-B** virtual machine, on the **Start** screen, click the **Internet Information Services (IIS)** tile.
2. In the **Connections** pane, expand the **SEA-DEV12-B (SEA-DEV12-B\Administrator)** node.
3. If you receive a message that asks whether you want to get started with Microsoft Web Platform, click **No**. 
4. While still in the **Connections** pane, expand the **Sites** node, then expand the **Default Web Site** node, and then click the  **MyApp** web application node.
5. In the **Actions** pane, click **Export Application**.
6. In the **Export Application Package**, dialog box, click **Next**, and then click **Next** again.
7. In the **Package path** box, type **C:\MyApp.zip**, and then click **Next**.
8. Wait for the export to complete, and then click **Finish**.
9. On the **Start** screen, click the **Computer** tile to open File Explorer.
10. Locate the root of drive **C**.
11. Find the file that is named **MyApp** (it should have a compressed file icon).
12. Right-click the file, and then click **Copy**.
13. Go to **\\10.10.0.10\c$**.
14. In the file list, right-click an empty area, and then click **Paste**.
15. On the **20487B-SEA-DEV-A** virtual machine, on the **Start** screen, click the **Internet Information Services (IIS)** tile.
16. In the **Connections** pane, expand the **SEA-DEV12-A (SEA-DEV12-A\Administrator)** node.
17. If you receive a message that asks whether you want to get started with Microsoft Web Platform, click **No**.
18. While still in the **Connections** pane, expand the **Sites** node, and then expand the **Default Web Site** node.
19. Click the **Default Web Site** node.
20. In the **Actions** pane, click **Import Application**.
21. In the **Package path** box of the **Import Application Package**, dialog box, type **C:\MyApp.zip**, and then click **Next**.
22. Click **Next** again, and verify that in the **Enter Application Package Information** step of the wizard, the **Application Path** box is set to **MyApp. Click Next**.
23. Wait for the import to complete, and then click **Finish**.
24. On the **Start** screen, click **Internet Explorer**.
25. In the address bar, type **http://localhost/MyApp/api/values**, and press **Enter**.
26. Wait until the response is returned, click **Open** at the bottom of the browser, and then click **Notepad**. Verify that you see three values.

    >**Note** : If you get a message that says, &quot;Windows can&#39;t open this type of file (.json)&quot;, click **Try an app on this PC** , and then click **Notepad**.



# Lesson 3: Command-Line Tools for Web Deploy

### Demonstration: Using Windows PowerShell Cmdlets

#### Preparation Steps

This demonstration uses the output of the demonstration in Lesson 2, **Creating and Deploying Web Application Packages**. Before you begin this demonstration, you must complete the following steps:
 1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2. In Hyper-V Manager, click **MSL-TMG1**, and in the Actions pane, click **Start**.
 3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Actions pane, click **Start**.
 4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

 6. Return to Hyper-V Manager, click **20487B-SEA-DEV-B**, and in the **Actions** pane, click **Start**.
 7. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 8. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the **20487B-SEA-DEV-A** virtual machine, on the desktop, click the **Windows PowerShell** icon on the taskbar.
2. In the Administrator: Windows PowerShell window, type the following command and press Enter.

  ```cs
		Add-PSSnapin WDeploySnapin3.0
```
3. To get the credentials for the destination server, while still in the Administrator: Windows PowerShell window, type the following command and press Enter.

  ```cs
		$cred = Get-Credential
```
4. In the **Windows PowerShell Credentials Request** dialog box, type the following credentials, and then click **OK**:
   - Username: **Administrator**
   - Password: Pa$$w0rd
5. To create a publish settings file, type the following command and press Enter.

  ```cs
		New-WDPublishSettings -ComputerName 10.10.0.11 –Credentials $cred -AgentType MSDepSvc -FileName:"C:\Server.publishsettings"
```
6. To synchronize the web application to the remote server, type the following command and press Enter.

  ```cs
		Sync-WDApp "Default Web Site/MyApp" "Default Web Site/MyAppDeployedWithPowerShell" -DestinationPublishSettings "C:\Server.publishsettings"
```
7. On the **Start** screen, click **Internet Explorer**.

8. In the address bar, type **http://10.10.0.11/MyAppDeployedWithPowerShell/api/values**, and then press **Enter**.
9. Wait until the response is returned, click **Open** at the bottom of the browser, and then click **Notepad**. Verify that you see three values.

   >**Note:** If you get a message that says, &quot;Windows can&#39;t open this type of file (.json)&quot;, click **Try an app on this PC** , and then click **Notepad**.

# Lesson 5: Continuous Delivery with TFS and Git

### Demonstration: Continuous Delivery with Visual Studio Team Services

#### Preparation Steps

For this demonstration, you will use the available virtual machine environment. Before you begin this demonstration, you must complete the following steps:
 1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2. In Hyper-V Manager, click **MSL-TMG1**, and in the Actions pane, click **Start**.
 3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Actions pane, click **Start**.
 4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**
  
To present this demonstration, you must have a Microsoft Account. If you have not created a Microsoft Account before, create one before you start the demonstration.

#### Demonstration Steps

1. On the **Start** screen, click the **Internet Explorer** tile.
2. Go to **https://go.microsoft.com/fwlink/?LinkId=307137**.
3. On the **Sign in** page, type your **Microsoft account** email address and password, and then click **Sign in**.
4. In the account text box, type a unique account name, select **Team Foundation Version Control**, and then click **Continue**.
5. If a page opens requiring you to enter personal information, enter your personal information as required, and then click  **Continue**.
6. If the **Congratulations** dialog box appears, close it.

   >**Note:** The Visual Studio Team Services website will stop supporting Internet Explorer 9 from September 2016. If you see a degraded or broken experience, install a supported browser, which can be the most recent version of Mozilla Firefox or Google Chrome, on the virtual machine.

7. On the top-most toolbar, click **Team Services.**
8. In the **Recent project &amp; teams** section, click **New**.
9. In the **Project name** box, type **20487B**.
10. In the **Version control** drop-down list, select **Team Foundation Version Control**, and then click **Create project**.
11. Wait for the project to be created, and then click **Navigate to project**.
12. If the **Congratulations** dialog box appears, close it. On the **Start** screen, click the **Visual Studio 2012** tile.
13. On the **Start** screen, click the **Visual Studio 2012** tile.
14. On the **View** menu, click **Team Explorer**.
15. In the Team Explorer window, click **Connect**, and then click the **Connect** link under **Team Foundation Service**.
16. In the **Connect to Team Foundation Server** dialog box, click **Servers**, and then click **Add**.
17. In the **Add Team Foundation Server** dialog box, in the **Name or URL of Team Foundation Server** box, type the URL of your account (in the following format: https://_youraccount_.visualstudio.com), and then click **OK**. In the **Add/Remove Team Foundation Server** dialog box, click **Close**.
18. In the **Connect to Team Foundation Server** dialog box, select the **20487B** project, and then click **Connect**.
19. On the **File** menu, point to **New** , and then click **Project**.
20. In the **New Project** dialog box, in the left pane, expand the **Installed** node, expand the **Templates** node, expand the  **Visual C#** node, click the **Web** node, and then select **ASP.NET MVC 4 Web Application** from the list of templates.

21. In the **Name** box, type **MyTFSWebApp**.
22. In the **Location** box, type **D:\Allfiles\Mod08\DemoFiles\TfsContinuousDelivery\begin**.
23. Clear the **Create directory for solution** check box, select the **Add to source control** check box, and then click **OK**.
24. In the **New ASP.NET MVC 4 Project** dialog box, select **Web API**, and then click **OK**.
25. In the **Add Solution MyTFSWebApp to Source Control** window, click **OK**.In Solution Explorer, right-click the **Solution &#39;MyTFSWebApp&#39;** node, and then click **Check In**.
26. In Solution Explorer, right-click the **Solution &#39;MyTFSWebApp&#39;** node, and then click **Check In**.
27. In Team Explorer, click **Check In**.
28. If you are prompted by a **Check-in Confirmation** dialog box, click **Yes**. Wait for the check in to complete.
29. Return to the browser window and go to **https://manage.windowsazure.com**.
30. If a page appears, prompting for your email address, type your email address, and then click **Continue**. Wait for the sign-in page to appear, type your email address and password, and then click **Sign In.**

   >**Note:** During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account that you previously used, and then continue to provide your credentials.

31. If the **Windows Azure Tour** dialog box appears, click close (the **X** button).
32. In the navigation pane, click **WEB APPS**, click **NEW**, and then click **QUICK CREATE**.
33. In the **URL** box, type a unique name.
34. From the **APP SERVICE PLAN** drop-down list, select **Create new App Service plan**.
35. From the **REGION** drop-down list, select the region that is closest to your location.
36. Click **CREATE WEB APP**. Wait for the web app to be created. Click the newly created web app, and then click the **Set up deployment from source control** link in the **Integrate source control** section.
37. Click the newly created web app, and then click the **Set up deployment from source control** link in the **Integrate source control** section.
38. In the **SET UP DEPLOYMENT** dialog box, click **Visual Studio Online,** and then click **Next**.
39. In the **Authorize connection** dialog box, enter your Visual Studio Team Services account name in the box, and then click  **Authorize Now**.
40. In the browser window that opened, click **Accept**, and then wait for the window to close.
41. In the **SET UP DEPLOYMENT** dialog box, open the **REPOSITORY NAME** drop-down list, select **20487B**, and then click  **Complete**.
42. Return to Visual Studio 2012, and on the **View** menu, click **Solution Explorer**.
43. In Solution Explorer, expand the **Controllers** folder, right-click the **ValuesController.cs** file, and then select **Check Out for Edit**.
44. In the **Check Out** dialog box, click **Check Out**.
45. In Solution Explorer, double-click **ValuesController.cs**, locate the **Get** method that is without parameters, and replace the  **return** line of the method with the following code:

  ```cs
		return new string[] { "value1", "value2", "value3" };
```
46. To save the file, press Ctrl+S.
47. In Solution Explorer, right-click **ValuesController.cs**, and then click **Check In**.
48. In Team Explorer, click **Check In**.
49. If you are prompted by a **Check-in Confirmation** dialog box, click **Yes**. Wait for the check-in to complete.
50. Click the **Pending Changes | 20487B** title in Team Explorer, and then click **Builds** from the drop-down list.
51. Double-click the build definition that appears under the **All Build Definitions** group.
52. In Build Explorer, click the **Queued** tab.
53. Review the list of builds. If the last build is still running, click **Refresh** on the **Build Explorer** bar every 10 seconds until the build is marked as succeeded.
54. In Build Explorer, click the **Completed** tab, and verify that the completed build appears in the list.
55. Return to the browser, and review the list of deployments on the **DEPLOYMENTS** tab.
56. Click **BROWSE** on the portal&#39;s taskbar. After the web application appears in the newly opened window, append the  **api/values** string to the end of the URL, and then press **Enter**.
57. Click **Open** in the **open or save** dialog box that appears, and then click **Notepad**. If you see a message that says, &quot;Windows can&#39;t open this type of file (.json)&quot;, click **Try an app on this PC**, and then click **Notepad**.
58. In Notepad, verify that you see a JSON array that contains **three** values.
59. In Visual Studio 2012, on the **File** menu, point to **New**, and then click **Project**.
60. In the **New Project** dialog box, in the left pane, expand the **Installed** node, expand the **Templates** node, expand the  **Visual C#** node, click the **Web** node, and then select **ASP.NET MVC 4 Web Application** from the list of templates.
61. Clear the **Add to source control** check box, and then click **OK**.
62. In the **New ASP.NET MVC 4 Project** dialog box, click **Cancel**.
63. In the **New Project** dialog box, click **Cancel**.

   >**Note:** If you do not clear the **Add to source control** check box, you will be prompted to register with a source control each time you add a new project.


# Lesson 6: Best Practices for Production Deployment

### Demonstration: Transforming Web.config Files

#### Preparation Steps

For this demonstration, you will use the available virtual machine environment. Before you begin this demonstration, you must complete the following steps:
 1. On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2. In Hyper-V Manager, click **MSL-TMG1**, and in the Actions pane, click **Start**.
 3. In Hyper-V Manager, click **20487B-SEA-DEV-A**, and in the Actions pane, click **Start**.
 4. In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5. Sign in by using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the **Start** screen, click the **Visual Studio 2012** tile.
2. On the **File** menu, point to **New**, and then click **Project**.
3. In the **New Project** dialog box, in the navigation pane, expand the **Installed** node, expand the **Templates** node, expand the **Visual C#** node, click the **Web** node, and then select **ASP.NET MVC 4 Web Application** from the list of templates.
4. In the **Name** box, type **WebConfigTransformations**.
5. In the **Location** box, type **D:\Allfiles\Mod08\DemoFiles\WebConfigTransformations\begin**.
6. Clear the **Create directory for solution** check box, and then click **OK**.
7. In the **New ASP.NET MVC 4 Project** dialog box, select **Web API**, and then click **OK**.
8. In Solution Explorer, double-click **Web.config**.
9. Review the connection string named **DefaultConnection** in the **connectionStrings** section. The connection string uses the  **LocalDb** database.
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
    - Site Name: **Default Web Site/ MyProductionApp**
    - Destination URL: **http://localhost/MyProductionApp**

19. Click **Validate Connection** , and wait for the green check mark to appear.
20. Click **Next** , and select **Release** from the **Configuration** drop-down list. Click **Next**, and then click **Publish**.  
    Wait until the publishing completes and the browser opens.

21. Close the browser and return to Visual Studio 2012.
22. On the **File** menu, point to **Open**, and then click **Web Site**.
23. Click **Local IIS** in the navigation pane.
24. Expand the **IIS Sites** node in the websites tree, expand the **Default Web Site** node, select the **MyProductionApp** node, and then click **Open**.
25. In Solution Explorer, expand the **http://localhost/MyProductionApp** website, and double-click the **Web.config** file.
26. Review the connection string named **DefaultConnection** in the **&lt;connectionStrings&gt;** section. Note that it now uses the  **ProductionSQLServer** database.
27. Show students the **&lt;compilation&gt;** configuration section under the **&lt;system.web&gt;** configuration group. Point out the missing **debug** attribute.

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

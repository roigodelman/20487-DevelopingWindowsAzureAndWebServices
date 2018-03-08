# Module 1: Overview of Service and Cloud Technologies

 Wherever you see a path to file starting at [repository root], replace it with the absolute path to the directory in which the 20487 repository resides. 
 e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487, then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

# Lesson 4: Cloud Computing

### Demonstration: Exploring the Microsoft Azure portal

#### Preparation Steps

  >**Note**: This demonstration requires an Azure account. Make sure you have a valid account before starting the demonstration.

#### Demonstration Steps

1. Open Microsoft Edge.
2. Navigate to **https://portal.azure.com**.
3. If a page appears prompting for your email address, enter your email address, and then click **Next** and enter your password, and then click **Sign In**.
4. If the **Stay signed in?** dialog appears, click **Yes**.

   >**Note**: During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account that you previously used, and then continue to provide your credentials.

5. Review the items in the pane on the left side of the screen and understand the different services that you can manage with the Azure portal.
6. Click **+ Create a resource** on the left side of the portal, and then click **Web App**. The **App Name** and **Resource Group** text boxes and **App Service Plan/Location** appear on the right side of the screen.
7. In the **App Name** text box, type the following web app name: **WebAppDemo**_YourInitials_ (Replace _YourInitials_ with your initials).  

   The app name you typed is going to be part of the URL that you will use when connecting to the web application.
   
8. In the **Resource Group** text box, select **Create new**.
9. Click **App Service Plan/Location**,  and then click **Create new**.
10. In **App Service Plan**, enter **BlueYonder**.
11. In **Location**, select the location that is closest to you.
12. Click **Ok.**
13. Click **Create**, and then wait until the web app is deployed.
14. In the **All Resources** pane, click the web app that you created in the previous step (the one that is named **WebAppDemo** _YourInitials_).  

    Currently the web app has no content. In module 06, &quot;Hosting Services&quot;, you will see how to publish web apps using Visual Studio 2017.
  
15. Go over the different sections and understand their purpose:  
  a. **General** (no section). Provides an overview about the activity of the web app, access control and metadata.  
  b. **Deployment**. Controls how and when the application is deployed.  
  c. **Settings**. Allows you to control settings such as monitoring capabilities, remote access, security tokens, scaling and more.
16. Close the browser.

# Lesson 5: Exploring the Blue Yonder Airlines&#39; Travel Companion Application

### Demonstration: Using the Travel Companion Application   

#### Demonstration setup

Expected Setup time: Up to 10 Minutes.

You will need to have both the **Azure Storage Emulator** the server applications up and running before you start this demonstration.
Follow these steps to set up the demo:

1. Verify that you have Azure Storage Emulator installed:
    - Open the **Start** menu.
    - Search for **Microsoft Azure Storage Emulator**
    - If you have the emulator installed, skip to step 3.
2. Download the Microsoft Azure Emulator:
    - Open a browser and navigate to the following address: **https://go.microsoft.com/fwlink/?linkid=717179&clcid=0x409**
    - When the download is finished, open the file.
    - Check **I accept the terms in the License Agreement**
    - Click **Install**.
3. Open the **Start** menu.
4. In the **Start** menu, search for and open **Microsoft Azure Storage Emulator**.
5. Open **Visual Studio 2017.**
6. On the **File** menu, point to **Open**, and then click **Project/Solution**.
7. Go to **[repository root]\AllFiles\20487C\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server**, select the **BlueYonder.Server.sln** file, and then click **Open**.
8. In the **Solution Explorer**, right click the **BlueYonder.Server.Booking.WebHost** project.
9. Point to **Debug** and then click **Start new instance**.
10. Open a new instance of **Visual Studio 2017**.
11. On the **File** menu, point to **Open**, and then click **Project/Solution**.
12. Go to **[repository root]\AllFiles\20487C\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Server**, select the **BlueYonder.Companion.sln** file, and then click **Open**.
13. In the **Solution Explorer**, right click the **BlueYonder.Companion.Host** project.
14. Point to **Debug** and then click **Start new instance**.

#### Demonstration Steps


1. Open **Visual Studio 2017.**
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Go to **[repository root]\AllFiles\20487C\Mod01\DemoFiles\BlueYonderDemo\BlueYonder.Companion.Client**, select the **BlueYonder.Companion.Client.sln** file, and then click **Open**.
4. If the **Settings** window appears, select **Developer mode**.
5. If the **Use developer features** dialog box appears, click **Yes**.
6. In Solution Explorer, right-click the **BlueYonder.Companion.Client** project, and then click **Set as StartUp Project**.
7. To start the client app without debugging, press **Ctrl+F5**.
8. If you are prompted to allow the app to run in the background, click **Allow**.  
 
    Understand the purpose and features of the Blue Yonder Companion app; it is a travel reservation and management app. It can help you search and book flights, manage your trip schedule, store and manage pictures and videos from trips, and provide weather information for your trip destinations.
 
9. After the client app starts, right-click or swipe from the bottom of the screen to open the app bar.
10. Click **Search**, and then in the **Search** text box, type **New**. 
    >**Note**: The app communicates with the front-end service to retrieve a list of flights to a location whose name begins with _New_; for example, New York.  
11. If you are prompted to allow the app to share your location, click **Yes**. 
12. Select any destination from the list of search results, and then click the **Purchase this trip** link.
13. In the **First Name** text box, enter your first name.
14. In the **Last Name** text box, enter your last name.
15. In the **Passport** text box, type **Aa1234567**.
16. In the **Mobile Phone** text box, type **555-5555555**.
17. In the **Home Address** text box, type **423 Main St.**
18. In the **Email Address** text box, enter your email address.
19. Click **Purchase**.

    Now the app sends the purchase request to the front-end service. The front-end service saves the purchase information, and then sends a separate purchase request to the back-end service for additional processing. After the back-end and front-end services complete their task, the client app displays a confirmation message.
 
    You will implement the purchase feature, including the back-end service purchase feature, in the upcoming labs.  
    
20. To close the confirmation message click **Close**.
21. On the **Blue Yonder Companion** page, point to **New York at a Glance**, and notice that the weather forecast is also retrieved from the front-end service. You will implement the weather service in the upcoming labs.
22. Click the current trip from Seattle to New York.
23. On the **Current Trip** page, right-click or swipe from the bottom of the screen to display the app bar, and then click **Media**.
24. On the **Media** page, right-click or swipe from the bottom of the screen to display the app bar.
25. Review the available buttons. You can upload items to Azure Public or Private Storage, and share them with other clients.

    You will implement the upload and download features in the upcoming labs.

    >**Note**: Do not click the upload buttons, because you have not created any Azure Storage accounts yet. If you click any of the upload buttons, the app will fail and close.

26. Close all open windows.

©2018 Microsoft Corporation. All rights reserved.

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

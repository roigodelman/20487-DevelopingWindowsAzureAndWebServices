# Module 7: Windows Azure Service Bus

# Lesson 1: Windows Azure Service Bus Relays

### Demonstration: Creating Service Bus Relays

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. In the **File name** text box, type **D:\AllFiles\Mod07\DemoFiles\ServiceBusRelay\begin\ServiceBusRelay\ServiceBusRelay.sln**, and then click **Open**.
4. In **Solution Explorer**, expand the **ServiceBusRelay.Server** project, and then double-click **Program.cs**.
5. Explain that the service endpoint is configured to receive TCP messages directly on port 747.
6. In **Solution Explorer**, expand the **ServiceBusRelay.WebClient** project, expand **Controllers**, and then double-click **HomeController.cs**.
7. Locate the **Write** method, and explain that the Web client consumes the service by using the address and port of the server.
8. In **Solution Explorer**, right-click the root node of the solution, and then click **Properties**.
9. In the **Solution &#39;ServiceBusRelay&#39; Property Pages** dialog box, select the **Multiple startup projects** option.
10. In the grid view, for the **ServiceBusRelay.Server** and **ServiceBusRelay.WebClient** projects, change the action to **Start**.
11. Click **OK**.
12. To start the solution without debugging, press Ctrl+F5. Wait until the console application displays the message **The server is running**.
13. Position the browser and console windows next to each other so that you can show both windows.
14. In the browser window, in the **Text** text box, type **Hello** and click **Write to console**. The **Hello** text will appear in the console window.
15. Close the browser and the console window.
16. On the Start menu, click the **Microsoft Edge** tile.
17. Go to the Microsoft Azure portal at **http://portal.azure.com**.
18. If a page appears, asking for your email address and password, type your email address and password, and then click **Sign in**.

    >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

19. If the **Windows Azure Tour** dialog box appears, close it.
20. In the left navigation pane, click **SERVICE BUS**.
21. At the bottom of the page, click **CREATE**.
22. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    a. Name: **ServiceBusDemo07**** YourInitials** (Replace _YourInitials_ with your initials).      
    b. Pricing Tier: **Standard**. \
    c. Subscription: either **Free Trial** or any available subscription
       
       >**Note:** If you don't have any available subscriptions, create one before proceeding.
       
    d. Resource Group: Select **Create New** and type in **ServiceBusDemo07**** YourInitials** (Replace _YourInitials_ with your initials). \
    e. Region: Select the region closest to your location.

23. To create the namespace,at the bottom of the dialog box, click the **Create** button, and then wait until the namespace is active.
24. In the **Dashboard**, under the **All Resources** tile, you should see your newly created service bus. Click it.
    >**Note:** If you cannot see the service bus, you can click **See more...** to access the full list of resources, alternatively, you can expand the tile by dragging its bottom-right corner. 
25. In the service bus box, click the **Shared access policies** tab on the left-hand menu, then click **RootManageSharedAccessKey**.
26. In the box that opened to the right, Click the **Copy** icon to the right-side of the **Primary Key** text box.
27. If you are prompted to allow access to your clipboard, click **Allow access**.
28. Close the dialog box, and then return to Visual Studio 2017.
29. In **Solution Explorer**, right-click the **ServiceBusRelay.Server** project, and then click **Manage NuGet Packages**.
30. In the **Manage NuGet Packages** dialog box, on the navigation pane, expand the **Online** node, and then click the **NuGet official package source** node.
31. Press Ctrl+E and type **WindowsAzure.ServiceBus**.
32. In the center pane, click the **Microsoft Azure Service Bus** package, and then click **Install**.
33. If a **License Acceptance** dialog box appears, click **I Accept**.
34. Wait for installation to complete, and then click **Close** to close the dialog box.
35. In **Solution Explorer**, right-click the **ServiceBusRelay.WebClient** project, and then click **Manage NuGet Packages**.
36. To install the **WindowsAzure.ServiceBus** NuGet package in the **ServiceBusRelay.WebClient** project, repeat steps 30-34.
37. In **Solution Explorer**, expand the **ServiceBusRelay.Server** project, and then double-click **Program.cs**.
38. To the beginning of the file, add the following **using** directive.

  ```cs
        using Microsoft.ServiceBus;
```
39. Locate the call to the **ServiceHost** constructor and change the host&#39;s address from **net.tcp://127.0.0.1:747/** to **sb://ServiceBusDemo07** YourInitials **.servicebus.windows.net** (Replace _YourInitials_ with your initials).
40. Locate the call to the **host.AddServiceEndpoint** method and update the binding to **NetTcpRelayBinding** by using the following code.

  ```cs
        var endpoint = host.AddServiceEndpoint(typeof(IConsoleService), new NetTcpRelayBinding(), "console");
```
41. Add a new **TransportClientEndpointBehavior** to the endpoint by adding the following code before calling the **host.Open** method.

  ```cs
        endpoint.EndpointBehaviors.Add(new TransportClientEndpointBehavior
        {
             TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "[access_key]")
        });
```
42. Select the _[access_key]_ placeholder in the code you added, and press Ctrl+V to paste the key you have copied from the Azure portal over it.
43. To save the changes, press Ctrl+S.
44. In **Solution Explorer** , expand the **ServiceBusRelay.WebClient** project, then expand **Controllers** , and then double-click **HomeController.cs**.
45. To the beginning of the file, add the following **using** directive.

  ```cs
        using Microsoft.ServiceBus;
```
46. In the **Write** method, locate the call to the **ChannelFactory** constructor.
47. In the constructor call, change the binding from **NetTcpBinding** to **NetTcpRelayBinding**.
48. In the constructor call, change the endpoint address from **net.tcp://127.0.0.1:747/console** to **sb://ServiceBusDemo07** YourInitials **.servicebus.windows.net/console** (Replace _YourInitials_ with your initials).
49. Before calling the **factory.CreateChannel** method, add the following code.

  ```cs
        factory.Endpoint.EndpointBehaviors.Add(new TransportClientEndpointBehavior { TokenProvider =         TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "[access_key]")});
```
50. Select the _[access_key]_ placeholder in the code you added, and press Ctrl+V to paste the key you have copied from the Windows Azure portal over it.
51. Press Ctrl+S to save the changes.
52. Go back to the Azure portal.
53. On the lower-left side of the portal, click **NEW**.
54. Click **COMPUTE**.
55. Click **WEB APP**.
56. Click **QUICK CREATE**.
57. In the **URL** text box type **ServiceBusDemo07YourInitials** (Replace _YourInitials_ with your initials).
58. In the **APP SERVICE PLAN** drop-down list, select **Create new App Service plan.**
59. In the **REGION** drop-down list, select the region closest to your location.
60. Click the **Create Web App** icon. The site is added to the **Web Apps** table and its status displays **Creating**.
61. Wait until the status changes to **Running**.
62. In the **web apps** table, click the name of the new web app.
63. If the **DASHBOARD** tab is not shown, click the **DASHBOARD** tab.
64. On the right side, under **quick glance** links, click **Download the publish profile**.
65. Save the file to **D:\Allfiles\Mod07\DemoFiles\ServiceBusRelay**.
66. Return to Visual Studio 2012.
67. In **Solution Explorer**, right-click the **ServiceBusRelay.WebClient** project, and then click **Publish**.
68. In the **Publish Web** dialog box, click **Import** , and then go to **D:\Allfiles\Mod07\DemoFiles\ServiceBusRelay**. Select the profile file that you downloaded previously, and then click **Open**.
69. In the **Import Publish Profile** dialog box, click **OK**.
70. Click **Publish**. Visual Studio 2012 publishes the web application according to the settings that are provided in the profile file. After deployment is complete, a browser will open showing the uploaded web application.
71. Return to Visual Studio 2012.
72. In **Solution Explorer** , right-click the **ServiceBusRelay.Server** project, and then click **Set as StartUp Project**.
73. To start the application without debugging, Press Ctrl+F5. Wait until the console application displays the message **The server is running**.
74. Position the browser and console windows next to each other so that you can show both windows.
75. In the browser window, in the **Text** text box, type **Hello** , and then click **Write to console**. The text **Hello** will appear in the console window.

    >**Note:** You can also ask students to open a browser window on their computers, go to the address of the web app, type messages, and send them to the console window. Show the students the console window displaying their messages.

76. Close the browser window, and then close the console window.

# Lesson 2: Windows Azure Service Bus Queues

### Demonstration: Sending Messages by Using Azure Service Bus Queues

#### Preparation Steps

For this demonstration, you will use the available virtual machine environment. Before you begin this demo, you must complete the following steps:
 1.	On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2.	In Hyper-V Manager, click **MSL-TMG1**, and then in the Actions pane, click **Start**.
 3.	In Hyper-V Manager, click **20487B-SEA-DEV-A**, and then in the Actions pane, click **Start**.
 4.	In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5.	Sign in using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2012** tile.
2. On the **File** menu, point to **Open** , and then click **Project\Solution**.
3. Type **D:\Allfiles\Mod07\DemoFiles\QueuesDemo\Begin\QueuesDemo.sln** in the **File name** text box, and then click **Open**.
4. In **Solution Explorer** , right-click the root node of the solution, point to **Add** , and then click **New Project**.
5. In the **Add New Project** dialog box, on the navigation pane, expand the **Installed** node, expand the **Visual C#** node, click the **Windows** node, and then select **Console Application** from the list of templates.
6. In the **Name** text box, type **ServiceBusMessageSender**.
7. In the **Location** text box, type **D:\Allfiles\Mod07\DemoFiles\QueuesDemo\Begin**
8. Click **OK**.
9. In **Solution Explorer**, right-click the **ServiceBusMessageSender** project, and then click **Manage NuGet Packages**.
10. In the **Manage NuGet Packages** dialog box, on the navigation pane, expand the **Online** node, and then click the **NuGet official package source** node.
11. Press Ctrl+E, and then type **WindowsAzure.ServiceBus**.
12. In the center pane, click the **Microsoft Azure Service Bus** package, and then click **Install**.
13. If a **License Acceptance** dialog box appears, click **I Accept**.
14. Wait for installation to complete, and then click **Close** to close the window.
15. In **Solution Explorer**, right-click the **ServiceBusMessageSender** project, and then click **Add Reference**.
16. In the **Reference Manager** dialog box, expand the **Assemblies** node in the pane on the left side, and then click **Framework**.
17. Scroll down the assemblies list, point to the **System.Configuration** assembly, select the check box next to the assembly name, and then click **OK.**
18. On the Start screen, click the **Internet Explorer** tile.
19. Go to the Azure portal at **http://manage.windowsazure.com**.
20. If a page appears, asking for your email address, type your email address, and then click **Continue**. Wait for the **Sign In** page to appear, type your email address and password, and then click **Sign In.**

    >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

21. If the **Windows Azure Tour** dialog box appears, close it.
22. In the left navigation pane, click **SERVICE BUS**.
23. If you already created an Azure Service Bus namespace in the previous demo in this module, skip to step 27
24. At the bottom of the page, click **CREATE**.
25. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    - NAMESPACE NAME: **ServiceBusDemo07YourInitials** (Replace _YourInitials_ with your initials).
    - TYPE: **MESSAGING**
    - MESSAGING TIER: **STANDARD**
    - REGION: Select the region closest to your location.

26. To create the namespace,at the bottom of the dialog box, click the **V** icon, and wait until the namespace is active.
27. Click the name of the new/existing namespace.
28. At the bottom of the page, click **CONNECTION INFORMATION**.
29. Place the mouse cursor over the connection string text that starts with **Endpoint=**
30. Click the **Copy** icon to the right side of the text.
31. If you are prompted to allow access to your clipboard, click **Allow access**.
32. Return to Visual Studio 2012.
33. In **Solution Explorer**, under the **ServiceBusMessageSender** project, double-click **App.config**.
34. In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
35. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
36. To save the changes, press Ctrl+S.
37. In **Solution Explorer**, under the **ServiceBusMessageReceiver** project, double-click **App.config**.
38. In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
39. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
40. To save the changes, press Ctrl+S.
41. In **Solution Explorer**, under the **ServiceBusMessageSender** project, double-click **Program.cs**.
42. At the beginning of the file, add the following **using** directives.

  ```cs
        using Microsoft.ServiceBus;
        using Microsoft.ServiceBus.Messaging;
        using System.Configuration;
```
43. To create a namespace manager for the Service Bus queue, append the following code to the **Main** method.

  ```cs
        string connectionString = CloudConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
```
44. To verify that the queue exists, append the following code to the **Main** method.

  ```cs
        if (!namespaceManager.QueueExists("servicebusqueue"))
          {   
             namespaceManager.CreateQueue("servicebusqueue");
          }
```
45. Create a queue client by appending the following code to the **Main** method.

  ```cs
        QueueClient queueClient = QueueClient.Create("servicebusqueue");
```
46. To continuously get input from the user, and then to send it to the queue, add the following code to the **Main** method.

  ```cs
        Console.WriteLine("Enter text to send to the queue and press Enter.");
        string message = string.Empty;
        while ((message = Console.ReadLine()) != string.Empty)
        {
            // Create and Send Message to Queue
            var brokeredMessage = new BrokeredMessage(message);
            queueClient.Send(brokeredMessage);
        }
```
47. To save the changes, press Ctrl+S.
48. In **Solution Explorer** , right-click the **ServiceBusMessageSender** project, and then click **Set as StartUp Project**.
49. To run the sender console application, press Ctrl+F5.
50. Wait for the console prompt **Enter text to send to the queue and press Enter**.
51. Type **Hello**, and then press Enter.
51. Type **Goodbye**, and then press Enter.
53. Press Enter, and then press any key to close the console window.
54. In Visual Studio 2012, in **Solution Explorer**, under the **ServiceBusMessageReceiver** project, double-click **Program.cs**.
55. Show that the code in the **Main** method also connects to the Service Bus queue, and after connecting to the queue, it pulls messages from the queue and prints them.
56. Show how you use the **Receive** and **Complete** methods to implement Peek-Lock.
57. In **Solution Explorer**, right-click the **ServiceBusMessageReceiver** project, and then click **Set as StartUp Project**.
58. To run the receiver console application, press Ctrl+F5.
59. Check that the console window titled **Receiver** prints the text **Hello** and then **Goodbye**.
60. Close the console window.

# Lesson 3: Windows Azure Service Bus Topics

### Demonstration: Subscription-Based Messaging with Azure Service Bus Topics

#### Preparation Steps

For this demo, you will use the available virtual machine environment. Before you begin this demo, you must complete the following steps:
 1.	On the host computer, click **Start**, point to **Administrative Tools**, and then click **Hyper-V Manager**.
 2.	In Hyper-V Manager, click **MSL-TMG1**, and then in the Actions pane, click **Start**.
 3.	In Hyper-V Manager, click **20487B-SEA-DEV-A**, and then in the Actions pane, click **Start**.
 4.	In the **Actions** pane, click **Connect**. Wait until the virtual machine starts.
 5.	Sign in using the following credentials:
    - User name: **Administrator**
    - Password: **Pa$$w0rd**

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2012** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. In the **File name** text box, type **D:\Allfiles\Mod07\DemoFiles\TopicsDemo\TopicsDemo.sln**, and then click **Open**.
4. Explain to the students that the solution contains four console application projects. The **ServiceBusTopicPublisher** project creates topics, defines the available subscriptions and sends four sales messages.
5. The three remaining projects act as subscribers and print messages, sent to their subscription, to the console.

   - **ExpensivePurchasesSubscriber**. Receives sales that are over $4000.
   - **CheapPurchasesSubscriber**. Receives sales that are under $4000.
   - **AuditSubscriber**. Receives all sales.

6. In **Solution Explorer**, expand the **ServiceBusTopicPublisher** project, and then double-click **Program.cs**.
7. Locate the **Main** method. Explain to the students the use of the **NamespaceManager** class for checking if the topic exists, and then creating the topic if it does not exist.
8. Examine the code that creates the subscriptions. Show the use of the **NamespaceManager.CreateSubscription** method and the **SqlFilter** class.
9. Show the students how the four **BrokeredMessage** objects are created, and how their properties are set accordingly.
10. Demonstrate how the **TopicClient** object is created, and how it is used to send messages to the Service Bus.
11. In **Solution Explorer** , expand the **ExpensivePurchasesSubscriber** project, and then double-click **Program.cs**.
12. Locate the **Main** method, and show the students the use of the **SubscriptionClient**, class. The code connects to the **productsalestopic** topic with the **ExpensivePurchases** subscription.
13. Explain to the students that the code continuously receives messages from the subscription and prints them to the console.
14. Show the students the **CheapPurchasesSubscriber** and **AuditSubscriber** projects, and explain that the difference between these projects is just the name of the subscription that is being passed to the **Create** method in **SubscriptionClient**.
15. On the Start screen, click the **Internet Explorer** tile.
16. Go to the Microsoft Azure portal at **http://manage.windowsazure.com**.
17. If a page appears, asking for your email address, type your email address, and then click **Continue**. Wait for the **Sign In** page to appear, type your email address and password, and then click **Sign In.**

    >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

18. If the **Windows Azure Tour** dialog box appears, close it.
19. In the left navigation pane, click **SERVICE BUS**.
20. If you already created an Azure Service Bus namespace in the previous demo in this module, skip to step 24.
21. At the bottom of the page, click **CREATE**.
22. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    - NAMESPACE NAME: **ServiceBusDemo07YourInitials** (Replace _YourInitials_ with your initials).
    - TYPE: **MESSAGING**
    - MESSAGING TIER: **STANDARD**
    - REGION: Select the region closest to your location.

23. To create the namespace,at the bottom of the dialog box, click the **V** icon, and wait until the namespace is active.
24. Click the name of the new/existing namespace.
25. At the bottom of the page, click **CONNECTION INFORMATION**.
26. Point the cursor to the connection string text that starts with **Endpoint=**
27. Click the **Copy** icon to the right side of the text.
28. If you are prompted to allow access to your clipboard, click **Allow access.**
29. Return to Visual Studio 2012.
30. In Solution Explorer, under the **ServiceBusTopicPublisher** project, double-click **App.config**.
31. In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
32. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
33. To save the changes, press Ctrl+S.
34. Open the **App.config** file in the **ExpensivePurchasesSubscriber**, **CheapPurchasesSubscriber**, and **AuditSubscriber** projects, and then perform steps 30-32 in each configuration file.
35. In **Solution Explorer**, right-click the **ServiceBusTopicPublisher** project, and then click **Set as StartUp Project**.
36. To start the project without debugging, press Ctrl+F5.
37. Wait until the console prompt shows the message **Press any key to start publishing messages** , and then press Enter.
38. Wait until the console prompt shows the message **Sending complete** , and then press Enter to close the console window.
39. Return to Visual Studio 2012.
40. In **Solution Explorer**, right-click the root node of the solution, and then click **Properties**.
41. In the **Solution &#39;TopicsDemo&#39; Property Pages** dialog box, click **Multiple startup projects**.
42. In the grid view, change the action to **Start** for the following projects:

    - **AuditSubscriber**
    - **CheapPurchasesSubscriber**
    - **ExpensivePurchasesSubscriber**

43. Click **OK**.
44. To run the projects, press Ctrl+F5.
45. Wait for the three console windows to open. Show the students that messages are received in the console windows of the subscribers according to the subscription filter defined in the **ServiceBusTopicPublisher** project:

    - The **Audit Subscriber** console window prints all four messages to the console.
    - The **Cheap Purchases Subscriber** console window prints two messages describing two purchases with a price that is less than $4000.
    - The **Expensive Purchases Subscriber** console window prints two messages describing two purchases with a price that is more than $4000.

46. Close the three console windows.

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

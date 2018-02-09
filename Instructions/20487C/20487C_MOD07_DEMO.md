# Module 7: Microsoft Azure Service Bus

# Lesson 1: Microsoft Azure Service Bus Relays

### Demonstration: Creating Service Bus Relays

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. In the **File name** text box, type **D:\AllFiles\Mod07\DemoFiles\ServiceBusRelay\begin\ServiceBusRelay\ServiceBusRelay.sln**, and then click **Open**.
4. In **Solution Explorer**, expand the **ServiceBusRelay.Server** project, and then double-click **Program.cs**.  

   Explain that the service endpoint is configured to receive TCP messages directly on port 747.
   
5. In **Solution Explorer**, expand the **ServiceBusRelay.WebClient** project, expand **Controllers**, and then double-click **HomeController.cs**.
6. Locate the **Write** method, and explain that the Web client consumes the service by using the address and port of the server.
7. In **Solution Explorer**, right-click the root node of the solution, and then click **Properties**.
8. In the **Solution &#39;ServiceBusRelay&#39; Property Pages** dialog box, select the **Multiple startup projects** option.
9. In the grid view, for the **ServiceBusRelay.Server** and **ServiceBusRelay.WebClient** projects, change the action to **Start**.
10. Click **OK**.
11. To start the solution without debugging, press Ctrl+F5. Wait until the console application displays the **The server is running** message.
12. Position the browser and console windows next to each other so that you can show both windows.
13. In the browser window, in the **Text** text box, type **Hello**, and then click **Write to console**. The **Hello** text will appear in the console window.
14. Close the browser and the console window.
15. On the Start menu, click the **Microsoft Edge** tile.
16. Go to the Microsoft Azure portal at **http://portal.azure.com**.
17. If a page appears, asking for your email address and password, type your email address and password, and then click **Sign in**.

     >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

18. If the **Windows Azure Tour** dialog box appears, close it.
19. On the top-left side of the portal, click the **+** button.
20. Click **Enterprise Integration**, and then click **Service Bus**.
21. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    a. Name: Type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials).      
    b. Pricing Tier: Select **Standard**. \
    c. Subscription: Select either **Free Trial** or any available subscription.
       
       >**Note:** If you don't have an available subscription, create one before proceeding.
       
    d. Resource Group: Select **Create New**, and then type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials). \
    e. Region: Select the region closest to your location.

22. To create the namespace, at the bottom of the dialog box, click **Create**, and then wait until the namespace is active.
23. In the **Dashboard**, under the **All Resources** tile, you should see your newly created service bus. Click it.
     >**Note:** If you cannot see the service bus, click **See more** to access the full list of resources. Alternatively, you can expand the tile by dragging its bottom-right corner. 
24. In the service bus box, on the menu on the left, click the **Shared access policies** tab, and then click **RootManageSharedAccessKey**.
25. In the box that opens to the right, click the **Copy** icon on the right side of the **Primary Key** text box.
26. If you are prompted to allow access to your Clipboard, click **Allow access**.
27. Close the dialog box, and then return to Visual Studio 2017.
28. In **Solution Explorer**, right-click the **ServiceBusRelay.Server** project, and then click **Manage NuGet Packages**.
29. In the navigation pane of the **Manage NuGet Packages** dialog box, expand the **Online** node, and then click the **NuGet official package source** node.
30. Press Ctrl+E, and then type **WindowsAzure.ServiceBus**.
31. In the center pane, click the **Microsoft Azure Service Bus** package, and then click **Install**.
32. If a **License Acceptance** dialog box appears, click **I Accept**.
33. Wait for the installation to complete, and then click **Close** to close the dialog box.
34. In **Solution Explorer**, right-click the **ServiceBusRelay.WebClient** project, and then click **Manage NuGet Packages**.
35. To install the **WindowsAzure.ServiceBus** NuGet package in the **ServiceBusRelay.WebClient** project, repeat steps 29-33.
36. In **Solution Explorer**, expand the **ServiceBusRelay.Server** project, and then double-click **Program.cs**.
37. To the beginning of the file, add the following **using** directive.

  ```cs
        using Microsoft.ServiceBus;
```
38. Locate the call to the **ServiceHost** constructor and change the host&#39;s address from **net.tcp://127.0.0.1:747/** to **sb://ServiceBusDemo07** _YourInitials_**.servicebus.windows.net** (Replace _YourInitials_ with your initials).
39. Locate the call to the **host.AddServiceEndpoint** method, and then update the binding to **NetTcpRelayBinding** by using the following code.

  ```cs
        var endpoint = host.AddServiceEndpoint(typeof(IConsoleService), new NetTcpRelayBinding(), "console");
```
40. Add a new **TransportClientEndpointBehavior** to the endpoint by adding the following code before calling the **host.Open** method.

  ```cs
        endpoint.EndpointBehaviors.Add(new TransportClientEndpointBehavior
        {
             TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "[access_key]")
        });
```
41. Select the _[access_key]_ placeholder in the code you added, and press Ctrl+V to paste the key you have copied from the Azure portal.
42. To save the changes, press Ctrl+S.
43. In **Solution Explorer**, expand the **ServiceBusRelay.WebClient** project, then expand **Controllers**, and then double-click **HomeController.cs**.
44. To the beginning of the file, add the following **using** directive.

  ```cs
        using Microsoft.ServiceBus;
```
45. In the **Write** method, locate the call to the **ChannelFactory** constructor.
46. In the constructor call, change the binding from **NetTcpBinding** to **NetTcpRelayBinding**.
47. In the constructor call, change the endpoint address from **net.tcp://127.0.0.1:747/console** to **sb://ServiceBusDemo07** YourInitials **.servicebus.windows.net/console** (Replace _YourInitials_ with your initials).
48. Before calling the **factory.CreateChannel** method, add the following code.

  ```cs
        factory.Endpoint.EndpointBehaviors.Add(new TransportClientEndpointBehavior { TokenProvider =         TokenProvider.CreateSharedAccessSignatureTokenProvider("RootManageSharedAccessKey", "[access_key]")});
```
49. Select the _[access_key]_ placeholder in the code you added, and press Ctrl+V to paste the key you have copied from the Azure portal.
50. To save the changes, press Ctrl+S.
51. Go back to the Azure portal.
52. On the top-left side of the portal, click the **+** button.
53. Click **Web & Mobile**.
54. Click **Web App**.
55. In the **App name** text box, type **ServiceBusDemo07** _YourInitials_ (Replace _YourInitials_ with your initials).
56. Click **App Service Plan/Location**, and then click **Create New**.
57. In the **App Service Plan** text box, type **ServiceBusDemo07** _YourInitials_ (Replace _YourInitials_ with your initials).
58. In the **Location** drop-down list, select the region closest to your location.
59. Click **OK**.
60. Click **Create**. Wait until the web app is created.
61. In the **All Resources** box, locate and select the web app you just created.
62. Click **Overview**, and then click **Download the publish profile**.
63. Save the file to **D:\Allfiles\Mod07\DemoFiles\ServiceBusRelay**.
64. Return to Visual Studio 2012.
65. In **Solution Explorer**, right-click the **ServiceBusRelay.WebClient** project, and then click **Publish**.
66. In the **Publish Web** dialog box, click **Import**, and then go to **D:\Allfiles\Mod07\DemoFiles\ServiceBusRelay**. Select the profile file that you downloaded earlier, and then click **Open**.
67. In the **Import Publish Profile** dialog box, click **OK**.
68. Click **Publish**. Visual Studio 2012 publishes the web application according to the settings that are provided in the profile file. After the deployment completes, the uploaded web application opens in a new browser window.
69. Return to Visual Studio 2012.
70. In **Solution Explorer**, right-click the **ServiceBusRelay.Server** project, and then click **Set as StartUp Project**.
71. To start the application without debugging, press Ctrl+F5. Wait until the console application displays the **The server is running** message.
72. Position the browser and console windows next to each other so that you can show both windows.
73. In the browser window, in the **Text** text box, type **Hello** , and then click **Write to console**. The text **Hello** appears in the console window.

     >**Note:** You can also ask students to open a browser window on their computers, go to the address of the web app, type messages, and send them to the console window. Show the students their messages in the console window.

74. Close the browser window, and then close the console window.

# Lesson 2: Microsoft Azure Service Bus Queues

### Demonstration: Sending Messages by Using Azure Service Bus Queues

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project\Solution**.
3. Type **D:\Allfiles\Mod07\DemoFiles\QueuesDemo\Begin\QueuesDemo.sln** in the **File name** text box, and then click **Open**.
4. In **Solution Explorer**, right-click the root node of the solution, point to **Add**, and then click **New Project**.
5. In the navigation pane of the **Add New Project** dialog box, expand the **Installed** node, expand the **Visual C#** node, click the **Windows** node, and then from the list of templates, select **Console Application**.
6. In the **Name** text box, type **ServiceBusMessageSender**.
7. In the **Location** text box, type **D:\Allfiles\Mod07\DemoFiles\QueuesDemo\Begin**.
8. Click **OK**.
9. In **Solution Explorer**, right-click the **ServiceBusMessageSender** project, and then click **Manage NuGet Packages**.
10. In the navigation pane of the **Manage NuGet Packages** dialog box, expand the **Online** node, and then click the **NuGet official package source** node.
11. Press Ctrl+E, and then type **WindowsAzure.ServiceBus**.
12. In the center pane, click the **Microsoft Azure Service Bus** package, and then click **Install**.
13. If a **License Acceptance** dialog box appears, click **I Accept**.
14. Wait for the installation to complete, and then click **Close** to close the window.
15. In **Solution Explorer**, right-click the **ServiceBusMessageSender** project, and then click **Add Reference**.
16. In the **Reference Manager** dialog box, expand the **Assemblies** node in the pane on the left side, and then click **Framework**.
17. Scroll down the assemblies list, point to the **System.Configuration** assembly, select the check box next to the assembly name, and then click **OK.**
18. Go to the Azure portal at **http://portal.azure.com**.
19. If a page appears asking for your email address and password, type your email address and password, and then click **Sign in**.

    >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

20. If the **Windows Azure Tour** dialog box appears, close it.
21. On the top-left side of the portal, click the **+** button.
22. Click **Enterprise Integration**, and then click **Service Bus**.
23. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    a. Name: Type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials).      
    b. Pricing Tier: Select **Standard**. \
    c. Subscription: Select either **Free Trial** or any available subscription.
       
       >**Note:** If you don't have an available subscription, create one before proceeding.
       
    d. Resource Group: Select **Create New**, and then type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials). \
    e. Region: Select the region closest to your location.

24. To create the namespace, at the bottom of the dialog box, click **Create**, and then wait until the namespace is active.
25. In the **Dashboard**, under the **All Resources** tile, you should see your newly created service bus. Click it.
    >**Note:** If you cannot see the service bus, click **See more** to access the full list of resources. Alternatively, you can expand the tile by dragging its bottom-right corner. 
26. In the service bus box, on the menu on the left, click the **Shared access policies** tab, then click **RootManageSharedAccessKey**.
27. In the box that opens to the right, click the **Copy** icon on the right side of the **Primary Key** text box.
28. If you are prompted to allow access to your clipboard, click **Allow access**.
29. Close the dialog box, and then return to Visual Studio 2017.
30. In **Solution Explorer**, under the **ServiceBusMessageSender** project, double-click **App.config**.  
    
    In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
    
31. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
32. To save the changes, press Ctrl+S.
33. In **Solution Explorer**, under the **ServiceBusMessageReceiver** project, double-click **App.config**.
34. In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
35. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
36. To save the changes, press Ctrl+S.
37. In **Solution Explorer**, under the **ServiceBusMessageSender** project, double-click **Program.cs**.
38. At the beginning of the file, add the following **using** directives.

  ```cs
        using Microsoft.ServiceBus;
        using Microsoft.ServiceBus.Messaging;
        using System.Configuration;
```
39. To create a namespace manager for the Service Bus queue, append the following code to the **Main** method.

  ```cs
        string connectionString = CloudConfigurationManager.AppSettings["Microsoft.ServiceBus.ConnectionString"];
        var namespaceManager = NamespaceManager.CreateFromConnectionString(connectionString);
```
40. To verify that the queue exists, append the following code to the **Main** method.

  ```cs
        if (!namespaceManager.QueueExists("servicebusqueue"))
          {   
             namespaceManager.CreateQueue("servicebusqueue");
          }
```
41. Create a queue client by appending the following code to the **Main** method.

  ```cs
        QueueClient queueClient = QueueClient.Create("servicebusqueue");
```
42. To continuously get input from the user, and then to send it to the queue, add the following code to the **Main** method.

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
43. To save the changes, press Ctrl+S.
44. In **Solution Explorer**, right-click the **ServiceBusMessageSender** project, and then click **Set as StartUp Project**.
45. To run the sender console application, press Ctrl+F5.
46. Wait for the **Enter text to send to the queue and press Enter** console prompt.
47. Type **Hello**, and then press Enter.
48. Type **Goodbye**, and then press Enter.
49. Press Enter, and then press any key to close the console window.
50. In Visual Studio 2012, in **Solution Explorer**, under the **ServiceBusMessageReceiver** project, double-click **Program.cs**.
51. Show that the code in the **Main** method also connects to the Service Bus queue, and after connecting to the queue, it pulls messages from the queue and prints them.
52. Show how you use the **Receive** and **Complete** methods to implement Peek-Lock.
53. In **Solution Explorer**, right-click the **ServiceBusMessageReceiver** project, and then click **Set as StartUp Project**.
54. To run the receiver console application, press Ctrl+F5.
55. Check that the **Receiver** console window prints the text **Hello**, and then **Goodbye**.
56. Close the console window.

# Lesson 3: Microsoft Azure Service Bus Topics

### Demonstration: Subscription-Based Messaging with Azure Service Bus Topics

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. In the **File name** text box, type **D:\Allfiles\Mod07\DemoFiles\TopicsDemo\TopicsDemo.sln**, and then click **Open**.  
   
   Explain that the solution contains four console application projects. The **ServiceBusTopicPublisher** project creates topics, defines the available subscriptions, and sends four sales messages.
   
   The three remaining projects act as subscribers, and print messages sent to their subscription to the console.

   - **ExpensivePurchasesSubscriber**. Receives sales that are over $4000.
   - **CheapPurchasesSubscriber**. Receives sales that are under $4000.
   - **AuditSubscriber**. Receives all sales.

5. In **Solution Explorer**, expand the **ServiceBusTopicPublisher** project, and then double-click **Program.cs**.
6. Locate the **Main** method. Explain the use of the **NamespaceManager** class to check if the topic exists, and then to create the topic if it does not exist.
7. Examine the code that creates the subscriptions. Show the use of the **NamespaceManager.CreateSubscription** method, and the **SqlFilter** class.
8. Show how the four **BrokeredMessage** objects are created, and how their properties are set accordingly.
9. Demonstrate how the **TopicClient** object is created, and how it is used to send messages to the Service Bus.
10. In **Solution Explorer**, expand the **ExpensivePurchasesSubscriber** project, and then double-click **Program.cs**.
11. Locate the **Main** method, and show the use of the **SubscriptionClient** class. The code connects to the 
**productsalestopic** topic with the **ExpensivePurchases** subscription.  

    Explain that the code continuously receives messages from the subscription and prints them to the console.
    
12. Show the **CheapPurchasesSubscriber** and **AuditSubscriber** projects, and explain that the difference between these projects is just the name of the subscription that is being passed to the **Create** method in **SubscriptionClient**.
13. On the Start screen, click the **Internet Explorer** tile.
14. Go to the Azure portal at **http://manage.windowsazure.com**.
15. If a page appears, asking for your email address, type your email address, and then click **Continue**. Wait for the 
**Sign In** page to appear, type your email address and password, and then click **Sign In.**

     >**Note:** If during the sign-in process, a page appears asking you to choose from a list of previously used accounts, select the account you previously used, and then continue to type your credentials.

16. If the **Windows Azure Tour** dialog box appears, close it.
17. On the top-left side of the portal, click the **+** button.
18. Click **Enterprise Integration**, and then click **Service Bus**.
19. In the **CREATE A NAMESPACE** dialog box, provide the following information:

    a. Name: Type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials).      
    b. Pricing Tier: Select **Standard**. \
    c. Subscription: Select either **Free Trial** or any available subscription.
       
       >**Note:** If you don't have an available subscription, create one before proceeding.
       
    d. Resource Group: Select **Create New**, and then type **ServiceBusDemo07 _YourInitials_** (Replace _YourInitials_ with your initials). \
    e. Region: Select the region closest to your location.

20. To create the namespace, at the bottom of the dialog box, click **Create**, and then wait until the namespace is active.
21. In the **Dashboard**, under the **All Resources** tile, you should see your newly created service bus. Click it.
     >**Note:** If you cannot see the service bus, click **See more** to access the full list of resources. Alternatively, you can expand the tile by dragging its bottom-right corner.
22. In the service bus box, on the menu on the left, click the **Shared access policies** tab, then click **RootManageSharedAccessKey**.
23. Point the cursor to the connection string text that starts with **Endpoint=**.
24. Click the **Copy** icon on the right side of the text.
25. If you are prompted to allow access to your clipboard, click **Allow access.**
26. Return to Visual Studio 2017.
27. In **Solution Explorer**, under the **ServiceBusTopicPublisher** project, double-click **App.config**.
28. In the **&lt;appSettings&gt;** section, locate the **Microsoft.ServiceBus.ConnectionString** key.
29. To paste the connection string over the existing connection string placeholder, select the content of the **value** attribute, and then press Ctrl+V.
30. To save the changes, press Ctrl+S.
31. Open the **App.config** file in the **ExpensivePurchasesSubscriber**, **CheapPurchasesSubscriber**, and **AuditSubscriber**  projects, and then perform steps 27-29 in each configuration file.
32. In **Solution Explorer**, right-click the **ServiceBusTopicPublisher** project, and then click **Set as StartUp Project**.
33. To start the project without debugging, press Ctrl+F5.
34. Wait until the console prompt shows the **Press any key to start publishing messages** message, and then press Enter.
35. Wait until the console prompt shows the **Sending complete** message, and then press Enter to close the console window.
36. Return to Visual Studio 2017.
37. In **Solution Explorer**, right-click the root node of the solution, and then click **Properties**.
38. In the **Solution &#39;TopicsDemo&#39; Property Pages** dialog box, click **Multiple startup projects**.
39. In the grid view, change the action to **Start** for the following projects:

    - **AuditSubscriber**
    - **CheapPurchasesSubscriber**
    - **ExpensivePurchasesSubscriber**

40. Click **OK**.
41. To run the projects, press Ctrl+F5.
42. Wait for the three console windows to open. Show the students that messages are received in the console windows of the subscribers according to the subscription filter defined in the **ServiceBusTopicPublisher** project:

    - The **Audit Subscriber** console window prints all four messages to the console.
    - The **Cheap Purchases Subscriber** console window prints two messages describing two purchases with a price that is less than $4000.
    - The **Expensive Purchases Subscriber** console window prints two messages describing two purchases with a price that is more than $4000.

43. Close the three console windows.

©2018 Microsoft Corporation. All rights reserved.

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

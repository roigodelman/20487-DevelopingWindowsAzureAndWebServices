# Module 5: Creating WCF Services

> Wherever you see a path to file starting at [repository root], replace it with the absolute path to the directory in which the 20487 repository resides. 
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487, then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

# Lesson 2: Creating and Implementing a Contract

### Demonstration: Creating a WCF Service

1. On the Start menu, right click the **Visual Studio 2017** tile and select **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. On the **File** menu, point to **Open**, and then click **Project/Solution**.
4. Go to **[Repository Root]\Allfiles\20487C\Mod05\DemoFiles\CreatingWCFService\begin**.
5. Select the **CreatingWCFService.sln** file, and then click **Open**.
6. In **Solution Explorer**, expand the **Service** project, and then double-click **IHotelBookingService.cs**. This interface is the service contract, but it still has to be configured with **[ServiceContract]** and **[ServiceOperation]** attributes.
7. In **Solution Explorer**, double-click **HotelBookingService.cs**. This class is the implementation of the service contract.
8. In **Solution Explorer**, double-click **BookingResponse.cs**. This class is a data contract that is returned by the service operation **BookHotel**. It still has to be configured with the **[DataContract]** and **[DataMember]** attributes.
9. In **Solution Explorer**, right-click the project, and then click **Add Reference**.
10. In the **Reference Manager** dialog box, expand the **Assemblies** node in the left pane, and then click **Framework**.
11. Scroll down the assemblies list, point to the **System.Runtime.Serialization** assembly, and then select the check box next to the assembly name.
12. Scroll down the assemblies list some more, point to the **System.ServiceModel** assembly, and then select the check box next to the assembly name. Click **OK** to close the dialog box.
13. In **Solution Explorer**, double-click **IHotelBookingService.cs**.
14. Add the following **using** directive.

   ```cs
        using System.ServiceModel;
```
15. Add the **[ServiceContract]** attribute above the **IHotelBookingService** interface declaration.
16. Add the **[OperationContract]** attribute above the **BookHotel** method declaration.  

    These attributes mark the interface as a WCF service contract and expose the interface methods as service operations.
 
17. To save the file, press Ctrl+S.
18. In **Solution Explorer**, double-click **BookingResponse.cs**.
19. Add a **using** directive for the **System.Runtime.Serialization** namespace after the last **using** directive.
20. To the **BookingResponse** class declaration, add the **[DataContract]** attribute.
21. To each of the properties of the class, add the **[DataMember]** attribute.  

    These attributes define the classes as data contracts so that WCF can serialize/deserialize them appropriately.
    
22. To save the file, press Ctrl+S.
23. In **Solution Explorer**, double-click **Reservation.cs**.
24. To the **Reservation** class declaration, add the **[DataContract]** attribute.
25. To each of the properties of the class, add the **[DataMember]** attribute.
26. To save the file, press Ctrl+S.
27. In Visual Studio 2017, press F5 to run the project. Check that the WCF Test Client opens, and the service project&#39;s tree in the left pane displays the **BookHotel()** operation node.
28. Double-click the operation **BookHotel**, locate the **HotelName** parameter in the **Request** section on the right, and enter**HotelA** in the **Value** column.
29. To make a request to the service, click **Invoke**.
30. If a security warning appears, click **OK**.
31. Check that the **Response** section displays the following:

    - BookingReference: **AR3254**
    - IsApproved: **True**

32. Close the WCF Test Client application.
33. Close Visual Studio 2017.

# Lesson 3: Configuring and Hosting WCF Services

### Demonstration: Configuring Endpoints in Code and in Configuration

1. On the Start menu, right click the **Visual Studio 2017** tile and select **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. On the **File** menu, point to **Open**, and then click **Project/Solution**.
4. Go to **[Repository Root]\Allfiles\20487C\Mod05\DemoFiles\DefineServiceEndpoints\begin**, select **DefineServiceEndpoints.sln**, and then click **Open**.
5. In **Solution Explorer**, expand the **ServiceHost** project, and then double-click **App.config**.
6. Locate the **&lt;system.serviceModel&gt;** section group. In this section of the file, you define the base address, endpoints, and behaviors of the service.
7. To the **&lt;serviceBehaviors&gt;** element, add the following configuration:

```cs
    <behavior>
        <serviceMetadata httpGetEnabled="True"/>
    </behavior>
```  
   The service cannot be tested unless the service behavior has the **serviceMetadata** behavior.
8. To add base addresses, add the following configuration to the **&lt;service&gt;** element.

```cs
    <host>
        <baseAddresses>
            <add baseAddress="http://localhost:8733/" />
        </baseAddresses>
    </host>
```
9. To save the file, press Ctrl+S.
10. In **Solution Explorer**, right-click **App.config**, and then click **Edit WCF Configuration**.
11. In the **Configuration** pane of the **Service Configuration Editor** window, expand the **HotelBooking.HotelBookingService** node,  right-click the **Endpoints** node, and then click **New Service Endpoint.**
12. In the **General** tab, enter the following information.

    - Address: **booking**
    - Binding: **basicHttpBinding**
    - Contract: **HotelBooking.IHotelBookingService**

13. On the **File** menu, click **Save**.
14. On the **File** menu, click **Exit**.
15. Return to Visual Studio 2012. If you receive a message saying that the **App.config** file has changed, click **Yes**.  

    You added a service endpoint that uses **basicHttpBinding**. This endpoint uses a relative address (**booking**). This is why the base addresses section is also required.
    
16. In **Solution Explorer**, under the **ServiceHost** project, click **Program.cs**.
17. After the initialization of the **ServiceHost**, add the following line of code to the **Main** method.

   ```cs
        host.AddServiceEndpoint(typeof(IHotelBookingService), new NetTcpBinding(), "booking");
```

   You added an endpoint that uses **NetTcpBinding**, with a relative address (**booking**).  
    
18. To save the file, press Ctrl+S.  
19. In **Solution Explorer**, right-click the **ServiceHost** project, and then click **Set as StartUp Project**.  
20. To run the project, press F5.  
21. Verify that the service is hosted in the console application.  
22. On the Start screen, click the **Developer Command Prompt for VS2017** tile.  
23. In the command prompt, type the following command and press Enter.  

   ```cs
        WcfTestClient  http://localhost:8733/
```
24. After the **WCF Test Client** tool opens, review the service and its endpoint. Notice that the service has two endpoints.
25. Under the TCP binding endpoint, double-click the method **BookHotel**.
26. In the parameter **HotelName** (under the **Value** column), enter the value **HotelA**.
27. To send the request to the service, click **Invoke**.
28. If a security warning appears, click **OK**.
29. Check that the **Response** section displays the following:

    - BookingReference: **AR3254**
    - IsApproved: **True**

30. In the left pane, right-click the **http://localhost:8733/** node, and then click **Copy Address**.
31. On the Start menu, click the **Microsoft Edge** icon, select all the text in the browser address bar, and press Ctrl+V to paste the metadata address you copied. Press Enter.
32. The page displays an explanation of how to consume the service from a client. Click **http://localhost:8733/?wsdl**, and then review the overall structure of the WSDL file.
33. Close the browser and the WCF Test Client window.
34. Return to Visual Studio 2012, press Shift+F5 to stop debugging, and then close Visual Studio 2012.

# Lesson 4: Consuming WCF Services

### Demonstration 1: Adding a Service Reference

1. On the Start menu, right click the **Visual Studio 2017** tile and select **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. On the **File** menu, point to **Open**, and then click **Project/Solution**.
4. Go to **[Repository Root]\Allfiles\20487C\Mod05\DemoFiles\AddingServiceReference\begin**.
5. Select the **AddServiceReference.sln** file, and then click **Open**.
6. In **Solution Explorer**, expand the **ServiceHost** project and double-click **App.config**. **App.config** contains three service endpoints: one with **BasicHttpBinding**, one with **NetTcpBinding**, and one MEX endpoint. The service must have the **ServiceMetadata** behavior before you can add a service reference with Visual Studio 2012.
7. In **Solution Explorer**, right-click the **ServiceHost** project, and then click **Set as StartUp Project**.
8. To start the service host without debugging, press Ctrl+F5.
9. Wait for the service host console to display the **Service Hosted Successfully!** message.
10. Return to Visual Studio 2017. In **Solution Explorer**, under the **ServiceClient** project, right-click **Service References**, and then click **Add Service Reference**.
11. In the **Add Service Reference** dialog box, in the **Address** box, enter **http://localhost:8733/HotelBooking**,  and then click  **Go**.
12. Wait for the service information to download. In the **Namespace** box, enter **HotelBooking**, and then click **OK**. The service proxy class and the data contract classes are added under the **HotelBooking** namespace.
13. In **Solution Explorer**, expand the **ServiceClient** project, and double-click the **app.config** file. Point out the new **&lt;system.serviceModel&gt;** section group that Visual Studio added to the file. The configuration contains the client endpoints and binding configuration.
14. In **Solution Explorer**, under the **ServiceClient** project, double-click **Program.cs**.
15. Select the code that appears in comments, click the **Edit** menu, point to **Advanced**, and then click **Uncomment Selection**.  
    
    The code in the **Main** method initializes a new instance of the generated service proxy **HotelBooking.HotelBookingServiceClient.** The generated proxy implements the service contract interface and, therefore, you can use it to start the service methods as if it was a local object.

16. To save the file, press Ctrl+S.
17. In **Solution Explorer**, right-click **ServiceClient**, point to **Debug**, and then click **Start new instance**.  

    Wait until the console application starts and displays the following message: &quot;Booking response: Approved, booking reference: AR3254&quot;.
    
18. To close the client console application, press Enter.
19. Switch to the service console application window, and press Enter to shut down the service.
20. Close Visual Studio 2017.

### Demonstration 2: Using Channel Factories

1. On the Start menu, right click the **Visual Studio 2017** tile and select **Run as Administrator**.
2. In the **User Account Control** modal, click **Yes**.
3. On the **File** menu, point to **Open**, and then click **Project/Solution**.
4. Go to **[Repository root]\Allfiles\20487C\Mod05\DemoFiles\UsingChannelFactory\begin**.
5. Select the **UsingChannelFactory.sln** file, and then click **Open**.
6. In **Solution Explorer**, expand the **ServiceHost** project, and then double-click **App.config**. The **App.config** file contains three service endpoints: one with **BasicHttpBinding**, one with **NetTcpBinding**, and one MEX endpoint.
7. In **Solution Explorer**, right-click the **ServiceClient** project, point to **Add**, and then click **Reference**.
8. In the left pane of the **Reference Manager** dialog box, expand the **Solution** node, and then click **Projects**.
9. In the projects list, point to **Common**, and then select the check box next to the project name.
10. In the left pane, expand the **Assemblies** node, and then click **Framework**. Scroll down the assemblies list, point to the  **System.ServiceModel** assembly, select the check box next to the assembly name, and then click **OK**.  

    The reference was added to gain access to the service and data contracts and to the WCF client libraries.
    
11. In **Solution Explorer**, expand the **ServiceClient** project and double-click **Program.cs**.
12. To the **using** section at the top of the file, add the following **using** directives.

   ```cs
        using HotelBooking;
        using System.ServiceModel;
```
13. At the beginning of the **Main** method, add the following lines of code before the commented code.

   ```cs
        ChannelFactory<IHotelBookingService> serviceFactory =
            new ChannelFactory<IHotelBookingService>
                (new BasicHttpBinding(),
                "http://localhost:8733/HotelBooking/HotelBookingHttp");
                
        IHotelBookingService proxy = serviceFactory.CreateChannel();
   ```  
    
   The service contract interface is passed as a generic type parameter to the **ChannelFactory&lt;T&gt;** generic class. 
   
14. Select the code that appears in comments, click the **Edit** menu, point to **Advanced**, and then click **Uncomment Selection**.  
15. To save the file, press Ctrl+S.  
16. In **Solution Explorer**, right-click the **ServiceHost** project, and then click **Set as StartUp Project**.  
17. To start the service host without debugging, press Ctrl+F5.  
18. Wait for the service host console to display the **Service Hosted Successfully!** message.  
19. Return to Visual Studio 2017. In **Solution Explorer**, right-click the **ServiceClient** project, and then click **Set as StartUp Project**.  
20. To start the client without debugging, press Ctrl+F5.  
21. Wait until the console application starts and displays the **Booking response: Approved, booking reference: AR3254** message.  
22. Close both console applications and Visual Studio 2017.  

©2018 Microsoft Corporation. All rights reserved.  

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

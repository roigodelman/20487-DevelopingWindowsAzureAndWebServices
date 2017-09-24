# Module 5: Creating WCF Services

# Lesson 2: Creating and Implementing a Contract

### Demonstration: Creating a WCF Service

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Go to **D:\Allfiles\Mod05\DemoFiles\CreatingWCFService\begin**.
4. Select the file **CreatingWCFService.sln** and then click **Open**.
5. In Solution Explorer, expand the **Service** project, and double-click **IHotelBookingService.cs**. This interface is the service contract, but it still has to be configured with **[ServiceContract]** and **[ServiceOperation]** attributes.
6. In Solution Explorer, double-click **HotelBookingService.cs**. This class is the implementation of the service contract.
7. In Solution Explorer, double-click **BookingResponse.cs**. This class is a data contract that is returned by the service operation **BookHotel**. It still has to be configured with the **[DataContract]** and **[DataMember]** attributes.
8. In Solution Explorer, right-click the project, and click **Add Reference**.
9. In the **Reference Manager** dialog box, expand the **Assemblies** node in the left pane and click **Framework**.
10. Scroll down the assemblies list, point to the **System.Runtime.Serialization** assembly, and select the check box next to the assembly name.
11. Scroll down the assemblies list some more, point to the **System.ServiceModel** assembly, and select the check box next to the assembly name. Click **OK** to close the dialog box.
12. In Solution Explorer, double-click **IHotelBookingService.cs**.
13. Add the following **using** directive.

   ```cs
        using System.ServiceModel;
```
14. Add the **[ServiceContract]** attribute above the **IHotelBookingService** interface declaration.
15. Add the **[OperationContract]** attribute above the **BookHotel** method declaration.
16. These attributes mark the interface as a WCF service contract and expose the interface methods as service operations.
17. To save the file, press Ctrl+S.
18. In Solution Explorer, double-click **BookingResponse.cs**.
19. Add a **using** directive for the **System.Runtime.Serialization** namespace after the last **using** directive.
20. To the **BookingResponse** class declaration, add the **[DataContract]** attribute.
21. To each of the properties of the class, add the **[DataMember]** attribute.
22. These attributes define the classes as data contracts so that WCF can serialize/deserialize them appropriately.
23. To save the file, press Ctrl+S.
24. In Solution Explorer, double-click **Reservation.cs**.
25. To the **Reservation** class declaration, add the **[DataContract]** attribute.
26. To each of the properties of the class, add the **[DataMember]** attribute.
27. To save the file, press Ctrl+S.
28. In Visual Studio 2017, press F5 to run the project. Check that the WCF Test Client opens, and the service project&#39;s tree in the left pane displays the **BookHotel()** operation node.
29. Double-click the operation **BookHotel** , locate the **HotelName** parameter in the **Request** section on the right, and enter  **HotelA** in the **Value** column.
30. To make a request to the service, click **Invoke**.
31. If a security warning appears, click **OK**.
32. Check that the **Response** section displays the following:

  - BookingReference: **AR3254**
  - IsApproved: **True**

33. Close the WCF Test Client application.
34. Close Visual Studio 2017.

# Lesson 3: Configuring and Hosting WCF Services

### Demonstration: Configuring Endpoints in Code and in Configuration

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Go to **D:\Allfiles\Mod05\DemoFiles\DefineServiceEndpoints\begin** , select **DefineServiceEndpoints.sln** , and then click  **Open**.
4. In Solution Explorer, expand the **ServiceHost** project and double-click **App.config**.
5. Locate the **&lt;system.serviceModel&gt;** section group. In this section of the file, you define the base address, endpoints, and behaviors of the service.
6. To the **&lt;serviceBehaviors&gt;** element, add the following configuration:

   ```cs
        <behavior>
          <serviceMetadata httpGetEnabled="True"/>
        </behavior>
```
7. The service cannot be tested unless the service behavior has the **serviceMetadata** behavior.
8. To add base addresses, add the following configuration to the **&lt;service&gt;** element.

   ```cs
        <host>
           <baseAddresses>
               <add baseAddress="http://localhost:8733/" />
           </baseAddresses>
        </host>
```
9. To save the file, press Ctrl+S.
10. In Solution Explorer, right-click **App.config** and click **Edit WCF Configuration**.
11. In the Service Configuration Editor window, expand the **HotelBooking.HotelBookingService** node in the **Configuration** pane, right-click the **Endpoints** node, and click **New Service Endpoint.**
12. In the **General** tab, enter the following information.

  - Address: **booking**
  - Binding: **basicHttpBinding**
  - Contract: **HotelBooking.IHotelBookingService**

13. On the **File** menu, click **Save**.
14. On the **File** menu, click **Exit**.
15. Return to Visual Studio 2012. If you receive a message saying that the **App.config** file has changed, click **Yes**.
16. You added a service endpoint that uses **basicHttpBinding**. This endpoint uses a relative address ( **booking** ). This is why the base addresses section is also required.
17. In Solution Explorer, under the **ServiceHost** project, click **Program.cs**.
18. After the initialization of the **ServiceHost** , add the following line of code to the **Main** method.

   ```cs
        host.AddServiceEndpoint(typeof(IHotelBookingService), new NetTcpBinding(), "booking");
```
19. You added an endpoint that uses **NetTcpBinding** , with a relative address ( **booking** ).
20. To save the file, press Ctrl+S.
21. In Solution Explorer, right-click the **ServiceHost** project and click **Set as StartUp Project**.
22. To run the project, press F5.
23. Verify that the service is hosted in the console application.
24. On the Start screen, click the **Developer Command Prompt for VS2017** tile.
25. In the command prompt, type the following command and press Enter.

   ```cs
        WcfTestClient  http://localhost:8733/
```
26. After the **WCF Test Client** tool opens, review the service and its endpoint. Notice that the service has two endpoints.
27. Under the TCP binding endpoint, double-click the method **BookHotel**.
28. In the parameter **HotelName** (under the **Value** column), enter the value **HotelA**.
29. To send the request to the service, click **Invoke**.
30. If a security warning appears, click **OK**.
31. Check that the **Response** section displays the following:

  - BookingReference: **AR3254**
  - IsApproved: **True**

32. In the left pane, right-click the **http://localhost:8733/** node, and click **Copy Address**.
33. On the Start menu, click the **Microsoft Edge** icon, select all the text in the browser address bar, and press Ctrl+V to paste the metadata address you copied. Press Enter.
34. The page displays an explanation of how to consume the service from a client. Click the link  **http://localhost:8733/?wsdl** and review the overall structure of the WSDL file.
35. Close the browser and the WCF Test Client window.
36. Return to Visual Studio 2012, press Shift+F5 to stop debugging, and then close Visual Studio 2012.

# Lesson 4: Consuming WCF Services

### Demonstration 1: Adding a Service Reference

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Go to **D:\Allfiles\Mod05\DemoFiles\AddingServiceReference\begin**.
4. Select the file **AddServiceReference.sln** and click **Open**.
5. In Solution Explorer, expand the **ServiceHost** project and double-click **App.config**. **App.config** contains three service endpoints: one with **BasicHttpBinding** , one with **NetTcpBinding** , and one MEX endpoint. The service must have the **ServiceMetadata** behavior before you can add a service reference with Visual Studio 2012.
6. In Solution Explorer, right-click the **ServiceHost** project and click **Set as StartUp Project**.
7. To start the service host without debugging, press Ctrl+F5.
8. Wait for the service host console to display the message &quot;Service Hosted Successfully!&quot;
9. Return to Visual Studio 2017. In Solution Explorer, right-click **Service References** under the **ServiceClient** project and click **Add Service Reference**.
10. In the **Add Service Reference** dialog box, enter **http://localhost:8733/HotelBooking** in the **Address** box, and then click  **Go**.
11. Wait for the service information to download, enter **HotelBooking** in the **Namespace** box, and then click **OK**. The service proxy class and the data contract classes are added under the **HotelBooking** namespace.
12. In Solution Explorer, expand the **ServiceClient** project, and double-click the **app.config** file. Point out the new **&lt;system.serviceModel&gt;** section group that Visual Studio added to the file. The configuration contains the client endpoints and binding configuration.
13. In Solution Explorer, under the **ServiceClient** project, double-click **Program.cs**.
14. Select the code that appears in comments, click the **Edit** menu, point to **Advanced** , and click **Uncomment Selection**. The code in the **Main** method initializes a new instance of the generated service proxy  **HotelBooking.HotelBookingServiceClient.** The generated proxy implements the service contract interface and, therefore, you can use it to start the service methods as if it was a local object.
15. To save the file, press Ctrl+S.
16. In Solution Explorer, right-click **ServiceClient** , point to **Debug** , and click **Start new instance**.
17. Wait until the console application starts and displays the following message: &quot;Booking response: Approved, booking reference: AR3254&quot;.
18. To close the client console application, press Enter.
19. Switch to the service console application window, and press Enter to shut down the service.
20. Close Visual Studio 2017.

### Demonstration 2: Using Channel Factories

1. On the Start menu, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Go to **D:\Allfiles\Mod05\DemoFiles\UsingChannelFactory\begin**.
4. Select the file **UsingChannelFactory.sln** and click **Open**.
5. In Solution Explorer, expand the **ServiceHost** project, and double-click **App.config**. The **App.config** file contains three service endpoints: one with **BasicHttpBinding** , one with **NetTcpBinding** , and one MEX endpoint.
6. In Solution Explorer, right-click the **ServiceClient** project, point to **Add** and then click **Reference**.
7. In the **Reference Manager** dialog box, expand the **Solution** node in the left pane, and then click **Projects**.
8. In the projects list, point to **Common** and select the check box next to the project name.
9. In the left pane, expand the **Assemblies** node and click **Framework**. Scroll down the assemblies list, point to the  **System.ServiceModel** assembly, select the check box next to the assembly name, and click **OK**.
10. The reference was added to gain access to the service and data contracts and to the WCF client libraries.
11. In Solution Explorer, expand the **ServiceClient** project and double-click **Program.cs**.
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
14. The service contract interface is passed as a generic type parameter to the **ChannelFactory&lt;T&gt;** generic class.
15. Select the code that appears in comments, click the **Edit** menu, point to **Advanced** , and then click **Uncomment Selection**.
16. To save the file, press Ctrl+S.
17. In Solution Explorer, right-click the **ServiceHost** project, and then click **Set as StartUp Project**.
18. To start the service host without debugging, press Ctrl+F5.
19. Wait for the service host console to display the message &quot;Service Hosted Successfully!&quot;
20. Return to Visual Studio 2017. In Solution Explorer, right-click the **ServiceClient** project, and then click **Set as StartUp Project**.
21. To start the client without debugging, press Ctrl+F5.
22. Wait until the console application starts and displays the message &quot;Booking response: Approved, booking reference: AR3254&quot;.
23. Close both console applications and Visual Studio 2017. 

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

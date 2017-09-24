# Module 5: Creating WCF Services

## Lab: Creating and Consuming the WCF Booking Service

#### Scenario

Until now, most of Blue Yonder Airlines&#39; booking systems were internal systems, connected to the booking database of the company. Because there are plans to move the newly created ASP.NET Web API service to a location outside the internal network of the company (probably to the Microsoft Azure cloud), there is a need to create a new service that can receive booking requests from both internal and external applications. Because the requirements from the new service include features such as support for TCP and MSMQ communication, it was decided that the new service will be a WCF service. In this lab, you will create a WCF service for the booking subsystem. In addition, you will update the ASP.NET Web API booking service to use the new WCF booking service.

#### Objectives

After completing this lab, you will be able to:

- Create service and data contract, and implement the service contract.
- Configure a WCF service for TCP and host it in a console application.
- Consume a WCF service from a client application.

### Exercise 1: Creating the WCF Booking Service

#### Scenario

The first step in creating a WCF service is to define the service contract and data contracts. Only then can you begin implementing the service contract. In this exercise, you will define a service contract interface for the booking service along with the required data contracts, and then you will implement the service contract.

The main tasks for this exercise are as follows:

1. Create a data contract for the booking request

2. Create a service contract for the booking service

3. Implement the service contract

#### Task 1: Create a data contract for the booking request

1. Open the **BlueYonder.Server.sln** solution file from the  **D:\AllFiles\Mod05\LabFiles\begin\BlueYonder.Server** folder.
2. In the **BlueYonder.BookingService.Contracts** project, add the **TripDto** data contract class.

   a. Set the access modifier of the class to **public** and decorate it with the **[DataContract]** attribute.  
   b. Add the following properties to the class.

  | **Name** | **Type** |
  | --- | --- |
  | FlightScheduleId | int |
  | Status | FlightStatus |
  | Class | SeatClass |
  
   c. Decorate each of the new properties with the **[DataMember]** attribute.

  >**Note:** In order to use data contract object, the **System.ServiceModel** and **System.Runtime.Serialization** assemblies need to be added to the project references. The begin solution already contains those assemblies.

3. Add the **ReservationDto** data contract class.

   a. Set the access modifier of the class to **public** and decorate it with the **[DataContract]** attribute.  
   b. Add the following properties to the class.

  | **Name** | **Type** |
  | --- | --- |
  | TravelerId | int |
  | ReservationDate | DateTime |
  | DepartureFlight | TripDto |
  | ReturnFlight | TripDto |

   c. Decorate each of the new properties with the **[DataMember]** attribute.

  >**Note:** Review the **ReservationCreationFault** class in the **Faults** folder. The class will be used later as a data contract object to mark a fault reservation. ** **

#### Task 2: Create a service contract for the booking service

1. In the **BlueYonder.BookingService.Contracts** project, add a new interface named **IBookingService**. Set the access modifier of the interface to **public**.
2. Decorate the interface with the **[ServiceContract]** attribute and set the **Namespace** parameter of the attribute to [http://blueyonder.server.interfaces/](http://blueyonder.server.interfaces/).
3. Add the **CreateReservation** method to the interface, and define it as an operation contract.

  - The method should receive a parameter named **request** of type **ReservationDto** , and return a string.
  - Decorate the method with the **[OperationContract]** attribute.
  - Decorate the method with the **[FaultContract]** attribute, and set the attribute&#39;s parameter to the type of the  **ReservationCreationFault** class.

#### Task 3: Implement the service contract

1. In the **BlueYonder.BookingService.Implementation** project, implement the **IBookingService** in the **BookingService** class.

   a. Change the class declaration, so it will implement the **IBookingService** interface.  
   b. Decorate the class with the **[ServiceBehavior]** attribute and set the attribute&#39;s **InstanceContextMode** parameter to  **InstanceContextMode.PerCall**.

2. In the service implementation class, implement the service contract.

   - Create the **CreateReservation** method, but do not fill it with code yet.

  >**Note:** At this point, the class will not compile because no value is returned from the method. Ignore this for now, because you will soon write the missing code.

3. Start implementing the **CreateReservation** method by verifying whether the request contains information about the departure flight. If the departure flight information is missing, throw a fault exception.

   a. If the **request.DepartureFlight** property is **null** , throw a **FaultException** of type **ReservationCreationFault**.
   b. In the **FaultException** constructor, create a new instance of **ReservationCreationFault** with the following property values.

 | **Property** | **Value** |
 | --- | --- |
 | Description | Reservation must include a departure flight |
  ReservationDate | request.ReservationDate |

   c. In the **FaultException** constructor, set the second constructor parameter to the reason string **&quot;Invalid flight info&quot;**.

4. Continue implementing the **CreateReservation** method by creating a **Reservation** object.

   - Initialize the **Reservation** object according to the following table.

      | **Property** | **Value** |
      | --- | --- |
      | TravelerId | request.TravelerId |
      | ReservationDate | request.ReservationDate |
      | DepartureFlight | A new **Trip** object with the following values. |
   
      | **Property** | **Value** |    
      | --- | --- |
      | Class | request.DepartureFlight.Class |         
      | Status | request.DepartureFlight.Status    |        
      | FlightScheduleID | request.DepartureFlight.FlightScheduleID | 

5. Continue implementing the **CreateReservation** method by checking whether the return flight is not **null**. If  **request.ReturnFlight** is not null, add a trip to the reservation object you created.

   - Initialize the **reservation.ReturnFlight** property with a new **Trip** object, and set its properties according to the following table.

     | **Property** | **Value** |
     | --- | --- |
     | Class | request.ReturnFlight.Class |
     | Status | request.ReturnFlight. Status |
     | FlightScheduleID | request.ReturnFlight.FlightScheduleID |

6. Continue implementing the **CreateReservation** method by adding the new reservation object to the database:

   a. Create a new **ReservationRepository** object and initialize it with the **ConnectionName** field.  
   b. Use the **ReservationUtils.GenerateConfirmationCode** static method to generate a confirmation code and assign it to the **reservation.ConfirmationCode** property before saving the new reservation.  
   c. Call the **Add** method and then the **Save** method of the repository to save the newly created **reservation**.  
   d. Return the generated confirmation code to the client.

  >**Note:** To make sure the context and the database connection are disposed properly at the end of the service operation, you should place the repository-related code in a **using** block.

7. Insert a breakpoint at the beginning of the **CreateReservation** method.

>**Results**: You will be able to test your results only at the end of the second exercise.

### Exercise 2: Configuring and Hosting the WCF Service

#### Scenario

The second step in creating the WCF service is to create a project for hosting the service. In this exercise, you will create a service host, configure it with a TCP endpoint, and use it to make the service available for clients.

The main tasks for this exercise are as follows:

1. Configure the console project to host the WCF service with the TCP endpoint

2. Create the service hosting code

#### Task 1: Configure the console project to host the WCF service with the TCP endpoint

1. In the **BlueYonder.BookingService.Host** project, add a reference to the **System.ServiceModel** assembly.

  >**Note:** The starter solution already contains all the project references that are needed for the project. This includes the **BlueYonder.BookingService.Contracts** , **BlueYonder.BookingService.Implementation BlueYonder.DataAccess** ,and **BlueYonder.Entities** projects, and the Entity Framework 5.0 package assembly.

2. Review the **FlightScheduleDatabaseInitializer.cs** file in the **BlueYonder.BookingService.Host** project. Observe how the  **Seed** method initializes the database with predefined locations and flights.

3. In the **BlueYonder.BookingService.Host** project,open the **App.config** file, and add a service configuration section for the Booking WCF service.

   a. Add the **&lt;system.serviceModel&gt;** element to the configuration, and in it, add the **&lt;services&gt;** element.  
   b. In the **&lt;services&gt;** element, add a **&lt;service&gt;** element, and then set its **name** attribute to  **BlueYonder.BookingService.Implementation.BookingService**.

4. Add an endpoint configuration to the service configuration you added in the previous step.

   - In the **&lt;service&gt;** element, add an **&lt;endpoint&gt;** element with the following attributes.

    | **Attribute** | **Value** |
    | --- | --- |
    | name | BookingTcp |
    | address | net.tcp://localhost:900/BlueYonder/Booking/ |
    | binding | netTcpBinding |
    | contract | BlueYonder.BookingService.Contracts.IBookingService |

5. In the **App.config** file, add a connection string to the local SQL Express.

   ```cs
        <connectionStrings>
           <add name="BlueYonderServer" connectionString="Data Source=.\SQLEXPRESS;Database=BlueYonder.Server.Lab5;Integrated    Security=SSPI" 
        providerName="System.Data.SqlClient" />
        </connectionStrings>
```
  >**Note:** You can copy the connection string from the ASP.NET Web API services configuration file in  **D:\Allfiles\Mod05\Labfiles\begin\BlueYonder.Server\BlueYonder.Companion.Host\Web.config**. Make sure you change the database parameter to **BlueYonder.Server.Lab5**.
```

 ##### Task 2: Create the service hosting code

1. In the **BlueYonder.BookingService.Host** project, open the **Program.cs** file, and then add two static event handler methods to handle the **ServiceOpening** and **ServiceOpened** events of the service host.

   - Each of the methods receives two parameters: **sender** , of type **object** , and **args** , of type **EventArgs**.
   - In each method, write a short status message to the console window.

2. In the **Main** method, add the following code to initialize the database.

   ```cs
        var dbInitializer = new FlightScheduleDatabaseInitializer();
        dbInitializer.InitializeDatabase(new TravelCompanionContext(Implementation.BookingService.ConnectionName));
   ```
3. In the **Main** method, add code to host the **BookingService** service class.

   a. Create a new instance of the **ServiceHost** class for the **BookingService** service class.  
   b. Register to the service host&#39;s **Opening** and **Opened** events with the **ServiceOpening** and **ServiceOpened** methods, respectively.  
   c. Open the service host, wait for user input, and then close the service host.

  >**Note:** Refer to Lesson 3, &quot;Configuring and Hosting WCF Services&quot;, Topic 1, &quot;Hosting WCF Services&quot;, for an example on how to open the service host, wait for user input, and then close the service host.

4. Run the **BlueYonder.BookingService.Host** project in the debug mode and verify it opens without throwing exceptions. Keep the console window open, because you will need to use it later in the lab.

>**Results**: You will be able to start the console application and open the service host.

### Exercise 3: Consuming the WCF Service from the ASP.NET Web API Booking Service

#### Scenario

After you create the WCF service, you can consume it from the ASP.NET Web API web application. In this exercise, you will configure the client endpoint in the ASP.NET Web API web application, and use the **ChannelFactory&lt;T&gt;** generic class to create a client proxy. You will then use the new proxy to call the WCF service, create the reservation on the backend system, and get the reservation confirmation code in return.

The main tasks for this exercise are as follows:

1. Add a reference to the service contract project in the ASP.NET Web API project

2. Add the client configuration to the Web.config file

3. Call the Booking service by using ChannelFactory&lt;T&gt;

4. Debug the WCF service with the client app

#### Task 1: Add a reference to the service contract project in the ASP.NET Web API project

1. Open the **D:\AllFiles\Mod05\LabFiles\begin\BlueYonder.Server\BlueYonder.Companion.sln** solution file in a new Visual Studio 2012 instance, and add the **BlueYonder.BookingService.Contracts** project from the  **D:\Allfiles\Mod05\Labfiles\begin\BlueYonder.Server\BlueYonder.BookingService.Contracts** folder to the solution.

2. In the **BlueYonder.Companion.Controllers** project, add a reference to the **BlueYonder.BookingService.Contracts** project.
3. In the **BlueYonder.Companion.Host** project, add a reference to the **BlueYonder.BookingService.Contracts** project.

#### Task 2: Add the client configuration to the Web.config file

1. In the **BlueYonder.Companion.Host** project, open the **Web.config** file, and add a client endpoint configuration to call the Booking WCF service.

   a. Add the **&lt;system.serviceModel&gt;** element to the configuration, and in it, add the **&lt;client&gt;** element.  
   b. In the **&lt;client&gt;** element add an **&lt;endpoint&gt;** element with the following attributes. 

   | **Configuration Parameter** | **Value** |
   | --- | --- |
   | address | net.tcp://localhost:900/BlueYonder/Booking |
   | binding | netTcpBinding |
   | contract | BlueYonder.BookingService.Contracts.IBookingService |
   | name | BookingTcp |

  >**Note:** Make sure you set the **name** attribute of the endpoint to **BookingTcp** , because you will use this endpoint name in the code to locate the endpoint configuration.

#### Task 3: Call the Booking service by using ChannelFactory&lt;T&gt;

1. In the **BlueYonder.Companion.Controllers** project, open the **ReservationsController.cs** file. In the **ReservationsController** class, create a channel factory object for the **IBookingService** service contract, and store it in a field named **factory**.

   - In the channel factory constructor, use the **BookingTcp** endpoint configuration name.

2. In the **CreateReservationOnBackendSystem** method, uncomment the code that creates the **TripDto and ReservationDto** objects.
3. In the **CreateReservationOnBackendSystem** method, create a new channel by using the channel factory you have created.

   a. Before the **try** block, create the channel by calling the **factory.CreateChannel** method.  
   b. Store the newly created channel in a variable of type **IBookingService**.

4. In the **try** block, call the **CreateReservation** operation of the **Booking** service.

   a. The **CreateReservation** operation returns the confirmation code string. Store the returned string in a local variable.  
   b. After calling the service, close the channel by casting the channel object to the **ICommunicationObject** interface, and then call its **Close** method.  
   c. Return the confirmation code string and remove the **return** statement that is currently at the end of the method.

  >**Note:** Refer to Lesson 4, &quot;Consuming WCF Services&quot;, Topic 2, &quot;Creating a Service Proxy with ChannelFactory&lt;T&gt;&quot;, for an example on how to close a channel.

5. Change the first **catch** block from

   ```cs
        catch (HttpException fault)
   ```
     to
   ```
        catch(FaultException<ReservationCreationFault> fault)
   ```
   a. Inside the **catch** block, throw an **HttpResponseException** with an **HttpResponseMessage** object.  
   b. Create the **HttpResponseMessage** by using the **Request.CreateResponse** method. Set the status code to **BadRequest** (HTTP 400), and the content of the message to the description of the fault.  
   c. Abort the connection in case of Exception, by calling the **Abort** method on the proxy object.

6. In the second **catch** block, abort the connection before calling the **throw** statement.

   - Abort the connection by casting the channel object to the **ICommunicationObject** interface, and then calling its **Abort** method.

7. In the **Post** method, before adding the new reservation to the repository, call the Booking WCF service and set the reservation&#39;s confirmation code.

   a. Call the **CreateReservationOnBackendSystem** method with the **newReservation** object.  
   b. Store the returned confirmation code of the reservation in the **newReservation.ConfirmationCode** property.

  >**Note:** The reservation should be saved to the database with the confirmation code you got from the WCF service, so make sure you set the confirmation code property before adding the reservation to the repository.

8. Build the solution.

  >**Note:** The **BlueYonder.Companion.Host** project is already configured for web hosting with IIS. Building the solution will make the web application ready for use.

#### Task 4: Debug the WCF service with the client app

1. Place a breakpoint on the line of code that calls the **CreateReservationOnBackendSystem** method, and start debugging the web application.
2. Open the **BlueYonder.Companion.Client** solution from the **D:\AllFiles\Mod05\LabFiles\begin** folder, and run it without debugging.
3. Search for **New** and purchase a new trip from _Seattle_ to _New York_.
4. Debug the **BlueYonder.Companion** and **BlueYonder.Server** solutions. Verify that the ASP.NET Web API service is able to call the WCF service. Continue running both solutions and verify that the client is showing the new reservation.

>**Results**: After you complete this exercise, you will be able to run the Blue Yonder Companion client application and purchase a trip.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

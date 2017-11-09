# Module 4 - Extending and Securing ASP.NET Web API Services

## Lab: Extending Travel Companion&#39;s ASP.NET Web API Services

#### Scenario

Blue Yonder Airlines wishes to add more features to their client application: searchable flight schedule, getting an RSS feed for flight schedules, and changing the way the client connects to the service, by using  a secured HTTPS channel. In this lab, you will implement the necessary changes to the ASP.NET Web API services you previously created. In addition, you will use these changes to apply validations and dependency injection to the services and make them more secured, manageable, and testable.

#### Objectives

After completing this lab, you will be able to:

- Secure the service&#39;s communication with HTTPS.
- Use System.ComponentModel.DataAnnotations and an ActionFilter to validate a model.
- Create a MediaTypeFormatter to support returning image content.
- Create an OData queryable service action.
- Use the dependency resolver to inject repository types.

### Exercise 1: Creating a Dependency Resolver for Repositories

#### Scenario

Controllers use repositories as data providers. In the current implementation, the controller is responsible to create the repository in the constructor. This is not a good practice because the controller knows the type of the repository that results in a coupling.

In the following exercise, you will decouple the controller and repository by using the dependency injection technique to inject the repository interface as a parameter in the controller constructor.

You will start by creating a dependency resolver class that is responsible for creating the repositories. You will then register the dependency resolver class in the **HttpConfiguration** to automatically create a repository when a controller is used. Finally, you will use Microsoft Fakes and create a stub for a repository, and then use it in a unit test project to test the location controller.

The main tasks for this exercise are as follows:

1. Change the FlightController constructor to support injection

2. Create a dependency resolver class

3. Register the dependency resolver class with HttpConfiguration

#### Task 1: Change the FlightController constructor to support injection

1. Open the BlueYonder.Companion.sln solution from the [Repository Root]\AllFiles\20487C\Mod04\LabFiles\begin\BlueYonder.Companion folder.
2. In the **BlueYonder.Companion.Controllers** project, change the **LocationsController** class constructor to receive the locations repository:

   a. Change the **LocationsController** constructor method, so that it receives an **ILocationRepository** object.  
   b. Initialize the **Locations** member with the constructor parameter.

  >**Note:** The same pattern was already applied in the starter solution for the rest of the controller classes (**TravelersController** , **FlightsController** , **ReservationsController** , and **TripsController**).  
   Open those classes to review the constructor definition.

#### Task 2: Create a dependency resolver class

1. Open the **BlueYonderResolver** class from the **BlueYonder.Companion.Controllers** project, and in the **GetService** method, add the missing code for resolving the **LocationsController** class.

  - If the **serviceType** parameter is of the type **LocationsController** , create an instance of the **LocationsController** class with the required repository.

  >**Note:** The BlueYonderResolver class implements the IDependencyResolver interface.

#### Task 3: Register the dependency resolver class with HttpConfiguration

1. Open the **WebApiConfig.cs** file, located under the **App\_Start** folder in the **BlueYonder.Companion.Host** project, and set  **BlueYonderResolver** as the new dependency resolver.

   - Use the **DependencyResolver** property of the **config** object to set the dependency resolver.

2. Test the application and the **DependencyResolver** injection:

   a. Open the **LocationsController** class from the **BlueYonder.Companion.Controllers** project, and place a breakpoint in the constructor.  
   b. Run the **BlueYonder.Companion.Host** project in debug mode.  
   c. Navigate to the address http://localhost:9239/Locations.  
   d. Return to Visual Studio 2012 and verify that the code breaks on the breakpoint and that the constructor parameter is initialized (not null).

3. Open the **LocationControllerTest** test class from the **BlueYonder.Companion.Controllers.Tests** project, and examine the code in the **Initialize** method. The test initialization process uses the **StubILocationRepository** type,which was auto-generated with the **Fakes** framework. This stub repository mimics the real location repository. You use the fake repository to test the code, instead of using the real repository, which requires using a database for the test. When running unit tests, you should use fake objects to replace external components, in order to reduce the complexity of creating and executing the test.

  >**Note:** For additional information about Fakes, see: [http://go.microsoft.com/fwlink/?LinkID=298770&amp;clcid=0x409](http://go.microsoft.com/fwlink/?LinkID=298770&amp;clcid=0x409)

4. Test the application by using the **Fakes** mock framework and running the test project.

   - On the **Test** menu, point to **Run** , and the click **All Tests**.

>**Results**: You will be able to inject data repositories to the controllers instead of creating them explicitly inside the controllers. This will decouple the controllers from the implementation of the repositories.

### Exercise 2: Adding OData Capabilities to the Flight Schedule Service

#### Scenario

OData is a data access protocol that provides standard CRUD access of a data source via a website.

To add support for the OData protocol, you will install the NuGet package **Microsoft.AspNet.WebApi.OData** , and then decorate the methods that should support OData with the **[Queryable]** attribute.

The main tasks for this exercise are as follows:

1. Add a Queryable action to the flight schedule service

2. Handle the search event in the client application and query the flight schedule service by using OData filters

#### Task 1: Add a Queryable action to the flight schedule service

1. Use the Package Manager Console window to install the **5.2.3** version of the **Microsoft.AspNet.WebApi.OData** NuGet package in the **BlueYonder.Companion.Controllers** project.
2. In the **BlueYonder.Companion.Controllers** project, open the **LocationsController** class, and decorate the **Get** method overload, which has three parameters, with the **[EnableQuery]** attribute.
3. Change the method implementation to use the repository&#39;s **GetAll** method and return **IQueryable** instead of  **IEnumerable**.
4. Remove the parameters from the method declaration. You will not need them anymore because the OData infrastructure will take care of the query filtering.
5. In the **BlueYonder.Companion.Host** project, expand the **App_Data** folder and open the **WebApiConfig.cs** file.
6. In the **Register** method, add the following code:
```cs
        ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
        builder.EntitySet<LocationDTO>("Locations");
        builder.EntityType<LocationDTO >().Filter("City");
        config.MapODataServiceRoute(
                routeName: "ODataRoute",
                routePrefix: "odata",
                model: builder.GetEdmModel());
        config.EnsureInitialized();
```

#### Task 2: Handle the search event in the client application and query the flight schedule service by using OData filters

With the OData endpoint up and running, you will now need to consume it on the client. The **BlueYonder.Companion.Client** project has a connected service named **LocationService**.
**LocationService** is an OData connected service that allows you to interact with the OData endpoint using regular C# code.
You will need to change the current implementation of the client to fetch locations using the OData service instead of using a regular HTTP request.

1. Open the BlueYonder.Companion.Client solution from the **[Repository root]\AllFiles\20487C\Mod04\LabFiles\begin\BlueYonder.Companion.Client** folder.
2. Open the **Helpers\DataManager.cs** file from the **BlueYonder.Companion.Client** project.
3. Add the following code to the **using** section:
	```cs
		using BlueYonder.Compaion.Entities;
	```
3. Locate **GetLocationsAsync** at line 86.
4. Replace the current implementation with the following code:
	```cs
	Container container = new Container(new Uri(Addresses.BaseODataUri));
    return await container.Locations.AddQueryOption("$filter", $"contains(tolower(City),tolower('{query}'))").ExecuteAsync();
	```


>**Results**: Your web application exposes the OData protocol that supports the **Get** request of the locations data.

### Exercise 3: Applying Validation Rules in the Booking Service

#### Scenario

To make your web application more robust, you need to add some validations to the incoming data.

You will apply a validation rule to your server to verify that all required fields of a model are sent from the client before handling the request.

You will decorate the **Travel** model with attributes that define the required fields, then you will derive from the **ActionFilter** class and implement the validation of the model. Finally, you will add the validation to the **Post** action.

The main tasks for this exercise are as follows:

1. Add Data Annotations to the Traveler Class

2. Implement the Action filter to validate the model

3. Apply the custom attribute to the PUT and POST actions in the booking service

#### Task 1: Add Data Annotations to the Traveler Class

1. Return to the **BlueYonder.Companion** solution. In the **BlueYonder.Entities**  project, use data annotation attributes to validate the **Traveler** class&#39; properties by using the following rules:


   - **FirstName** , **LastName** , and **HomeAddress** should use the **[Required]** validation attribute.
   - **MobilePhone** should use the **[Phone]** validation attribute.
   - **Email** should use the **[EmailAddress]** validation attribute.

#### Task 2: Implement the Action filter to validate the model

1. In the **BlueYonder.Companion.Controllers** project, create a public class named **ModelValidationAttribute** that derives from **ActionFilterAttribute**. Add the new class to the project&#39;s **ActionFilters** folder.
2. In the new class, override the **OnActionExecuting** method and implement it as follows:

   a. Check if the model state is valid by using the **actionContext.ModelState.IsValid** property.  
   b. If the model state is not valid, call **actionContext.Request.CreateErrorResponse** to create an error response message.

    >**Note:** The **CreateErrorResponse** method is an extension method. To use it, add a **using** directive for the  **System.Net.Http** namespace.

   c. For the error response, use the overload that expects an **HttpStatusCode** enum and an **HttpError** object.    
   d. Use **HttpStatusCode.BadRequest** , and initialize the **HttpError** object with **actionContext.ModelState** and the **true**  Boolean value for the second constructor parameter.

#### Task 3: Apply the custom attribute to the PUT and POST actions in the booking service

1. In the **BlueYonder.Companion.Controllers** project, open the **TravelersController** class, and decorate the **Put** and **Post** methods with the **[ModelValidation]** attribute.
2. Build the solution.

>**Results**: Your web application will verify that the minimum necessary information is sent by the client before trying to handle it.

### Exercise 4: Securing the Communication between the Client and the Server

#### Scenario

To support a secured communication between the service and the client, you will need to define a certificate.

You will need to add an HTTPS binding to the service and specify the certificate to be used by the binding. Finally, you will need to configure the client to communicate with the service over the secured connection.

The main tasks for this exercise are as follows:

1. Change IIS Express to use HTTPS

2. Test the client application against the secure connection

#### Task 1: Add an HTTPS binding in IIS

1. In the **BlueYonder.Companion** solution, select the **BlueYonder.Companion.Host** project, and then go to the properties view.
2. Change **SSL Enabled** from **False** to **True**.
3. Run the application.
4. You will be prompted to trust the IIS Express SSL certificate. Click **Yes**.
5. A **Security Warning** window will pop up asking you if you want to install the certificate. Click **Yes**.

#### Task 3: Test the client application against the secure connection

1. Return to the **BlueYonder.Companion.Client** solution in the **20487B-SEA-DEV-C** virtual machine.
	

   - Open the **Addresses.cs** file from the **BlueYonder.Companion.Shared** project and in the **BaseUri** property, change the URL from using HTTP to HTTPS.

2. Run the client app, search for **New** , and purchase a flight from _Seattle_ to _New York._ Provide an incorrect value in the email field and verify if you get a validation error message originating from the service.


   - Use any non-valid email address, such as **ABC** or **ABC@DEF**.

3. Correct the email address and purchase the trip. Verify that the trip was purchased successfully, and then close the client app.

>**Results**: The communication with your web application will be secured by using a certificate.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

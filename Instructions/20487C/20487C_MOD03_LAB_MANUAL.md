# Module 3: Creating and Consuming ASP. NET Web API Services

# Lab: Creating the Travel Reservation ASP. NET Web API Service

> Wherever  you see a path to file starting at *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487,
then the following path: [repository root]\AllFiles\20487C\Mod03 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod03

#### Scenario

Now that the data layer has been created, services that provide travel destination information, flight schedules, and booking capabilities can be developed. Blue Yonder Airlines intends to support many device types. The back-end service must have an HTTP-based service. Therefore, the service is to be implemented by using ASP.NET Web API. In this lab, you will create a Web API service that supports basic CRUD actions over Blue Yonder Airlines&#39; database. In addition, you will update the Travel Companion Windows Store app to consume the newly created service.

#### Objectives

After you complete this lab, you will be able to:

- Create an ASP. NET Web API service.
- Implement **CRUD** actions in the service.
- Consume an ASP. NET Web API service with the **System.Net.HttpClient** library.

### Exercise 1: Creating an ASP.NET Web API Service

#### Scenario

Implement the travelers&#39; service by using ASP.NET Web API. Start by creating a new ASP.NET Web API controller, and implement CRUD functionality using the POST, GET, PUT, and DELETE HTTP methods.

The main tasks for this exercise are as follows:

1. Create a new API Controller for the Traveler Entity

#### Task 1: Create a new API Controller for the Traveler Entity

1. Open the  **[repository root]\AllFiles\20487C\Mod03\LabFiles\begin\BlueYonder.Companion\BlueYonder.Companion.sln** solution file, and add a new class called **TravelersController** to the **BlueYonder.Companion.Controllers** project.

2. Change the access modifier of the class to **public** , and derive it from the **ApiController** class.
3. Create a private property named **Travelers** of type **ITravelerRepository** and initialize it in the constructor.

   a. Create a new property named **Travelers** of type **ITravelerRepository**.  
   b. Create a default constructor for the **TravelersController** class.  
   c. Initialize the **Travelers** property with a new instance of the **TravelerRepository** class.

4. Create an action method named **Get** to handle GET requests.

   a. The method receives a **string** parameter named **id** and returns an **HttpResponseMessage** object. Call the **FindBy** method of the **ITravelerRepository** interface to search for a traveler using the **id** parameter. The ID of the traveler is stored in the traveler&#39;s   **TravelerUserIdentity** property.  
   b. If the traveler was found, use the **Request.CreateResponse** to return an HTTP response message with the traveler. Set the status code of the response to **OK**.  
   c. If a traveler was not found, use the **Request.CreateResponse** to return an empty message. Set the status code to **NotFound** (HTTP 404).

5. Insert a breakpoint at the beginning of the **Get** method.
6. Create an action method to handle **POST** requests.

   a. The method receives a **Traveler** parameter called **traveler** and returns an **HttpResponseMessage** object. Implement the method by calling the **Add** and then the **Save** methods of the **Travelers** repository.  
   b. Create an **HttpResponseMessage** returning the **HttpStatusCode.Created** status and the newly created traveler object.  
   c. Set the **Location** header of the response to the URI where you can access the newly created traveler. The new URI should be a concatenation of the request URI and the new traveler&#39;s ID.

   >**Note:** You can refer to the implementation of the **Post** method in the **ReservationsController** class for an example of how to set the **Location** header.
   
   d. Return the HTTP response message.

7. Insert a breakpoint at the beginning of the **Post** method.
8. Create an action method to handle **PUT** requests.


 - The method receives a **string** parameter called **id** and a **Traveler** parameter called **traveler**. The method returns an  **HttpResponseMessage** object.
 - If the traveler does not exist in the database, use the **Request.CreateResponse** method to return an HTTP response message with the **HttpStatusCode.NotFound** status.

  >**Note:** To check if the traveler exists in the database, use the **FindBy** method as you did in the **Get** method.


   - If the traveler exists, call the **Edit** and then the **Save** methods of the **Travelers** repository to update the traveler, and then use the **Request.CreateResponse** method, to return an HTTP response message with the **HttpStatusCode.OK** status.


  >**Note:** The **HTTP PUT** method can also be used to create resources. Checking if the resources exist is performed here for simplicity.


9. Insert a breakpoint at the beginning of the **Put** method.
10. Create an action method to handle **DELETE** requests.


 - The method receives a **string** parameter called **id**.
 - If the traveler does not exist in the database, use the **Request.CreateResponse** method to return an HTTP response message with the **HttpStatusCode.NotFound** status.

  >**Note:** To check if the traveler exists in the database, use the **FindBy** method as you did in the **Get** method.


  - If the traveler exists, call the **Delete** and then the **Save** methods of the **Travelers** repository, and then use the  **Request.CreateResponse** method, to return an HTTP response message with the **HttpStatusCode.OK** status.
  

  >**Results** : After completing this exercise, you should have run the project from Visual Studio 2012 and access the travelers&#39; service.

### Exercise 2: Consuming an ASP.NET Web API Service

#### Scenario

Consume the travelers&#39; service from the client application. Start by implementing the **GetTravelerAsync** method by invoking a  **GET** request to retrieve a specific traveler from the server. Continue by implementing the **CreateTravelerAsync** method by invoking a POST request to create a new traveler. And complete the exercise by implementing the **UpdateTravelerAsync** method by invoking a **PUT** request to update an existing traveler.

The main tasks for this exercise are as follows:

1. Consume the API Controller from a Client Application

2. Debug the Client App

#### Task 1: Consume the API Controller from a Client Application

1. Open the **[repository root]\AllFiles\20487C\Mod03\LabFiles\begin\BlueYonder.Companion.Client\BlueYonder.Companion.Client.sln** solution.

2. In the **BlueYonder.Companion.Client** project, open the **DataManager** class from the **Helpers** folder and implement the  **GetTravelerAsync** method.

   a. Remove the **return null** line of code.  
   b. Build the relative URI using the string format **&quot;{0}travelers/{1}&quot;**. Replace the {0} placeholder with the **BaseUri** property and the {1} placeholder with the **hardwareId** variable.  
   c. Call the **client.GetAsync** method with the relative address you constructed. Use the **await** keyword to call the method asynchronously. Store the response in a variable called **response**.  
   d. Check the value of the **response.IsSuccessStatusCode** property. If the value is **false** , return **null**.  
   e If the value of the **response.IsSuccessStatusCode** property is **true** , read the response into a string by using the  **response.Content.ReadAsStringAsync** method. Use the **await** keyword to call the method asynchronously.  
   f. Use the **JsonConvert.DeserializeObjectAsync** static method to convert the JSON string to a **Traveler** object. Call the method using the **await** keyword and return the deserialized traveler object.

3. Insert a breakpoint at the beginning of the **GetTravelerAsync** method.
4. Review the **CreateTravelerAsync** method. The method sets the **ContentType** header to request a JSON response. The method then uses the **PostAsync** method to send a POST request to the server.
5. Insert a breakpoint at the beginning of the **CreateTravelerAsync** method.
6. Review the **UpdateTravelerAsync** method. The method uses the **client.PutAsync** method to send a PUT request to the server.
7. Insert a breakpoint at the beginning of the **UpdateTravelerAsync** method.

#### Task 2: Debug the Client App

1. Start debugging the **BlueYonder.Companion.Host** project.

2. Start debugging the **BlueYonder.Companion.Client** project.
3. Debug the client app and verify that you break before sending a GET request to the server. Press F5 to continue running the code.
4. Debug the service code.

   a. The breakpoint you have set in the **Get** method of the **TravelersController** class should be highlighted. Inspect the value of the **id** parameter.  
   b. Press F5 to continue running the code.

5. Debug the client app.

   a. The code execution breaks inside the **CreateTravelerAsync** method.  
   b. Press F5 to continue running the code.

6. Debug the service code.

   a. The breakpoint you have set in the **Post** method solution should be highlighted. Inspect the contents of the **traveler**  parameter.  
   b. Press F5 to continue running the code.

7. Use the client app to purchase a flight from _Seattle_ to _New York_.

   a. Display the app bar search for the word **New**. Purchase the trip from _Seattle_ to _New York_.    
   b. Fill in the traveler information according to the following table and then click **Purchase**.  

   | **Field** | **Value** |
   | --- | --- |
   | First Name | Your first name |
   | Last Name | Your last name |
   | Passport | **Aa1234567** |
   | Mobile Phone | **555-5555555** |
   | Home Address | **423 Main St.** |
   | Email Address | Your email address |

   c. The code execution breaks inside the **UpdateTravelerAsync** method. Press F5 to continue running the code.

8. Debug the service code.

   a. The breakpoint you have set in the **Put** method solution should be highlighted.  
   b. Inspect the contents of the **traveler** parameter. Press F5 to continue running the code.

9. Close the confirmation message, and then close the client app.
10. Stop the debugging in Visual Studio 2017.

>**Results** : After completing this exercise, you should have run the BlueYonder Companion client application and created a traveler when purchasing a trip. You also should have retrieved an existing traveler and updated its details.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

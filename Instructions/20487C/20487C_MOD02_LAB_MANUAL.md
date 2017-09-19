# Module 2: Querying and Manipulating Data Using Entity Framework

# Lab: Creating a Data Access Layer by Using Entity Framework

#### Scenario

Blue Yonder Airlines is creating the new Travel Companion app for mobile devices that will help end-users browse the travel destinations of the company and book flights online. In this lab, you will create the server-side data layer by using Entity Framework and test your code by using integration tests.

#### Objectives

After completing this lab, you will be able to:

- Create an Entity Framework model by using Code First.
- Query an Entity Framework model by using LINQ to Entities and Entity SQL.
- Manipulate data by using Entity Framework.
- Create a database transaction with the **TransactionScope** class.

#### Lab Setup

Estimated Time: **60 minutes**


### Exercise 1: Creating a Data Model

#### Scenario

Before you create the services for the Blue Yonder Airlines Travel Companion application, you need to create the data access layer the services will use. The data access layer uses Entity Framework to load entities from the database, and to save new and updated entities back to the database.

In this exercise, you will create data model classes to represent trips and reservations, implement a **DbContext** -derived class, and create a new repository class for the **Reservation** entity.

The main tasks for this exercise are as follows:

1. Explore the existing Entity framework data model project

2. Prepare the data model classes for Entity Framework

3. Add the newly created entities to the context

4. Implement a the reservation repository

#### Task 1: Explore the existing Entity framework data model project

1. Open the **BlueYonder.Companion** solution file from the **D:\AllFiles\Mod02\LabFiles\begin\BlueYonder.Companion** folder.

2. In the **BlueYonder.Entities** project, locate the **FlightSchedule** class.

   - Locate the **FlightScheduleId** field and explore the use of the **DatabaseGenerated** attribute.
   - Locate the **Flight** property and explore the **ForeignKey** attribute.

3. Locate the **FlightRepository** class under the **Repositories** folder, in the **BlueYonder.DataAccess** project, and explore its contents.

   >**Note:** The **FlightRepository** class implements the _Repository_ pattern. The Repository pattern is designed to decouple the data access strategy from the business logic layer that handles the data. The repository exposes the data access functionality and implements it internally by using a specific data access strategy, which in this case is Entity Framework. By using repositories, you can easily create a mock, replacing the repository, and improve the testability of the business logic.

   >For more information about the Repository pattern and its related patterns, see [http://go.microsoft.com/fwlink/?LinkID=298756&amp;clcid=0x409](http://go.microsoft.com/fwlink/?LinkID=298756&amp;clcid=0x409).

   >In Lab 4, &quot;Extending Travel Companion&#39;s ASP.NET Web API Services&quot;, Module 4, &quot;Extending and Securing ASP.NET Web API Services&quot;, you will see how to increase testability by using mocked repositories.

#### Task 2: Prepare the data model classes for Entity Framework

1. Open the **Trip** class from the **BlueYonder.Entities** project, set the **FlightInfo** property as **virtual** , and decorate it with the **[ForeignKey]** attribute.

   - Declare the **FlightScheduleID** property to be **virtual**.
   - Add a **[ForeignKey]** attribute to the **FlightInfo** property.
   - Set the attribute to use the **FlightScheduleID** property as the foreign key property.

   >**Note:** In addition, Entity Framework will detect the virtual property in the **Trip** class and will create a new derived proxy class that implements lazy loading for the **FlightInfo** property.

   >When you load trip entities from the database, the entity object will be of the derived trip proxy type, and not of the **Trip**  type.

2. Open the **Reservation** class from the **BlueYonder.Entities** project, and make the following changes:

   - Declare the **DepartureFlight** property to be **virtual** , and add a **[ForeignKey]** attribute to it.The attribute should use the **DepartFlightScheduleID** property for the foreign key.
   - Declare the **ReturnFlight** property to be **virtual** , and add a **[ForeignKey]** attribute to it.The attribute should use the **ReturnFlightScheduleID** property for the foreign key.
   - Set the **ReturnFlightScheduleID** property as nullable to make it optional.

   >**Note:** Setting the **ReturnFlightScheduleID** foreign key property to a nullable int indicates that this relation is not mandatory (0-N relation, meaning a reservation does not require a return flight). The **DepartFlightScheduleID** foreign key property is not nullable and therefore indicates the relation is mandatory (1-N relation, meaning every reservation must have a departing flight)

#### Task 3: Add the newly created entities to the context

1. Open the **TravelCompanionContext** class from the **BlueYonder.DataAccess** and in it, add a new **DbSet&lt;T&gt;** property for the **Reservation** entity. Name the property **Reservations**.

2. Add another **DbSet&lt;T&gt;** property for the **Trip** entities, and name the property **Trips**.

#### Task 4: Implement a the reservation repository

1. Open the **ReservationRepository** class under the **Repositories** folder, in the **BlueYonder.DataAccess** project, and implement the **GetSingle** method. The method retrieves a **Reservation** entity by its primary key.

   - Create a LINQ query to retrieve the reservation from the **TravelCompanionContext**. **Reservations** DbSet.
   - If the entity is not found, return **null**.

   >**Note:** The **ReservationRepository** class already contains a **TravelCompanionContext** instance by the name **context.**

2. Implement the **Edit** method. The method should make sure that the reservation is loaded in the context by calling the **Find** method, and then apply the new values to the loaded entity.

   >**Note:** You can refer to Lesson 4, &quot;Manipulating Data&quot;, Topic 4, &quot;Updating Entities&quot;, for an example of how to update a detached entity by using the **Find** method.

3. Implement the **Dispose** method to dispose the private **context** member.

   - Make sure you only dispose the **context** member if the object was initiated (not null).
   - Set the context member to null after calling its **Dispose** method.

   >**Note:** If you want to see examples of the implementation of the **Dispose** method, refer to its implementation in other repositories.

4. To complete the implementation of the **ReservationRepository** class, uncomment the comments from the following methods:

   - **GetAll**
   - **Add**
   - **Delete**

   >**Note:** Review the implementation of the **Delete** method to understand how cascade delete was implemented, so that when a Reservation is deleted, its **DepartureFlight** and **ReturnFlight** objects are deleted as well.

   
   >**Results** : After you complete this exercise, the Entity Framework Code First model is ready for testing.

### Exercise 2: Querying and Manipulating Data

#### Scenario

In the previous exercise, you created the entities, context, and repository classes to handle the data in the database. Before you create the services that use the new repositories, you need to test the repositories, to verify they work properly. In this exercise, you will create tests for the repositories that will issue queries and manipulate data. In addition, you will create tests for using multiple repositories with a single transaction.

The main tasks for this exercise are as follows:

1. Explore the existing integration test project

2. Create queries by using LINQ to Entities

3. Create queries by using Entity SQL

4. Create a test for manipulating data

5. Use cross-repositories integration tests with System.Transactions

6. Run the tests, and explore the database created by Entity framework

#### Task 1: Explore the existing integration test project

1. Explore the **FlightQueries** class in the **BlueYonder.IntegrationTests** project.

   - The **TestInitialize** static method is responsible for initializing the database and the test data, and all the other methods are intended to test various queries with lazy load and eager load.

2. Explore the insert, update, and delete tests in the **FlightActions** class.

    - Observe the use of the **Assert** static class to verify the results of the test.

#### Task 2: Create queries by using LINQ to Entities

1. In the **BlueYonder.IntegrationTests** project, open the **ReservationQueries** test class, and implement the  **GetReservationWithFlightsEagerLoad** test method.

    - Create a **ReservationRepository** object in a **using** block.
    - Write a LINQ to Entities query that retrieves a **Reservation** entity having confirmation code **1234** , and performs an eager loading of its departure and return flights.
    - Use the repository&#39;s **GetAll** method to get a data source for the query.
    - For the eager load, use the **Include** method.
    - Use the **Assert** static class to verify the reservations entity was loaded, as well as its departing and returning flights.
    - To prevent any lazy load operations, use the **Assert** static class outside the **using** block scope.

2. In the **GetReservationWithFlightsLazyLoad** test method, add two Assert tests to verify that lazy load works.

    - Use the **Assert** static class to verify that the departing and returning flights of the reservation are not null.
    - Place the call to the **Assert** static class in the **using** block, after the comment.

   >**Note:** By examining the value of the navigation properties, you are invoking the lazy load mechanism.

3. In the **GetReservationsWithSingleFlightWithoutLazyLoad** test method, turn lazy loading off.

    - Add the code to turn off lazy loading before the repository is created.

   >**Note:** You can refer to Lesson 3, &quot;Querying Data&quot;, Topic 4, &quot;Load Entities by Using Lazy and Eager Loading&quot;, for an example of how to turn off lazy load. 

#### Task 3: Create queries by using Entity SQL

- In the **GetOrderedReservations** test method, create and run an Entity SQL query.

   - The query retrieves all the **Reservation** entities in descending order of their confirmation code.
   - To create an Entity SQL query, use the **ObjectContext.CreateQuery&lt;T&gt;** generic method.
   - The **context** variable already contains an inner **ObjectContext** object. To access it, cast the **context** variable to the  **IObjectContextAdapter** interface, and then access its **ObjectContext** property.

   >**Note:** Refer to Lesson 3, &quot;Querying Data&quot;, Topic 2, &quot;Query the Database By Using Entity SQL&quot;, for an example of how to write Entity SQL queries and execute them with the **ObjectContext** object.

#### Task 4: Create a test for manipulating data

- In the **BlueYonder.IntegrationTests** project, open the **FlightActions** test class, and in **UpdateFlight** test method implement the rest of the test.

   - Create a **FlightRepository** object within a **using** block.
   - Call the repository&#39;s **Edit** method and then the **Save** method to update the Flight entity in the database.
   - In a new repository, after the **using** block, search for the updated flight by its new flight number.
   - Verify that the flight was found.

   >**Note:** Most of the boilerplate code for creating a repository, saving the entity, and then locating the entity in a new repository can be found in the **DeleteFlight** test method in the **FlightActions** class.

#### Task 5: Use cross-repositories integration tests with System.Transactions

1. In the **FlightActions** class, locate the **UpdateUsingTwoRepositories** and inspect its code.

   - Locate the code that creates the Location and Flight repositories.
   - Each repository is created with a separate context, meaning each repository will use a separate transaction when saving changes.
   - Locate the code for loading and updating the flight and location objects.
   - Each entity is updated and saved in a separate transaction, but because both transactions are located in the same transaction scope, both transactions are not yet committed.

2. In the **UpdateUsingTwoRepositories** method,locate the query below the comment _//TODO: Lab 02, Exercise 2 Task 5.2: Review the query for the updated flight that is inside the transaction scope_.

   >**Note:** When querying from inside a transaction scope, you will get the updated values of entities, while other users, not participating in the transaction, will see the old values, until the transaction commits.

3. Locate the commented call to the **Complete** method.

   >**Note:** Without setting the transaction as complete, both transactions will roll back after the transaction scope closes.

4. Locate the query below the comment _//TODO: Lab 02, Exercise 2 Task 5.4 : Review the query for the updated flight that is outside the transaction scope_.

   >**Note:** After the transaction is rolled back, attempts to locate the updated entity will fail.

#### Task 6: Run the tests, and explore the database created by Entity framework

1. In the **TravelCompanionDatabaseInitializer** class, complete the implementation of the **Seed** method by adding the two reservations to the context and saving the changes.

    - Add the **reservation1** and **reservation2** variables to the **Reservations** collection of the context.
    - Call the **SaveChanges** method of the context.

2. Run all the tests in the solution and verify they pass.

    - To run all tests, open the **Test Explorer** window from the **Test** menu, and then click **Run All.**
    - Verify that all 16 methods have passed the test.

3. Open SQL Server Management Studio, connect to the **.\SQLEXPRESS** database server, then locate the **BlueYonder.Companion.Lab02** database in Object Explorer, and browse the tables that were created by Entity Framework.

   
   >**Results** : The Entity Framework data model works as designed and is verified by tests.


©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

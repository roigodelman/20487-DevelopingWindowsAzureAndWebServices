# Module 3: Creating and Consuming ASP. NET Web API Services

# Lesson 2: Creating an ASP. NET Web API Service

> Wherever  you see a path to file starting at *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487,
then the following path: [repository root]\AllFiles\20487C\Mod03 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod03

### Demonstration: Creating Your First ASP. NET Web API Service

#### Demonstration Steps

1. Open **Visual Studio 2017**
2. On the File menu, point to **New** , and then click **Project**.
3. In the **New Project** dialog box in the left-side of the navigation pane, expand the **Installed** node, expand the **Visual C#** node, click the **Web** node, and then click **ASP.NET Web Application (.NET Framework)** from the list of template.
4. In the **Name** text box, type **MyApp**.
5. In the **Location** text box, type **[repository root]\Allfiles\20487C\Mod03\Democode\FirstWebApiService\Begin** , and then click **OK**.
6. In the **New ASP.NET Web Application** dialog box, click **Web API** , and then click **OK**.
7. In **Solution Explorer** , under the **MyApp** project, expand the **App\_Start** folder, and then double-click **WebApiConfig.cs**.
8. Locate the **MapHttpRoute** method call and notice the different parameters that are used in the method call.
9. In **Solution Explorer** , under the **MyApp** project, expand the **Controllers** folder, and then double-click **ValuesController.cs**.
10. Locate the parameterless **Get** action method, and notice how this method can be invoked by using HTTP (for example, using the _/api/values_ relative URI).
11. To start the application without debugging, press Ctrl+F5. The default browser will open (e.g. - Microsoft Edge).
12. In the address bar of the browser, append the **api/values** to the end of the address, and then press Enter.
13. Depending on the browser, you will see an array with two values, the formatting may change depending on the browser:
        - Google Chrome - XML
        - Microsoft Edge, Firefox - JSON
14. Return to Visual Studio 2017.
15. In the **ValuesController** class, locate the parameterless **Get** action method, and change its code to the following.

	```cs
        [ActionName("List")]  
        public IEnumerable<string> Get()  
        {  
              return new string[] { "value1", "value2" };  
        } 
```

16. Press Ctrl+S to save the changes.
17. In Solution Explorer, double-click **WebApiConfig.cs** under **App\_Start** folder.
18. Add the following code to the beginning of the **Register** method.

	```cs
        config.Routes.MapHttpRoute(  
             name: "ActionApi",  
             routeTemplate: "api/{controller}/{action}/{id}",  
             defaults: new { id = RouteParameter.Optional }  
        );
```

19. Press Ctrl+S to save the changes.
20. To start debugging the application, press **F5**.
21. In the address bar of the browser, append the **api/values/list** to the end of the address, and then press Enter.
22. You should see the same result as in step 13.

# Lesson 3: Handling HTTP Requests and Responses

### Demonstration: Throwing Exceptions

#### Demonstration Steps

1. Open **Visual Studio 2017**
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. In the **File Name** text box, type **[repository root]\Allfiles\20487C\Mod03\Democode\ThrowHttpResponseException\Begin\start.sln** , and then click **Open**.
4. In **Solution Explorer** , under the **Start** project, expand the **Controllers** folder, and double-click **DestinationsController.cs**.
5. Locate the **Get** method call and notice how **HttpResponseMessage** is used to control errors.
6. Change the signature of the **Get** method to the following signature.

	```cs
        public Destination Get(int id)
```        
    
7. Remove the if-else statement and return the **destination** variable. When you are finished, the **Get** method should look like this.
 
	```cs
        public Destination Get(int id)  
        {  
           var destination = _destinations.Where(d=>d.Id == id).FirstOrDefault();  
           return destination;  
        }
```

8. Discuss what should be the result of such a call (404 not found status). Notice that the string **null** is being returned instead (with 200 OK status).
9. Change the **Get** method to validate the existence of a destination before sending a response. When you are finished, the **Get** method should look like this.

	```cs
        public Destination Get(int id)  
        {  
           var destination = _destinations.Where(d=>d.Id == id).FirstOrDefault();    
       
           if (destination == null)    
              throw new HttpResponseException(    
                 new HttpResponseMessage(HttpStatusCode.NotFound));    
           return destination;    
        }    
    ```

10. Press Ctrl+S to save the changes.
11. To start the application without debugging, press Ctrl+F5. The Internet Explorer 10 browser will open.
12. In the address bar of the browser, append the **api/destinations/1** to the end of the address, and then press Enter.
13. When prompted by the browser, click **Open** , and open the file in Notepad. Verify that you see information for Seattle.
14. Close Notepad and return to the browser. Append **api/destinations/6** to the end of the address, and then press Enter. Verify that you get an HTTP 404 response.

# Lesson 4: Hosting and Consuming ASP.NET Web API Services

### Demonstration 1: Consuming Services Using JQuery
     
#### Demonstration Steps

1. Open **Visual Studio 2017**.
2. On the File menu, point to **Open** , and then click **Project/Solution**.
3. In the **File Name** box, type **[repository root]\Allfiles\20487C\Mod03\Democode\ConsumingFromJQuery\Begin\JQueryClient\JQueryClient.sln** , and then click **Open**.
4. In **Solution Explorer** , under the **JQueryClient** project, expand the **Views** folder, then expand the **Home** folder and double-click **Index.cshtml**.
5. Locate the script section and notice that jQuery retrieves the data from the server and how this method of work enables AJAX.
6. In the beginning of the script section, add jQuery code to register to the submit event of the **deleteLocation** form. The registration code should look as follows.

	```cs
        $("#deleteLocation").submit(function (event) {  
              // this prevents the form from submitting  
            event.preventDefault();  
        });  
```

7. Add code retrieving the value of the **LocationId** input, and then create a delete call to the destination resource by using the jQuery **AJAX** function. The registration code should look as follows.

	```cs
        $("#deleteLocation").submit(function (event) {  
               // this prevents the form from submitting  
            event.preventDefault();    
            
            var desId = $(this).find('input[name="LocationId"]').val();  
       
            $.ajax({  
                type: 'DELETE',  
                url: 'destinations/' + desId  
            });  
        });  
``` 

8. Press Ctrl+S to save the changes.
9. In **Solution Explorer** , under the **JQueryClient** project, expand the **Controllers** folder and double-click **DestinationsController.cs**.
10. Locate the **Delete** method, and then click it. Press F9 to put a breakpoint on the **Delete** method.
11. Press F5 to debug the application. After the browser opens, show that the page lists the destinations retrieved from the **Get** method of the DestinationsAPI controller.
12. Type **1** in the **Location id** box, and then click **delete**.
13. Show that the breakpoint in the **Delete** method is reached.
14. Press Shift+F5 to stop the debugger.

### Demonstration 2: Consuming Services Using HttpClient

#### Demonstration Steps

1. Oepn **Visual Studio 2017**
2. On the **File** menu, point to **Open** , and then click **Project/Solution.**
3. In the **File Name** text box, type  **[repository root]\Allfiles\20487C\Mod03\Democode\ConsumingFromHttpClient\Begin\HttpClientApplication\HttpClientApplication.sln** ,and then click **Open**.
4. In **Solution Explorer** , under the **HttpClientApplication.Client** project, right-click **References** , and then click **Manage NuGet Packages**
5. Inside the **NuGet Manager** window, click **Online** on the left menu, and enter **Microsoft.AspNet.WebApi.Client** in the search box on the upper-right side.
6. In the search results list, locate the **Microsoft ASP.NET Web API 2.2 Client Libraries** ,and then click **Install**.
7. If a **License Acceptance** dialog box appears, click **I Accept**.
8. After the installation is complete, click **Close** to close the **NuGet Manager** window.
9. In **Solution Explorer** , under the **HttpClientApplication.Client** project, double-click **Program.cs.**
10. Locate the **CallServer** method and code to send a GET request for the destinations resource and print the responses content as string. As soon as you are finished, the **CallServer** method should look as follows.

	```cs
        static async Task CallServer()  
        {  
             var client = new HttpClient  
             {  
                  BaseAddress = new Uri("http://localhost:12534/")  
             };  
             HttpResponseMessage message = await client.GetAsync("api/Destinations");  
             var res = await message.Content.ReadAsStringAsync();  
             Console.WriteLine(res);  
        }  
```

11. Add code to deserialize the request content into a **List&lt;Destinations&gt;** by using the **HttpContent.ReadAsAsync&lt;T&gt;** method. As soon as you are finished, the **CallServer** method should look like this.

	```cs
        static async Task CallServer()  
        {  
              var client = new HttpClient  
              {  
                     BaseAddress = new Uri("http://localhost:12534/")  
              };  
              
              HttpResponseMessage message = await client.GetAsync("api/Destinations");  
              var res = await message.Content.ReadAsStringAsync();  
              Console.WriteLine(res);  
              
              var destinations = await message.Content.ReadAsAsync<List<Destination>>();  
              Console.WriteLine(destinations.Count);  
        }
```

12. Press Ctrl+S to save the changes.
13. In Solution Explorer, right-click the **HttpClientApplication.Host** project, and then click **Set as StartUp Project**.
14. Press Ctrl+F5 to start the server application without debugging.
15. In Solution Explorer, under **HttpClientApplication.Client** project, double-click **Program.cs**.
16. In the **Program** class, locate the **client** variable definition at the beginning of the **CallServer** method, and then click it. To put a breakpoint in that line, press **F9**.
17. In Solution Explorer, right-click the **HttpClientApplication.Client** project, point to **Debug** , and then click **Start new instance** , and wait for a program to hit your breakpoint.
18. To execute the **CallServer** method line-by-line, press **F10**.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

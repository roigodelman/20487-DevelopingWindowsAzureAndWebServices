# Module 4 - Extending and Securing ASP. NET Web API Services

# Lesson 1 - The ASP. NET Web API Pipeline

### Demonstration 1: The Flow of Requests and Responses Through the Pipeline

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\RequestResponseFlow\begin\RequestResponseFlow**.
4. Select the **RequestResponseFlow.sln**, file and then click **Open**.
5. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, right-click the **Extensions** folder, point to **Add**, and then click **New Item**.
6. In the **Add New Item** dialog box, in the pane on the left side, expand the **Installed** node, expand the **Visual C#** node, click the **Code** node, and then click **Class** in the list of items.
7. In the **Name** box, type **TraceHandler.cs**, and then click **Add**.
8. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, expand the **Extensions** folder, and then double-click  **TraceHandler.cs**.
9. Add the following by using directives at the top of the **TraceHandler.cs** file.

	```cs
        using System.Diagnostics;
        using System.Net.Http;
        using System.Threading;
        using System.Threading.Tasks;
	```
10. To derive from the **DelegatingHandler** base class, use the following code to change the class declaration of the **TraceHandler** class.

	```cs
        public class TraceHandler : DelegatingHandler
	```
11. Use the following code to override the **SendAsync** method.

	```cs
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
        }
	```
12. Use the following code to implement the **SendAsync** method.
	```cs
        Trace.WriteLine("Trace Handler start");
        Trace.WriteLine(request);

        var response = await base.SendAsync(request, cancellationToken);

        Trace.WriteLine(response);
        Trace.WriteLine("Trace Handler end");

        return response;
	```
    The **System.Debug.Trace** class is used to write **HttpRequestMessage** before calling the  **base.SendAsync** method, which calls the next handler in the message handlers stack. Additionally, the **System.Debug.Trace** class is used write **HttpResponseMessage** after the **base.SendAsync** method completes.
    
13. In the **SendAsync** method, right-click the first line of code, point to **Breakpoint**, and then click **Insert Breakpoint**.
14. To save the file, press CTRL+S.
15. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, expand the **App_Start** folder, and then double-click the  **WebApiConfig.cs** file.
16. Add the following by using the directive at the top of the **WebApiConfig.cs** file.

	```cs
        using RequestResponseFlow.Web.Extensions;
	```
17. In the **Register** method, add a new instance of the **TraceHandler** class to the **config.MessageHandlers** property by using the following code.

	```cs
        config.MessageHandlers.Add(new TraceHandler());
	```
    ASP.NET Web API will add the new trace handler to the host handlers stack, affecting every service call.
18. To save the file, press CTRL+S.
19. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, right-click the **Extensions** folder, point to **Add**, and then click **New Item**.
20. In the pane on the left side of the **Add New Item** dialog box, expand the **Installed** node, expand the **Visual C#** node, click the **Code** node, and then click **Class** in the list of items.
21. In the **Name** box, type **TraceFilterAttribute.cs**, and then click **Add**.
22. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, expand the **Extensions** folder, and then double-click the  **TraceFilterAttribute.cs** file.
23. Add the following by using the directives at the top of the **TraceFilterAttribute.cs** file.

	```cs
        using System.Diagnostics;
        using System.Net.Http;
        using System.Threading;
        using System.Threading.Tasks;
        using System.Web;
        using System.Web.Http.Controllers;
        using System.Web.Http.Filters;
	```
24. Use the following code to change the class declaration of the **TraceFilterAttribute** class to derive from the **Attribute** base class and implement the **IActionFilter** interface.

	```cs
        public class TraceFilterAttribute : Attribute, IActionFilter
	```
25. Use the following code to declare the **ExecuteActionFilterAsync** method.

	```cs
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
        }
	```
26. Use the following code to implement the **ExecuteActionFilterAsync** method.

	```cs
        Trace.WriteLine("Trace filter start");

        foreach (var item in actionContext.ActionArguments.Keys)
                Trace.WriteLine(string.Format("{0}: {1}", item, actionContext.ActionArguments[item]));
        
        var response = await continuation();

        Trace.WriteLine(string.Format("Trace filter response: {0}", response));
        return response;
	```
    The **System.Debug.Trace** class is used to write the action arguments from the **actionContext.ActionArguments** property before calling the **continuation** delegate, and to write **HttpResponseMessage** after the **continuation** delegate returns a response.

    The **AllowMultiple** property needs to be implemented too because this indicates if the attribute can be applied multiple times on the same action or controller.
    
27. Implement the **AllowMultiple** property by adding the following code to the class.

	```cs
        public bool AllowMultiple
        {
            get { return true; }
        }
	```
28. To save the file, press CTRL+S.
29. To set a breakpoint, place the cursor on the first line of the **ExecuteActionFilterAsync** method and then press F9.
30. In **Solution Explorer**, under the **RequestResponseFlow.Web** project, expand the **Controllers** folder, and then double-click the **ValuesController.cs** file.
31. Add the following by using the directive at the top of the **ValuesController.cs** file.

	```cs
        using RequestResponseFlow.Web.Extensions;
	```
32. To change the **ValuesController** class declaration, use the following code to decorate **ValuesController** with the **TraceFilter** attribute.

	```cs
        [TraceFilter]
        public class ValuesController : ApiController
	```
    Applying this attribute on a class indicates to **ApiController** that the **TraceFilterAttribute** action filter needs to be executed for every action in the **ValuesController** class.
33. To save the file, press Ctrl+S.
34. To start debugging the application, press F5.
35. In the browser, wait for the application to load, and then append the **api/values/** suffix to the address bar and press Enter.
36. Return to Visual Studio 2017.
37. On the **Debug** menu, point to **Windows**, and then click **Call Stack**.
38. Right-click the **Call Stack** pane, and make sure that the **Show External Code** option is selected.
39. Review the lines in the **Call Stack** that display the calls to **HttpControllerHandler**, **HttpServer**, and  **DelegatingHandler**.
40. To continue debugging, press F5 When the debugger breaks inside **TraceFilterAttribute**, review the lines executed by the **ApiController** class.
41. To stop the debugger, press Shift+F5.

### Demonstration 2: Creating Asynchronous Actions


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\AsynchronousActions\begin\AsynchronousActions**.
4. Select the **AsynchronousActions.sln** file and then click **Open**.
5. In **Solution Explorer**, expand the **AsynchronousActions.Web** project, then expand **Controllers**, and then double-click **CountriesController.cs**.  

   The **GetCountries** method synchronously retrieves a list of countries from another web service. To better utilize the thread pool, the **Get** method and the **GetCountries** method should both run asynchronously.
   
6. Locate the **GetCountries** method and change its declaration to the following code.

	```cs
        private async Task<XDocument> GetCountries()
	```
7. Replace the first line of code in the **GetCountries** method with the following code.

	```cs
        var client = new HttpClient();
	```
8. Add the following **using** directive to the beginning of the file.

	```cs
        using System.Net.Http.Headers;
	```
9. Replace the second line of code in the **GetCountries** method with the following code.

	```cs
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
	```
10. Replace the third line of code in the **GetCountries** method with the following code.

	```cs
        var response = await client.GetAsync("http://localhost:8371/api/countries");
	```
11. Replace the fourth line of code in the **GetCountries** method with the following code.

	```cs
        var document = XDocument.Load(await response.Content.ReadAsStreamAsync());
	```
12. In the **Get** method, add the **await** keyword before calling the **GetCountries** method. The resultant code should resemble the following.

	```cs
        var result = await GetCountries();
	```
13. Change the declaration of the **Get** method to the following code.

	```cs
        public async Task<IEnumerable<string>> Get()
	```
14. To save the changes, press Ctrl+S.
15. In **Solution Explorer**, right-click the root solution node, and then click **Properties**.
16. In the **Solution &#39;AsynchronousActions&#39; Property Pages** dialog box, select the **Multiple startup projects** option.
17. In the projects list, click the **Action** cell of every project, and then select **Start**.
18. Click the **DataServices** project line, and then click the up arrow button to start the project before the  **AsynchronousActions.Web** project.
19. Click **OK**.
20. To start both the projects without debugging, press Ctrl+F5.
21. In the browser, notice the XML containing the list of countries.
22. Close the browser.

### Demonstration 3: Returning Images by Using Media Type Formatters


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\ImagesWithMediaTypeFormatter**.
4. Select the **ImagesWithMediaTypeFormatter.sln** file, and then click **Open**.
5. In **Solution Explorer**, expand the **ImagesWithMediaTypeFormatter.Host** project, then expand the **Controllers** folder, and then double-click **ValuesController.cs**.
6. Review the contents of the **ValuesController** class. The controller handles **Value** objects.
7. Notice the **Value** class. The **[IgnoreDataMember]** attribute prevents the serialization of the **Thumbnail** property.
8. In **Solution Explorer**, under the **ImagesWithMediaTypeFormatter.Host** project, expand the **Formatters** folder, and then double-click **ImageFormatter.cs**.
9. Locate the **ImageFormatter** constructor. The **SupportedMediaTypes** collection contains the mime types supported by the media type formatter.
10. Locate the **CanWriteType** method. The method controls when the formatter is used to change the output. The method will return true when the response contains a **Value** object.
11. Locate the **WriteToStream** method. The method uses the **Thumbnail** property of the **Value** object to locate the image file and return it instead of the object returned by the controller&#39;s action.
12. In **Solution Explorer**, under the **ImagesWithMediaTypeFormatter.Host** project, under the **Formatters** folder, double-click  **UriFormatHandler.cs**.
13. Review the code of the **SendAsync** method. The method checks the request&#39;s URL. If the extension in the URL matches one of the image types, the extension is removed from the URL, a matching mime type is added to the request, and the request is sent to the next component in the pipeline.
14. To start the web application without debugging, press Ctrl+F5.
15. To open the developer tools window, in the browser, press F12.
16. In the developer tools window, click the **Network** tab.
17. On the **Network** tab, click **Start capturing**.
18. On the web page, type **2** in the text box, and then click **Get default**.
19. On the **Network** tab, click **Go to detailed view**.
20. On the **Request headers** tab, notice that the **Accept** header is set to **&ast;/&ast;**.
21. Click the **Response headers** tab and notice that the **Content-Type** is set to **application/json; charset=utf-8**. The default content type of ASP.NET Web API is JSON.
22. Click **Clear**.
23. Click **Get JSON**.
24. On the **Network** tab, click **Go to detailed view**.
25. On the **Request headers** tab, notice that the **Accept** header contains the string **application/json**.
26. Click the **Response headers** tab and notice that **Content-Type** is set to **application/json; charset=utf-8**.
27. Click the **Response body** tab and review the JSON string. The **Thumbnail** property is not present because it was omitted from the serialization.
28. Click **Clear**.
29. Click **Get XML**.
30. On the **Network** tab, click **Go to detailed view**.
31. On the **Request headers** tab, notice that the **Accept** header contains the string **application/xml**.
32. Click the **Response headers** tab and notice that **Content-Type** is set to **application/xml; charset=utf-8**.
33. Click the **Response body** tab and review the XML string.
34. Click **Clear**.
35. Click **Get image**.
36. On the **Network** tab, click **Go to detailed view**.
37. On the **Request headers** tab, notice that the **Accept** header contains the string **image/png**.
38. Click the **Response headers** tab and notice that **Content-Type** is set to **image/png**.
39. Click the **Response body** tab and review the image with the number two.
40. Click **Clear**.
41. To close the developer tools window, press F12.

# Lesson 2: Creating OData Services

### Demonstration: Creating and Consuming an OData Services


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\ODataService\begin\ConsumingODataService**.
4. Select the **ConsumingODataService.sln** file, and then click **Open**.
5. In **Solution Explorer**, under the **ConsumingODataService.Host** project, expand the **Controllers** folder node, and then double-click the **CoursesController.cs** file.
6. Review the **Get** action, which returns **IQueryable&lt;Course&gt;** and is also decorated with the **[Queryable]** attribute.
7. This is done to enable OData queries.  
   The **CoursesController** class derives from the **ODataController** base class, which handles the formatting.   
8. In **Solution Explorer**, under the **ConsumingODataService.Host** project, double-click the **Global.asax** file.
9. Review the content of the **SetupOData** method.  
   The **ODataConventionModelBuilder** class is used to create an entity data model, which will be used to create the OData metadata. The **MapODataRoute** method is used to create a new route that exposes the OData metadata and the various controllers in the model.
10. In **Solution Explorer**, right-click the **ConsumingODataService.Host** project, and then click **Set as StartUp Project**.
11. To start the project without debugging, press Ctrl+F5 .
12. Return to Visual Studio 2017.
13. In **Solution Explorer**, under the **ODataService.Client** project, right-click the **references** node and then select **Add Service Reference**.
14. In the **Address** text box, type **http://localhost:57371/OData**, and then click **Go**.

    >**Note** : OData URLs are case-sensitive. Use the casing as shown in the instruction.

15. In the **Namespace** text box, type **OData**, and then click **OK**.
16. In **Solution Explorer**, under the **ODataService.Client** project, double-click the **Program.cs** file.
17. Use the following code to in the **Main** method, to create a new instance of the **OData.Container** class.

	```cs
        var container = new OData.Container(new Uri("http://localhost:57371/OData"));
	```
    >**Note** : OData URLs are case-sensitive. Use the casing as shown in the instruction.

18. Use the following code to create a LINQ query to select the WCF course from the container&#39;s **Courses** property.

	```cs
        var course = (from c in container.Courses
               where c.Name == "WCF"
               select c).FirstOrDefault();
	```
19. Use the following code to print the name and ID of the course.

	```cs
        Console.WriteLine("the course {0} has the Id: {1}", course.Name, course.Id);
        Console.ReadKey();
	```
20. To save the changes, press Ctrl+S .
21. In **Solution Explorer**, right-click the **ODataService.Client** project, point to **Debug**, and then click **Start New Instance** to run the client application.
22. Notice how the query returns data about the WCF course.  

    The application transforms the LINQ query to an OData service call to the server. The server then uses OData querying in CoursesController to query the database.

# Lesson 3: Implementing Security in ASP.NET Web API Services

### Demonstration: Creating Secured ASP.NET Web API Services


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\WebAPISecurity**.
4. Select the **WebAPISecurity.sln** file, and then click **Open**.
5. In **Solution Explorer**, under the **WebAPISecurity** project, double-click **AuthenticationMessageHandler.cs**.
6. Locate the **SendAsync** method, and review its code. First the method checks if the request contains _Basic_ authentication information by checking the **HttpRequestMessage.Headers.Authorization.Scheme** property. If the request does not contain the _Authorization_ header, it is sent to the next handler without checking.

   >**Note**: The authentication handler does not require all the requests to contain the authentication information. This is because some actions in this demonstration need to be accessible to anonymous users.

7. Review the code in the first **if** statement. If the request contains _Basic_ authentication information, then the code retrieves the identity from the HTTP _Authorization_ header, parses it into the username and password, and then sends the identity to be verified in the **AuthenticateUser** method. If the authentication fails, an _Unauthorized_ response is sent back to the client.

   >**Note**: In _Basic_ authentication, the username and password are encoded to a single Base64 string.

8. In the **SendAsync** method, review the code in the last **if** statement. An action can return an unauthorized response if it requires authentication and the user did not supply it, or if it requires the user to have a specific role, which the user does not have. If an unauthorized response is returned from the action, the code will add the _Basic_ authentication type to notify the client of the expected authentication type.
9. Locate the **AuthenticateUser** method, and review its code. After the identity is authenticated, the code creates the  **GenericIdentity** and the **GenericPrincipal** objects to identify the user and its roles. The principal is then attached to the **Thread.CurrentPrincipal** property to have it available for the authorization process.
10. In **Solution Explorer**, under the **WebAPISecurity** project, expand **Controllers**, and then double-click the  **ValuesController.cs** file.  

    You need to understand the use of the **[Authorize]** and the **[AllowAnonymous]** attributes. The **[Authorize]** attribute, which decorates the controller, verifies that the client was authenticated before invoking the controller&#39;s actions. The **[AllowAnonymous]** attribute decorating the second **Get** method skips the authentication check, allowing anonymous users to invoke the decorated action.
    
11. In **Solution Explorer**, under the **WebAPISecurity** project, expand **App_Start**, and then double-click **WebApiConfig.cs**.
12. Locate the **Register** method, and review the **MessageHandler.Add** method call. This is how the authentication message handler is attached to the message handling pipeline.
13. To start the application without debugging, press Ctrl+F5.
14. In the browser, append the **api/values/1** suffix to the address bar and press Enter. Verify that you can see an XML reply with the response of the action.
15. Remove **/1** from the address in the address bar and then press Enter.
16. In the **Windows Security** dialog box, type the following information:

   * User name: **Admin**
   * Password: **Admin1**

17. Click **OK**. Verify that the dialog box reappears.
18. Type the following information in the **Windows Security** dialog box:

   * User name: **Admin**
   * Password: **Admin**

19. Click **OK**. Verify that you can see an XML reply with the response of the action.
20. Close the browser.

# Lesson 4: Injecting Dependencies into Controllers

### Demonstration: Creating a Dependency Resolver


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\DependencyResolver**.
4. Select the **DependencyResolver.sln** file, and then click **Open**.
5. In **Solution Explorer**, under the **DependencyResolver** project, expand **Controllers**, and then double-click  **CoursesController.cs**.
6. Review the **Get** method. The method uses the **__context_** private member, which is initialized in the constructor.
7. Review the **CoursesController** constructor. The constructor receives the context as an **ISchoolContext** interface to decouple the controller from a specific implementation of the interface.
8. In **Solution Explorer**, under the **DependencyResolver** project, expand **Infrastracture**, and then double-click  **ManualDependencyResolver.cs.**
9. Review the **GetService** method. The method returns an instance of a specific service based on the **serviceType** parameter.
10. In **Solution Explorer**, under the **DependencyResolver** project, expand **App_Start**, and then double-click **WebApiConfig.cs**.
11. Review the **Register** method. The dependency resolver that will be used by ASP.NET Web API is the one that is set in the **config.DependencyResolver** property.
12. To start the project without debugging, press Ctrl+F5 .
13. To call the **Get** action of the **CoursesController** class, in the browser window, append the **api/courses** relative address to the address bar, and then press Enter.

©2018 Microsoft Corporation. All rights reserved.

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

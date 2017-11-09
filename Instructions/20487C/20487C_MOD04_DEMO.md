# Module 4 - Extending and Securing ASP. NET Web API Services

# Lesson 1 - The ASP. NET Web API Pipeline

### Demonstration 1: The Flow of Requests and Responses Through the Pipeline

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\RequestResponseFlow\begin\RequestResponseFlow**.
4. Select the file **RequestResponseFlow.sln**, and then click **Open**.
5. In Solution Explorer, under the **RequestResponseFlow.Web** project, right-click the **Extensions** folder. Point to **Add** , and click **New Item**.
6. In the **Add New Item** dialog box, in the pane on the left side, expand the **Installed** node, expand the **Visual C#** node, click the **Code** node, and then click **Class** in the list of items.
7. In the **Name** box, type **TraceHandler.cs** , and then click **Add**
8. In Solution Explorer, under the **RequestResponseFlow.Web** project, expand the **Extensions** folder and double-click  **TraceHandler.cs**.
9. Add the following by using directives at the top of the **TraceHandler.cs** file.

	```cs
        using System.Diagnostics;
        using System.Net.Http;
        using System.Threading;
        using System.Threading.Tasks;
	```
10. To derive from the **DelegatingHandler** base class, change the class declaration of the **TraceHandler** class by using the following code.

	```cs
        public class TraceHandler : DelegatingHandler
	```
11. Override the **SendAsync** method by using the following code.

	```cs
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
        }
	```
12. Implement the **SendAsync** method by using the following code.

	```cs
        Trace.WriteLine("Trace Handler start");
        Trace.WriteLine(request);

        var response = await base.SendAsync(request, cancellationToken);

        Trace.WriteLine(response);
        Trace.WriteLine("Trace Handler end");

        return response;
	```
13. The **System.Debug.Trace** class is used to write the **HttpRequestMessage** before calling the  **base.SendAsync** method, which calls the next  handler in the message handlers stack, and that the **System.Debug.Trace** class is also used write the **HttpResponseMessage** after the **base.SendAsync** method completes.
14. In the **SendAsync** method, right-click the first line of code, point to **Breakpoint** , and then click **Insert Breakpoint**.
15. Press CTRL+S to save the file.
16. In Solution Explorer, under the **RequestResponseFlow.Web** project, expand the **App_Start** folder. Double-click the  **WebApiConfig.cs** file.
17. Add the following by using the directive at the top of the **WebApiConfig.cs** file.

	```cs
        using RequestResponseFlow.Web.Extensions;
	```
18. In the **Register** method, add a new instance of the **TraceHandler** class to the **config.MessageHandlers** property by using the following code.

	```cs
        config.MessageHandlers.Add(new TraceHandler());
	```
19. ASP.NET Web API will add the new trace handler to the host handlers stack, affecting every service call.
20. Press CTRL+S to save the file.
21. In Solution Explorer, under the **RequestResponseFlow.Web** project, right-click the **Extensions** folder. Point to **Add** and click **New Item**.
22. In the **Add New Item** dialog box, in the pane on the left side, expand the **Installed** node, expand the **Visual C#** node, click the **Code** node, and then click **Class** in the list of items.
23. In the **Name** box, type **TraceFilterAttribute.cs** , and then click **Add**.
24. In Solution Explorer, under the **RequestResponseFlow.Web** project, expand the **Extensions** folder. Double-click the  **TraceFilterAttribute.cs** file.
25. Add the following by using the directives at the top of the **TraceFilterAttribute.cs** file.

	```cs
        using System.Diagnostics;
        using System.Net.Http;
        using System.Threading;
        using System.Threading.Tasks;
        using System.Web;
        using System.Web.Http.Controllers;
        using System.Web.Http.Filters;
	```
26. Change the class declaration of the **TraceFilterAttribute** class to derive from the **Attribute** base class and implement the  **IActionFilter** interface by using the following code.

	```cs
        public class TraceFilterAttribute : Attribute, IActionFilter
	```
27. Declare the **ExecuteActionFilterAsync** method by using the following code.

	```cs
        public async Task<HttpResponseMessage> ExecuteActionFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken, Func<Task<HttpResponseMessage>> continuation)
        {
        }
	```
28. Implement the **ExecuteActionFilterAsync** method by using the following code.

	```cs
        Trace.WriteLine("Trace filter start");

        foreach (var item in actionContext.ActionArguments.Keys)
                Trace.WriteLine(string.Format("{0}: {1}", item, actionContext.ActionArguments[item]));
        
        var response = await continuation();

        Trace.WriteLine(string.Format("Trace filter response: {0}", response));
        return response;
	```
29. The **System.Debug.Trace** class is used to write the action arguments from the  **actionContext.ActionArguments** property before calling the **continuation** delegate, and to write the **HttpResponseMessage** after the **continuation** delegate returns a response.
30. The **AllowMultiple** property needs to be implemented too because this indicates if the attribute can be applied multiple times on the same action or controller.
31. Implement the **AllowMultiple** property by adding the following code to the class.

	```cs
        public bool AllowMultiple
        {
            get { return true; }
        }
	```
32. Press CTRL+S to save the file.
33. Place the cursor on the first line of the **ExecuteActionFilterAsync** method and press F9 to set a breakpoint.
34. In Solution Explorer, under the **RequestResponseFlow.Web** project, expand the **Controllers** folder and double-click the  **ValuesController.cs**.
35. Add the following by using the directive at the top of the **ValuesController.cs** file.

	```cs
        using RequestResponseFlow.Web.Extensions;
	```
36. Change the **ValuesController** class declaration. Decorate **ValuesController** with the **TraceFilter** attribute by using the following code.

	```cs
        [TraceFilter]
        public class ValuesController : ApiController
	```
37. Applying this attribute on a class indicates to **ApiController** that the **TraceFilterAttribute** action filter needs to be executed for every action in the **ValuesController** class.
38. Press Ctrl+S to save the file.
39. Press F5 to start debugging the application.
40. In the browser, wait for the application to load, and then append the suffix **api/values/** to the address bar and press Enter.
41. Return to Visual Studio 2017.
42. On the **Debug** menu, point to **Windows**, and then click **Call Stack**.
43. Right-click the **Call Stack** pane, and make sure that the **Show External Code** option is selected.
44. Review the lines in the **Call Stack** that display the calls of the **HttpControllerHandler**, **HttpServer**, and  **DelegatingHandler**.
45. Press F5 to continue debugging. When the debugger breaks inside **TraceFilterAttribute**, review the lines executed by the **ApiController** class.
46. Press Shift+F5 to stop the debugger.

### Demonstration 2: Creating Asynchronous Actions


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\AsynchronousActions\begin\AsynchronousActions**.
4. Select the file **AsynchronousActions.sln** and then click **Open**.
5. In Solution Explorer, expand the **AsynchronousActions.Web** project, then expand **Controllers** , and then double-click  **CountriesController.cs**.
6. The **GetCountries** method synchronously retrieves a list of countries from another web service. To better utilize the thread pool, the **Get** method and the **GetCountries** method should both run asynchronously.
7. Locate the **GetCountries** method and change its declaration to the following code.

	```cs
        private async Task<XDocument> GetCountries()
	```
8. Replace the first line of code in the **GetCountries** method with the following code.

	```cs
        var client = new HttpClient();
	```
9. Add the following **using** directive to the beginning of the file.

	```cs
        using System.Net.Http.Headers;
	```
10. Replace the second line of code in the **GetCountries** method with the following code.

	```cs
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
	```
11. Replace the third line of code in the **GetCountries** method with the following code.

	```cs
        var response = await client.GetAsync("http://localhost:8371/api/countries");
	```
12. Replace the fourth line of code in the **GetCountries** method with the following code.

	```cs
        var document = XDocument.Load(await response.Content.ReadAsStreamAsync());
	```
13. In the **Get** method, add the **await** keyword before calling the **GetCountries** method. The resultant code should resemble the following.

	```cs
        var result = await GetCountries();
	```
14. Change the declaration of the **Get** method to the following code.

	```cs
        public async Task<IEnumerable<string>> Get()
	```
15. Press Ctrl+S to save the changes.
16. In Solution Explorer, right-click the root solution node, and then click **Properties**.
17. In the **Solution &#39;AsynchronousActions&#39; Property Pages** dialog box, select the **Multiple startup projects** option.
18. In the projects list, click the **Action** cell of every project, and select **Start**.
19. Click the **DataServices** project line, and then click the up arrow button to start the project before the  **AsynchronousActions.Web** project.
20. Click **OK**.
21. Press Ctrl+F5 to start both the projects without debugging.
22. In the browser, notice the XML containing the list of countries.
23. Close the browser.

### Demonstration 3: Returning Images by Using Media Type Formatters


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\ImagesWithMediaTypeFormatter**.
4. Select the file **ImagesWithMediaTypeFormatter.sln** and then click **Open**.
5. In Solution Explorer, expand the **ImagesWithMediaTypeFormatter.Host** project, then expand the **Controllers** folder, and then double-click **ValuesController.cs**.
6. Review the content of the **ValuesController** class. The controller handles **Value** objects.
7. Notice the **Value** class. The **[IgnoreDataMember]** attribute prevents the serialization of the **Thumbnail**  property.
8. In Solution Explorer, under the **ImagesWithMediaTypeFormatter.Host** project, expand the **Formatters** folder, and then double-click **ImageFormatter.cs**.
9. Locate the **ImageFormatter** constructor. The **SupportedMediaTypes** collection contains the mime types supported by the media type formatter.
10. Locate the **CanWriteType** method. The method controls when the formatter is used to change the output. The method will return true when the response contains a **Value** object.
11. Locate the **WriteToStream** method. The method uses the **Thumbnail** property of the **Value** object to locate the image file and return it instead of the object returned by the controller&#39;s action.
12. In Solution Explorer, under the **ImagesWithMediaTypeFormatter.Host** project, under the **Formatters** folder, double-click  **UriFormatHandler.cs**.
13. Review the code of the **SendAsync** method. The method checks the request&#39;s URL. If the extension in the URL matches one of the image types, the extension is removed from the URL, a matching mime type is added to the request, and the request is sent to the next component in the pipeline.
14. Press Ctrl+F5 to start the web application without debugging.
15. In the browser, press F12 to open the developer tools window.
16. In the developer tools window, click the **Network** tab.
17. On the **Network** tab, click **Start capturing**.
18. On the web page, type **2** in the text box, and then click **Get default**.
19. On the **Network** tab, click **Go to detailed view**.
20. On the **Request headers** tab, notice that the **Accept** header is set to ***/***.
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
41. Press F12 to close the developer tools window.

# Lesson 2: Creating OData Services

### Demonstration: Creating and Consuming an OData Services


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\ODataService\begin\ConsumingODataService**.
4. Select the file **ConsumingODataService.sln**, and then click **Open**.
5. In Solution Explorer, under the **ConsumingODataService.Host** project, expand the **Controllers** folder node, and double-click the **CoursesController.cs** file.
6. Review the **Get** action, which returns **IQueryable&lt;Course&gt;** and is also decorated with the **[Queryable]** attribute.
7. This is done to enable OData queries.
8. The **CoursesController** class derives from the **ODataController** base class, which handles the formatting.
9. In Solution Explorer, under the **ConsumingODataService.Host** project, double-click the **Global.asax** file.
10. Review the content of the **SetupOData** method. The **ODataConventionModelBuilder** class is used to create an entity data model, which will be used to create the OData metadata. The **MapODataRoute** method is used to create a new route that exposes the OData metadata and the various controllers in the model.
11. In Solution Explorer, right-click the **ConsumingODataService.Host** project and click **Set as StartUp Project**.
12. Press Ctrl+F5 to start the project without debugging.
13. Return to Visual Studio 2017.
14. In Solution Explorer, under the **ODataService.Client** project, right-click the **references** node and select **Add Service Reference**.
15. In the **Address** text box, type **http://localhost:57371/OData** and click the **Go** button.

  >**Note** : OData URLs are case-sensitive. Use the casing as shown in the instruction.

16. In the **Namespace** text box, type **OData** and click **OK**.
17. In Solution Explorer, under the **ODataService.Client** project, double-click the **Program.cs** file.
18. In the **Main** method, create a new instance of the **OData.Container** class by using the following code.

	```cs
        var container = new OData.Container(new Uri("http://localhost:57371/OData"));
	```
  >**Note** : OData URLs are case-sensitive. Use the casing as shown in the instruction.

19. Use a LINQ query to select the WCF course from the container&#39;s **Courses** property by using the following code.

	```cs
        var course = (from c in container.Courses
               where c.Name == "WCF"
               select c).FirstOrDefault();
	```
20. Print the name and ID of the course by using the following code.

	```cs
        Console.WriteLine("the course {0} has the Id: {1}", course.Name, course.Id);
        Console.ReadKey();
	```
21. Press Ctrl+S to save the changes.
22. In Solution Explorer, right-click the **ODataService.Client** project, point to **Debug** and click **Start New Instance** to run the client application.
23. Notice how the query returns data about the WCF course.
24. The application transforms the LINQ query to an OData service call to the server. The server then uses OData querying in CoursesController to query the database.

# Lesson 3: Implementing Security in ASP.NET Web API Services

### Demonstration: Creating Secured ASP.NET Web API Services


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\WebAPISecurity**.
4. Select the file **WebAPISecurity.sln** and then click **Open**.
5. In Solution Explorer, under the **WebAPISecurity** project, double-click **AuthenticationMessageHandler.cs**.
6. Locate the **SendAsync** method, and review its code. First the method checks if the request contains _Basic_ authentication information by checking the **HttpRequestMessage.Headers.Authorization.Scheme** property. If the request does not contain the _Authorization_ header, it is sent to the next handler without checking.

  >**Note** : The authentication handler does not require all the requests to contain the authentication information. This is because some actions in this demonstration need to be accessible to anonymous users.

7. Review the code in the first **if** statement. If the request contains _Basic_ authentication information, then the code retrieves the identity from the HTTP _Authorization_ header, parses it into the username and password, and then sends the identity to be verified in the **AuthenticateUser** method. If the authentication fails, an _Unauthorized_ response is sent back to the client.

  >**Note** : In _Basic_ authentication, the username and password are encoded to a single Base64 string.

8. Review the code in the last **if** statement in the **SendAsync** method. An action can return an unauthorized response if it requires authentication and the user did not supply it, or if it requires the user to have a specific role, which the user does not have. If an unauthorized response is returned from the action, the code will add the _Basic_ authentication type to notify the client of the expected authentication type.
9. Locate the **AuthenticateUser** method, and review its code. After the identity is authenticated, the code creates **GenericIdentity** and **GenericPrincipal** objects to identify the user and its roles. The principal is then attached to the **Thread.CurrentPrincipal** property to have it available for the authorization process.
10. In Solution Explorer, under the **WebAPISecurity** project, expand **Controllers** , and then double-click **ValuesController.cs**.
11. You need to understand the use of the **[Authorize]** and **[AllowAnonymous]** attributes. The **[Authorize]** attribute, which decorates the controller, verifies that the client was authenticated before invoking the controller&#39;s actions. The **[AllowAnonymous]** attribute decorating the second **Get** method skips the authentication check, allowing anonymous users to invoke the decorated action.
12. In Solution Explorer, under the **WebAPISecurity** project, expand **App_Start** , and then double-click **WebApiConfig.cs**.
13. Locate the **Register** method, and review the **MessageHandler.Add** method call. This is how the authentication message handler is attached to the message handling pipeline.
14. Press Ctrl+F5 to start the application without debugging.
15. In the browser, append the suffix **api/values/1** to the address bar and press Enter. Verify that you can see an XML reply with the response of the action.
16. Remove **/1** from the address in the address bar and then press Enter.
17. In the **Windows Security** dialog box, type the following information:

 - User name: **Admin**
 - Password: **Admin1**

18. Click **OK**. Verify that the dialog box reappears.
19. Type the following information in the **Windows Security** dialog box:

 - User name: **Admin**
 - Password: **Admin**

20. Click **OK**. Verify that you can see an XML reply with the response of the action.
21. Close the browser.

# Lesson 4: Injecting Dependencies into Controllers

### Demonstration: Creating a Dependency Resolver


#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open** , and then click **Project/Solution**.
3. Browse to **D:\Allfiles\Mod04\DemoFiles\DependencyResolver**.
4. Select the file **DependencyResolver.sln** and then click **Open**.
5. In Solution Explorer, under the **DependencyResolver** project, expand **Controllers** , and then double-click  **CoursesController.cs**.
6. Review the **Get** method. The method uses the **_context** private member, which is initialized in the constructor.
7. Review the **CoursesController** constructor. The constructor receives the context as an **ISchoolContext** interface to decouple the controller from a specific implementation of the interface.
8. In Solution Explorer, under the **DependencyResolver** project, expand **Infrastracture** , and then double-click  **ManualDependencyResolver.cs.**
9. Review the **GetService** method. The method returns an instance of a specific service based on the  **serviceType** parameter.
10. In Solution Explorer, under the **DependencyResolver** project, expand **App_Start** , and then double-click **WebApiConfig.cs**.
11. Review the **Register** method. The dependency resolver that will be used by ASP.NET Web API is the one that is set in the **config.DependencyResolver** property.
12. Press Ctrl+F5 to start the project without debugging.
13. In the browser window, append the **api/courses** relative address to the address bar and press Enter to call the **Get** action of the **CoursesController** class.

©2016 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

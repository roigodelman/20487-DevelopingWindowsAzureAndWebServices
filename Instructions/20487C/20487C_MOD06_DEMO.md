# Module 6: Hosting Services

> Wherever  you see a path to a file that starts with *[repository root]*, replace it with the absolute path to the folder in which the 20487 repository resides.
> For example, if you cloned or extracted the 20487 repository to **C:\Users\John Doe\Downloads\20487**,
the following path: **[repository root]\AllFiles\20487C\Mod06** should be changed to **C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06**.

# Lesson 1: Hosting Services On-Premises

### Demonstration: How to Host WCF Services in IIS

#### Preparation Steps

You need to activate Microsoft Internet Information Services (IIS) on your machine. The instructions below are for Windows 10 only.

1. Open **Search Windows** by clicking the magnifying glass next to the **Start** menu.
2. Type **Turn windows features on or off**, and then press Enter.
3. Select the **Internet Information Services** check box once. A black square should appear within the white check box.
4. Expand **Internet Information Services**, and then expand **World Wide Web Services**.
5. Select the **Application Development Features** check box.
6. You also need to activate Windows Communication Foundation (WCF) support before you can deploy WCF services to IIS. To do this:  
	a) In the **Windows Features** window, expand **.NET Framework 4.7 Advances Services**.  
	b) Expand **WCF Services**, and then select all the check boxes inside it (**HTTP Activation**, **Message Queuing (MSMQ) Activation**, and so on.) 
7. Click **Ok**. Windows proceeds to install the required components. After this, IIS will is activated.
8. Verify that IIS works:  
	a) Open a browser.  
	b) Navigate to **http://localhost/**.  
	c) An **Internet Information Services** page should appear.

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **New**, and then click **Project**.
3. In navigation pane of the **New Project** dialog box, expand the **Installed** node, expand the **Templates** node, and then expand the **Visual C#** node.
4. Click the **WCF** node, and then from the list of templates, select **WCF Service Application**.
5. In the **Name** text box, type **MyIISService**.
6. In the **Location** text box, type **[repository root]\Allfiles\20487C\Mod06\DemoFiles\WCF**.
7. Clear the **Create directory for solution** check box, and then click **OK**.
8. In Solution Explorer, under the **MyIISService** project, double-click **web.config**.

    >**Note**: The reason there is no &lt;services&gt; section in the &lt;system.serviceModel&gt; is because IIS automatically defines the base address according to the virtual directory where the services are hosted, and the default endpoint configuration of WCF therefore does not require any specific configuration for each endpoint.

9. In Solution Explorer, under the **MyIISService** project, right-click **Service1.svc**, and then click **View Markup**.

    >**Note**: When IIS receives a request addressed to the .svc file, it uses the value of the **Service** attribute from the file to know which service it needs to call.

10. In Solution Explorer, click the **MyIISService** project node.
11. To build and debug the application, press F5. After the application is built, a browser opens.
12. In the Internet Explorer window, locate the address bar. To the base address of the service, add the absolute path: **Service1.svc?wsdl**, and then press Enter. Review the Web Services Description Language (WSDL) file.

    >**Note**: Lesson 4, &quot;Consuming WCF Services&quot; in Module 5, "Creating WCF Services" of Course 20487 explains how to use WSDL to generate service proxies.

13. Return to Visual Studio and stop the debugger by pressing Shift+F5.
14. Publish the service to IIS:
	a. In Solution Explorer, right-click the **MyIISService** project, and then click **Publish**. The **Publish** tab opens.
	b. Select **IIS, FTP, etc**, and then click **Publish**. The **Publish** window opens.
	c. In **Publish method**, select **Web Deploy**.
	d. In **Server**, type **localhost**.
	e. In **Site Name**, type **Default Web Site/MyIISService**.
	f. In **Destination URL**, type **http://localhost/MyIISService/Service1.svc?wsdl**.
	g. Click **Save**.  
	
    Visual Studio 2017 proceeds to build and deploy the service to IIS. When it's done, a browser automatically opens and navigates to the service WSDL.
    
15. Review the WCF service WSDL.

©2018 Microsoft Corporation. All rights reserved.

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

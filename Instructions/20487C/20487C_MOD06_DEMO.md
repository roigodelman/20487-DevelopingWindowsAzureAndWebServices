# Module 6: Hosting Services

> Wherever  you see a path to file starting at *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.
> e.g. - you cloned or extracted the 20487 repository to C:\Users\John Doe\Downloads\20487,
then the following path: [repository root]\AllFiles\20487C\Mod06 will become C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06

# Lesson 1: Hosting Services On-Premises

### Demonstration: How to Host WCF Services in IIS

#### Preparation steps

You will need to activate IIS on your machine. The instructions below are for Windows 10 only.

1. Open **Search Windows** by clicking the magnifying glass next to the **Start** menu.
2. Enter **Turn windows features on or off** and press **Enter**
3. Check **Internet Information Services** once, a black square should appear within the white check box.
4. Expand **Internet Information Services** and then expand **World Wide Web Services**
5. Check **Application Development Features**
6. We will also need to activate WCF support before we can deploy WCF services to IIS. To do this:
	- In the **Windows Features** window, expand **.NET Framework 4.7 Advances Services**
	- Expand **WCF Services** and check all the checkboxes inside it (**HTTP Activation**, **Message Queuing (MSMQ) Activation**, etc) 
6. Click **Ok**, Windows will proceed to install the required components, when it is done, IIS will be on.
7. Verify that IIS works:
	- Open a browser.
	- Navigate to **http://localhost/**
	- An **Internet Information Services** page should show up.

#### Demonstration Steps

1. On the Start screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **New** , and then click **Project**.
3. In the **New Project** dialog box, on the navigation pane, expand the **Installed** node, expand the **Templates** node, and then expand the **Visual C#** node.
4. Click the **WCF** node, and then select **WCF Service Application** from the list of templates.
5. In the **Name** text box, type **MyIISService**.
6. In the **Location** text box, type **[repository root]\Allfiles\20487C\Mod06\DemoFiles\WCF**.
7. Clear the **Create directory for solution** check box, and then click **OK**.
8. In Solution Explorer, under the **MyIISService** project, double-click **web.config**.

  >**Note** : The reason there is no &lt;services&gt; section in the &lt;system.serviceModel&gt; is because IIS automatically defines the base address according to the virtual directory where the services are hosted, and the default endpoint configuration of WCF therefore does not require any specific configuration for each endpoint.

9. In Solution Explorer, under the **MyIISService** project, right-click **Service1.svc** , and then click **View Markup**.

  >**Note** : When IIS receives a request addressed to the .svc file, it uses the value of the Service attribute from the file to know which service it needs to call.

10. In Solution Explorer, click the **MyIISService** project node.
11. To build and debug the application, press F5. After the application is built, a browser opens.
12. In the Internet Explorer window, locate the address bar. To the base address of the service, add the absolute path: **Service1.svc?wsdl**, and then press Enter. Review the WSDL file.

  >**Note** : Module 5, &quot;Creating WCF Services&quot; and Lesson 4, &quot;Consuming WCF Services&quot; in Course 20487 explains how to use WSDL to generate service proxies.

13. Return to Visual Studio and stop the debugger by pressing Shift+F5.
15. Publish the service to IIS:
	- In Solution Explorer, right-click **MyIISService** project, and then click **Publish**. The **Publish** tab opens.
	- Select **IIS, FTP, etc** and click **Publish**. The **Publish** window opens.
	- In **Publish method**, select **Web Deploy**
	- In **Server**, enter **localhost**
	- In **Site Name**, enter **Default Web Site/MyIISService**
	- In **Destination URL**, enter **http://localhost/MyIISService/Service1.svc?wsdl**
	- Click **Save**
16. Visual Studio 2017 will proceed to build and deploy the service to IIS. When it's done a browser will automatically open up and navigate to the service WSDL.
17. Review the WCF service WSDL.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the  [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are  **not**  included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

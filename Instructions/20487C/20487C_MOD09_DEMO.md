# Module 9: Microsoft Azure Storage

   Wherever  you see a path to a file starting with *[repository root]*, replace it with the absolute path to the directory in which the 20487 repository resides.
   For example, if you cloned or extracted the 20487 repository to **C:\Users\John Doe\Downloads\20487**,
the following path: **[repository root]\AllFiles\20487C\Mod06** should be changed to **C:\Users\John Doe\Downloads\20487\AllFiles\20487C\Mod06**.

# Lesson 1: Introduction to Microsoft Azure Storage

### Demonstration: Creating a Microsoft Azure Storage Account

#### Demonstration Steps

1. Open **Microsoft Edge**.
2. Go to the Microsoft Azure portal at **http://portal.azure.com**.
3. If a page appears, prompting for your email address, enter your email address, and then click **Continue**. Wait for the sign-in page to appear, enter your email address and password, and then click **Sign In.**

   >**Note**: During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account you previously used, and then enter your credentials.

4. On the portal&#39;s menu blade, click **New**.
5. To create a storage account, on the **New** blade, click **Storage**, and then click **Storage account- blob, file, table, queue**   
6. On the **Create storage account** blade, enter the following details:  
    - In the **Name** text box, enter **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lowercase). This name will be used to access the blob, queue, and table resources for the account. Note that the storage account URLs are always written in lowercase and cannot contain any special characters such as hyphens or underscores.  
    - In the **Resource group** text box, enter **demostorageaccount**.  
    - In the **Location** box, select the region closest to your location. To reduce communication latency, it is better to create the storage account in the same region in which you deploy your application.  
7. Click **Create**, and then wait until the storage account is created.  

   >**Note**: If you get a message that the storage account creation failed because you reached your storage account limit, delete an  existing storage account, and then retry the step.

8. Click the storage account you just created, click **Properties**, and then review the different URLs for the blob, table, and queue resources.
9. Review the options on the **Configurations** tab.
10. Click **Access keys**.
11. Review the two access keys and the option to regenerate them.

    >**Note**: You need the access keys to access the storage account. The secondary key is required to renew the primary key; for example, if the primary key is compromised.


# Lesson 2: Microsoft Azure Blob Storage

### Demonstration: Uploading and Downloading Blobs from the Storage Emulator

#### Demonstration Steps

1. Open **Visual Studio 2017**.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Go to **[repository root]\Allfiles\20487C\Mod09\DemoFiles\BlobsStorageEmulator**.
4. Select the **BlobsStorageEmulator.sln** file, and then click **Open**.
5. In **Solution Explorer**, expand the **BlobStorageEmulator** project, expand the **Roles** folder, right-click **BlobStorage.Web**, and then click **Properties**.
6. In the **Properties** window, click the **Settings** tab.  

   Note that the **PhotosStorage** connection string points to the storage emulator, that runs on the local computer.
   
7. In **Solution Explorer**, expand the **BlobStorage.Web** project, and then double-click **ContainerHelper.cs**.
8. Locate the **GetContainer** method. Note how the **CloudStorageAccount.Parse** static method is used to create a **CloudStorageAccount** object from the connection string.
9. Review the use of the **CreateCloudBlobClient** method, and how it is used to create a **CloudBlobClient** object that controls the blob resources in the storage account.
10. Review the use of the **GetContainerReference** method and how it is used to return a **CloudBlobContainer** object that controls a specific blob container named **files**. Additionally, review how the **CreateIfNotExists** method verifies that the blob container exists and creates it if it does not exist.

    >**Note**: The name of the container that is passed into the **GetContainerReference** method must be in lowercase.

11. Review how the **SetPermissions** method configures the access level of the blob container.

    >**Note**: The default permission for a container is private, which means the container is not publicly accessible from the Internet, and you can only access it by using the storage account access key.

12. In **Solution Explorer**, under the **BlobStorage.Web** project, expand the **Controllers** folder, and then double-click  **HomeController.cs**.
13. Locate the **Index** method, and note the call to the **ListBlobs** method. Blob containers can be hierarchical, but you can request the return list to be flattened.
14. Review how the blob type is checked inside the **foreach** loop, because there is a difference among the blob types: Block, Page, and Directory.
15. Locate the **UploadFile** method. Review how the **GetBlockBlobRefrence** method is used for getting a reference to a block blob within the container. The method will create a new reference and return it because the blob is not currently in the container.
16. In **Solution Explorer**, under the **BlobStorage.Web** project, under the **Controllers** folder, double-click **BlobsController.cs**, and locate the **Get** method.
17. Review the use of the **GetBlockBlobRefrence** method. After uploading the file to the blob, the reference is used to download the file.  
    Note that the code that copies the stream to the response stream will stay the same even when working with other sources of data streams.  
18. In **Solution Explorer**, right-click the **BlobStorageEmulator** project, and then click **Set as StartUp Project**.
19. To run the web application by using the Azure Compute and Storage Emulator, press Ctrl+F5.
20. Click **Browse**. Go to **D:\Allfiles\Mod09\LabFiles\Assets**, select the **EmpireStateBuilding.jpg** file, and then click **Open**.
21. Click **Upload**.  

    Review the links shown on the page. The **Direct Download** link will try to download the file directly from the blob container by using its HTTP URL. The **Download** link will try to download the file from the blob container by using the storage API.
    
22. Click **Browse**. Go to **D:\Allfiles\Mod09\LabFiles\Assets**, select the **StatueOfLiberty.jpg** file, and then click **Open**.
23. Click **Upload**.
24. In the Empire State building row, click **Direct Download**. Verify that the photo of the Empire State building appears.

    >**Note**: If you are using Google Chrome to display the demo, clicking the **Direct Download** link or the **Download** link will result in the actual download of the file.

25. Review the URL in the address bar. The Storage Emulator has its own port, and the URL is composed of the name of the storage account: **devstoreaccount1**, the name of the container: **files**, and the name of the blob: **EmpireStateBuilding.jpg**.
26. Return to the previous tab, in the Statue of Liberty row, click **Download**, and then verify that the photo of the Statue of Liberty appears.
27. Close the browser.
28. Return to Visual Studio 2012. On the **View** menu, click **Server Explorer**.
29. In **Server Explorer**, expand **Windows Azure Storage**, right-click **Development**, and then click **Refresh**.
30. In **Server Explorer**, expand **Development**, then expand **Blobs**, and then double-click the **files** node.
34. Review the list of blobs in the **files [Container]** window. These are the same blobs that were displayed in the browser.



# Lesson 3: Microsoft Azure Table Storage

### Demonstration: Working with Tables and Reshaping Entities

#### Demonstration Steps

1. Open **Microsoft Edge**.
2. Go to the Azure portal at **http://portal.azure.com**.
3. If a page appears, prompting for your email address, enter your email address, and then click **Continue**. Wait for the sign-in page to appear, enter your email address and password, and then click **Sign In.**

   >**Note:** During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account you previously used, and then enter your credentials.

4. On the left navigation blade, click **Storage accounts**.
5. If you have already created an Azure storage account in the first demonstration in this module, skip to step 9.
6. On the **Storage accounts** blade, click **Add**.
7. In **Create storage account**, enter the following information:

   - Name: **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lower-case).
   - In the **Resource group** box, enter **demostorageaccount**.
   - In the **Location** box, select the region closest to your location.

8. Click **Create**. Wait until the storage account is created.
9. Go to your newly created storage account.
10. On the **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lower-case) blade, go to the **Access Keys** tab.
11. On the **Access Keys** tab, click the copy icon to the right of the **key1** box.
12. Close the open blades.
13. On the **Start** screen, click the **Visual Studio 2017** tile.
14. On the **File** menu, point to **Open**, and then click **Project/Solution**.
15. Go to **D:\Allfiles\Mod09\DemoFiles\TableStorage**.
16. Select the **TableStorage.sln** file, and then click **Open**.
17. In **Solution Explorer**, expand the **TableStorage** project, and then double-click **Web.config**.
18. In the **&lt;appSettings&gt;** element, locate the **StorageAccount** application setting.
19. In the **value** attribute value string, replace the **[AccountName]** placeholder with **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lowercase).
20. In the **value** attribute value string, select the **[AccountKey]** placeholder and press Ctrl+V to overwrite it with the account key you copied from the Azure portal.
21. To save the changes, press Ctrl+S.
22. In **Solution Explorer**, expand the **TableStorage** project, expand **Models**, and then double-click **Country.cs**.
23. Review the **Country** class. Notice that the **Country** class is derived from the **TableEntity** class.
24. Review the use of the **PartitionKey** and the **RowKey** properties:   
    The **TableEntity** class contains these two properties.   
    The **RowKey** is the unique identifier of the entity, and therefore holds the name of the country.  
    The **PartitionKey** is used for partitioning and scalability. For this demonstration, the partition is set according to the continent of the country.  

25. In **Solution Explorer**, under the **TableStorage** project, expand **Controllers**, and then double-click **CountriesController.cs**.
26. Review the contents of the **GetTable** method.  
    The **CreateIfNotExists** method verifies if the table exists, and creates it if it does not exist. The method returns a **CloudTable** object, which is used for querying and adding entities to the table.
27. Locate the **Index** method and review its contents.  
    The **TableQuery** object is used to build queries that will be run against the table. The first query is used to get all the entries in the table. The second query uses string base query, generated by the **TableQuery.GenerateFilterCondition** static method to query for countries in a given continent.
28. Review the contents of the **Add** method. The **TableOperation** object is used to create an insert operation, later we use **table.Execute** to run the operation, at that point it is persisted in the table storage.
29. To start the web application without debugging, press Ctrl+F5 .
30. Enter the following information in the browser:

    - Language: **Italian**
    - Continent: **Europe**
    - Name: **Italy**

31. Click **Add**, and wait for the country to appear in the countries table.
32. Enter the following information in the browser:

    - Language: **Chinese**
    - Continent: **Asia**
    - Name: **China**

33. Click **Add**, and wait for the two countries to appear in the **countries** table.
34. In the browser, append **countries?continent=Europe** to the address bar, and then press Enter.  
    The filtered list should show only one country.

    > **Warning**: Before continuing any further, make sure that you are logged in to Visual Studio using the email you used to register to Microsoft Azure. Otherwise you will not be able to perform the next steps.

35. Close the browser and return to Visual Studio 2017.
36. On the **View** menu, click **Server Explorer**.
37. In **Server Explorer**, expand **Azure**.
38. In **Server Explorer**, right-click **Storage**, and then click **Attach External Storage**.
39. In **Add New Storage Account**, in the **Account name** box, type **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lowercase).
40. Place the cursor in the **Account key** box, and then press Ctrl+V to enter the account key you copied from the Azure portal.
41. Click **OK**.
42. In **Server Explorer**, under **Storage**, expand the added storage account, expand **Tables**, and then double-click the **Countries** node.
43. Review the contents of the table. The table contains the **PartitionKey**, **RowKey**, **TimeStamp**, and **Language** columns.
44. In **Solution Explorer**, under the **TableStorage** project, under **Models**, double-click **Country.cs**.
45. Add the following property code to the **Country** class:

  ```cs
		public int Population{ get; set; }
```
46. To save the changes, press Ctrl+S.
47. In **Solution Explorer**, under the **TableStorage** project, under **Controllers**, double-click **CountriesController.cs**.
48. Locate the **Add** method, and then add the following code before calling the **GetTableContext** method:

  ```cs
		country.Population = int.Parse(collection["Population"]);
```
49. To save the changes, press Ctrl+S .
50. To start the web application without debugging, press Ctrl+F5.
51. Enter the following information in the browser:

    - Population: **65350000**
    - Language: **French**
    - Continent: **Europe**
    - Name: **France**

52. Click **Add**, and then wait for the table to display the three countries.
53. Return to Visual Studio 2017, and in **Server Explorer**, double-click the **Countries** node.
54. Click the **Refresh** icon. The **Population** column should appear in the table.



# Lesson 4: Microsoft Azure Queue Storage

### Demonstration: Working with Queues

#### Demonstration Steps

1. Open **Microsoft Edge**.
2. Go to the Azure portal at **http://portal.azure.com**.
3. If a page appears, prompting for your email address, enter your email address, and then click **Continue**. Wait for the sign-in page to appear, enter your email address and password, and then click **Sign In**.

   >**Note:** During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account you previously used, and then enter your credentials.

4. On the navigation blade on the left, click **Storage accounts**
5. If you have already created an Azure storage account in the first demonstration in this module, skip to step 9.
6. On the **Storage accounts** blade, click **Add**.
7. In **Create storage account**, enter the following information:

   - Name: **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lower-case).
   - In the **Resource group** box, enter **demostorageaccount**.
   - In the **Location** box, select the region closest to your location.

8. Click **Create**. Wait until the storage account is created.
9. Go to your newly created storage account.
   On the **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lowercase) blade, click the **Access Keys** tab.
10. On the **Access Keys** tab, click the copy icon to the right of the **key1** box.
11. Close the open blades.
12. Open **Visual Studio 2017**.
13. On the **File** menu, point to **Open**, and then click **Project/Solution**.
14. Go to **[repository root]\Allfiles\20487C\Mod09\DemoFiles\WorkingWithAzureQueues**.
15. Select the **WorkingWithAzureQueues.sln** file, and then click **Open**.
16. In **Solution Explorer**, expand the **WorkingWithAzureQueues.Sender** project, and then double-click **App.config**.
17. In the **&lt;connectionStrings&gt;** element, locate the **StorageConnectionString** connection string.
18. In the **value** attribute value string, replace the **[AccountName]** placeholder with **demostorageaccount***yourinitials* (_yourinitials_ is your initials in lowercase).
19. In the **value** attribute value string, select the **[AccountKey]** placeholder and press Ctrl+V to overwrite it with the account key you copied from the Azure portal.
20. To save the changes, press Ctrl+S.
21. In **Solution Explorer**, expand the **WorkingWithAzureQueues.Receiver** project, and then double-click **App.config**.
22. In the **&lt;connectionStrings&gt;** element, locate the **StorageConnectionString** connection string.
23. In the **value** attribute value string, replace the **[AccountName]** placeholder with **demostorageaccount***yourinitials* (_yourinitials_ in your initials in lowercase).
24. In the **value** attribute value string, select the **[AccountKey]** placeholder and press Ctrl+V to overwrite it with the account key you copied from the Azure portal.
25. To save the changes, press Ctrl+S .
26. In **Solution Explorer**, expand the **WorkingWithAzureQueues.Sender** project, and then double-click **Program.cs**.
27. Review the code in the **Main** method:  
    The **GetQueueReference** method returns a reference to the **messagesqueue** Azure queue. The queue name must be in lowercase, with no spaces or dashes.  
    The **CreateIfNotExists** method verifies if the queue exists, and creates it if it does not exist.  
    The code in the **for** loop creates a new message by creating a **CloudQueueMessage** object with a _string_ content, and then adds the message to the queue by calling the **AddMessage** method.  

    >**Note:** The **CloudQueueMessage** object can contain a _string_ content, and binary data in the form of a byte array.

29. In **Solution Explorer**, expand the **WorkingWithAzureQueues.Receiver** project, and then double-click **Program.cs**.
30. Review the code in the **Main** method:  
    The code uses a reference to the **messagesqueue** queue, and creates it if it does not exist.  
    The code in the **while** loop retrieves a message from the queue by calling the **GetMessage** method, handles the message, and then removes it from the queue by calling the **DeleteMessage** method.

    >**Note:** When a message is being retrieved from the queue, it is not removed from the queue, but is marked as locked for a certain duration. When the message is locked, other queue clients cannot retrieve it. After you successfully handle the message, you must delete it from the queue, otherwise the lock will expire, making the message visible and retrievable by queue clients.

31. In **Solution Explorer**, right-click the solution&#39;s root node, and then click **Properties**.
32. In the **Solution &#39;WorkingWithAzureQueues&#39; Property Pages** dialog box, select the **Multiple startup projects** option.
33. In the grid view, change the action to **Start** for the **WorkingWithAzureQueues.Sender** and **WorkingWithAzureQueues.Receiver** projects.
34. Click **OK**.
35. To run both projects, press Ctrl+F5.
36. Place both the **Sender** and **Receiver** console windows next to each other.
37. View the contents of both the windows. Each message that is sent to the queue is retrieved from the queue.
38. Close the **Sender** console window. Wait for the **Receiver** application to finish handling the queued messages, and then close the **Receiver** console window.



# Lesson 5: Restricting Access to Microsoft Azure Storage

### Demonstration: Configuring Shared Access Signature for a Blob Container

#### Demonstration Steps

1. On the **Start** screen, click the **Visual Studio 2017** tile.
2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
3. Go to **[repository root]\Allfiles\20487C\Mod09\DemoFiles\SharedAccessSignature**.
4. Select the **SharedAccessSignature.sln** file, and then click **Open**.
5. In **Solution Explorer**, expand the **SASDemo** project, expand the **Model** folder, and then double-click **AzureStorageHelper.cs**.
6. In the **AzureStorageHelper** class, locate the **GetBlobContainer** method, and review its contents:  
   After the blob container is created, its permissions are changed to prevent public access to its contents.  
   The **BlobContainerPermissions** class contains the permissions of the blob container, including its public access level, and a set of shared access policies that can be applied to contained blobs.  
   The **SharedAccessBlobPolicy** class contains the settings of a shared access policy. The code in the **GetBlobContainer** method creates a shared access policy that grants read permissions for one minute, from the time the container is created.  
   To apply the new permissions, the code calls the **SetPermissions** method.

7. Locate the **GetPicturesReferences** method, and review its contents:  
   The method iterates the list of blobs and returns a **SASPicture** object that contains basic information about each blob, including its public URL, which is not accessible, and a valid URL which has a shared access signature in it.  
   To create a shared access signature, the method calls the **CloudBlobContainer.GetSharedAccessSignature** method with the name of the access policy that was created for the blob container.

8. Locate the **ExtendPolicy** method and review its contents. The method updates the expiration time of the access policy by updating the **SharedAccessExpiryTime** property.
9. To run the web application by using the Azure Compute and Storage Emulator, press Ctrl+F5 .
10. Click **Browse**. Go to **D:\Allfiles\Mod09\LabFiles\Assets**, select the **EmpireStateBuilding.jpg** file, and then click **Open**.
11. Click **Upload**.
12. In the blob list, click the **Will not work** link, and wait for the new tab to open and show the HTTP 404 message (The webpage cannot be found).  
    Review the address in the address bar. Public access is not permitted for the blob container, and therefore direct links will not work.
13. Return to the **Home Page – Shared Access Signature** tab, and check the date and time next to the **Will work until** link. If the time next to the link has passed, click **Extend Policy** and wait for the page to refresh the expiration time.
14. Click the **Will work until** link, and verify that the photo appears in a new tab.
15. Review the address in the address bar, and the query string parameters:

    - _sv_: Signed version. The version of the Azure storage service
    - _sr_: Signed resource. Specifies whether the signature is for a single blob ( **b** ) or the entire container ( **c** )
    - _si_: Signed identifier. The name of the shared access policy used for this signature
    - _sig_: Signature. The hashed authentication signature

16. Return to the **Home Page – Shared Access Signature** tab, and wait until the time next to the **Will work until** link has passed.
17. Click the **Will work until** link, and review the error message, which indicates that the authentication failed because the shared access signature expired.
18. Return to the **Home Page – Shared Access Signature** tab, click **Extend Policy**, wait for the page to refresh the expiration time, and then click the **Will work until** link. Verify that the photo appears in a new tab.
19. Close the browser.

©2017 Microsoft Corporation. All rights reserved.

The text in this document is available under the [Creative Commons Attribution 3.0 License](https://creativecommons.org/licenses/by/3.0/legalcode), additional terms may apply. All other content contained in this document (including, without limitation, trademarks, logos, images, etc.) are **not** included within the Creative Commons license grant. This document does not provide you with any legal rights to any intellectual property in any Microsoft product. You may copy and use this document for your internal, reference purposes.

This document is provided &quot;as-is.&quot; Information and views expressed in this document, including URL and other Internet Web site references, may change without notice. You bear the risk of using it. Some examples are for illustration only and are fictitious. No real association is intended or inferred. Microsoft makes no warranties, express or implied, with respect to the information provided here.

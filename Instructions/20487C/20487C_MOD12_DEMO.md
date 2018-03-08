# Module 12: Scaling Services

## Lesson 2: Load Balancing

### Demonstration: Scaling out Web Applications in Microsoft Azure

#### Preparation Steps

For this demo, you will use your Azure account to create and configure a scalable **App Service**.
Before you begin this demo, you must complete the following steps:
 1. Go to **https://portal.azure.com**.
 2. If a page appears, prompting you to provide your email address, type your email address, and then click **Continue**. Wait for the **Sign In** page to appear, type your password, and then click **Sign In.**
   >**Note:** During the sign-in process, if a page appears prompting you to choose from a list of previously used accounts, select the account that you previously used, and then continue to type your credentials.
 
#### Demonstration Steps

### Create the **Web App** in the **Azure Portal**

 1. In the Azure portal select the **App Services** from the left menu to open the **App Services pane**.
 2. In the **App Services pane**, Click the **+ Add** on the left of the top tab to create a new **App Service**.
 3. In the **App Service pane**, click the **Web App tile** to open the **Web App pane**, and click the **Create button** at the bottom to create the new **Web App**.  
 4. In the **Web App create pane**:
  - Set the **App Name** to be **ScalableAzureApp**.
  > **Note:** While editing of the **App Name** the edit-box may become red indicating thet the name is unavailable. In this case you can add your *Initials* or maybe some digits until you find an available name for the App you are creating.  
  - Select the **Subscription** you want to use.
  - Create or select an existing **Resource Group** for the Web App you are creating.
  - Select the OS to be **Windows**.
  - Select or create an **App Service plane/Location** for the Web App you are creating.
  - Mark the **Application Insights** option be **Off**.
  - Click the **Create** button to create the **Web App**.
 5. Select the **App Services** from the left menu to open the **App Services pane** again, and select the created ScalableAzureApp.
 6. in the ScalableAzureApp options pane:
  - select the **Scale up (App Service plan)**, choose the **B1 Basic** pricing tier and click the **Select** button to set the selection.
  - select the **Scale out (App Service plan)** and in the **Configure** tab set the **Instance count** to be **2** to define the number of Instances for the **Web App**. 
  - finally select the **Overview** and then the **Get publish profile** tab to download the **profile** of the **Web App** to your computer.
  > **Note:** You will need this **profile** later in order to **publish** the project to the **Web App**.  

### Demonstration: Setting Microsoft Azure Redis Cache

#### Preparation Steps

### Setup the **Redis Cache** using the **Azure Portal**

 1. In the Azure portal select **+ New** from the left menu to open the **New pane**.
 2. From the **New pane** select **Databases**, and then from the **Databases pane** select the **Redis Cache**.
 3. In **Redis Cache pane**:
  - set the DNS, you can use the same name you used for the **Web App** or you can choose another name as you prefer.
  - select the **Subscription** you want to use.
  - select the **Resource Groupe** to be the same one you used for the **Web App**.
 > **Note:** It is recommended to configure your services to work in the same region.
  - select the pricing tier to be **Basic C0 250 MB Cache**.
  - click the **Create button** to set and deploy the Redis service.
  - wait for Redis deployment to complete, this may take some time to complete the deployment.
 4. From the left menu, select **All resources**, and click on the Redis service you have deployed to get the **Redis pane**. 
 5. From the **Redis pane**, select the **Access keys**, copy the **Primary connection string (StackExchange.Redis)** and save it.
 > **Note:** You will need this **connection string** later in order to configure the **ScalableAzureApp Project** to use the **Redis Cache**. 

#### Demonstration Steps

### Build and Publish the **ScalableAzureApp Project** using **Visual Studio 2017**

 1. Open Visual Studio 2017.
 2. On the **File** menu, point to **Open**, and then click **Project/Solution**.
 3. Go to **????\ScalableAzureApp**.
 4. Select the file **ScalableAzureApp.sln**, and then click **Open**.
 5. In Visual Studio 2017, In the **Solution Explorer** pane, expand the **ScalableAzureApp** project, and open the **Web.config**.
 6. In the **Web.config**, search for the **key="ReidsConnectionString"** and replave its value with the connection string you saved while configuring the **Redis Cache**.
   - this should look something like: 
      <appSettings>
        <add key="ReidsConnectionString" value="*pace the connection string here*" />
      </appSettings>
 7. In Visual Studio 2017, In the **Solution Explorer** pane, right-click the **ScalableAzureApp** project, and then click  **Publish**.
 8. In the **Publish pane**, click on the **Create new profile**, select the **Import profile** from the listed **Targets**, and click on the **Create Profile** button. This will open the **File Explorer**. Navigate to the directory where you saved the **Publish profile** and open it.
 9. Click the **Publish** option and wait for the web application to be deployed.
 10. When deploy is complete you your **Web App** should be opened in the Browser.
 > **Note:** You can also enter the **Azure Portal**, and select **App Services** from the left menu, and select the **ScalableAzureApp**. In the **ScalableAzureApp pane**, you can select the **Overview** to find the **URL** of the **ScalableAzureApp** **Web App**. In this pane you can also select the **Scale out (App Service plan)** to view or edit the scale of the **ScalableAzureApp**.       




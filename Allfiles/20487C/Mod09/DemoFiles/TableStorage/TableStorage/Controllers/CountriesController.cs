using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Table.DataServices;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TableStorage.Controllers
{
    public class CountriesController : Controller
    {
        const string CountriesTable = "Countries";

        private CloudTable GetTable()
        {
            // Connect to the storage account
            CloudStorageAccount storageAccount =
                CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("StorageAccount"));

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            // Verify the table exists
            CloudTable table = tableClient.GetTableReference(CountriesTable);
            table.CreateIfNotExists();

            return table;
        }

        public ActionResult Index(string continent)
        {
            CloudTable table = GetTable();
            List<Country> countries;
            TableQuery<Country> query;
            if (string.IsNullOrEmpty(continent))
            {
                // No specific continent required. Retrieve entire table content
                query = new TableQuery<Country>();
            }
            else
            {
                // Filter by continent
                query = new TableQuery<Country>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, continent));
            }
            countries = table.ExecuteQuery(query).ToList();
            return View(countries);
        }

        [HttpPost]
        public ActionResult Add(FormCollection collection)
        {
            // Create the country entity from the form content
            Country country = new Country(collection["Name"], collection["Continent"])
            {
                Language = collection["Language"]
            };

            // Add the country entity to the table
            CloudTable table = GetTable();
            TableOperation insert = TableOperation.Insert(country);
            table.Execute(insert);

            // Reload the countries list
            return RedirectToAction("Index");
        }
    }
}

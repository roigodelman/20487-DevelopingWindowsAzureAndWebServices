using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml.Linq;

namespace AsynchronousActions.Web.Controllers
{
    public class CountriesController : ApiController
    {
        // GET api/countries
        public IEnumerable<string> Get()
        {
            var result = GetCountries();
            return result.Descendants("{http://www.worldbank.org}name").Select(x => x.Value);
        }
  
        private XDocument GetCountries()
        {            
            var client = (HttpWebRequest)WebRequest.Create("http://localhost:8371/api/countries");            
            client.Accept = "application/xml";
            WebResponse response = client.GetResponse();
            var document = XDocument.Load(response.GetResponseStream());
            return document;
        }
    }
}

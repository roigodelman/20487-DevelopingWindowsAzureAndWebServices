using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataService.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new OData.Container(new Uri("http://localhost:57371/odata"));

            var course = (from c in container.Courses
                          where c.Name == "WCF"
                          select c).FirstOrDefault();

            Console.WriteLine("the course {0} has the Id: {1}", course.Name, course.Id);
            Console.ReadKey();
        }
    }
}

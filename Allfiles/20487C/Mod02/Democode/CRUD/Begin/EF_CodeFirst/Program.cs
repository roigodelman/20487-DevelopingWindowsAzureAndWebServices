using System;
using System.Data.Entity;
using System.Linq;
using EF_CodeFirst.Infra;
using System.Data.SqlClient;
using EF_CodeFirst.Model;
using System.Collections.Generic;

namespace EF_CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializing the database and populating seed data using DropCreateDatabaseIfModelChanges initializer
            (new DropCreateDBOnModelChanged()).InitializeDatabase(new SchoolContext());


        }
    }
}

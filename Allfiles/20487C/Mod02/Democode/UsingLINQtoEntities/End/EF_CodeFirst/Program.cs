using System;
using System.Data.Entity;
using System.Linq;
using EF_CodeFirst.Infra;

namespace EF_CodeFirst
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initializing the database and populating seed data using DropCreateDatabaseIfModelChanges initializer
            (new DropCreateDBOnModelChanged()).InitializeDatabase(new SchoolContext());

            // Creating a SchoolContext to be used to access data
            using (var context = new SchoolContext())
            {

                // Getting the courses list from the database
                var courses = (from c in context.Courses
                               select c);

                // Writing the courses list to the console
                foreach (var course in courses)
                {
                    // For each course, writing the students list to the console
                    Console.WriteLine("Course: {0}", course.Name);
                    foreach (var student in course.Students)
                    {
                        Console.WriteLine("\tStudent name: {0}", student.Name);
                    }
                }

                // Waiting for user input before closing the console window
                Console.ReadLine();
            }
        }
    }
}

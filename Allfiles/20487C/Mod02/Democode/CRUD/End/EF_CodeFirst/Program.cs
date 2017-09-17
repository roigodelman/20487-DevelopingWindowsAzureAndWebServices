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

            // Creating a SchoolContext to be used to access data
            using (var context = new SchoolContext())
            {

                // Getting the WCF Course from the courses repository
                Course WCFCourse = (from course in context.Courses
                                    where course.Name == "WCF"
                                    select course).Single();

                // Creating two new students
                Student firstStudent = new Student() { Name = "Thomas Andersen" };
                Student secondStudent = new Student() { Name = "Terry Adams" };

                // Adding the students to the WCF course
                WCFCourse.Students.Add(firstStudent);
                WCFCourse.Students.Add(secondStudent);

                // Giving the course teacher a 1000$ raise
                WCFCourse.CourseTeacher.Salary += 1000;

                // Getting a student called Student_1
                Student studentToRemove = WCFCourse.Students.Where((student) => student.Name == "Student_1").FirstOrDefault();

                // Remove a student from the WCF course
                WCFCourse.Students.Remove(studentToRemove);

                context.SaveChanges();

                // Print the course details to the console
                Console.WriteLine(WCFCourse);
                Console.ReadLine();
            }
        }
    }
}

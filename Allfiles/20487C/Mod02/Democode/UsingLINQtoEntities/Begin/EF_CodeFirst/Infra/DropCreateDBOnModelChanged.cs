using System;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using EF_CodeFirst.Model;

namespace EF_CodeFirst.Infra
{
    internal class DropCreateDBOnModelChanged : DropCreateDatabaseAlways<SchoolContext>
    {
        // After the database been droped and recreated, Seed method is being called for creating initial data
        protected override void Seed(SchoolContext context)
        {
            // Creating two teacher objects
            var englishTeacher = new Teacher { Name = "Ben Andrews", Salary = 200000, };
            var mathTeacher = new Teacher { Name = "Patrick Hines", Salary = 250000, };

            // Adding the teacher to the Teachers DBSet
            context.Teachers.Add(mathTeacher);
            context.Teachers.Add(englishTeacher);

            // Generating ten courses
            for (int i = 0; i < 10; i++)
            {
                var course = new Course { Name = "Course_" + i, Students = new List<Student>()};
                
                // For each course, generating ten students and assigning them to the current course
                for (int j = 0; j < 10; j++)
                {
                    var student = new Student {  Name = "Student_" + j, };
                    course.Students.Add(student);
                }
                context.Courses.Add(course);
            }

            // Saving the changes to the database
            context.SaveChanges();
        }
    }
}



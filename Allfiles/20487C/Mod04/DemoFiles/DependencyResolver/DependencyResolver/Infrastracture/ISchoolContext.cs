using DependencyResolver.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace DependencyResolver.Infrastracture
{
    public interface ISchoolContext
    {
        DbSet<Person> Persons { get; set; }
        DbSet<Student> Students { get; set; }
        DbSet<Teacher> Teachers { get; set; }
        DbSet<Course> Courses { get; set; }
    }
}

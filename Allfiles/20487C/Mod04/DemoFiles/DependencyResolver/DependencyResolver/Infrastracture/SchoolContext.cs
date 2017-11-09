using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyResolver.Model;
using DependencyResolver.Infrastracture;

namespace DependencyResolver.Infra
{
    public class SchoolContext : DbContext, ISchoolContext
    {
        public SchoolContext() 
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public virtual DbSet<Person> Persons { get; set; }
        public virtual DbSet<Student> Students { get; set; }
        public virtual DbSet<Teacher> Teachers { get; set; }
        public virtual DbSet<Course> Courses { get; set; }
        

    }
}

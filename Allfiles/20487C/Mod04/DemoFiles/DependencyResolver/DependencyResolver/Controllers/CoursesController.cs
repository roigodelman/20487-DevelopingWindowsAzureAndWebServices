using DependencyResolver.Infrastracture;
using DependencyResolver.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DependencyResolver.Controllers
{
    public class CoursesController : ApiController
    {
        ISchoolContext _context;

        public CoursesController(ISchoolContext context)
        {
            _context = context;
        }

        public IEnumerable<Course> Get()
        {
            return _context.Courses.ToList();
        }
    }
}
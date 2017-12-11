using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using Service;
using System.Web.Security;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(CalcService));
            CreateUsers();

            host.Open();
            Console.WriteLine("Service is opened. Press Enter to stop");
            Console.ReadLine();
            host.Close();
        }

        internal static void CreateUsers()
        {            
            MembershipCreateStatus status;

            if (Membership.FindUsersByName("User1").Count == 0)
            {
                Membership.CreateUser("User1", "Pa$$w0rd", "User1@BlueYonder.com", "First character of password", "P", true, out status);
            }

            if (Roles.Enabled)
            {
                if (!Roles.RoleExists("StandardUsers"))
                {
                    Roles.CreateRole("StandardUsers");
                    Roles.AddUserToRole("User1", "StandardUsers");
                }
                if (!Roles.RoleExists("Managers"))
                {
                    Roles.CreateRole("Managers");
                }
            }
        }
    }
}

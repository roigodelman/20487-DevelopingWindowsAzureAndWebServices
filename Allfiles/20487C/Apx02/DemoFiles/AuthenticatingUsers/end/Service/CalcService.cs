using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;

namespace Service
{
    public class CalcService : ICalc
    {
        [PrincipalPermission(SecurityAction.Demand, Role="StandardUsers")]
        public int Add(int a, int b)
        {
            return a + b;
        }

        [PrincipalPermission(SecurityAction.Demand, Role = "Managers")]
        public int Sub(int a, int b)
        {
            return a - b;
        }

        public int Mul(int a, int b)
        {
            new PrincipalPermission("User1", null).Demand();
            return a * b;
        }

        public int Div(int a, int b)
        {
            new PrincipalPermission(null, "Managers").Demand();
            return a / b;
        }
    }
}

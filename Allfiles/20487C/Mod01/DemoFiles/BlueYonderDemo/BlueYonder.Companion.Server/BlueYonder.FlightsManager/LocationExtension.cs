using BlueYonder.Companion.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlueYonder.FlightsManager
{
    public static class LocationExtension
    {
        public static string GetFullLocation(this LocationDTO location)
        {
            if (location.State == null)
            {
                return (location.City + ", " + location.Country);
            }
            else
            {
                return (location.City + ", " + location.State + " " + location.Country);
            }
        }
    }
}
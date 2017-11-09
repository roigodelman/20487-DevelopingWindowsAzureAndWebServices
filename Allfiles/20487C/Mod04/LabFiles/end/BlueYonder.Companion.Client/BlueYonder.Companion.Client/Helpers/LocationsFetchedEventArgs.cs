using System.Collections.Generic;
using BlueYonder.Companion.Client.DataModel;
using BlueYonder.Companion.Client.DataTransferObjects;

namespace BlueYonder.Companion.Client.Helpers
{
    public class LocationsFetchedEventArgs
    {
        public string QueryText { get; private set; }
        public LocationDTO[] Locations { get; private set; }

        public LocationsFetchedEventArgs(string queryText, LocationDTO[] locations)
        {
            QueryText = queryText;
            Locations = locations;
        }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.OData.Query;

namespace BlueYonder.Companion.Entities
{
    public class LocationDTO
    {
        [Key]
        public int LocationId { get; set; }

        public string Country { get; set; }

        public string State { get; set; }

        [Filter]
        public string City { get; set; }

        public string CountryCode { get; set; }

        public string TimeZoneId { get; set; }

        public string ThumbnailImageFile { get; set; }
    }
}

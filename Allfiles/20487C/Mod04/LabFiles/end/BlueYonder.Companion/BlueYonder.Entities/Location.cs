using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;

namespace BlueYonder.Entities
{
    [DataContract]
    public class Location
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        [DataMember]
        public int LocationId { get; set; }

        [DataMember]
        public string Country { get; set; }

        [DataMember]
        public string State { get; set; }

        [DataMember]
        public string City { get; set; }

        [DataMember]
        public string CountryCode { get; set; }

        [JsonIgnore]
        [XmlIgnore]
        [NotMapped]
        public TimeZoneInfo LocationTimeZone { get; set; }

        [DataMember]
        public string ThumbnailImageFile
        {
            get;
            set;
        }

        [JsonIgnore]
        [XmlIgnore]
        public string TimeZoneId
        {
            get
            {
                return LocationTimeZone.Id;
            }
            set
            {
                LocationTimeZone = TimeZoneInfo.FindSystemTimeZoneById(value);                
            }
        }
        //public List<TravellerTip> Tips { get; set; }
    }
}
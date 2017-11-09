using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace BlueYonder.Entities
{
    public class Traveler
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TravelerId { get; set; }

        public string TravelerUserIdentity { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Phone]
        public string MobilePhone { get; set; }

        [Required]
        public string HomeAddress { get; set; }

        public string Passport { get; set; }

        [EmailAddress]
        public string Email { get; set; }
    }
}
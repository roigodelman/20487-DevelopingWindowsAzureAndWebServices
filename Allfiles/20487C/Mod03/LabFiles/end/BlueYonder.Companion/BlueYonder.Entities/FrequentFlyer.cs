using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Entities
{
    public class FrequentFlyer
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FrequentFlyerId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string MobilePhone { get; set; }

        public string HomeAddress { get; set; }

        public string Passport { get; set; }

        public int Miles { get; set; }
    }
}

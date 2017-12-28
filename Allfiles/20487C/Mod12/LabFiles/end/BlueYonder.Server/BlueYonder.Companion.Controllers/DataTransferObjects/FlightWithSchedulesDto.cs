using BlueYonder.Companion.Entities;
using BlueYonder.Entities;
using BlueYonder.Server.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Companion.Controllers.DataTransferObjects
{
    public class FlightWithSchedulesDto : FlightDTO
    {
        public string FlightNumber { get; set; }
        
        public int FlightId { get; set; }
        
        public LocationDTO From { get; set; }

        public LocationDTO To { get; set; }

        public IEnumerable<ScheduleDto> Schedules { get; set; }
    }
}

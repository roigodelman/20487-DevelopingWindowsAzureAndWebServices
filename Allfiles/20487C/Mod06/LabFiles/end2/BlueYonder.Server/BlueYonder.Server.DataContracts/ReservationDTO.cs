using BlueYonder.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BlueYonder.Server.DataContracts
{
    [DataContract]
    public class ReservationDTO : ReservationRequestDTO
    {              
        [DataMember]
        public string ConfirmationCode { get; set; }
    }
}

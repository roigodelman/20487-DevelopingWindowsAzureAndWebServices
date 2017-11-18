﻿using System;
using System.Runtime.Serialization;

namespace BlueYonder.Server.DataContracts.Faults
{
    [DataContract]
    public class ReservationCreationFault
    {
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public DateTime ReservationDate { get; set; }               
    }
}

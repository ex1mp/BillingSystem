using BillingSystem.TStation.Entities.InformationTransfer.Requests;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.TStation.Entities.InformationTransfer.Responds
{
    public class Respond
    {
        public Request Request;
        public PhoneNumber Source { get; set; }
        public RespondState State { get; set; }
    }
}

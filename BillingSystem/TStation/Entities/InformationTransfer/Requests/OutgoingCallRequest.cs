using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.TStation.Entities.InformationTransfer.Requests
{
    public class OutgoingCallRequest : Request
    {
        public PhoneNumber Target { get; set; }
    }
}

using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.CallInformation
{
    public interface ICallInfo
    {
        public PhoneNumber Source { get; set; }
        public PhoneNumber Target { get; set; }
        public DateTime Started { get; set; }
        public TimeSpan Duration { get; set; }
        double Cost { get; set; }
    }
}

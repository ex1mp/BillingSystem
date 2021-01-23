using BillingSystem.CallInformation;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Storage
{
    public class CallStorage
    {
        private List<ICallInfo> callInformation;
        public CallStorage()
        {
            CallInformation = new List<ICallInfo>();
        }

        public List<ICallInfo> CallInformation { get => callInformation; set => callInformation = value; }

        public void Add(ICallInfo call)
        {
            CallInformation.Add(call);
        }
    }
}

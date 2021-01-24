using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.CallInformation;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.Billing.CallHandling
{
    public class CallProcessing
    {
        private List<ICallInfo> callStorage;
        private List<IContract> contractStorage;
        public CallProcessing(List<ICallInfo> callStorage, List<IContract> contractStorage)
        {
            this.callStorage = callStorage;
            this.contractStorage = contractStorage;
        }
        public void CallInfoPrepared(object sender, CallInfo callInfo)
        {
            double cost = 0;
            var account = contractStorage.FirstOrDefault(x => x.Number == callInfo.Source).Account;
            var duration = account.RemainingMinutes - callInfo.Duration.TotalMinutes;
            if (duration >= 0)
            {
                account.RemainingMinutes = duration;
            }
            else if (duration < 0)
            {
                account.RemainingMinutes = 0;
                cost = contractStorage.FirstOrDefault(x => x.Number == callInfo.Source).
                TariffPlan.CostPerMinute * Math.Abs(duration);
                account.Balance -= cost;
            }
            callInfo.Cost = cost;
            callStorage.Add(callInfo);
        }
    }
}

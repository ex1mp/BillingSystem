using BillingSystem.Billing.Storage;
using BillingSystem.CallInformation;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.CallHandling
{
    public class CallProcessing
    {
        private CallStorage callStorage;
        private ContractStorage contractStorage;
        public CallProcessing(CallStorage callStorage, ContractStorage contractStorage)
        {
            this.callStorage = callStorage;
            this.contractStorage = contractStorage;
        }
        public void CallInfoPrepared(object sender, CallInfo callInfo)
        {
            double cost = 0;
            var account = contractStorage.FindContractByPhoneNumber(callInfo.Source).Account;
            var duration = account.RemainingMinutes - callInfo.Duration.TotalMinutes;
            if (duration >= 0)
            {
                account.RemainingMinutes = duration;
            }
            else if (duration < 0)
            {
                account.RemainingMinutes = 0;
                cost = contractStorage.FindContractByPhoneNumber(callInfo.Source).
                TariffPlan.CostPerMinute * Math.Abs(duration);
                account.Balance -= cost;
            }
            callInfo.Cost = cost;
            callStorage.Add(callInfo);
        }
    }
}

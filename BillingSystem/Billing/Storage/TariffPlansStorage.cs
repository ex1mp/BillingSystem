using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.Billing.Entities.TariffPlans;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.CallInformation;
using BillingSystem.TStation;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.Ports;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BillingSystem.Billing.Storage
{
    public class TariffPlansStorage
    {
        private List<ITariffPlan> tariffPlans;

        public TariffPlansStorage()
        {
            TariffPlans = new List<ITariffPlan>();
        }
        public TariffPlansStorage(List<ITariffPlan> tariffPlans)
        {
            TariffPlans = tariffPlans;
        }

        public List<ITariffPlan> TariffPlans { get => tariffPlans; set => tariffPlans = value; }

        public void Addtariff(double costPerMinute, bool creditMethodOfCalculation, short freeMinutes, string tarifName)
        {
            tariffPlans.Add(new TariffPlan(costPerMinute, creditMethodOfCalculation, freeMinutes,tarifName));
        }

        public void RemoveTariff(Guid guid)
        {
            tariffPlans.RemoveAll(x => x.guid == guid);
        }
    }
}

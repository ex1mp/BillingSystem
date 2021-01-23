using BillingSystem.Billing.Entities.Accounts;
using BillingSystem.Billing.Entities.TariffPlans;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Contracts
{
    public class Contract:IContract
    {
        public DateTime ContractStartDate { get; }
        public Nullable<DateTime> ContractCloseDate { get; }
        public IUser Client { get; }
        public ITariffPlan TariffPlan { get; }
        public IAccount Account { get; set; }
        public PhoneNumber Number { get; set; }
        public Contract(IUser client, ITariffPlan tariffPlan, PhoneNumber number)
        {
            ContractStartDate = DateTime.Now;
            Client = client;
            TariffPlan = tariffPlan;
            Account = new Account(tariffPlan.FreeMinutes);
            Number = number;
        }
    }
}

using BillingSystem.Billing.Entities.Accounts;
using BillingSystem.Billing.Entities.TariffPlans;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Contracts
{
    public interface IContract
    {
        DateTime ContractStartDate { get; }
        Nullable<DateTime> ContractCloseDate { get; }
        IUser Client { get; }
        ITariffPlan TariffPlan { get; }
        IAccount Account { get; set; }
        PhoneNumber Number { get; set; }
    }
}

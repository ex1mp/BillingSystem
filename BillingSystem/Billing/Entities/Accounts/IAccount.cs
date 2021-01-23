using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Accounts
{
    public interface IAccount
    {
        double Balance { get; set; }
        double RemainingMinutes { get; set; }
        bool AccountActive { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Accounts
{
    public class Account : IAccount
    {
        public double Balance { get; set; }
        public double RemainingMinutes { get; set; }
        public bool AccountActive { get; set; }
        public Account()
        {
            Balance = 0;
            RemainingMinutes = 0;
            AccountActive = true;
        }
        public Account(double remainingMinutes)
        {
            RemainingMinutes = remainingMinutes;
            AccountActive = true;
        }
        
    }
}

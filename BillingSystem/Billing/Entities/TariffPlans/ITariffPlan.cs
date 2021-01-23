using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.TariffPlans
{
    public interface ITariffPlan
    {
        double CostPerMinute { get; set; }
        bool CreditMethodOfCalculation { get; set; }
        short FreeMinutes { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.TariffPlans
{
    public interface ITariffPlan
    {
        Guid guid { get; set; }
        string TarifName { get; set; }
        double CostPerMinute { get; set; }
        bool CreditMethodOfCalculation { get; set; }
        short FreeMinutes { get; set; }
    }
}

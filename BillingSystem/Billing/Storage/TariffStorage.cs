using BillingSystem.Billing.Entities.TariffPlans;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Storage
{
    public class TariffStorage
    {
        private List<ITariffPlan> tariffPlans;
        public List<ITariffPlan> TariffPlans { get => tariffPlans; set => tariffPlans = value; }
    }
}

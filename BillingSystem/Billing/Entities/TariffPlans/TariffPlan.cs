﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.TariffPlans
{
    class TariffPlan:ITariffPlan
    {
        public Guid guid { get; set; }
        public string TarifName { get; set; }
        public double CostPerMinute { get; set; }
        public bool CreditMethodOfCalculation { get; set; }
        public short FreeMinutes { get; set; }
        public TariffPlan(double costPerMinute, bool creditMethodOfCalculation, short freeMinutes,string tarifName)
        {
            this.CostPerMinute = costPerMinute;
            this.CreditMethodOfCalculation = creditMethodOfCalculation;
            this.FreeMinutes = freeMinutes;
            guid = Guid.NewGuid();
            TarifName = tarifName;
        }

    }
}

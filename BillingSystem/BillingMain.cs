using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.Billing.Entities.TariffPlans;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.TStation;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.Ports;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;


namespace BillingSystem
{
    class BillingMain
    {
        private List<ITariffPlan> tariffPlans;
        private List<IContract> contracts;
        private List<IUser> users;
        public BillingMain()
        {
            tariffPlans = new List<ITariffPlan>();
            contracts = new List<IContract>();
            users = new List<IUser>();
        }
    }
}

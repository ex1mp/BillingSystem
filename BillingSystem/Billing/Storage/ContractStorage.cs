using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.Billing.Storage
{
    public class ContractStorage
    {
        private List<IContract> contracts;
        public IContract FindContractByPhoneNumber(PhoneNumber number)
        {
            return contracts.FirstOrDefault(x => x.Number == number);
        }
    }
}

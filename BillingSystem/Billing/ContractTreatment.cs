using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.Billing.Entities.TariffPlans;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.Billing.Storage
{
    public class ContractTreatment
    {
        private List<IContract> contracts;
        private List<IContract> contractsArchive;
        public event EventHandler<ITerminal> TerminalIssue;
        public event EventHandler<PhoneNumber> TerminalWriteOff;
        public ContractTreatment()
        {
            Contracts = new List<IContract>();
            ContractsArchive = new List<IContract>();
        }

        public List<IContract> Contracts { get => contracts; set => contracts = value; }
        public List<IContract> ContractsArchive { get => contractsArchive; set => contractsArchive = value; }
        public ITerminal IssueTerminal(IContract contract)
        {
            var terminal = new Terminal(contract.Number);
            if (TerminalIssue != null)
            {
                TerminalIssue(this, terminal);
            }
            return terminal;
        }
        public void AgreementCreation(IUser user, ITariffPlan tariffPlan, PhoneNumber phoneNumber)
        {
            Contracts.Add(new Contract(user, tariffPlan, phoneNumber));
        }
        public void AgreementTermination(PhoneNumber number)
        {
            var contr = contracts.FirstOrDefault(x => x.Number == number);
            contr.ContractCloseDate = DateTime.Now;
            contractsArchive.Add(contr);
            contracts.Remove(contr);
            TerminalWriteOff(this, contr.Number);
        }  
        public void FreezeInvoicesForLatePayments()
        {
            foreach (var contract in contracts)
            {
                if (contract.Account.Balance<=0)
                {
                    contract.Account.AccountActive = false;
                }
            }
        }
    }
}

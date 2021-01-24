using BillingSystem.Billing;
using BillingSystem.Billing.Entities.Users;
using BillingSystem.Billing.Storage;
using BillingSystem.CallInformation;
using BillingSystem.TStation;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.Ports;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Threading;

namespace BillingSystem
{
    class TestBilling
    {
        CallProcessing callProcessing;
        ContractTreatment contractTreatment;
        Station station;
        UserStorage userStorage;
        TariffPlansStorage tariffPlansStorage;
        CallsStorage callsStorage;
        public TestBilling()
        {
            callsStorage = new CallsStorage();
            tariffPlansStorage = new TariffPlansStorage();
            userStorage = new UserStorage();
            contractTreatment = new ContractTreatment();
            callProcessing = new CallProcessing(callsStorage.CallStorage, contractTreatment.Contracts);
            station = new Station(new List<ITerminal>(), new List<IPort>(), contractTreatment.Contracts);
            station.CallInfoPrepared += Station_CallInfoPrepared;
            contractTreatment.TerminalIssue += ContractTreatment_TerminalIssue;
            contractTreatment.TerminalWriteOff += ContractTreatment_TerminalWriteOff;
            tariffPlansStorage.Addtariff(2, true, 500,"Tarif_1");
            tariffPlansStorage.Addtariff(5, false, 0, "Tarif_2");
            User user1 = new User("User_1");
            User user2 = new User("User_2");
            User user3 = new User("User_3");
            contractTreatment.AgreementCreation(user1, tariffPlansStorage.TariffPlans[0], new PhoneNumber("1111"));
            contractTreatment.AgreementCreation(user2, tariffPlansStorage.TariffPlans[0], new PhoneNumber("2222"));
            contractTreatment.AgreementCreation(user2, tariffPlansStorage.TariffPlans[1], new PhoneNumber("3333"));
            var term1=contractTreatment.IssueTerminal(contractTreatment.Contracts[0]);
            var term2=contractTreatment.IssueTerminal(contractTreatment.Contracts[1]);
            var term3 = contractTreatment.IssueTerminal(contractTreatment.Contracts[2]);
            contractTreatment.Contracts[2].Account.Balance = 800;
            Console.WriteLine("________");

            term1.Plug();

            Console.WriteLine("________");

            term2.Plug();

            Console.WriteLine("________");

            term1.Call(new PhoneNumber("2222"));

            Console.WriteLine("________");

            term2.Answer();
            Console.WriteLine("________");

            term1.Drop();
            Console.WriteLine("________");
            term3.Call(new PhoneNumber("2222"));
            term2.Answer();
            term2.Drop();
            Console.WriteLine("________");
            term3.Call(new PhoneNumber("1111"));
            term1.Answer();
            //Thread.Sleep(200000);
            Console.WriteLine("@@@@");
            term3.Drop();
            Console.WriteLine("________");
            term1.Call(new PhoneNumber("2222"));
            term2.Answer();
            term2.Drop();
            term1.Call(new PhoneNumber("3333"));
            term3.Answer();
            term3.Drop();

            foreach (var item in callsStorage.CallStorage)
            {
                Console.WriteLine(" cost " + item.Cost + " date " + item.Started + " source " + item.Source.Value + " target " + item.Target.Value );
            }
            Console.WriteLine("________");
            foreach (var item in callsStorage.FindCallsByDate(DateTime.Now, new PhoneNumber("1111"), callsStorage.CallStorage))
            {
                Console.WriteLine(" cost " + item.Cost + " date " + item.Started + " source " + item.Source.Value + " target " + item.Target.Value);
            }
            Console.WriteLine("________");

            Console.ReadLine();
        }

        private void ContractTreatment_TerminalWriteOff(object sender, PhoneNumber e)
        {
            station.DeliteTerminal(sender, e);
        }

        private void ContractTreatment_TerminalIssue(object sender, ITerminal e)
        {
            station.AddTerminal(sender, e);
        }

        private void Station_CallInfoPrepared(object sender, CallInfo e)
        {
            callProcessing.CallInfoPrepared(sender,e);
        }
    }
}

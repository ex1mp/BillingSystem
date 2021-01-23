using BillingSystem.TStation;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.Ports;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;

namespace BillingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            List<ITerminal> ter = new List<ITerminal>();
            var term1 = new Terminal(new PhoneNumber("111111"));
            List<IPort> por = new List<IPort>();
            Port port = new Port();
            por.Add(port);
            por.Add(new Port());
            Station station = new Station(ter, por);
            station.Add(term1);
            var term2 = new Terminal(new PhoneNumber("111112"));
            station.Add(term2);
            term1.Plug();
            term2.Plug();
            term1.Call(new PhoneNumber("111112"));
            term2.Answer();
            Console.WriteLine("++++++++++");
            term1.Drop();
            Console.WriteLine("+++=++++++");
            term2.Call(new PhoneNumber("111111"));
            Console.WriteLine("--------------");
            term1.Answer();
            Console.WriteLine("___________");
            term1.Drop();
        }
    }
}

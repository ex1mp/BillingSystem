using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.TStation.Entities.Ports
{
    public interface IPort
    {
        PortState State { get; set; }

        //event EventHandler<PortState> StateChanging;
        event EventHandler<PortState> PortStateChanged;
        void ClearEvents();
        void RegisterEventHandlersForTerminal(ITerminal terminal);
    }
}

using BillingSystem.TStation.Entities.InformationTransfer.Requests;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.TStation.Entities.Ports
{
    public class Port : IPort
    {
        private PortState _state = PortState.UnPlagged;
        public PortState State
        {
            get
            {
                return _state;
            }
            set
            {
                if (_state != value)
                {
                    OnStateChanged(this, value);
                    _state = value;
                }
            }
        }
        public event EventHandler<PortState> PortStateChanged;
        protected virtual void OnStateChanged(object sender, PortState state)
        {
            if (PortStateChanged != null)
            {
                PortStateChanged(sender, state);
            }
        }
        public void ClearEvents()
        {
            this.PortStateChanged = null;
        }
        public void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.OutgoingConnection += this.OnOutgoingCall;
            terminal.Plugging += (port, args) => { this.State = PortState.Free; };
            terminal.UnPlugging += (port, args) => { this.State = PortState.UnPlagged; };
        }
        public void OnOutgoingCall(object sender, Request request)
        {
            if (request.GetType() == typeof(OutgoingCallRequest) && this.State == PortState.Free)
            {
                this.State = PortState.Busy;
            }
        }
    }
}

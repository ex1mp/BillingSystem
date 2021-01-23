using BillingSystem.Billing.Entities.Contracts;
using BillingSystem.CallInformation;
using BillingSystem.TStation.Entities;
using BillingSystem.TStation.Entities.InformationTransfer.Requests;
using BillingSystem.TStation.Entities.InformationTransfer.Responds;
using BillingSystem.TStation.Entities.Ports;
using BillingSystem.TStation.Entities.Terminals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.TStation
{
    public class Station
    {
        public Station(List<ITerminal> terminalCollection, List<IPort> portCollection,List<IContract> contracts)
        {
            this._terminalCollection = terminalCollection;
            this._portCollection = portCollection;
            this.contracts = contracts;
            this._connectionCollection = new List<CallInfo>();
            this._callCollection = new List<CallInfo>();
            this._portMapping = new Dictionary<PhoneNumber, IPort>();
        }
        private List<IContract> contracts;
        private List<CallInfo> _connectionCollection;
        private List<CallInfo> _callCollection;
        private List<ITerminal> _terminalCollection;
        private List<IPort> _portCollection;
        private Dictionary<PhoneNumber, IPort> _portMapping;

        protected ITerminal GetTerminalByPhoneNumber(PhoneNumber number)
        {
            return _terminalCollection.FirstOrDefault(x => x.Number == number);
        }

        protected IPort GetPortByPhoneNumber(PhoneNumber number)
        {
            return _portMapping[number];
        }
        private bool CheckBalance(PhoneNumber number)
        {
            var acc = contracts.FirstOrDefault(x => x.Number == number);
            if (acc!=null && acc.Account.AccountActive)
            {
                return acc.Account.Balance > 0 ? true : acc.TariffPlan.CreditMethodOfCalculation ? true : false; 
            }
            return false;
        }

        protected void RegisterOutgoingRequest(OutgoingCallRequest request)
        {
            if ((request.Source != default(PhoneNumber) && request.Target != default(PhoneNumber)) &&
                (GetCallInfo(request.Source) == null && GetConnectionInfo(request.Source) == null)
                && CheckBalance(request.Source))
            {
                var callInfo = new CallInfo()
                {
                    Source = request.Source,
                    Target = request.Target,
                    Started = DateTime.Now
                };

                ITerminal targetTerminal = GetTerminalByPhoneNumber(request.Target);
                IPort targetPort = GetPortByPhoneNumber(request.Target);

                if (targetPort.State == PortState.Free)
                {
                    _connectionCollection.Add(callInfo);
                    targetPort.State = PortState.Busy;
                    targetTerminal.IncomingRequestFrom(request.Source);
                }
                else
                {
                    OnCallInfoPrepared(this, callInfo);
                }
            }
        }
        public void OnOutgoingRequest(object sender, Request request)
        {
            if (request.GetType() == typeof(OutgoingCallRequest))
            {
                RegisterOutgoingRequest(request as OutgoingCallRequest);
            }
        }
        public void RegisterEventHandlersForTerminal(ITerminal terminal)
        {
            terminal.OutgoingConnection += this.OnOutgoingRequest;
            terminal.IncomingRespond += OnIncomingCallRespond;
            terminal.IncomingRequest += (sender, request) => Console.WriteLine("{0} received request for incoming connection from {1}", terminal.Number, request.Source);
            terminal.Online += (sender, args) => { Console.WriteLine("Terminal {0} turned to online mode", terminal.Number); };
            terminal.Offline += (sender, args) => { Console.WriteLine("Terminal {0} turned to offline mode", terminal.Number); };

        }
        public void RegisterEventHandlersForPort(IPort port)
        {
            port.PortStateChanged += (sender, state) => { Console.WriteLine("Station detected the port {0} changed its State to {1}", port.GetHashCode(), state); };

        }

        public void Add(ITerminal terminal)
        {
            var freePort = _portCollection.Except(_portMapping.Values).FirstOrDefault();
            if (freePort != null)
            {
                _terminalCollection.Add(terminal);

                MapTerminalToPort(terminal, freePort);

                // register event handlers

                this.RegisterEventHandlersForTerminal(terminal);
                this.RegisterEventHandlersForPort(freePort);
            }
        }

        protected void MapTerminalToPort(ITerminal terminal, IPort port)
        {
            _portMapping.Add(terminal.Number, port);
            port.RegisterEventHandlersForTerminal(terminal);
            terminal.RegisterEventHandlersForPort(port);
        }

        protected void UnMapTerminalFromPort(ITerminal terminal, IPort port)
        {
            _portMapping.Remove(terminal.Number);
            terminal.ClearEvents();
            port.ClearEvents();
        }

        /// <summary>
        /// raise when the station generates a new CallInfo for billing 
        /// </summary>
        public event EventHandler<CallInfo> CallInfoPrepared;

        protected virtual void OnCallInfoPrepared(object sender, CallInfo callInfo)
        {
            if (CallInfoPrepared != null)
            {
                CallInfoPrepared(sender, callInfo);
            }
        }

        protected CallInfo GetConnectionInfo(PhoneNumber actor)
        {
            return this._connectionCollection.FirstOrDefault(x => (x.Source == actor || x.Target == actor));
        }

        protected CallInfo GetCallInfo(PhoneNumber actor)
        {
            return this._callCollection.FirstOrDefault(x => (x.Source == actor || x.Target == actor));
        }

        protected void SetPortStateWhenConnectionInterrupted(PhoneNumber source, PhoneNumber target)
        {
            var sourcePort = GetPortByPhoneNumber(source);
            if (sourcePort != null)
            {
                sourcePort.State = PortState.Free;
            }

            var targetPort = GetPortByPhoneNumber(target);
            if (targetPort != null)
            {
                targetPort.State = PortState.Free;
            }
        }

        protected void InterruptConnection(CallInfo callInfo)
        {
            this._callCollection.Remove(callInfo);
            SetPortStateWhenConnectionInterrupted(callInfo.Source, callInfo.Target);
            OnCallInfoPrepared(this, callInfo);
        }

        protected void InterruptActiveCall(CallInfo callInfo)
        {
            callInfo.Duration = DateTime.Now - callInfo.Started;
            this._callCollection.Remove(callInfo);
            SetPortStateWhenConnectionInterrupted(callInfo.Source, callInfo.Target);
            OnCallInfoPrepared(this, callInfo);
        }

        public void OnIncomingCallRespond(object sender, Respond respond)
        {
            var registeredCallInfo = GetConnectionInfo(respond.Source);
            if (registeredCallInfo != null)
            {
                switch (respond.State)
                {
                    case RespondState.Drop:
                        {
                            InterruptConnection(registeredCallInfo);
                            break;
                        }
                    case RespondState.Accept:
                        {
                            MakeCallActive(registeredCallInfo);
                            break;
                        }
                }
            }
            else
            {
                CallInfo currentCallInfo = GetCallInfo(respond.Source);
                if (currentCallInfo != null)
                {
                    this.InterruptActiveCall(currentCallInfo);
                }
            }
        }
        public void OutgoingRespondHandler(object sender, Respond respond)
        {
            CallInfo currentCallInfo = GetCallInfo(respond.Source);
            if (currentCallInfo != null && respond.State == RespondState.Drop)
            {
                this.InterruptActiveCall(currentCallInfo);
            }
        }

        protected void MakeCallActive(CallInfo callInfo)
        {
            this._connectionCollection.Remove(callInfo);
            callInfo.Started = DateTime.Now;
            this._callCollection.Add(callInfo);
        }

        public void ClearEvents()
        {
            this.CallInfoPrepared = null;
        }
    }
}

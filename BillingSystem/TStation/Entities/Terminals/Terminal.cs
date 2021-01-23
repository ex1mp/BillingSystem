using BillingSystem.TStation.Entities.InformationTransfer.Requests;
using BillingSystem.TStation.Entities.InformationTransfer.Responds;
using BillingSystem.TStation.Entities.Ports;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.TStation.Entities.Terminals
{
    public class Terminal : ITerminal
    {
        private Request ServerIncomingRequest { get; set; }

        protected bool IsOnline { get; private set; }

        public PhoneNumber Number { get; set; }

        public Terminal(PhoneNumber number)
        {
            Number = number;
        }

        public event EventHandler<IncomingCallRequest> IncomingRequest;

        public event EventHandler<Request> OutgoingConnection;

        public event EventHandler<Respond> IncomingRespond;

        public event EventHandler Plugging;

        public event EventHandler UnPlugging;

        public event EventHandler Online;

        public event EventHandler Offline;

        public void Call(PhoneNumber target)
        {
            if (!IsOnline)
            {
                OnOutgoingCall(this, target);
                OnOnline(this, null);
            }
        }
        protected virtual void OnOutgoingCall(object sender, PhoneNumber target)
        {
            if (OutgoingConnection != null)
            {
                ServerIncomingRequest = new OutgoingCallRequest() { Source = Number, Target = target };
                OutgoingConnection(sender, ServerIncomingRequest);
            }
        }


        protected virtual void OnIncomingRequest(object sender, IncomingCallRequest request)
        {
            if (IncomingRequest != null)
            {
                IncomingRequest(sender, request);
            }
            ServerIncomingRequest = request;
        }

        public void IncomingRequestFrom(PhoneNumber source)
        {
            OnIncomingRequest(this, new IncomingCallRequest() { Source = source });
        }

        public void Drop()
        {
            if (ServerIncomingRequest != null)
            {
                OnOutgoingRespond(this, new Respond() { Source = Number, State = RespondState.Drop, Request = ServerIncomingRequest });
                if (IsOnline)
                {
                    OnOffline(this, null);
                }
            }
        }

        public void Answer()
        {
            if (!IsOnline && ServerIncomingRequest != null)
            {
                OnOutgoingRespond(this, new Respond() { Source = Number, State = RespondState.Accept, Request = ServerIncomingRequest });
                OnOnline(this, null);
            }
        }


        protected virtual void OnOutgoingRespond(object sender, Respond respond)
        {
            if (IncomingRespond != null && ServerIncomingRequest != null)
            {
                IncomingRespond(sender, respond);
            }
        }

        protected virtual void OnOnline(object sender, EventArgs args)
        {
            if (Online != null)
            {
                Online(sender, args);
            }
            IsOnline = true;
        }

        protected virtual void OnOffline(object sender, EventArgs args)
        {
            if (Offline != null)
            {
                Offline(sender, args);
                ServerIncomingRequest = null;
            }
            IsOnline = false;
        }


        public void Plug()
        {
            OnPlugging(this, null);
        }

        public void Unplug()
        {
            if (IsOnline)
            {
                Drop();
                OnUnPlugging(this, null);
            }
        }

        protected virtual void OnPlugging(object sender, EventArgs args)
        {
            if (Plugging != null)
            {
                Plugging(sender, args);
            }
        }

        protected virtual void OnUnPlugging(object sender, EventArgs args)
        {
            if (UnPlugging != null)
            {
                UnPlugging(sender, args);
            }
        }

        public void ClearEvents()
        {
            IncomingRequest = null;
            IncomingRespond = null;
            OutgoingConnection = null;
            Online = null;
            Offline = null;
            Plugging = null;
            UnPlugging = null;
        }

        public virtual void RegisterEventHandlersForPort(IPort port)
        {
            port.PortStateChanged += (sender, state) =>
            {
                if (IsOnline && state == PortState.Free)
                {
                    OnOffline(sender, null);
                }
            };
        }
    }
}

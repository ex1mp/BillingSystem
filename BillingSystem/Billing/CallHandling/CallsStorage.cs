using BillingSystem.CallInformation;
using BillingSystem.TStation.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BillingSystem.Billing.CallHandling
{
    public class CallsStorage
    {
        private List<ICallInfo> callStorage;
        public CallsStorage()
        {
           callStorage = new List<ICallInfo>();

        }
        public IEnumerable<ICallInfo> ReportAllCals(PhoneNumber number,IEnumerable<ICallInfo> callStorage)
        {
            return callStorage.Where(x => x.Source == number);
        }
        public IEnumerable<ICallInfo> FindCallsByDate(DateTime date, PhoneNumber number, IEnumerable<ICallInfo> callStorage)
        {
            return callStorage.Where(x => x.Source == number && x.Started.Date == date.Date);
        }
        public IEnumerable<ICallInfo> FindCallsToSpecificNumber(PhoneNumber sourceNumber, PhoneNumber targetNumber, IEnumerable<ICallInfo> callStorage)
        {
            return callStorage.Where(x => x.Source == sourceNumber && x.Target == targetNumber);
        }
        public IEnumerable<ICallInfo> FindByCallAmount(double lowerPriceLimit, double upperPriceLimit, IEnumerable<ICallInfo> callStorage)
        {
            return callStorage.Where(x => x.Cost >= lowerPriceLimit && x.Cost <= upperPriceLimit);
        }


    }
}

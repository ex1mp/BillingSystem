using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Users
{
    class User:IUser
    {
        public string FullName { get; }
        public User(string userName)
        {
            FullName = userName;
        }
    }
}

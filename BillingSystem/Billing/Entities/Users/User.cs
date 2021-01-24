using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Users
{
    class User:IUser
    {
        public Guid guid { get; set; }
        public string FullName { get; }
        public User(string userName)
        {
            FullName = userName;
            guid= Guid.NewGuid();
        }
    }
}

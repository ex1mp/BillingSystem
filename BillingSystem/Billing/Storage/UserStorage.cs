using BillingSystem.Billing.Entities.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing
{
    class UserStorage
    {
        private List<IUser> users;
        public UserStorage()
        {
            Users = new List<IUser>();
        }

        public List<IUser> Users { get => users; set => users = value; }

        public void AddUser(string userName)
        {
            Users.Add(new User(userName));
        }
        public void RemoveUser(Guid guid)
        {
            Users.RemoveAll(x => x.guid == guid);
        }
    }
}

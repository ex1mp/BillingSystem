﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BillingSystem.Billing.Entities.Users
{
    public interface IUser
    {
        Guid guid { get; set; }
        string FullName { get; }
    }
}

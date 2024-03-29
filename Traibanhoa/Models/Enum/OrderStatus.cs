﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Enum
{
    public enum OrderStatus
    {
        DELETED = 0,
        PENDING = 1,
        ACCEPTED = 2,
        CANCEL = 3, // by user
        DENIED = 4, // by staff
        SHIPPING = 5,
        DELIVERED = 6,
        DELIVERED_FAIL = 7,
    }
}

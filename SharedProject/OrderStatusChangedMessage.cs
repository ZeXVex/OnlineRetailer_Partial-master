﻿using System;
using System.Collections.Generic;
using System.Text;

namespace SharedProject
{
    public class OrderStatusChangedMessage
    {
        public int? CustomerID { get; set; }

        public IEnumerable<Order.OrderLine> OrderLines { get; set; }
    }
}

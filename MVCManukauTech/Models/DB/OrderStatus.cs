using System;
using System.Collections.Generic;

namespace MVCManukauTech.Models.DB
{
    public partial class OrderStatus
    {
        public OrderStatus()
        {
            Orders = new HashSet<Orders>();
        }

        public int OrderStatusId { get; set; }
        public string Description { get; set; }

        public ICollection<Orders> Orders { get; set; }
    }
}

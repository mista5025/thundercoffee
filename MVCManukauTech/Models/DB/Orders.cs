using System;
using System.Collections.Generic;

namespace MVCManukauTech.Models.DB
{
    public partial class Orders
    {
        public Orders()
        {
            OrderDetails = new HashSet<OrderDetails>();
        }

        public int OrderId { get; set; }
        public string Username { get; set; }
        public string SessionId { get; set; }
        public int? OrderStatusId { get; set; }
        public DateTime? DateOfSession { get; set; }
        public DateTime? DateOfOrder { get; set; }
        public DateTime? DateOfShipping { get; set; }
        public int? TransactionId { get; set; }
        public string Notes { get; set; }
        public string CustomerName { get; set; }
        public string DeliveryAddress { get; set; }
        public string Phone { get; set; }
        public string EmailAddress { get; set; }
        public string CardOwner { get; set; }
        public string CardType { get; set; }
        public DateTime? ExpiryDate { get; set; }

        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}

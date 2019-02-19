using System;
using System.Collections.Generic;

namespace MVCManukauTech.Models.DB
{
    public partial class Products
    {
        public Products()
        {
            OrderDetails = new HashSet<OrderDetails>();
            Reviews = new HashSet<Reviews>();
        }

        public string ProductId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string ImageFileName { get; set; }
        public double? UnitCost { get; set; }
        public string Description { get; set; }
        public bool IsDownload { get; set; }
        public string DownloadFileName { get; set; }

        public Categories Category { get; set; }
        public ICollection<OrderDetails> OrderDetails { get; set; }
        public ICollection<Reviews> Reviews { get; set; }
    }
}

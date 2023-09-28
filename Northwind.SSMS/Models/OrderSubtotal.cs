using System;
using System.Collections.Generic;

namespace Northwind.SSMS.Models
{
    public partial class OrderSubtotal
    {
        public int OrderId { get; set; }
        public decimal? Subtotal { get; set; }
    }
}

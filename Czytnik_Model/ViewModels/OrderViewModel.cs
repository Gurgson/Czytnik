using System;
using System.Collections.Generic;

namespace Czytnik_Model.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal CalculatedPrice { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
    }
}

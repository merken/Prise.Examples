using System;
using System.Collections.Generic;
using System.Text;

namespace OrdersControllerPlugin.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double PriceIncVAT { get; set; }
    }
}

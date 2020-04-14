using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.WindowsAzure.Storage.Table;

namespace OrdersControllerPlugin.Models
{
    public class OrderTableEntity : TableEntity
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public double PriceIncVAT { get; set; }
    }
}

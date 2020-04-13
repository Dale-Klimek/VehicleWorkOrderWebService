using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleWorkOrder.Shared.Models
{
    class CarWithWorkOrders : SingleCar
    {
        public IEnumerable<SingleWorkOrder> WorkOrders { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleWorkOrder.Database.Models.Views
{
    public class CarView
    {
        public int CarId { get; set; }
        public string VehicleIdentification { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Series { get; set; }
        public string Trim { get; set; }
        public int? Doors { get; set; }
        public string VehicleType { get; set; }
        public string TransmissionStyle { get; set; }
        public DateTime ScannedDate { get; set; }
        public DateTime LastScanned { get; set; }
        public long WorkOrderCount { get; set; }
    }
}

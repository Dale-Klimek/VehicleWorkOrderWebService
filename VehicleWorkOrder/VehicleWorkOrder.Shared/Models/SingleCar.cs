namespace VehicleWorkOrder.Shared.Models
{
    using System;
    using System.Collections.Generic;

    public class SingleCar
    {
        public SingleCar()
        {
            Pictures = new List<PhotoDto>();
        }
        public int? Id { get; set; }
        public string VehicleIdentification { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int? Year { get; set; }
        public string Series { get; set; }
        public string Trim { get; set; }
        public int? Doors { get; set; }
        public string VehicleType { get; set; }
        public string TransmissionStyle { get; set; }
        public long WorkOrderCount { get; set; }
        public DateTime ScannedDate { get; set; }
        public DateTime LastScanned { get; set; }
        public IEnumerable<PhotoDto> Pictures { get; set; }
    }
}

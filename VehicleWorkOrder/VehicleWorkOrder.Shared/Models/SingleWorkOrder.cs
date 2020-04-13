namespace VehicleWorkOrder.Shared.Models
{
    using System;
    using System.Collections.Generic;
    using Enum;

    public class SingleWorkOrder
    {
        public SingleWorkOrder()
        {
            Pictures = new List<PhotoDto>();
        }

        public int? Id { get; set; }
        public int CarId { get; set; }
        public string WorkOrder { get; set; }
        public string PurchaseOrder { get; set; }
        public TechnicianDto Technician { get; set; }
        public string RepairOrder { get; set; }
        // Probably want to move this to a car
        public DateTime? ScannedDate { get; set; }
        public FeatureAdded FeatureAdded { get; set; }
        public string Notes { get; set; }
        public LocationDto Location { get; set; }
        public IEnumerable<PhotoDto> Pictures { get; set; }
    }
}

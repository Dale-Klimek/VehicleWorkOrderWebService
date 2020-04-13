namespace VehicleWorkOrder.Database.Models.Views
{
    using System;

    public class WorkOrderView
    {
        public int WorkOrderId { get; set; }
        public string WorkOrder { get; set; }
        public DateTime? DateAdded { get; set; }
        public string Year { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string VehicleIdentification { get; set; }
        public string StockNumber { get; set; }
        public string RepairOrder { get; set; }
        public string PurchaseOrder { get; set; }
        public string TechnicianName { get; set; }
        public bool IsDeleted { get; set; }
    }
}

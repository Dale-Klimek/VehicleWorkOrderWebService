namespace VehicleWorkOrder.Shared.Models
{
    using System;
    using System.Linq;
    using Enum;

    public class WorkOrderDto
    {
        public WorkOrderDto()
        {
            ScannedDate = DateTime.Now;
        }
        public WorkOrderDto(SingleCar singleCar) : this()
        {
            UpdateCar(singleCar);
        }
        public int Id { get; set; }
        public string VehicleIdentification { get; private set; }
        public string Make { get; private set; }
        public string Model { get; private set; }
        public int? Year { get; private set; }
        public string Series { get; set; }
        public string Trim { get; set; }
        //public string WorkOrder { get; set; }
        public string PurchaseOrder { get; set; }
        public TechnicianDto Technician { get; set; }
        public string RepairOrder { get; set; }
        public DateTime ScannedDate { get; private set; }
        public string StockNumber => VehicleIdentification?.Reverse().ToString().Substring(0, 6).Reverse().ToString();
        public FeatureAdded FeatureAdded { get; set; }
        public string Notes { get; set; }
        public int? Doors { get; set; }
        public string VehicleType { get; set; }
        public string TransmissionStyle { get; set; }

        void UpdateCar(SingleCar singleCar)
        {
            VehicleIdentification = singleCar.VehicleIdentification;
            Make = singleCar.Make;
            Model = singleCar.Model;
            Year = singleCar.Year;
            Series = singleCar.Series;
            Trim = singleCar.Trim;
            VehicleType = singleCar.VehicleType;
            Doors = singleCar.Doors;
            TransmissionStyle = singleCar.TransmissionStyle;
        }
    }
}

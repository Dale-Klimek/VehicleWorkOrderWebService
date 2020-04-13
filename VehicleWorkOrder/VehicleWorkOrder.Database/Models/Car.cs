namespace VehicleWorkOrder.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Shared.Models;

    public class Car
    {
        [Key]
        public int Id { get; set; }
        [StringLength(17)] public string VehicleIdentification { get; set; }
        public Model Model { get; set; }
        public int Year { get; set; }
        [StringLength(100)] public string Series { get; set; }
        [StringLength(50)] public string Trim { get; set; }
        public int? Doors { get; set; }
        [StringLength(100)] public string VehicleType { get; set; }
        [StringLength(50)] public string TransmissionStyle { get; set; }
        public DateTime ScannedDate { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastScanned { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public static Car GetCar(WorkOrderDto dto, Model model)
        {
            var car = new Car()
            {
                Model = model
            };
            car.UpdateCar(dto);
            return car;
        }

        public void UpdateCar(WorkOrderDto dto)
        {
            Doors = dto.Doors;
            Series = dto.Series;
            TransmissionStyle = dto.TransmissionStyle;
            Trim = dto.Trim;
            VehicleIdentification = dto.VehicleIdentification;
            VehicleType = dto.VehicleType;
            Year = dto.Year ?? 1900;
        }
    }
}

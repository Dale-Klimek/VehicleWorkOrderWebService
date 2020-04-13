namespace VehicleWorkOrder.Database.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Shared.Enum;
    using Shared.Models;

    public class WorkOrder
    {
        [Key]
        public int Id { get; set; }
        public int CarId { get; set; }
        public Car Car { get; set; }
        [StringLength(20)]
        public string WorkOrderNumber { get; set; }
        [StringLength(20)]
        public string PurchaseOrder { get; set; }

        public Technician Technician { get; set; }
        [StringLength(20)]
        public string RepairOrder { get; set; }
        public FeatureAdded FeatureAdded { get; set; }
        [StringLength(255)]
        public string Notes { get; set; }

        public User UserAdded { get; set; }
        public int UserAddedId { get; set; }

        public Location Location { get; set; }

        public DateTime? DateAdded { get; set; }
        public DateTime LastUpdated { get; set; }
        public bool IsDeleted { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public static WorkOrder GetWorkOrder(WorkOrderDto dto, Car car, User user, Technician tech)
        {
            var workOrder = new WorkOrder()
            {
                Car = car,
                UserAdded = user,
            };
            workOrder.UpdateWorkOrder(dto, tech);
            return workOrder;
        }

        public void UpdateWorkOrder(WorkOrderDto dto, Technician tech)
        {
            Id = dto.Id;
            PurchaseOrder = dto.PurchaseOrder;
            Technician = tech;
            RepairOrder = dto.RepairOrder;
            FeatureAdded = dto.FeatureAdded;
            Notes = dto.Notes;
        }
    }
}

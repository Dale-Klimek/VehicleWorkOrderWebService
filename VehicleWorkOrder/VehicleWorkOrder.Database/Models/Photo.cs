namespace VehicleWorkOrder.Database.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Photo
    {
        [Key]
        public long Id { get; set; }
        public Guid Name { get; set; }
        public int? CarId { get; set; }
        public int? WorkOrderId { get; set; }
        public bool IsDeleted { get; set; }
    }
}

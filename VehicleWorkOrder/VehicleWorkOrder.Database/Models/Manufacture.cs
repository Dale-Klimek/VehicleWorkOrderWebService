namespace VehicleWorkOrder.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Manufacture
    {
        [Key] public short Id { get; set; }
        [StringLength(50)]
        public string ManufactureName { get; set; }
    }
}

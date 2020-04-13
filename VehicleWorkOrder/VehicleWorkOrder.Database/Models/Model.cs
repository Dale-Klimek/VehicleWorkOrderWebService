namespace VehicleWorkOrder.Database.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Model
    {
        [Key]
        public short Id { get; set; }
        public Manufacture Manufacture { get; set; }
        [StringLength(50)]
        public string ModelName { get; set; }
    }
}

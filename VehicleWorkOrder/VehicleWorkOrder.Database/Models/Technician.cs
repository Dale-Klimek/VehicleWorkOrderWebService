using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleWorkOrder.Database.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Technician
    {
        [Key]
        public short Id { get; set; }
        [StringLength(50)]
        public string FirstName { get; set; }
        [StringLength(75)]
        public string LastName { get; set; }
    }
}

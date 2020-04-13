using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VehicleWorkOrder.Database.Models
{
    public class Location
    {
        [Key]
        public short Id { get; set; }
        [StringLength(100)]
        public string LocationDescription { get; set; }
    }
}

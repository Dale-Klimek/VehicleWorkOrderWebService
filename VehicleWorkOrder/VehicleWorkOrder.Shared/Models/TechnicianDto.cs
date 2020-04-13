using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleWorkOrder.Shared.Models
{
    public class TechnicianDto
    {
        public short Id { get; set; }
        
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string GetName => FirstName + " " + LastName;
    }
}

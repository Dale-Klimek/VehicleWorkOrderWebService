using System;
using System.Collections.Generic;
using System.Text;

namespace VehicleWorkOrder.Shared.Models
{
    public class PhotoDto : ICloneable
    {
        public long? Id { get; set; }
        public byte[] Photo { get; set; }
        public Guid Name { get; set; }
        public bool IsDeleted { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }
}

namespace VehicleWorkOrder.Database
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using Models.Views;

    public class WorkOrderContext : DbContext
    {
        public WorkOrderContext(DbContextOptions<WorkOrderContext> options) : base(options)
        {
            
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Manufacture> Manufactures { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Technician> Technicians { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<CarView> CarsView { get; set; }
        public DbSet<WorkOrderView> WorkOrderView { get; set; }
        public DbSet<Photo> Photos { get; set; }
        public DbSet<Location> Location { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // might need to check column attributes
                var result = entityType.GetProperties()
                    .Where(x => x.PropertyInfo != null &&  x.PropertyInfo.PropertyType.FullName != null && x.PropertyInfo.PropertyType.FullName.Equals("System.String"));
                foreach (var property in result)
                {
                    var length = 0;
                    foreach (var attribute in property.PropertyInfo.GetCustomAttributesData())
                    {
                        if (attribute.AttributeType.Name == "StringLengthAttribute")
                        {
                            var value = attribute.ConstructorArguments.FirstOrDefault().Value;
                            int.TryParse(value.ToString(), out length);
                        }
                    }
                    if(length != 0)
                        property.SetColumnType($"varchar({length})");
                }
            }

            modelBuilder.Entity<CarView>(e =>
            {
                e.HasNoKey();
                e.ToView("CarView");
            });

            modelBuilder.Entity<WorkOrderView>(e =>
            {
                e.HasNoKey();
                e.ToView("WorkOrderView");
            });
        }
    }
}

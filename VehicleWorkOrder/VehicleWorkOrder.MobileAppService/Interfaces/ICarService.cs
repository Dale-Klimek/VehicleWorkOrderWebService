namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared.Models;

    public interface ICarService
    {
        IAsyncEnumerable<SingleCar> GetCarsAsync();
        Task<SingleCar> AddOrUpdateCarAsync(SingleCar car);
        Task<SingleCar> AddCarAsync(SingleCar car);
        Task<SingleCar> UpdateCarAsync(SingleCar car);
        Task<SingleCar> GetCar(string vehicleIdentification, bool includeImage = false);
        Task<SingleCar> GetCar(int id, bool includeImage = false);
    }
}
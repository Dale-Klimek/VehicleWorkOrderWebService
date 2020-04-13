using System.Collections.Generic;
using System.Threading.Tasks;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    public interface ILocationService
    {
        IAsyncEnumerable<LocationDto> GetLocations();
        Task<LocationDto> GetLocation(short id);
        Task<LocationDto> AddLocation(string description);
        Task UpdateLocation(LocationDto dto); 
        Task DeleteLocation(short id);
    }
}
namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared.Models;

    public interface ITechnicianService
    {
        IAsyncEnumerable<TechnicianDto> GetTechnicians();
        Task<TechnicianDto> GetTechnician(short id);
        Task<TechnicianDto> AddTechnician(string firstName, string lastName);
        Task UpdateTechnician(TechnicianDto dto);
        Task DeleteTechnician(short id);
    }
}
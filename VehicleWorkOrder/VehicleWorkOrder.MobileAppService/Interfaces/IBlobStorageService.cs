namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    public interface IBlobStorageService
    {
        Task Initialize();
        Task<Stream> Download(string name);
        Task<Guid> Upload(Stream stream);
    }
}
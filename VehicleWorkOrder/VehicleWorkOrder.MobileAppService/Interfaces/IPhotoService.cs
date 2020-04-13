using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using VehicleWorkOrder.Database.Models;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    public interface IPhotoService
    {
        Task<MemoryStream> GetPhoto(Guid name, bool isCompressed = true);
        IAsyncEnumerable<PhotoDto> GetPhotosByCar(int carId);
        Task<Guid> Upload(byte[] stream);
        Task UpdateDeletedPhotos(IEnumerable<PhotoDto> photos);
        Task<ICollection<Photo>> UploadPictures(IEnumerable<PhotoDto> pics);
        Task<ICollection<PhotoDto>> LoadPhotos(int id);
        IAsyncEnumerable<PhotoDto> GetPhotosByWorkOrder(int workOrder);
        Task<PhotoDto> GetPhoto(long id, bool compress = true);
    }
}
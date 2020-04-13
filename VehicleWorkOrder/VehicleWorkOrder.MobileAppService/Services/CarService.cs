namespace VehicleWorkOrder.MobileAppService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Database;
    using Database.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Shared.Models;

    public class CarService : ICarService
    {
        private readonly WorkOrderContext _context;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;

        public CarService(WorkOrderContext context, IMapper mapper, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _photoService = photoService;
        }

        public async IAsyncEnumerable<SingleCar> GetCarsAsync()
        {
            await foreach (var car in _context.CarsView.OrderByDescending(c => c.LastScanned).AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<SingleCar>(car);
            }
        }

        public async Task<SingleCar> AddOrUpdateCarAsync(SingleCar car)
        {
            if (car.Id is null)
            {
                return await AddCarAsync(car).ConfigureAwait(false);
            }

            var result = await _context.Cars.SingleOrDefaultAsync(c => c.Id == car.Id.Value).ConfigureAwait(false);
            if (result is null)
            {
                return await AddCarAsync(car).ConfigureAwait(false);
            }

            var updatedCar = _mapper.Map<Car>(car);
            _context.Cars.Attach(updatedCar);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return car;
        }

        public async Task<SingleCar> AddCarAsync(SingleCar car)
        {
            return await AddCarAsync(car, false).ConfigureAwait(false);
        }

        private async Task<SingleCar> AddCarAsync(SingleCar car, bool skipCheck)
        {
            car.Id = null;

             if(await _context.Cars.AnyAsync(c => c.VehicleIdentification == car.VehicleIdentification).ConfigureAwait(false))
                 throw new ArgumentException("Not allowed to add a car with the same VIN number");

            var model = await _context.Models.SingleOrDefaultAsync(m => m.ModelName == car.Model).ConfigureAwait(false);
            if (model is null)
                model = await AddModel(car.Model, car.Make).ConfigureAwait(false);
            var result = _mapper.Map<Car>(car);

            result.Model = model;
            result.LastUpdated = DateTime.UtcNow;
            result.ScannedDate = DateTime.UtcNow;
            result.LastScanned = DateTime.UtcNow;

            _context.Cars.Add(result);
            result.Photos = await _photoService.UploadPictures(car.Pictures).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return _mapper.Map<SingleCar>(result);
        }

        private Manufacture AddManufacture(string make)
        {
            var manufacturer = new Manufacture() {ManufactureName = make};
            return manufacturer;
        }

        private async Task<Model> AddModel(string model, string make)
        {
            var manufacturer = await _context.Manufactures.SingleOrDefaultAsync(m => m.ManufactureName == make)
                             .ConfigureAwait(false) ?? AddManufacture(make);

            var result = new Model(){Manufacture = manufacturer, ModelName = model};
            return result;
        }

        public async Task<SingleCar> UpdateCarAsync(SingleCar car)
        {
            if (!await _context.Cars.AnyAsync(c => c.VehicleIdentification == car.VehicleIdentification).ConfigureAwait(false))
            {
                // car doesn't exist. We need to add the car not update the car
                return await AddCarAsync(car, true).ConfigureAwait(false);
            }
            var model = await _context.Models.SingleOrDefaultAsync(m => m.ModelName == car.Model).ConfigureAwait(false) ??
                        await AddModel(car.Model, car.Make).ConfigureAwait(false);

            var updatedCar = _mapper.Map<Car>(car);
            updatedCar.Model = model;
            updatedCar.LastUpdated = DateTime.UtcNow;
            _context.Cars.Attach(updatedCar);
            var entry = _context.Entry(updatedCar);
            entry.State = EntityState.Modified;
            entry.Property(nameof(Car.ScannedDate)).IsModified = false;
            entry.Property(nameof(Car.LastScanned)).IsModified = false;

            updatedCar.Photos = await _photoService.UploadPictures(car.Pictures.Where(c => !c.IsDeleted)).ConfigureAwait(false);
            await _photoService.UpdateDeletedPhotos(car.Pictures.Where(c => c.IsDeleted)).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return car;
        }

        public async Task<SingleCar> GetCar(string vehicleIdentification, bool includeImage = false)
        {
            var result = await _context.CarsView.SingleOrDefaultAsync(c => c.VehicleIdentification == vehicleIdentification)
                .ConfigureAwait(false);
            if(result is null)
                return null;
            var car = _mapper.Map<SingleCar>(result);

            car.WorkOrderCount = await _context.WorkOrders.LongCountAsync(w => w.CarId == result.CarId).ConfigureAwait(false);
            var carModel = await _context.Cars.SingleAsync(c => c.Id == car.Id).ConfigureAwait(false);
            carModel.LastScanned = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            if (!includeImage)
                return car;

            Debug.Assert(car.Id != null, "car.Id != null");
            return await LoadPhotos(car, car.Id.Value);
        }

        public async Task<SingleCar> GetCar(int id, bool includeImage = false)
        {
            var result = await _context.CarsView.SingleOrDefaultAsync(c => c.CarId == id).ConfigureAwait(false);
            if (result is null)
                return null; 
            var car = _mapper.Map<SingleCar>(result);
            if (!includeImage)
                return car;
            return await LoadPhotos(car, id).ConfigureAwait(false);
        }

        private async Task<SingleCar> LoadPhotos(SingleCar car, int id)
        {
            var list = new List<PhotoDto>();

            await foreach (var photo in _photoService.GetPhotosByCar(id).ConfigureAwait(false))
            {
                list.Add(photo);
            }

            car.Pictures = list;
            return car;
        }

    }
}

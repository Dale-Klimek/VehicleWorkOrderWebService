namespace VehicleWorkOrder.MobileAppService.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Shared.Models;

    public class CarController : BaseApiController
    {
        private readonly ICarService _service;
        private readonly IPhotoService _photoService;

        public CarController(ICarService service, IPhotoService photoService)
        {
            _service = service;
            _photoService = photoService;
        }

        [HttpGet]
        public IAsyncEnumerable<SingleCar> GetCars() => _service.GetCarsAsync();

        [HttpGet("{vehicleIdentification:length(17)}")]
        public async Task<ActionResult<SingleCar>> GetCar(string vehicleIdentification, bool includeImages = false)
        {
            var result = await _service.GetCar(vehicleIdentification, includeImages).ConfigureAwait(false);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpGet("{id:int}")]

        public async Task<ActionResult<SingleCar>> GetCar(int id, bool includeImages = false)
        {
            var result = await _service.GetCar(id, includeImages).ConfigureAwait(false);
            if (result is null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SingleCar>> AddOrUpdateCar(SingleCar car)
        {
            return Ok(await _service.AddOrUpdateCarAsync(car).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<ActionResult<SingleCar>> AddCar(SingleCar singleCar)
        {
            return Ok(await _service.AddCarAsync(singleCar).ConfigureAwait(false));
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCar(SingleCar singleCar)
        {
            return Ok(await _service.UpdateCarAsync(singleCar).ConfigureAwait(false));
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteCar(int id)
        {
            return Ok();
        }

        [HttpGet("{carId}")]
        public IAsyncEnumerable<PhotoDto> GetPhotos(int carId) => _photoService.GetPhotosByCar(carId);

    }
}

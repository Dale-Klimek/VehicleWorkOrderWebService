using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleWorkOrder.MobileAppService.Interfaces;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Controllers
{
    public class LocationController : BaseApiController
    {
        private readonly ILocationService _service;

        public LocationController(ILocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public IAsyncEnumerable<LocationDto> GetLocations() => _service.GetLocations();
    }
}

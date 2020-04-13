using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleWorkOrder.MobileAppService.Interfaces;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Controllers
{
    public class PhotoController : BaseApiController
    {
        private readonly IPhotoService _photoService;

        public PhotoController(IPhotoService photoService)
        {
            _photoService = photoService;
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PhotoDto>> GetPhoto(long id)
        {
            var result = await _photoService.GetPhoto(id, false).ConfigureAwait(false);
            if (result is null)
                return NotFound();
            return Ok(result);
        }


    }
}

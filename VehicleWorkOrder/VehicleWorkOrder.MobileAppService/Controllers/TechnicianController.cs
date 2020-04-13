namespace VehicleWorkOrder.MobileAppService.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Shared.Models;

    public class TechnicianController : BaseApiController
    {
        private readonly ITechnicianService _service;

        public TechnicianController(ITechnicianService service)
        {
            _service = service;
        }

        [HttpGet]
        public IAsyncEnumerable<TechnicianDto> GetTechnicians() => _service.GetTechnicians();

        [HttpPost]
        public async Task<ActionResult<TechnicianDto>> AddTechnician(TechnicianDto dto)
        {
            if (dto.FirstName is null || dto.LastName is null)
                return BadRequest("First and Last name required");
            return Created(nameof(GetTechnician), await _service.AddTechnician(dto.FirstName, dto.LastName).ConfigureAwait(false));

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TechnicianDto>> GetTechnician(short id)
        {
            return Ok(await _service.GetTechnician(id).ConfigureAwait(false));
        }

        [HttpPut]
        public async Task<ActionResult> UpdateTechnician(TechnicianDto dto)
        {
            await _service.UpdateTechnician(dto).ConfigureAwait(false);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteTechnician(short id)
        {
            await _service.DeleteTechnician(id);
            return Ok();
        }
    }
}

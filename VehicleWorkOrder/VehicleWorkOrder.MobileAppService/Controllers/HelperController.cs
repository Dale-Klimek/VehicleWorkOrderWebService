namespace VehicleWorkOrder.MobileAppService.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelperController : Controller
    {
        [HttpGet]
        public IActionResult WakeUp()
        {
            return Ok();
        }
    }
}

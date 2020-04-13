namespace VehicleWorkOrder.MobileAppService.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    
    //.Net core has no issue with this but swagger is having an issue
    // So changing the way I do this so I can use swagger
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]
    //[Route("api/[controller]/[action]/{id?}")]
    [Authorize]
    [ApiController]
    public class BaseApiController : Controller
    {
    }
}

namespace VehicleWorkOrder.MobileAppService.Services
{
    using System.Linq;
    using System.Security.Claims;
    using Interfaces;
    using Microsoft.AspNetCore.Http;

    public class ClaimsService : IClaimsService
    {
        private readonly IHttpContextAccessor _context;

        public ClaimsService(IHttpContextAccessor context)
        {
            _context = context;
        }

        public string GetUserName()
        {
            if (_context.HttpContext.User.Identity is ClaimsIdentity principal)
            {
                return principal.Claims.SingleOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            }

            // ToDo: Come back and fix this.
            foreach (var claim in _context.HttpContext.User.Claims)
            {
                return claim.Value;
            }
            return null;
        }
    }
}

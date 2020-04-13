namespace VehicleWorkOrder.MobileAppService.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Interfaces;
    using Microsoft.AspNetCore.Mvc;

    using Shared.Models;

    public class WorkOrderController : BaseApiController
    {
        private readonly IWorkOrderService _workOrderService;
        private readonly IPhotoService _photoService;

        public WorkOrderController(IWorkOrderService workOrderService, IPhotoService photoService)
        {
            _workOrderService = workOrderService;
            _photoService = photoService;
        }


        [HttpGet("{id}")]
        public IAsyncEnumerable<SingleWorkOrder> GetWorkOrdersByCar(int carId) =>
            _workOrderService.GetWorkOrdersByCar(carId);

        [HttpGet]
        public IAsyncEnumerable<WorkOrderListItem> GetWorkOrders() => _workOrderService.GetWorkOrders();

        [HttpGet("{searchParameters}")]
        public IAsyncEnumerable<WorkOrderListItem> SearchWorkOrders(string searchParameters) =>
            _workOrderService.SearchWorkOrders(searchParameters);

        [HttpGet("{id}")]
        public async Task<ActionResult<SingleWorkOrder>> GetWorkOrder(int id, bool includeImages = false)
        {
            return Ok(await _workOrderService.GetWorkOrder(id, includeImages).ConfigureAwait(false));
        }

        [HttpPost]
        public async Task<IActionResult> AddWorkOrder(SingleWorkOrder workOrder)
        {
            await _workOrderService.AddWorkOrder(workOrder).ConfigureAwait(false);
            return Ok();
        }

        [HttpGet("{id}")]
        public IAsyncEnumerable<PhotoDto> GetPhotos(int id) => _photoService.GetPhotosByWorkOrder(id);

        [HttpPut]
        public async Task<ActionResult<SingleWorkOrder>> UpdateWorkOrder(SingleWorkOrder workOrder)
        {
            await _workOrderService.UpdateWorkOrder(workOrder);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkOrder(int id)
        {
            await _workOrderService.DeleteWorkOrder(id);
            return Ok();
        }
    }
}

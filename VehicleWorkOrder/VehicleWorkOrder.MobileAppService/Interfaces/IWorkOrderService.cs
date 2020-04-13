namespace VehicleWorkOrder.MobileAppService.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Shared.Models;

    public interface IWorkOrderService
    {
        Task<SingleWorkOrder> GetWorkOrder(int id, bool includeImages = false);
        IAsyncEnumerable<SingleWorkOrder> GetWorkOrdersByCar(int carId);
        IAsyncEnumerable<WorkOrderListItem> GetWorkOrders();
        Task AddWorkOrder(SingleWorkOrder workOrder);
        Task UpdateWorkOrder(SingleWorkOrder workOrder);
        IAsyncEnumerable<WorkOrderListItem> SearchWorkOrders(string searchParameter);
        Task DeleteWorkOrder(int id);
    }
}
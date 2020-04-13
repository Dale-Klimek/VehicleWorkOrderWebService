namespace VehicleWorkOrder.MobileAppService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Database;
    using Database.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Shared.Models;

    public class WorkOrderService : IWorkOrderService
    {
        private readonly WorkOrderContext _context;
        private readonly IMapper _mapper;
        private readonly IClaimsService _claimsService;
        private readonly IPhotoService _photoService;

        public WorkOrderService(WorkOrderContext context, IMapper mapper, IClaimsService claimsService, IPhotoService photoService)
        {
            _context = context;
            _mapper = mapper;
            _claimsService = claimsService;
            _photoService = photoService;
        }

        public async Task<SingleWorkOrder> GetWorkOrder(int id, bool includeImages = false)
        {
            try
            {
                var info = (await (from w in _context.WorkOrders
                                   join t in _context.Technicians on w.Technician.Id equals t.Id
                                   join l in _context.Location on w.Location.Id equals l.Id into workOrders
                                   from subWorkOrders in workOrders.DefaultIfEmpty()
                                   where w.Id == id && w.IsDeleted == false
                                   select new { w, t, subWorkOrders }).ToListAsync().ConfigureAwait(false)).Single();

                var result = _mapper.Map<SingleWorkOrder>(info.w);
                result.Technician = _mapper.Map<TechnicianDto>(info.t);
                result.Location = _mapper.Map<LocationDto>(info.subWorkOrders);
                if (!includeImages)
                    return result;
                result.Pictures = await _photoService.LoadPhotos(id).ConfigureAwait(false);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return new SingleWorkOrder();
        }

        public async IAsyncEnumerable<SingleWorkOrder> GetWorkOrdersByCar(int carId)
        {
            await foreach (var workOrder in _context.WorkOrders.Where(w => w.CarId == carId && w.IsDeleted == false).OrderByDescending(w => w.DateAdded).AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<SingleWorkOrder>(workOrder);
            }
        }

        public async IAsyncEnumerable<WorkOrderListItem> GetWorkOrders()
        {
            await foreach (var workOrder in _context.WorkOrderView.Where(w => w.IsDeleted == false).OrderByDescending(s => s.DateAdded).AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<WorkOrderListItem>(workOrder);
            }
        }

        public async IAsyncEnumerable<WorkOrderListItem> SearchWorkOrders(string searchParameter)
        {
            var search = _context.WorkOrderView.Where(w => w.IsDeleted == false && (
                w.PurchaseOrder.Contains(searchParameter) || w.RepairOrder.Contains(searchParameter) ||
                w.StockNumber.Contains(searchParameter) || w.VehicleIdentification.Contains(searchParameter)));
            await foreach (var workOrder in search.OrderByDescending(s => s.DateAdded).AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<WorkOrderListItem>(workOrder);
            }
        }

        public async Task AddWorkOrder(SingleWorkOrder workOrder)
        {
            var tech = await _context.Technicians.SingleOrDefaultAsync(t => t.Id == workOrder.Technician.Id);
            var wo = _mapper.Map<WorkOrder>(workOrder);
            if (workOrder.Location != null)
                wo.Location = await _context.Location.SingleOrDefaultAsync(l => l.Id == workOrder.Location.Id);
            
            wo.Technician = tech;
            wo.UserAdded = await GetUser().ConfigureAwait(false);
            wo.DateAdded = DateTime.UtcNow;
            wo.LastUpdated = DateTime.UtcNow;
            wo.Photos = await _photoService.UploadPictures(workOrder.Pictures).ConfigureAwait(false);
            _context.WorkOrders.Add(wo);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task UpdateWorkOrder(SingleWorkOrder workOrder)
        {
            if(workOrder.Id is null)
            {
                await AddWorkOrder(workOrder).ConfigureAwait(false);
                return;
            }
            var wo = _mapper.Map<WorkOrder>(workOrder);
            _context.WorkOrders.Attach(wo);
            var entry = _context.Entry(wo);
            entry.State = EntityState.Modified;
            entry.Property(nameof(WorkOrder.DateAdded)).IsModified = false;
            wo.UserAdded = await GetUser().ConfigureAwait(false);
            wo.LastUpdated = DateTime.UtcNow;
            wo.Photos = await _photoService.UploadPictures(workOrder.Pictures.Where(p => !p.IsDeleted)).ConfigureAwait(false);
            await _photoService.UpdateDeletedPhotos(workOrder.Pictures.Where(p => p.IsDeleted)).ConfigureAwait(false);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        private async Task<User> GetUser()
        {
            var userName = _claimsService.GetUserName();
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName).ConfigureAwait(false);
            if (user is null)
            {
                user = new User() { UserName = userName };
                _context.Users.Add(user);
            }

            return user;
        }

        public async Task DeleteWorkOrder(int id)
        {
            var result = await _context.WorkOrders.Where(w => w.Id == id).SingleOrDefaultAsync().ConfigureAwait(false);
            if (result is null)
                throw new InvalidOperationException($"Can not delete a work order by the id {id}");
            result.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }
}

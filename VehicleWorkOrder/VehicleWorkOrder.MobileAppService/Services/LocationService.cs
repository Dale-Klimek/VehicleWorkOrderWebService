using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VehicleWorkOrder.Database;
using VehicleWorkOrder.Database.Models;
using VehicleWorkOrder.MobileAppService.Interfaces;
using VehicleWorkOrder.Shared.Models;

namespace VehicleWorkOrder.MobileAppService.Services
{
    public class LocationService : ILocationService
    {
        private readonly WorkOrderContext _context;
        private readonly IMapper _mapper;

        public LocationService(WorkOrderContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async IAsyncEnumerable<LocationDto> GetLocations()
        {
            await foreach (var location in _context.Location.AsNoTracking().AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<LocationDto>(location);
            }
        }


        public async Task<LocationDto> GetLocation(short id)
        {
            var location = await _context.Location.FindAsync(id).ConfigureAwait(false);
            if (location is null)
                throw new ArgumentException("Invalid technician id");
            return _mapper.Map<LocationDto>(location);
        }

        public async Task<LocationDto> AddLocation(string description)
        {
            var location = new Location()
            {
                LocationDescription = description
            };

            _context.Location.Add(location);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return _mapper.Map<LocationDto>(location);
        }

        public async Task UpdateLocation(LocationDto dto)
        {
            var location = _mapper.Map<Location>(dto);
            _context.Location.Update(location);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteLocation(short id)
        {
            var result = await _context.Location.SingleOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
            if (result is null)
                return;
            _context.Location.Remove(result);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

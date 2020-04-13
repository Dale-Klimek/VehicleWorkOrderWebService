namespace VehicleWorkOrder.MobileAppService.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using AutoMapper;
    using Database;
    using Database.Models;
    using Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Shared.Models;

    public class TechnicianService : ITechnicianService
    {
        private readonly WorkOrderContext _context;
        private readonly IMapper _mapper;

        public TechnicianService(WorkOrderContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async IAsyncEnumerable<TechnicianDto> GetTechnicians()
        {
            await foreach (var tech in _context.Technicians.AsNoTracking().AsAsyncEnumerable().ConfigureAwait(false))
            {
                yield return _mapper.Map<TechnicianDto>(tech);
            }
        }

        public async Task<TechnicianDto> GetTechnician(short id)
        {
            var tech = await _context.Technicians.FindAsync(id).ConfigureAwait(false);
            if (tech is null)
                throw new ArgumentException("Invalid technician id");
            return _mapper.Map<TechnicianDto>(tech);
        }

        public async Task<TechnicianDto> AddTechnician(string firstName, string lastName)
        {
            var tech = new Technician()
            {
                FirstName = firstName,
                LastName = lastName
            };

            _context.Technicians.Add(tech);
            await _context.SaveChangesAsync().ConfigureAwait(false);
            return _mapper.Map<TechnicianDto>(tech);
        }

        public async Task UpdateTechnician(TechnicianDto dto)
        {
            var tech = _mapper.Map<Technician>(dto);
            _context.Technicians.Update(tech);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }

        public async Task DeleteTechnician(short id)
        {
            var result = await _context.Technicians.SingleOrDefaultAsync(t => t.Id == id).ConfigureAwait(false);
            if (result is null)
                return;
            _context.Technicians.Remove(result);
            await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}

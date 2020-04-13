namespace VehicleWorkOrder.MobileAppService
{
    using System;
    using System.Collections.Generic;
    using AutoMapper;
    using Database.Models;
    using Database.Models.Views;
    using Shared.Models;

    public class MapperProfile : Profile
    {
        private readonly TimeZoneInfo _centralTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time");

        public MapperProfile()
        {
            CreateMap<IEnumerable<TechnicianDto>, IEnumerable<Technician>>().ReverseMap();
            CreateMap<TechnicianDto, Technician>().ReverseMap();
            CreateMap<WorkOrderDto, WorkOrder>().ReverseMap();
            CreateMap<SingleCar, Car>()
                .ForMember(m => m.Model, opt => opt.Ignore())
                .ForMember(m => m.ScannedDate, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeToUtc(s.ScannedDate, _centralTimeZone)))
                .ForMember(m => m.LastScanned, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeToUtc(s.LastScanned, _centralTimeZone)))
                .ReverseMap()
                .ForMember(m => m.Model, opt => opt.Ignore())
                .ForMember(m => m.ScannedDate, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeFromUtc(s.ScannedDate, _centralTimeZone)))
                .ForMember(m => m.LastScanned, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeFromUtc(s.LastScanned, _centralTimeZone)));
            CreateMap<SingleWorkOrder, WorkOrder>()
                .ForMember(m => m.WorkOrderNumber, opt => opt.MapFrom(s => s.WorkOrder))
                .ReverseMap();
            CreateMap<CarView, SingleCar>()
                .ForMember(m => m.Id, opt => opt.MapFrom(s => s.CarId))
                .ForMember(m => m.ScannedDate, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeFromUtc(s.ScannedDate, _centralTimeZone)))
                .ForMember(m => m.LastScanned, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeFromUtc(s.LastScanned, _centralTimeZone)))
                .ReverseMap()
                .ForMember(m => m.ScannedDate, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeToUtc(s.ScannedDate, _centralTimeZone)))
                .ForMember(m => m.LastScanned, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeToUtc(s.LastScanned, _centralTimeZone)));
            CreateMap<WorkOrderListItem, WorkOrderView>()
                .ForMember(m => m.DateAdded, opt => opt.MapFrom(s => TimeZoneInfo.ConvertTimeToUtc(s.DateAdded, _centralTimeZone)))
                .ReverseMap()
                .ForMember(m => m.DateAdded, opt => opt.MapFrom(s => s.DateAdded.HasValue ? TimeZoneInfo.ConvertTimeFromUtc(s.DateAdded.Value, _centralTimeZone) : DateTime.MinValue ));
            CreateMap<PhotoDto, Photo>().ReverseMap();
            CreateMap<LocationDto, Location>().ReverseMap();
        }
    }
}

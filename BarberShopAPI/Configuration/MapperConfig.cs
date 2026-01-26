using AutoMapper;
using BarberShopAPI.Data;
using BarberShopAPI.DTO;

namespace BarberShopAPI.Configuration
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Service, ServiceResponseDTO>();
            CreateMap<CreateServiceDTO, Service>();
            CreateMap<UpdateServiceDTO, Service>();

            CreateMap<Appointment, AppointmentCreatedDTO>();
            CreateMap<Appointment, AppointmentDetailsDTO>()
                        .ForMember(d => d.BarberFullName, opt => opt.MapFrom(a => a.Barber.FirstName + " " + a.Barber.LastName))
                        .ForMember(d => d.ServiceName, opt => opt.MapFrom(a => a.Service.Name));

        }
    }
}

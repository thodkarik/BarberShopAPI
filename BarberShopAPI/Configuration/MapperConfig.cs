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
        }
    }
}

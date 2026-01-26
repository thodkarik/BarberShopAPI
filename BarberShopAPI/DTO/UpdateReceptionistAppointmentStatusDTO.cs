using BarberShopAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BarberShopAPI.DTO
{
    public class UpdateReceptionistAppointmentStatusDTO
    {
        [Required]
        public AppointmentStatus Status { get; set; }
    }
}

using BarberShopAPI.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace BarberShopAPI.DTO
{
    public class UpdateAppointmentStatusDTO
    {
        [Required]
        public AppointmentStatus Status { get; set; }
    }
}

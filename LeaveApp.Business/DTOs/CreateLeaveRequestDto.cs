using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace LeaveApp.Business.DTOs
{
    public class CreateLeaveRequestDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Geçerli bir izin türü seçmelisiniz.")]
        public int LeaveTypeId { get; set; }

        public string Description { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using LeaveApp.Domain.Enums;

namespace LeaveApp.Business.DTOs
{
    public class LeaveRequestWithUserDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public int LeaveTypeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public LeaveStatus Status { get; set; }
        public int? ApprovedByUserId { get; set; }
        public DateTime? DecisionDate { get; set; }
    }
}

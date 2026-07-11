using LeaveApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Domain.Entities
{
    public class LeaveRequest : BaseEntity
    {

        public int UserId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public LeaveStatus Status { get; set; }

        public int? ApprovedByUserId { get; set; }

        public DateTime? DecisionDate { get; set; }
    }
}

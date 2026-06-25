using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Domain.Entities
{
    public class LeaveRequest
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int LeaveTypeId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Description { get; set; }

        public int Status { get; set; }

        public int? ApprovedByUserId { get; set; }

        public DateTime? DecisionDate { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}

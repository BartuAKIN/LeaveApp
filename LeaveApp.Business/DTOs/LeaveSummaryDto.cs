using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace LeaveApp.Business.DTOs
{
    public class LeaveTypeSummaryDto
    {
        public string LeaveTypeName { get; set; }
        public int TotalDays { get; set; }
    }

    public class AnnualLeaveSummaryDto
    { 
        public int UsedDays { get; set; }
        public int RemainingDays { get; set; }
        public int TotalQuota { get; set; }
    }

   public class TeamLeaveBarChartDto
    {
        public string UserFullName { get; set; }
        public List<LeaveTypeSummaryDto> LeaveBreakdown { get; set; }
    }

    public class MonthlyLeaveCountDto
    {
        public int Month {  get; set; }

        public string MonthName { get; set; }

        public int TotalRequests { get; set; }
    }
}
